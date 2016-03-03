using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaleoAutomation
{
    class Configurations : IEnumerable
    {
        private Node head = null;
        private Node tail = null;

        public int Count
        {
            get {
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

        public void Add(Configuration conf)
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

    internal class Node
    {
        public Node next = null;
        public object data = null;

        public Node(object conf = null)
        {
            data = conf;
        }
    }
}
