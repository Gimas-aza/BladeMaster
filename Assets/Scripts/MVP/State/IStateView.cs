using Assets.DI;

namespace Assets.MVP.State
{
    public interface IStateView
    {
        void Init(StateMachine stateMachine, UIElements elements, UIEvents events, DIContainer container);
        void Enter();
        void Exit();
    }
}