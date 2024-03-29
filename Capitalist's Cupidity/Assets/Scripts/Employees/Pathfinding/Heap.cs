﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{

    T[] items;
    int currentItemCount;

	public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        sortUp(item);
        currentItemCount++;
    }

    public T removeFirst()
    {
        T firstItem = items[0];
        currentItemCount--;

        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        sortDown(items[0]);

        return firstItem;
    }

    public bool contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    public void updateItem(T item)
    {
        sortUp(item);
    }

    public int count
    {
        get
        {
            return currentItemCount;
        }
    }

    void sortDown(T item)
    {
        while(true)
        {
            int childIndexLeft = item.heapIndex * 2 + 1;
            int childIndexRight = item.heapIndex * 2 + 2;

            int swapIndex = 0;

            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if(childIndexRight < currentItemCount)
                {
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void sortUp(T item)
    {
        int parentIndex = (item.heapIndex - 1) / 2;

        while(true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
            {
                swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.heapIndex - 1) / 2;
        }
    }
    void swap(T A, T B)
    {
        items[A.heapIndex] = B;
        items[B.heapIndex] = A;

        int itemAIndex = A.heapIndex;
        A.heapIndex = B.heapIndex;
        B.heapIndex = itemAIndex;
    }
        
}

public interface IHeapItem<T> : IComparable<T>
{
    int heapIndex
    {
        get;
        set;
    }
}
