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
        public JsonHandler()
        {
        }

        public delegate void DataTableHandler(DataTable sender);
        public event DataTableHandler GetDataTable;

        public delegate void ModelObjectHandler(object sender);
        public event ModelObjectHandler GetModelObject;

        private string sJsonPath;
        private string sJsonRead;

        private JObject jObject;

        public void InitializeData(string jsonSchemaName)
        {
            sJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\", jsonSchemaName);
            sJsonRead = File.ReadAllText(sJsonPath);

            jTokens = JArray.Parse(sJsonRead).ToList();

            GetDataTable?.Invoke(CreateDataTable());
            GetModelObject?.Invoke(jTokens);
        }

        private List<JToken> jTokens;

        public DataTable CreateDataTable()
        {
            try
            {
                DataTable dtMain = new DataTable();

                jObject = jTokens[0] as JObject;
                List<JProperty> listProperties = new List<JProperty>();
                listProperties = jObject.Properties().ToList();

                List<string> listColName = new List<string>();

                foreach (JProperty prop in listProperties)
                {
                    if (prop.Value.HasValues)
                    {
                        foreach (JProperty subProp in prop.Value[0].ToList())
                        {
                            listColName.Add($"{prop.Name}({subProp.Name})");
                        }
                    }
                    else
                    {
                        listColName.Add($"{prop.Name}");
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
