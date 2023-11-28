using k8s;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace KubApp.Controllers
{
    [ApiController]
    [Route("greetings")]
    public class MainController : ControllerBase
    {
        [HttpGet(Name = "GetMessage")]
        public async Task<string> Get()
        {
            var podInfos = new StringBuilder();

            try
            {
                var config = KubernetesClientConfiguration.InClusterConfig();
                var client = new Kubernetes(config);

                var pods = await client.ListNamespacedPodAsync("default");

                pods.Items.ToList().ForEach(x => podInfos.AppendLine(x.Metadata.Name + " - " + x.Spec.NodeName));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            string hostName = GetHostName();

            return $"{podInfos}\r\nHello Everyone!\r\n\r\nHow Are You?\r\n\r\nFine, Thank You!\r\n\r\nThis came from {hostName}!";
        }

        private string GetHostName()
        {
            string ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();

            try
            {
                // Obtém o nome do host associado ao endereço IP
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                return $"Não foi possível obter o nome do host: {ex.Message}";
            }
        }
    }
}