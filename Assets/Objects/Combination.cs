/// <author>Steve Leonetti</author>
/// <summary>
/// Parent class to all Combination subclasses.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract partial class Combination<T>
{
    #region Fields
    public OrderedSet<T> set;
    protected bool[] mask;
    public int subSetSize;
    public int iteration = -1;
    #endregion

    /// <summary>
    /// Constructs the fully functioning Permutation set.
    /// </summary>
    /// <param name="_set"></param>
    /// <param name="_subSetSize"></param>
    public Combination(OrderedSet<T> _set, int _subSetSize)
    {
        set = _set;
        mask = new bool[set.Count];
        subSetSize = _subSetSize;
    }

    /// <summary>
    /// Loads the SubSet with the proper items in the big set.
    /// </summary>
    /// <returns></returns>
    protected OrderedSet<T> loadSubSet()
    {
        OrderedSet<T> subSet = new OrderedSet<T>();

        for (int i = 0; i < mask.Length; i++)
        {
            if (mask[i])
                subSet.Add(set[i]);
        }

        return subSet;
    }
}