namespace DotNet.Testcontainers.Containers.Builders
{
  using System.Linq;
  using DotNet.Testcontainers.Containers.Configurations.LocalStack;
  using DotNet.Testcontainers.Containers.Modules.LocalStack;
  using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
  using static DotNet.Testcontainers.Containers.Configurations.LocalStack.LocalStackTestcontainerConfiguration;

  /// <summary>
  /// This class applies the extended Testcontainer configurations for LocalStack.
  /// </summary>
  public static class TestcontainersBuilderLocalStackExtension
  {
    public static ITestcontainersBuilder<LocalStackTestcontainer> WithLocalStack(this ITestcontainersBuilder<LocalStackTestcontainer> builder, LocalStackTestcontainerConfiguration configuration)
    {
      builder = configuration.Environments.Aggregate(builder, (current, environment) => current.WithEnvironment(environment.Key, environment.Value));

      return builder
        .WithImage(configuration.Image)
        .WithPortBinding(configuration.Port, configuration.DefaultPort)
        .WithWaitStrategy(configuration.WaitStrategy)
        .ConfigureContainer(container =>
        {
          container.ContainerPort = configuration.DefaultPort;
        });
    }

    public static ITestcontainersBuilder<LocalStackTestcontainer> WithLocalStack(this ITestcontainersBuilder<LocalStackTestcontainer> builder)
    {
      LocalStackTestcontainerConfiguration configuration = new LocalStackTestcontainerConfiguration()
      {
        DebugMode = true,
        Region = "eu-central-1",
        Services = new Service[] { Service.S3, Service.IAM, Service.LAMBDA, Service.DYNAMODB, Service.DYNAMODB_STREAMS }
      };

      builder = configuration.Environments.Aggregate(builder, (current, environment) => current.WithEnvironment(environment.Key, environment.Value));

      return builder
        .WithImage(configuration.Image)
        .WithPortBinding(configuration.Port, configuration.DefaultPort)
        .WithWaitStrategy(configuration.WaitStrategy)
        .ConfigureContainer(container =>
        {
          container.ContainerPort = configuration.DefaultPort;
        });
    }
  }
}
