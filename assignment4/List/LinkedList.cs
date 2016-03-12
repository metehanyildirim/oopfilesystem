using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* This is our LinkedList it is self-explanatory */
namespace assignment4.LinkedList
{
    class LinkedList
    {
        private Node head { get; set; }
        private Node iteration;

        //Constructor
        public LinkedList()
        {
            head = null;
            iteration = null;
        }

        public void ittohead()
        {
            iteration = head;
        }

        //Methods
        public bool isEmpty() // Is the list empty?
        {
            if (head == null)
                return true;
            else
                return false;
        }

        public void addElement(FSElement element){
            if (isEmpty()) // The list is empty
            {
                Node temp = new Node(element, null);
                head = temp;
                iteration = head;
            }
            else // The list is not empty
            {
                Node temp = head;
                Node ourNode = new Node(element, null);
                while (temp.getNext() != null)
                {
                    temp = temp.getNext();
                }
                temp.setNext(ourNode);
            }
        }

        public void deleteElement(FSElement element)
        {
            Node temp = head.getNext(); // Element right after head
            Node prev = head;
            if (!isEmpty())
            {
                if (element.Equals(head.getData())) // It's the first element
                {
                    head = head.getNext();
                    iteration = head;
                }
                else
                {
                    while (temp != null)
                    {
                        if (element.Equals(temp.getData())) // Is it equal?
                        {
                            prev.setNext(temp.getNext());
                            return;
                        }
                        prev = temp;
                        temp = temp.getNext();
                    }
                }
            }
        }

        public FSElement searchElement(string input)
        {
            Node temp = head;
            while (temp != null)
            {
                if (temp.getData().url.Equals(input))
                {
                    return temp.getData();
                }
                temp = temp.getNext();
            }
            return null;
        }

        public FSElement searchDir(string input)
        {
            Node temp = head;
            while (temp != null)
            {
                if (temp.getData().url.Equals(input))
                {
                    Directory oe = temp.getData() as Directory;
                    if (oe != null)
                        return temp.getData();
                }
                temp = temp.getNext();
            }
            return null;
        }

        public FSElement searchFile(string input)
        {
            Node temp = head;
            while (temp != null)
            {
                if (temp.getData().url.Equals(input))
                {
                    File oe = temp.getData() as File;
                    if (oe != null)
                        return temp.getData();
                }
                temp = temp.getNext();
            }
            return null;
        }

        public bool hasNext()
        {
            if (!isEmpty())
            {
                if (iteration == null)
                {
                    iteration = head;
                    return false;
                }
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        public FSElement next()
        {
            Node temp = iteration;
            iteration = iteration.getNext();
            return temp.getData();
        }

    }
}
