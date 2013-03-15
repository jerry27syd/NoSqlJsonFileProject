using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NoSqlJsonFileProject;

namespace WindowsFormsApplicationDemo
{
    [DataContract]
    public class Order : NoSqlJsonFile<Order>
    {
        [DataMember]
        public DateTime OrderedDate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<OrderDetail> OrderDetails { get; set; }

        private int _discountPriceOnQuantity = 10;

        public Order AddProductDetail(Product product, float price, int quantity)
        {
            if (OrderDetails == null)
            {
                OrderDetails = new List<OrderDetail>();
            }
            var orderDetail = new OrderDetail();
            orderDetail.OrderedProduct = product;
            orderDetail.OrderedPrice = price;
            orderDetail.Quantity = quantity;
            OrderDetails.Add(orderDetail);

            // 5 percents off on a product order if the total quantity is greater than 10
            if (quantity > _discountPriceOnQuantity)
            {
                orderDetail.OrderedPrice = orderDetail.OrderedPrice * 0.95f;
            }
            return this;
        }

    }
}