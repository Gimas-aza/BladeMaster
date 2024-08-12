namespace Assets.EntryPoint
{
    public interface IObject 
    {
        T CreateObject<T>() where T : UnityEngine.Behaviour;
    }
}
