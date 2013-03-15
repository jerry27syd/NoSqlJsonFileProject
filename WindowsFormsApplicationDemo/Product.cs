using System.Runtime.Serialization;
using NoSqlJsonFileProject;

namespace WindowsFormsApplicationDemo
{
    [DataContract]
    public class Product : NoSqlJsonFile<Product>
    {
        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public float UnitPrice { get; set; }
    }
}