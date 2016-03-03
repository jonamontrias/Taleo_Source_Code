using System;
using System.Collections;

namespace TaleoAutomation
{
    public class Configuration : IEnumerable
    {
        private Node head = null;
        private Node tail = null;

        public int Count
        {
            get
            {
                int i = 0;
                Node current = head;
                while (current != null)
                {
                    i++;
                    current = current.next;
                }
                return i;
            }
        }

        public void Add(string conf)
        {
            if (head == null)
            {
                head = new Node(conf);
                tail = head;
            }
            else
            {
                tail.next = new Node(conf);
                tail = tail.next;
            }
        }

        public IEnumerator GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.data;
                current = current.next;
            }
        }
    }
}