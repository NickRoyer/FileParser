using System;
using FileParser.Tests;


namespace FileParser
{
    class Program
    {               
        static void Main(string[] args)
        {
            Console.WriteLine("Test processing large data file");
            //new FileParserTest().RunTest(); // skip file load time comparisons
            new MovieRepositoryTest().TestRepositories();
        }

        
    }
}
