using System;
using System.IO;
using SecretSantaApp.Business;

namespace SecretSantaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"./SecretSanta.json"; 
            
            var json = new JsonFileLoader().GetDataFromFile(path);
            Console.WriteLine(json);
            Console.ReadLine();
        }
    }
}
