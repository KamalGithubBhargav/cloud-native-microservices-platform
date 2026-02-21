using CloudNative.Customer.Application.Features.Customer.Commands;
using CloudNative.Customer.Application.Mappers;
using CloudNative.Customer.Core.Entities;
using CloudNative.Customer.Core.Interfaces;
using MediatR;

namespace CloudNative.Customer.Application.Features.Customer.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;

        public CreateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var companyProfileTypeReq = CustomerMapper.Mapper.Map<CreateCustomerEntity>(request);
            var response = await _repository.CreateCustomer(companyProfileTypeReq, cancellationToken);
            return response;
        }
    }
}
