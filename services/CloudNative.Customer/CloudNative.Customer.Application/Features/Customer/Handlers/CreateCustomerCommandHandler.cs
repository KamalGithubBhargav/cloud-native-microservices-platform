using CloudNative.ConfigLibrary.Interfaces;
using CloudNative.Customer.Application.Features.Customer.Commands;
using CloudNative.Customer.Application.Mappers;
using CloudNative.Customer.Core.Constants;
using CloudNative.Customer.Core.Entities;
using CloudNative.Customer.Core.Interfaces;
using Confluent.Kafka;
using MediatR;
using System.Text.Json;

namespace CloudNative.Customer.Application.Features.Customer.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;
        private readonly IKafkaProducerService _producer;

        public CreateCustomerCommandHandler(ICustomerRepository repository, IKafkaProducerService producer)
        {
            _repository = repository;
            _producer = producer;
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var companyProfileTypeReq = CustomerMapper.Mapper.Map<CreateCustomerEntity>(request);
            var response = await _repository.CreateCustomer(companyProfileTypeReq, cancellationToken);
            if (response)
            {
                var consumerInfo = KafkaRegistry.Consumers.First(x => x.Topic == "customer-topic");
                var message = JsonSerializer.Serialize(request);

                await _producer.ProduceAsync(consumerInfo.Topic, message);

            }
            return response;
        }
    }
}
