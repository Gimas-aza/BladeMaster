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
            _knives = new List<IKnife>();
            _knivesIndex = 0;
        }

        public void CreateKnife(GameObject templateKnife, int amount)
        {
            if (templateKnife == null)
            {
                Debug.LogError("Template knife is null");
                return;
            }

            for (var i = 0; i < amount; i++)
            {
                var newKnifeObject = Instantiate(templateKnife, transform);
                if (!newKnifeObject.TryGetComponent<IKnife>(out var newKnife))
                {
                    Debug.LogError("KnifeComponent is missing on the instantiated object");
                    Destroy(newKnifeObject);
                    continue;
                }

                newKnife.SetTransform(transform);
                newKnife.SetActive(false);
                _knives.Add(newKnife);
            }

            templateKnife.SetActive(false);

            if (_knives.Count > 0)
            {
                _knives[0].SetActive(true);
            }
        }

        public void ThrowKnife(float force)
        {
            if (_knives == null || _knives.Count == 0)
            {
                Debug.LogError("No knives available in the pool");
                return;
            }

            if (_knivesIndex < _knives.Count)
            {
                var knifeToThrow = _knives[_knivesIndex];
                knifeToThrow.SetActive(true);
                knifeToThrow.SetGlobalParent();
                knifeToThrow.Throw(force);

                _knivesIndex++;

                if (_knivesIndex < _knives.Count)
                {
                    _knives[_knivesIndex].SetActive(true);
                }
            }
        }

        public List<IKnife> GetKnives() => _knives;
    }
}
