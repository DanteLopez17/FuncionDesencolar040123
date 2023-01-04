using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuncionDesencolar040123
{
    public class Servicio : IServicio
    {
        private readonly ServiceBusClient _serviceBusClient;

        public Servicio(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        public async Task GetQueues(CancellationToken stoppingToken)
        {
            int bandera = 0;
            ServiceBusReceiver receptor = _serviceBusClient.CreateReceiver("primerazure");
            do
            {
                ServiceBusReceivedMessage receptorMensaje = await receptor.ReceiveMessageAsync(TimeSpan.FromMilliseconds(1000), stoppingToken);
                if (receptorMensaje != null)
                {
                    var jsonString = receptorMensaje.Body.ToString();
                    if (jsonString != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(jsonString);
                        if (json.Nombre != null && json.Apellido != null && json.NroDocumento != null)
                        {
                            await receptor.CompleteMessageAsync(receptorMensaje);
                            //string cadena = "Data Source=DESKTOP-LORI9GP; Initial Catalog=AzureCliente; user id=sa;password=123456; TrustServerCertificate=True;";
                            string cadenaBdAzure = "Server=tcp:servidorbdazurecliente.database.windows.net,1433;Initial Catalog=AzureCliente;Persist Security Info=False;User ID=dante.lopez@softtek.com@servidorbdazurecliente;Password=Azuredb10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                            //string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultDbConnection"].ConnectionString;

                            SqlConnection con = new SqlConnection(cadenaBdAzure);
                            con.Open();
                            string fechaFormateada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string sqlQuery = "Insert into ClienteListo(Nombre, Apellido, NroDocumento, Date) VALUES (@Nombre, @Apellido, @NroDocumento, @Date)";
                            using (SqlCommand comando = new SqlCommand(sqlQuery, con))
                            {
                                comando.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = json.Nombre;
                                comando.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = json.Apellido;
                                comando.Parameters.Add("@NroDocumento", SqlDbType.VarChar).Value = json.NroDocumento;
                                comando.Parameters.Add("@Date", SqlDbType.VarChar).Value = fechaFormateada;
                                comando.ExecuteNonQuery();
                            }
                        }
                        else bandera = 1;
                    }
                    else bandera = 1;
                }
                else bandera = 1;
            } while (bandera == 0);
        }

    }
}
