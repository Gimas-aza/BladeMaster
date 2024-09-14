using System.Collections.Generic;
using Assets.GameProgression;
using Assets.MVP;
using Assets.MVP.Model;
using Assets.ShopManagement;
using UnityEngine;

namespace Assets.EntryPoint
{
    public interface IInitializer
    {
        void Init() { }
        void Init(
            ISpawnerEnemies spawnerEnemies,
            IKnivesPool knivesPool,
            ILevelManager levelManager
        ) { }
        void Init(List<IModel> model) { }
        void Init(Presenter presenter, StateView currentState) { }
        void Init(GameObject gameObject, int currentLevelIndex) { }
        void Init(int levelIndex) { }
        void Init(
            IShop shop,
            ISaveSystem saveSystem,
            ref IPlayerProgressionData dataStorage,
            ILevelManager levelManager
        ) { }
        void Init(ISaveSystem saveSystem, IShopData shop) { }
    }
}
