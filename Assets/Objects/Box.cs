using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Box : SudokuSet
{
    public List<Row> linkedRows;
    public List<Column> linkedColumns;

    public Box(int _index) : base(_index)
    {
        linkedRows = new List<Row>();
        linkedColumns = new List<Column>();
    }

    public void AddRow(Row r)
    {
        linkedRows.Add(r);
    }

    public void AddColumn(Column c)
    {
        linkedColumns.Add(c);
    }
}