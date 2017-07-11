using System;
using System.Collections;
using System.Collections.Generic;

namespace TwoWayList
{
    class TwoWayList<T> : IEnumerable<T>, ICollection<T>
    {
        private int counter;
        private bool readOnly;
        private delegate void Added();
        private delegate void Removed();
        private delegate void Cleared();
        private event Added onAdd;
        private event Removed onRemove;
        private event Cleared onClear;

        private Node<T> head;
        private Node<T> tail;

        private Node<T> LastNode()
        {
            Node<T> cur = head;
            while (cur.Next != null)
            {
                cur = cur.Next;
            }
            return cur;
        }

        /// <summary>
        /// Get Node object at specified position.
        /// Index starting from 0.
        /// </summary>
        private Node<T> NodeAt(int pos) 
        {
            if (pos > counter + 1)
            {
                throw new IndexOutOfRangeException();
            }

            if (pos == this.counter + 1)
            {
                return this.tail;
            }

            if (pos == 0)
            {
                return this.head;
            }

            if (counter - pos > counter / 2)
            {
                Node<T> cur = tail;
                pos = counter - 1 - pos;
                while (pos != 0)
                {
                    cur = cur.Previous;
                    pos--;
                }
                return cur;
            }
            else
            {
                Node<T> cur = head;
                while (pos != 0)
                {
                    cur = cur.Next;
                    pos--;
                }
                return cur;
            }
        }


        public void Insert(T item, int pos) //before the position
        {
            if (pos > counter + 1)
            {
                throw new IndexOutOfRangeException();
            }

            if (counter == 1)
            {
                Node<T> one = new Node<T>();
                one.Data = item;
                one.Previous = null;
                one.Next = this.head;
                this.head = one;
                onAdd();
                return;
            }

            if (pos == counter + 1) //as Add method
            {
                this.Add(item);
            }

            if (pos == counter) //before last
            {
                Node<T> one = new Node<T>();
                one.Data = item;
                var lastNode = this.LastNode();
                one.Previous = lastNode.Previous;
                one.Next = lastNode;
                one.Previous.Next = one;
                lastNode.Previous = one;
                onAdd();
                return;
            }
            else
            {
                Node<T> one = new Node<T>();
                one.Data = item;
                Node<T> onPos = NodeAt(pos);
                one.Next = onPos;
                one.Previous = onPos.Previous;
                onPos.Previous = one;
                one.Previous.Next = one;
                onAdd();
                return;
            }

        }

        public T First()
        {
            return head.Data;
        }

        public T Last()
        {
            return tail.Data;
        }

        public int Count
        {
            get
            {
                return counter;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return readOnly;
            }
        }

        public void Add(T item)
        {
            if (counter == 0)
            {
                Node<T> two = new Node<T>();
                two.Data = item;
                two.Previous = null;
                two.Next = null;
                this.head = two;
                counter++;
                return;
            }

            Node<T> one = new Node<T>();
            one.Data = item;
            one.Previous = tail;
            tail = one;
            one.Previous.Next = one;
            one.Next = null;
            counter++;
            onAdd();
        }

        public void Clear()
        {
            if (counter == 0)
                return;
            head.Dispose();
            if (counter == 1)
                return;
            tail.Dispose();
            onClear();
        }

        public bool Contains(T item)
        {
            Node<T> cur = head;
            while (cur.Next != null)
            {
                if (cur.Data.Equals(item))
                    return true;
                cur = cur.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array[arrayIndex] = this.NodeAt(arrayIndex).Data;
        }

        public T[] CopyToArray()
        {
            T[] array = new T[counter];
            Node<T> node = head;
            for (int i = 0; i < counter; i++)
            {
                array[i] = node.Data;
                node = node.Next;
            }
            return array;
        }


        public bool Remove(T item)
        {
            Node<T> cur = head;
            while (cur.Next != null)
            {
                if (cur.Data.Equals(item))
                {
                    cur.Dispose();
                    onRemove();
                    return true;
                }
                cur = cur.Next;
            }
            onRemove();
            return false;
        }


        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)new TwoWayEnum<T>(this.CopyToArray());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }

    public class TwoWayEnum<T> : IEnumerator
    {
        public T[] _nodesData;
        int position = -1;

        public TwoWayEnum(T[] list)
        {
            _nodesData = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _nodesData.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public T Current
        {
            get
            {
                try
                {
                    return _nodesData[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    public class TwoWayNodeEnum<T> : IEnumerator
    {
        public Node<T>[] _nodes;
        int position = -1;

        public TwoWayNodeEnum(Node<T>[] list)
        {
            _nodes = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _nodes.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Node<T> Current
        {
            get
            {
                try
                {
                    return _nodes[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
