using AutoMapper;
using CloudNative.Customer.Application.Features.Customer.Commands;
using CloudNative.Customer.Core.Entities;

namespace CloudNative.Customer.Application.Mappers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Mapping Create Customer to Create Customer Command
            CreateMap<CreateCustomerEntity, CreateCustomerCommand>().ReverseMap();
        }
    }

}
