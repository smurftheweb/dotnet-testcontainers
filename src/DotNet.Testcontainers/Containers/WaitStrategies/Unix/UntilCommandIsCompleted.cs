namespace DotNet.Testcontainers.Containers.WaitStrategies.Unix
{
  using System;
  using System.Threading.Tasks;
  using DotNet.Testcontainers.Clients;

  internal class UntilCommandIsCompleted : IWaitUntil
  {
    private readonly string[] command;

    public UntilCommandIsCompleted(string command) : this("/bin/sh", "-c", command)
    {
    }

    public UntilCommandIsCompleted(params string[] command)
    {
      this.command = command;
    }

    public virtual async Task<bool> Until(Uri endpoint, string id)
    {
      using (var client = new TestcontainersClient(endpoint))
      {
        var exitCode = await client.ExecAsync(id, this.command)
          .ConfigureAwait(false);
        return 0L.Equals(exitCode);
      }
    }
  }
}
