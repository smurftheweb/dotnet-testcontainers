namespace DotNet.Testcontainers.Containers.Modules.LocalStack
{
  using System;
  using System.Collections.Generic;
  using System.Text;
    using DotNet.Testcontainers.Containers.Configurations;
  using DotNet.Testcontainers.Containers.Configurations.LocalStack;
  using DotNet.Testcontainers.Containers.Modules.Abstractions;
  using static DotNet.Testcontainers.Containers.Configurations.LocalStack.LocalStackTestcontainerConfiguration;

  public class LocalStackTestcontainer : HostedServiceContainer
  {
    public string Region { get; private set; }
    public string SecretKey { get { return "secretkey"; } }
    public string AccessKey { get { return "accesskey"; } }

    internal LocalStackTestcontainer(ITestcontainersConfiguration configuration) : base(configuration)
    {
      if (configuration is LocalStackTestcontainerConfiguration localConfig)
      {
        this.Region = localConfig.Region;
      }
      else
      {
        this.Region = "eu-central-1";
      }

    }

    public Uri Endpoint { get
      {
        var host = string.IsNullOrEmpty(this.IpAddress) ? this.Hostname : this.IpAddress;
        return new Uri("http://" + host + ":" + this.Port);
      }
    }

    //public LocalStackTestcontainer WithServices(params Service[] services)
    //{
    //  this.selectedServices.AddRange(services);
    //  return this;
    //}

  }
}
