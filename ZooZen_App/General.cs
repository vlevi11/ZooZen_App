using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public   class General
    {

        private   DataTable dt;
        private Api @object;

        public General()
        {
           // todo
        }

        public General(Api @object)
        {
            this.@object = @object;
        }

        public   DataTable GetProducts()
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

                dt = new DataTable();
                dt.Columns.Add("ProductName", typeof(string));
                dt.Columns.Add("Sku", typeof(string));
                dt.Columns.Add("SitePrice", typeof(decimal));


                foreach (ProductDTO item in deserializedResponse.Content)
                {
                    dt.Rows.Add(item.ProductName, item.Bvin, item.SitePrice);
                }

                return dt;
               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }



        public   void SavedItem(string bvin, string newvalue)
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
                int.TryParse(newvalue, out i);
                productDTO.SitePrice = i;
                // call the API to update the affiliate
                ApiResponse<ProductDTO> response = proxy.ProductsUpdate(productDTO);

               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public DataTable GetDataGridViewDataSource()
        {
            return dt;
        }
    }
}
