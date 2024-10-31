using ConferenceBookingAPI.Model.Dto.UserAuthDto;
using ConferenceBookingAPI.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConferenceBookingAPI.UserAuth
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserAuthenticationService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ApiResponse<string>> RegisterSuperAdminAsync(RegisterModelDto param)
        {
            try
            {
                var _isUserExist = await _userManager.FindByNameAsync(param.UserName);
                if(_isUserExist != null)
                {
                    return new ApiResponse<string>
                    {
                        Data = "This user already exists!",
                        ErrorMessage = "Error",
                        IsSuccess = false
                    };
                }

                var _userData = new ApplicationUser
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = param.UserName,
                    Email = param.Email,
                    FirstName = param.FirstName,
                    LastName = param.LastName,
                };
                var _createResult = await _userManager.CreateAsync(_userData, param.Password);
                if (!_createResult.Succeeded)
                {
                    return new ApiResponse<string>
                    {
                        Data = "",
                        IsSuccess = false,
                        ErrorMessage = _createResult.Errors.FirstOrDefault()?.Description
                    };

                }
                if (!await _roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.SuperAdmin));
                }
                if (await _roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
                {
                    await _userManager.AddToRoleAsync(_userData, UserRoles.SuperAdmin);
                }

                return new ApiResponse<string>
                {
                    Data = "Super Admin Registered",
                    IsSuccess = true,
                    ErrorMessage = ""
                };
            
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Data = "",
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ApiResponse<string>> RegisterAdminAsync(RegisterModelDto param)
        {
            var _isUserExist = await _userManager.FindByNameAsync(param.UserName);
            if (_isUserExist != null)
            {
                return new ApiResponse<string>
                {
                    Data = "This user already exists!",
                    ErrorMessage = "Error",
                    IsSuccess = false
                };
            }

            var _userData = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = param.UserName,
                Email = param.Email,
                FirstName = param.FirstName,
                LastName = param.LastName
            };

            var _createResult = await _userManager.CreateAsync(_userData, param.Password);
            if (!_createResult.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Data = _createResult.Errors.FirstOrDefault()?.Description,
                    IsSuccess = false,
                    ErrorMessage = "Error"
                };
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.AdminRole));
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.AdminRole))
            {
                await _userManager.AddToRoleAsync(_userData, UserRoles.AdminRole);
            }

            return new ApiResponse<string>
            {
                Data = "Admin Registered",
                IsSuccess = true,
                ErrorMessage = ""
            };
        }

        public async Task<ApiResponse<string>> RegisterUserAsync(RegisterModelDto param)
        {
            var _isUserExist = await _userManager.FindByNameAsync(param.UserName);
            if (_isUserExist != null)
            {
                return new ApiResponse<string>
                {
                    Data = "User is already taken",
                    IsSuccess = false,
                    ErrorMessage = "Error"
                };
            }

            var _userData = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = param.UserName,
                Email = param.Email,
                FirstName = param.FirstName,
                LastName = param.LastName
            };

            var _createResult = await _userManager.CreateAsync(_userData, param.Password);
            if (!_createResult.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Data = _createResult.Errors.FirstOrDefault()?.Description,
                    IsSuccess = false,
                    ErrorMessage = "Error"
                };
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.UserRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.UserRole));
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.UserRole))
            {
                await _userManager.AddToRoleAsync(_userData, UserRoles.UserRole);
            }

            return new ApiResponse<string>
            {
                Data = "User Registered",
                IsSuccess = true,
                ErrorMessage = ""
            };
        }

        public async Task<ApiResponse<UserLoginDto>> LoginAsync(LoginModelDto param)
        {
            var _checkUser = await _userManager.FindByNameAsync(param.UserName);
            if (_checkUser == null || !await _userManager.CheckPasswordAsync(_checkUser, param.Password))
            {
                return new ApiResponse<UserLoginDto>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = "Invalid credentials"
                };
            }

            var _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));
            var _authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _checkUser.Id),
                new Claim(ClaimTypes.Name, _checkUser.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(_checkUser);
            foreach (var role in roles)
            {
                _authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var _token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(60),
                claims: _authClaims,
                signingCredentials: new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var _newRefreshToken = await _userManager.GenerateUserTokenAsync(_checkUser, "SuperAdmin", "RefreshToken");


            var userInfo = new UserLoginDto
            {
                userID = _checkUser.Id,
                UserName = _checkUser.UserName!,
                Token = new JwtSecurityTokenHandler().WriteToken(_token),
                UserToken = _newRefreshToken,
                UserRole = roles,
                ConferenceId = _checkUser.ConferenceId
            };

            return new ApiResponse<UserLoginDto>
            {
                Data = userInfo,
                IsSuccess = true,
                ErrorMessage = ""
            };
        }

        public async Task<ApiResponse<List<AdminUsersDto>>> GetAdminsAsync()
        {
            try
            {
                // Get all users
                var allUsers = await _userManager.Users.ToListAsync();
                var adminUsers = new List<AdminUsersDto>();

                foreach (var user in allUsers)
                {
                    // Check if the user has the Admin role
                    if (await _userManager.IsInRoleAsync(user, UserRoles.AdminRole))
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var userLoginDto = new AdminUsersDto
                        {
                            UserId = user.Id,
                            UserName = user.UserName!,
                            ConferenceId = user.ConferenceId,
                        };

                        adminUsers.Add(userLoginDto);
                    }
                }

                return new ApiResponse<List<AdminUsersDto>>
                {
                    Data = adminUsers,
                    IsSuccess = true,
                    ErrorMessage = ""
                };
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return new ApiResponse<List<AdminUsersDto>>
                {
                    Data = null,
                    IsSuccess = false,
                    ErrorMessage = ex.Message // You can customize the error message
                };
            }
        }

        public async Task<ApiResponse<int>> GetUserConferenceId(string userId)
        {
            try
            {
                var _user = await _userManager.FindByIdAsync(userId);
                var _conferenceId = _user.ConferenceId;

                if (_conferenceId != null)
                {
                    return new ApiResponse<int>
                    {
                        Data = (int)_conferenceId,
                        ErrorMessage = "",
                        IsSuccess = true
                    };
                }

                return new ApiResponse<int>
                {
                    Data = 0,
                    IsSuccess = true,
                    ErrorMessage = "No conference id found"
                };
                
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Data = 0,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }



    }
}
