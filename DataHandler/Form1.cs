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

        private const string jsonProductSchema = "ProductSchema.json";
        private const string jsonCustomerSchema = "CustomerSchema.json";

        private JsonHandler jsonHandler;

        private DataTable dtMain;

        private void InitializeData()
        {
            try
            {
                string[] arrComboItems = new string[2];
                arrComboItems[0] = jsonProductSchema.ToString();
                arrComboItems[1] = jsonCustomerSchema.ToString();

                comboBox1.Items.AddRange(arrComboItems);
                comboBox1.SelectedIndex = 0;
                comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

                ComboBox1_SelectedIndexChanged(comboBox1, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender is ComboBox comboBox)
                {
                    if (comboBox.SelectedIndex == 0)
                    {
                        jsonHandler = new JsonHandler(jsonProductSchema);
                    }
                    else
                    {
                        jsonHandler = new JsonHandler(jsonCustomerSchema);
                    }

                    jsonHandler.GetDataTable += JsonHandler_GetDataTable; ;
                    jsonHandler.GetJArray += JsonHandler_GetJArray; ;
                    jsonHandler.InitializeData();
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private JArray mJArray;

        private void JsonHandler_GetJArray(JArray sender)
        {
            try
            {
                mJArray = sender;

                if (comboBox1.SelectedIndex == 0)
                {
                    ProductModel(ref mJArray);
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    CustomerModel(ref mJArray);
                }
                //Console.WriteLine("☆" + sender.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CustomerModel(ref JArray sender)
        {
            try
            {
                List<Customer> listCustomers = sender.ToObject<List<Customer>>();

                foreach (Customer customer in listCustomers)
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

        private void ProductModel(ref JArray sender)
        {
            try
            {
                List<Product> listProducts = sender.ToObject<List<Product>>();

                foreach (Product product in listProducts)
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
    }
}
