using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
/* This class contains our main list object with the . directory and the current directory. */
namespace assignment4
{
    class MainList
    {
        //Variables
        public FSElement maindirectory; // This is a global look at the file system
        public FSElement currdirectory; // This is for using "cd" command.
        public StreamWriter wr;

        //Constructors
        public MainList(string init)
        {
            StringReader sr = new StringReader(init);
            maindirectory = initfunc(null, sr);
            currdirectory = maindirectory;
            ls(null);
        }

        public MainList(string init, string outputtext)
        {
            StringReader sr = new StringReader(init);
            maindirectory = initfunc(null, sr);
            wr = new StreamWriter(outputtext);
            wr.WriteLine("System initialized.");
            currdirectory = maindirectory;
        }

        //Functions
        
        /* This function decides whether the output will be written to a console or output text. */
        public void txtorcon(string input){
            if(wr == null){
                System.Console.WriteLine(input);
            }else{
                wr.WriteLine(input);
            }
        }

        /* Just puts "--------------------" and a line */
        public void splcom()
        {
            txtorcon("");
            txtorcon("--------------------");
        }

        /*This function applies the recursive structure for "LS" command */
        public void listDirectory(FSElement ourDirectory, string spaces)
        {
            Directory element = ourDirectory as Directory;
            while (element.fslist.hasNext())
            {
                FSElement e = element.fslist.next();
                Directory ourElement = e as Directory;
                if (ourElement == null) // It's a file
                {
                    txtorcon(spaces + " |--- " + e.url);
                }
                else
                {
                    txtorcon(spaces + " |---<" + e.url + ">");
                    String spaced = spaces + " |   ";
                    listDirectory(ourElement, spaced);
                    spaced = spaces;
                }
            }
        } 

        /* This is the "LS" function" */
        public void ls(string input){
            if (input == null) // There is no second parameter so we just list the current directory
            {
                txtorcon("");
                txtorcon("List " + currdirectory.fullParent + currdirectory.url);
                txtorcon("<" + currdirectory.url + ">");
                listDirectory(currdirectory, "");
                splcom();
            }
            else
            {
                FSElement ourelement = currdirectory;
                string[] els = input.Split('/');
                for (int i = 0; i < els.Length; i++)
                {
                    if (i == 0 && els[i].Equals(maindirectory.url))
                    {
                        ourelement = maindirectory;
                        continue;
                    }
                    Directory oe = ourelement as Directory;
                    if (oe.fslist.searchDir(els[i]) == null)
                    {
                        txtorcon("");
                        if (els[0].Equals(maindirectory.url))
                            txtorcon("List " + input);
                        else
                            txtorcon("List " + currdirectory.fullParent + currdirectory.url + "/" + input);
                        txtorcon("Error: A directory must exist in the given url.");
                        splcom();
                        return;
                    }
                    else
                    {
                        ourelement = oe.fslist.searchDir(els[i]);
                    }
                }
                txtorcon("");
                if (els[0].Equals(maindirectory.url))
                    txtorcon("List " + input);
                else
                    txtorcon("List " + currdirectory.fullParent + currdirectory.url + "/" + input);
                txtorcon("<" + ourelement.url + ">");
                listDirectory(ourelement, "");
                splcom();
            }
        }
        
        public Directory initfunc(Directory element, StringReader sr)
        {
            char c;
            StringBuilder sb = new StringBuilder();
            while ((c = (char)sr.Read()) != 65535)
            {
                if(c != '(' && c != ')' && c != ','){ // It's a letter or a number continue adding till not.
                    sb.Append(c);
                }
                if (c == '(') // It's a directory
                {
                    string fullParent = "";
                    if (element == null) // First iteration for creating main directory
                    {
                        Directory ourDirectory = new Directory(sb.ToString(), fullParent);
                        sb.Clear();
                        ourDirectory = initfunc(ourDirectory, sr);
                        element = ourDirectory;
                    }
                    else
                    {
                        fullParent = element.fullParent + element.url + "/";
                        Directory ourDirectory = new Directory(sb.ToString(), fullParent);
                        sb.Clear();
                        ourDirectory = initfunc(ourDirectory, sr);
                        element.fslist.addElement(ourDirectory);
                    }

                }
                if (c == ',') // It's a File!
                {
                    string fullparent = "";
                    fullparent = element.fullParent + element.url + "/";
                    if(sb.ToString() != "")
                        element.fslist.addElement(new File(sb.ToString(), fullparent)); // File added.
                    sb.Clear();
                }
                if (c == ')') // It's a file again and execution finishes
                {
                    if (sb.Length == 0) // There is no
                    {
                        return element;
                    }
                    else
                    {
                        string fullparent = element.fullParent + element.url + "/";
                        element.fslist.addElement(new File(sb.ToString(), fullparent));
                        return element;
                    }
                }
            }
            txtorcon("System initialized.");
            return element;
        }

        /* This is the function for "CreateFile" */
        public void crtfile(string fullurl)
        {
            string[] elements = fullurl.Split('/');
            FSElement ourelement = currdirectory;
            for (int i = 0; i < elements.Length - 1; i++)
            {
                if (i == 0 && elements[i] == maindirectory.url)
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(elements[i]) == null)
                {
                    txtorcon("");
                    if(elements[0] == maindirectory.url)
                        txtorcon("Create file " + fullurl);
                    else
                        txtorcon("Create file " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
                    txtorcon("Error : Directory to create the file does not exist.");
                    splcom();
                    return;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(elements[i]);
                }
            }
            string fullparent = ourelement.fullParent + ourelement.url + "/";
            Directory thefinaldir = ourelement as Directory;
            if (thefinaldir.fslist.searchElement(elements[elements.Length - 1]) == null) // Checks if the file already exists.
            {
                FSElement thefile = new File(elements[elements.Length - 1], fullparent);
                thefinaldir.fslist.addElement(thefile);
                txtorcon("");
                txtorcon("Create file " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
                txtorcon("Create file executed successfully.");
                splcom();
            }
            else
            {
                txtorcon("");
                txtorcon("Create file " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
                txtorcon("Error: That file already exists!");
                splcom();
            }
        }

        /*This is the function for "Make Directory" */
        public void makeDir(string fullurl)
        {
            string[] elements = fullurl.Split('/');
            for (int i = 0; i < elements.Length; i++) // Control for the root directory
            {
                if (fullurl.Equals(maindirectory.url))
                {
                    txtorcon("");
                    txtorcon("Error: A file named " + maindirectory.url +"cannot be created as it is reserved for the root directory.");
                    splcom();
                    return;
                }
            }
            FSElement ourelement = currdirectory;
            for (int i = 0; i < elements.Length; i++)
            {
                if (i == 0 && elements[i] == maindirectory.url)
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (i == elements.Length - 1 && oe.fslist.searchDir(elements[i]) != null)
                {
                    txtorcon("");
                    if(elements[0] == maindirectory.url)
                        txtorcon("Make directory " + fullurl);
                    else
                        txtorcon("Make directory " + currdirectory.fullParent + currdirectory.url  + "/" + fullurl);
                    txtorcon("Error: That directory already exists!");
                    splcom();
                    return;
                }
                if (oe.fslist.searchDir(elements[i]) == null)
                {
                    string fullP = ourelement.fullParent + ourelement.url + "/";
                    FSElement crtdir = new Directory(elements[i], oe.ToString() + "/");
                    oe.fslist.addElement(crtdir);
                    ourelement = crtdir;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(elements[i]);
                }
            }
            txtorcon("");
            if (elements[0] == maindirectory.url)
                txtorcon("Make directory " + fullurl);
            else
                txtorcon("Make directory " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
            txtorcon("Make directory executed successfully.");
            splcom();
        }

        public void rmError(string fullurl)
        {
            txtorcon("");
            if (fullurl.Contains(maindirectory.url))
                txtorcon("Remove file " + fullurl);
            else
                txtorcon("Remove file " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
            txtorcon("Error : Given URL is not a file.");
            splcom();
        }

        /* This is the function for removing files. */
        public void rmFile(string fullurl)
        {
            string[] elements = fullurl.Split('/');
            FSElement ourelement = currdirectory;
            for (int i = 0; i < elements.Length - 1; i++) // Go to the selected element
            {
                if (i == 0 && elements[i] == maindirectory.url){ // If url starts with "./" start from main directory
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(elements[i]) == null) // The directory doesn't exist.
                {
                    rmError(fullurl);
                    return;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(elements[i]);
                }
            }
            Directory finaldir = ourelement as Directory;
            FSElement rmfile;
            if ((rmfile = finaldir.fslist.searchFile(elements[elements.Length - 1])) == null)
            {
                rmError(fullurl);
            }
            else
            {
                finaldir.fslist.deleteElement(finaldir.fslist.searchFile(elements[elements.Length - 1]));
                txtorcon("");
                if(elements[0] == maindirectory.url)
                    txtorcon("Remove file " + fullurl);
                else
                    txtorcon("Remove file " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
                txtorcon("Remove file executed successfully.");
                splcom();
            }
        }

        /*This is for the "rm -d" command meaning its for removing directories. */
        public void rmDir(string fullurl)
        {
            if (fullurl.Equals(maindirectory.url))
            {
                txtorcon("");
                txtorcon("Remove directory " + fullurl);
                txtorcon("Error: A directory must exist in the given url.");
                splcom();
                return;

            }
            string[] elements = fullurl.Split('/');
            FSElement ourelement = currdirectory;
            for (int i = 0; i < elements.Length - 1; i++)
            {
                if (i == 0 && elements[i] == maindirectory.url)
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(elements[i]) == null)
                {
                    txtorcon("");
                    if (elements[0].Equals(maindirectory.url))
                        txtorcon("Remove directory " + fullurl);
                    else
                        txtorcon("Remove directory " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
                    txtorcon("Error: A directory must exist in the given url.");
                    splcom();
                    return;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(elements[i]);
                }
            }
            Directory finaldir = ourelement as Directory;
            if (finaldir.fslist.searchDir(elements[elements.Length - 1]) == null)
            {
                txtorcon("");
                if (elements[0].Equals(maindirectory.url))
                    txtorcon("Remove directory " + fullurl);
                else
                    txtorcon("Remove directory " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
                txtorcon("Error: A directory must exist in the given url.");
                splcom();
                return;
            }
            else
            {
                finaldir.fslist.deleteElement(finaldir.fslist.searchDir(elements[elements.Length - 1]));
                txtorcon("");
                if (elements[0].Equals(maindirectory.url))
                    txtorcon("Remove directory " + fullurl);
                else
                    txtorcon("Remove directory " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
                txtorcon("Remove directory executed successfully.");
                splcom();
            }
        }

        /* This is for the "cd" command meaning its for changing directories. */
        public void chdir(string fullurl)
        {
            string[] elements = fullurl.Split('/');
            for (int i = 0; i < elements.Length; i++)
            {
                if (i == 0 && elements[i] == maindirectory.url)
                {
                    currdirectory = maindirectory;
                    continue;
                }
                Directory current = currdirectory as Directory;
                if (current.fslist.searchDir(elements[i]) == null)
                {
                    if (elements[0] == maindirectory.url)
                        txtorcon("Change directory " + fullurl);
                    else
                        txtorcon("Change directory " + currdirectory.fullParent + currdirectory.url + "/" + fullurl);
                    txtorcon("One of the directories doesn't exist!");
                    splcom();
                    return;
                }
                else
                {
                    currdirectory = current.fslist.searchDir(elements[i]);
                }
            }
            txtorcon("");
            txtorcon("Change directory " + currdirectory.fullParent + currdirectory.url);
            txtorcon("Change directory executed successfully.");
            splcom();
        }

        /* This function is for copying files. */
        public void cpfile(string source, string destin)
        {
            string[] sources = source.Split('/');
            string[] destins = destin.Split('/');
            FSElement ourelement = currdirectory;
            for (int i = 0; i < sources.Length - 1; i++)
            {
                if (i == 0 && sources[i] == maindirectory.url)
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(sources[i]) == null)
                {
                    txtorcon("");
                    if(sources[0] == maindirectory.url)
                        txtorcon("Copy file " + source + " " + destin);
                    else
                        txtorcon("Copy file " + currdirectory.fullParent + currdirectory.url + "/" + source + " " + currdirectory.fullParent + currdirectory.url + "/" + destin);
                    txtorcon("Error: A file must exist at the source url");
                    splcom();
                    return;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(sources[i]);
                }
            }
            Directory finaldir = ourelement as Directory;
            if(finaldir.fslist.searchFile(sources[sources.Length - 1]) == null){
                                    txtorcon("");
                    if(sources[0] == maindirectory.url)
                        txtorcon("Copy file " + source + " " + destin);
                    else
                        txtorcon("Copy file " + currdirectory.fullParent + currdirectory.url + "/" + source + " " + currdirectory.fullParent + currdirectory.url + "/" + destin);
                    txtorcon("Error: A file must exist at the source url");
                    splcom();
                    return;
            }
            ourelement = currdirectory;
            for (int i = 0; i < destins.Length - 1; i++)
            {
                if (i == 0 && destins[i].Equals(maindirectory.url))
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(destins[i]) == null)
                {
                    string fullParent = oe.fullParent + oe.url + "/";
                    FSElement ourdir = new Directory(destins[i], fullParent);
                    oe.fslist.addElement(ourdir);
                    ourelement = ourdir;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(destins[i]);
                }
            }
            Directory destindir = ourelement as Directory;
            if (destindir.fslist.searchElement(destins[destins.Length - 1]) == null)
            {
                destindir.fslist.addElement(new File(destins[destins.Length - 1], (destindir.fullParent + destindir.url + "/")));
                txtorcon("");
                if (sources[0] == maindirectory.url)
                    txtorcon("Copy file " + source + " " + destin);
                else
                    txtorcon("Copy file " + currdirectory.fullParent + currdirectory.url + "/" + source + " " + currdirectory.fullParent + currdirectory.url + "/" + destin);
                txtorcon("Copy file executed successfully.");
                splcom();
            }
            else
            {
                txtorcon("");
                if (sources[0] == maindirectory.url)
                    txtorcon("Copy file " + source + " " + destin);
                else
                    txtorcon("Copy file " + currdirectory.fullParent + currdirectory.url + "/" + source + " " + currdirectory.fullParent + currdirectory.url + "/" + destin);
                txtorcon("Error: Element with a same name already exists.");
                splcom();
                return;
            }

        }

        public void recurcp(FSElement finaldir, FSElement tobecopied)
        {
            Directory findir = finaldir as Directory;
            Directory copy = tobecopied as Directory;
            while (findir.fslist.hasNext())
            {
                FSElement ourelement = findir.fslist.next();
                Directory oe = ourelement as Directory;
                if (oe == null) // It's a file
                {
                    string FileP = copy.fullParent + copy.url + "/";
                    copy.fslist.addElement(new File(ourelement.url, FileP));
                }
                else // It's a directory
                {
                    string DirP = copy.fullParent + copy.url + "/";
                    copy.fslist.addElement(new Directory(oe.url, DirP));
                    recurcp(copy.fslist.searchDir(oe.url), ourelement);
                }
            }
        }
        
        public void cpDir(string source, string destin)
        {
            string[] sources = source.Split('/');
            string[] destins = destin.Split('/');
            FSElement ourelement = currdirectory;
            for (int i = 0; i < sources.Length; i++)
            {
                if (i == 0 && sources[i] == maindirectory.url)
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(sources[i]) == null)
                {
                    string outsource;
                    string outdestin;
                    if (sources[0] == maindirectory.url)
                        outsource = source;
                    else
                        outsource = currdirectory.fullParent + currdirectory.url + "/" + source;
                    if (destins[0] == maindirectory.url)
                        outdestin = destin;
                    else
                        outdestin = currdirectory.fullParent + currdirectory.url + "/" + destin;
                    txtorcon("Copy directory " + outsource + " " + outdestin);
                    txtorcon("Error: A directory must exist at the source url");
                    splcom();
                    return;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(sources[i]);
                }
            }
            Directory finaldir = ourelement as Directory; // THIS IS THE "TO BE COPIED" DIRECTORY
            ourelement = currdirectory;
            for (int i = 0; i < destins.Length; i++)
            {
                if(i == 0 && destins[i].Equals(maindirectory.url)){
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(destins[i]) == null)
                {
                    string fullP = ourelement.fullParent + ourelement.url + "/";
                    FSElement ourdir = new Directory(destins[i], fullP);
                    oe.fslist.addElement(ourdir);
                    ourelement = ourdir;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(destins[i]);
                }
            }
            Directory destindir = ourelement as Directory;
            FSElement addeddir = new Directory(finaldir.url, destindir.fullParent + destindir.url + "/");
            destindir.fslist.addElement(addeddir);
            recurcp(finaldir, addeddir);
            txtorcon("");
            string outsourced;
            string outdestind;
            if (sources[0] == maindirectory.url)
                outsourced = source;
            else
                outsourced = currdirectory.fullParent + currdirectory.url + "/" + source;
            if (destins[0] == maindirectory.url)
                outdestind = destin;
            else
                outdestind = currdirectory.fullParent + currdirectory.url + "/" + destin;
            txtorcon("Copy directory " + outsourced + " " + outdestind);
            txtorcon("Copy directory executed successfully.");
            splcom();
        }

        /*This function is for "Move File" */
        public void moveFile(string source, string destin)
        {
            string outsourced;
            string outdestind;
            string[] sources = source.Split('/');
            string[] destins = destin.Split('/');
            FSElement ourelement = currdirectory;
            for (int i = 0; i < sources.Length - 1; i++)
            {
                if (i == 0 && sources[i].Equals(maindirectory.url))
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(sources[i]) == null)
                {
                    txtorcon("");
                    txtorcon("");
                    if (sources[0] == maindirectory.url)
                        outsourced = source;
                    else
                        outsourced = currdirectory.fullParent + currdirectory.url + "/" + source;
                    if (destins[0] == maindirectory.url)
                        outdestind = destin;
                    else
                        outdestind = currdirectory.fullParent + currdirectory.url + "/" + destin;
                    txtorcon("Move file " + outsourced + " " + outdestind);
                    txtorcon("Error:A file must exist at the source url");
                    splcom();
                    return;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(sources[i]);
                }
            }
            Directory finaldir = ourelement as Directory;
            if (finaldir.fslist.searchFile(sources[sources.Length - 1]) == null)
            {
                txtorcon("");
                if (sources[0] == maindirectory.url)
                    outsourced = source;
                else
                    outsourced = currdirectory.fullParent + currdirectory.url + "/" + source;
                if (destins[0] == maindirectory.url)
                    outdestind = destin;
                else
                    outdestind = currdirectory.fullParent + currdirectory.url + "/" + destin;
                txtorcon("Move file " + outsourced + " " + outdestind);
                txtorcon("Error: A file must exist at the source url");
                splcom();
                return;
            }
            finaldir.fslist.deleteElement(finaldir.fslist.searchFile(sources[sources.Length - 1]));
            ourelement = currdirectory;
            for (int i = 0; i < destins.Length - 1; i++)
            {
                if (i == 0 && destins[i] == maindirectory.url)
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(destins[i]) == null)
                {
                    string fullP = ourelement.fullParent + ourelement.url + "/";
                    FSElement ourdir = new Directory(destins[i], fullP);
                    oe.fslist.addElement(ourdir);
                    ourelement = ourdir;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(destins[i]);
                }
            }
            Directory destindir = ourelement as Directory;
            if (destindir.fslist.searchElement(destins[destins.Length - 1]) == null) // Element doesn't exist we can go on
            {
                string fileP = destindir.fullParent + destindir.url + "/";
                destindir.fslist.addElement(new File(destins[destins.Length - 1], fileP));
                txtorcon("");
                if (sources[0] == maindirectory.url)
                    outsourced = source;
                else
                    outsourced = currdirectory.fullParent + currdirectory.url + "/" + source;
                if (destins[0] == maindirectory.url)
                    outdestind = destin;
                else
                    outdestind = currdirectory.fullParent + currdirectory.url + "/" + destin;
                txtorcon("Move file " + outsourced + " " + outdestind);
                txtorcon("Move file executed successfully.");
                splcom();
                return;
            }
            else
            {
                txtorcon("");
                if (sources[0] == maindirectory.url)
                    txtorcon("Move File " + source + " " + destin);
                else
                    txtorcon("Move File " + currdirectory.fullParent + currdirectory.url + "/" + source + " " + currdirectory.fullParent + currdirectory.url + "/" + destin);
                txtorcon("Error: Element with a same name already exists.");
                splcom();
                return;
            }
        }

        /*This function is for "Move Directory" */
        public void moveDir(string source, string destin)
        {
            if (source.Equals(maindirectory.url + "/"))
            {
                txtorcon("");
                txtorcon("Move directory " + source + " " + destin);
                txtorcon("Error: Element with a same name already exists.");
                splcom();
                return;
            }
            string outsourced;
            string outdestind;
            string[] sources = source.Split('/');
            string[] destins = destin.Split('/');
            FSElement ourelement = currdirectory;
            for (int i = 0; i < sources.Length - 1; i++)
            {
                if (i == 0 && sources[i].Equals(maindirectory.url))
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchElement(sources[i]) == null)
                {
                    txtorcon("");
                    if (sources[0] == maindirectory.url)
                        outsourced = source;
                    else
                        outsourced = currdirectory.fullParent + currdirectory.url + "/" + source;
                    if (destins[0] == maindirectory.url)
                        outdestind = destin;
                    else
                        outdestind = currdirectory.fullParent + currdirectory.url + "/" + destin;
                    txtorcon("Move directory " + outsourced + " " + outdestind);
                    txtorcon("Error: A directory must exist at the source url");
                    splcom();
                    return;
                }
                else
                {
                    ourelement = oe.fslist.searchElement(sources[i]);
                }
            }
            Directory finaldir = ourelement as Directory;
            FSElement ourdir;
            if(finaldir.fslist.searchDir(sources[sources.Length - 1]) == null){
                txtorcon("");
                if (sources[0] == maindirectory.url)
                    outsourced = source;
                else
                    outsourced = currdirectory.fullParent + currdirectory.url + "/" + source;
                if (destins[0] == maindirectory.url)
                    outdestind = destin;
                else
                    outdestind = currdirectory.fullParent + currdirectory.url + "/" + destin;
                txtorcon("Move directory " + outsourced + " " + outdestind);
                txtorcon("Error: A directory must exist at the source url");
                splcom();
                return;
            }else{
                ourdir = finaldir.fslist.searchDir(sources[sources.Length - 1]);
                finaldir.fslist.deleteElement(finaldir.fslist.searchDir(sources[sources.Length - 1]));
            }
            ourelement = currdirectory;
            for (int i = 0; i < destins.Length; i++)
            {
                if (i == 0 && destins[i].Equals(maindirectory.url))
                {
                    ourelement = maindirectory;
                    continue;
                }
                Directory oe = ourelement as Directory;
                if (oe.fslist.searchDir(destins[i]) == null)
                {
                    string fullP = ourelement.fullParent + ourelement.url + "/";
                    FSElement crtdir = new Directory(destins[i], fullP);
                    oe.fslist.addElement(crtdir);
                    ourelement = crtdir;
                }
                else
                {
                    ourelement = oe.fslist.searchDir(destins[i]);
                }
            }
            Directory findir = ourelement as Directory;
            if (findir.fslist.searchDir(ourdir.url) == null)
            {
                ourdir.fullParent = ourelement.fullParent + ourelement.url + "/";
                findir.fslist.addElement(ourdir);
                txtorcon("");
                if (sources[0] == maindirectory.url)
                    outsourced = source;
                else
                    outsourced = currdirectory.fullParent + currdirectory.url + "/" + source;
                if (destins[0] == maindirectory.url)
                    outdestind = destin;
                else
                    outdestind = currdirectory.fullParent + currdirectory.url + "/" + destin;
                txtorcon("Move directory " + outsourced + " " + outdestind);
                txtorcon("Move directory executed successfully.");
                splcom();
            }
            else
            {
                txtorcon("");
                if (sources[0] == maindirectory.url)
                    outsourced = source;
                else
                    outsourced = currdirectory.fullParent + currdirectory.url + "/" + source;
                if (destins[0] == maindirectory.url)
                    outdestind = destin;
                else
                    outdestind = currdirectory.fullParent + currdirectory.url + "/" + destin;
                txtorcon("Move directory " + outsourced + " " + outdestind);
                txtorcon("Error: Element with a same name already exists.");
                splcom();
                return;
            }
        }

        public void outputclose()
        {
            wr.Close();
        }

    }
}
