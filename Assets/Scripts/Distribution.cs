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
        float weight;
        public float Weight { get { return weight; } set { weight = value; } }

        public float CombinedWeight { get; set; }

        [Range(0f, 100f), SerializeField]
        float percentage;
        public float Percentage { get { return percentage; } set { if (value >= 0 && value <= 100) percentage = value; } }

        [SerializeField]
        bool unique;
        public bool Unique { get { return unique; } }

        [SerializeField]
        T value;
        public T Value { get { return value; } set { this.value = value; } }
    }

    public abstract class Distribution<T, T_ITEM> : MonoBehaviour
        where T_ITEM : DistributionItem<T>, new()
    {
        [SerializeField]
        List<T_ITEM> items;
        public List<T_ITEM> Items { get { return items; } }

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
            if (!addedItem && items.Count > nbItems)
                items[items.Count - 1].Weight = 0;

            foreach (T_ITEM item in items)
            {
                if (item.Weight < 0)
                    item.Weight = 0;
            }

            ComputePercentages();
            nbItems = items.Count;
        }

        void ComputePercentages()
        {
            combinedWeight = 0;

            foreach (T_ITEM item in items)
            {
                combinedWeight += item.Weight;
                item.CombinedWeight = combinedWeight;
            }

            foreach (T_ITEM item in items)
                item.Percentage = item.Weight * 100 / combinedWeight;
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
                    if (random <= item.CombinedWeight)
                    {
                        return item.Value;
                    }
                }

                nbIterations++;
            }

            throw new UnityException("Error while drawing an item.");
        }

        public void Add(T value, float weight)
        {
            items.Add(new T_ITEM { Value = value, Weight = weight });
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
                T val = items[i].Value;
                if (EqualityComparer<T>.Default.Equals(val, value))
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetItems(List<T_ITEM> referenceItems)
        {
            List<T_ITEM> temp = new List<T_ITEM>(referenceItems);
            items = temp;
        }
    }
}