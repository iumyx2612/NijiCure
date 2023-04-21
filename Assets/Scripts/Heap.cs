using System;
using System.Collections;
using System.Collections.Generic;


// This is a data structure to speed up the A* algorithm
public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int currentItemCount;
    
    // Constructor
    public Heap (int maxHeapSize) // Maximum size of the Heap
    {
        items = new T[maxHeapSize];
    }
    
    // Function to add item to the Heap
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item; 
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirstItem()
    {
        // Get the first item in the Heap
        T firstItem = items[0];
        // Rebuild the Heap
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        
        return firstItem;
    }
    
    // Change the priority of an item
    public void UpdateItem(T item)
    {
        SortUp(item);
        
    }
    
    // Check if Heap contains specific item
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }
    
    // Number of items in the heap
    public int Count
    {
        get { return currentItemCount; }
    }
    
    
    // Function to sort item in the Heap
    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int leftChildIndex = item.HeapIndex * 2 + 1;
            int rightChildIndex = item.HeapIndex * 2 + 2;
            int higherPrioIndex = 0;
            
            // If the item has at least one child
            if (leftChildIndex < currentItemCount)
            {
                higherPrioIndex = leftChildIndex;
                // If the item has child on the right
                if (rightChildIndex < currentItemCount)
                {
                    // Which child has higher priority
                    // Then swap index to that child
                    if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0)
                    {
                        higherPrioIndex = rightChildIndex;
                    }
                }
                // Check if parent has higher priority than its highest priority child
                // Then swap
                if (item.CompareTo(items[higherPrioIndex]) < 0)
                {
                    Swap(item, items[higherPrioIndex]);
                }
                // Parent has higher priority than both its children
                else
                {
                    return;
                }
            }
            // If has no child
            else
            {
                return;
            }
        }
    }
    
    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
