using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NoSqlJsonFileProject;


namespace ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            

            Console.ReadKey();
        }
    }

    [DataContract]
    class MyClass :NoSqlJsonFile<MyClass>
    {
        [DataMember]
        List<NoneTypeTest> NoneTypeTest { get; set; }
    }

    class NoneTypeTest
    {
        
    }
    
}
