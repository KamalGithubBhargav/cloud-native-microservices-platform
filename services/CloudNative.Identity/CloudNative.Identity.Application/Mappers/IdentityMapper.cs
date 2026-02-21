using AutoMapper;

namespace CloudNative.Identity.Application.Mappers
{
    internal class IdentityMapper
    {
        /// <summary>
        /// Mapper
        /// </summary>
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<IdentityMappingProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        });

        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper = Lazy.Value;
    }
}
