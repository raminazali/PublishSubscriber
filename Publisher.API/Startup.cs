using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mongo;
using Mongo.Models;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Interface;
using Reciever;
using ResponseApi;
using ResponseApi.Interface;
using Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Publisher.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            

            services.AddHttpContextAccessor();

            

            services.Configure<RabitMqSettings>(Configuration.GetSection(nameof(RabitMqSettings)));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabitMqSettings>>().Value);


            services.AddSingleton<IResponseApi, ResponseApiC>();

            services.AddSingleton<IRequestClass, Requester>();

            services.AddHostedService<RabbitListener>();
            services.AddHostedService<Listen>();

            services.Configure<MongoDbSettings>(Configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            services.AddSwaggerGen();

            /*services.AddSingleton(serviceProvider =>
            {
                var uri = new Uri("amqp://guest:raminazali@localhost:5672");
                return new ConnectionFactory
                {
                    Uri = uri,
                    DispatchConsumersAsync = true
                };
            });*/


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project v1"); });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<VistiMiddleware>();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
