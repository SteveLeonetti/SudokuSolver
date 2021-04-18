using System.Collections.Generic;

/// <author>Steve Leonetti</author>
/// <summary>
/// Stores any number of unique items and holds their order.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class OrderedSet<T> : List<T>
{
    /// <summary>
    /// Adds item to the List, but doesn't allow multiple copies to be inserted.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public new bool Add(T item)
    {
        if (Contains(item))
            return false;

        base.Add(item);

        return true;
    }

    public bool Equals(OrderedSet<T> secondary)
    {
        // Check if both empty sets
        if (this.Count == 0 && secondary.Count == 0)
            return true;

        // Check if counts don't match
        if (this.Count != secondary.Count)
            return false;
        
        // Check if contents are the same
        for (int index = 0; index < this.Count; index++)
        {
            if (!this[index].Equals(secondary[index]))
                return false;
        }
        
        // There were not any differences, thus return true that the sets are the same
        return true;
    }
}