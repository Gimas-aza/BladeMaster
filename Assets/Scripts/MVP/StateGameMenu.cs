using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class StateGameMenu : IStateView
    {
        private VisualElement _root;
        private Presenter _presenter;

        public event UnityAction<float> MonitorInputRotation;

        public StateGameMenu(VisualElement root, Presenter presenter)
        {
            _root = root;
            OnMonitorInput();
            presenter.RegisterEventsForView(ref MonitorInputRotation);
        }

        private async void OnMonitorInput()
        {
            while (true)
            {
                Vector3 acceleration = Input.acceleration;
                if (acceleration.x > 0.3f || acceleration.x < -0.3f) 
                {
                    MonitorInputRotation?.Invoke(acceleration.x);
                }

                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}