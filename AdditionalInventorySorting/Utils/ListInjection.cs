using System;
using System.Collections.Generic;
using System.Linq;

namespace AdditionalInventorySorting.Utils;

public static class ListInjection
{
    public static bool TryInsertAfter<T>(List<T> list, Func<T, T, bool> comparer, T afterWhat, params T[] itemsToAdd)
    {
        if (itemsToAdd.Length == 0)
        {
            return false;
        }

        T firstItemToAdd = itemsToAdd.First();

        int afterWhatIndex = -1;

        // Check list in reverse since we add items after, and we want to check if aleady added
        for (int i = list.Count - 1; i >= 0; i--)
        {
            T item = list[i];
            if (comparer(item, firstItemToAdd))
            {
                // Already patched
                return false;
            }

            if (comparer(item, afterWhat))
            {
                afterWhatIndex = i;
                break;
            }
        }

        if (afterWhatIndex == -1)
        {
            return false;
        }

        foreach (T item in itemsToAdd.Reverse())
        {
            list.Insert(afterWhatIndex + 1, item);
        }

        return true;
    }
}
