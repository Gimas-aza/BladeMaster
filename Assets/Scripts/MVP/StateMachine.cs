using System;
using System.Collections.Generic;
using System.Linq;
using Assets.DI;
using Assets.MVP.State;

namespace Assets.MVP
{
    public class StateMachine
    {
        private IStateView _currentState;
        private IStateView _previousState;
        private Dictionary<Type, IStateView> _states;
        private UIElements _uiElements;
        private UIEvents _uiEvents;
        private Presenter _presenter;
        private DIContainer _container;

        public StateMachine(UIElements elements, UIEvents events, Presenter presenter, DIContainer container)
        {
            _uiElements = elements;
            _uiEvents = events;
            _presenter = presenter;
            _container = container;
            _states = new Dictionary<Type, IStateView>();
        }

        public void ChangeState<T>() where T : IStateView, new()
        {
            _currentState?.Exit();
            _previousState = _currentState;
            _currentState = GetState<T>();
            _currentState.Enter();
        }

        public void BackToPreviousState()
        {
            _currentState.Exit();
            _currentState = _previousState;
            _currentState.Enter();
        }

        public void RegisterEvents()
        {
            _presenter.RegisterEventsForView(_container);
        }

        private IStateView GetState<T>() where T : IStateView, new()
        {
            var stateType = typeof(T);
            if (!_states.Keys.Contains(stateType))
            {
                var state = new T();
                state.Init(this, _uiElements, _uiEvents, _container);
                _states.Add(stateType, state);
            }

            return _states[stateType];
        }
    }
}