using CloudNative.Customer.Core.Entities;
using CloudNative.Customer.Core.Interfaces;
using CloudNative.Customer.Infrastructure.Database;
using System.Net;

namespace CloudNative.Customer.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        async Task<bool> ICustomerRepository.CreateCustomer(CreateCustomerEntity entity, CancellationToken cancellationToken)
        {

            var customerEntity = new Customers
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Address = entity.Address,
                City = entity.City,
                CountryId = entity.CountryId,
                PhoneNumber = entity.PhoneNumber,
                IsActive = entity.IsActive
            };

            await _context.Customers.AddAsync(customerEntity, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);
            entity.Id = customerEntity.Id;
            return result > 0;
        }
    }
}
