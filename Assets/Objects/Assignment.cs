namespace Assets.Objects
{
    public class Assignment
    {
        public SNode SNode { get; set; }
        public int Value { get; set; }

        public Assignment(SNode _s, int _v)
        {
            SNode = _s;
            Value = _v;
        }

        public Assignment()
        {
            SNode = null;
            Value = 0;
        }
    }
}