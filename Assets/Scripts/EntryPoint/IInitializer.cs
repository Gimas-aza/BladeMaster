using System.Collections.Generic;
using Assets.MVP;
using Assets.MVP.Model;
using UnityEngine;

namespace Assets.EntryPoint
{
    public interface IInitializer
    {
        void Init() {}
        void Init(ISaveSystem saveSystem) {}
        void Init(List<IModel> model) {}
        void Init(Presenter presenter, StateView currentState) {}
        void Init(GameObject gameObject) {}
    }
}