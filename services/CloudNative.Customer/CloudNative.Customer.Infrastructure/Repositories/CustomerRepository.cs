using CloudNative.Customer.Core.Entities;
using CloudNative.Customer.Core.Interfaces;

namespace CloudNative.Customer.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        Task<bool> ICustomerRepository.CreateCustomer(CreateCustomerEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
