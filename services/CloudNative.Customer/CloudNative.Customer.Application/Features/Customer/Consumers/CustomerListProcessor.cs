using CloudNative.ConfigLibrary.Interfaces;

namespace CloudNative.Customer.Application.Features.Customer.Consumers
{
    public class CustomerListProcessor : IMessageProcessor
    {
        public Task ProcessAsync(string message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
