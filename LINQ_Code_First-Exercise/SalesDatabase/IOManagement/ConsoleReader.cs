using SalesDatabase.IOManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesDatabase.IOManagement
{
    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
