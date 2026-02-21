using CloudNative.Customer.Core.Entities;

namespace CloudNative.Customer.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<bool> CreateCustomer(CreateCustomerEntity entity, CancellationToken cancellationToken);
    }
}
