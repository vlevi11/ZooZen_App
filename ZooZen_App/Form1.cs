using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hotcakes.Commerce.Contacts;
using Hotcakes.CommerceDTO;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1.Contacts;
using Hotcakes.CommerceDTO.v1.Orders;
using Hotcakes.Modules.Core.Areas.Account.Models;
using Newtonsoft.Json;


namespace ZooZen_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //DESIGN
            this.BackColor = ColorTranslator.FromHtml("#ffebbd");
            this.button1.BackColor = ColorTranslator.FromHtml("#1e4b57");
            this.button2.BackColor = ColorTranslator.FromHtml("#1e4b57");
            this.dataGridView1.BackgroundColor = ColorTranslator.FromHtml("#f4A905");
            
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
            try
            {
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
                    dt.Rows.Add(item.ProductName, item.Bvin, item.SitePrice);
                }

                dataGridView1.DataSource = dt;
                label1.Text = String.Format("Selected products : {0}", dt.Rows);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }



        private void SavedItem(string bvin)
        {
            // endpoint implementálása
            string url = "http://20.234.113.211:8096/";
            string key = "1-4e562905-01dc-461b-b352-328f936981da";

            Api proxy = new Api(url, key);

            try
            {

                // get a matching affiliate from the REST API to update
                var productDTO = proxy.ProductsFind(bvin).Content;   // visszaad  egy ProductDTO object 

                int i;
                int.TryParse(textBox1.Text, out i);
                productDTO.SitePrice = i;
                // call the API to update the affiliate
                ApiResponse<ProductDTO> response = proxy.ProductsUpdate(productDTO);

                label1.Text = "Sikeres mentés.";
            }
            catch (Exception e)
            {
                label1.Text = e.Message;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int k = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow dr = dataGridView1.Rows[k];
            string s = dr.Cells[2].Value.ToString();
            textBox1.Text = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 1. Kell egy ciklus a griden
            int k = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow dr = dataGridView1.Rows[k];
            string s = dr.Cells[1].Value.ToString();

            // meg kell hivni a POST methodust
            SavedItem(s);

            // Adatok frissítése
            GetProducts();
        }

    }
}
