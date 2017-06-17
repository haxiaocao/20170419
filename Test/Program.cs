using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void PrintType<T>(T first,T second)
        {
            Console.WriteLine(typeof(T));
        }
        static void Main(string[] args)
        {
            //PrintType(1, new object());
            //PrintType<object>(1,new object());

            //PrintType(1,2);
            //PrintType<int>(1,2);

           

            
           Console.ReadLine();
        }

        
       

        
       
    }   
}
