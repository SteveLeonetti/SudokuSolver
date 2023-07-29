using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Objects
{
    public class StackSet<T> : Stack<T>
    {
        public T Pop(T item)
        {
            Pop();
            return item;
        }

        public bool PushOverride(T item)
        {
            if (!Contains(item))
            {
                Push(item);
                return true;
            }
            else
                return false;
        }
    }
}
