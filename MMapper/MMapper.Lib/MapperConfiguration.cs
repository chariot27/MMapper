using System;
using System.Collections.Generic;

namespace MMapper.Lib
{
    public class MapperConfiguration
    {
        private readonly Dictionary<Tuple<Type, Type>, object> _profiles = new Dictionary<Tuple<Type, Type>, object>();

        public void CreateMap<TSource, TDestination>(Action<MapProfile<TSource, TDestination>> config = null)
        {
            var key = Tuple.Create(typeof(TSource), typeof(TDestination));

            if (!_profiles.ContainsKey(key))
            {
                var profile = new MapProfile<TSource, TDestination>();
                if (config != null)
                {
                    config(profile);
                }
                _profiles[key] = profile;
            }
        }

        public MapProfile<TSource, TDestination> GetProfile<TSource, TDestination>()
        {
            var key = Tuple.Create(typeof(TSource), typeof(TDestination));
            object profile;

            if (_profiles.TryGetValue(key, out profile))
            {
                return profile as MapProfile<TSource, TDestination>;
            }

            return null;
        }
    }
}
