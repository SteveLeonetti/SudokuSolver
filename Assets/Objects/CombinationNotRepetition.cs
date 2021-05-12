/// <author>Steve Leonetti</author>
/// <summary>
/// Handles iteration through a set to find all combinations possible in the set.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class CombinationNoRepetition<T> : Combination<T>
{
    /// <summary>
    /// Constructs the fully functioning Permutation set.
    /// </summary>
    /// <param name="_set"></param>
    /// <param name="_subSetSize"></param>
    public CombinationNoRepetition(OrderedSet<T> _set, int _subSetSize) : base(_set, _subSetSize)
    {
    }

    /// <summary>
    /// Gets the next iteration of the Combination set.
    /// </summary>
    /// <returns></returns>
    public OrderedSet<T> IterateNext()
    {
        if (set.Count == 0)
            return null;

        iteration++;
        #region StarterSet
        if (iteration == 0)
        {
            reset();

            return loadSubSet();
        }
        #endregion

        #region Search
        // Find the FURTHEST right 'true' space in the 'mask' array that can still move right.
        int numTruePassed = 0;
        int a = mask.Length - 1;
        if (mask[a])
        {
            while (a >= 0 && mask[a])
            {
                a--;
                numTruePassed++;
            }
        }

        while (a >= 0 && !mask[a])
            a--;
        #endregion

        #region IsFinished
        // Checks if there weren't any 'true' spaces that could move
        if (a < 0 || a == 0 && !mask[a] || a >= mask.Length - 1)
        {
            iteration = -1;
            reset();
            return null;    // List is fully iterated.
        }
        #endregion

        #region Movement
        // Move 'true' one space to the right.
        mask[a] = false;
        mask[++a] = true;

        for (; numTruePassed > 0; numTruePassed--)
            mask[++a] = true;

        while (a < mask.Length - 1)
            mask[++a] = false;
        #endregion

        return loadSubSet();
    }

    /// <summary>
    /// Resets the combination to iteration 0.
    /// </summary>
    private void reset()
    {
        mask = new bool[set.Count];

        for (int i = 0; i < subSetSize; i++)
            mask[i] = true;
    }
}