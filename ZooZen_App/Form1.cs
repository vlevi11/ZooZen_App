using Hotcakes.CommerceDTO.v1.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;



namespace ZooZen_App
{
    public partial class Form1 : Form
    {
        private Api @object;
        private DataTable dt;
        private General gen;
        public Form1()
        {
            InitializeComponent();
            //DESIGN
            this.BackColor = ColorTranslator.FromHtml("#ffebbd");
            this.button1.BackColor = ColorTranslator.FromHtml("#1e4b57");
            this.button2.BackColor = ColorTranslator.FromHtml("#1e4b57");
            this.dataGridView1.BackgroundColor = ColorTranslator.FromHtml("#f4A905");
            gen = new General();
        }

        public Form1(Api @object)
        {
            this.@object = @object;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource =  gen.GetProducts();
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
            gen.SavedItem(s, textBox1.Text);

            label1.Text = "Sikeres mentés.";

            // Adatok frissítése
            dataGridView1.DataSource = gen.GetProducts();
        }



    }
}
