using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*This class is the struct{ FSelement data; Node next; } equilavent of C# */
namespace assignment4.LinkedList
{
    class Node
    {
        private FSElement data; // I failed to understand { get; set; } so Im doing this the java way.
        private Node next;

        //Constructors
        public Node()
        {
            data = null;
            next = null;
        }
        public Node(FSElement _data, Node _next)
        {
            data = _data;
            next = _next;
        }

        //Getters and Setters
        public FSElement getData()
        {
            return data;
        }

        public void setData(FSElement _data)
        {
            data = _data;
        }

        public Node getNext()
        {
            return next;
        }

        public void setNext(Node _next)
        {
            next = _next;
        }
    }
}
