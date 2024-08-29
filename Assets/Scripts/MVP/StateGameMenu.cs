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
        private bool _isClampingTouch = false;

        public event UnityAction<float> MonitorInputRotation;
        public event UnityAction MonitorInputTouchBegin;
        public event UnityAction MonitorInputTouchEnded;

        public StateGameMenu(VisualElement root, Presenter presenter)
        {
            _root = root;
            presenter.RegisterEventsForView(ref MonitorInputRotation, ref MonitorInputTouchBegin, ref MonitorInputTouchEnded);
            OnMonitorInputInFixedUpdate();
            OnMonitorInputInUpdate();
        }

        private async void OnMonitorInputInFixedUpdate()
        {
            while (true)
            {
                OnMonitorInputAcceleration();

                await UniTask.WaitForFixedUpdate();
            }
        }

        private void OnMonitorInputAcceleration()
        {
            Vector3 acceleration = Input.acceleration;
            if (acceleration.x > 0.3f || acceleration.x < -0.3f) 
            {
                MonitorInputRotation?.Invoke(acceleration.x);
            }
        }

        private async void OnMonitorInputInUpdate()
        {
            while (true)
            {
                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        _isClampingTouch = true;
                        MonitorInputTouchBegin?.Invoke();
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (!_isClampingTouch) return;

                        _isClampingTouch = false;
                        MonitorInputTouchEnded?.Invoke();
                    }
                }

                await UniTask.WaitForEndOfFrame();
            }
        }
    }
}