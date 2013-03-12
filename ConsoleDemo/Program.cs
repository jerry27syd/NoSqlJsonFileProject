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

            var list = Employee.Enumerable();
            foreach (var employee in list)
            {
                foreach (var date in employee.Dates)
                {
                    Console.WriteLine(date);
                }
            }

            Console.Read();

            Employee e = new Employee();
            e.FirstName = "Hello" + new Random().Next(100);
            e.LastName = "World";
            e.Dates = new List<DateTime>();
            e.Dates.Add(DateTime.Now.AddDays(1d));
            e.Dates.Add(DateTime.Now.AddDays(4d));
            e.Dates.Add(DateTime.Now.AddDays(5d));
            e.Save();

            Console.ReadKey();
        }
    }


    [DataContract]
    class Employee : NoSqlJsonFile<Employee>
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public List<DateTime> Dates { get; set; }

    }
}
