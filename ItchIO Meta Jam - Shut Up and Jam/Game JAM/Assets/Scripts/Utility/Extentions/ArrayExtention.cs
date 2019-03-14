using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtention
{
    /// <summary>
    /// Applies a passed function to every member of the array.
    /// </summary>
    /// <typeparam name="I">Item type.</typeparam>
    /// <param name="target">Object being extended.</param>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I>(this I[] target, System.Func<I, I> action)
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i] = action.Invoke(target[i]);
        }
    }
}
