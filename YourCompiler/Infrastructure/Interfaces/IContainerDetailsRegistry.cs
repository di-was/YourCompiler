namespace YourCompiler.Infrastructure.Interfaces
{
    public interface IContainerDetailsRegistry
    {
        public abstract ContainerDetails Resolve(string language);
    }
}
