﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace AzureIOT.Repositories
{
    public class DeviceRepository
    {
        public static RegistryManager? registryManager;
        private static string connStringIotHub = "HostName=ashuiotdevicehub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=RG7hc7hCrJPY+Ohz0X/Z2Ejb3/wEphz9HGJ5cXcCfgQ=";

        public static async Task AddDeviceAsync(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentNullException("noDeviceId");
            }

            Device device;
            registryManager = RegistryManager.CreateFromConnectionString(connStringIotHub);
            device = await registryManager.AddDeviceAsync(new Device(deviceId));
            return;
        }

        public static async Task<Device> GetDeviceAsync(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentNullException("noDeviceId");
            }

            Device device;
            registryManager = RegistryManager.CreateFromConnectionString(connStringIotHub);
            device = await registryManager.GetDeviceAsync(deviceId);
            return device;
        }

        public static async Task DeleteDeviceAsync(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentNullException("noDeviceId");
            }

            registryManager = RegistryManager.CreateFromConnectionString(connStringIotHub);
            await registryManager.RemoveDeviceAsync(deviceId);
        }

        public static async Task<Device> UpdateDeviceAsync(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new ArgumentNullException("noDeviceId");
            }

            Device device;
            registryManager = RegistryManager.CreateFromConnectionString(connStringIotHub);
            device=await registryManager.GetDeviceAsync(deviceId);
            device.StatusReason = "UpdatedSuccessfully";
            device = await registryManager.UpdateDeviceAsync(device);
            return device;
        }
    }
}
