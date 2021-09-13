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
        private readonly IMongoRepository<ReqDataMongo> _CostumerReqeust;
        private readonly IMongoRepository<ReqDataForUser> _UserRequest;


        public Listen(IServiceProvider serviceProvider, RabitMqSettings settings) : base(settings)
        {
            var CreatedScope = serviceProvider.CreateScope().ServiceProvider;
            _CostumerReqeust = CreatedScope.GetRequiredService<IMongoRepository<ReqDataMongo>>();
            _UserRequest = CreatedScope.GetRequiredService<IMongoRepository<ReqDataForUser>>();
        }
        public override void ConsumerMessages(string content)
        {
            ReqData Response = JsonSerializer.Deserialize<ReqData>(content);
            if (!String.IsNullOrWhiteSpace(Response.Body))
            {
                if (Response.OperationId == 1)
                {

                    var body = JsonSerializer.Deserialize<Customer>(Response.Body);

                    List<Customer> CustomerBody = new List<Customer> { body };

                    var reqDataMongo = new ReqDataMongo
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

                    var reqDataForUser = new ReqDataForUser
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
