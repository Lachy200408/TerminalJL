using System;
namespace AyudaParaCode
{
    public class Program
    {
        static void Main(string[] args)
        {
            ClaseAyudas F = new ClaseAyudas();
            string s= Console.ReadLine();
            Queue<string> queue = F.GetQueue(s);
            while (queue.Count > 0)
            Console.WriteLine(queue.Dequeue());
        }
    }
}
