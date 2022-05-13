﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using EFreshStore.Models;
using Newtonsoft.Json;

namespace EFreshStore.Utility
{
    public class UtilityClass
    {
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }
            return dt;
        }

        public static DataTable ConvertXLSXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch
            {
            }
            finally
            {
                oledbConn.Close();
            }
            return dt;
        }

        public static bool IsAllColumnExist(DataTable tableNameToCheck, List<string> columnsNames)
        {
            bool iscolumnExist = true;
            try
            {
                if (null != tableNameToCheck && tableNameToCheck.Columns != null)
                {
                    foreach (string columnName in columnsNames)
                    {
                        if (!tableNameToCheck.Columns.Contains(columnName))
                        {
                            iscolumnExist = false;
                            break;
                        }
                    }
                }
                else
                {
                    iscolumnExist = false;
                }
            }
            catch (Exception)
            {

            }
            return iscolumnExist;
        }

        public static List<string> MeghnaUserProperty()
        {
            List<string>columns=new List<string>();
            columns.Add("EmployeeId");
            columns.Add("Name");
            columns.Add("MobileNo");
            columns.Add("Email");
            columns.Add("Designation");
            columns.Add("DeliveryAddress1");
            columns.Add("DeliveryAddress2");
            return columns;
        }

        public static List<string> CorporateUserProperty()
        {
            List<string> columns = new List<string>();
            columns.Add("EmployeeId");
            columns.Add("Name");
            columns.Add("MobileNo");
            columns.Add("Email");
            columns.Add("Designation");
            //columns.Add("Address");

            return columns;
        }

        public static ErrorMessage ConvertJsonToMessage(string json)
        {
          return  JsonConvert.DeserializeObject<ErrorMessage>(json);
        }
        
    }
}