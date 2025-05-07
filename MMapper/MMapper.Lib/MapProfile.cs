using System;
using System.Collections.Generic;

namespace MMapper.Lib
{
    public class MapProfile<TSource, TDestination>
    {
        private readonly Dictionary<string, string> _propertyMap = new Dictionary<string, string>();

        public void ForMember(string sourcePropertyName, string destinationPropertyName)
        {
            if (!string.IsNullOrWhiteSpace(sourcePropertyName) && !string.IsNullOrWhiteSpace(destinationPropertyName))
            {
                _propertyMap[sourcePropertyName] = destinationPropertyName;
            }
        }

        public Dictionary<string, string> GetMappings()
        {
            return _propertyMap;
        }
    }
}
