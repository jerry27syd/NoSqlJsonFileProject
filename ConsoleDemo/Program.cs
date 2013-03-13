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

            List<Customer> customers = Customer.List();
            foreach (Customer c in customers)
            {
                Console.WriteLine(c.FirstName + " " + c.LastName);
            }


            Customer customer = new Customer();
            customer.FirstName = "Anna";
            customer.LastName = "Lee";
            customer.Save();


            customer = Customer.Get("");

            Console.Read();
            Console.ReadKey();
        }
    }


    [DataContract]
    class Customer : NoSqlJsonFile<Customer>
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public List<Product> Products { get; set; }
    }

    [DataContract]
    class Product : NoSqlJsonFile<Product>
    {
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
