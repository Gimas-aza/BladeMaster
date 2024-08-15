namespace Assets.EntryPoint
{
    public interface IInitializer
    {
        void Init() {}
        void Init(ISaveSystem saveSystem) {}
    }
}