using System.Reflection;

namespace ConferenceBookingAPI.Services.Extensions
{
    public static class ObjectExtensions
    {
        public static void UpdateFrom<TTarget, TSource>(this TTarget target, TSource source)
        {
            var targetProperties = typeof(TTarget).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var targetProperty in targetProperties)
            {
                var sourceProperty = sourceProperties.FirstOrDefault(p => p.Name == targetProperty.Name &&
                                                                          p.PropertyType == targetProperty.PropertyType);
                if (sourceProperty != null)
                {
                    var sourceValue = sourceProperty.GetValue(source);
                    if (sourceValue != null)
                    {
                        targetProperty.SetValue(target, sourceValue);
                    }
                }
            }
        }
    }
}
