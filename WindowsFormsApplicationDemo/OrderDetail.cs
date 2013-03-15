using System.Runtime.Serialization;
using NoSqlJsonFileProject;

namespace WindowsFormsApplicationDemo
{
    [DataContract]
    public class OrderDetail : NoSqlJsonFile<OrderDetail>
    {
        [DataMember]
        public Product OrderedProduct { get; set; }

        [DataMember]
        public float OrderedPrice { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}