using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdbManager
{
    class Class1
    {
        static void Main(string[] args)
        {
            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (string s in resourceNames)
            {
                Console.WriteLine(s);
            }
            Console.ReadKey();
        }
    }
}
