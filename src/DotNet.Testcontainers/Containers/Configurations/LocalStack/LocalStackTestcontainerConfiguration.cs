namespace DotNet.Testcontainers.Containers.Configurations.LocalStack
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using DotNet.Testcontainers.Containers.Configurations.Abstractions;
  using DotNet.Testcontainers.Containers.WaitStrategies;

  public class LocalStackTestcontainerConfiguration : HostedServiceConfiguration
  {
    private const string LocalStackImage = "localstack/localstack:0.12.6";

    private const int LocalStackPort = 4566; // since 0.11, only one port is made public

    public bool DebugMode { get; set; } = true;
    public string Region { get; set; } = "eu-central-1";

    private Service[] _Services;
    public Service[] Services {
      get { return this._Services; }
      set {
        this._Services = value;
        this.Environments.Remove("SERVICES");
        this.Environments.Add("SERVICES", this.ServicesList);
      } }
    public string ServicesList { get { return Services == null ? null : ServicesListToDockerInput(Services); } }

    public LocalStackTestcontainerConfiguration() : this(LocalStackImage)
    {
    }

    public LocalStackTestcontainerConfiguration(string image) : base(image, LocalStackPort)
    {
      // prepare environment for serverless - todo: add option here!
      this.Environments.Add("DEFAULT_REGION", "eu-central-1");
      this.Environments.Add("SERVICES", "serverless");
      this.Environments.Add("DOCKER_HOST", "unix:///var/run/docker.sock");
      this.Environments.Add("DEBUG", this.DebugMode ? "1" : "0");
    }

    public override IWaitForContainerOS WaitStrategy => Wait.ForUnixContainer()
      .UntilPortIsAvailable(LocalStackPort);
      // todo: add wait for message strategy

    #region AWS Services
    public enum Service
    {
      API_GATEWAY,
      KINESIS,
      DYNAMODB,
      DYNAMODB_STREAMS,
      S3,
      LAMBDA,
      IAM
    }
    

    /// <summary>
    /// The details of the available services. I am not sure we care about ports after v0.11
    /// </summary>
    private readonly Dictionary<Service, LocalStackService> servicesMap = new Dictionary<Service, LocalStackService>()
    {
      { Service.API_GATEWAY,      new LocalStackService(Service.API_GATEWAY, "apigateway", 4567) },
      { Service.KINESIS,          new LocalStackService(Service.KINESIS, "kinesis", 4568) },
      { Service.DYNAMODB,         new LocalStackService(Service.DYNAMODB, "dynamodb", 4569) },
      { Service.DYNAMODB_STREAMS, new LocalStackService(Service.DYNAMODB_STREAMS, "dynamodbstreams", 4570) },
      { Service.S3,               new LocalStackService(Service.S3, "s3", 4572) },
      { Service.LAMBDA,           new LocalStackService(Service.LAMBDA, "lambda", 4574) },
      { Service.IAM,              new LocalStackService(Service.IAM, "iam", 4593) },
    };

    private string ServicesListToDockerInput(Service[] services)
    {
      var builder = new StringBuilder();
      foreach (var service in services)
      {
        var localService = this.servicesMap[service];
        builder.Append(localService.LocalStackName);
        builder.Append(',');
      }
      var result = builder.ToString();
      return result.Remove(result.Length - 1);
    }

    /// <summary>
    /// Represents an available AwsService to be hosted by the localstack
    /// </summary>
    private readonly struct LocalStackService
    {
      public LocalStackService(Service service, string localStackName, int internalPort) {
        this.Service = service;
        this.LocalStackName = localStackName;
        this.InternalPort = internalPort;
      }

      public Service Service { get; }
      public string LocalStackName { get; }
      public int InternalPort { get; }

      public override string ToString()
      {
        return $"{nameof(Service)} ({LocalStackName} => {InternalPort}";
      }
    }

    #endregion
  }
}
