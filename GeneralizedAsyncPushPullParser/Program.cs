using System;

namespace GeneralizedAsyncPushPullParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            parser.Token += (token) =>
            {
                Console.WriteLine($"token: “{token}”");
            };
            parser.Push("hi");
            parser.Push(" ");
            parser.Push("there ");
            parser.Push("man");
            parser.Push(null);
        }
    }
}
