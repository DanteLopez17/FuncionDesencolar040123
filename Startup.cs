using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(FuncionDesencolar040123.Startup))]
namespace FuncionDesencolar040123
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            var cadenaAzure = "Endpoint=sb://practicaazure.servicebus.windows.net/;SharedAccessKeyName=DirectivaServiceBus;SharedAccessKey=dRoNmhwqXKMP29XF1kQHeYC6fgvZwZiCstHWD5TnEoo=;EntityPath=primerazure";

            //string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["AzureBusConnection"].ConnectionString;


            builder.Services.AddSingleton((p) =>
            {
                return new ServiceBusClient(cadenaAzure, new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets });
            });

            builder.Services.AddScoped<IServicio, Servicio>();
        }
    }
}
