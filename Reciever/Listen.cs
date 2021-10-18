using Microsoft.Extensions.DependencyInjection;
using Mongo;
using Mongo.Models;
using MongoDB.Bson;
using RabbitMQ;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Reciever
{
    public class Listen: RabbitListener
    {
        private readonly IMongoRepository<CustomerRequestBody> _CostumerReqeust;
        private readonly IMongoRepository<UserRequestBody> _UserRequest;


        public Listen(IServiceProvider serviceProvider, RabitMqSettings settings) : base(settings)
        {
            var CreatedScope = serviceProvider.CreateScope().ServiceProvider;
            _CostumerReqeust = CreatedScope.GetRequiredService<IMongoRepository<CustomerRequestBody>>();
            _UserRequest = CreatedScope.GetRequiredService<IMongoRepository<UserRequestBody>>();
        }
        // Consume the Data From RabbitMq Queue and save body of request to mongodb 
        public override void ConsumerMessages(string content)
        {
            Request Response = JsonSerializer.Deserialize<Request>(content);
            if (!String.IsNullOrWhiteSpace(Response.Body))
            {
                // OperationId 1 means Costumer model, Else for User Data Insert
                if (Response.OperationId == 1)
                {
                    var body = JsonSerializer.Deserialize<Customer>(Response.Body);

                    List<Customer> CustomerBody = new List<Customer> { body };

                    var reqDataMongo = new CustomerRequestBody
                    {
                        Body = CustomerBody,
                        Header = Response.Header,
                        IP = Response.Connection,
                        Path = Response.Path
                    };
                    _CostumerReqeust.InsertOne(reqDataMongo);
                }
                else
                {
                    var body = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<User>(Response.Body);

                    List<User> UserBody = new List<User> { body };

                    var reqDataForUser = new UserRequestBody
                    {
                        Body = UserBody,
                        Header = Response.Header,
                        IP = Response.Connection,
                        Path = Response.Path
                    };
                    _UserRequest.InsertOne(reqDataForUser);
                }
            }
        }
    }
}
