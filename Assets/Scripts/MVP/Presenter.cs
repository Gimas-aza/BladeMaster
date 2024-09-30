using System.Collections.Generic;
using Assets.DI;
using Assets.EntryPoint;
using Assets.EntryPoint.Model;

namespace Assets.MVP
{
    public class Presenter : IInitializer
    {
        private List<IModel> _models;

        public void Init(IResolver container)
        {
            _models = container.Resolve<List<IModel>>();
        }

        public void RegisterEventsForView(DIContainer diContainer)
        {
            IResolver container = diContainer;

            foreach (var model in _models)
            {
                model.SubscribeToEvents(container);
            }
        }
    }
}
