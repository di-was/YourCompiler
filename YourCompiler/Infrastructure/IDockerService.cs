
namespace YourCompiler.Infrastructure
{
    public interface IDockerService
    {
        public static abstract Task<string> runContainer(ContainerDetails details);
    }
}
