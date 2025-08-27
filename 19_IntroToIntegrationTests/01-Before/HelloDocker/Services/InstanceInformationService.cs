using System;

namespace HelloDocker.Services
{
    public class InstanceInformationService
    {
        private const string HostName = "HOSTNAME";
        private const string DefaultEnvInstanceGuid = "LOCAL";

        public string RetrieveInstanceInfo()
        {
            var hostName = Environment.GetEnvironmentVariable(HostName)
                ?? DefaultEnvInstanceGuid;

            return hostName.Length >= 5 ? hostName[^5..] : hostName;
        }
    }
}
