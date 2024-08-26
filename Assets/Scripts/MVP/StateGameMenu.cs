using UnityEngine.UIElements;

namespace Assets.MVP
{
    public class StateGameMenu : IStateView
    {
        private VisualElement _root;

        public StateGameMenu(VisualElement root)
        {
            _root = root;
        }
    }
}