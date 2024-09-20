namespace Assets.EntryPoint
{
    public interface IResolver
    {
        T Resolve<T>(string tag = null);
    }
}