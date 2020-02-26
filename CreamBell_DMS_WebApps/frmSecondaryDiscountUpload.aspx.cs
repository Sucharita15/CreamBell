using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using Elmah;

//samarth saxena 
namespace CreamBell_DMS_WebApps
{
    public partial class frmSecondaryDiscountUpload : System.Web.UI.Page
    {
        string strQuery = string.Empty;
        SqlCommand cmd;
        int customertype = 0;
        string customertypename = "";
        int itemtype = 0;
        string itemtypename = "";
        int SDType = 0;
        double SDValue = 0;
        string err;


        string query;
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        
        
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                ViewState["Upload Try"] = 0;
                panelAddLine.Visible = true;
                AsyncFileUpload1.Visible = false;
                btnUplaod.Visible = false;
                LblMessage1.Text = "";
                lblMessage.Text = "";
                ddlCustomerTypeNew.Visible = true;
                lblCustomer.Visible = true;
                ddlCustomerNew.Visible = false;
                ddlItemNameNew.Visible = true;
                ItemTypeCode.Visible = true;
                lblCustomer.Visible = false;
                lblCustomerType.Visible = true;
                lblItemType.Visible = true;
                ddlItemTypeNew.Visible = true;
                ImDnldTemp.Visible = false;
                CalendarExtender1.StartDate = DateTime.Now;
                CalendarExtender2.StartDate = DateTime.Now;
                ddlCustomerNew.Items.Clear();


                txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");


                ddlItemNameNew.Visible = false;
                ItemTypeCode.Visible = false;
                baseObj.FillSaleHierarchy();
                fillCustomerType();
                fillItemType();
                fillSchemeType();
                fillSiteAndState();
                ShowExistingSD();

                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode == null && Convert.ToString(Session["LOGINTYPE"]) != "3")
                {
                    LnkDownloadSiteMaster.Visible = false;
                    btnGetData.Visible = false;
                }
            }
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
                LblMessage1.Text = "";
                lblMessage.Text = "";
                ddlCustomerTypeNew.Visible = true;
                lblCustomer.Visible = false;
                ddlCustomerNew.Visible = false;
                ddlItemNameNew.Visible = true;
                ddlItemTypeNew.Visible = true;
                ImDnldTemp.Visible = false;
                Lblstate.Visible = true;
                LblSiteId.Visible = true;
                ddlStateNew.Visible = true;
                ddlSiteIdNew.Visible = true;
                lblCustomerType.Visible = true;
                lblItemType.Visible = true;
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null || Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    btnGetData.Visible = true;
                }

                ddlItemNameNew.Visible = false;
                ItemTypeCode.Visible = false;
                fillCustomerType();
                fillItemType();
                fillSchemeType();




                txtValue.Text = string.Empty;

                ShowExistingSD();

                lblMessage.Text = string.Empty;
                CalendarExtender1.StartDate = DateTime.Now;
                CalendarExtender2.StartDate = DateTime.Now;
                ddlCustomerNew.Items.Clear();


                txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                Session["LineItem"] = null;



                #endregion
            }
            if (rdo.ID == "rdoExcelEntry")
            {
                #region Excel Entry

                panelAddLine.Visible = false;
                AsyncFileUpload1.Visible = true;
                btnUplaod.Visible = true;
                LblMessage1.Text = "";
                lblMessage.Text = "";
                lblCustomerType.Visible = false;
                lblItemType.Visible = false;
                ddlItemTypeNew.Visible = false;
                ddlCustomerTypeNew.Visible = false;
                lblCustomer.Visible = false;
                ddlCustomerNew.Visible = false;
                Lblstate.Visible = false;
                LblSiteId.Visible = false;
                ddlStateNew.Visible = false;
                ddlSiteIdNew.Visible = false;
                ddlItemNameNew.Visible = false;
                ItemTypeCode.Visible = false;
                ddlItemNameNew.Visible = false;
                ImDnldTemp.Visible = true;
                btnGetData.Visible = false;
                txtValue.Text = string.Empty;
                ShowExistingSD();
                lblMessage.Text = string.Empty;

                CalendarExtender1.StartDate = DateTime.Now;
                CalendarExtender2.StartDate = DateTime.Now;
                ddlCustomerNew.Items.Clear();


                txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");



                #endregion

            }


        }
        public void fillCustomerType()
        {
            ddlCustomerTypeNew.Items.Clear();
            ddlCustomerTypeNew.Items.Add("Select...");
            ddlCustomerTypeNew.Items.Add("All");
            ddlCustomerTypeNew.Items.Add("Group");
            ddlCustomerTypeNew.Items.Add("Individual");
            ddlCustomerNew.Visible = false;
            lblCustomer.Visible = false;
        }
        public void fillSchemeType()
        {
            ddlSchemeTypeNew.Items.Clear();
            ddlSchemeTypeNew.Items.Add("Percentage");
            ddlSchemeTypeNew.Items.Add("Value");

        }
        public void fillItemType()
        {
            ddlItemTypeNew.Items.Clear();
            ddlItemTypeNew.Items.Add("Select...");
            ddlItemTypeNew.Items.Add("Product Group");
            ddlItemTypeNew.Items.Add("Sub Category");
            ddlItemTypeNew.Items.Add("Individual Item");
            ddlItemNameNew.Visible = false;
            ItemTypeCode.Visible = false;
        }
        public void fillCustomerGroup()
        {
            ddlCustomerNew.Items.Clear();
            strQuery = "Select CUSTGROUP_CODE+'-'+CUSTGROUP_NAME as Name,CUSTGROUP_CODE from ax.ACXCUSTGROUPMASTER where DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' and  Blocked = 0";
            ddlCustomerNew.Items.Clear();
            ddlCustomerNew.Items.Add("Select...");
            baseObj.BindToDropDownp(ddlCustomerNew, strQuery, "Name", "CUSTGROUP_CODE");

        }
        public void FillItems()
        {
            ddlItemNameNew.Items.Clear();
            // DDLMaterialCode.Items.Add("Select...");
            strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE where block=0  order by Product_Name";
            ddlItemNameNew.Items.Clear();
            ddlItemNameNew.Items.Add("Select...");
            baseObj.BindToDropDownp(ddlItemNameNew, strQuery, "Product_Name", "Product_Code");
        }

        public void FillItemGroup()
        {

            string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' and block=0 order by a.Product_Group";
            ddlItemNameNew.Items.Clear();
            ddlItemNameNew.Items.Add("Select...");
            baseObj.BindToDropDownp(ddlItemNameNew, strProductGroup, "Product_Group", "Product_Group");
        }
        public void FillItemSubCategory()
        {

            string strProductGroup = "Select Distinct a.PRODUCT_SUBCATEGORY from ax.InventTable a where a.PRODUCT_SUBCATEGORY <>'' and block=0 order by a.PRODUCT_SUBCATEGORY";
            ddlItemNameNew.Items.Clear();
            ddlItemNameNew.Items.Add("Select...");
            baseObj.BindToDropDownp(ddlItemNameNew, strProductGroup, "PRODUCT_SUBCATEGORY", "PRODUCT_SUBCATEGORY");
        }
        public void fillCustomers()
        {

            strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0  AND SITE_CODE='" + ddlSiteIdNew.SelectedValue.ToString() + "'";
            ddlCustomerNew.Items.Clear();

            ddlCustomerNew.Items.Add("Select...");
            baseObj.BindToDropDownp(ddlCustomerNew, strQuery, "Name", "Customer_Code");
        }
        protected void ddlCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblMessage1.Text = "";
            lblMessage.Text = "";
            if (ddlCustomerTypeNew.SelectedIndex == 1)
            {
                ddlCustomerNew.Items.Clear();
                ddlCustomerNew.Visible = false;
                lblCustomer.Visible = false;

            }
            else if (ddlCustomerTypeNew.SelectedIndex == 2)
            {
                fillCustomerGroup();
                ddlCustomerNew.Visible = true;
                lblCustomer.Text = "Customer Group Name";
                lblCustomer.Visible = true;

            }
            else if (ddlCustomerTypeNew.SelectedIndex == 3)
            {
                fillCustomers();
                ddlCustomerNew.Visible = true;
                lblCustomer.Text = "Customer Name";
                lblCustomer.Visible = true;

            }
            else
            {
                ddlCustomerNew.Visible = false;

                lblCustomer.Visible = false;
            }

        }
        protected void ddlItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblMessage1.Text = "";
            lblMessage.Text = "";
            ddlItemNameNew.Visible = true;
            ItemTypeCode.Visible = true;
            ddlItemNameNew.Items.Clear();
            if (ddlItemTypeNew.SelectedIndex == 1)
            {
                FillItemGroup();
                ItemTypeCode.Text = "Select Product Group";
            }
            else if (ddlItemTypeNew.SelectedIndex == 2)
            {
                FillItemSubCategory();
                ItemTypeCode.Text = "Select Sub Category";
            }
            else if (ddlItemTypeNew.SelectedIndex == 3)
            {
                FillItems();
                ItemTypeCode.Text = "Select Product";
            }
            else
            {
                ddlItemNameNew.Visible = false;
                ItemTypeCode.Visible = false;
            }

        }
        protected void ddlSchemeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblMessage1.Text = "";
            lblMessage.Text = "";
            if (ddlSchemeTypeNew.SelectedIndex == 0)
            {
                lblSchemeType.Text = "Enter Percentage";
                lblSchemeType.Visible = true;
            }
            else
            {
                lblSchemeType.Text = "Enter Value";
                lblSchemeType.Visible = true;
            }
        }

        protected void ImDnldTemp_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ExcelTemplate/SD_TEMPLATE.xlsx");
        }
        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            Boolean ValidateInput = true;

            if (ddlCustomerTypeNew.SelectedIndex == 0)
            {
                ValidateInput = false;
                err = "Please Select Customer Type";
            }
            if (ddlItemTypeNew.SelectedIndex == 0)
            {
                ValidateInput = false;
                err = "Please Select Item Type";
            }

            if (Session["ISDISTRIBUTOR"].ToString() == "N")
            {
                if (ddlStateNew.SelectedIndex == 0)
                {
                    ValidateInput = false;
                    err = "Please Select State";
                }
                if (ddlSiteIdNew.SelectedIndex == 0)
                {
                    ValidateInput = false;
                    err = "Please Select SiteId";
                }
            }

            if (ddlCustomerTypeNew.SelectedIndex > 1 && ddlCustomerNew.SelectedIndex == 0)
            {
                err = "Please fill " + lblCustomer.Text + ".";
                ValidateInput = false;
            }
            if (ddlItemTypeNew.SelectedIndex > 0 && ddlItemNameNew.SelectedIndex == 0)
            {
                err = "Please fill" + ItemTypeCode.Text + ". ";
                ValidateInput = false;
            }
            if (txtValue.Text == "" || txtValue.Text == "0")
            {
                err = "Please " + lblSchemeType.Text + ".";
                ValidateInput = false;
            }
            if (txtFromDate.Text.ToString() != "" && txtToDate.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
                {
                    err = "'From Date' cannot be greater than 'To Date'";
                    ValidateInput = false;
                }
                if (Convert.ToDateTime(txtFromDate.Text) < Convert.ToDateTime(DateTime.Now.Date))
                {
                    err = "'From Date' cannot be greater than current date";
                    ValidateInput = false;
                }
            }
            if (txtFromDate.Text.ToString() == "" || txtToDate.Text.ToString() == "")
            {
                err = "Date is Mandatory";
                ValidateInput = false;
            }

            if (ddlSchemeTypeNew.SelectedIndex == 0)
            {
                try
                {
                    if (Convert.ToDecimal(txtValue.Text.ToString()) > 100)
                    {
                        err = "Percentage cannot be greater than 99.99%";
                        ValidateInput = false;
                    }
                }
                catch(Exception ex)
                {
                    err = "Please " + lblSchemeType.Text + ".";
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            if (ValidateInput)
            {


                if (ddlCustomerTypeNew.SelectedIndex == 1)
                {
                    customertype = 0;
                    customertypename = "All";

                }
                else if (ddlCustomerTypeNew.SelectedIndex == 2)
                {
                    customertype = 1;
                    customertypename = ddlCustomerNew.SelectedItem.Value.ToString();
                }
                else if (ddlCustomerTypeNew.SelectedIndex == 3)
                {
                    customertype = 2;
                    customertypename = ddlCustomerNew.SelectedItem.Value.ToString();
                }
                if (ddlItemTypeNew.SelectedIndex == 1)
                {
                    itemtype = 0;
                    itemtypename = ddlItemNameNew.SelectedItem.Value;
                }
                else if (ddlItemTypeNew.SelectedIndex == 2)
                {
                    itemtype = 1;
                    itemtypename = ddlItemNameNew.SelectedItem.Value;
                }
                else if (ddlItemTypeNew.SelectedIndex == 3)
                {
                    itemtype = 2;
                    itemtypename = ddlItemNameNew.SelectedItem.Value;
                }
                SDType = ddlSchemeTypeNew.SelectedIndex;
                SDValue = Convert.ToDouble(txtValue.Text.ToString());

                string CheckDuplicateRecords = "EXEC SPCheckDuplicateRecords '" + ddlSiteIdNew.SelectedValue.ToString() + "','" + itemtype + "','" + itemtypename + "','" + customertype + "','" + customertypename + "','" + txtFromDate.Text + "','" + txtToDate.Text + "','" + "1'";

                query = "insert into [ax].[ACXSECONDARYDISCOUNT] (siteid,customertype,customertypecode,ITEMTYPE,ITEMTYPECODE,sdtype,sdvalue,startingdate,endingdate,MODIFIEDBY,CREATEDBY,DATAAREAID,[status],RECID) " +
                    "values ('" + ddlSiteIdNew.SelectedValue.ToString() + "'," + customertype + ",'" + customertypename + "'," + itemtype + ",'" + itemtypename + "'," + SDType + "," + SDValue + ",'" + txtFromDate.Text + "','" + txtToDate.Text + "','" + Session["SiteCode"].ToString() + "','" + Session["SiteCode"].ToString() + "','7200',1,(select isnull(max(recid)+1,1) from [ax].[ACXSECONDARYDISCOUNT]))";
                try
                {
                    int DuplicateRecordsCount = Convert.ToInt32(baseObj.GetScalarValue(CheckDuplicateRecords));

                    if (DuplicateRecordsCount > 0)
                    {
                        lblMessage.Text = "Secondary Discount Already Uploaded For Selected Date Range";
                        lblMessage.Visible = true;
                    }
                    else
                    {
                        baseObj.ExecuteCommand(query);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "Successfully added", true);
                        lblMessage.Text = "";
                        LblMessage1.Text = "Successfully added";
                        LblMessage1.Visible = true;
                        fillCustomerType();
                        fillItemType();
                    }

                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    if (ex.ToString().Contains("Violation of PRIMARY KEY"))
                    {
                        lblMessage.Text = "Cannot insert or update duplicate record";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", ex.ToString(), true);
                        LblMessage1.Text = "";
                        lblMessage.Text = "";
                        lblMessage.Text = ex.ToString();
                        lblMessage.Visible = true;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", err.Replace("'", "''"), true);
                LblMessage1.Text = "";
                lblMessage.Text = "";
                lblMessage.Text = err;
                lblMessage.Visible = true;
            }
            ShowExistingSD();
        }
        public void ShowExistingSD()
        {
            DataTable dtSD = new DataTable();
            strQuery = "select SD.SITEID,IV.NAME AS SITENAME,(select case when [customertype] = 0 then 'All' when [customertype] = 1 then 'Group' when [customertype] = 2 then 'Individual' end) as [customertype],[customertype] Ctype," +
                    " customertypecode,(select case when [ITEMTYPE] = 0 then 'Product Group' when [ITEMTYPE] = 1 then 'Sub Category' when [ITEMTYPE] = 2 then 'Individual Item' end  ) as [ITEMTYPE],[ITEMTYPE] Itype," +
                    " ITEMTYPECODE,(select case when [sdtype] = 1 then 'Value' when [sdtype] = 0 then 'Percentage' end  )as [sdtype],[sdtype] stype,sdvalue,CONVERT(NVARCHAR, startingdate,106) startingdate,CONVERT(NVARCHAR, endingdate,106) endingdate,(select case when [status] = 1 then 'Active' when [status] = 0 then 'Inactive' end  )as [status],(SELECT CASE WHEN CAST(endingdate AS DATE) < CAST(GETDATE() AS DATE) THEN 'EXPIRED' ELSE 'RUNNING' end) AS Remark,SD.recid"
                        + " from [ax].[ACXSECONDARYDISCOUNT] SD JOIN AX.INVENTSITE IV ON SD.SITEID=IV.SITEID where SD.SITEID ='" + ddlSiteIdNew.SelectedValue.ToString() + "' and status='1' order by SD.recid desc";


            dtSD = baseObj.GetData(strQuery);
            //ViewState["GVDATA"] = dtSD;
            gvDetails.DataSource = dtSD;
            gvDetails.DataBind();
            gvDetails.Visible = true;
        }
        public static void RemoveTimezoneForDataSet(DataSet ds)
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


        protected void btnUplaod_Click(object sender, EventArgs e)
        {
            LblMessage1.Visible = false;
            SqlTransaction tran = null;
            string query = string.Empty;
            SqlConnection conn = baseObj.GetConnection();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            Boolean ValidateInputs = true;
            //if (Session["ISDISTRIBUTOR"].ToString() == "N")
            //{
            //    if (ddlState.SelectedIndex == 0)
            //    {
            //        ValidateInputs = false;
            //        err = "Please Select State";
            //    }
            //    if (ddlSiteId.SelectedIndex == 0)
            //    {
            //        ValidateInputs = false;
            //        err = "Please Select SiteId";
            //    }
            //}
            if (ValidateInputs)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    if (AsyncFileUpload1.HasFile)
                    {
                        //#region
                        string fileName = System.IO.Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                        AsyncFileUpload1.PostedFile.SaveAs(Server.MapPath("~/Uploads/" + fileName));
                        string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                        string conString = string.Empty;
                        string extension = Path.GetExtension(AsyncFileUpload1.PostedFile.FileName);
                        DataTable dtExcelData = new DataTable();

                        //excel upload
                        dtExcelData = CreamBell_DMS_WebApps.App_Code.ExcelUpload.ImportExcelXLS(Server.MapPath("~/Uploads/" + fileName), true);

                        DataTable dtFilter = new DataTable();
                        dtFilter.Columns.Add("Site_Id");
                        dtFilter.Columns.Add("Customer_Type");
                        dtFilter.Columns.Add("Customer_Type_Code");
                        dtFilter.Columns.Add("Item_Type");
                        dtFilter.Columns.Add("Item_Type_Code");
                        dtFilter.Columns.Add("SD_Value");
                        dtFilter.Columns.Add("SD_Type");
                        dtFilter.Columns.Add("From_Date");
                        dtFilter.Columns.Add("To_Date");

                        tran = conn.BeginTransaction();
                        cmd.Transaction = tran;
                        cmd.Connection = conn;
                        int j = 0;
                        for (int k = 0; k < dtExcelData.Rows.Count; k++)
                        {

                            //If Same Site , Same Transaction Type for any Item exists, System will not process the Excel Upload activity.
                            dtFilter.Rows.Add();
                            dtFilter.Rows[j]["Site_Id"] = ddlSiteIdNew.SelectedValue.ToString();
                            dtFilter.Rows[j]["Customer_Type"] = dtExcelData.Rows[k]["Customer_Type"].ToString();
                            dtFilter.Rows[j]["Customer_Type_Code"] = dtExcelData.Rows[k]["Customer_Type_Code"].ToString();
                            dtFilter.Rows[j]["Item_Type"] = dtExcelData.Rows[k]["Item_Type"].ToString();
                            dtFilter.Rows[j]["Item_Type_Code"] = dtExcelData.Rows[k]["Item_Type_Code"].ToString();
                            dtFilter.Rows[j]["SD_Value"] = dtExcelData.Rows[k]["SD_Value"].ToString();
                            //dtForShownUnuploadData.Rows[j]["ADDRESS2"] = dtExcelData.Rows[k]["ADDRESS2"].ToString();
                            dtFilter.Rows[j]["SD_Type"] = dtExcelData.Rows[k]["SD_Type"].ToString();
                            dtFilter.Rows[j]["From_Date"] = dtExcelData.Rows[k]["From_Date"].ToString();
                            dtFilter.Rows[j]["To_Date"] = dtExcelData.Rows[k]["To_Date"].ToString();

                            j += 1;
                        }


                        DataSet lds = new DataSet();
                        DataTable dt = new DataTable();
                        //lds.Tables.Add(dtForShownUnuploadData);
                        dtExcelData.TableName = "Table1";
                        lds.Tables.Add(dtExcelData);
                        RemoveTimezoneForDataSet(lds);
                        string ls_xml = lds.GetXml();
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        query = "USP_INSERT_SD";
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@XmlData", ls_xml);
                        cmd.Parameters.AddWithValue("@Site_ID", ddlSiteIdNew.SelectedValue.ToString());
                        if (Session["LOGINTYPE"].ToString() == "3")
                        {
                            cmd.Parameters.AddWithValue("@ASite_ID", Session["USERID"].ToString());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ASite_ID", Session["SiteCode"].ToString());
                        }
                        cmd.Parameters.AddWithValue("@User_type", Session["LOGINTYPE"].ToString());
                        dt.Load(cmd.ExecuteReader());
                        gridviewRemarks.DataSource = dt;
                        gridviewRemarks.DataBind();
                        ModalPopupExtender1.Show();

                        tran.Commit();
                        ShowExistingSD();
                        lblMessage.Visible = false;
                        //lblError.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ".";
                        // lblError.ForeColor = System.Drawing.Color.Green;

                    }
                }

                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    if (ex.ToString().Contains("Violation of PRIMARY KEY"))
                    {
                        lblMessage.Text = "Cannot insert or update duplicate record";
                        tran.Rollback();
                    }
                    else
                    {
                        lblMessage.Text = ex.Message;
                        //lblError.ForeColor = System.Drawing.Color.Red;
                        tran.Rollback();
                    }
                }

                finally
                {
                    if (conn != null)
                    {
                        if (conn.State.ToString() == "Open") { conn.Close(); }
                    }


                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", err.Replace("'", "''"), true);
                LblMessage1.Text = "";
                lblMessage.Text = "";
                lblMessage.Text = err;
                lblMessage.Visible = true;
            }
        }

        protected void lnkbtnStatus_Click(object sender, EventArgs e)
        {
            string query = string.Empty;
            SqlConnection conn = baseObj.GetConnection();
            cmd = new SqlCommand();
            cmd.Connection = conn;

            LinkButton btn = sender as LinkButton;
            //GridViewRow row = btn.NamingContainer as GridViewRow;
            GridViewRow row = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            string test = row.Cells[3].Text;

            string ITEMTYPE = ((HiddenField)row.FindControl("hnditemtype")).Value.ToString();// (row.Cells[2].Text);
            string recid = ((HiddenField)row.FindControl("hndrecid")).Value.ToString();// (row.Cells[2].Text);
            string ITEMTYPECODE = row.Cells[5].Text.Replace("&nbsp;", "").Replace("amp;","");
            string CUSTOMERTYPE = ((HiddenField)row.FindControl("hndcusttype")).Value.ToString(); //(row.Cells[0].Text);
            string CUSTOMERTYPECODE = row.Cells[3].Text.Replace("&nbsp;", "");
            string STARTINGDATE = row.Cells[8].Text.ToString();
            string ENDINGDATE = row.Cells[9].Text.ToString();
            try
            {
                if (btn.Text == "Active")
                {
                    query = "update ax.ACXSECONDARYDISCOUNT set STATUS=0 WHERE ITEMTYPE=" + ITEMTYPE + " AND ITEMTYPECODE='" + ITEMTYPECODE + "' AND recid='" + recid + "' AND CUSTOMERTYPE=" + CUSTOMERTYPE + " AND CUSTOMERTYPECODE='" + CUSTOMERTYPECODE + "' AND CAST(STARTINGDATE AS DATE)=CAST('" + STARTINGDATE + "' AS DATE) AND CAST(ENDINGDATE AS DATE)=CAST('" + ENDINGDATE + "' AS DATE) AND SITEID ='" + ddlSiteIdNew.SelectedValue.ToString() + "'";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    ShowExistingSD();
                    lblMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                if (ex.ToString().Contains("Violation of PRIMARY KEY"))
                {
                    lblMessage.Text = "The record which you are updating gets duplicated in table";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Text = ex.ToString();
                    lblMessage.Visible = true;
                }
            }
            //else if (btn.Text == "Inactive")
            //{
            //    query = "update ax.ACXSECONDARYDISCOUNT set STATUS=1 WHERE ITEMTYPE=" + ITEMTYPE + " AND ITEMTYPECODE='" + ITEMTYPECODE + "' AND recid='" + recid + "' AND CUSTOMERTYPE=" + CUSTOMERTYPE + " AND CUSTOMERTYPECODE='" + CUSTOMERTYPECODE + "' AND CAST(STARTINGDATE AS DATE)=CAST('" + STARTINGDATE + "' AS DATE) AND CAST(ENDINGDATE AS DATE)=CAST('" + ENDINGDATE + "' AS DATE) AND  SITEID ='" + Session["SiteCode"].ToString() + "'";
            //    cmd.CommandText = query;
            //    cmd.ExecuteNonQuery();
            //}

            ShowExistingSD();
        }
        protected void OnStatusSelect(object sender, EventArgs e)
        {
            //Accessing BoundField Column
            string name = gvDetails.SelectedRow.Cells[0].Text;

            //Accessing TemplateField Column controls

        }
        private void ExportToExcelNew()
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtSD = new DataTable();
                //strQuery = "select (select case when [customertype] = 0 then 'All' when [customertype] = 1 then 'Group' when [customertype] = 2 then 'Individual' end) as [Customer Type],[customertype] Ctype," +
                //        " customertypecode,(select case when [ITEMTYPE] = 0 then 'Product Group' when [ITEMTYPE] = 1 then 'Sub Category' when [ITEMTYPE] = 2 then 'Individual Item' end  ) as [ITEM TYPE],[ITEMTYPE] Itype," +
                //        " ITEMTYPECODE,(select case when [sdtype] = 1 then 'Value' when [sdtype] = 0 then 'Percentage' end  )as [sdtype],[sdtype] stype,sdvalue,CONVERT(NVARCHAR, startingdate,106) startingdate,CONVERT(NVARCHAR, endingdate,106) endingdate,(select case when [status] = 1 then 'Active' when [status] = 0 then 'Inactive' end  )as [status]"
                //            + "from[ax].[ACXSECONDARYDISCOUNT]"
                //            + " where SITEID ='" + Session["SiteCode"].ToString() + "' order by recid asc";

                //strQuery = "select "+"'"+"SD.siteid,IV.NAME AS SITENAME,(select case when[customertype] = 0 then 'All' when[customertype] = 1 then 'Group' when[customertype] = 2 then 'Individual' end) as [Customer Type], customertypecode as [Customer Type Code]," +
                //            " (select case when [ITEMTYPE] = 0 then 'Product Group' when[ITEMTYPE] = 1 then 'Sub Category' when[ITEMTYPE] = 2 then 'Individual Item' end  ) as [ITEM TYPE]," +
                //             " ITEMTYPECODE as [ITEM TYPE CODE],(select case when[sdtype] = 1 then 'Value' when[sdtype] = 0 then 'Percentage' end  )as [SD Type],sdvalue as [SD Value],CONVERT(NVARCHAR, startingdate, 106) [From Date],CONVERT(NVARCHAR, endingdate, 106) [TO Date],(select case when [status] = 1 then 'Active' when[status] = 0 then 'Inactive' end  )as [Status] from[ax].[ACXSECONDARYDISCOUNT] SD join AX.INVENTSITE IV ON SD.SITEID = IV.SITEID WHERE SD.SITEID = '" + ddlSiteId.SelectedValue.ToString() + "' and status = 1 order by SD.recid DESC";

                strQuery = "EXEC [AX].[EXPORTSDTOEXCEL] '" + ddlSiteIdNew.SelectedValue.ToString() + "'";

                dtSD = baseObj.GetData(strQuery);
                //dt = (DataTable)ViewState["GVDATA"];

                GridView gvvc = new GridView();
                gvvc.DataSource = dtSD;
                gvvc.DataBind();


                if (gvvc.Rows.Count > 0)
                {
                    lblMessage.Visible = false;
                    string sFileName = "SecondaryDiscountUpload.xls";

                    sFileName = sFileName.Replace("/", "");
                    // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    EnableViewState = false;

                    System.IO.StringWriter objSW = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                    foreach (GridViewRow row in gvvc.Rows)
                    {
                        //row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        { 
                            cell.CssClass = "textmode";
                        }
                    }

                    gvvc.RenderControl(objHTW);

                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(objSW.ToString());
                    Response.Flush();
                    Response.End();

                    //dg = null;
                }

            }

            catch (Exception ex)
            {
                //LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }


        protected void btnExport2Excel_Click(object sender, EventArgs e)
        {
            ExportToExcelNew();
        }

        protected void fillSiteAndState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlStateNew.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode + ' - ' + LS.Name AS NAME,LS.Name AS STATENAME from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY STATENAME ";
                ddlStateNew.Items.Add("Select...");
                baseObj.BindToDropDownp(ddlStateNew, sqlstr11, "Name", "Code");
            }
            else if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                DataTable dt = (DataTable)Session["SaleHierarchy"];
                DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATENAME");
                dtState.Columns.Add("Name", typeof(string), "STATE + ' - ' + STATENAME");
                DataView dv = dtState.DefaultView;
                dv.Sort = "STATENAME ASC";
                ddlStateNew.Items.Clear();
                ddlStateNew.DataSource = dv;
                ddlStateNew.DataTextField = "NAME";
                ddlStateNew.DataValueField = "STATE";
                ddlStateNew.DataBind();
                if (dv.Count > 1)
                {
                    ddlStateNew.Items.Insert(0, new ListItem("Select...", "Select..."));
                }


                DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                string AllSitesFromHierarchy = "";
                foreach (DataRow row in uniqueCols.Rows)
                {
                    if (AllSitesFromHierarchy == "")
                    {
                        AllSitesFromHierarchy += "'" + row["Distributor"].ToString() + "'";
                    }
                    else
                    {
                        AllSitesFromHierarchy += ",'" + row["Distributor"].ToString() + "'";
                    }
                }
                if(AllSitesFromHierarchy == "")
                {
                    AllSitesFromHierarchy = "''";
                }
                string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from AX.INVENTSITE IV JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                dt = baseObj.GetData(sqlstr1);

                ddlSiteIdNew.Items.Clear();
                ddlSiteIdNew.DataSource = dt;
                ddlSiteIdNew.DataTextField = "NAME";
                ddlSiteIdNew.DataValueField = "Code";
                ddlSiteIdNew.DataBind();
                if (dt.Rows.Count > 1)
                {
                    ddlSiteIdNew.Items.Insert(0, new ListItem("Select...", "Select..."));
                }
            }
            else
            {
                string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode + ' - ' + LS.Name as StateName,I.SiteId,I.SiteId + ' - ' + I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                //DataTable dt = baseObj.GetData(sqlstr1);
                baseObj.BindToDropDownp(ddlStateNew, sqlstr1, "StateName", "StateCode");
                baseObj.BindToDropDownp(ddlSiteIdNew, sqlstr1, "SiteName", "SiteId");
            }
        }


        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteIdNew.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME as Name,NAME as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND STATECODE = '" + ddlStateNew.SelectedItem.Value + "' Order by SiteName";
                ddlSiteIdNew.Items.Add("Select...");
                baseObj.BindToDropDownp(ddlSiteIdNew, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteIdNew.Items.Clear();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dt = (DataTable)Session["SaleHierarchy"];
                    DataTable uniqueCols = new DataTable();
                    if (ddlStateNew.SelectedItem.Text != "Select...")
                    {
                        DataTable selectedTable = dt.AsEnumerable().Where(r => r.Field<string>("STATE") == ddlStateNew.SelectedItem.Value.ToString()).CopyToDataTable();
                        uniqueCols = selectedTable.DefaultView.ToTable(true, "Distributor", "DistributorName");
                    }
                    else
                    {
                        uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                    }
                    //uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
                    string AllSitesFromHierarchy = "";
                    foreach (DataRow row in uniqueCols.Rows)
                    {
                        if (AllSitesFromHierarchy == "")
                        {
                            AllSitesFromHierarchy += "'" + row["Distributor"].ToString() + "'";
                        }
                        else
                        {
                            AllSitesFromHierarchy += ",'" + row["Distributor"].ToString() + "'";
                        }
                    }
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                    dt = baseObj.GetData(sqlstr1);
                    ddlSiteIdNew.DataSource = dt;
                    ddlSiteIdNew.DataTextField = "Name";
                    ddlSiteIdNew.DataValueField = "Code";
                    ddlSiteIdNew.DataBind();
                    ddlSiteIdNew.Items.Insert(0, "Select...");
                }
                else
                {
                    string sqlstr1 = @"Select SITEID as Code,SITEID + ' - ' + NAME as Name,NAME as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' and SITEID = '" + Session["SiteCode"].ToString() + "' Order by SiteName";
                    //ddlSiteId.Items.Add("All...");
                    baseObj.BindToDropDownp(ddlSiteIdNew, sqlstr1, "Name", "Code");
                }
            }
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowExistingSD();
            fillCustomers();
        }

        protected void LnkDownloadSiteMaster_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = new DataTable();
                //DataTable dtSD = new DataTable();
                ////strQuery = "select (select case when [customertype] = 0 then 'All' when [customertype] = 1 then 'Group' when [customertype] = 2 then 'Individual' end) as [Customer Type],[customertype] Ctype," +
                ////        " customertypecode,(select case when [ITEMTYPE] = 0 then 'Product Group' when [ITEMTYPE] = 1 then 'Sub Category' when [ITEMTYPE] = 2 then 'Individual Item' end  ) as [ITEM TYPE],[ITEMTYPE] Itype," +
                ////        " ITEMTYPECODE,(select case when [sdtype] = 1 then 'Value' when [sdtype] = 0 then 'Percentage' end  )as [sdtype],[sdtype] stype,sdvalue,CONVERT(NVARCHAR, startingdate,106) startingdate,CONVERT(NVARCHAR, endingdate,106) endingdate,(select case when [status] = 1 then 'Active' when [status] = 0 then 'Inactive' end  )as [status]"
                ////            + "from[ax].[ACXSECONDARYDISCOUNT]"
                ////            + " where SITEID ='" + Session["SiteCode"].ToString() + "' order by recid asc";

                //strQuery = "select(select case when[customertype] = 0 then 'All' when[customertype] = 1 then 'Group' when[customertype] = 2 then 'Individual' end) as [Customer Type], customertypecode as [Customer Type Code]," +
                //            " (select case when [ITEMTYPE] = 0 then 'Product Group' when[ITEMTYPE] = 1 then 'Sub Category' when[ITEMTYPE] = 2 then 'Individual Item' end  ) as [ITEM TYPE]," +
                //             " ITEMTYPECODE as [ITEM TYPE CODE],(select case when[sdtype] = 1 then 'Value' when[sdtype] = 0 then 'Percentage' end  )as [SD Type],sdvalue as [SD Value],CONVERT(NVARCHAR, startingdate, 106) [From Date],CONVERT(NVARCHAR, endingdate, 106) [TO Date],(select case when [status] = 1 then 'Active' when[status] = 0 then 'Inactive' end  )as [Status] from[ax].[ACXSECONDARYDISCOUNT] where SITEID = '" + ddlSiteId.SelectedValue.ToString() + "' and status = 1 order by recid asc";

                //dtSD = baseObj.GetData(strQuery);
                //dt = (DataTable)ViewState["GVDATA"];

                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                string strQuery = "EXEC [dbo].[SiteMaster] '" + Session["LOGINTYPE"].ToString() + "','" + Session["USERID"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(strQuery);

                GridView gvvc = new GridView();
                gvvc.DataSource = dt;
                gvvc.DataBind();


                if (gvvc.Rows.Count > 0)
                {
                    string sFileName = "SiteMaster.xls";

                    sFileName = sFileName.Replace("/", "");
                    // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    EnableViewState = false;

                    System.IO.StringWriter objSW = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                    foreach (GridViewRow row in gvvc.Rows)
                    {
                        //row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            cell.CssClass = "textmode";
                        }
                    }

                    gvvc.RenderControl(objHTW);

                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(objSW.ToString());
                    Response.Flush();
                    Response.End();
                    //dg = null;
                }

            }

            catch (Exception ex)
            {
                //LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }

        protected void btnGetData_Click(object sender, EventArgs e)
        {
            Boolean ValidateInputs = true;
            
            if (ddlStateNew.SelectedItem !=null && ddlStateNew.SelectedItem.Text == "Select...")
            {
                ValidateInputs = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please Select State !');", true);

                return;
            }
            if (ValidateInputs)
            {
                SqlCommand cmd = new SqlCommand();
                SqlTransaction tran = null;
                string query = string.Empty;
                SqlConnection conn = baseObj.GetConnection();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;

                tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                cmd.Connection = conn;
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    query = "AX.[Get_SD_Data_Statewise]";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@State", ddlStateNew.SelectedValue.ToString());
                    if (Session["LOGINTYPE"].ToString() == "3")
                    {
                        cmd.Parameters.AddWithValue("@UserCode", Session["USERID"].ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@UserCode", Session["SiteCode"].ToString());
                    }
                    cmd.Parameters.AddWithValue("@Usertype", Session["LOGINTYPE"].ToString());
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    GridView gvvc = new GridView();
                    gvvc.DataSource = dt;
                    gvvc.DataBind();


                    if (gvvc.Rows.Count > 0)
                    {
                        string sFileName = "SD_StateWiseData.xls";

                        sFileName = sFileName.Replace("/", "");
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                        Response.ContentType = "application/vnd.ms-excel";
                        EnableViewState = false;

                        System.IO.StringWriter objSW = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                        foreach (GridViewRow row in gvvc.Rows)
                        {
                            //row.BackColor = Color.White;
                            foreach (TableCell cell in row.Cells)
                            {
                                cell.CssClass = "textmode";
                            }
                        }

                        gvvc.RenderControl(objHTW);

                        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                        Response.Write(style);
                        Response.Output.Write(objSW.ToString());
                        Response.Flush();
                        Response.End();
                    }


                    tran.Commit();


                }

                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                    //lblError.ForeColor = System.Drawing.Color.Red;
                    tran.Rollback();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }

                finally
                {
                    if (conn != null)
                    {
                        if (conn.State.ToString() == "Open") { conn.Close(); }
                    }
                }
            }
        }
    }
}