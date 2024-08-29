using System.Collections.Generic;
using Assets.Knife;
using UnityEngine;

namespace Assets.Player
{
    public class KnivesPoolComponent : MonoBehaviour
    {
        private List<IKnife> _knives;
        private IKnife _templateKnife;

        private void Awake()
        {
            _knives = new();
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
            if (_knives.Count == 0) Debug.LogError("Knives list is empty");; 

            foreach (var knife in _knives)
            {
                if (!knife.IsActive())
                {
                    knife.SetActive(true);
                    knife.Throw(force);
                    break;
                }
            }
        }
    }
}
