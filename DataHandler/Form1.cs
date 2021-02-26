using DataHandler.Handlers;
using DataHandler.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DataHandler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }
        
        private JsonHandler jsonHandler;

        private DataTable dtMain;

        private void InitializeData()
        {
            try
            {
                comboBox1.Items.AddRange(XmlHandler.GetInstance().ListSchemaName.ToArray());
                comboBox1.SelectedIndex = 0;
                comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
                ComboBox1_SelectedIndexChanged(comboBox1, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender is ComboBox comboBox)
                {
                    jsonHandler = new JsonHandler();

                    jsonHandler.GetDataTable += JsonHandler_GetDataTable;
                    jsonHandler.GetModelObject += JsonHandler_GetModelObject;
                    jsonHandler.InitializeData(comboBox1.SelectedItem.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void JsonHandler_GetDataTable(DataTable sender)
        {
            try
            {
                dtMain = sender;
                dataGridView1.DataSource = dtMain;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<JToken> JToken;
        private void JsonHandler_GetModelObject(object sender)
        {
            try
            {
                JToken = sender as List<JToken>;

                if (comboBox1.SelectedIndex == 0)
                {
                    ProductModel(ref JToken);
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    CustomerModel(ref JToken);
                }
                //Console.WriteLine("☆" + sender.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CustomerModel(ref List<JToken> sender)
        {
            try
            {
                List<Customer> listItems = new List<Customer>();
                foreach (JToken item in sender)
                {
                    listItems.Add(item.ToObject<Customer>());
                }

                foreach (Customer customer in listItems)
                {
                    List<string> listOneRow = new List<string>();
                    listOneRow.Add(customer.Id.ToString());
                    listOneRow.Add(customer.Name.ToString());
                    listOneRow.Add(customer.Country.ToString());
                    dtMain.Rows.Add(listOneRow.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProductModel(ref List<JToken> sender)
        {
            try
            {
                List<Product> listItems = new List<Product>();
                foreach (JToken item in sender)
                {
                    listItems.Add(item.ToObject<Product>());
                }

                foreach (Product product in listItems)
                {
                    List<string> listOneRow = new List<string>();

                    foreach (ProductDetails productDetail in product.ProductDetails)
                    {
                        //Console.WriteLine($"제품: {product.Name}, {productDetail.Description}");

                        listOneRow.Add(product.Id.ToString());
                        listOneRow.Add(product.Category);
                        listOneRow.Add(product.Name);
                        listOneRow.Add(product.Color);
                        listOneRow.Add(product.Stock.ToString());
                        listOneRow.Add(product.Price.ToString());

                        if (productDetail.Description != null)
                        {
                            listOneRow.Add(productDetail.Description);
                        }
                        else
                        {
                            listOneRow.Add(string.Empty);
                        }
                        if (productDetail.ImgUrl != null)
                        {
                            listOneRow.Add(productDetail.ImgUrl);
                        }
                        else
                        {
                            listOneRow.Add(string.Empty);
                        }

                        dtMain.Rows.Add(listOneRow.ToArray());
                    }

                    //Console.WriteLine("★" + sender.Count);
                    //sender = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false; //enable last row 
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false; //enable last row 
        }
    }
}
