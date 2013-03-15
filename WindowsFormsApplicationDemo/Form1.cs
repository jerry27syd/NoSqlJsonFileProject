using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplicationDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Generated - Form1_Script.ccc
           

            dataGridView1.EndEdit();
            dataGridView2.EndEdit();
            dataGridView3.EndEdit();
            dataGridView4.EndEdit();
            customerBindingSource.DataSource = Customer.List();
            productBindingSource.DataSource = Product.List();
            orderDetailBindingSource.DataSource = OrderDetail.List();
            orderBindingSource.DataSource = Order.List();

            dataGridView1.CellEndEdit += delegate
            {
                var currentRow = dataGridView1.CurrentRow;
                if (currentRow == null) return;
                var customer = currentRow.DataBoundItem as Customer;
                if (customer != null)
                {
                    customer.Save();
                }
            };

            dataGridView2.CellEndEdit += delegate
            {
                var currentRow = dataGridView2.CurrentRow;
                if (currentRow == null) return;
                var product = currentRow.DataBoundItem as Product;
                if (product != null)
                {
                    product.Save();
                }
            };

            dataGridView3.CellEndEdit += delegate
            {
                var currentRow = dataGridView3.CurrentRow;
                if (currentRow == null) return;
                var orderDetail = currentRow.DataBoundItem as OrderDetail;
                if (orderDetail != null)
                {
                    orderDetail.Save();
                }
            };

            dataGridView4.CellEndEdit += delegate
            {
                var currentRow = dataGridView4.CurrentRow;
                if (currentRow == null) return;
                var order = currentRow.DataBoundItem as Order;
                if (order != null)
                {
                    order.Save();
                }
            };
            #endregion
            
        }

        private void purchaseButton_Click(object sender, EventArgs e)
        {
        }


    }
}
