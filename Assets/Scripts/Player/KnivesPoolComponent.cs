using System.Collections.Generic;
using Assets.Knife;
using UnityEngine;

namespace Assets.Player
{
    public class KnivesPoolComponent : MonoBehaviour
    {
        private List<IKnife> _knives;
        private IKnife _templateKnife;
        private float _force = 20;

        private void Awake()
        {
            _knives = new();
        }

        public void Init()
        {
        }

        public void CreateKnife(GameObject templateKnife, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var newObject = GameObject
                    .Instantiate(templateKnife, transform)
                    .TryGetComponent<KnifeComponent>(out var newKnife);
                if (!newObject)
                    Debug.LogError("KnifeComponent is null");

                _knives.Add(newKnife);
            }
        }

        private void ThrowKnife()
        {
            foreach (var knife in _knives)
            {
                knife.Throw(_force);
            }
        }
    }
}
