using AutoMapper;

namespace CloudNative.Customer.Application.Mappers
{
    public class CustomerMapper
    {
        /// <summary>
        /// Mapper
        /// </summary>
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod!.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<CustomerMappingProfile>();
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
