using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Saplin.Controls;
using System.Collections;
using Elmah;

namespace CreamBell_DMS_WebApps.App_Code
{

    public class Global
    {
        public SqlConnection conn = null;
        public SqlCommand cmd = null;
        public SqlTransaction trans;
        public DataTable dt = null;
        public SqlDataAdapter da = null;
        public static string strSessionID;
        

        public static void ExportDataTable(DataTable dtExport, string FileName, string ExportType, string HeaderText)
        {
            try
            {
                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dtExport;
                GridView1.DataBind();
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                HttpContext.Current.Response.Charset = "";
                if (ExportType.ToUpper() == "WORD")
                {
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-word ";
                }
                else if (ExportType.ToUpper() == "PDF")
                {
                    HttpContext.Current.Response.ContentType = "application/pdf";
                }
                else
                {
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                }

                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
                if (ExportType.ToUpper() == "EXCEL")
                {
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                }
                GridView1.RenderControl(hw);
                //style to format numbers to string
                if (ExportType.ToUpper() == "EXCEL")
                {
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    HttpContext.Current.Response.Write(style);
                }
                HttpContext.Current.Response.Write("<table><tr><td colspan=\"" + dtExport.Columns.Count + "\"><center> <b>" + HeaderText + " </b></center></td></tr></table>");
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public String ConvertDataTableTojSonString(DataTable dataTable)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer =
                   new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<String, Object>>();
            try
            {

                Dictionary<String, Object> row;

                foreach (DataRow dr in dataTable.Rows)
                {
                    row = new Dictionary<String, Object>();
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    tableRows.Add(row);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return serializer.Serialize(tableRows);
        }

        public string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["CreamBell"].ToString();
        }

        public SqlConnection GetConnection()
        {
            conn = new SqlConnection(GetConnectionString());
            conn.Open();
            return conn;
        }

        public void CloseSqlConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public void RemoveTimezoneForDataSet(DataSet ds)
        {
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {

                    if (dc.DataType == typeof(DateTime))
                    {
                        dc.DateTimeMode = DataSetDateTime.Unspecified;
                    }
                }
            }
        }

        public string[] CalculatePrice(string ItemCode, Int32 Box)          //---Date 10 April: Rahul Ranjan---//
        {
            string[] returnResult = null;
            try
            {
                decimal crate;
                decimal price;
                decimal Ltr;
                decimal FinalAmount;


                GetConnection();

                string query = " Select PRODUCT_CODE,Product_group, PRODUCT_NAME, PRODUCT_MRP, PRODUCT_PACKSIZE, PRODUCT_CRATE_PACKSIZE, UOM, LTR from ax.ACXPRODUCTMASTER " +
                               " where PRODUCT_CODE='" + ItemCode + "' ";

                DataTable dt = GetData(query);
                if (dt.Rows.Count > 0)
                {
                    decimal ProductMRP = Convert.ToDecimal(dt.Rows[0]["PRODUCT_MRP"].ToString());
                    decimal ProductPackSize = Convert.ToDecimal(dt.Rows[0]["PRODUCT_PACKSIZE"].ToString());
                    decimal ProductCratePackSize = Convert.ToDecimal(dt.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString());
                    decimal Litre = Convert.ToDecimal(dt.Rows[0]["LTR"].ToString());

                    crate = Math.Round((Box / ProductCratePackSize), 2);             //--Crate Value--//
                    Ltr = Math.Round((((Box * ProductPackSize) * Litre) / 1000), 2);   //Ltr Value in Unit (ltr)--//
                    price = Math.Round(ProductMRP, 2);                               //--Actual MRP of item--//
                    FinalAmount = Math.Round((price * Box), 2);                      //--Value of Product after Box entry --//

                    returnResult = new string[5];
                    returnResult[0] = crate.ToString();
                    returnResult[1] = Ltr.ToString();
                    returnResult[2] = price.ToString();
                    returnResult[3] = FinalAmount.ToString();
                    returnResult[4] = dt.Rows[0]["UOM"].ToString();
                }
                return returnResult;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return returnResult;
            }

        }

//        public string[] CalculatePrice1(string ItemCode, String RetailerCode, decimal Value, string EntryType)          //---Date 10 April: Rahul Ranjan---//
//        {
//            string[] returnResult = null;
//            try
//            {
//                decimal crate;
//                decimal price;
//                //decimal packSize;
//                decimal Ltr;
//                decimal FinalAmount;
//                decimal Box;


//                GetConnection();
                
//                string query = "";
//                if (RetailerCode != "")
//                {
////                    query = @"select top 1 I.ITEMID as PRODUCT_CODE,I.Product_group, I.PRODUCT_NAME,t.AMOUNT as PRODUCT_MRP, I.PRODUCT_PACKSIZE, I.PRODUCT_CRATE_PACKSIZE,
////                                I.UOM, I.LTR  from ax.InventTable I
////                                Inner join DBO.PriceDiscTable t on I.ITEMID =t.ITEmRelation
////                                where ACCOUNTRELATION = (select PriceGroup from ax.ACXCUSTMASTER where CUSTOMER_CODE = '" + RetailerCode + "') "
////                + " and I.ITEMID ='" + ItemCode + "' and t.FromDate<=getdate() order by t.FromDate desc,MODIFIEDDATETIME DESC";

//                    query = @"select I.ITEMID as PRODUCT_CODE,I.Product_group, I.PRODUCT_NAME,t.AMOUNT as PRODUCT_MRP, I.PRODUCT_PACKSIZE, I.PRODUCT_CRATE_PACKSIZE,
//                                I.UOM, I.LTR  from ax.InventTable I
//                                Inner join DBO.ACX_UDF_GETPRICE(getdate(),(select PriceGroup from ax.ACXCUSTMASTER where CUSTOMER_CODE = '" + RetailerCode + "'),'" + ItemCode + "') t on I.ITEMID =t.ITEmRelation";
                                
//                }
//                else
//                {
//                    query = " Select ItemId as PRODUCT_CODE,Product_group, PRODUCT_NAME, PRODUCT_MRP, PRODUCT_PACKSIZE, PRODUCT_CRATE_PACKSIZE, UOM, LTR from ax.InventTable " +
//                              " where ItemId='" + ItemCode + "' ";
//                }

//                DataTable dt = GetData(query);
//                if (dt.Rows.Count > 0)
//                {
//                    decimal ProductMRP = Convert.ToDecimal(dt.Rows[0]["PRODUCT_MRP"].ToString());
//                    decimal ProductPackSize = Convert.ToDecimal(dt.Rows[0]["PRODUCT_PACKSIZE"].ToString());
//                    decimal ProductCratePackSize = Convert.ToDecimal(dt.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString());
//                    decimal Litre = Convert.ToDecimal(dt.Rows[0]["LTR"].ToString());

//                    if (EntryType == "Crate")
//                    {
//                        Box = Math.Round((ProductCratePackSize * Value), 2);
//                        Ltr = Math.Round(((Box * Litre * ProductPackSize) / 1000), 2);               //Changed Logic on 12 May 2016[Pramod Sir]
//                        price = Math.Round(ProductMRP, 2);
//                        FinalAmount = Math.Round((price * Box), 2);

//                        returnResult = new string[6];
//                        returnResult[0] = Box.ToString();
//                        returnResult[1] = Ltr.ToString();
//                        returnResult[2] = price.ToString();
//                        returnResult[3] = FinalAmount.ToString();
//                        returnResult[4] = dt.Rows[0]["UOM"].ToString();
//                        returnResult[5] = EntryType;
//                    }

//                    if (EntryType == "Box")
//                    {
//                        crate = Math.Round(Math.Ceiling(Value / ProductCratePackSize), 2);
//                        Ltr = Math.Round(((Value * Litre * ProductPackSize) / 1000), 2);            //Changed Logic on 12 May 2016[Pramod Sir]
//                        price = Math.Round(ProductMRP, 2);
//                        FinalAmount = Math.Round((price * Value), 2);

//                        returnResult = new string[6];
//                        returnResult[0] = crate.ToString();
//                        returnResult[1] = Ltr.ToString();
//                        returnResult[2] = price.ToString();
//                        returnResult[3] = FinalAmount.ToString();
//                        returnResult[4] = dt.Rows[0]["UOM"].ToString();
//                        returnResult[5] = EntryType;


//                    }

//                }
//                else
//                {
//                    returnResult = new string[6];
//                    returnResult[0] = "";
//                    returnResult[1] = "";
//                    returnResult[2] = "";
//                    returnResult[3] = "";
//                    returnResult[4] = "";
//                    returnResult[5] = "";
//                }
//                return returnResult;
//            }
//            catch (Exception ex)
//            {
//                return returnResult;
//            }
            

//        }

           
        public string[] CalculatePrice1(string ItemCode, String RetailerCode, decimal Value, string EntryType, string siteid = "", int IsRetailer = 1)          //---Date 10 April: Rahul Ranjan---//
        {
            string[] returnResult = null;
            try
            {
                decimal crate;
                decimal price;
                //decimal packSize;
                decimal Ltr;
                decimal FinalAmount;
                decimal Box;

                GetConnection();

                string query = "EXEC ax.ACX_GetProductRate '" + ItemCode + "','" + RetailerCode + "','" + siteid + "'";

                DataTable dt = GetData(query);
                if (dt.Rows.Count > 0)
                {
                    decimal ProductMRP = Convert.ToDecimal(dt.Rows[0]["PRODUCT_MRP"].ToString());
                    decimal ProductPackSize = Convert.ToDecimal(dt.Rows[0]["PRODUCT_PACKSIZE"].ToString());
                    decimal ProductCratePackSize = Convert.ToDecimal(dt.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString());
                    decimal Litre = Convert.ToDecimal(dt.Rows[0]["LTR"].ToString());

                    if (EntryType == "Crate")
                    {
                        Box = Math.Round((ProductCratePackSize * Value), 2);
                        Ltr = Math.Round(((Box * Litre * ProductPackSize) / 1000), 2);               //Changed Logic on 12 May 2016[Pramod Sir]
                        price = Math.Round(ProductMRP, 2);
                        FinalAmount = Math.Round((price * Box), 2);

                        returnResult = new string[6];
                        returnResult[0] = Box.ToString();
                        returnResult[1] = Ltr.ToString();
                        returnResult[2] = price.ToString();
                        returnResult[3] = FinalAmount.ToString();
                        returnResult[4] = dt.Rows[0]["UOM"].ToString();
                        returnResult[5] = EntryType;
                    }

                    if (EntryType == "Box")
                    {
                        crate = Math.Round(Math.Ceiling(Value / (ProductCratePackSize == 0 ? 1 : ProductCratePackSize)), 2);
                        Ltr = Math.Round(((Value * Litre * ProductPackSize) / 1000), 2);            //Changed Logic on 12 May 2016[Pramod Sir]
                        price = Math.Round(ProductMRP, 2);
                        FinalAmount = Math.Round((price * Value), 2);

                        returnResult = new string[6];
                        returnResult[0] = crate.ToString();
                        returnResult[1] = Ltr.ToString();
                        returnResult[2] = price.ToString();
                        returnResult[3] = FinalAmount.ToString();
                        returnResult[4] = dt.Rows[0]["UOM"].ToString();
                        returnResult[5] = EntryType;
                    }
                }
                else
                {
                    returnResult = new string[6];
                    returnResult[0] = "";
                    returnResult[1] = "";
                    returnResult[2] = "";
                    returnResult[3] = "";
                    returnResult[4] = "";
                    returnResult[5] = "";
                }
                return returnResult;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            
                returnResult = new string[6];
                returnResult[0] = "";
                returnResult[1] = "";
                returnResult[2] = "";
                returnResult[3] = "";
                returnResult[4] = "";
                returnResult[5] = "";
                return returnResult;
            }


        }

        public void DbEventWrite(string ProcessId,string SiteId,int EventId,string EventDesc,int RowFound)
        {
            try
            {
                //GetConnection();
                //string value = string.Empty;
                //cmd = new SqlCommand();
                //cmd.Connection = conn;
                //cmd.CommandTimeout = 100;
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "usp_Ins_EventLog";
                //cmd.Parameters.AddWithValue("@SiteId", SiteId);
                //cmd.Parameters.AddWithValue("@ProcessId", ProcessId);
                //cmd.Parameters.AddWithValue("@EventNo", EventId);
                //cmd.Parameters.AddWithValue("@EventDesc", EventDesc);
                //cmd.Parameters.AddWithValue("@RowFound", RowFound);
                ////cmd.Parameters.AddWithValue("@SessionId", strSessionID);
                
                //cmd.ExecuteNonQuery();                
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }
            

        public DataSet GetDsData(string query)
        {
            GetConnection();
            DataSet ds = new DataSet();
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds); 
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return ds;
        }
        public DataTable GetData(string query)
        {
            dt = new DataTable();
            try
            {
                cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                da = new SqlDataAdapter(cmd);
                GetConnection();
                cmd.Connection = conn;
                da.Fill(dt);
                //dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                CloseSqlConnection();
                //conn.Close();
                //conn.Dispose();
            }
            return dt;
        }

        public int ExecuteCommand(string query)
        {
            GetConnection();
            int rowAffected = 0;
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                trans.Rollback();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return rowAffected;
        }

        //internal void BindToDropDown(DropDownCheckBoxes drpCustGrp, string query, string v1, string v2)
        //{
        //    throw new NotImplementedException();
        //}

        //Bind TO dropdown/////
        public void BindToDropDown(DropDownList DDL, string strQuery, string display, string val)
        {
            try
            {
                int intTotalRec;
                System.Data.SqlClient.SqlDataAdapter sqlda;
                System.Data.DataSet dsDDLItem;

                GetConnection();
                cmd = new System.Data.SqlClient.SqlCommand(strQuery, conn);
                sqlda = new System.Data.SqlClient.SqlDataAdapter(cmd);
                dsDDLItem = new System.Data.DataSet();
                sqlda.Fill(dsDDLItem);
                intTotalRec = dsDDLItem.Tables[0].Rows.Count;
                for (int i = 0; i <= intTotalRec - 1; i++)
                {
                    DDL.Items.Add(new ListItem(dsDDLItem.Tables[0].Rows[i][display].ToString(), dsDDLItem.Tables[0].Rows[i][val].ToString()));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        ////
        public DataTable GetData_New(string query, CommandType type, List<string> list, List<string> item)
        {
            
            dt = new DataTable();
            try
            {
                cmd = new SqlCommand();
                
                cmd.CommandTimeout = 0;
                cmd.CommandType = type;
                if (type == CommandType.StoredProcedure)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        cmd.Parameters.AddWithValue(list[i].ToString(), item[i].ToString());
                    }
                }
                cmd.CommandText = query;
                //dt.Load(cmd.ExecuteReader());
                GetConnection();
                cmd.Connection = conn;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                CloseSqlConnection();
                //conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return dt;
        }

        public DataSet GetDataSet_New(string query, CommandType type, List<string> list, List<string> item)
        {

            DataSet ds = new DataSet();
            try
            {
                cmd = new SqlCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = type;
                if (type == CommandType.StoredProcedure)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        cmd.Parameters.AddWithValue(list[i].ToString(), item[i].ToString());
                    }
                }
                cmd.CommandText = query;
                GetConnection();
                cmd.Connection = conn;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                CloseSqlConnection();
                //conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return ds;
        }

        internal void GetData()
        {
            throw new NotImplementedException();
        }

        public string GetScalarValueOld(string query)
        {
            GetConnection();
            string value = string.Empty;
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                object obj = cmd.ExecuteScalar();
                value = obj.ToString();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return value;
        }

        public string GetScalarValue(string query)
        {
            GetConnection();
            string value = null;
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                object obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    value = obj.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return value;
        }

        public string GetNumSequence(int NumSeqType, string SiteId, string DataAreaId)
        {
            GetConnection();
            string value = string.Empty;
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACXNUMBERSEQ";
                cmd.Parameters.AddWithValue("@NUMSEQTYPE",NumSeqType);
                cmd.Parameters.AddWithValue("@SITEID",SiteId);
                cmd.Parameters.AddWithValue("@DATAAREAID",DataAreaId);
                cmd.Parameters.AddWithValue("@Type","N");

                object obj = cmd.ExecuteScalar();
                value = obj.ToString();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return value;
        }

        public DataTable GetNumSequenceNew(int NumSeqType, string SiteId, string DataAreaId)
        {
            GetConnection();
            string value = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACXNUMBERSEQ";
                cmd.Parameters.AddWithValue("@NUMSEQTYPE", NumSeqType);
                cmd.Parameters.AddWithValue("@SITEID", SiteId);
                cmd.Parameters.AddWithValue("@DATAAREAID", DataAreaId);
                cmd.Parameters.AddWithValue("@Type", "N");

                //dt.Load(cmd.ExecuteReader());
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return dt;
        }

        public int UpdateLastNumSequence(int NumSeqType, string SiteId, string DataAreaId, SqlConnection conn, SqlTransaction trans)
        {
            int a = 0;
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACXNUMBERSEQ";
                cmd.Parameters.AddWithValue("@NUMSEQTYPE", NumSeqType);
                cmd.Parameters.AddWithValue("@SITEID", SiteId);
                cmd.Parameters.AddWithValue("@DATAAREAID", DataAreaId);
                cmd.Parameters.AddWithValue("@Type", "U");

                a = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return a;
        }


        public static decimal ParseDecimal(string value)
        {
            try
            {
                if (value.Trim().Length == 0 || value == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(value);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return 0;
            }
        }



        internal void BindToDropDown(RadioButtonList chkCustomerName, string sqlstr, string p1, string p2)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalBox(string itamid, decimal crate, decimal pcs, decimal box)
        {
            try
            {
                GetConnection();
                string query = " Select ItemId as PRODUCT_CODE,PRODUCT_PACKSIZE, PRODUCT_CRATE_PACKSIZE from ax.InventTable " +
                            " where ItemId='" + itamid + "' ";
                decimal TotalBox = 0;
                DataTable dt = GetData(query);
                if (dt.Rows.Count > 0)
                {
                    decimal ProductPackSize = Convert.ToDecimal(dt.Rows[0]["PRODUCT_PACKSIZE"].ToString());
                    decimal ProductCratePackSize = Convert.ToDecimal(dt.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString());
                    // decimal Litre = Convert.ToDecimal(dt.Rows[0]["LTR"].ToString());

                    TotalBox = (crate * ProductCratePackSize)
                                     + box
                                     + (pcs / ProductPackSize);
                }

                //if (TotalBox == 0 || TotalBox == null)
                //{
                //    return 0;
                //}
                //else
                //{
                return TotalBox;

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return 0;
            }
        }

        public DataTable FillDataTable(string query)
        {
            DataTable dt = new DataTable("temp");
            try
            {
                SqlConnection con = GetConnection();
                using (SqlDataAdapter dap = new SqlDataAdapter(query, con))
                {
                    dap.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return dt;
            }

        }

        public DataTable GetPcsBillingApplicability(string State, string ProductGroup)
        {
            GetConnection();
            string value = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACX_USP_PCSBillingApplicable";
                cmd.Parameters.AddWithValue("@STATECODE", State);
                cmd.Parameters.AddWithValue("@ProductGroup", ProductGroup);

                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return dt;
        }

        public static bool GetPrevSelection(string SchemeCode, string SetNo, ref GridView gv,string SrlNo)
        {
            try
            {
                foreach (GridViewRow rw in gv.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    HiddenField hdnSchemeType = (HiddenField)rw.FindControl("hdnSchemeType");
                    HiddenField hdnSchSrlNo = (HiddenField)rw.FindControl("hdnSchSrlNo");
                    if (chkBx.Checked && rw.Cells[1].Text != SchemeCode)
                    {
                        return true;
                    }
                    if (chkBx.Checked && rw.Cells[7].Text == "0" && hdnSchemeType.Value.ToString() == "2" && SrlNo != hdnSchSrlNo.Value.ToString())
                    {
                        return true;
                    }
                    if (chkBx.Checked && rw.Cells[1].Text == SchemeCode && rw.Cells[7].Text != SetNo)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return false;
        }

        public  bool ValidateSchemeQty(string SelectedShemeCode, ref GridView gvScheme)
        {
            try
            {
                string schemeCode = string.Empty;
                int cnt = 0;

                string strSQLCond = "DECLARE @tbl Table (SchemeCode Varchar(50),SetNo int,Slab int,FreeBox int,PicBox int,FreePCS int,PicPCS int) ";
                //string strSQL = " SELECT Top 1 SetNo FROM @tbl WHERE SetNo=0 AND (ISNULL(FreeBox,0) + ISNULL(FreePCS,0)) >0 " +
                //               //" GROUP BY SetNo,Slab Having ((Max(FreeBox) <> SUM(PicBox)) OR (MAX(FreePCS) <> SUM(PicPCS)))";
                //               " GROUP BY SetNo,Slab Having MAX(FreeBox+FreePCS) <> SUM(PicBox+PicPCS)";

                string strSQL = " SELECT CASE WHEN SUM(PicBox) >0 THEN MAX(FreeBox) -SUM(PicBox) WHEN SUM(PicPCS) >0 THEN CASE WHEN SUM(PicPCS) >0 THEN MAX(FreePCS) -SUM(PicPCS) ELSE 0 END ELSE -1 END " +
                                " FROM @tbl WHERE SetNo=0 AND (ISNULL(FreeBox,0) + ISNULL(FreePCS,0)) >0 GROUP BY SetNo";

                if (SelectedShemeCode.Trim().Length == 0 && gvScheme.Rows.Count > 0)
                {
                    return false;
                }
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkSelect = (CheckBox)rw.FindControl("chkSelect");
                    if (schemeCode != rw.Cells[1].Text && chkSelect.Checked == true)
                    {
                        cnt += 1;
                        schemeCode = rw.Cells[1].Text;
                        if (cnt > 1)
                        {
                            return false;
                        }
                    }

                    if (rw.Cells[1].Text == SelectedShemeCode)
                    {
                        if (rw.Cells[8].Text == "&nbsp;")
                        { rw.Cells[8].Text = "0"; }
                        if (rw.Cells[11].Text == "&nbsp;")
                        { rw.Cells[11].Text = "0"; }
                        if (rw.Cells[11].Text == "&nbsp;")
                        { rw.Cells[11].Text = "0"; }
                        if (rw.Cells[11].Text == "&nbsp;")
                        { rw.Cells[11].Text = "0"; }
                        TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                        TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                        txtQty.Text = (txtQty.Text.Trim().Length == 0 ? "0" : txtQty.Text);
                        txtQtyPcs.Text = (txtQtyPcs.Text.Trim().Length == 0 ? "0" : txtQtyPcs.Text);

                        strSQLCond = strSQLCond + " INSERT INTO @tbl VALUES ('" + rw.Cells[1].Text + "'," + rw.Cells[7].Text + "," + rw.Cells[6].Text + "," + rw.Cells[8].Text + "," + txtQty.Text + "," + rw.Cells[11].Text + "," + txtQtyPcs.Text + ")";
                    }
                }
                DataTable dt = new DataTable();
                dt = this.GetData(strSQLCond + strSQL);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) == 0)
                    { return true; }
                    else
                    { return false; }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        public bool ExcelDataCheckForPcsApplicability(DataTable dtData, string stateCode)
        {
            bool retValue = true;
            try
            {
                for (int i = 0; i < dtData.Rows.Count; i++)
                {

                    if (dtData.Rows[i]["Pcs"].ToString().Trim().Length == 0)
                    {
                        dtData.Rows[i]["Pcs"] = "0";
                    }
                    if (dtData.Rows[i]["Qty"].ToString().Trim().Length == 0)
                    {
                        dtData.Rows[i]["Qty"] = "0";
                    }
                    if (dtData.Rows[i]["Crate"].ToString().Trim().Length == 0)
                    {
                        dtData.Rows[i]["Crate"] = "0";
                    }
                    if (dtData.Rows[i]["ProductCode"].ToString().Trim().Length == 0 || dtData.Rows[i]["ProductCode"].ToString() == "0")
                    {
                        dtData.Rows[i].Delete();
                        dtData.AcceptChanges();
                        continue;
                    }
                    if (!GetPcsBillingApplicabilityByProdCode(stateCode, dtData.Rows[i]["ProductCode"].ToString()) && Convert.ToInt32(dtData.Rows[i]["Pcs"].ToString()) > 0)
                    {
                        retValue = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);

                retValue = false;
            }
            return retValue;
        }

        public bool GetPcsBillingApplicabilityByProdCode(string State, string ProductCode)
        {
            GetConnection();
            bool value = false;
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACX_USP_PCSBillingApplicableByProdCode";
                cmd.Parameters.AddWithValue("@STATECODE", State);
                cmd.Parameters.AddWithValue("@ProductCode", ProductCode);

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][1].ToString() == "Y")
                    { value = true; }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                SqlConnection.ClearPool(conn);
            }
            return value;
        }

        public static decimal ConvertToDecimal(object value)
        {
            decimal retValue;
            try
            {
                if (value != null && value.ToString().Trim().Length > 0)
                {
                    retValue = Convert.ToDecimal(value);
                }
                else
                {
                    retValue = 0;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                retValue = 0;
            }
            return retValue;
        }

        public bool IsDate(string inputDate)
        {
            bool isDate = true;
            try
            {
                DateTime dt = DateTime.Parse(inputDate);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                isDate = false;
            }
            return isDate;
        }

        public void SessionData()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();// on page load!!!!
                                                                                  // string query1 = "select *from AX.ACX_SALESHIERARCHY ";
                SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                //cmd.CommandTimeout = 100;
                cmd.CommandType = CommandType.StoredProcedure;
                String querysp = "[dbo].[usp_GetSALESHIERARCHY]";
                //string query2 = "SELECT case when isnull(HOSCODE,'')='' then 'HOS' ELSE HOSCODE END HOSCODE,case when isnull(executivecode,'')='' then 'EX' ELSE executivecode END executivecode ,case when isnull(RMCODE,'')='' then 'RM' ELSE RMCODE END RMCODE ,case when isnull(RMNAME,'')='' then 'RMn' ELSE RMNAME END RMNAME ,case when isnull(HOSNAME,'')='' then 'HOSn' ELSE HOSNAME END HOSNAME ,case when isnull(zmcode,'')='' then 'ZM' ELSE zmcode END zmcode,case when isnull(ZMNAME,'')='' then 'ZMn' ELSE ZMNAME END ZMNAME  ,case when isnull(gmcode,'')='' then 'GM' ELSE gmcode END gmcode,case when isnull(GMNAME,'')='' then 'GMn' ELSE GMNAME END GMNAME  ,case when isnull(vpcode,'')='' then 'VP' ELSE vpcode END vpcode ,case when isnull(VPNAME,'')='' then 'VPn' ELSE VPNAME END VPNAME   ,case when isnull(asmcode,'')='' then 'ASM' ELSE asmcode END asmcode ,case when isnull(RMNAME,'')='' then 'RMn' ELSE RMNAME END RMNAME   ,case when isnull(RMcode,'')='' then 'RM' ELSE RMcode END RMcode ,case when isnull(ASMNAME,'')='' then 'ASMn' ELSE ASMNAME END ASMNAME   ,case when isnull(dgmcode,'')='' then 'DGM' ELSE DGMCODE END dgmcode, case when isnull(DGMNAME,'')='' then 'DGMn' ELSE DGMNAME END DGMNAME, * FROM [7002].[ax].[ACX_SALESHIERARCHY] ;";            
                cmd.CommandText = querysp;

                cmd.Parameters.AddWithValue("@USERCODE", Convert.ToString(HttpContext.Current.Session["USERID"]));
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                HttpContext.Current.Session["s"] = dt;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public static string GetSelectState(ref ListBox lstState,bool WithComma)
        {
            string StateList = "";
            foreach (ListItem item in lstState.Items)
            {
                if (item.Selected)
                {
                    if (StateList == "")
                    {
                        if (WithComma)
                        { StateList = "'" + item.Value.ToString() + "'"; }
                        else
                        { StateList = item.Value.ToString(); }
                    }
                    else
                    {
                        if (WithComma)
                        { StateList += ",'" + item.Value.ToString() + "'"; }
                        else
                        { StateList += "," + item.Value.ToString(); }
                    }
                }
            }
            return StateList;
        }
        public static string GetSelectBUList(ref ListBox LstBu, bool WithComma)
        {
            string List = "";
            try
            {
                foreach (ListItem item in LstBu.Items)
                {
                    if (item.Selected)
                    {
                        if (List == "")
                        {
                            if (WithComma)
                            { List = "'" + item.Value.ToString() + "'"; }
                            else
                            { List = item.Value.ToString(); }
                        }
                        else
                        {
                            if (WithComma)
                            { List += ",'" + item.Value.ToString() + "'"; }
                            else
                            { List += "," + item.Value.ToString(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return List;
        }
        public static string GetSelectCustGroup(ref ListBox lstCustGroup, bool WithComma)
        {
            string CustGroup = "";
            try
            {
                foreach (ListItem item in lstCustGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (CustGroup == "")
                        {
                            if (WithComma)
                            { CustGroup = "'" + item.Value.ToString() + "'"; }
                            else
                            { CustGroup = item.Value.ToString(); }
                        }
                        else
                        {
                            if (WithComma)
                            { CustGroup += ",'" + item.Value.ToString() + "'"; }
                            else
                            { CustGroup += "," + item.Value.ToString(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return CustGroup;
        }

        public static string GetSelectSite(ref ListBox lstSite, bool WithComma)
        {
            string SiteList = "";
            try
            {
                foreach (ListItem item in lstSite.Items)
                {
                    if (item.Selected)
                    {
                        if (SiteList == "")
                        {
                            if (WithComma)
                            { SiteList = "'" + item.Value.ToString() + "'"; }
                            else
                            { SiteList = item.Value.ToString(); }

                        }
                        else
                        {
                            if (WithComma)
                            { SiteList += ",'" + item.Value.ToString() + "'"; }
                            else
                            { SiteList += "," + item.Value.ToString(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return SiteList;
        }
        public static string GetSelectCustMaster(ref ListBox lstCustMaster, bool WithComma)
        {
            string CustMaster = "";
            try
            {
                foreach (ListItem item in lstCustMaster.Items)
                {
                    if (item.Selected)
                    {
                        if (CustMaster == "")
                        {
                            if (WithComma)
                            { CustMaster = "'" + item.Value.ToString() + "'"; }
                            else
                            { CustMaster = item.Value.ToString(); }
                        }
                        else
                        {
                            if (WithComma)
                            { CustMaster += ",'" + item.Value.ToString() + "'"; }
                            else
                            { CustMaster += "," + item.Value.ToString(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return CustMaster;
        }
        public static DataTable HierarchyDataTable(ref CheckBoxList chkListHOS, ref CheckBoxList chkListVP, ref CheckBoxList chkListGM, ref CheckBoxList chkListDGM, ref CheckBoxList chkListRM, ref CheckBoxList chkListZM, ref CheckBoxList chkListASM, ref CheckBoxList chkListEXECUTIVE)
        {
            DataTable DtSaleHierarchy = null;
            try
            {
                List<string> item = new List<string>();
                string HOS, VP, GM, DGM, RM, ZM, ASM, EXECUTIVE;
                HOS = VP = GM = DGM = RM = ZM = ASM = EXECUTIVE = "";
                DtSaleHierarchy = (DataTable)HttpContext.Current.Session["SaleHierarchy"];
                DataTable dtfinal = new DataTable();
                #region Get Selected Value from Controls
                foreach (System.Web.UI.WebControls.ListItem litem in chkListHOS.Items)
                {
                    if (litem.Selected)
                    {
                        if (HOS.Length == 0)
                            HOS = "'" + litem.Value.ToString() + "'";
                        else
                            HOS += ",'" + litem.Value.ToString() + "'";
                    }

                }
                var newhos = chkListHOS.Items.Cast<ListItem>().Where(it => it.Selected == true).Select(it => it.Value);
                string result = string.Join(",", newhos);
                foreach (System.Web.UI.WebControls.ListItem litem in chkListVP.Items)
                {
                    if (litem.Selected)
                    {
                        if (VP.Length == 0)
                            VP = "'" + litem.Value.ToString() + "'";
                        else
                            VP += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in chkListGM.Items)
                {
                    if (litem.Selected)
                    {
                        if (GM.Length == 0)
                            GM = "'" + litem.Value.ToString() + "'";
                        else
                            GM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in chkListDGM.Items)
                {
                    if (litem.Selected)
                    {
                        if (DGM.Length == 0)
                            DGM = "'" + litem.Value.ToString() + "'";
                        else
                            DGM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in chkListRM.Items)
                {
                    if (litem.Selected)
                    {
                        if (RM.Length == 0)
                            RM = "'" + litem.Value.ToString() + "'";
                        else
                            RM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in chkListZM.Items)
                {
                    if (litem.Selected)
                    {
                        if (ZM.Length == 0)
                            ZM = "'" + litem.Value.ToString() + "'";
                        else
                            ZM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in chkListASM.Items)
                {
                    if (litem.Selected)
                    {
                        if (ASM.Length == 0)
                            ASM = "'" + litem.Value.ToString() + "'";
                        else
                            ASM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in chkListEXECUTIVE.Items)
                {
                    if (litem.Selected)
                    {
                        if (EXECUTIVE.Length == 0)
                            EXECUTIVE = "'" + litem.Value.ToString() + "'";
                        else
                            EXECUTIVE += ",'" + litem.Value.ToString() + "'";
                    }
                }

                #endregion
                string SearchCondition;
                SearchCondition = "";
                #region BindSearch Condition
                if (HOS.Length > 0)
                {
                    SearchCondition = "HOSCODE in (" + HOS + ")";
                }
                if (VP.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and VPCODE in (" + VP + ")";
                    else
                        SearchCondition = " VPCODE in (" + VP + ")";
                }
                if (GM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and GMCODE in (" + GM + ")";
                    else
                        SearchCondition = " GMCODE in (" + GM + ")";
                }
                if (DGM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and DGMCODE in (" + DGM + ")";
                    else
                        SearchCondition = " DGMCODE in (" + DGM + ")";
                }
                if (RM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and RMCODE in (" + RM + ")";
                    else
                        SearchCondition = " RMCODE in (" + RM + ")";
                }
                if (ZM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and ZMCODE in (" + ZM + ")";
                    else
                        SearchCondition = " ZMCODE in (" + ZM + ")";
                }
                if (ASM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and ASMCODE in (" + ASM + ")";
                    else
                        SearchCondition = " ASMMCODE in (" + ASM + ")";
                }
                if (EXECUTIVE.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and EXECUTIVECODE in (" + EXECUTIVE + ")";
                    else
                        SearchCondition = " EXECUTIVECODE in (" + EXECUTIVE + ")";
                }

                #endregion
                if (SearchCondition.Length > 0)
                {
                    return DtSaleHierarchy.Select(SearchCondition).CopyToDataTable();
                }
                else
                {
                    return DtSaleHierarchy.Clone();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return DtSaleHierarchy;
            }
        }
        
        public static DataTable HierarchyDataTable1(ref ListBox lstHOS, ref ListBox lstVP, ref ListBox lstGM, ref ListBox lstDGM, ref ListBox lstRM, ref ListBox lstZM, ref ListBox lstASM, ref ListBox lstEXECUTIVE)
        {
            DataTable DtSaleHierarchy = null;
            try
            {
                List<string> item = new List<string>();
                string HOS, VP, GM, DGM, RM, ZM, ASM, EXECUTIVE;
                HOS = VP = GM = DGM = RM = ZM = ASM = EXECUTIVE = "";
                DtSaleHierarchy = (DataTable)HttpContext.Current.Session["SaleHierarchy"];
                DataTable dtfinal = new DataTable();
                #region Get Selected Value from Controls
                foreach (System.Web.UI.WebControls.ListItem litem in lstHOS.Items)
                {
                    if (litem.Selected)
                    {
                        if (HOS.Length == 0)
                            HOS = "'" + litem.Value.ToString() + "'";
                        else
                            HOS += ",'" + litem.Value.ToString() + "'";
                    }

                }
                foreach (System.Web.UI.WebControls.ListItem litem in lstVP.Items)
                {
                    if (litem.Selected)
                    {
                        if (VP.Length == 0)
                            VP = "'" + litem.Value.ToString() + "'";
                        else
                            VP += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in lstGM.Items)
                {
                    if (litem.Selected)
                    {
                        if (GM.Length == 0)
                            GM = "'" + litem.Value.ToString() + "'";
                        else
                            GM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in lstDGM.Items)
                {
                    if (litem.Selected)
                    {
                        if (DGM.Length == 0)
                            DGM = "'" + litem.Value.ToString() + "'";
                        else
                            DGM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in lstRM.Items)
                {
                    if (litem.Selected)
                    {
                        if (RM.Length == 0)
                            RM = "'" + litem.Value.ToString() + "'";
                        else
                            RM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in lstZM.Items)
                {
                    if (litem.Selected)
                    {
                        if (ZM.Length == 0)
                            ZM = "'" + litem.Value.ToString() + "'";
                        else
                            ZM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in lstASM.Items)
                {
                    if (litem.Selected)
                    {
                        if (ASM.Length == 0)
                            ASM = "'" + litem.Value.ToString() + "'";
                        else
                            ASM += ",'" + litem.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in lstEXECUTIVE.Items)
                {
                    if (litem.Selected)
                    {
                        if (EXECUTIVE.Length == 0)
                            EXECUTIVE = "'" + litem.Value.ToString() + "'";
                        else
                            EXECUTIVE += ",'" + litem.Value.ToString() + "'";
                    }
                }

                #endregion
                string SearchCondition;
                SearchCondition = "";
                #region BindSearch Condition
                if (HOS.Length > 0)
                {
                    SearchCondition = "HOSCODE in (" + HOS + ")";
                }
                if (VP.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and VPCODE in (" + VP + ")";
                    else
                        SearchCondition = " VPCODE in (" + VP + ")";
                }
                if (GM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and GMCODE in (" + GM + ")";
                    else
                        SearchCondition = " GMCODE in (" + GM + ")";
                }
                if (DGM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and DGMCODE in (" + DGM + ")";
                    else
                        SearchCondition = " DGMCODE in (" + DGM + ")";
                }
                if (RM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and RMCODE in (" + RM + ")";
                    else
                        SearchCondition = " RMCODE in (" + RM + ")";
                }
                if (ZM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and ZMCODE in (" + ZM + ")";
                    else
                        SearchCondition = " ZMCODE in (" + ZM + ")";
                }
                if (ASM.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and ASMCODE in (" + ASM + ")";
                    else
                        SearchCondition = " ASMMCODE in (" + ASM + ")";
                }
                if (EXECUTIVE.Length > 0)
                {
                    if (SearchCondition.Length > 0)
                        SearchCondition += " and EXECUTIVECODE in (" + EXECUTIVE + ")";
                    else
                        SearchCondition = " EXECUTIVECODE in (" + EXECUTIVE + ")";
                }

                #endregion
                if (SearchCondition.Length > 0)
                {
                    return DtSaleHierarchy.Select(SearchCondition).CopyToDataTable();
                }
                else
                {
                    return DtSaleHierarchy.Clone();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return DtSaleHierarchy;
            }
        }

        public void FillSaleHierarchy()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();// on page load!!!!
                SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[usp_GetSALESHIERARCHY]";

                cmd.Parameters.AddWithValue("@USERCODE", Convert.ToString(System.Web.HttpContext.Current.Session["USERID"]));
                DataTable dt = new DataTable();
                conn.Open();
                dt.Load(cmd.ExecuteReader());
                System.Web.HttpContext.Current.Session["SaleHierarchy"] = dt;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public void FillSaleHierarchy_Active()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();// on page load!!!!
                SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[usp_GetSALESHIERARCHY]";

                cmd.Parameters.AddWithValue("@USERCODE", Convert.ToString(System.Web.HttpContext.Current.Session["USERID"]));
                cmd.Parameters.AddWithValue("@ISACTIVE", "1");
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                System.Web.HttpContext.Current.Session["SaleHierarchy"] = dt;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void BindToDropDownp(ListBox DDL, string strQuery, string display, string val)
        {
            try
            {
                int intTotalRec;
                System.Data.SqlClient.SqlDataAdapter sqlda;
                System.Data.DataSet dsDDLItem;

                GetConnection();
                cmd = new System.Data.SqlClient.SqlCommand(strQuery, conn);
                sqlda = new System.Data.SqlClient.SqlDataAdapter(cmd);
                dsDDLItem = new System.Data.DataSet();
                sqlda.Fill(dsDDLItem);
                intTotalRec = dsDDLItem.Tables[0].Rows.Count;
                for (int i = 0; i <= intTotalRec - 1; i++)
                {
                    DDL.Items.Add(new ListItem(dsDDLItem.Tables[0].Rows[i][display].ToString(), dsDDLItem.Tables[0].Rows[i][val].ToString()));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}