using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DataHandler.Handlers
{
    class JsonHandler
    {
        public delegate void DataTableHandler(DataTable sender);
        public event DataTableHandler GetDataTable;

        public delegate void JArrayHandler(JArray sender);
        public event JArrayHandler GetJArray;

        private string sJsonPath;
        private string sJsonRead;
        private JArray arrJArray;

        public JsonHandler(string jsonSchemaName)
        {
            sJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\", jsonSchemaName);
        }

        public void InitializeData()
        {
            sJsonRead = File.ReadAllText(sJsonPath);

            GetDataTable?.Invoke(CreateDataTable());

            GetJArray?.Invoke(arrJArray);
        }

        public DataTable CreateDataTable()
        {
            try
            {
                DataTable dtMain = new DataTable();

                arrJArray = JArray.Parse(sJsonRead);
                List<JProperty> listProperties = new List<JProperty>();
                listProperties = arrJArray.Children<JObject>().Take(1).Properties().ToList();

                List<string> listColName = new List<string>();

                foreach (JProperty prop in listProperties)
                {
                    if (prop.Value.HasValues)
                    {
                        foreach (JProperty subProp in prop.Value[0].ToList())
                        {
                            listColName.Add($"{prop.Name}({subProp.Name})");
                            //Console.WriteLine($"{prop.Name}({subProp.Name})");
                            //dtMain.Columns.Add(DataTableColumnGenerator(prop.Name, subProp.Name));
                        }
                    }
                    else
                    {
                        listColName.Add($"{prop.Name}");
                        //Console.WriteLine(prop.Name);
                        //dtMain.Columns.Add(DataTableColumnGenerator(prop.Name));
                    }
                }

                dtMain.Columns.AddRange(DataTableColumnGenerator(listColName));

                return dtMain;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataColumn[] DataTableColumnGenerator(List<string> listCol)
        {
            try
            {
                DataColumn[] dtCol = new DataColumn[listCol.Count];

                for (int i = 0; i < listCol.Count; i++)
                {
                    DataColumn col = new DataColumn();
                    col.ColumnName = listCol[i];
                    dtCol[i] = col;
                }

                return dtCol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataColumn DataTableColumnGenerator(string colName, string subColName = null)
        {
            try
            {
                DataColumn dtCol = new DataColumn();

                if(subColName != null)
                {
                    dtCol.ColumnName = $"{colName}({subColName})";
                }
                else
                {
                    dtCol.ColumnName = colName;
                }

                return dtCol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
