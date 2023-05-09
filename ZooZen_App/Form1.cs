using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hotcakes.CommerceDTO;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1.Contacts;
using Hotcakes.CommerceDTO.v1.Orders;
using Newtonsoft.Json;


namespace ZooZen_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetProducts();
        }


        public void GetProducts()
        {
            string url = "http://20.234.113.211:8096/";
            string key = "1-4e562905-01dc-461b-b352-328f936981da";

            Api proxy = new Api(url, key);

            // call the API to find all orders in the store
            ApiResponse<List<ProductDTO>> response = proxy.ProductsFindAll();
            string json = JsonConvert.SerializeObject(response);

            ApiResponse<List<ProductDTO>> deserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<ProductDTO>>>(json);

            DataTable dt = new DataTable();
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Sku", typeof(string));
            dt.Columns.Add("SitePrice", typeof(decimal));


            foreach (ProductDTO item in deserializedResponse.Content)
            {
                dt.Rows.Add(item.ProductName, item.Sku, item.SitePrice);
            }

            dataGridView1.DataSource = dt;
        }

    }
}
