using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassForKeyshort
{
    //you can ctrl+J to see all the shortcut actually.
    public class KeyshortForWritting
    {
        //ctor
        public KeyshortForWritting()
        {
            //cw : short for Console.WriteLine();
            Console.WriteLine("good mornign .....");

            //try 
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }


            //tryf
            try
            {

            }
            finally
            {

            }

            //for
            for (int i = 0; i < 10; i++)
            {
                
            }

            //forr
            for (int i = 10 - 1; i >= 0; i--)
            {
                
            }

            //foreach
            var names = new List<string>();
            foreach (var name in names)
            {
                
            }

            //while
            while (true)
            {
                
            }
        }

        //prop+ Tab, then you can use Tab to enter your name and your type .
        public int MyProperty { get; set; }
        public int MyProperty { get; set; }

        //porpfull
        private int myVar;
        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }
        
        
    }
}
