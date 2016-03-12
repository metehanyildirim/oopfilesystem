using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment4
{
    class Directory: FSElement
    {
        public string url { get; set; }
        public string fullParent { get; set; }
        public LinkedList.LinkedList fslist = new LinkedList.LinkedList();

        // Constructors
        public Directory(string _url, string _fullParent)
        {
            url = _url;
            fullParent = _fullParent;
        }

        public Directory()
        {
            url = null;
            fullParent = null;
        }

        // Functions
        public override bool Equals(object obj)
        {
            if (obj == null) // is it a null pointer?
                return false;
            FSElement element = obj as Directory;
            if (element == null) // Not the type we want
            {
                return false;
            }
            else
            {
                if (element.url == url && element.fullParent == fullParent)
                    return true;
                else
                    return false;
            }
        }
        
        public override string ToString()
        {
            return fullParent + url;
        }
        
    }
}
