namespace DotNet.Testcontainers.Tests.Unit.Containers.Unix.LocalStack
{
  using System.Threading.Tasks;
  using DotNet.Testcontainers.Containers.Builders;
  using DotNet.Testcontainers.Containers.Configurations.LocalStack;
  using DotNet.Testcontainers.Containers.Modules.LocalStack;
  using Xunit;

  public class LocalStackTestcontainerTest //: IClassFixture<LocalStackFixture>
  {
    //private readonly LocalStackFixture localStackFixture;

    //public LocalStackTestcontainerTest(LocalStackFixture localStackFixture)
    //{
    //  this.localStackFixture = localStackFixture;
    //}

    [Fact]
    public async Task ConnectionEstablishedWithConfig()
    {
      // Given
      var testcontainersBuilder = new TestcontainersBuilder<LocalStackTestcontainer>()
        .WithLocalStack(new LocalStackTestcontainerConfiguration
        {
          DebugMode = true,
          Services = new LocalStackTestcontainerConfiguration.Service[] {
            LocalStackTestcontainerConfiguration.Service.S3
          }
        });

      // When
      // Then
      await using (var testcontainer = testcontainersBuilder.Build())
      {
        await testcontainer.StartAsync();

        Assert.NotNull(testcontainer.Endpoint);
      }
    }

    [Fact]
    public async Task ConnectionEstablishedWithNoConfig()
    {
      // Given
      var testcontainersBuilder = new TestcontainersBuilder<LocalStackTestcontainer>()
        .WithLocalStack();

      // When
      // Then
      await using (var testcontainer = testcontainersBuilder.Build())
      {
        await testcontainer.StartAsync();

        Assert.NotNull(testcontainer.Endpoint);
      }
    }
  }
}
