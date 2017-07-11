using System;

namespace TwoWayList
{
    public class Node<T> : IDisposable
    {
        private Node<T> next;
        private T data;
        private Node<T> previous;

        public T Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public Node<T> Previous
        {
            get
            {
                return previous;
            }

            set
            {
                previous = value;
            }
        }

        public Node<T> Next
        {
            get
            {
                return next;
            }

            set
            {
                next = value;
            }
        }

        public void Dispose()
        {
            this.previous.next = null;
            this.next.previous = null;
            this.previous = null;
            this.next = null;
        }
    }
    
}
