using Assets.EntryPoint;

namespace Assets.MVP.Model
{
    public interface IModel
    {
        void SubscribeToEvents(IResolver container);
    }
}
