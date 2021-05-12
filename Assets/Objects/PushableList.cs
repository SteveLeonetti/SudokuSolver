using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Objects
{
    public class PushableList<T> : List<T>
    {
        public T Pop(T item)
        {
            Remove(item);
            return item;
        }

        public bool Push(T item)
        {
            if (!Contains(item))
            {
                Add(item);
                return true;
            }
            else
                return false;
        }
    }
}
