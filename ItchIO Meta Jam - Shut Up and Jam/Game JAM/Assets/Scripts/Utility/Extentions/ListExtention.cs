using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtention
{
    /// <summary>
    /// Adds an item to the list, but only if that item doesn't already exists.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="item">Item being added.</param>
    public static void AddOnce<I>(this List<I> target, I item)
    {
        if (!target.Contains(item))
        {
            target.Add(item);
        }
    }

    /// <summary>
    /// Adds a collection of items to the list, but only if those items don't already exist.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="items">Item set being added.</param>
    public static void AddRangeOnce<I>(this List<I> target, IEnumerable<I> items)
    {
        foreach(I i in items)
        {
            if (!target.Contains(i))
            {
                target.Add(i);
            }
        }
    }

    /// <summary>
    /// Removes any duplicates inside a list.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="keepOrder">If you want to keep the old list's item order.</param>
    /// <returns>Modified object.</returns>
    public static List<I> RemoveDuplicates<I>(this List<I> target)
    {
        List<I> result = new List<I>();

        foreach (I item in target)
        {
            if (!result.Contains(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    /// <summary>
    /// Extracts any duplicates inside a list.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <returns>List containing any duplicates (but without any duplicate items).</returns>
    public static List<I> ExtractDuplicates<I>(this List<I> target)
    {
        Dictionary<I, bool> dupes = new Dictionary<I, bool>();

        foreach(I i in target)
        {
            if (dupes.ContainsKey(i))
            {
                dupes[i] = true;
            }
            else
            {
                dupes.Add(i, false);
            }
        }

        List<I> result = new List<I>();

        foreach(KeyValuePair<I, bool> kv in dupes)
        {
            if (kv.Value)
            {
                result.Add(kv.Key);
            }
        }

        return result;
    }

    /// <summary>
    /// Extracts duplicates inside a list. However, the item must be found over [dupeCount] times before being extracted.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="dupeCount">How many times the item needs to be found before being extracted.</param>
    /// <returns>List containing duplicates (but without any duplicate items).</returns>
    public static List<I> ExtractDuplicates<I>(this List<I> target, int dupeCount)
    {
        Dictionary<I, int> dupes = new Dictionary<I, int>();

        foreach (I i in target)
        {
            if (dupes.ContainsKey(i))
            {
                dupes[i]++;
            }
            else
            {
                dupes.Add(i, 1);
            }
        }

        List<I> result = new List<I>();

        foreach (KeyValuePair<I, int> kv in dupes)
        {
            if (kv.Value >= dupeCount)
            {
                result.Add(kv.Key);
            }
        }

        return result;
    }

    /// <summary>
    /// Extention of the Sort function. Takes a simple bool function in a similar fashion to Linq's OrderBy function. If the function returns true, it'll get placed before.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="sortBy">Sorting function. Will usually be written in the following style : (item1, item2) => comparison</param>
    /// <returns>Sorted list.</returns>
    public static List<I> Sort<I>(this List<I> target, System.Func<I, I, bool> sortBy)
    {
        List<I> result = new List<I>();

        for (int x = 0; x < target.Count; x++)
        {
            if (x == 0)
            {
                result.Add(target[x]);
                continue;
            }

            bool exit = false;
            for(int y = 0; y < result.Count; y++)
            {
                if (sortBy(target[x], result[y]))
                {
                    result.Insert(y, target[x]);
                    exit = true;
                    break;
                }
            }

            if (!exit)
            {
                result.Add(target[x]);
            }
        }

        return result;
    }

    /// <summary>
    /// Replace all items in List into given item.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="item">Replacement item.</param>
    public static void Populate<I>(this List<I> target, I item)
    {
        for (int i = 0; i < target.Count; i++)
        {
            target[i] = item;
        }
    }

    /// <summary>
    /// Replace all items in List into given item and expands list to reach given count if given count is larger.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="item">Replacement item.</param>
    /// <param name="count">How many items the List must contain.</param>
    public static void Populate<I>(this List<I> target, I item, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (i < target.Count)
            {
                target[i] = item;
            }
            else
            {
                target.Add(item);
            }
        }
    }

    /// <summary>
    /// Applies a passed function to every member of the list.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I>(this List<I> target, System.Func<I, I> action)
    {
        for(int i = 0; i < target.Count; i++)
        {
            target[i] = action.Invoke(target[i]);
        }
    }
}
