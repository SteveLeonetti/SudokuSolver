using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SudokuSet : OrderedSet<SNode>
{
    public int Index;
    public bool IsSolved = false;
    public OrderedSet<int> SolvedValues = new OrderedSet<int>();
    public OrderedSet<int> UnsolvedValues = new OrderedSet<int>();

    public SudokuSet(int _index)
    {
        Index = _index;
    }

    public void ValueSolved(int value)
    {
        UnsolvedValues.Remove(value);
        SolvedValues.Add(value);
        if (UnsolvedValues.Count == 0)
            IsSolved = true;
    }

    public SudokuSet Trim()
    {
        SudokuSet newSet = new SudokuSet(Index);

        foreach (SNode sNode in this)
        {
            if (!sNode.IsSolved)
                newSet.Add(sNode);
        }

        return newSet;
    }
}