using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace assignment4
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3) // is the arguments as expected?
            {
                string init = args[0];
                MainList thelist = new MainList(init);
                string input;
                while ((input = System.Console.ReadLine()) != "-1")
                {
                    string[] tokens = input.Split(' ');
                    if (tokens[0].Equals("cp"))
                    { // It's a copy function.
                        if (tokens[1].Equals("-f"))
                        { //It's a "COPY FILE" function.
                            if (tokens.Length == 4)
                                thelist.cpfile(tokens[2], tokens[3]);
                        }
                        if (tokens[1].Equals("-d"))
                        {
                            if (tokens.Length == 4)
                                thelist.cpDir(tokens[2], tokens[3]);
                        }
                    }
                    if (tokens[0].Equals("mv"))
                    { //It's a move function.
                        if (tokens[1].Equals("-f"))
                        {
                            if (tokens.Length == 4)
                                thelist.moveFile(tokens[2], tokens[3]);
                        }
                        if (tokens[1].Equals("-d"))
                        {
                            if (tokens.Length == 4)
                                thelist.moveDir(tokens[2], tokens[3]);
                        }
                    }
                    if (tokens[0].Equals("crtfl"))
                    {
                        thelist.crtfile(tokens[1]);
                    }
                    if (tokens[0].Equals("mkdir"))
                    {
                        thelist.makeDir(tokens[1]);
                    }
                    if (tokens[0].Equals("rm"))
                    {
                        if (tokens[1].Equals("-f"))
                            thelist.rmFile(tokens[2]);
                        if (tokens[1].Equals("-d"))
                            thelist.rmDir(tokens[2]);
                    }
                    if (tokens[0].Equals("cd"))
                    {
                        if(tokens.Length == 2)
                            thelist.chdir(tokens[1]);
                    }
                    if (tokens[0].Equals("ls"))
                    {
                        if (tokens.Length == 1)
                            thelist.ls(null);
                        if (tokens.Length == 2)
                            thelist.ls(tokens[1]);
                    }
                }
            }
            else
            {
                try
                {
                    using (StreamReader sr = new StreamReader(args[1]))
                    {
                        string init = args[0];
                        MainList thelist = new MainList(init, args[2]);
                        string ourline;
                        while ((ourline = sr.ReadLine()) != null)
                        {
                            string[] tokens = ourline.Split();
                            if (tokens[0].Equals("cp"))
                            { // It's a copy function.
                                if (tokens[1].Equals("-f"))
                                { //It's a "COPY FILE" function.
                                    if (tokens.Length == 4)
                                        thelist.cpfile(tokens[2], tokens[3]);
                                }
                                if (tokens[1].Equals("-d"))
                                {
                                    if (tokens.Length == 4)
                                        thelist.cpDir(tokens[2], tokens[3]);
                                }
                            }
                            if (tokens[0].Equals("mv"))
                            { //It's a move function.
                                if (tokens[1].Equals("-f"))
                                {
                                    if (tokens.Length == 4)
                                        thelist.moveFile(tokens[2], tokens[3]);
                                }
                                if (tokens[1].Equals("-d"))
                                {
                                    if (tokens.Length == 4)
                                        thelist.moveDir(tokens[2], tokens[3]);
                                }
                            }
                            if (tokens[0].Equals("crtfl"))
                            {
                                thelist.crtfile(tokens[1]);
                            }
                            if (tokens[0].Equals("mkdir"))
                            {
                                thelist.makeDir(tokens[1]);
                            }
                            if (tokens[0].Equals("rm"))
                            {
                                if (tokens[1].Equals("-f"))
                                    thelist.rmFile(tokens[2]);
                                if (tokens[1].Equals("-d"))
                                    thelist.rmDir(tokens[2]);
                            }
                            if (tokens[0].Equals("cd"))
                            {
                                thelist.chdir(tokens[1]);
                            }
                            if (tokens[0].Equals("ls"))
                            {
                                if (tokens.Length == 1)
                                    thelist.ls(null);
                                if (tokens.Length == 2)
                                    thelist.ls(tokens[1]);
                            }
                        }
                        sr.Close();
                        thelist.outputclose();
                    }
                }
                catch (IOException e)
                {
                    System.Console.WriteLine("The file could not be read.");
                    System.Console.WriteLine(e.Message);
                }
            }
        }
    }
}
