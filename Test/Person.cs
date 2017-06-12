using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Person
    {
        public int Age { get;private set; }
        public string Name { get;private set; }
        public static readonly string TO_STRING = "OK,101";

        List<Person> friends = new List<Person>();
        public List<Person> Friends
        {
            get { return friends; }
        }

        //Location home = new Location();
        //public Location Home
        //{
        //    get { return home; }
        //}

        public Person()
        {
            Console.WriteLine("No argument constructor:");
            Console.WriteLine("readonly tosting:"+TO_STRING);
        }

        public Person(string name)
        {
            Name = name;
        }

        public Person(string name,int age)
        {
            Name = name;
            Age = age;
            Console.WriteLine("readonly tostring :"+TO_STRING);
        }
    }
}
