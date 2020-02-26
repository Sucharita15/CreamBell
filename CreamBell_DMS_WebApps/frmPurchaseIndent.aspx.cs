using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPurchaseIndent : System.Web.UI.Page
    {
        string strmessage = string.Empty;
        int slno = 0;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;

        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new App_Code.Global();
        string strQuery = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            BtnAddItem.Focus();
            gridviewRecordNotExist.DataSource = null;
            gridviewRecordNotExist.DataBind();
            Panel1.Visible = false;
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            //BtnAddItem.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(BtnAddItem, null) + ";");
            if (!IsPostBack)
            {
                if (Session["SiteCode"] != null)
                {
                    string siteid = Session["SiteCode"].ToString();
                    if (lblsite.Text == "")
                    {
                        lblsite.Text = siteid;
                    }
                    Session["LineItem"] = null;
                    Session["Exempt_CurVal"] = null;
                }
                if (Session["TEMP"] != null)
                {

                    try
                    {
                        string firstProductExemptValue = string.Empty;
                        string secondProductExemptValue = string.Empty;
                        bool SameProducts = true;
                        List<string> ilist = new List<string>();
                        List<string> litem = new List<string>();
                        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                        string search = "", query = "";
                        DataTable dt = new DataTable();

                        search = Session["TEMP"].ToString();
                        query = "select A.Siteid,A.Indent_No,REPLACE(CONVERT(VARCHAR(11),A.Indent_Date,106), ' ','/') as Indent_Date,Coalesce(A.Status,0) as Status from ax.ACXPURCHINDENTHEADER A  where Siteid='" + lblsite.Text + "' and Indent_No='" + search + "' ";
                        dt = obj.GetData(query);
                        int row = dt.Rows.Count;
                        if (row > 0)
                        {
                            txtIndentNo.Text = dt.Rows[0]["Indent_No"].ToString();
                            txtRequiredDate.Text = dt.Rows[0]["Indent_Date"].ToString();
                            int status = Convert.ToInt16(dt.Rows[0]["Status"].ToString());
                            if (status == 1)
                            {
                                gvDetails.Columns[10].Visible = false;
                                btnConfirm.Enabled = false;
                            }
                            else
                            {
                                gvDetails.Columns[10].Visible = true;
                                btnConfirm.Enabled = true;
                            }
                            btnNew.Enabled = true;
                            btnSave.Enabled = false;
                        }
                        else
                        {
                            ClearAll();
                            btnNew.Enabled = true;
                            btnSave.Enabled = true;
                        }
                        dt.Clear();
                        query = "[ACX_PurchaseIndent_Line]";
                        ilist.Add("@Site_Code"); litem.Add(lblsite.Text);

                        ilist.Add("@indent_No"); litem.Add(txtIndentNo.Text);
                        ilist.Add("@status"); litem.Add("SELECT");
                        dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);
                        
                        firstProductExemptValue = obj.GetScalarValue("SELECT EXEMPT from ax.INVENTTABLE WHERE itemid = '" + dt.Rows[0]["Product_Code"].ToString() + "'");
                        for (int j = 1; j < dt.Rows.Count; j++)
                        {
                            secondProductExemptValue = obj.GetScalarValue("SELECT EXEMPT from ax.INVENTTABLE WHERE itemid = '" + dt.Rows[j]["Product_Code"].ToString() + "'");
                            if(firstProductExemptValue != secondProductExemptValue)
                            {
                                SameProducts = false;
                                break;
                            }
                        }

                        if (dt.Rows.Count > 0 && SameProducts == true)
                        {
                            if(firstProductExemptValue == "1")
                            {
                                rdExempt.Checked = true;
                                rdNonExempt.Checked = false;
                            }
                            else
                            {
                                rdExempt.Checked = false;
                                rdNonExempt.Checked = true;
                            }
                            gvDetails.DataSource = dt;
                            gvDetails.DataBind();
                            GridViewFooterCalculate(dt);
                            rdExempt.Enabled = false;
                            rdNonExempt.Enabled = false;
                            rdoManualEntry.Enabled = false;
                            rdoExcelEntry.Enabled = false;
                        }
                        else if (dt.Rows.Count > 0 && SameProducts == false)
                        {
                            string message = "alert('This indent has both exempted and non-exempted products...');";
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                            //gvDetails.DataSource = null;
                            //gvDetails.DataBind();
                        }

                        else
                        {
                            //dt.Rows.Add(dt.NewRow());
                            //gvDetails.DataSource = dt;
                            //gvDetails.DataBind();
                            //int columncount = gvDetails.Rows[0].Cells.Count;
                            //gvDetails.Rows[0].Cells.Clear();
                            //gvDetails.Rows[0].Cells.Add(new TableCell());
                            //gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;

                            DataRow dr = null;
                            dr = dt.NewRow();
                            dr["Line_No"] = 1;
                            dr["Product_Group"] = string.Empty;
                            dr["Product_Subcategory"] = string.Empty;
                            dr["Product_Code"] = string.Empty;
                            dr["Product_Name"] = string.Empty;
                            dr["Crates"] = 0;
                            dr["Box"] = 0;
                            dr["Ltr"] = 0;
                            dt.Rows.Add(dr);
                            gvDetails.DataSource = dt;
                            gvDetails.DataBind();
                            GridViewFooterCalculate(dt);
                            int count = gvDetails.Rows.Count;
                            for (int i = count; i > 0; i--)
                            {
                                gvDetails.Rows[i - 1].Cells.Clear();
                            }

                        }
                        Session["TEMP"] = null;
                    }
                    catch(Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    finally
                    {

                    }

                }
                string query1 = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                ddlBusinessUnit.Items.Add("All...");
                obj.BindToDropDown(ddlBusinessUnit, query1, "bu_desc", "bu_code");
                DeleteUnSavedData();
                GetSalesOffce();
                BindGridview();
                ProductGroup();
                FillProductCode();

                txtRequiredDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);// DateTime.Today.ToString();

                CalendarExtender1.StartDate = DateTime.Now;

            }
            else
            {
                string StatePriceGroup = "";
                StatePriceGroup = obj.GetScalarValue("SELECT PRICEGROUP from[ax].[inventsite] where siteid = '" + Session["SiteCode"] + "'");
                if(StatePriceGroup == null)
                { StatePriceGroup = ""; }
                if (StatePriceGroup.Length == 0)
                {
                    StatePriceGroup = obj.GetScalarValue("SELECT TOP 1 ACX_PRICEGROUP FROM AX.LOGISTICSADDRESSSTATE WHERE STATEID = (SELECT STATECODE FROM AX.INVENTSITE WHERE SITEID = '" + Session["SiteCode"] + "')");
                }
                StatePriceGroup = Convert.ToString(StatePriceGroup).Trim() == "" ? "-" : StatePriceGroup;
                DataTable dtPriceList = obj.GetData("SELECT * FROM DBO.ACX_UDF_GETPRICE(GETDATE(),'" + StatePriceGroup + "','')");
                foreach (ListItem item in DDLMaterialCode.Items)
                {
                    DataRow[] dr = dtPriceList.Select("ITEMRELATION='" + item.Value.ToString() + "' AND AMOUNT>0");
                    if (dr.Length == 0)
                    {
                        item.Attributes.Add("style", "background-color:red;color:white;font-weight:bold;");
                    }

                }
            }
        }


        private void DeleteUnSavedData()
        {
            try
            {
                DataTable dt = new DataTable();
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string query = "Delete FROM ax.ACXPURCHINDENTLINE where SITEID='" + lblsite.Text + "' and Indent_No=''";
                obj.ExecuteCommand(query);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void GetSalesOffce()
        {
            try
            {
                string query;
                //test
                // query = "select A.[SALEOFFICE_CODE] ,A.[SALEOFFICE_NAME] from ax.ACXSITEMASTER A Where A.SITEID='" + lblsite.Text + "'";
                //query = "select A.[ACXPLANTCODE],A.[ACXPLANTNAME] from ax.inventsite A Where A.SITEID='" + lblsite.Text + "' Or A.SITEID IN (SELECT DISTINCT OTHER_SITE FROM AX.ACX_SDLinking WHERE SubDistributor='" + lblsite.Text + "')";
                //Change by Sushil Get the site name of linked sub distributor code and the plant name
                //query = "select CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTCODE] ELSE A.[ACXPLANTNAME] END [ACXPLANTCODE], CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTNAME] ELSE (SELECT TOP 1 NAME FROM AX.INVENTSITE WHERE SITEID=A.ACXPLANTNAME) END [ACXPLANTNAME] from ax.inventsite A Where A.SITEID='" + lblsite.Text + "' union select SITEID,NAME from AX.INVENTSITE A Where A.SITEID IN (SELECT DISTINCT OTHER_SITE FROM AX.ACX_SDLinking WHERE SubDistributor='" + lblsite.Text + "' AND BLOCKED=0)";
                query = "select CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTCODE] ELSE A.[ACXPLANTNAME] END [ACXPLANTCODE], CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTNAME] ELSE (SELECT TOP 1 NAME FROM AX.INVENTSITE WHERE SITEID=A.ACXPLANTNAME) END [ACXPLANTNAME] from ax.inventsite A Where A.SITEID='" + lblsite.Text + "' union select SITEID,NAME from AX.INVENTSITE A Where A.SITEID IN (SELECT DISTINCT OTHER_SITE FROM AX.ACX_SDLinking WHERE SubDistributor='" + lblsite.Text + "' AND BLOCKED=0)";
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                DataTable dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    drpSalesOff.Items.Clear();
                    drpSalesOff.DataSource = dt;
                    drpSalesOff.DataTextField = "ACXPLANTNAME";
                    drpSalesOff.DataValueField = "ACXPLANTCODE";
                    drpSalesOff.DataBind();
                    txtSalesOfficeCode.Text = dt.Rows[0]["ACXPLANTCODE"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void BindGridview()
        {
            try
            {
                string query;
                query = "ACX_PurchaseIndent_Line";
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                DataTable dt = new DataTable();
                List<string> ilist = new List<string>();
                List<string> item = new List<string>();
                ilist.Add("@Site_Code");
                // item.Add(Session["SiteCode"].ToString());
                item.Add(lblsite.Text);
                ilist.Add("@indent_No");
                item.Add(txtIndentNo.Text.Trim());
                ilist.Add("@status");
                item.Add("SELECT");
                dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    GridViewFooterCalculate(dt);
                }
                else
                {
                    DataRow dr = null;
                    dr = dt.NewRow();
                    dr["Line_No"] = 1;
                    dr["Product_Group"] = string.Empty;
                    dr["Product_Code"] = string.Empty;
                    dr["Product_SubCategory"] = string.Empty;
                    dr["Product_Name"] = string.Empty;
                    dr["Crates"] = 0;
                    dr["Box"] = 0;
                    dr["Ltr"] = 0;
                    dt.Rows.Add(dr);
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    GridViewFooterCalculate(dt);
                    int count = gvDetails.Rows.Count;
                    for (int i = count; i > 0; i--)
                    {
                        gvDetails.Rows[i - 1].Cells.Clear();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ClearAll();
            btnConfirm.Enabled = false;
            btnSave.Enabled = true;
            gvDetails.Columns[10].Visible = true;

            int count = gvDetails.Rows.Count;
            for (int i = count; i > 0; i--)
            {
                gvDetails.Rows[i - 1].Cells.Clear();
            }
            BindGridview();
            Session["LineItem"] = null;
            Session["Exempt_Curval"] = null;
        }

        public void ClearAll()
        {
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            Session["LineItem"] = null;
            Session["Exempt_Curval"] = null;
            btnNew.Enabled = true;
            //btnConfirm.Enabled = false;       //Commmented By rahul 
            //btnSave.Enabled = false;          //Commented By Rahul
            btnConfirm.Enabled = true;
            btnSave.Enabled = true;
            rdExempt.Enabled = true;
            rdNonExempt.Enabled = true;
            rdoManualEntry.Enabled = true;
            rdoExcelEntry.Enabled = true;
            rdoManualEntry.Checked = true;
            rdoExcelEntry.Checked = false;
            rdExempt.Checked = false;
            rdNonExempt.Checked = true;
            txtSerch.Text = "";
            txtIndentNo.Text = "";
            txtRequiredDate.Text = "";
            txtRequiredDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);// DateTime.Today.ToString();
            int count = gvDetails.Rows.Count;
            for (int i = count; i > 0; i--)
            {
                gvDetails.Rows[i - 1].Cells.Clear();
            }
            ddlBusinessUnit.SelectedIndex = 0;
            ProductGroup();
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("--Select--");
            DDLMaterialCode.Items.Clear();
            DDLMaterialCode.Items.Add("--Select--");
            FillProductCode();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtSalesOfficeCode.Text == "")
            {
                string query = "";
                query = "select distinct(A.SITEID) as Site_Code from ax.inventsite A where A.Name='" + drpSalesOff.SelectedItem.Text + "'";
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                DataTable dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    txtSalesOfficeCode.Text = dt.Rows[0]["Site_Code"].ToString();
                }
            }
            if (gvDetails.Rows.Count <= 0)
            {
                string message = "alert('Please Add At least One Product!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                return;
            }
            //=================Start Validation=============
            if (gvDetails.Rows.Count > 0)
            {
                Label PG = (Label)gvDetails.Rows[0].Cells[1].FindControl("Product_GroupCode");
                if (PG.Text == "")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Product Group!!');", true);
                    string message = "alert('Please Enter Product Group!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    return;
                }
                Label PN = (Label)gvDetails.Rows[0].Cells[2].FindControl("Product_Name");
                if (PN.Text == "")
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Product Name!!');", true);
                    string message = "alert('Please Enter Product Name!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
                Label box = (Label)gvDetails.Rows[0].Cells[3].FindControl("Box");
                if (box.Text == "0")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Box Qty!!');", true);
                    string message = "alert('Please Enter Box Qty!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
            }
            if (txtRequiredDate.Text == "")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter the Valid Date!!');", true);                
                string message = "alert('Please Enter the Valid Date!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                return;
            }
            //================End Validation=======================

            string strIndentNo = IndentNo();
            if (strmessage == string.Empty)
            {
                txtIndentNo.Text = strIndentNo;
                SaveHeader();
                slno = 0;
                btnConfirm.Enabled = true;
                btnNew.Enabled = true;
                btnSave.Enabled = false;
                string message = "alert('Indent: " + txtIndentNo.Text + " Saved Successfully!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                Session["LineItem"] = null;
                Session["Exempt_Curval"] = null;
            }
            else
            {
                btnConfirm.Enabled = false;
                btnNew.Enabled = true;
                btnSave.Enabled = true;
                ClearAll();
            }
            //}
        }

        public void SaveHeader()
        {
            try
            {
                {
                    if (Session["LineItem"] != null)
                    {
                        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                        conn = obj.GetConnection();
                        DataTable dtNumSeq = obj.GetNumSequenceNew(0, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());

                        string NUMSEQ = string.Empty;
                        if (dtNumSeq != null)
                        {
                            txtIndentNo.Text = dtNumSeq.Rows[0][0].ToString();
                            NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                        }
                        else
                        {
                            return;
                        }

                        cmd1 = new SqlCommand("[dbo].[ACX_PurchaseIndent_Line]");
                        transaction = conn.BeginTransaction();
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;

                        //=============Save Line=============
                        int i = 0;
                        string status = "INSERT";
                        foreach (GridViewRow grv in gvDetails.Rows)
                        {

                            Label drpPrGr = (Label)grv.Cells[1].FindControl("Product_GroupCode");
                            string PrGr = drpPrGr.Text;
                            Label drpPrCode = (Label)grv.Cells[3].FindControl("Product_Code");
                            string PrCode = drpPrCode.Text;
                            Label Box = (Label)grv.Cells[4].FindControl("Box");
                            Label Crates = (Label)grv.Cells[5].FindControl("Crates");
                            Label Ltr = (Label)grv.Cells[6].FindControl("Ltr");

                            if (status == "INSERT")
                            {

                                i = i + 1;
                                cmd1.Parameters.Clear();
                                cmd1.Parameters.AddWithValue("@Line_No", i);
                                cmd1.Parameters.AddWithValue("@Site_Code", lblsite.Text);
                                cmd1.Parameters.AddWithValue("@indent_No", txtIndentNo.Text);
                                cmd1.Parameters.AddWithValue("@Product_GroupCode", PrGr);
                                cmd1.Parameters.AddWithValue("@Product_Code", PrCode);
                                cmd1.Parameters.AddWithValue("@Crates", Convert.ToDecimal(Crates.Text));
                                cmd1.Parameters.AddWithValue("@Box", Convert.ToDecimal(Box.Text));
                                cmd1.Parameters.AddWithValue("@Ltr", Convert.ToDecimal(Ltr.Text));
                                cmd1.Parameters.AddWithValue("@status", "INSERT");
                                cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());

                                cmd1.ExecuteNonQuery();
                            }
                            

                        }
                        //==============Save Header=========

                        cmd = new SqlCommand("USP_ACX_PurchaseIndent_Header");
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@indent_No", txtIndentNo.Text);
                        cmd.Parameters.AddWithValue("@Required_Date", txtRequiredDate.Text);
                        cmd.Parameters.AddWithValue("@status", "INSERT");
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                        cmd.Parameters.AddWithValue("@ACXPLANTNAME", drpSalesOff.SelectedValue.ToString());

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    else
                    {
                        string message = "alert('Please Add Line Items First !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string message = "alert('Error:" + ex.Message + " !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }

        public string IndentNo()
        {
            try
            {
                string IndNo = string.Empty;
                string Number = string.Empty;
                int intTotalRec;

                string strQuery = "Select ISNULL(MAX(CAST(RIGHT(Indent_No,8) AS INT)),0)+1 as new_IndentNo from ax.ACXPURCHINDENTHEADER A  where [Siteid]='" + lblsite.Text + "'";
                DataTable dt = new DataTable();
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                dt = obj.GetData(strQuery);
                intTotalRec = dt.Rows.Count;

                if (dt.Rows[0]["new_IndentNo"].ToString() != "0")
                {
                    string st = dt.Rows[0]["new_IndentNo"].ToString();
                    if (st.Length < 10)
                    {
                        int len = st.Length;
                        int plen = 10 - len;
                        for (int i = 0; i < plen; i++)
                        {
                            st = "0" + st;
                        }
                        IndNo = st;
                        return IndNo;
                    }
                }
                else
                {
                    string st = "1";
                    if (st.Length < 10)
                    {
                        int len = st.Length;
                        int plen = 10 - len;
                        for (int i = 0; i < plen; i++)
                        {
                            st = "0" + st;
                        }
                        IndNo = st;
                        return IndNo;
                    }

                }

                return IndNo;

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                strmessage = ex.Message.ToString();
                return strmessage;
            }

        }

        public string IndentNo_Old()
        {
            try
            {
                string IndNo = string.Empty;
                string Number = string.Empty;
                int intTotalRec;
                string strQuery = "Select top 1 coalesce(substring(A.Indent_No,7,4),0) as new_IndentNo from ax.ACXPURCHINDENTHEADER A where [Siteid]='" + Session["SiteCode"].ToString() + "' order by createddatetime desc";
                //string strQuery = "Select coalesce(max(Indent_No),0) as new_IndentNo from ax.ACXPURCHINDENTHEADER where [Siteid]='" + Session["SiteCode"].ToString() + "'";                
                DataTable dt = new DataTable();
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                dt = obj.GetData(strQuery);
                intTotalRec = dt.Rows.Count;
                if (intTotalRec > 0)
                {

                    if (dt.Rows[0]["new_IndentNo"].ToString() != "0")
                    {
                        string st = dt.Rows[0]["new_IndentNo"].ToString();
                        if (st.Length < 10)
                        {
                            int len = st.Length;
                            int plen = 10 - len;
                            for (int i = 0; i < plen; i++)
                            {
                                st = "0" + st;
                            }
                        }
                        Number = st.Substring(6, 4);
                        int intnumber = Convert.ToInt32(Number) + 1;
                        Number = intnumber.ToString().PadLeft(4, '0');
                        st = Session["SiteCode"].ToString();
                        if (st.Length < 10)
                        {
                            int len = st.Length;
                            int plen = 10 - len;
                            for (int i = 0; i < plen; i++)
                            {
                                st = "0" + st;
                            }
                        }
                        IndNo = st.Substring(4, 6) + Number;
                        return IndNo;
                    }
                    else
                    {
                        int intnumber = 1;
                        Number = intnumber.ToString().PadLeft(4, '0');
                        string st = Session["SiteCode"].ToString();
                        if (st.Length < 10)
                        {
                            int len = st.Length;
                            int plen = 10 - len;
                            for (int i = 0; i < plen; i++)
                            {
                                st = "0" + st;
                            }
                        }
                        IndNo = st.Substring(4, 6) + Number;
                        return IndNo;
                    }
                }
                else
                {
                    int intnumber = 1;
                    Number = intnumber.ToString().PadLeft(4, '0');
                    string st = Session["SiteCode"].ToString();
                    if (st.Length < 10)
                    {
                        int len = st.Length;
                        int plen = 10 - len;
                        for (int i = 0; i < plen; i++)
                        {
                            st = "0" + st;
                        }
                    }
                    IndNo = st.Substring(4, 6) + Number;
                    return IndNo;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                strmessage = ex.Message.ToString();
                return strmessage;
            }

        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtIndentNo.Text != string.Empty)
            {

                string firstProductExemptValue = string.Empty;
                string secondProductExemptValue = string.Empty;
                bool SameProducts = true;
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string query = "";
                DataTable dt = new DataTable();
                query = "[ACX_PurchaseIndent_Line]";
                ilist.Add("@Site_Code"); litem.Add(lblsite.Text);

                ilist.Add("@indent_No"); litem.Add(txtIndentNo.Text);
                ilist.Add("@status"); litem.Add("SELECT");
                dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);

                firstProductExemptValue = obj.GetScalarValue("SELECT EXEMPT from ax.INVENTTABLE WHERE itemid = '" + dt.Rows[0]["Product_Code"].ToString() + "'");
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    secondProductExemptValue = obj.GetScalarValue("SELECT EXEMPT from ax.INVENTTABLE WHERE itemid = '" + dt.Rows[j]["Product_Code"].ToString() + "'");
                    if (firstProductExemptValue != secondProductExemptValue)
                    {
                        SameProducts = false;
                        break;
                    }
                }
                if (dt.Rows.Count > 0 && SameProducts == false)
                {
                    string message = "alert('This indent will not be confirmed because it has both exempted and non-exempted products...');";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    return;
                }

                else
                {
                    string query1 = "Update ax.ACXPURCHINDENTHEADER set Status=1 where Indent_No='" + txtIndentNo.Text + "' and Siteid='" + Session["SiteCode"].ToString() + "'";
                    //CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                    int row = obj.ExecuteCommand(query1);
                    if (row > 0)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Indent No: " + txtIndentNo.Text + " hase been confirmed .');", true);
                        string message = "alert('Indent No: " + txtIndentNo.Text + " hase been confirmed .');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    }
                }   
            }
            else
            {
                btnSave_Click(sender, e);
                string query = "Update ax.ACXPURCHINDENTHEADER set Status=1 where Indent_No='" + txtIndentNo.Text + "' and Siteid='" + Session["SiteCode"].ToString() + "'";
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                int row = obj.ExecuteCommand(query);
                //if (row > 0)
                //{
                //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Indent No: " + txtIndentNo.Text + " hase been confirmed .');", true);
                //}
            }
            ClearAll();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string search = "", query = "";
                DataTable dt = new DataTable();

                search = txtSerch.Text;
                query = "select A.Siteid,A.Indent_No,REPLACE(CONVERT(VARCHAR(11),A.Indent_Date,106), ' ','/') as Indent_Date,Coalesce(A.Status,0) as Status from ax.ACXPURCHINDENTHEADER A  where Siteid='" + lblsite.Text + "' and Indent_No='" + search + "' ";
                dt = obj.GetData(query);
                int row = dt.Rows.Count;
                if (row > 0)
                {
                    txtIndentNo.Text = dt.Rows[0]["Indent_No"].ToString();
                    txtRequiredDate.Text = dt.Rows[0]["Indent_Date"].ToString();
                    int status = Convert.ToInt16(dt.Rows[0]["Status"].ToString());
                    if (status == 1)
                    {
                        gvDetails.Columns[10].Visible = false;
                        btnConfirm.Enabled = false;
                    }
                    else
                    {
                        gvDetails.Columns[10].Visible = true;
                        btnConfirm.Enabled = true;
                    }
                    btnNew.Enabled = true;
                    btnSave.Enabled = false;
                }
                else
                {
                    ClearAll();
                    btnNew.Enabled = true;
                    btnSave.Enabled = true;
                }
                dt.Clear();
                query = "[ACX_PurchaseIndent_Line]";
                ilist.Add("@Site_Code"); litem.Add(lblsite.Text);

                ilist.Add("@indent_No"); litem.Add(txtIndentNo.Text);
                ilist.Add("@status"); litem.Add("SELECT");
                dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    GridViewFooterCalculate(dt);
                }
                else
                {
                    //dt.Rows.Add(dt.NewRow());
                    //gvDetails.DataSource = dt;
                    //gvDetails.DataBind();
                    //int columncount = gvDetails.Rows[0].Cells.Count;
                    //gvDetails.Rows[0].Cells.Clear();
                    //gvDetails.Rows[0].Cells.Add(new TableCell());
                    //gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;

                    DataRow dr = null;
                    dr = dt.NewRow();
                    dr["Line_No"] = 1;
                    dr["Product_Group"] = string.Empty;
                    dr["Product_Subcategory"] = string.Empty;
                    dr["Product_Code"] = string.Empty;
                    dr["Product_Name"] = string.Empty;
                    dr["Crates"] = 0;
                    dr["Box"] = 0;
                    dr["Ltr"] = 0;
                    dt.Rows.Add(dr);
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    GridViewFooterCalculate(dt);
                    int count = gvDetails.Rows.Count;
                    for (int i = count; i > 0; i--)
                    {
                        gvDetails.Rows[i - 1].Cells.Clear();
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }

        protected void Product_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;
            string query;

            //query = "select distinct(Product_Code) as Product_Code,Product_Name from ax.ACXPRODUCTMASTER where Product_Group='" + ddl.SelectedItem.Value + "' order by Product_Name";
            query = "select distinct(ITEMID) as Product_Code,Product_Name from ax.INVENTTABLE where BLOCK=0 AND Product_Group='" + ddl.SelectedItem.Value + "' order by Product_Name";
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            dt = obj.GetData_New(query, CommandType.Text, ilist, litem);

            DropDownList ddlPrCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code");
            DropDownList ddlPrName = (DropDownList)row.Cells[0].FindControl("drpProduct_Name");

            ddlPrCode.DataSource = dt;
            ddlPrCode.DataTextField = "Product_Code";
            ddlPrCode.DataValueField = "Product_Code";
            ddlPrCode.DataBind();
            ddlPrCode.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlPrName.DataSource = dt;
            ddlPrName.DataTextField = "Product_Name";
            ddlPrName.DataValueField = "Product_Code";
            ddlPrName.DataBind();
            ddlPrName.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        protected void Product_Group_Entry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;
            string query;
            TextBox txtBox = (TextBox)row.Cells[0].FindControl("txtBox_Entry");
            txtBox.Text = string.Empty;
            TextBox txtCrate = (TextBox)row.Cells[0].FindControl("txtCrates_Entry");
            txtCrate.Text = string.Empty;

            //query = "select distinct(Product_Code) as Product_Code,concat(Product_Code,' - ',Product_Name) as Product_Name from ax.ACXPRODUCTMASTER where Product_Group='" + ddl.SelectedItem.Value + "' order by Product_Name";
            //query = "select distinct(PRODUCT_SUBCATEGORY) as PRODUCT_SUBCATEGORY from ax.ACXPRODUCTMASTER where Product_Group='" + ddl.SelectedItem.Value + "' order by PRODUCT_SUBCATEGORY";
            query = "select distinct(PRODUCT_SUBCATEGORY) as PRODUCT_SUBCATEGORY from ax.INVENTTABLE where BLOCK=0  AND Product_Group='" + ddl.SelectedItem.Value + "' order by PRODUCT_SUBCATEGORY";
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            //dt = obj.GetData_New(query, CommandType.Text, ilist, litem);
            dt = obj.GetData(query);

            DropDownList ddlPrCat = (DropDownList)row.Cells[2].FindControl("drpProduct_SubCategory_Entry");

            ddlPrCat.DataSource = dt;
            ddlPrCat.DataTextField = "PRODUCT_SUBCATEGORY";
            ddlPrCat.DataValueField = "PRODUCT_SUBCATEGORY";
            ddlPrCat.DataBind();
            ddlPrCat.Items.Insert(0, new ListItem("--Select--", "0"));
            //obj.BindToDropDown(ddlPrCat, query, "PRODUCT_SUBCATEGORY", "PRODUCT_SUBCATEGORY");

            //DropDownList ddlPrCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code_Entry");
            //DropDownList ddlPrName = (DropDownList)row.Cells[0].FindControl("drpProduct_Name_Entry");


            //ddlPrCode.DataSource = dt;
            //ddlPrCode.DataTextField = "Product_Code";
            //ddlPrCode.DataValueField = "Product_Code";
            //ddlPrCode.DataBind();
            //ddlPrCode.Items.Insert(0, new ListItem("--Select--", "0"));
            //ddlPrName.DataSource = dt;
            //ddlPrName.DataTextField = "Product_Name";
            //ddlPrName.DataValueField = "Product_Code";
            //ddlPrName.DataBind();
            //ddlPrName.Items.Insert(0, new ListItem("--Select--", "0"));

            //ddlPrName.Focus();

            // ddl.Attributes.Add("SelectedIndexChanged", " this.disabled = true; " + ClientScript.GetPostBackEventReference(ddl, null) + ";");
        }

        protected void drpProduct_Code_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;

            DropDownList ddlPrGr = (DropDownList)row.Cells[0].FindControl("drpProduct_Group");
            DropDownList ddlPrName = (DropDownList)row.Cells[0].FindControl("drpProduct_Name");

            string a = ddl.SelectedItem.Text;
            ddlPrName.SelectedIndex = ddlPrName.Items.IndexOf(ddlPrName.Items.FindByValue(a));
        }

        protected void drpProduct_Code_Entry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;

            DropDownList ddlPrGr = (DropDownList)row.Cells[0].FindControl("drpProduct_Group_Entry");
            DropDownList ddlPrName = (DropDownList)row.Cells[0].FindControl("drpProduct_Name_Entry");

            string a = ddl.SelectedItem.Text;
            ddlPrName.SelectedIndex = ddlPrName.Items.IndexOf(ddlPrName.Items.FindByValue(a));

        }

        protected void drpProduct_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;

            DropDownList ddlPrGr = (DropDownList)row.Cells[0].FindControl("drpProduct_Group");
            DropDownList ddlPrCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code");

            string a = ddl.SelectedItem.Value;
            ddlPrCode.SelectedIndex = ddlPrCode.Items.IndexOf(ddlPrCode.Items.FindByValue(a));
        }

        protected void drpProduct_Name_Entry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;

            DropDownList ddlPrGr = (DropDownList)row.Cells[0].FindControl("drpProduct_Group_Entry");
            DropDownList ddlPrCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code_Entry");
            DropDownList drpProductSubCat = (DropDownList)row.Cells[0].FindControl("drpProduct_SubCategory_Entry");

            //==============If ProductCode is select 1st then fill the Product group and sub category===========
            if (ddlPrGr.SelectedItem.Text == "--Select--" && drpProductSubCat.SelectedItem.Text == "--Select--")
            {
                string query = "select Product_Group,PRODUCT_SUBCATEGORY  from ax.INVENTTABLE where ItemId='" + ddl.SelectedItem.Value + "' order by Product_Name";
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    ddlPrGr.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                    //=============For Product Sub Cat======
                    drpProductSubCat.DataSource = dt;
                    drpProductSubCat.DataTextField = "PRODUCT_SUBCATEGORY";
                    drpProductSubCat.DataValueField = "PRODUCT_SUBCATEGORY";
                    drpProductSubCat.DataBind();
                }
            }
            //==============================================

            string a = ddl.SelectedItem.Value;
            ddlPrCode.SelectedIndex = ddlPrCode.Items.IndexOf(ddlPrCode.Items.FindByValue(a));

            for (int i = 0; i < gvDetails.Rows.Count; i++)
            {
                Label lblPrCode = (Label)gvDetails.Rows[i].FindControl("Product_Code");
                string PrCode1 = lblPrCode.Text;
                if (PrCode1 == ddlPrCode.SelectedItem.Value)
                {
                    ////this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + ddl.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');", true);
                    string message = "alert('" + ddl.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    TextBox txtBox = (TextBox)row.Cells[0].FindControl("txtBox_Entry");
                    txtBox.Text = string.Empty;
                    txtBox.ReadOnly = true;
                    TextBox txtCrate = (TextBox)row.Cells[0].FindControl("txtCrates_Entry");
                    txtCrate.Text = string.Empty;
                    return;
                }
                else
                {
                    TextBox txtBox = (TextBox)row.Cells[0].FindControl("txtBox_Entry");
                    txtBox.Text = string.Empty;
                    TextBox txtCrate = (TextBox)row.Cells[0].FindControl("txtCrates_Entry");
                    txtCrate.Text = string.Empty;
                    txtBox.ReadOnly = false;
                    txtBox.Focus();
                }
            }
            //=================================
            TextBox lblBox = (TextBox)row.Cells[0].FindControl("txtBox_Entry");
            TextBox lblCrate = (TextBox)row.Cells[0].FindControl("txtCrates_Entry");
            DropDownList drpEntryType = (DropDownList)row.Cells[0].FindControl("drpEntryType");
            if (drpEntryType.SelectedItem.Text == "BOX")
            {
                lblCrate.ReadOnly = true;
                lblBox.ReadOnly = false;
                lblBox.Focus();
            }
            else if (drpEntryType.SelectedItem.Text == "CRATE")
            {
                lblBox.ReadOnly = true;
                lblCrate.ReadOnly = false;
                lblCrate.Focus();
            }
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDetails.EditIndex = e.NewEditIndex;
            BindGridview();
        }

        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDetails.EditIndex = -1;
            BindGridview();
        }

        //protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvDetails.PageIndex = e.NewPageIndex;
        //    BindGridview();
        //}

        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DropDownList drpPrGr = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("drpProduct_Group"); ;
            string PrGr = drpPrGr.SelectedItem.Value;
            DropDownList drpPrCode = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("drpProduct_Code");
            string PrCode = drpPrCode.SelectedItem.Value;
            DropDownList drpPrName = (DropDownList)gvDetails.Rows[e.RowIndex].FindControl("drpProduct_Name");
            string PrName = drpPrName.SelectedItem.Value;
            TextBox Box = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtBox");
            TextBox Crates = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtCrates");
            TextBox Ltr = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtLtr");
            Label Line_no = (Label)gvDetails.Rows[e.RowIndex].FindControl("Line_No");
            slno = Convert.ToInt16(Line_no.Text);

            crudoperations("UPDATE", PrGr, PrCode, PrName, Crates.Text, Box.Text, Ltr.Text, slno);
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label Lineno = (Label)gvDetails.Rows[e.RowIndex].FindControl("Line_No");
            Label Product_Code = (Label)gvDetails.Rows[e.RowIndex].FindControl("Product_Code");

            if (txtIndentNo.Text != "")
            {
                slno = Convert.ToInt16(Lineno.Text);
                crudoperations("DELETE", "", "", "", "", "", "", slno);
            }
            else
            {
                if (Session["LineItem"] != null)
                {
                    DataTable dt = Session["LineItem"] as DataTable;
                    dt.AsEnumerable().Where(r => r.Field<string>("Product_Code") == (Product_Code.Text)).ToList().ForEach(row => row.Delete());
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    Session["LineItem"] = dt;
                    //========For refresh the grid after delete a record======
                    Session["LineItem"] = null;

                    int rowno = 0;
                    dt.Clear();
                    // DataTable dtLineItem = new DataTable();
                    foreach (GridViewRow grv in gvDetails.Rows)
                    {
                        Label PrGr = (Label)grv.Cells[1].FindControl("Product_GroupCode");
                        Label PrCat = (Label)grv.Cells[2].FindControl("Product_SubCategory");
                        Label PrCode = (Label)grv.Cells[3].FindControl("Product_Code");
                        Label PrName = (Label)grv.Cells[5].FindControl("Product_Name");
                        Label Box = (Label)grv.Cells[6].FindControl("Box");
                        Label StockQty = (Label)grv.Cells[6].FindControl("Stock_Qty");
                        Label Crates = (Label)grv.Cells[7].FindControl("Crates");
                        Label Ltr = (Label)grv.Cells[8].FindControl("Ltr");
                        Label lblProductCode = (Label)grv.Cells[4].FindControl("lblProductCode");
                        rowno = rowno + 1;

                        DataRow row = null;
                        row = dt.NewRow();

                        // row = dtLineItems.NewRow();
                        row["Line_No"] = rowno;
                        row["Product_Group"] = PrGr.Text;
                        row["Product_SubCategory"] = PrCat.Text;
                        row["Product_Code"] = PrCode.Text;
                        row["Product_Name"] = PrName.Text;
                        row["Stock_Qty"] = StockQty.Text;
                        row["Crates"] = Convert.ToDecimal(Crates.Text.Trim().ToString());
                        row["Box"] = Convert.ToDecimal(Box.Text.Trim().ToString());
                        row["Ltr"] = Convert.ToDecimal(Ltr.Text.Trim().ToString());
                        row["lblProductCode"] = lblProductCode.Text.Trim().ToString();

                        dt.Rows.Add(row);
                    }
                    Session["LineItem"] = dt;
                    GridViewFooterCalculate(dt);
                }
            }
            DataTable dt11 = new DataTable();
            dt11 = (DataTable)Session["LineItem"];
            if (dt11 != null)
            {
                if (dt11.Rows.Count == 0)
                {
                    rdExempt.Enabled = true;
                    rdNonExempt.Enabled = true;
                    Session["Exempt_CurVal"] = null;
                }
            }
        }

        protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    try
            //    {
            //        //=================EntryType=======
            //        DropDownList drpEntry = (DropDownList)e.Row.FindControl("drpEntryType");
            //        drpEntry.Items.Insert(0, new ListItem("BOX", "0"));
            //        drpEntry.Items.Insert(1, new ListItem("CRATE", "1"));
            //        drpEntry.SelectedIndex = 0;
            //        TextBox txtBox = (TextBox)e.Row.FindControl("txtBox_Entry");
            //        TextBox txtCrate = (TextBox)e.Row.FindControl("txtCrates_Entry");
            //        txtBox.ReadOnly = false;
            //        txtCrate.ReadOnly = true;
            //        //============Material Group
            //        DropDownList ddl = (DropDownList)e.Row.FindControl("drpProduct_Group_Entry");
            //        DataTable dt = new DataTable();
            //        List<string> ilist = new List<string>();
            //        List<string> litem = new List<string>();                    
            //        string query;

            //        //query = "select distinct(Product_Group) as Product_Group from ax.ACXPRODUCTMASTER order by Product_Group";
            //        query = "select distinct(Product_Group) as Product_Group from ax.INVENTTABLE order by Product_Group";
            //        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            //        dt = obj.GetData_New(query, CommandType.Text, ilist, litem);

            //        ddl.DataSource = dt;
            //        ddl.DataTextField = "Product_Group";
            //        ddl.DataValueField = "Product_Group";
            //        ddl.DataBind();
            //        ddl.Items.Insert(0, new ListItem("--Select--", "0"));

            //        //===========Product SubCategory=======
            //        DropDownList subCat = (DropDownList)e.Row.FindControl("drpProduct_SubCategory_Entry");
            //        dt = new DataTable();

            //       // query = "select distinct(PRODUCT_SUBCATEGORY) as PRODUCT_SUBCATEGORY from ax.ACXPRODUCTMASTER  where Product_Group='" + ddl.SelectedItem.Value + "' order by PRODUCT_SUBCATEGORY";                   
            //        query = "select distinct(PRODUCT_SUBCATEGORY) as PRODUCT_SUBCATEGORY from ax.INVENTTABLE where Product_Group='" + ddl.SelectedItem.Value + "' order by PRODUCT_SUBCATEGORY";                   
            //        dt = obj.GetData_New(query, CommandType.Text, ilist, litem);

            //        subCat.DataSource = dt;
            //        subCat.DataTextField = "PRODUCT_SUBCATEGORY";
            //        subCat.DataValueField = "PRODUCT_SUBCATEGORY";
            //        subCat.DataBind();
            //        subCat.Items.Insert(0, new ListItem("--Select--", "0"));

            //        //============Material Code

            //        DropDownList ddlProduct = (DropDownList)e.Row.FindControl("drpProduct_Code_Entry");

            //        //============Material Name

            //        DropDownList ddlPName = (DropDownList)e.Row.FindControl("drpProduct_Name_Entry");
            //        query = "";
            //        //query = "select distinct(Product_Code) as Product_Code,concat(Product_Code,' - ',Product_Name) as Product_Name from ax.ACXPRODUCTMASTER where Product_Group='" + ddl.SelectedItem.Value + "' and PRODUCT_SUBCATEGORY='" + subCat.SelectedItem.Value + "' order by Product_Name";
            //        if (ddl.SelectedItem.Text == "--Select--" && subCat.SelectedItem.Text == "--Select--")
            //        {
            //            query = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE  order by Product_Name";
            //        }
            //        else
            //        {
            //            query = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE where Product_Group='" + ddl.SelectedItem.Value + "' and PRODUCT_SUBCATEGORY='" + subCat.SelectedItem.Value + "' order by Product_Name";
            //        }

            //        dt = new DataTable();
            //        dt = obj.GetData_New(query, CommandType.Text, ilist, litem);

            //        ddlProduct.DataSource = dt;
            //        ddlProduct.DataTextField = "Product_Code";
            //        ddlProduct.DataValueField = "Product_Code";
            //        ddlProduct.DataBind();
            //        ddlProduct.Items.Insert(0, new ListItem("--Select--", "0"));                   

            //        ddlPName.DataSource = dt;
            //        ddlPName.DataTextField = "Product_Name";
            //        ddlPName.DataValueField = "Product_Code";
            //        ddlPName.DataBind();
            //        ddlPName.Items.Insert(0, new ListItem("--Select--", "0"));                   

            //    }
            //    catch
            //    {

            //    }
            //    finally
            //    {

            //    }
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddList = (DropDownList)e.Row.FindControl("drpProduct_Group");
                    DataTable dt = new DataTable();
                    List<string> ilist = new List<string>();
                    List<string> litem = new List<string>();
                    string query;

                    //==========Material Group
                    // query = "select distinct(Product_Group) as Product_Group from ax.ACXPRODUCTMASTER order by Product_Group";
                    query = "select distinct(Product_Group) as Product_Group from ax.INVENTTABLE WHERE BLOCK=0 order by Product_Group";
                    CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                    dt = obj.GetData_New(query, CommandType.Text, ilist, litem);

                    ddList.DataSource = dt;
                    ddList.DataTextField = "Product_Group";
                    ddList.DataValueField = "Product_Group";
                    ddList.DataBind();

                    DataRowView dr = e.Row.DataItem as DataRowView;
                    string PGR = dr.Row.ItemArray[1].ToString();
                    ddList.SelectedIndex = ddList.Items.IndexOf(ddList.Items.FindByText(PGR));

                    //==============SubCategory=======
                    DropDownList subCat = (DropDownList)e.Row.FindControl("Product_SubCategory");
                    query = "";
                    // query = "select distinct(PRODUCT_SUBCATEGORY) as PRODUCT_SUBCATEGORY from ax.ACXPRODUCTMASTER where Product_Group='" + ddList.SelectedItem.Value + "' order by PRODUCT_SUBCATEGORY";
                    query = "select distinct(PRODUCT_SUBCATEGORY) as PRODUCT_SUBCATEGORY from ax.INVENTTABLE where BLOCK=0 AND Product_Group='" + ddList.SelectedItem.Value + "' order by PRODUCT_SUBCATEGORY";
                    dt = new DataTable();
                    dt = obj.GetData_New(query, CommandType.Text, ilist, litem);

                    subCat.DataSource = dt;
                    subCat.DataTextField = "PRODUCT_SUBCATEGORY";
                    subCat.DataValueField = "PRODUCT_SUBCATEGORY";
                    subCat.DataBind();
                    subCat.Items.Insert(0, new ListItem("--Select--", "0"));

                    //=========Material Code

                    DropDownList ddlProduct = (DropDownList)e.Row.FindControl("drpProduct_Code");

                    //============Material Name

                    DropDownList ddlPName = (DropDownList)e.Row.FindControl("drpProduct_Name");
                    query = "";
                    //query = "select distinct(Product_Code) as Product_Code,concat(Product_Code,' - ',Product_Name) as Product_Name from ax.ACXPRODUCTMASTER where Product_Group='" + ddList.SelectedItem.Value + "' and PRODUCT_SUBCATEGORY='" + subCat.SelectedItem.Value + "' order by Product_Name";
                    query = "select distinct(ITEMID) as Product_Code,concat(Product_Code,' - ',Product_Name) as Product_Name from ax.INVENTTABLE where BLOCK=0 AND Product_Group='" + ddList.SelectedItem.Value + "' and PRODUCT_SUBCATEGORY='" + subCat.SelectedItem.Value + "' order by Product_Name";
                    dt = new DataTable();
                    dt = obj.GetData_New(query, CommandType.Text, ilist, litem);

                    ddlProduct.DataSource = dt;
                    ddlProduct.DataTextField = "Product_Code";
                    ddlProduct.DataValueField = "Product_Code";
                    ddlProduct.DataBind();
                    ddlProduct.Items.Insert(0, new ListItem("--Select--", "0"));

                    string Prcode = dr.Row.ItemArray[2].ToString();
                    ddlProduct.SelectedIndex = ddlProduct.Items.IndexOf(ddlProduct.Items.FindByText(Prcode));

                    ddlPName.DataSource = dt;
                    ddlPName.DataTextField = "Product_Name";
                    ddlPName.DataValueField = "Product_Code";
                    ddlPName.DataBind();
                    ddlPName.Items.Insert(0, new ListItem("--Select--", "0"));

                    string Prname = dr.Row.ItemArray[3].ToString();
                    ddlPName.SelectedIndex = ddlPName.Items.IndexOf(ddlPName.Items.FindByText(Prname));
                }
            }
        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                DropDownList drpPrGr = (DropDownList)gvDetails.FooterRow.FindControl("drpProduct_Group_Entry");
                string PrGr = drpPrGr.SelectedItem.Value;
                DropDownList drpPrCat = (DropDownList)gvDetails.FooterRow.FindControl("drpProduct_SubCategory_Entry");
                string PrCat = drpPrCat.SelectedItem.Value;
                DropDownList drpPrCode = (DropDownList)gvDetails.FooterRow.FindControl("drpProduct_Code_Entry");
                string PrCode = drpPrCode.SelectedItem.Value;
                DropDownList drpPrName = (DropDownList)gvDetails.FooterRow.FindControl("drpProduct_Name_Entry");
                string PrName = drpPrName.SelectedItem.Value;
                string PName = drpPrName.SelectedItem.Text; ;
                TextBox Box = (TextBox)gvDetails.FooterRow.FindControl("txtBox_Entry");
                TextBox Crates = (TextBox)gvDetails.FooterRow.FindControl("txtCrates_Entry");
                TextBox Ltr = (TextBox)gvDetails.FooterRow.FindControl("txtLtr_Entry");
                TextBox txtProductCode = (TextBox)gvDetails.FooterRow.FindControl("txtProductCode_Entry");

                //=============Validation=================
                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    Label lblPrCode = (Label)gvDetails.Rows[i].FindControl("Product_Code");
                    string PrCode1 = lblPrCode.Text;
                    if (PrCode1 == PrCode)
                    {
                        // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('"+ PName +" is already exists in the list .Please Select Another Product !!');", true);

                        string message = "alert('" + PName + " is already exists in the list .Please Select Another Product !!');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                        return;
                    }

                }
                if (PrGr == "")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Product Group!!');", true);
                    string message = "alert('Please Enter Product Group!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
                if (PrCode == "")
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Product !!');", true);

                    string message = "alert('Please Enter Product Group!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
                if (PrName == "")
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Product !!');", true);

                    string message = "alert('Please Enter Product !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    return;
                }
                if (Box.Text == "")
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Box !!');", true);

                    string message = "alert('Please Enter Box !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    return;
                }
                if (Crates.Text == "")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Box !!');", true);
                    string message = "alert('Please Enter Box !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
                if (Ltr.Text == "")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter Box !!');", true);

                    string message = "alert('Please Enter Box !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }

                //========================================
                slno = 0;
                ////================================
                if (txtIndentNo.Text != "")
                {
                    try
                    { //span i = (span)gvDetails.FooterRow.FindControl("Line_No");
                        slno = gvDetails.Rows.Count + 1;
                        // slno = Convert.ToInt16(i.Text);
                        //crudoperations("INSERT", PrGr, PrCode, PrName, Crates.Text, Box.Text, Ltr.Text, slno);
                        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                        conn = obj.GetConnection();
                        cmd1 = new SqlCommand("[dbo].[ACX_PurchaseIndent_Line]");
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Clear();
                        cmd1.Parameters.AddWithValue("@Line_No", slno);
                        cmd1.Parameters.AddWithValue("@Site_Code", lblsite.Text);
                        cmd1.Parameters.AddWithValue("@indent_No", txtIndentNo.Text);
                        cmd1.Parameters.AddWithValue("@Product_GroupCode", PrGr);
                        cmd1.Parameters.AddWithValue("@Product_Code", PrCode);
                        cmd1.Parameters.AddWithValue("@Crates", Convert.ToDecimal(Crates.Text));
                        cmd1.Parameters.AddWithValue("@Box", Convert.ToDecimal(Box.Text));
                        cmd1.Parameters.AddWithValue("@Ltr", Convert.ToDecimal(Ltr.Text));
                        cmd1.Parameters.AddWithValue("@status", "INSERT");
                        cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());

                        cmd1.ExecuteNonQuery();

                        gvDetails.EditIndex = -1;

                        BindGridview();
                    }

                    catch(Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt = Session["ItemTable"] as DataTable;
                    if (Session["LineItem"] == null)
                    {
                        DataColumn column = new DataColumn();
                        column.DataType = System.Type.GetType("System.Int32");
                        column.AutoIncrement = true;
                        column.AutoIncrementSeed = 1;
                        column.AutoIncrementStep = 1;
                        column.ColumnName = "Line_No";
                        //-----------------------------------------------------------//

                        dtLineItems = new DataTable("LineItemTable");
                        dtLineItems.Columns.Add(column);
                        // dtLineItems.Columns.Add("Line_No", typeof(int));
                        dtLineItems.Columns.Add("Product_Group", typeof(string));
                        dtLineItems.Columns.Add("Product_SubCategory", typeof(string));
                        dtLineItems.Columns.Add("Product_Code", typeof(string));
                        dtLineItems.Columns.Add("Product_Name", typeof(string));
                        dtLineItems.Columns.Add("Box", typeof(decimal));
                        dtLineItems.Columns.Add("Crates", typeof(decimal));
                        dtLineItems.Columns.Add("Ltr", typeof(decimal));
                        dtLineItems.Columns.Add("lblProductCode", typeof(string));

                    }
                    else
                    {
                        dtLineItems = (DataTable)Session["LineItem"];
                    }
                    DataRow row;
                    row = dtLineItems.NewRow();

                    row["Product_Group"] = PrGr;
                    row["Product_SubCategory"] = PrCat;
                    row["Product_Code"] = PrCode;
                    row["Product_Name"] = PName;
                    row["Crates"] = Convert.ToDecimal(Crates.Text.Trim().ToString());
                    row["Box"] = Convert.ToDecimal(Box.Text.Trim().ToString());
                    row["Ltr"] = Convert.ToDecimal(Ltr.Text.Trim().ToString());
                    row["lblProductCode"] = txtProductCode.Text.Trim().ToString();
                    dtLineItems.Rows.Add(row);
                    //Update session table
                    Session["LineItem"] = dtLineItems;
                    dt = dtLineItems; ;

                    if (dt.Rows.Count > 0)
                    {
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();

                    }
                    else
                    {
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();

                    }

                }
                Box.Text = string.Empty;
                //===========================
                // crudoperations("INSERT", PrGr, PrCode, PrName, Crates.Text, Box.Text, Ltr.Text,slno);              
            }

            //============            
        }

        protected void crudoperations(string status, string PrGr, string PrCode, string PrName, string Crates, string Box, string Ltr, int slno)
        {
            try
            {
                DataTable dt = new DataTable();
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                String query = "";
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                query = "Select * from  [ax].[ACXPURCHINDENTLINE] where SITEID='" + lblsite.Text + "' and INDENT_NO='' and PRODUCT_CODE='" + PrCode + "'";
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    string message = "alert('Already selected Product No: " + PrCode + " ');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    return;
                }

                dt = new DataTable();

                if (status == "INSERT")
                {
                    ilist.Add("@Line_No"); litem.Add(Convert.ToString(slno));
                    ilist.Add("@Site_Code"); litem.Add(lblsite.Text);
                    ilist.Add("@indent_No"); litem.Add(txtIndentNo.Text);
                    ilist.Add("@Product_GroupCode"); litem.Add(PrGr);
                    ilist.Add("@Product_Code"); litem.Add(PrCode);
                    ilist.Add("@Crates"); litem.Add(Crates);
                    ilist.Add("@Box"); litem.Add(Box);
                    ilist.Add("@Ltr"); litem.Add(Ltr);
                    ilist.Add("@status"); litem.Add("INSERT");
                    ilist.Add("@DATAAREAID"); litem.Add(Session["DATAAREAID"].ToString());

                    query = "[dbo].[USP_ACX_PurchaseIndent_Line]";
                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);
                }
                else if (status == "UPDATE")
                {
                    ilist.Add("@Site_Code"); litem.Add(lblsite.Text);
                    ilist.Add("@indent_No"); litem.Add(txtIndentNo.Text);
                    ilist.Add("@Product_GroupCode"); litem.Add(PrGr);
                    ilist.Add("@Product_Code"); litem.Add(PrCode);
                    ilist.Add("@Crates"); litem.Add(Crates);
                    ilist.Add("@Box"); litem.Add(Box);
                    ilist.Add("@Ltr"); litem.Add(Ltr);
                    ilist.Add("@Line_no"); litem.Add(Convert.ToString(slno));
                    ilist.Add("@status"); litem.Add("UPDATE");
                    ilist.Add("@DATAAREAID"); litem.Add(Session["DATAAREAID"].ToString());
                    query = "[dbo].[USP_ACX_PurchaseIndent_Line]";
                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);

                }
                else if (status == "DELETE")
                {
                    string lineno = Convert.ToString(slno);
                    ilist.Add("@Site_Code"); litem.Add(lblsite.Text);
                    ilist.Add("@indent_No"); litem.Add(txtIndentNo.Text);
                    ilist.Add("@Line_no"); litem.Add(Convert.ToString(slno));
                    ilist.Add("@status"); litem.Add("DELETE");
                    query = "[dbo].[USP_ACX_PurchaseIndent_Line]";
                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);
                }

                gvDetails.EditIndex = -1;

                BindGridview();
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void drpSalesOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSalesOfficeCode.Text = drpSalesOff.SelectedItem.Value;
            Session["LineItem"] = null;
        }

        protected void txtBox_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            TextBox txtB = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtB.Parent.Parent;
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            string query;
            int idx = row.RowIndex;

            DropDownList ddlPrGr = (DropDownList)row.Cells[0].FindControl("drpProduct_Group");
            DropDownList ddlPrCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code");

            if (Convert.ToDecimal(txtB.Text) > 0)
            {
                query = "select  " + txtB.Text + "/A.Product_Crate_PackSize as Crates,(" + txtB.Text + "*A.Product_PackSize*A.Ltr)/1000 as Ltr from ax.INVENTTABLE A where A.ItemId='" + ddlPrCode.SelectedItem.Value + "' and Product_Group='" + ddlPrGr.SelectedItem.Value + "'";

                dt = obj.GetData(query);

                TextBox lblCrates = (TextBox)row.Cells[0].FindControl("txtCrates");
                TextBox lblLtr = (TextBox)row.Cells[0].FindControl("txtLtr");

                //lblCrates.Text = dt.Rows[0]["Crates"].ToString();
                //lblLtr.Text = dt.Rows[0]["Ltr"].ToString();
                if (dt.Rows.Count > 0)
                {
                    decimal c, l;
                    c = Convert.ToDecimal(dt.Rows[0]["Crates"].ToString());
                    l = Convert.ToDecimal(dt.Rows[0]["Ltr"].ToString());

                    lblCrates.Text = c.ToString("#.##");
                    lblLtr.Text = l.ToString("#.##");
                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);
                string message = "alert('Please Enter a valid number!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
        }

        protected void txtBox_Entry_TextChanged(object sender, EventArgs e)

        {
            try
            {
                DataTable dt = new DataTable();
                TextBox txtB = (TextBox)sender;
                GridViewRow row = (GridViewRow)txtB.Parent.Parent;
                Button btnBox = (Button)row.Cells[6].FindControl("btnAdd");
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string query;
                int idx = row.RowIndex;
                DropDownList ddlPrGr = (DropDownList)row.Cells[0].FindControl("drpProduct_Group_Entry");
                DropDownList ddlPrCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code_Entry");


                if (Convert.ToDecimal(txtB.Text) > 0)
                {
                    //query = "select Round((" + txtB.Text + "/A.Product_Crate_PackSize),0) as Crates,(" + txtB.Text + "*A.Product_PackSize*A.Ltr)/1000 as Ltr from ax.INVENTTABLE A where A.ItemId='" + ddlPrCode.SelectedItem.Value + "' and Product_Group='" + ddlPrGr.SelectedItem.Value + "'";
                    query = "select CEILING(" + txtB.Text + "/A.Product_Crate_PackSize) as Crates,(" + txtB.Text + "*A.Product_PackSize*A.Ltr)/1000 as Ltr from ax.INVENTTABLE A where A.ItemId='" + ddlPrCode.SelectedItem.Value + "' and Product_Group='" + ddlPrGr.SelectedItem.Value + "'";
                    dt = obj.GetData(query);
                    TextBox lblCrates = (TextBox)row.Cells[0].FindControl("txtCrates_Entry");
                    TextBox lblLtr = (TextBox)row.Cells[0].FindControl("txtLtr_Entry");
                    if (dt.Rows.Count > 0)
                    {
                        decimal c, l;
                        c = Convert.ToDecimal(dt.Rows[0]["Crates"].ToString());
                        l = Convert.ToDecimal(dt.Rows[0]["Ltr"].ToString());
                        lblCrates.Text = c.ToString("#.##");
                        lblLtr.Text = l.ToString("#.##");



                        btnBox.Focus();
                        btnBox.CausesValidation = false;

                        //// btnBox.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnBox, null) + ";");
                        // btnBox.UseSubmitBehavior = false;
                        //btnBox.Visible = false;


                    }
                    // Button btnadd = (Button)row.Cells[0].FindControl("drpProduct_Group_Entry");

                }
                else
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);

                    string message = "alert('Please Enter a valid number!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtSerch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Focus();
        }

        protected void drpProduct_SubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void drpProduct_SubCategory_Entry_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DropDownList ddlPrCat = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlPrCat.Parent.Parent;
            DropDownList PrGr = (DropDownList)row.Cells[0].FindControl("drpProduct_Group_Entry");
            int idx = row.RowIndex;
            string query;

            //query = "select distinct(PRODUCT_SUBCATEGORY) as PRODUCT_SUBCATEGORY from ax.ACXPRODUCTMASTER where Product_Group='" + ddl.SelectedItem.Value + "' order by PRODUCT_SUBCATEGORY";
            //query = "select distinct(Product_Code) as Product_Code,concat(Product_Code,' - ',Product_Name) as Product_Name from ax.ACXPRODUCTMASTER where Product_Group='" + PrGr.SelectedItem.Value + "' and PRODUCT_SUBCATEGORY='" + ddlPrCat.SelectedItem.Value + "' order by Product_Name";
            query = "select distinct(ItemId) as Product_Code,concat(ItemId,' - ',Product_Name) as Product_Name from ax.INVENTTABLE where BLOCK=0 AND Product_Group='" + PrGr.SelectedItem.Value + "' and PRODUCT_SUBCATEGORY='" + ddlPrCat.SelectedItem.Value + "' order by Product_Name";
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            dt = obj.GetData_New(query, CommandType.Text, ilist, litem);

            DropDownList ddlPrCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code_Entry");
            DropDownList ddlPrName = (DropDownList)row.Cells[0].FindControl("drpProduct_Name_Entry");

            ddlPrCode.DataSource = dt;
            ddlPrCode.DataTextField = "Product_Code";
            ddlPrCode.DataValueField = "Product_Code";
            ddlPrCode.DataBind();
            //ddlPrCode.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlPrName.DataSource = dt;
            ddlPrName.DataTextField = "Product_Name";
            ddlPrName.DataValueField = "Product_Code";
            ddlPrName.DataBind();
            //ddlPrName.Items.Insert(0, new ListItem("--Select--", "0"));

            ddlPrName.Focus();

        }

        protected void txtProductCode_Entry_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtProductCode = (TextBox)sender;
                GridViewRow row = (GridViewRow)txtProductCode.Parent.Parent;
                DropDownList drpProductGroup = (DropDownList)row.Cells[0].FindControl("drpProduct_Group_Entry");
                DropDownList drpProductCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code_Entry");
                DropDownList drpProductSubCat = (DropDownList)row.Cells[0].FindControl("drpProduct_SubCategory_Entry");
                DropDownList drpProductName = (DropDownList)row.Cells[0].FindControl("drpProduct_Name_Entry");
                TextBox txtBox = (TextBox)row.Cells[0].FindControl("txtBox_Entry");
                TextBox txtCrate = (TextBox)row.Cells[0].FindControl("txtCrates_Entry");
                txtBox.Text = string.Empty;
                txtCrate.Text = string.Empty;

                DataTable dt = new DataTable();
                string query = " select [PRODUCT_GROUP],[PRODUCT_SUBCATEGORY],[ITEMID] as Product_Code,[PRODUCT_NAME],[ITEMID]+' - '+[PRODUCT_NAME] as Product from [ax].[INVENTTABLE] where [ITEMID]='" + txtProductCode.Text + "'";
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    drpProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                    //=============For Product Sub Cat======
                    drpProductSubCat.DataSource = dt;
                    drpProductSubCat.DataTextField = "PRODUCT_SUBCATEGORY";
                    drpProductSubCat.DataValueField = "PRODUCT_SUBCATEGORY";
                    drpProductSubCat.DataBind();
                    //=============For Product Code======
                    drpProductCode.DataSource = dt;
                    drpProductCode.DataTextField = "Product_Code";
                    drpProductCode.DataValueField = "Product_Code";
                    drpProductCode.DataBind();
                    //=============For Product Name======
                    drpProductName.DataSource = dt;
                    drpProductName.DataTextField = "Product";
                    drpProductName.DataValueField = "Product_Code";
                    drpProductName.DataBind();

                    txtBox.Focus();

                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void drpEntryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList drpEntryType = (DropDownList)sender;
            GridViewRow row = (GridViewRow)drpEntryType.Parent.Parent;
            TextBox lblBox = (TextBox)row.Cells[0].FindControl("txtBox_Entry");
            TextBox lblCrate = (TextBox)row.Cells[0].FindControl("txtCrates_Entry");
            TextBox lblLtr = (TextBox)row.Cells[0].FindControl("txtLtr_Entry");
            if (drpEntryType.SelectedItem.Text == "BOX")
            {
                lblCrate.ReadOnly = true;
                lblBox.ReadOnly = false;
                lblBox.Text = string.Empty;
                lblCrate.Text = string.Empty;
                lblLtr.Text = string.Empty;
                lblBox.Focus();
            }
            else if (drpEntryType.SelectedItem.Text == "CRATE")
            {
                lblBox.ReadOnly = true;
                lblCrate.ReadOnly = false;
                lblBox.Text = string.Empty;
                lblCrate.Text = string.Empty;
                lblLtr.Text = string.Empty;
                lblCrate.Focus();
            }
        }

        protected void txtCrates_Entry_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                TextBox txtCrate = (TextBox)sender;
                GridViewRow row = (GridViewRow)txtCrate.Parent.Parent;
                Button btnAdd = (Button)row.Cells[6].FindControl("btnAdd");
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string query;
                int idx = row.RowIndex;
                DropDownList ddlPrGr = (DropDownList)row.Cells[0].FindControl("drpProduct_Group_Entry");
                DropDownList ddlPrCode = (DropDownList)row.Cells[0].FindControl("drpProduct_Code_Entry");


                if (Convert.ToDecimal(txtCrate.Text) > 0)
                {
                    //query = "select Round((" + txtB.Text + "/A.Product_Crate_PackSize),0) as Crates,(" + txtB.Text + "*A.Product_PackSize*A.Ltr)/1000 as Ltr from ax.INVENTTABLE A where A.ItemId='" + ddlPrCode.SelectedItem.Value + "' and Product_Group='" + ddlPrGr.SelectedItem.Value + "'";
                    query = "select CEILING(" + txtCrate.Text + "*A.Product_Crate_PackSize) as Box,(" + txtCrate.Text + "*A.Product_Crate_PackSize*A.Product_PackSize*A.Ltr)/1000 as Ltr from ax.INVENTTABLE A where A.ItemId='" + ddlPrCode.SelectedItem.Value + "' and Product_Group='" + ddlPrGr.SelectedItem.Value + "'";
                    dt = obj.GetData(query);
                    TextBox lblBox = (TextBox)row.Cells[0].FindControl("txtBox_Entry");
                    TextBox lblLtr = (TextBox)row.Cells[0].FindControl("txtLtr_Entry");
                    if (dt.Rows.Count > 0)
                    {
                        decimal box, ltr;
                        box = Convert.ToDecimal(dt.Rows[0]["Box"].ToString());
                        ltr = Convert.ToDecimal(dt.Rows[0]["Ltr"].ToString());
                        lblBox.Text = box.ToString("#.##");
                        lblLtr.Text = ltr.ToString("#.##");



                        btnAdd.Focus();
                        btnAdd.CausesValidation = false;

                        btnAdd.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
                        // btnBox.UseSubmitBehavior = false;
                        //btnBox.Visible = false;


                    }
                    // Button btnadd = (Button)row.Cells[0].FindControl("drpProduct_Group_Entry");

                }
                else
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);

                    string message = "alert('Please Enter a valid number!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        //====================13/5/16=================
        public void ProductGroup()

        {
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' AND BLOCK=0 order by a.Product_Group";
            if (ddlBusinessUnit.SelectedItem.Text != "All..." && rdExempt.Checked == true)
            {
                strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' AND BLOCK=0 AND EXEMPT='1' and BU_CODE='" + ddlBusinessUnit.SelectedValue.ToString() + "'  order by a.Product_Group";
            }
            else if (ddlBusinessUnit.SelectedItem.Text == "All..." && rdExempt.Checked == true)
            {
                strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' AND BLOCK=0 AND EXEMPT='1' and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')  order by a.Product_Group";
            }
            else if (ddlBusinessUnit.SelectedItem.Text != "All..." && rdExempt.Checked == false)
            {
                strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' AND BLOCK=0 AND EXEMPT='0' and BU_CODE = '"+ddlBusinessUnit.SelectedValue.ToString() +"' order by a.Product_Group";
            }
            else if (ddlBusinessUnit.SelectedItem.Text == "All..." && rdExempt.Checked == false)
            {
                strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' AND BLOCK=0 AND EXEMPT='0' and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')  order by a.Product_Group";
            }
            DDLProductGroup.Items.Clear();
            DDLProductGroup.Items.Add("--Select--");
            baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

        }

        public void FillProductCode()
        {
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DDLMaterialCode.Items.Clear();
            string StatePriceGroup = "", WhereClause = "", bucode = "";
            StatePriceGroup = obj.GetScalarValue("SELECT PRICEGROUP from[ax].[inventsite] where siteid = '" + Session["SiteCode"] + "'");
            if (StatePriceGroup == null)
            { StatePriceGroup = ""; }
            if (StatePriceGroup.Length == 0)
            {
                StatePriceGroup = obj.GetScalarValue("SELECT TOP 1 ACX_PRICEGROUP FROM AX.LOGISTICSADDRESSSTATE WHERE STATEID = (SELECT STATECODE FROM AX.INVENTSITE WHERE SITEID = '" + Session["SiteCode"] + "')");
            }
            StatePriceGroup = Convert.ToString(StatePriceGroup).Trim() == "" ? "-" : StatePriceGroup;
            DataTable dtPriceList = obj.GetData("SELECT * FROM DBO.ACX_UDF_GETPRICE(GETDATE(),'" + StatePriceGroup + "','')");

            if (DDLProductGroup.Text != "--Select--")
            {
                WhereClause = " AND Product_Group='" + DDLProductGroup.SelectedValue + "'";
            }
            if (DDLProductSubCategory.Text != "--Select--")
            {
                if (DDLProductSubCategory.Text != "")
                {
                    WhereClause = WhereClause + " AND PRODUCT_SUBCATEGORY ='" + DDLProductSubCategory.SelectedItem.Value + "'";
                }
            }
            if (ddlBusinessUnit.SelectedItem.Text == "All...")
            {
                bucode = "AND BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
            }
            if (ddlBusinessUnit.SelectedItem.Text != "All...")
            {
                bucode = "AND BU_CODE='"+ddlBusinessUnit.SelectedValue.ToString()+"' ";
            }

            strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE WHERE BLOCK=0 " + WhereClause + " "+ bucode + " order by Product_Name";
            //else
            //{
            //    strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE WHERE BLOCK=0 AND BU_CODE in ('" + ddlBusinessUnit.SelectedItem.Value.ToString() + "')" + WhereClause + " order by Product_Name";
            //}
            if (rdExempt.Checked == true)
            {
                strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE WHERE BLOCK=0 AND EXEMPT='1' " + WhereClause + " "+ bucode + " order by Product_Name";
            }
            else
            {
                strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE WHERE BLOCK=0 AND EXEMPT='0' " + WhereClause + " "+ bucode + " order by Product_Name";
            }

            DDLMaterialCode.Items.Clear();
            DDLMaterialCode.Items.Add("--Select--");
            baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Product_Name", "Product_Code");
            foreach (ListItem item in DDLMaterialCode.Items)
            {
                DataRow[] dr = dtPriceList.Select("ITEMRELATION='" + item.Value.ToString() + "' AND AMOUNT>0");
                if (dr.Length == 0)
                {
                    item.Attributes.Add("style", "background-color:red;color:white;font-weight:bold;");
                }

            }
            DDLMaterialCode.Focus();
        }
        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                        + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "' AND P.BLOCK=0";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("--Select--");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            FillProductCode();
            DDLProductSubCategory.Focus();

            //===============
            txtEnterQty.Focus();
        }

        private void ShowData_ForExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string FilterQuery = string.Empty;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string query = string.Empty;

            try
            {
                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "[dbo].[ACX_PurchaseIndentProductList]";

                cmd.Parameters.AddWithValue("@siteid", Session["SiteCode"].ToString());
                if (rdExempt.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@EXEMPT", "1");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EXEMPT", "0");
                }
                //cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.CommandText = query;

                DataTable dt = new DataTable();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                //=================Create Excel==========

                //var workbook = new ExcelFile();

                //// Add new worksheet to Excel file.
                //var worksheet = workbook.Worksheets.Add("New worksheet");

                //// Set the value of the cell "A1".
                //worksheet.Cells["A1"].Value = "Hello world!";

                //// Save Excel file.
                //workbook.Save("Workbook.xls");
                //string attachment = "attachment; filename=PurchaseIndent.xls";
                //Response.ClearContent();
                //StringWriter strwritter = new StringWriter();
                //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("content-disposition", attachment);

                //string tab = "";
                //foreach (DataColumn dc in dt.Columns)
                //{
                //    Response.Write(tab + dc.ColumnName);
                //    tab = "\t";
                //}
                //Response.Write("\n");
                //int i;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    tab = "";
                //    for (i = 0; i < dt.Columns.Count; i++)
                //    {
                //        Response.Write(tab + dr[i].ToString());
                //        tab = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();
                string myServerPath = Server.MapPath("~/ExcelTemplate/PurchaseIndentTemplate.xlsx");

                //var excel = new Microsoft.Office.Interop.Excel.Application();
                //var workbook = excel.Workbooks.Add(true);

                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                //excel.Visible = true;
                //excel.Worksheets.Add(dtDataByfilter.Tables[0], "Target1");
                Microsoft.Office.Interop.Excel.Workbook wbExiting = excel.Workbooks.Open(myServerPath);
                Microsoft.Office.Interop.Excel.Worksheet sh = wbExiting.Sheets[1];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sh.Cells[i + 2, 1] = dt.Rows[i][0];
                        sh.Cells[i + 2, 2] = dt.Rows[i][1];
                        sh.Cells[i + 2, 3] = dt.Rows[i][2];
                        sh.Cells[i + 2, 4] = dt.Rows[i][3];
                    }
                }

                string strUniqueFileName = string.Empty;
                strUniqueFileName = Session["USERID"].ToString();

                string myServerPath1 = Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx");
                FileInfo file = new FileInfo(myServerPath1);
                if (file.Exists)
                {
                    file.Delete();
                }

                wbExiting.SaveAs(Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx"), Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                excel.DisplayAlerts = false;
                wbExiting.Close(false, Type.Missing, Type.Missing);
                excel.Quit();

                FileInfo file1 = new FileInfo(myServerPath1);

                if (file1.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment; filename=PurchaseIndent.xlsx");
                    Response.AddHeader("Content-Type", "application/Excel");
                    Response.ContentType = "application/vnd.xls";
                    Response.AddHeader("Content-Length", file1.Length.ToString());
                    Response.WriteFile(file1.FullName);
                    // file.Delete();    
                    Response.End();
                }
                else
                {
                    Response.Write("This file does not exist.");
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        protected void DDLProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //strQuery = "Select distinct P.ITEMID+'-'+P.Product_Name as Name,P.ITEMID from ax.InventTable P where P.BLOCK=0 AND Product_Group='" + DDLProductGroup.SelectedValue + "' and P.PRODUCT_SUBCATEGORY ='" + DDLProductSubCategory.SelectedItem.Value + "'"; //--AND SITE_CODE='657546'";
            //DDLMaterialCode.Items.Clear();
            ////DDLMaterialCode.Items.Add("--Select--");
            //baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Name", "ITEMID");
            FillProductCode();
            DDLMaterialCode.Focus();
        }

        private void dtChangeZeroToNull(DataTable dataTable)
        {
            List<string> dcNames = dataTable.Columns
                                    .Cast<DataColumn>()
                                    .Select(x => x.ColumnName)
                                    .ToList(); //This querying of the Column Names, you could do with LINQ
            foreach (DataRow row in dataTable.Rows) //This is the part where you update the cell one by one
                foreach (string columnName in dcNames)
                    row[columnName] = row[columnName] == DBNull.Value ? 0 : row[columnName];
        }

        //protected void btnUplaod_Click(object sender, EventArgs e)
        //{
        //    SqlConnection conn = baseObj.GetConnection();
        //    cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandTimeout = 0;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    string query = "abc";
        //    cmd.CommandText = query;
        //    DataTable dt = new DataTable();
        //    dt.Load(cmd.ExecuteReader());
        //    //gridview1.DataSource = dt;
        //    //gridview1.DataBind();
        //    gridviewRecordNotExist.DataSource = dt;
        //    gridviewRecordNotExist.DataBind();
        //    ModalPopupExtender1.Show();
        //    gvDetails.Visible = false;
        //}
        protected void btnUplaod_Click(object sender, EventArgs e)
        {
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            try
            {
                if (AsyncFileUpload1.HasFile)
                {
                    //  #region
                    gvDetails.Visible = true;
                    string fileName = System.IO.Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                    //if (Path.GetExtension(AsyncFileUpload1.PostedFile.FileName) != ".xls")
                    //{
                    //    string message = "alert('Only .xls is allowed.');";
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    //    return;
                    //}
                    AsyncFileUpload1.PostedFile.SaveAs(Server.MapPath("~/Uploads/" + fileName));
                    string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                    string conString = string.Empty;
                    string extension = Path.GetExtension(AsyncFileUpload1.PostedFile.FileName);
                    DataTable dtExcelData = new DataTable();

                    //excel upload
                    dtExcelData = CreamBell_DMS_WebApps.App_Code.ExcelUpload.ImportExcelXLSO_PurchaseIndent(Server.MapPath("~/Uploads/" + fileName), true);

                    dtChangeZeroToNull(dtExcelData);

                    dtExcelData.TableName = "PurchaseIndent";
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtExcelData);
                    string StockAdjXml = ds.GetXml();

                    conn = baseObj.GetConnection();
                    cmd = new SqlCommand("ACX_PURCHASEINDENTEXCELUPLOAD");
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                    if (rdExempt.Checked == true)
                    {
                        cmd.Parameters.AddWithValue("@EXEMPT", "1");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EXEMPT", "0");
                    }
                    cmd.Parameters.AddWithValue("@XmlData", StockAdjXml);

                    SqlDataAdapter da = new SqlDataAdapter();

                    DataSet ds1 = new DataSet();
                    da.SelectCommand = cmd;
                    da.Fill(ds1);


                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtPurchIndent = new DataTable();
                        dtPurchIndent.Columns.Add("Line_No", typeof(string));
                        dtPurchIndent.Columns.Add("Product_Group", typeof(string));
                        dtPurchIndent.Columns.Add("Product_SubCategory", typeof(string));
                        dtPurchIndent.Columns.Add("Product_Code", typeof(string));
                        dtPurchIndent.Columns.Add("Product_Name", typeof(string));
                        dtPurchIndent.Columns.Add("Stock_Qty", typeof(decimal));
                        dtPurchIndent.Columns.Add("Box", typeof(decimal));
                        dtPurchIndent.Columns.Add("lblProductCode", typeof(string));
                        dtPurchIndent.Columns.Add("Crates", typeof(decimal));
                        dtPurchIndent.Columns.Add("Ltr", typeof(decimal));
                        dtPurchIndent.AcceptChanges();

                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            decimal TotalQty = 0, TotalcrateQty = 0, crateQty = 0, boxQty = 0, crateSize = 0, LtrQty = 0, ltr = 0, ProductPackSize = 0;
                            crateSize = App_Code.Global.ParseDecimal(ds1.Tables[0].Rows[i]["PRODUCT_CRATE_PACKSIZE"].ToString());
                            ltr = App_Code.Global.ParseDecimal(ds1.Tables[0].Rows[i]["LTR"].ToString());
                            ProductPackSize = App_Code.Global.ParseDecimal(ds1.Tables[0].Rows[i]["PRODUCT_PACKSIZE"].ToString());
                            boxQty = App_Code.Global.ParseDecimal(ds1.Tables[0].Rows[i]["BoxQty"].ToString());
                            crateQty = App_Code.Global.ParseDecimal(ds1.Tables[0].Rows[i]["Crate"].ToString());
                            TotalQty = crateQty * crateSize + boxQty;
                            TotalcrateQty = TotalQty / crateSize;
                            LtrQty = (TotalQty * ltr * ProductPackSize) / 1000;

                            dtPurchIndent.Rows.Add();
                            dtPurchIndent.Rows[i]["Line_No"] = ds1.Tables[0].Rows[i]["SNO"].ToString();
                            dtPurchIndent.Rows[i]["Product_Group"] = ds1.Tables[0].Rows[i]["ProductGroup"].ToString();
                            dtPurchIndent.Rows[i]["Product_SubCategory"] = ds1.Tables[0].Rows[i]["ProductSubCategory"].ToString();
                            dtPurchIndent.Rows[i]["Product_Code"] = ds1.Tables[0].Rows[i]["Product_Code"].ToString();
                            dtPurchIndent.Rows[i]["lblProductCode"] = ds1.Tables[0].Rows[i]["Product_Code"].ToString();
                            dtPurchIndent.Rows[i]["Product_Name"] = ds1.Tables[0].Rows[i]["ProductDesc"].ToString();
                            dtPurchIndent.Rows[i]["Stock_Qty"] = Convert.ToDecimal(ds1.Tables[0].Rows[i]["StockQty"].ToString());
                            dtPurchIndent.Rows[i]["Box"] = Convert.ToDecimal(TotalQty.ToString("0.0000"));
                            dtPurchIndent.Rows[i]["Crates"] = Convert.ToDecimal(TotalcrateQty.ToString("0.00"));
                            dtPurchIndent.Rows[i]["Ltr"] = Convert.ToDecimal(LtrQty.ToString("0.00"));
                            //lblHidden.Text = ds1.Tables[0].Rows[i]["UOM"].ToString();
                        }
                        if (dtPurchIndent.Rows.Count > 0)
                        {
                            Session["LineItem"] = dtPurchIndent;
                            gvDetails.DataSource = dtPurchIndent;
                            gvDetails.DataBind();
                            gvDetails.Visible = true;
                            GridViewFooterCalculate(dtPurchIndent);
                        }
                        else
                        {
                            gvDetails.DataSource = null;
                            gvDetails.DataBind();
                            gvDetails.Visible = false;
                        }
                    }
                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        gridviewRecordNotExist.DataSource = (DataTable)ds1.Tables[1];
                        gridviewRecordNotExist.DataBind();
                        Panel1.Visible = true;
                        //ModalPopupExtender1.Show();
                    }
                    //Session["LineItem"] = ds1.Tables[0];
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.Visible = true;
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open")
                    {
                        conn.Close();
                    }
                }
            }

        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            //BtnAddItem.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(BtnAddItem, null) + ";");
            lblMessage.Text = string.Empty;
            //=============Validation=================
            foreach (GridViewRow grv in gvDetails.Rows)
            {
                // string product = grv.Cells[4].Text;
                Label lblProduct = (Label)grv.FindControl("Product_Code");
                if (DDLMaterialCode.SelectedItem.Value == lblProduct.Text)
                {
                    txtEnterQty.Text = "";
                    txtQtyBox.Text = "";
                    txtQtyCrates.Text = "";
                    txtLtr.Text = "";
                    txtEnterCrate.Text = "";

                    string message = "alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
            }

            //==============================================
            if (DDLProductGroup.Text == "Select..." || DDLProductGroup.Text == "")
            {
                string message = "alert('Select Material Group !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                DDLProductGroup.Focus();
                return;
            }
            if (DDLMaterialCode.Text == string.Empty || DDLMaterialCode.Text == "Select...")
            {
                string message = "alert('Select Product First !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                DDLMaterialCode.Focus();
                return;
            }
            if (txtQtyBox.Text == string.Empty || txtQtyBox.Text == "0.0000")
            {
                string message = "alert('Qty cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                return;
            }
            if (txtQtyCrates.Text == string.Empty)
            {
                string message = "alert('Price cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                return;
            }
            if (txtLtr.Text == string.Empty || txtLtr.Text == "0")
            {
                string message = "alert('ltr cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                return;
            }

            if (rdExempt.Checked == true)
            {
                if (Session["Exempt_CurVal"].ToString() != "1")
                {

                    string message = "alert('Please add Exempted Products only');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    resetlinetextboxes();

                    return;
                }
            }

            if (rdNonExempt.Checked == true)
            {
                if (Session["Exempt_CurVal"].ToString() != "0")
                {

                    string message = "alert('Please add NonExempted Products only');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    resetlinetextboxes();
                    return;
                }
            }


            //=================================================
            slno = 0;
            ////================================
            if (txtIndentNo.Text != "")
            {
                try
                {
                    slno = gvDetails.Rows.Count + 1;
                    CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                    conn = obj.GetConnection();
                    cmd1 = new SqlCommand("[dbo].[ACX_PurchaseIndent_Line]");
                    cmd1.Connection = conn;
                    cmd1.Transaction = transaction;
                    cmd1.CommandTimeout = 3600;
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Clear();
                    cmd1.Parameters.AddWithValue("@Line_No", slno);
                    cmd1.Parameters.AddWithValue("@Site_Code", lblsite.Text);
                    cmd1.Parameters.AddWithValue("@indent_No", txtIndentNo.Text);
                    cmd1.Parameters.AddWithValue("@Product_GroupCode", DDLProductGroup.SelectedItem.Value);
                    cmd1.Parameters.AddWithValue("@Product_Code", DDLMaterialCode.SelectedItem.Value);
                    cmd1.Parameters.AddWithValue("@Crates", Convert.ToDecimal(txtQtyCrates.Text));
                    cmd1.Parameters.AddWithValue("@Box", Convert.ToDecimal(txtQtyBox.Text));
                    cmd1.Parameters.AddWithValue("@Ltr", Convert.ToDecimal(txtLtr.Text));
                    cmd1.Parameters.AddWithValue("@status", "INSERT");
                    cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());

                    cmd1.ExecuteNonQuery();

                    gvDetails.EditIndex = -1;

                    BindGridview();
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                dt = Session["ItemTable"] as DataTable;
                if (Session["LineItem"] == null)
                {
                    DataColumn column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.AutoIncrement = true;
                    column.AutoIncrementSeed = 1;
                    column.AutoIncrementStep = 1;
                    column.ColumnName = "Line_No";
                    //-----------------------------------------------------------//

                    dtLineItems = new DataTable("LineItemTable");
                    dtLineItems.Columns.Add(column);
                    // dtLineItems.Columns.Add("Line_No", typeof(int));
                    dtLineItems.Columns.Add("Product_Group", typeof(string));
                    dtLineItems.Columns.Add("Product_SubCategory", typeof(string));
                    dtLineItems.Columns.Add("Product_Code", typeof(string));
                    dtLineItems.Columns.Add("Product_Name", typeof(string));
                    dtLineItems.Columns.Add("Stock_Qty", typeof(string));
                    dtLineItems.Columns.Add("Box", typeof(decimal));
                    dtLineItems.Columns.Add("Crates", typeof(decimal));
                    dtLineItems.Columns.Add("Ltr", typeof(decimal));
                    dtLineItems.Columns.Add("lblProductCode", typeof(string));

                }
                else
                {
                    dtLineItems = (DataTable)Session["LineItem"];
                    dtLineItems.AsEnumerable().Where(r => r.Field<string>("Product_Code") == (string.Empty)).ToList().ForEach(product => product.Delete());
                }
                DataRow row;
                row = dtLineItems.NewRow();

                row["Product_Group"] = DDLProductGroup.Text;
                row["Product_SubCategory"] = DDLProductSubCategory.Text;
                row["Product_Code"] = DDLMaterialCode.SelectedItem.Value;
                row["Product_Name"] = DDLMaterialCode.SelectedItem.Text;
                string result = obj.GetScalarValue("SELECT cast (ISNULL(TRANSQTY,0) as decimal(18,2)) as TRANSQTY FROM vw_CurrentStock WHERE SiteCode = '" + Session["SiteCode"].ToString() + "' AND PRODUCTCODE = '" + DDLMaterialCode.SelectedItem.Value + "' and TransLocation in (select top 1 mainwarehouse from ax.inventsite where siteid = '" + Session["SiteCode"].ToString() + "')");
                row["Stock_Qty"] = result == null ? "0" : result;
                row["Crates"] = Convert.ToDecimal(txtQtyCrates.Text.Trim().ToString());
                row["Box"] = Convert.ToDecimal(txtQtyBox.Text.Trim().ToString());
                row["Ltr"] = Convert.ToDecimal(txtLtr.Text.Trim().ToString());
                row["lblProductCode"] = DDLMaterialCode.SelectedItem.Value.Trim().ToString();
                dtLineItems.Rows.Add(row);
                //Update session table
                Session["LineItem"] = dtLineItems;
                dt = dtLineItems;

                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = true;
                    GridViewFooterCalculate(dt);

                }
                else
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();

                }
            }
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            txtEnterCrate.Text = string.Empty;
            //BtnAddItem.Enabled = true;
            //BtnAddItem.Attributes.Add("onclick", " this.disabled = false; " + ClientScript.GetPostBackEventReference(BtnAddItem, null) + ";");

            DDLMaterialCode.Focus();
            DDLProductSubCategory.Items.Clear();
            ddlBusinessUnit.SelectedIndex = 0;
            rdExempt.Enabled = false;
            rdNonExempt.Enabled = false;
            ProductGroup();
            FillProductCode();
            txtStockQty.Text = "";
        }
        private void GridViewFooterCalculate(DataTable dt)
        {
            //decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Value"));          //For Total[Sum] Value Show in Footer--//
            decimal tBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("Box"));
            decimal tCrate = dt.AsEnumerable().Sum(row => row.Field<decimal>("Crates"));
            decimal tLtr = dt.AsEnumerable().Sum(row => row.Field<decimal>("Ltr"));
            //decimal tTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("Tax_Amount"));

            //===============11-5-2016=====
            gvDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[6].Text = "TOTAL";
            gvDetails.FooterRow.Cells[6].Font.Bold = true;

            gvDetails.FooterRow.Visible = true;
            gvDetails.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[8].Text = tBox.ToString("N2");
            gvDetails.FooterRow.Cells[8].Font.Bold = true;

            gvDetails.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[9].Text = tCrate.ToString("N2");
            gvDetails.FooterRow.Cells[9].Font.Bold = true;

            gvDetails.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[10].Text = tLtr.ToString("N2");
            gvDetails.FooterRow.Cells[10].Font.Bold = true;

            //gvDetails.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Left;
            //gvDetails.FooterRow.Cells[15].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[15].Text = tTaxAmount.ToString("N2");
            //gvDetails.FooterRow.Cells[15].Font.Bold = true;

            //===============11-5-16=============
            //gvDetails.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Left;
            //gvDetails.FooterRow.Cells[17].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[17].Text = tAddTaxAmount.ToString("N2");
            //gvDetails.FooterRow.Cells[17].Font.Bold = true;

            //gvDetails.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Left;
            //gvDetails.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[13].Text = tDiscValue.ToString("N2");
            //gvDetails.FooterRow.Cells[13].Font.Bold = true;
        }

        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            txtEnterCrate.Text = string.Empty;
            DataTable dt = new DataTable();


            //====================================
            //if (DDLProductGroup.Text == "--Select--")
            //{
            string query = "select Product_Group,PRODUCT_SUBCATEGORY,EXEMPT  from ax.INVENTTABLE where ItemId='" + DDLMaterialCode.SelectedItem.Value + "' order by Product_Name";
            dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DDLProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                ProductSubCategory();
                //=============For Product Sub Cat======
                DDLProductSubCategory.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
                //DDLProductSubCategory.DataSource = dt;
                //DDLProductSubCategory.DataTextField = "PRODUCT_SUBCATEGORY";
                //DDLProductSubCategory.DataValueField = "PRODUCT_SUBCATEGORY";
                //DDLProductSubCategory.DataBind();
                Session["Exempt_CurVal"] = dt.Rows[0]["EXEMPT"];

            }
            // }


            //===================Validation======
            foreach (GridViewRow grv in gvDetails.Rows)
            {

                Label lblProduct = (Label)grv.FindControl("Product_Code");
                if (DDLMaterialCode.SelectedItem.Value == lblProduct.Text)
                {
                    txtEnterQty.Text = "";
                    txtQtyBox.Text = "";
                    txtQtyCrates.Text = "";
                    txtLtr.Text = "";
                    txtEnterCrate.Text = "";

                    ////this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');", true);
                    string message = "alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }

            }
            //SELECT* FROM vw_CurrentStock WHERE SiteCode = '000059144' AND PRODUCTCODE = '10179'
            string result = obj.GetScalarValue("SELECT cast (ISNULL(TRANSQTY,0) as decimal(18,2)) as TRANSQTY FROM vw_CurrentStock WHERE SiteCode = '" + Session["SiteCode"].ToString() + "' AND PRODUCTCODE = '" + DDLMaterialCode.SelectedItem.Value + "' and TransLocation in (select top 1 mainwarehouse from ax.inventsite where siteid = '" +Session["SiteCode"].ToString() + "')");
            txtStockQty.Text = result == null ? "0" : result;
            string StatePriceGroup = "";
            StatePriceGroup = obj.GetScalarValue("SELECT PRICEGROUP from[ax].[inventsite] where siteid = '" + Session["SiteCode"] + "'");
            if (StatePriceGroup == null)
            { StatePriceGroup = ""; }
            if (StatePriceGroup.Length == 0)
            {
                StatePriceGroup = obj.GetScalarValue("SELECT TOP 1 ACX_PRICEGROUP FROM AX.LOGISTICSADDRESSSTATE WHERE STATEID = (SELECT STATECODE FROM AX.INVENTSITE WHERE SITEID = '" + Session["SiteCode"] + "')");
            }
            StatePriceGroup = Convert.ToString(StatePriceGroup).Trim() == "" ? "-" : StatePriceGroup;
            DataTable dtPriceList = obj.GetData("SELECT * FROM DBO.ACX_UDF_GETPRICE(GETDATE(),'" + StatePriceGroup + "','')");
            foreach (ListItem item in DDLMaterialCode.Items)
            {
                DataRow[] dr = dtPriceList.Select("ITEMRELATION='" + item.Value.ToString() + "' AND AMOUNT>0");
                if (dr.Length == 0)
                {
                    item.Attributes.Add("style", "background-color:#3399FF;color:white;font-weight:bold;");
                }

            }
            txtEnterCrate.Focus();
        }
        public void ProductSubCategory()
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                        + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "' AND P.BLOCK=0";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("--Select--");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            // FillProductCode();
            DDLProductSubCategory.Focus();
        }
        protected void txtQtyBox_TextChanged(object sender, EventArgs e)
        {
            QtyCalcualtion();
            //try
            //{
            //    //string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value,string.Empty, int.Parse(txtEnterQty.Text), ddlEntryType.SelectedItem.Value);
            //    string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, string.Empty, int.Parse(txtEnterQty.Text), "Box");
            //    if (calValue.Length > 0)
            //    {
            //        if (calValue[5] == "Box")
            //        {
            //            txtQtyBox.Text = txtEnterQty.Text;
            //            txtQtyCrates.Text = calValue[0];
            //        }
            //        if (calValue[5] == "Crate")
            //        {
            //            txtQtyCrates.Text = txtEnterQty.Text;
            //            txtQtyBox.Text = calValue[0];
            //        }
            //        txtLtr.Text = calValue[1];

            //        lblHidden.Text = calValue[4];

            //        BtnAddItem.Focus();
            //        BtnAddItem.CausesValidation = false;

            //        BtnAddItem.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(BtnAddItem, null) + ";");
            //    }
            //}
            //catch (Exception ex)
            //{              
            //    ////this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + ex.Message.ToString() + "');", true);

            //    string message = "alert(' Price of Product is not defined.Please Select Another Product !!');";
            //    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            //}

        }

        protected void ddlEntryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            txtEnterCrate.Text = string.Empty;
            txtQtyBox.Focus();
        }

        protected void txtEnterCrate_TextChanged(object sender, EventArgs e)
        {
            QtyCalcualtion();
        }

        protected void QtyCalcualtion()
        {
            try
            {
                decimal TotalQty = 0, TotalcrateQty = 0, crateQty = 0, boxQty = 0, crateSize = 0, LtrQty = 0, ltr = 0, ProductPackSize = 0;

                // string query = "EXEC ax.ACX_GetProductRate '" + DDLMaterialCode.SelectedItem.Value + "',''";
                string query = "Select ItemId as PRODUCT_CODE,Product_group, PRODUCT_NAME, PRODUCT_MRP, PRODUCT_PACKSIZE, PRODUCT_CRATE_PACKSIZE, UOM, LTR from ax.InventTable where ItemId = '" + DDLMaterialCode.SelectedItem.Value + "'";
                DataTable dtItem = new DataTable();
                dtItem = baseObj.GetData(query);
                if (dtItem.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Product Crate PackSize !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    boxQty = App_Code.Global.ParseDecimal(txtEnterQty.Text);
                    crateQty = App_Code.Global.ParseDecimal(txtEnterCrate.Text);
                    crateSize = App_Code.Global.ParseDecimal(dtItem.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString());
                    ltr = App_Code.Global.ParseDecimal(dtItem.Rows[0]["LTR"].ToString());
                    ProductPackSize = App_Code.Global.ParseDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString());
                    TotalQty = crateQty * crateSize + boxQty;
                    TotalcrateQty = TotalQty / crateSize;
                    LtrQty = (TotalQty * ltr * ProductPackSize) / 1000;
                    //string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, "", TotalQty, "Box");
                    //if (calValue[0] != "")
                    {
                        txtQtyBox.Text = TotalQty.ToString("0.0000");
                        txtQtyCrates.Text = TotalcrateQty.ToString("0.00");
                        txtLtr.Text = LtrQty.ToString("0.00");

                        lblHidden.Text = dtItem.Rows[0]["UOM"].ToString();

                        BtnAddItem.Focus();
                        //BtnAddItem.CausesValidation = false;
                        //BtnAddItem.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(BtnAddItem, null) + ";");
                    }
                }
            }
            catch(Exception ex)
            {
                string message = "alert(' Price of Product is not defined.Please Select Another Product !!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLProductSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();
        }
        protected void resetlinetextboxes()
        {
            // txtBoxPcs.Text = "";
            // txtViewTotalBox.Text = "";
            // txtViewTotalPcs.Text = "";
            // txtPcs.Text = "";
            // txtCrate.Text = "";
            txtEnterQty.Text = "";
            txtQtyBox.Text = "";
            txtQtyCrates.Text = "";
            txtLtr.Text = "";
            //txtPrice.Text = "";
            //txtValue.Text = "";
        }

        protected void rdoManualEntry_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.ID == "rdoManualEntry")
            {
                #region Manual Entry

                panelAddLine.Visible = true;
                AsyncFileUpload1.Visible = false;
                btnUplaod.Visible = false;
                ImDnldTemp.Visible = false;
                //Panel1.Enabled = true;
                txtEnterQty.Text = string.Empty;
                txtQtyBox.Text = string.Empty;
                txtQtyCrates.Text = string.Empty;
                txtQtyCrates.Text = string.Empty;
                rdNonExempt.Enabled = true;
                rdExempt.Enabled = true;
                rdNonExempt.Checked = true;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                lblMessage.Text = string.Empty;
                CalendarExtender1.StartDate = DateTime.Now;
                ProductGroup();
                Session["LineItem"] = null;
                Session["SchemeLineItem"] = null;
                Session["PreSoNO"] = null;
                Session["SONO"] = null;
                //Session["Exempt_ComVal"] = null;
                Session["Exempt_CurVal"] = null;
                //Session["Comm_Exem"] = null;
                //Session["Curr_Exem"] = null;

                #endregion
            }
            if (rdo.ID == "rdoExcelEntry")
            {
                #region Excel Entry

                panelAddLine.Visible = false;
                AsyncFileUpload1.Visible = true;
                btnUplaod.Visible = true;
                rdNonExempt.Enabled = true;
                rdExempt.Enabled = true;
                rdNonExempt.Checked = true;
                ImDnldTemp.Visible = true;
                //Panel1.Enabled = false;
                txtEnterQty.Text = string.Empty;
                txtQtyBox.Text = string.Empty;
                txtQtyCrates.Text = string.Empty;
                txtQtyCrates.Text = string.Empty;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                CalendarExtender1.StartDate = DateTime.Now;
                ProductGroup();
                Session["LineItem"] = null;
                //Session["Exempt_ComVal"] = null;
                Session["Exempt_CurVal"] = null;
                //Session["Comm_Exem"] = null;
                //Session["Curr_Exem"] = null;
                Session["SchemeLineItem"] = null;
                Session["PreSoNO"] = null;
                Session["SONO"] = null;
                #endregion
            }
        }

        //protected void btnUplaod_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool b = ValidatePurchaseReturnHeaderData();
        //        if (b)
        //        {
        //            if (AsyncFileUpload1.HasFile)
        //            {
        //                //  #region
        //                gvDetails.Visible = true;
        //                string fileName = System.IO.Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
        //                //if (Path.GetExtension(AsyncFileUpload1.PostedFile.FileName) != ".xls")
        //                //{
        //                //    string message = "alert('Only .xls is allowed.');";
        //                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
        //                //    return;
        //                //}
        //                AsyncFileUpload1.PostedFile.SaveAs(Server.MapPath("~/Uploads/" + fileName));
        //                string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
        //                string conString = string.Empty;
        //                string extension = Path.GetExtension(AsyncFileUpload1.PostedFile.FileName);
        //                DataTable dtExcelData = new DataTable();

        //                //excel upload
        //                dtExcelData = CreamBell_DMS_WebApps.App_Code.ExcelUpload.ImportExcelXLSO(Server.MapPath("~/Uploads/" + fileName), true);

        //                dtChangeZeroToNull(dtExcelData);

        //                DataTable dtForShownUnuploadData = new DataTable();
        //                dtForShownUnuploadData.Columns.Add("ProductCode");
        //                dtForShownUnuploadData.Columns.Add("Qty");

        //                int j = 0;
        //                for (int i = 0; i < dtExcelData.Rows.Count; i++)
        //                {
        //                    if (dtExcelData.Rows[i]["ProductCode"].ToString() == "0")
        //                    {
        //                        DataRow dr = dtExcelData.Rows[i];
        //                        dtForShownUnuploadData.Rows.Add();
        //                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
        //                        dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
        //                        //Delete the rows from datatabel 
        //                        dr.Delete();
        //                        j += 1;
        //                    }

        //                }
        //                dtExcelData.AcceptChanges();


        //                //Create a new datatabel with valid datatypes 
        //                DataTable Exceltable = new DataTable();
        //                Exceltable.Columns.Add("ProductCode", typeof(string));
        //                Exceltable.Columns.Add("Qty", typeof(Double));
        //                Exceltable.Columns.Add("Crate", typeof(Double));
        //                Exceltable.Columns.Add("Pcs", typeof(Double));

        //                int p = 0;
        //                foreach (DataRow row in dtExcelData.Rows)
        //                {
        //                    //adding row in datatabel 
        //                    Exceltable.Rows.Add();
        //                    Exceltable.Rows[p]["ProductCode"] = dtExcelData.Rows[p]["ProductCode"].ToString();
        //                    Exceltable.Rows[p]["Qty"] = Convert.ToDouble(dtExcelData.Rows[p]["Qty"]);
        //                    Exceltable.Rows[p]["Crate"] = Convert.ToDouble(dtExcelData.Rows[p]["Crate"]);
        //                    Exceltable.Rows[p]["Pcs"] = Convert.ToDouble(dtExcelData.Rows[p]["Pcs"]);
        //                    p++;
        //                }
        //                //null the dtexcel tabel
        //                dtExcelData = null;

        //                //===============find duplicate value and merge it ====================
        //                var result1 = from row in Exceltable.AsEnumerable()
        //                              where row.Field<string>("ProductCode") != null
        //                              select row;
        //                //null the excelTable tabel
        //                Exceltable = null;

        //                var result = result1.AsEnumerable()
        //                             .GroupBy(r => new
        //                             {
        //                                 ProductCode = r.Field<String>("ProductCode"),
        //                                 // Qty = r.Field<Double>("Qty"),
        //                             })

        //                                   .Select(g =>
        //                                   {
        //                                       var row = g.First();
        //                                       row.SetField("Qty", g.Sum(r => r.Field<Double>("Qty")));
        //                                       row.SetField("Crate", g.Sum(r => r.Field<Double>("Crate")));
        //                                       row.SetField("Pcs", g.Sum(r => r.Field<Double>("Pcs")));
        //                                       return row;
        //                                   })
        //                                    .CopyToDataTable();
        //                ///after merging assign the value to dtExcelData
        //                dtExcelData = result;
        //                if (!baseObj.ExcelDataCheckForPcsApplicability(dtExcelData, Session["SCHSTATE"].ToString()))
        //                {
        //                    string message = "alert('Error in Excel Data! PCS Billing Not Allowed');";
        //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
        //                    return;
        //                }

        //                //===============================================================================
        //                string strCode = string.Empty;
        //                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
        //                conn = obj.GetConnection();

        //                #region SO  Number Generate

        //                string st = Session["SiteCode"].ToString();
        //                if (st.Length < 6)
        //                {
        //                    int len = st.Length;
        //                    int plen = 6 - len;
        //                    for (int k = 0; k < plen; k++)
        //                    {
        //                        st = "0" + st;
        //                    }
        //                }
        //                strCurrentN0 = DateTime.Now.ToString("yyMMddHHmmss");
        //                PRESO_NO = st.Substring(st.Length - 6).ToString() + strCurrentN0;

        //                string NUMSEQ = string.Empty;
        //                NUMSEQ = "1";

        //                string strPsrName = string.Empty;
        //                string strBeatName = string.Empty;

        //                if (ddlPSRName.Visible == true)
        //                {
        //                    strPsrName = ddlPSRName.SelectedItem.Value;
        //                    strBeatName = ddlBeatName.SelectedItem.Value;
        //                }

        //                #endregion

        //                #region Header Insert Data

        //                conn = obj.GetConnection();
        //                cmd = new SqlCommand("ACX_InsertSaleHeaderPre");
        //                transaction = conn.BeginTransaction();
        //                cmd.Connection = conn;
        //                cmd.Transaction = transaction;
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Clear();
        //                cmd.Parameters.AddWithValue("@statusProcedure", "Insert");
        //                cmd.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
        //                cmd.Parameters.AddWithValue("@RECID", "");
        //                cmd.Parameters.AddWithValue("@SO_NO", PRESO_NO);
        //                cmd.Parameters.AddWithValue("@PSR_CODE", strPsrName);
        //                cmd.Parameters.AddWithValue("@DELIVERY_DATE", Convert.ToDateTime(txtDeliveryDate.Text).ToString("dd-MMM-yyyy"));
        //                cmd.Parameters.AddWithValue("@SO_DATE", System.DateTime.Now.ToString("dd-MMM-yyyy"));
        //                cmd.Parameters.AddWithValue("@SO_VALUE", "5000");
        //                cmd.Parameters.AddWithValue("@PSR_BEAT", strBeatName);
        //                cmd.Parameters.AddWithValue("@CUST_REF_NO", "");
        //                cmd.Parameters.AddWithValue("@REMARK", "");
        //                cmd.Parameters.AddWithValue("@APP_SO_NO", "");
        //                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
        //                cmd.Parameters.AddWithValue("@APP_SO_DATE", "");
        //                cmd.Parameters.AddWithValue("@SO_APPROVEDATE", "");
        //                cmd.Parameters.AddWithValue("@STATUS", "0");
        //                cmd.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
        //                cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
        //                /*   ---------- GST Implementation Start ----------- */
        //                cmd.Parameters.AddWithValue("@DISTGSTINNO", Session["SITEGSTINNO"]);

        //                if (Session["SITEGSTINREGDATE"] != null && Session["SITEGSTINREGDATE"].ToString().Trim().Length > 0)
        //                {
        //                    cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", Session["SITEGSTINREGDATE"]);
        //                }
        //                cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
        //                cmd.Parameters.AddWithValue("@CUSTGSTINNO", txtGSTtin.Text);
        //                if (txtGSTtinRegistration.Text != null && txtGSTtinRegistration.Text.Trim().Length > 0)
        //                {
        //                    cmd.Parameters.AddWithValue("@CUSTGSTINREGDATE", txtGSTtinRegistration.Text);
        //                }
        //                cmd.Parameters.AddWithValue("@CUSTCOMPOSITIONSCHEME", (chkCompositionScheme.Checked == true ? 1 : 0));
        //                cmd.Parameters.AddWithValue("@BILLTOADDRESS", txtAddress.Text);
        //                cmd.Parameters.AddWithValue("@SHIPTOADDRESS", ddlShipToAddress.SelectedValue.ToString());
        //                cmd.Parameters.AddWithValue("@BILLTOSTATE", txtBilltoState.Text);
        //                /*   ---------- GST Implementation End  ----------- */
        //                cmd.ExecuteNonQuery();

        //                #endregion


        //                #region Line Insert Data on Same PURCH Order Number

        //                cmd1 = new SqlCommand("ACX_InsertSaleLinePreNew");
        //                cmd1.Connection = conn;
        //                cmd1.Transaction = transaction;
        //                cmd1.CommandTimeout = 0;
        //                cmd1.CommandType = CommandType.StoredProcedure;

        //                #endregion

        //                decimal TotalBoxQty;
        //                string boxpcs = "0.00";
        //                decimal boxQty = 0;
        //                decimal pcsQty = 0;
        //                string hsncode = "";
        //                string queryRecidLine = "Select Count(RECID) as RECID from [ax].[ACXSALESLINEPRE]";
        //                Int64 RecidLine = Convert.ToInt64(obj.GetScalarValue(queryRecidLine));
        //                int no = 0;
        //                for (int i = 0; i < dtExcelData.Rows.Count; i++)
        //                {
        //                    string sqlstr = "SELECT ItemID,PRODUCT_CRATE_PACKSIZE,PRODUCT_PACKSIZE,HSNCODE,EXEMPT From ax.inventTable WHERE ItemID = '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "'  and block=0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID='" + Convert.ToString(Session["SiteCode"]) + "')";
        //                    DataTable dtobjcheckproductcode = obj.GetData(sqlstr);
        //                    if (dtobjcheckproductcode.Rows.Count <= 0)
        //                    {
        //                        dtForShownUnuploadData.Rows.Add();
        //                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
        //                        dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
        //                        j += 1;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        //if(Session["Comm_Exem"] == null || Session["Comm_Exem"].ToString() == "")
        //                        //{
        //                        //    Session["Comm_Exem"] = dtobjcheckproductcode.Rows[0]["EXEMPT"].ToString();
        //                        //}
        //                        //
        //                        Session["Exempt_CurVal"] = dtobjcheckproductcode.Rows[0]["EXEMPT"].ToString();
        //                        //
        //                        //if(Session["Curr_Exem"].ToString() != Session["Comm_Exem"].ToString())
        //                        //{
        //                        //
        //                        //    string message = "alert('All products does not have have same Exempt value');";
        //                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
        //                        //    Session["Comm_Exem"] = null;
        //                        //    Session["Curr_Exem"] = null;
        //                        //    if (transaction != null)
        //                        //    {
        //                        //        transaction.Rollback();
        //                        //    }
        //                        //    LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : 0 Records.";
        //                        //    return;
        //                        //}
        //                        if (rdExempt.Checked == true)
        //                        {
        //                            if (Session["Exempt_CurVal"].ToString() != "1")
        //                            {
        //                                //b = false;
        //                                string message = "alert('All products does not have have same Exempt value');";
        //                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
        //                                if (transaction != null)
        //                                {
        //                                    transaction.Rollback();
        //                                }
        //                                LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : 0 Records.";
        //                                return;
        //                            }
        //                        }

        //                        if (rdNonExempt.Checked == true)
        //                        {
        //                            if (Session["Exempt_CurVal"].ToString() != "0")
        //                            {
        //                                string message = "alert('All products does not have have same Exempt value');";
        //                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
        //                                if (transaction != null)
        //                                {
        //                                    transaction.Rollback();
        //                                }
        //                                LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : 0 Records.";
        //                                return;
        //                            }
        //                        }

        //                        hsncode = dtobjcheckproductcode.Rows[0]["HSNCODE"].ToString();
        //                    }

        //                    try
        //                    {
        //                        if (Convert.ToDecimal(dtobjcheckproductcode.Rows[0]["PRODUCT_PACKSIZE"].ToString()) == 0)
        //                        {
        //                            dtForShownUnuploadData.Rows.Add();
        //                            dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
        //                            dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
        //                            j += 1;
        //                            continue;
        //                        }
        //                        else
        //                        {
        //                            TotalBoxQty = (Convert.ToDecimal(dtExcelData.Rows[i]["Crate"]) * Convert.ToDecimal(dtobjcheckproductcode.Rows[0]["PRODUCT_CRATE_PACKSIZE"]))
        //                                + Convert.ToDecimal(dtExcelData.Rows[i]["Qty"])
        //                                + (Convert.ToDecimal(dtExcelData.Rows[i]["Pcs"]) / Convert.ToDecimal(dtobjcheckproductcode.Rows[0]["PRODUCT_PACKSIZE"].ToString()));
        //                        }

        //                        boxQty = Math.Truncate(TotalBoxQty);                          //Extract Only Box Qty From Total Qty
        //                        pcsQty = Math.Round((TotalBoxQty - boxQty) * Convert.ToDecimal(dtobjcheckproductcode.Rows[0]["PRODUCT_PACKSIZE"].ToString()));
        //                        boxpcs = Convert.ToString(boxQty) + '.' + (Convert.ToString(pcsQty).Length == 1 ? "0" : "") + Convert.ToString(pcsQty);
        //                        if (TotalBoxQty == 0)
        //                        {
        //                            dtForShownUnuploadData.Rows.Add();
        //                            dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
        //                            dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
        //                            j += 1;
        //                            continue;
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        dtForShownUnuploadData.Rows.Add();
        //                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
        //                        dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
        //                        j += 1;
        //                        continue;
        //                    }


        //                    cmd1.Parameters.Clear();

        //                    string[] ReturnArray = null;

        //                    //ReturnArray = obj.CalculatePrice1(dtExcelData.Rows[i]["ProductCode"].ToString(), ddlCustomer.SelectedItem.Value, decimal.Parse(dtExcelData.Rows[i]["Qty"].ToString()), ddlEntryType.SelectedItem.Value.ToString());
        //                    ReturnArray = obj.CalculatePrice1(dtExcelData.Rows[i]["ProductCode"].ToString(), ddlCustomer.SelectedItem.Value, TotalBoxQty, "Box", Session["SiteCode"].ToString());
        //                    if (ReturnArray != null && ReturnArray[2].ToString() == "")
        //                    {
        //                        dtForShownUnuploadData.Rows.Add();
        //                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
        //                        dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
        //                        j += 1;
        //                        continue;

        //                    }
        //                    else if (ReturnArray != null && ReturnArray[2].ToString() != "")
        //                    {
        //                        DataTable dt = new DataTable();
        //                        //dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalc '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + TotalBoxQty + "," + Convert.ToDecimal(ReturnArray[2].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
        //                        if (txtAddress.Text.Trim().Length == 0)
        //                        {
        //                            string message = "alert('Please select Customers Bill To Address!');";
        //                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
        //                            ddlCustomer.Focus();
        //                            return;
        //                        }
        //                        if (txtBilltoState.Text.Trim().Length == 0)
        //                        {
        //                            string message = "alert('Please select Customers Bill To State!');";
        //                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
        //                            ddlCustomer.Focus();
        //                            return;
        //                        }
        //                        dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + TotalBoxQty + "," + Convert.ToDecimal(ReturnArray[2].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
        //                        if (dt.Rows.Count > 0)
        //                        {
        //                            cmd1.Parameters.AddWithValue("@statusProcedure", "Insert");
        //                            cmd1.Parameters.AddWithValue("@SO_NO", PRESO_NO);
        //                            cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
        //                            cmd1.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
        //                            cmd1.Parameters.AddWithValue("@PRODUCT_CODE", dtExcelData.Rows[i]["ProductCode"].ToString());
        //                            cmd1.Parameters.AddWithValue("@BOX", TotalBoxQty);
        //                            cmd1.Parameters.AddWithValue("@CRATES", ReturnArray[0].ToString());
        //                            cmd1.Parameters.AddWithValue("@LTR", ReturnArray[1].ToString());
        //                            cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(dt.Rows[0]["VALUE"]));
        //                            cmd1.Parameters.AddWithValue("@BasePrice", ReturnArray[2].ToString());
        //                            cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(dt.Rows[0]["Rate"]));

        //                            cmd1.Parameters.AddWithValue("@BOXPcs", boxpcs);

        //                            if (dt.Rows[0]["DISCTYPE"].ToString().ToUpper() == "PER")  // Percentage type
        //                            {
        //                                cmd1.Parameters.AddWithValue("@DiscType", "0");
        //                            }
        //                            else if (dt.Rows[0]["DISCTYPE"].ToString().ToUpper() == "VAL")  // Value Type
        //                            {
        //                                cmd1.Parameters.AddWithValue("@DiscType", "1");
        //                            }
        //                            else  // None
        //                            {
        //                                cmd1.Parameters.AddWithValue("@DiscType", "2");
        //                            }
        //                            cmd1.Parameters.AddWithValue("@Disc", Convert.ToDecimal(dt.Rows[0]["DISC"]));
        //                            cmd1.Parameters.AddWithValue("@DiscVal", Convert.ToDecimal(dt.Rows[0]["DISCVAL"]));
        //                            cmd1.Parameters.AddWithValue("@SECDISPER", Convert.ToDecimal(dt.Rows[0]["SECDISCPER"]));
        //                            cmd1.Parameters.AddWithValue("@SECDISVAL", Convert.ToDecimal(dt.Rows[0]["SECDISCAMT"]));
        //                            cmd1.Parameters.AddWithValue("@TDVAL", new decimal(0));
        //                            cmd1.Parameters.AddWithValue("@TD_PER", new decimal(0));
        //                            cmd1.Parameters.AddWithValue("@PE_VAL", new decimal(0));
        //                            cmd1.Parameters.AddWithValue("@PE_PER", new decimal(0));

        //                            //===========For Tax=========
        //                            cmd1.Parameters.AddWithValue("@Tax_Code", Convert.ToDecimal(dt.Rows[0]["TAX_PER"]));
        //                            cmd1.Parameters.AddWithValue("@Tax_Amount", Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]));
        //                            cmd1.Parameters.AddWithValue("@AddTax_Code", Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]));
        //                            cmd1.Parameters.AddWithValue("@AddTax_Amount", Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]));
        //                            //New Field added for GST
        //                            cmd1.Parameters.AddWithValue("@TaxComponent", dt.Rows[0]["TAX_CODE"]);
        //                            cmd1.Parameters.AddWithValue("@AddTaxComponent", dt.Rows[0]["ADDTAX_CODE"]);
        //                            cmd1.Parameters.AddWithValue("@CompositionScheme", (chkCompositionScheme.Checked == true ? 1 : 0));
        //                            cmd1.Parameters.AddWithValue("@HSNCode", dt.Rows[0]["HSNCODE"]);

        //                            //cmd1.Parameters.AddWithValue("@TaxComponent", dt.Rows[0]["TAX_CODE"]);
        //                            //cmd1.Parameters.AddWithValue("@AddTaxComponent", dt.Rows[0]["ADDTAX_CODE"]);
        //                            //cmd1.Parameters.AddWithValue("@CompositionScheme", (chkCompositionScheme.Checked == true ? 1 : 0));
        //                            //cmd1.Parameters.AddWithValue("@HSNCode", dt.Rows[0]["HSNCODE"]);
        //                            //cmd1.Parameters.AddWithValue("@TaxComponent", dt.Rows[0]["TAX_CODE"]);
        //                            //cmd1.Parameters.AddWithValue("@AddTaxComponent", dt.Rows[0]["ADDTAX_CODE"]);
        //                            //cmd1.Parameters.AddWithValue("@CompositionScheme", (chkCompositionScheme.Checked == true ? 1 : 0));
        //                            //cmd1.Parameters.AddWithValue("@HSNCode", dt.Rows[0]["HSNCODE"]);

        //                            int intCalculation = 0;

        //                            if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "PRICE")
        //                            {
        //                                intCalculation = 0;
        //                            }
        //                            else if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "MRP")
        //                            {
        //                                intCalculation = 1;
        //                            }
        //                            else
        //                            {
        //                                intCalculation = 2;
        //                            }
        //                            cmd1.Parameters.AddWithValue("@CalculationBase", intCalculation);
        //                            cmd1.Parameters.AddWithValue("@TaxableAmount", Convert.ToDecimal(dt.Rows[0]["TaxableAmount"]));
        //                            //====================
        //                            cmd1.Parameters.AddWithValue("@RECID", RecidLine + 1 + i);
        //                            cmd1.Parameters.AddWithValue("@UOM", ReturnArray[4].ToString());
        //                            cmd1.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
        //                            cmd1.ExecuteNonQuery();
        //                            no += 1;
        //                        }
        //                    }
        //                }
        //                // cmd.ExecuteNonQuery();
        //                transaction.Commit();
        //                rdExempt.Enabled = false;
        //                rdNonExempt.Enabled = false;
        //                Session["PreSoNO"] = null;
        //                Session["PreSoNO"] = PRESO_NO;
        //                BindSODetails(PRESO_NO);

        //                LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : " + no + " Records.";

        //                if (dtForShownUnuploadData.Rows.Count > 0)
        //                {
        //                    gridviewRecordNotExist.DataSource = dtForShownUnuploadData;
        //                    gridviewRecordNotExist.DataBind();
        //                    ModalPopupExtender1.Show();
        //                }
        //                else
        //                {
        //                    gridviewRecordNotExist.DataSource = null;
        //                    gridviewRecordNotExist.DataBind();
        //                }
        //            }
        //            else
        //            {
        //                LblMessage1.Text = "Please try again....!";
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage1.Text = ex.Message;
        //        LblMessage1.Visible = true;
        //        if (transaction != null)
        //        {
        //            transaction.Rollback();
        //        }
        //    }
        //    finally
        //    {
        //        if (conn != null)
        //        {
        //            if (conn.State.ToString() == "Open")
        //            {
        //                conn.Close();
        //            }
        //        }
        //    }
        //}

        protected void ImDnldTemp_Click(object sender, ImageClickEventArgs e)
        {
            ShowData_ForExcel();
        }

        protected void Exempt_CheckedChanged(object sender, EventArgs e)
        {
            DDLProductSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();
        }

    }
}