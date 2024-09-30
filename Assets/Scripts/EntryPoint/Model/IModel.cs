namespace Assets.EntryPoint.Model
{
    public interface IModel
    {
        void SubscribeToEvents(IResolver container);
    }
}
