using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NoSqlJsonFileProject;

namespace WindowsFormsApplicationDemo
{
    [DataContract]
    public class Customer : NoSqlJsonFile<Customer>
    {
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public bool ActiveMember { get; set; }

        [DataMember]
        public List<Order> Orders { get; set; }

        public bool ProcessEmailSuccessful { get; set; }

        public Order CreateNewOrder(string description = "")
        {
            if (Orders == null)
            {
                Orders = new List<Order>();
            }
            var order = new Order();
            order.OrderedDate = DateTime.Now;
            order.Description = description;
            Orders.Add(order);
            return order;
        }
    }
}
