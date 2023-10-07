using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeightedDistribution
{
    [System.Serializable]
    public class DistributionItem<T>
    {
        [SerializeField]
        public float weight;

        public float combinedWeight;

        [Range(0f, 100f), SerializeField]
        public float percentage;

        [SerializeField]
        public T value;
    }

    public abstract class Distribution<T, T_ITEM> : MonoBehaviour
        where T_ITEM : DistributionItem<T>, new()
    {
        [SerializeField]
        public List<T_ITEM> items;

        bool firstCompute = false;
        int nbItems;
        float combinedWeight;

        void Start()
        {
            
        }

        void OnItemsChange(bool addedItem = false)
        {
            // On Add Component
            if (items == null)
                return;

            if (!firstCompute)
            {
                nbItems = items.Count;
                firstCompute = true;
            }

            // On Add Item
            // if (!addedItem && items.Count > nbItems)
            //     items[items.Count - 1].Weight = 0;

            foreach (T_ITEM item in items)
            {
                if (item.weight < 0)
                    item.weight = 0;
            }

            ComputePercentages();
            nbItems = items.Count;
        }

        void ComputePercentages()
        {
            combinedWeight = 0;

            foreach (T_ITEM item in items)
            {
                combinedWeight += item.weight;
                item.combinedWeight = combinedWeight;
            }

            foreach (T_ITEM item in items)
                item.percentage = item.weight * 100 / combinedWeight;
        }

        void OnValidate()
        {
            OnItemsChange();
        }

        public T Draw()
        {
            if (items.Count == 0)
                throw new UnityException("Can't draw an item from an empty distribution!");

            ComputePercentages();

            int nbIterationsMax = 40;
            int nbIterations = 0;

            while (nbIterations < nbIterationsMax)
            {
                float random = Random.Range(0f, combinedWeight);
                foreach (T_ITEM item in items)
                {
                    if (random <= item.combinedWeight)
                    {
                        return item.value;
                    }
                }

                nbIterations++;
            }

            throw new UnityException("Error while drawing an item.");
        }

        public void Add(T value, float weight)
        {
            items.Add(new T_ITEM { value = value, weight = weight });
            OnItemsChange(true);
        }

        public void RemoveAt(int index)
        {
            if (items.Count - 1 < index || index < 0)
                return;
            items.RemoveAt(index);
            OnItemsChange();
        }

        public int IndexOf(T value)
        {
            for (int i = 0; i < items.Count; i++)
            {
                T val = items[i].value;
                if (EqualityComparer<T>.Default.Equals(val, value))
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetItems(List<T_ITEM> referenceItems)
        {
            List<T_ITEM> temp = new List<T_ITEM>();
            for (int i = 0; i < referenceItems.Count; i++)
            {
                T_ITEM item = new T_ITEM
                {
                    weight = referenceItems[i].weight,
                    combinedWeight = referenceItems[i].combinedWeight,
                    percentage = referenceItems[i].percentage,
                    value = referenceItems[i].value
                };
                temp.Add(item);
            }
            items = temp;
        }
    }
}