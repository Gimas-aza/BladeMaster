namespace Assets.DataStorageSystem
{
    public interface IStorage
    {
        DataStorage Load();
        void Save();
    }
}