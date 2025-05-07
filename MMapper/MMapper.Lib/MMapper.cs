using MMapper.Lib;
using System;
using System.Reflection;
using System.Text;

namespace MMapper
{
    public class MMapper
    {
        private readonly MapperConfiguration _configuration;

        public MMapper(MapperConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var profile = _configuration.GetProfile<TSource, TDestination>();
            var destination = new TDestination();

            var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var destProp in destinationProperties)
            {
                if (!destProp.CanWrite)
                    continue;

                string sourcePropName = destProp.Name;

                if (profile != null && profile.GetMappings().ContainsValue(destProp.Name))
                {
                    foreach (var kvp in profile.GetMappings())
                    {
                        if (kvp.Value == destProp.Name)
                        {
                            sourcePropName = kvp.Key;
                            break;
                        }
                    }
                }

                var sourceProp = FindProperty(sourceProperties, sourcePropName);

                if (sourceProp == null)
                    continue;

                try
                {
                    var sourceValue = sourceProp.GetValue(source, null);

                    if (sourceValue == null)
                        continue;

                    var convertedValue = TryConvert(sourceValue, destProp.PropertyType);

                    if (convertedValue != null)
                        destProp.SetValue(destination, convertedValue, null);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        string.Format("Erro ao mapear '{0}' para '{1}': {2}", sourcePropName, destProp.Name, ex.Message), ex);
                }
            }

            return destination;
        }

        private PropertyInfo FindProperty(PropertyInfo[] properties, string name)
        {
            foreach (var prop in properties)
            {
                if (prop.Name == name)
                    return prop;
            }
            return null;
        }

        private object TryConvert(object sourceValue, Type destinationType)
        {
            var sourceType = sourceValue.GetType();

            if (destinationType.IsAssignableFrom(sourceType))
                return sourceValue;

            try
            {
                if (sourceType == typeof(byte[]) && destinationType == typeof(string))
                    return Convert.ToBase64String((byte[])sourceValue);

                if (sourceType == typeof(string) && destinationType == typeof(byte[]))
                    return Convert.FromBase64String((string)sourceValue);

                return Convert.ChangeType(sourceValue, destinationType);
            }
            catch
            {
                try
                {
                    if (sourceType == typeof(string) && destinationType == typeof(byte[]))
                        return Encoding.UTF8.GetBytes((string)sourceValue);

                    if (sourceType == typeof(byte[]) && destinationType == typeof(string))
                        return Encoding.UTF8.GetString((byte[])sourceValue);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }
}
