using System.Collections.Generic;
using System.Linq;
using Assets.MVP;
using UnityEngine;

namespace Assets.Player
{
    public class KnivesPoolComponent : MonoBehaviour, IAmountOfKnives
    {
        private List<IKnife> _knives;
        private int _knivesIndex;

        public int Amount => _knives.Count - _knivesIndex;
        public int MaxAmount => _knives.Count;

        private void Awake()
        {
            _knives = new();
            _knivesIndex = 0;
        }

        public void CreateKnife(GameObject templateKnife, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var newObject = GameObject
                    .Instantiate(templateKnife, transform)
                    .TryGetComponent<IKnife>(out var newKnife);
                if (!newObject)
                    Debug.LogError("KnifeComponent is null");

                newKnife.SetTransform(transform);
                newKnife.SetActive(false);
                _knives.Add(newKnife);
            }

            Destroy(templateKnife);
            _knives[0].SetActive(true);
        }

        public void ThrowKnife(float force)
        {
            if (_knives.Count == 0)
                Debug.LogError("Knives list is empty");

            if (_knivesIndex < _knives.Count)
            {
                _knives[_knivesIndex].SetActive(true);
                _knives[_knivesIndex].SetGlobalParent();
                _knives[_knivesIndex].Throw(force);
                _knivesIndex++;

                var currentKnives = _knives
                    .Where((knife, index) => index == _knivesIndex)
                    .FirstOrDefault();
                currentKnives?.SetActive(true);
            }
        }

        public List<IKnife> GetKnives()
        {
            return _knives;
        }
    }
}
