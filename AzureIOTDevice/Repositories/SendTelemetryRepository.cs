using AzureIOT.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Repositories
{
    public class SendTelemetryRepository
    {
        public static RegistryManager? registryManager;
        public static DeviceClient? client;
        private static string connStringIotHub = "HostName=ashuiotdevicehub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=LP9BHd5P6jCgQ7t9zRoj2lvaKFbXNHR02itvAQdOaIY=";
        public static string connStringDevice = "HostName=ashuiotdevicehub.azure-devices.net;DeviceId=demodevice;SharedAccessKey=WMUXh7LOdyDNUAHMsPUBbtUai1g5QvroA2DTyFAbZLQ=";

        public static async Task SendMessage(string deviceId)
        {
            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(connStringIotHub);
                var device = await registryManager.GetTwinAsync(deviceId);
                DevicePropertiesModel properties = new DevicePropertiesModel();
                TwinCollection Prop;
                Prop = device.Properties.Reported;
                client = DeviceClient.CreateFromConnectionString(connStringDevice,
                    Microsoft.Azure.Devices.Client.TransportType.Mqtt);

                Random rand = new Random();

                while (true)
                {
                    var telemetry = new
                    {
                        temperature = Prop["temperature"] + rand.NextDouble() * 15,
                        humidity = Prop["humidity"] + rand.NextDouble() * 20
                };
                    var telemetryString = JsonConvert.SerializeObject(telemetry);
                    var message = new Microsoft.Azure.Devices.Client.Message(
                        Encoding.ASCII.GetBytes(telemetryString));
                    await client.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, telemetryString);
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
