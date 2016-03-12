using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment4
{
    class File: FSElement
    {
        public string url { get; set; }
        public string fullParent { get; set; }

        //Constructors
        public File(string _url, string _fullParent)
        {
            url = _url;
            fullParent = _fullParent;
        }
        public File()
        {
            url = null;
            fullParent = null;
        }

        //Functions
        public override bool Equals(object obj)
        {
            if (obj == null) // It's a null pointer.
                return false;
            FSElement element = obj as File; // Casting
            if (element == null)
                return false;
            else
            {
                if (url == element.url && fullParent == element.fullParent)
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
