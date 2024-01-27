using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    [Serializable]
    internal class BackgroundLayer
    {
        public int numBgElements;
        public List<GameObject> bgElementPrefab;
        public GameObject bgElementAnchor;
        public Vector3 posMin;
        public Vector3 posMax;
        public float scaleMin;
        public float scaleMax;
    }
    internal class BackgroundCrafter: MonoBehaviour
    {
        [SerializeField] private List<BackgroundLayer> backgroundLayers;

        private void Awake()
        {
            foreach(BackgroundLayer layer in backgroundLayers)
            {
                GameObject bgElement;
                for (int i = 0; i < layer.numBgElements; i++)
                {
                    bgElement = Instantiate(layer.bgElementPrefab.GetRandom());

                    Vector3 pos = Vector3.zero;
                    pos.x = Random.Range(layer.posMin.x, layer.posMax.x);
                    pos.y = Random.Range(layer.posMin.y, layer.posMax.y);
                    pos.z = Random.Range(layer.posMin.z, layer.posMax.z);

                    float scaleU = Random.value;
                    float scaleV = Mathf.Lerp(layer.scaleMin, layer.scaleMax, scaleU);

                    pos.y = Mathf.Lerp(layer.posMin.y, pos.y, scaleU);

                    pos.z = layer.posMin.z + (100 - 90 * scaleU) / 100  * (layer.posMax.z - layer.posMin.z);

                    bgElement.transform.position = pos;
                    bgElement.transform.localScale = Vector3.one * scaleV;

                    bgElement.transform.SetParent(layer.bgElementAnchor.transform);
                }
            }
        }
    }
}
