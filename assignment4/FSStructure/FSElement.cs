using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*This is an interface for our File System. It will be inherited by both Directory.cs and File.cs */
namespace assignment4
{
    interface FSElement
    {
        string url { get; set; } // the directory or file name
        string fullParent { get; set; } // full address of director or file excluding itself
        bool Equals(object obj);
    }
}
