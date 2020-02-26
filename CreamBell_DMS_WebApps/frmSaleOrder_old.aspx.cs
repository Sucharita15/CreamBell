using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmSaleOrder_old : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        public DataTable dtLineItems;
        public DataTable dtSchemeLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1, cmd2;
        SqlTransaction transaction;
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string strQuery = string.Empty;
        string Query1 = string.Empty;
        string Query2 = string.Empty;
        string Query3 = string.Empty;
        static int intApplicable = 0;

        string PRESO_NO = string.Empty;
        string strCurrentN0 = string.Empty;
        string SessionScheme = "SOSchemeGrid";
        string strtxtBoxQty = "0";

        // Boolean IsPageRefresh;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            chkCompositionScheme.Enabled = false;
            int fcs = Convert.ToInt32(Session["focus"]);
            if (fcs == 2)
            {
                BtnAddItem.Focus();
            }
            else if (fcs == 1)
            {
                btnGO.Focus();
            }
            else if (fcs == 3)
            {
                btnApply.Focus();
            }
            //else
            //{
            //    txtEnterQty.Focus();
            //}
            Session["focus"] = null;

            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
                CalendarExtender1.StartDate = DateTime.Now;

                ViewState["checksecdis"] = null;

                txtDeliveryDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                Session["LineItem"] = null;
                Session["SchemeLineItem"] = null;
                Session["PreSoNO"] = null;
                Session["SONO"] = null;
                //Session["Exempt_ComVal"] = null;
                Session["Exempt_CurVal"] = null;
                //Session["Comm_Exem"] = null;
                //Session["Curr_Exem"] = null;
                ViewState["OriginalData"] = null;

                ddlCustomerGroup.Focus();
                string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                ddlBusinessUnit.Items.Add("All...");
                baseObj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");
                fillCustomerGroup();
                ProductGroup();
                FillProductCode();
                if (Request.QueryString["SONO"] != null)
                {
                    string SONO = Request.QueryString["SONO"].ToString();
                    Session["PreSoNO"] = SONO;
                    BindSODetails(SONO);
                }
                if (Request.QueryString["Indent_NO"] != null && Request.QueryString["SiteId"] != null)
                {
                    string Indent_NO = Request.QueryString["Indent_NO"].ToString();
                    string CustomerCode = Request.QueryString["SiteId"].ToString();
                    BindIndentDetails(Indent_NO, CustomerCode);
                }

            }

        }

        public void BindIndentDetails(string Indent_NO, string CustomerCode)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtHeader = new DataTable();
                //=============Fill Header Details===================

                strQuery = "select A.SiteId as [CUSTOMER_CODE],A.ACXPlantName as SiteId " +
                        " from [ax].[ACXPURCHINDENTHEADER] A " +
                        " where A.Indent_No='" + Indent_NO + "' and A.ACXPlantName='" + Session["SiteCode"].ToString() + "' and A.SiteId='" + CustomerCode + "'  and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'";
                dt = baseObj.GetData(strQuery);
                if (dt.Rows.Count > 0)
                {
                    txtIndentNo.Text = Indent_NO;

                    strQuery = "select A.[CUSTOMER_CODE], A.[CUSTOMER_CODE]+'-'+A.[CUSTOMER_NAME] as CustomerName" +
                                ",B.[CUSTGROUP_CODE],B.[CUSTGROUP_CODE]+'-'+[CUSTGROUP_NAME] as CustGroupName" +
                                ",A.[PSR_CODE],A.[PSR_CODE]+'-'+C.[PSR_Name] as PSRName" +
                                ",A.[PSR_BEAT],A.[PSR_BEAT]+'-'+D.[BeatName] as BeatName" +
                                ",A.Mobile_No,A.Address1,A.State " +
                                " from [ax].[ACXCUSTMASTER] A " +
                                " Inner Join [ax].[ACXCUSTGROUPMASTER] B on A.[CUST_GROUP]=B.[CUSTGROUP_CODE]" +
                                " Left Join [ax].[ACXPSRMaster] C on C.[PSR_Code]=A.[PSR_Code]" +
                                " Left Join [ax].[ACXPSRBeatMaster] D on D.psrCode=A.psr_Code and D.BeatCode=A.Psr_Beat and D.Site_Code=A.[SITE_CODE] " +
                                // " where A.[SITE_CODE]='" + dt.Rows[0]["SiteId"].ToString() + "' and A.Customer_Code='" + dt.Rows[0]["CUSTOMER_CODE"].ToString() + "' ";
                                " where A.Customer_Code='" + dt.Rows[0]["CUSTOMER_CODE"].ToString() + "' and ( A.Customer_Code=(select SubDistributor from ax.ACX_SDLinking B where Other_Site='" + dt.Rows[0]["SiteId"].ToString() + "' and B.SubDistributor='" + dt.Rows[0]["CUSTOMER_CODE"].ToString() + "') Or A.[SITE_CODE]='" + dt.Rows[0]["SiteId"].ToString() + "' )";
                    dtHeader = baseObj.GetData(strQuery);
                    if (dtHeader.Rows.Count > 0)
                    {
                        ddlCustomerGroup.SelectedValue = dtHeader.Rows[0]["CUSTGROUP_CODE"].ToString();
                        ddlCustomerGroup_SelectedIndexChanged(this, null);
                        ddlCustomer.SelectedValue = dtHeader.Rows[0]["CUSTOMER_CODE"].ToString();
                        ddlCustomer_SelectedIndexChanged(this, null);
                        txtAddress.Text = dtHeader.Rows[0]["Address1"].ToString();
                        txtBilltoState.Text = dtHeader.Rows[0]["State"].ToString();
                    }
                }

                //=====================Delete From PreTable If The same detail is exists=====================
                string deletePre = "Select * From [ax].[ACXSALESHEADERpre] where Customer_Code='" + CustomerCode + "' and Cust_Ref_No='" + Indent_NO + "' and SiteId='" + Session["SiteCode"].ToString() + "'";
                dt = new DataTable();
                dt = baseObj.GetData(deletePre);
                if (dt.Rows.Count > 0)
                {

                    CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                    conn = obj.GetConnection();
                    cmd = new SqlCommand("ACX_USP_SDDeleteFromSalesOrderPre");
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Site_Code", Session["SITECODE"].ToString());
                    cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                    cmd.Parameters.AddWithValue("@SO_NO", dt.Rows[0]["SO_NO"].ToString());
                    cmd.Parameters.AddWithValue("@Customer_Code", dt.Rows[0]["Customer_Code"].ToString());
                    cmd.Parameters.AddWithValue("@Cust_Ref_No", dt.Rows[0]["Cust_Ref_No"].ToString());
                    cmd.ExecuteNonQuery();
                    transaction.Commit();

                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                //===========================================================================================


                FillLineItem(Indent_NO, CustomerCode);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //transaction.Rollback();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "');", true);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }

            }

        }

        public void BindSODetailsSD(string SONO)
        {
            DataTable dt = new DataTable();
            DataTable dtHeader = new DataTable();

            //================Fill Line Item Details==========
            //AddColumnInDataTable();
            Session["LineItem"] = null;
            strQuery = "Select A.[LINE_NO] as SNO,B.[PRODUCT_GROUP] as MaterialGroup,A.[PRODUCT_CODE] as ProductCodeName,B.Product_Name,A.[CRATES] as QtyCrates ,A.[BOXQty] as OnlyQtyBox,CAST(A.[PCSQTY] AS decimal(9,2)) as QtyPcs,CAST(A.[BOX] AS decimal(9,2)) as QtyBox,A.[LTR] as QtyLtr,A.[RATE] as Price,A.[Amount] as Value,A.[UOM],case when A.[DiscType]='0' then 'Per' when A.[DiscType]='1' then 'Val' else '2' end as DiscType,A.[Disc],isnull(A.Sec_Disc_Per,0) as Sec_Disc_Per,isnull(A.SEC_DISC_AMOUNT,0) as SEC_DISC_AMOUNT,isnull(A.TDValue,0) as TDValue,isnull(A.TD_Per,0) as TD_Per,isnull(A.PEValue,0) as PE,isnull(A.PE_Per,0) as PE_Per," +
                     " A.[DiscVal],A.[TAX_CODE],A.[TAX_AMOUNT],A.[ADDTAX_CODE],A.[ADDTAX_AMOUNT],B.Product_MRP as MRP,case A.[DiscCalculationBase] when 0 then 'Price' when 1 then 'MRP' else '2' end  as CalculationBase,A.[PRODUCT_CODE]+'-'+B.Product_Name as Product_Name ,ISNULL(A.BOXPCS,0.00) AS BOXPCS, case when A.[DiscType]='0' and A.[DiscCalculationBase]=1 then 'MRP' else 'Price' end as CalculationOn ,isnull(A.BasePrice,0) as BasePrice,isnull(A.TaxableAmount,0) as TaxableAmount,A.HSNCODE,A.TAXCOMPONENT,A.ADDTAXCOMPONENT,A.SchemeDiscPer as SchemeDisc,A.SchemeDiscValue as SchemeDiscVal ,ISNULL(ADDSCHDISCPER,0) ADDSCHDISCPER,ISNULL(ADDSCHDISCVAL,0) ADDSCHDISCVAL,ISNULL(ADDSCHDISCAMT,0) ADDSCHDISCAMT " +
                     " from [ax].[ACXSALESLINEPRE] A " +
                     " Inner Join ax.InventTable B on A.[PRODUCT_CODE]=B.ITEMID" +
                     " where A.SO_NO='" + SONO + "' and A.SiteId='" + Session["SiteCode"].ToString() + "' and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'" +
                     " Order By A.[LINE_NO] ";
            Session["LineItem"] = baseObj.GetData(strQuery);
            BindingGird();
            //dt = (DataTable)Session["LineItem"];
            //if (dt.Rows.Count > 0)
            //{
            //    gvDetails.DataSource = dt;
            //    gvDetails.DataBind();
            //    GridViewFooterCalculate(dt);
            //}
            //else
            //{
            //    gvDetails.DataSource = dt;
            //    gvDetails.DataBind();
            //}
            //=================For Scheme==================
            DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
            if (dtApplicable.Rows.Count > 0)
            {
                intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
            }
            if (intApplicable == 1 || intApplicable == 3)
            {
                BindSchemeGrid();
            }
            this.SchemeDiscCalculation();
        }

        public void BindSODetails(string SONO)
        {
            DataTable dt = new DataTable();
            DataTable dtHeader = new DataTable();
            //=============Fill Header Details===================
            strQuery = "Select SH.SiteId,SH.[CUSTOMER_CODE],SH.[PSR_CODE],SH.[PSR_BEAT],SH.[DELIVERY_DATE],CU.CUST_GROUP as CUSTGROUP_CODE " +
                       " From [ax].[ACXSALESHEADERPRE] SH " +
                       " Left Join ax.ACXCUSTMASTER CU On CU.[CUSTOMER_CODE]=SH.[CUSTOMER_CODE] " +
                       " Where SH.SO_NO='" + SONO + "' and SH.SiteId='" + Session["SiteCode"].ToString() + "' and SH.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'";
            dt = baseObj.GetData(strQuery);
            if (dt.Rows.Count > 0)
            {

                strQuery = "select A.[CUSTOMER_CODE], A.[CUSTOMER_CODE]+'-'+A.[CUSTOMER_NAME] as CustomerName" +
                            ",B.[CUSTGROUP_CODE],B.[CUSTGROUP_CODE]+'-'+[CUSTGROUP_NAME] as CustGroupName" +
                            ",A.[PSR_CODE],A.[PSR_CODE]+'-'+C.[PSR_Name] as PSRName" +
                            ",A.[PSR_BEAT],A.[PSR_BEAT]+'-'+D.[BeatName] as BeatName" +
                            ",A.Mobile_No,A.Address1,A.State " +
                            "from [ax].[ACXCUSTMASTER] A " +
                            " Inner Join [ax].[ACXCUSTGROUPMASTER] B on A.[CUST_GROUP]=B.[CUSTGROUP_CODE]" +
                            " Left Join [ax].[ACXPSRMaster] C on C.[PSR_Code]=A.[PSR_Code]" +
                            " Left Join [ax].[ACXPSRBeatMaster] D on D.psrCode=A.psr_Code and D.BeatCode=A.Psr_Beat and D.Site_Code=A.[SITE_CODE] " +
                            " where A.Customer_Code='" + dt.Rows[0]["CUSTOMER_CODE"].ToString() + "' " +
                            " AND (A.Customer_Code in (Select SD.SubDistributor from ax.ACX_SDLinking SD Where SD.Other_Site='" + dt.Rows[0]["SiteId"].ToString() + "' ) or A.[SITE_CODE] in ('" + dt.Rows[0]["SiteId"].ToString() + "') ) ";

                dtHeader = baseObj.GetData(strQuery);
                if (dtHeader.Rows.Count > 0)
                {
                    ddlCustomerGroup.SelectedValue = dtHeader.Rows[0]["CUSTGROUP_CODE"].ToString();
                    ddlCustomerGroup_SelectedIndexChanged(this, null);
                    ddlCustomer.SelectedValue = dtHeader.Rows[0]["CUSTOMER_CODE"].ToString();
                    ddlCustomer_SelectedIndexChanged(this, null);
                    txtAddress.Text = dtHeader.Rows[0]["Address1"].ToString();
                    txtBilltoState.Text = dtHeader.Rows[0]["State"].ToString();
                }
            }

            //================Fill Line Item Details==========
            //AddColumnInDataTable();
            Session["LineItem"] = null;
            Session["SchemeLineItem"] = null;
            strQuery = "SELECT A.[LINE_NO] AS SNO,B.[PRODUCT_GROUP] AS MaterialGroup,A.[PRODUCT_CODE] AS ProductCodeName,B.Product_Name,A.[CRATES] AS QtyCrates," +
                       " FLOOR(A.[BOX]) AS OnlyQtyBox,CAST((CAST(A.[BOX] AS decimal(18,6))-CAST(FLOOR(A.[BOX]) AS DECIMAL(9,0)))*B.Product_PackSize AS decimal(9,0)) as QtyPcs,CAST(A.[BOX] AS decimal(18,6)) as QtyBox,A.[LTR] as QtyLtr,A.[RATE] as Price,A.[Amount] as Value,A.[UOM],case when A.[DiscType]='0' then 'Per' when A.[DiscType]='1' then 'Val' else '2' end as DiscType,A.[Disc],isnull(A.Sec_Disc_Per,0) as Sec_Disc_Per,isnull(A.SEC_DISC_AMOUNT,0) as SEC_DISC_AMOUNT,isnull(A.TDValue,0) as TDValue" +
                       ",isnull(A.TD_Per,0) as TD_Per,isnull(A.PEValue,0) as PE,isnull(A.PE_Per,0) as PE_Per" +
                       ",A.[DiscVal],A.[TAX_CODE],A.[TAX_AMOUNT],A.[ADDTAX_CODE],A.[ADDTAX_AMOUNT],B.Product_MRP AS MRP,Case A.[DiscCalculationBase] WHEN 0 THEN 'Price' WHEN 1 THEN 'MRP' ELSE '2' END  AS CalculationBase,A.[PRODUCT_CODE]+'-'+B.Product_Name as Product_Name ,ISNULL(A.BOXPCS,0.00) AS BOXPCS, case when A.[DiscType]='0' and A.[DiscCalculationBase]=1 then 'MRP' else 'Price' end as CalculationOn ,isnull(A.BasePrice,0) as BasePrice,isnull(A.TaxableAmount,0) as TaxableAmount,A.HSNCODE,A.TAXCOMPONENT,A.ADDTAXCOMPONENT,A.SchemeDiscPer as SchemeDisc,A.SchemeDiscValue as SchemeDiscVal,ISNULL(ADDSCHDISCPER,0) ADDSCHDISCPER,ISNULL(ADDSCHDISCVAL,0) ADDSCHDISCVAL,ISNULL(ADDSCHDISCAMT,0) ADDSCHDISCAMT" +
                       " FROM [AX].[ACXSALESLINEPRE] A " +
                       " INNER JOIN ax.InventTable B on A.[PRODUCT_CODE]=B.ITEMID" +
                       " WHERE A.SO_NO='" + SONO + "' and A.SiteId='" + Session["SiteCode"].ToString() + "' and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'" +
                       " ORDER BY A.[LINE_NO] ";
            Session["LineItem"] = baseObj.GetData(strQuery);
            dt = (DataTable)Session["LineItem"];
            if (dt.Rows.Count > 0)
            { txtHdnTDValue.Text = dt.AsEnumerable().Sum(row => row.Field<decimal>("TDValue")).ToString(); }
            //string productcode = dt.Rows[0]["ProductCodeName"].ToString();
            string query1 = "select exempt from ax.inventtable where itemid = '" + dt.Rows[0]["ProductCodeName"].ToString() + "'";
            string exempt = baseObj.GetScalarValue(query1);
            if (exempt == "1")
            {
                rdExempt.Checked = true;
                rdNonExempt.Checked = false;
            }
            else
            {
                rdExempt.Checked = false;
                rdNonExempt.Checked = true;
            }
            rdExempt.Enabled = false;
            rdNonExempt.Enabled = false;
            ProductGroup();
            FillProductCode();
            BindingGird();
            //dt = (DataTable)Session["LineItem"];
            //if (dt.Rows.Count > 0)
            //{
            //    gvDetails.DataSource = dt;
            //    gvDetails.DataBind();
            //    GridViewFooterCalculate(dt);
            //}
            //else
            //{
            //    gvDetails.DataSource = dt;
            //    gvDetails.DataBind();
            //}
            //=================For Scheme==================
            DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
            if (dtApplicable.Rows.Count > 0)
            {
                intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
            }
            if (intApplicable == 1 || intApplicable == 3)
            {
                BindSchemeGrid();
            }
            this.SchemeDiscCalculation();
        }

        public void FillLineItem(string Indent_NO, string CustomerCode)
        {
            try
            {

                strQuery = @"select A.Line_No as SNO,A.Product_Group as MaterialGroup,A.Product_Code as ProductCode,cast(A.Crates AS decimal(10,2)) as QtyCrates,cast(A.Box AS decimal(10,2)) as Qty,cast(A.Ltr as decimal(10,2)) QtyLtr
                           ,BoxPcs=cast(A.Box AS decimal(10,2)),OnlyQtyBox=cast(A.Box AS decimal(10,2)),QtyPcs=0
                           ,Product_PackSize=coalesce(cast((select Product_PackSize from ax.inventtable where itemid=A.Product_Code) as int),1)
                           ,PRODUCT_CRATE_PACKSIZE=coalesce(cast((select PRODUCT_CRATE_PACKSIZE from ax.inventtable where itemid=A.Product_Code) as int),1)
                           from ax.ACXPurchIndentLine A 
                           Inner Join ax.ACXPURCHINDENTHEADER AH on A.Indent_No  = AH.Indent_No and A.SiteId = AH.SiteId
                           Where A.Indent_No='" + Indent_NO + "' and AH.[ACXPLANTNAME]='" + Session["SiteCode"].ToString() + "'  and AH.SiteId='" + CustomerCode + "' order by A.Line_no";

                DataTable dtExcelData = new DataTable();

                dtExcelData = baseObj.GetData(strQuery);

                string strCode = string.Empty;
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                conn = obj.GetConnection();

                #region SO  Number Generate

                string st = Session["SiteCode"].ToString();
                if (st.Length < 6)
                {
                    int len = st.Length;
                    int plen = 6 - len;
                    for (int k = 0; k < plen; k++)
                    {
                        st = "0" + st;
                    }
                }
                strCurrentN0 = DateTime.Now.ToString("yyMMddHHmmss");
                PRESO_NO = st.Substring(st.Length - 6).ToString() + strCurrentN0;

                string NUMSEQ = string.Empty;
                NUMSEQ = "1";

                string strPsrName = string.Empty;
                string strBeatName = string.Empty;

                if (ddlPSRName.Visible == true)
                {
                    strPsrName = ddlPSRName.SelectedItem.Value;
                    strBeatName = ddlBeatName.SelectedItem.Value;
                }

                #endregion

                #region Header Insert Data

                conn = obj.GetConnection();
                cmd = new SqlCommand("ACX_InsertSaleHeaderPre");
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@statusProcedure", "Insert");
                //cmd.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@CUSTOMER_CODE", CustomerCode);
                cmd.Parameters.AddWithValue("@RECID", "");
                cmd.Parameters.AddWithValue("@SO_NO", PRESO_NO);
                cmd.Parameters.AddWithValue("@PSR_CODE", strPsrName);
                cmd.Parameters.AddWithValue("@DELIVERY_DATE", Convert.ToDateTime(txtDeliveryDate.Text).ToString("dd-MMM-yyyy"));
                cmd.Parameters.AddWithValue("@SO_DATE", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                cmd.Parameters.AddWithValue("@SO_VALUE", "5000");
                cmd.Parameters.AddWithValue("@PSR_BEAT", strBeatName);
                cmd.Parameters.AddWithValue("@CUST_REF_NO", txtIndentNo.Text);
                cmd.Parameters.AddWithValue("@REMARK", "");
                cmd.Parameters.AddWithValue("@APP_SO_NO", "");
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@APP_SO_DATE", "");
                cmd.Parameters.AddWithValue("@SO_APPROVEDATE", "");
                cmd.Parameters.AddWithValue("@STATUS", "0");
                cmd.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                /*   ---------- GST Implementation Start ----------- */
                cmd.Parameters.AddWithValue("@DISTGSTINNO", Session["SITEGSTINNO"]);

                if (Session["SITEGSTINREGDATE"] != null && Session["SITEGSTINREGDATE"].ToString().Trim().Length > 0)
                {
                    cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", Session["SITEGSTINREGDATE"]);
                }
                cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                cmd.Parameters.AddWithValue("@CUSTGSTINNO", txtGSTtin.Text);
                if (txtGSTtinRegistration.Text != null && txtGSTtinRegistration.Text.Trim().Length > 0)
                {
                    cmd.Parameters.AddWithValue("@CUSTGSTINREGDATE", txtGSTtinRegistration.Text);
                }
                cmd.Parameters.AddWithValue("@CUSTCOMPOSITIONSCHEME", (chkCompositionScheme.Checked == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@BILLTOADDRESS", txtAddress.Text);
                cmd.Parameters.AddWithValue("@SHIPTOADDRESS", ddlShipToAddress.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@BILLTOSTATE", txtBilltoState.Text);
                /*   ---------- GST Implementation End  ----------- */
                cmd.ExecuteNonQuery();

                #endregion


                #region Line Insert Data on Same PURCH Order Number

                cmd1 = new SqlCommand("ACX_InsertSaleLinePreNew");
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.StoredProcedure;

                #endregion

                string queryRecidLine = "Select Count(RECID) as RECID from [ax].[ACXSALESLINEPRE]";
                Int64 RecidLine = Convert.ToInt64(obj.GetScalarValue(queryRecidLine));

                DataTable dtForShownUnuploadData = new DataTable();
                dtForShownUnuploadData.Columns.Add("ProductCode");
                dtForShownUnuploadData.Columns.Add("Qty");
                decimal TotalBoxQty;
                string hsncode = "";
                int j = 0;
                int no = 0;
                for (int i = 0; i < dtExcelData.Rows.Count; i++)
                {
                    string sqlstr = "select ItemID,HSNCODE from ax.inventTable where block=0 and ItemID = '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "'";
                    DataTable dtobjcheckproductcode = obj.GetData(sqlstr);
                    if (dtobjcheckproductcode.Rows.Count <= 0)
                    {
                        dtForShownUnuploadData.Rows.Add();
                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                        dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                        j += 1;
                        continue;
                    }
                    else
                    {
                        hsncode = dtobjcheckproductcode.Rows[0]["HSNCODE"].ToString();
                    }
                    try
                    {
                        TotalBoxQty = Convert.ToDecimal(dtExcelData.Rows[i]["Qty"]);//(Convert.ToDecimal(dtExcelData.Rows[i]["QtyCrates"]) * Convert.ToDecimal(dtExcelData.Rows[i]["PRODUCT_CRATE_PACKSIZE"]))
                        if (TotalBoxQty == 0)
                        {
                            dtForShownUnuploadData.Rows.Add();
                            dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                            dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                            j += 1;
                            continue;
                        }
                    }
                    catch(Exception ex)
                    {
                        dtForShownUnuploadData.Rows.Add();
                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                        dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                        j += 1;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        continue;
                    }

                    cmd1.Parameters.Clear();
                    string[] ReturnArray = null;
                    ReturnArray = obj.CalculatePrice1(dtExcelData.Rows[i]["ProductCode"].ToString(), CustomerCode, App_Code.Global.ParseDecimal(dtExcelData.Rows[i]["Qty"].ToString()), "Box", Session["SiteCode"].ToString());
                    if (ReturnArray != null && ReturnArray[2].ToString() == "")
                    {
                        dtForShownUnuploadData.Rows.Add();
                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                        dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                        j += 1;
                        continue;

                    }
                    else if (ReturnArray != null && ReturnArray[2].ToString() != "")
                    {
                        if (txtAddress.Text.Trim().Length == 0)
                        {
                            string message = "alert('Please select Customer Bill To Address!');";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                            ddlCustomer.Focus();
                            return;
                        }
                        if (txtBilltoState.Text.Trim().Length == 0)
                        {
                            string message = "alert('Please select Customer Bill To State!');";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                            ddlCustomer.Focus();
                            return;
                        }

                        DataTable dt = new DataTable();
                        string abc = "EXEC USP_ACX_GetSalesLineCalcGST '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(dtExcelData.Rows[i]["Qty"].ToString()) + "," + Convert.ToDecimal(ReturnArray[2].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'";
                        dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(dtExcelData.Rows[i]["Qty"].ToString()) + "," + Convert.ToDecimal(ReturnArray[2].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                        if (dt.Rows.Count > 0)
                        {
                            cmd1.Parameters.AddWithValue("@statusProcedure", "Insert");
                            cmd1.Parameters.AddWithValue("@SO_NO", PRESO_NO);
                            cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                            cmd1.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                            cmd1.Parameters.AddWithValue("@PRODUCT_CODE", dtExcelData.Rows[i]["ProductCode"].ToString());
                            cmd1.Parameters.AddWithValue("@BOX", TotalBoxQty);
                            cmd1.Parameters.AddWithValue("@BOXQty", TotalBoxQty);
                            cmd1.Parameters.AddWithValue("@BOXPcs", TotalBoxQty);
                            cmd1.Parameters.AddWithValue("@PcsQty", 0);
                            cmd1.Parameters.AddWithValue("@CRATES", ReturnArray[0].ToString());
                            cmd1.Parameters.AddWithValue("@LTR", ReturnArray[1].ToString());
                            cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(dt.Rows[0]["VALUE"]));
                            cmd1.Parameters.AddWithValue("@BasePrice", ReturnArray[2].ToString());
                            cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(dt.Rows[0]["Rate"]));
                            cmd1.Parameters.AddWithValue("@SECDISPER", Convert.ToDecimal(dt.Rows[0]["SECDISCPER"]));
                            cmd1.Parameters.AddWithValue("@SECDISVAL", Convert.ToDecimal(dt.Rows[0]["SECDISCAMT"]));

                            if (dt.Rows[0]["DISCTYPE"].ToString().ToUpper() == "PER")  // Percentage type
                            {
                                cmd1.Parameters.AddWithValue("@DiscType", "0");
                            }
                            else if (dt.Rows[0]["DISCTYPE"].ToString().ToUpper() == "VAL")  // Value Type
                            {
                                cmd1.Parameters.AddWithValue("@DiscType", "1");
                            }
                            else  // None
                            {
                                cmd1.Parameters.AddWithValue("@DiscType", "2");
                            }
                            cmd1.Parameters.AddWithValue("@Disc", Convert.ToDecimal(dt.Rows[0]["DISC"]));
                            cmd1.Parameters.AddWithValue("@DiscVal", Convert.ToDecimal(dt.Rows[0]["DISCVAL"]));
                            cmd1.Parameters.AddWithValue("@Tax_Code", Convert.ToDecimal(dt.Rows[0]["TAX_PER"]));
                            cmd1.Parameters.AddWithValue("@Tax_Amount", Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]));
                            cmd1.Parameters.AddWithValue("@AddTax_Code", Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]));
                            cmd1.Parameters.AddWithValue("@AddTax_Amount", Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]));

                            cmd1.Parameters.AddWithValue("@TaxComponent", dt.Rows[0]["TAX_CODE"]);
                            cmd1.Parameters.AddWithValue("@AddTaxComponent", dt.Rows[0]["ADDTAX_CODE"]);
                            cmd1.Parameters.AddWithValue("@CompositionScheme", (chkCompositionScheme.Checked == true ? 1 : 0));
                            cmd1.Parameters.AddWithValue("@HSNCode", dt.Rows[0]["HSNCODE"]);

                            int intCalculation = 0;

                            if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "PRICE")
                            {
                                intCalculation = 0;
                            }
                            else if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "MRP")
                            {
                                intCalculation = 1;
                            }
                            else
                            {
                                intCalculation = 2;
                            }
                            cmd1.Parameters.AddWithValue("@CalculationBase", intCalculation);
                            cmd1.Parameters.AddWithValue("@TaxableAmount", Convert.ToDecimal(dt.Rows[0]["TaxableAmount"]));
                            //====================

                            cmd1.Parameters.AddWithValue("@RECID", RecidLine + 1 + i);
                            cmd1.Parameters.AddWithValue("@UOM", ReturnArray[4].ToString());
                            cmd1.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                            cmd1.ExecuteNonQuery();
                            no += 1;
                        }
                    }

                }



                // cmd.ExecuteNonQuery();
                transaction.Commit();
                Session["PreSoNO"] = null;
                Session["PreSoNO"] = PRESO_NO;
                BindSODetailsSD(PRESO_NO);


                //LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : " + no + " Records.";

                if (dtForShownUnuploadData.Rows.Count > 0)
                {
                    gridviewRecordNotExist.DataSource = dtForShownUnuploadData;
                    gridviewRecordNotExist.DataBind();
                    ModalPopupExtender1.Show();
                }
                else
                {
                    gridviewRecordNotExist.DataSource = null;
                    gridviewRecordNotExist.DataBind();
                }
            }


            catch (Exception ex)
            {
                LblMessage1.Text = "";
                //transaction.Rollback();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                //conn.Close();
                // conn.Dispose();
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQuery = string.Empty;
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            string query = "USP_DropdownGetProductSubGroup";
            ilist.Add("@USERCODE"); litem.Add(Session["USERID"].ToString());
            ilist.Add("@IsStock"); litem.Add("0");
            ilist.Add("@Block"); litem.Add("1");
            ilist.Add("@ProductGroup"); litem.Add(DDLProductGroup.SelectedValue.ToString());
            DataTable dtBinddropdown = baseObj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);
            DDLProductSubCategory.DataSource = dtBinddropdown;
            DDLProductSubCategory.DataTextField = "CODE";
            DDLProductSubCategory.DataValueField = "CODE_DISC";
            DDLProductSubCategory.DataBind();
            DDLProductSubCategory.Items.Insert(0, new ListItem("--Select--", ""));
            FillProductCode();
            DDLProductSubCategory.Focus();

            //strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
            //             + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "' and P.block=0";
            //DDLProductSubCategory.Items.Clear();
            //DDLProductSubCategory.Items.Add("Select...");
            //baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            //// FillProductCode();
            //DDLProductSubCategory.Focus();

            //string strcondition = "";
            //if (DDLProductGroup.SelectedIndex > 0)
            //{ strcondition += " and inv.Product_Group = '" + DDLProductGroup.SelectedValue + "' "; }
            //strQuery = "SELECT distinct inv.ITEMID+'-'+inv.Product_Name as Name,inv.ITEMID from ax.INVENTTABLE inv where INV.block=0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')" + strcondition  ;
            //if (rdStock.Checked == true)
            //{
            //    strQuery = "SELECT distinct inv.ITEMID + '-' + inv.Product_Name as Name,inv.ITEMID from ax.INVENTTABLE inv" +
            //                " inner join AX.ACXINVENTTRANS TT on inv.itemid = tt.productcode and SITECODE = '" + Session["SiteCode"].ToString() + "'" +
            //                " inner join ax.INVENTSITE i on tt.TransLocation = i.MAINWAREHOUSE and i.SITEID = TT.SiteCode where " +
            //                " INV.block = 0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "') " + strcondition + " GROUP BY inv.ITEMID,inv.Product_Name having SUM(TT.TransQty) > 0";
            //}
            //DDLMaterialCode.DataSource = null;
            //DDLMaterialCode.Items.Clear();
            //DDLMaterialCode.Items.Add("Select...");
            //baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Name", "ITEMID");

            txtCrate.Text = "";
            txtPcs.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtEnterQty.Text = string.Empty;

            PcsBillingApplicable();
        }

        public void PcsBillingApplicable()
        {

            DataTable dt = baseObj.GetPcsBillingApplicability(Session["SCHSTATE"].ToString(), DDLProductGroup.SelectedItem.Value);  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
            string ProductGroupApplicable = string.Empty;

            if (dt.Rows.Count > 0)
            {
                ProductGroupApplicable = dt.Rows[0][1].ToString();
            }
            if (ProductGroupApplicable == "Y")
            {
                txtPcs.Enabled = true;
            }
            else
            {
                txtPcs.Enabled = false;
            }
        }

        public void fillCustomerGroup()
        {
            strQuery = "Select CUSTGROUP_CODE+'-'+CUSTGROUP_NAME as Name,CUSTGROUP_CODE from ax.ACXCUSTGROUPMASTER where DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' and  Blocked = 0";
            ddlCustomerGroup.Items.Clear();
            ddlCustomerGroup.Items.Add("Select...");
            baseObj.BindToDropDown(ddlCustomerGroup, strQuery, "Name", "CUSTGROUP_CODE");
            ddlCustomer.Items.Add("Select...");
        }

        private void ClearAll()
        {

        }
        public void ProductGroup()
        {
            string strQuery = string.Empty;
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            string query = "USP_DropdownGetProductGroup";
            ilist.Add("@USERCODE"); litem.Add(Session["USERID"].ToString());
            if (ddlBusinessUnit.SelectedItem.Text == "All...")
            {
                ilist.Add("@BU_CODE"); litem.Add("");
            }
            else
            {
                ilist.Add("@BU_CODE"); litem.Add(ddlBusinessUnit.SelectedItem.Value.ToString());
            }
            if (rdStock.Checked == true)
            {
                ilist.Add("@IsStock"); litem.Add("1");
            }
            else
            {
                ilist.Add("@IsStock"); litem.Add("0");
            }
            if (rdExempt.Checked == true)
            {
                ilist.Add("@EXEMPT"); litem.Add("1");
            }
            else
            {
                ilist.Add("@EXEMPT"); litem.Add("0");
            }
            ilist.Add("@Block"); litem.Add("1");
            DataTable dtBinddropdown = baseObj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);

            DDLProductGroup.DataSource = dtBinddropdown;
            DDLProductGroup.DataTextField = "CODE";
            DDLProductGroup.DataValueField = "CODE_DESC";
            DDLProductGroup.DataBind();
            DDLProductGroup.Items.Insert(0, new ListItem("--Select--", ""));
        }
        //public void ProductGroup()
        //{


        //    if (ddlBusinessUnit.SelectedItem.Text == "All...")
        //    { 
        //        DDLProductGroup.Items.Clear();
        //        string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' and A.block=0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
        //        if (rdStock.Checked == true)
        //        {
        //            strProductGroup += " AND a.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
        //        }
        //        strProductGroup += " order by a.Product_Group";
        //        DDLProductGroup.Items.Clear();
        //        DDLProductGroup.Items.Add("Select...");
        //        baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

        //    }
        //    else
        //    {
        //        DDLProductGroup.Items.Clear();
        //        string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>''  and A.block=0 and BU_CODE in ('" + ddlBusinessUnit.SelectedItem.Value.ToString() + "') ";
        //        if (rdStock.Checked == true)
        //        {
        //            strProductGroup += " AND a.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
        //        }
        //        DDLProductGroup.Items.Clear();
        //        strProductGroup += " order by a.Product_Group";
        //        DDLProductGroup.Items.Add("Select...");
        //        baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");
        //    }
        //    //string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>''  and A.block=0 order by a.Product_Group";
        //    //string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>''  and A.block=0 and BU_CODE in(select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Session["SiteCode"].ToString() + "') order by a.Product_Group";
        //    //DDLProductGroup.Items.Clear();
        //    //DDLProductGroup.Items.Add("Select...");
        //    //baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

        //}
        public void ProductSubCategory()
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P WHERE P.block=0 "
                        + " and P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            // FillProductCode();
            DDLProductSubCategory.Focus();
        }

        private DataTable AddLineItems()
        {

            try
            {
                //decimal SecDiscPer, Price , SecDiscVal,TDVal,BasePrice;

                //Price = Convert.ToDecimal(txtPrice.Text);
                int count;
                if (string.IsNullOrEmpty(txtSDiscPer.Text))
                {
                    txtSDiscPer.Text = "0";
                }
                if (string.IsNullOrEmpty(txtSDiscVal.Text))
                {
                    // SecDiscPer = Convert.ToDecimal(txtSDiscPer.Text);

                    //SecDiscVal = (Price * SecDiscPer) / 100;
                    txtSDiscVal.Text = "0";
                }
                // else
                //{
                //  SecDiscVal = Convert.ToDecimal(txtSDiscVal.Text);
                //}

                //if (string.IsNullOrEmpty(txtTDValue.Text))
                //{
                //    txtTDValue.Text = "0";
                //
                //}

                //TDVal = Convert.ToDecimal(txtTDValue.Text);
                //BasePrice = Price - SecDiscVal - TDVal;


                if (Session["LineItem"] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session["LineItem"];
                    if (dtLineItems.Rows.Count == 0)
                        AddColumnInDataTable();
                }
                DataRow[] dataPerDay = (from myRow in dtLineItems.AsEnumerable()
                                        where myRow.Field<string>("ProductCodeName") == DDLMaterialCode.SelectedValue.ToString()
                                        select myRow).ToArray();
                if (dataPerDay.Count() == 0)
                {
                    //===========================

                    #region  Insert into Session
                    if (txtBilltoState.Text.Trim().Length == 0)
                    {
                        string message = "alert('Please select Customers Bill To State!');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        ddlCustomer.Focus();
                        return dtLineItems;
                    }

                    DataTable dt = new DataTable();
                    string st123 = "EXEC USP_ACX_GetSalesLineCalcGST '" + DDLMaterialCode.SelectedValue.ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtQtyBox.Text.Trim()) + "," + Convert.ToDecimal(txtPrice.Text.Trim()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'";
                    dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + DDLMaterialCode.SelectedValue.ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtQtyBox.Text.Trim()) + "," + Convert.ToDecimal(txtPrice.Text.Trim()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                    // dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + DDLMaterialCode.SelectedValue.ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtQtyBox.Text.Trim()) + "," + BasePrice + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                    //String que = "EXEC USP_ACX_GetSalesLineCalcGST '" + DDLMaterialCode.SelectedValue.ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtQtyBox.Text.Trim()) + "," + Convert.ToDecimal(txtPrice.Text.Trim()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'";

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["RETMSG"].ToString().IndexOf("FALSE") >= 0)
                        {
                            string message = "alert('" + dt.Rows[0]["RETMSG"].ToString().Replace("FALSE|", "") + "');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            DDLMaterialCode.Focus();
                            return dtLineItems;
                        }
                        DataRow row;
                        row = dtLineItems.NewRow();
                        //int count = 1 + dtLineItems.Rows.Count;
                        //decimal tTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("Tax_Amount"));
                        if (dtLineItems.Rows.Count == 0)
                        {
                            count = 1 + dtLineItems.Rows.Count;
                        }
                        else
                        {
                            count = 1 + Convert.ToInt32(dtLineItems.AsEnumerable().Max(roww => roww.Field<int>("SNO")));
                        }

                        row["SNO"] = count;
                        row["MaterialGroup"] = DDLProductGroup.SelectedItem.Text.ToString();
                        row["ProductCodeName"] = DDLMaterialCode.SelectedValue.ToString();
                        row["Product_Name"] = DDLMaterialCode.SelectedItem.Text.ToString();
                        row["QtyCrates"] = Convert.ToDecimal(txtQtyCrates.Text.Trim().ToString());
                        row["OnlyQtyBox"] = Convert.ToDecimal(txtViewTotalBox.Text.Trim().ToString());
                        row["QtyBox"] = decimal.Parse(txtQtyBox.Text.Trim().ToString());

                        row["BoxPcs"] = Convert.ToDecimal(txtBoxPcs.Text.Trim());
                        row["QtyPcs"] = Convert.ToDecimal(txtViewTotalPcs.Text.Trim().ToString());
                        row["QtyLtr"] = Convert.ToDecimal(txtLtr.Text.Trim().ToString());
                        row["Price"] = Convert.ToDecimal(dt.Rows[0]["Rate"]);
                        row["BasePrice"] = Convert.ToDecimal(txtPrice.Text);
                        //row["BasePrice"] = Price;
                        row["Value"] = Convert.ToDecimal(dt.Rows[0]["VALUE"]);
                        row["UOM"] = lblHidden.Text;

                        row["DiscType"] = dt.Rows[0]["DISCTYPE"].ToString();
                        row["Disc"] = Convert.ToDecimal(dt.Rows[0]["DISC"]);
                        row["discval"] = Convert.ToDecimal(dt.Rows[0]["discval"]);
                        //row["Sec_Disc_Per"] = Convert.ToDecimal(txtSDiscPer.Text.Trim().ToString());
                        //row["SEC_DISC_AMOUNT"] = Convert.ToDecimal(txtSDiscVal.Text.Trim().ToString());
                        ////row["SEC_DISC_AMOUNT"] = SecDiscVal;

                        //row["Sec_Disc_Per"] = new decimal(0);
                        //row["SEC_DISC_AMOUNT"] = new decimal(0);
                        row["Sec_Disc_Per"] = Convert.ToDecimal(dt.Rows[0]["SECDISCPER"]);
                        row["SEC_DISC_AMOUNT"] = Convert.ToDecimal(dt.Rows[0]["SECDISCAMT"]);

                        //row["TDValue"] = Convert.ToDecimal(txtTDValue.Text.Trim().ToString());
                        row["TDValue"] = new decimal(0);
                        row["PE"] = new decimal(0);
                        row["PE_Per"] = new decimal(0);
                        row["TD_Per"] = new decimal(0);
                        //==========For Tax===============
                        row["Tax_Code"] = Convert.ToDecimal(dt.Rows[0]["TAX_PER"]);
                        row["Tax_Amount"] = Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]);
                        row["AddTax_Code"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]);
                        row["AddTax_Amount"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]);

                        row["MRP"] = Convert.ToDecimal(dt.Rows[0]["MRP"]);
                        row["TaxableAmount"] = Convert.ToDecimal(dt.Rows[0]["TaxableAmount"]);

                        row["SchemeDisc"] = new decimal(0);
                        row["SchemeDiscVal"] = new decimal(0);
                        row["ADDSCHDISCPER"] = new decimal(0);
                        row["ADDSCHDISCVAL"] = new decimal(0);
                        row["ADDSCHDISCAMT"] = new decimal(0);
                        /*  ----- GST Implementation Start */
                        row["TaxComponent"] = dt.Rows[0]["TAX_CODE"].ToString();
                        row["AddTaxComponent"] = dt.Rows[0]["ADDTAX_CODE"].ToString();
                        row["HSNCode"] = dt.Rows[0]["HSNCODE"].ToString();
                        /*  ----- GST Implementation End */

                        int intCalculation = 0;
                        string calculationBase = "";
                        //if (dt.Rows[0]["DISCTYPE"].ToString() == "Per" && dt.Rows[0]["CALCULATIONBASE"].ToString() == "MRP")
                        if (dt.Rows[0]["CALCULATIONBASE"].ToString() == "MRP")
                        {
                            row["CalculationOn"] = "MRP";
                        }
                        else
                        {
                            row["CalculationOn"] = "Price";
                        }
                        if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "PRICE")
                        {
                            calculationBase = "PRICE";
                        }
                        else if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "MRP")
                        {
                            calculationBase = "MRP";
                        }
                        else
                        {
                            calculationBase = "2";
                        }
                        row["CalculationBase"] = calculationBase.ToString();
                        dtLineItems.Rows.Add(row);
                        #region Insert PreTable

                        string strPsrName = string.Empty;
                        string strBeatName = string.Empty;

                        if (ddlPSRName.Visible == true)
                        {
                            strPsrName = ddlPSRName.SelectedItem.Value;
                            strBeatName = ddlBeatName.SelectedItem.Value;
                        }
                        CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                        //string SO_NO = ddlCustomer.SelectedItem.Value + System.DateTime.Now.ToString("yymmddhhmmss");
                        //--code logic by Amrita--//
                        string st = Session["SiteCode"].ToString();
                        if (st.Length < 6)
                        {
                            int len = st.Length;
                            int plen = 6 - len;
                            for (int j = 0; j < plen; j++)
                            {
                                st = "0" + st;
                            }
                        }

                        if (Session["LineItem"] == null)
                        {
                            strCurrentN0 = DateTime.Now.ToString("yyMMddHHmmss");
                            PRESO_NO = st.Substring(st.Length - 6).ToString() + strCurrentN0;

                            Session["SONO"] = PRESO_NO;

                            string NUMSEQ = string.Empty;
                            NUMSEQ = "1";

                            conn = obj.GetConnection();
                            cmd = new SqlCommand("ACX_InsertSaleHeaderPre");
                            transaction = conn.BeginTransaction();
                            cmd.Connection = conn;
                            cmd.Transaction = transaction;
                            cmd.CommandTimeout = 3600;
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@statusProcedure", "Insert");
                            cmd.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@RECID", "");
                            cmd.Parameters.AddWithValue("@SO_NO", PRESO_NO);
                            cmd.Parameters.AddWithValue("@PSR_CODE", strPsrName);
                            cmd.Parameters.AddWithValue("@DELIVERY_DATE", Convert.ToDateTime(txtDeliveryDate.Text).ToString("dd-MMM-yyyy"));
                            cmd.Parameters.AddWithValue("@SO_DATE", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                            cmd.Parameters.AddWithValue("@SO_VALUE", "5000");// have to check
                            cmd.Parameters.AddWithValue("@PSR_BEAT", strBeatName);
                            cmd.Parameters.AddWithValue("@CUST_REF_NO", "");
                            cmd.Parameters.AddWithValue("@REMARK", "");
                            cmd.Parameters.AddWithValue("@APP_SO_NO", "");
                            cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                            cmd.Parameters.AddWithValue("@APP_SO_DATE", "");
                            cmd.Parameters.AddWithValue("@SO_APPROVEDATE", "");
                            cmd.Parameters.AddWithValue("@STATUS", "0");
                            cmd.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                            cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);

                            /*   ---------- GST Implementation Start ----------- */
                            cmd.Parameters.AddWithValue("@DISTGSTINNO", Session["SITEGSTINNO"]);

                            if (Session["SITEGSTINREGDATE"] != null || Session["SITEGSTINREGDATE"].ToString().Trim().Length > 0)
                            {
                                cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", Convert.ToDateTime(Session["SITEGSTINREGDATE"]));
                            }

                            cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));

                            cmd.Parameters.AddWithValue("@CUSTGSTINNO", txtGSTtin.Text);

                            if (txtGSTtinRegistration.Text.Trim().Length > 0)
                            {
                                cmd.Parameters.AddWithValue("@CUSTGSTINREGDATE", Convert.ToDateTime(txtGSTtinRegistration.Text));
                            }

                            cmd.Parameters.AddWithValue("@CUSTCOMPOSITIONSCHEME", (chkCompositionScheme.Checked == true ? 1 : 0));

                            cmd.Parameters.AddWithValue("@BILLTOADDRESS", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@SHIPTOADDRESS", ddlShipToAddress.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@BILLTOSTATE", txtAddress.Text);
                            /*   ---------- GST Implementation End  ----------- */

                            cmd.ExecuteNonQuery();

                            //cmd1 = new SqlCommand("ACX_InsertSaleLinePre");
                            cmd1 = new SqlCommand("ACX_InsertSaleLinePreNew");
                            cmd1.Connection = conn;
                            cmd1.Transaction = transaction;
                            cmd1.CommandTimeout = 3600;
                            cmd1.CommandType = CommandType.StoredProcedure;
                        }
                        else
                        {
                            conn = obj.GetConnection();
                            transaction = conn.BeginTransaction();
                            //cmd1 = new SqlCommand("ACX_InsertSaleLinePre");
                            cmd1 = new SqlCommand("ACX_InsertSaleLinePreNew");
                            cmd1.Connection = conn;
                            cmd1.Transaction = transaction;
                            cmd1.CommandTimeout = 3600;
                            cmd1.CommandType = CommandType.StoredProcedure;
                            if (Session["SONO"] != null)
                            {
                                PRESO_NO = Session["SONO"].ToString();
                            }
                            else if (Session["PreSoNO"] != null)
                            {
                                PRESO_NO = Session["PreSoNO"].ToString();
                            }
                        }
                        cmd1.Parameters.AddWithValue("@statusProcedure", "Insert");
                        cmd1.Parameters.AddWithValue("@SO_NO", PRESO_NO);
                        cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                        cmd1.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                        cmd1.Parameters.AddWithValue("@PRODUCT_CODE", DDLMaterialCode.SelectedValue.ToString());
                        cmd1.Parameters.AddWithValue("@BOX", decimal.Parse(txtQtyBox.Text.Trim().ToString()));
                        cmd1.Parameters.AddWithValue("@BOXQty", decimal.Parse(txtViewTotalBox.Text.Trim().ToString()));
                        cmd1.Parameters.AddWithValue("@PcsQty", decimal.Parse(txtViewTotalPcs.Text.Trim().ToString()));
                        cmd1.Parameters.AddWithValue("@BOXPcs", decimal.Parse(txtBoxPcs.Text.Trim().ToString()));
                        cmd1.Parameters.AddWithValue("@CRATES", Convert.ToDecimal(txtQtyCrates.Text.Trim().ToString()));
                        cmd1.Parameters.AddWithValue("@LTR", Convert.ToDecimal(txtLtr.Text.Trim().ToString()));
                        cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(Convert.ToDecimal(dt.Rows[0]["VALUE"])));
                        cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(dt.Rows[0]["Rate"].ToString()));
                        cmd1.Parameters.AddWithValue("@BASEPRICE", Convert.ToDecimal(txtPrice.Text.Trim().ToString()));
                        //cmd1.Parameters.AddWithValue("@BASEPRICE", Price);

                        if (dt.Rows[0]["DISCTYPE"].ToString() == "Per")
                        {
                            cmd1.Parameters.AddWithValue("@DiscType", "0");
                        }

                        else if (dt.Rows[0]["DISCTYPE"].ToString() == "Val")
                        {
                            cmd1.Parameters.AddWithValue("@DiscType", "1");
                        }

                        else
                        {
                            cmd1.Parameters.AddWithValue("@DiscType", "2");
                        }

                        cmd1.Parameters.AddWithValue("@Disc", Convert.ToDecimal(dt.Rows[0]["DISC"]));
                        cmd1.Parameters.AddWithValue("@DiscVal", Convert.ToDecimal(dt.Rows[0]["DISCVAL"]));
                        //cmd1.Parameters.AddWithValue("@SECDISPER", Convert.ToDecimal(txtSDiscPer.Text.Trim().ToString()));
                        //cmd1.Parameters.AddWithValue("@SECDISVAL", Convert.ToDecimal(txtSDiscVal.Text.Trim().ToString()));
                        //cmd1.Parameters.AddWithValue("@SECDISVAL", SecDiscVal);
                        //cmd1.Parameters.AddWithValue("@TDVAL", Convert.ToDecimal(txtTDValue.Text.Trim().ToString()));
                        cmd1.Parameters.AddWithValue("@SECDISPER", Convert.ToDecimal(dt.Rows[0]["SECDISCPER"]));
                        cmd1.Parameters.AddWithValue("@SECDISVAL", Convert.ToDecimal(dt.Rows[0]["SECDISCAMT"]));
                        cmd1.Parameters.AddWithValue("@TDVAL", new decimal(0));
                        cmd1.Parameters.AddWithValue("@TD_PER", new decimal(0));
                        cmd1.Parameters.AddWithValue("@PE_VAL", new decimal(0));
                        cmd1.Parameters.AddWithValue("@PE_PER", new decimal(0));
                        //===========For Tax=========
                        cmd1.Parameters.AddWithValue("@Tax_Code", Convert.ToDecimal(dt.Rows[0]["TAX_PER"]));
                        cmd1.Parameters.AddWithValue("@Tax_Amount", Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]));
                        cmd1.Parameters.AddWithValue("@AddTax_Code", Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]));
                        cmd1.Parameters.AddWithValue("@AddTax_Amount", Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]));
                        //int intCalculation = 0;

                        if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "PRICE")
                        {
                            intCalculation = 0;
                        }
                        else if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "MRP")
                        {
                            intCalculation = 1;
                        }
                        else
                        {
                            intCalculation = 2;
                        }
                        cmd1.Parameters.AddWithValue("@CalculationBase", intCalculation);
                        cmd1.Parameters.AddWithValue("@TaxableAmount", Convert.ToDecimal(dt.Rows[0]["TaxableAmount"]));
                        //====================

                        cmd1.Parameters.AddWithValue("@RECID", "");
                        cmd1.Parameters.AddWithValue("@UOM", lblHidden.Text);
                        cmd1.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());

                        /*-------- GST IMPLEMENTATION START --------*/
                        cmd1.Parameters.AddWithValue("@HSNCODE", dt.Rows[0]["HSNCODE"]);
                        cmd1.Parameters.AddWithValue("@COMPOSITIONSCHEME", (chkCompositionScheme.Checked == true ? 1 : 0));
                        cmd1.Parameters.AddWithValue("@TAXCOMPONENT", dt.Rows[0]["TAX_CODE"]);
                        cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", dt.Rows[0]["ADDTAX_CODE"]);
                        /*-------- GST IMPLEMENTATION END ---------*/

                        cmd1.ExecuteNonQuery();

                        transaction.Commit();

                        //Update session table
                        Session["LineItem"] = dtLineItems;

                        //DDLProductGroup.SelectedIndex = 0;
                        //DDLMaterialCode.Items.Clear();

                        txtQtyCrates.Text = string.Empty;
                        txtQtyBox.Text = string.Empty;
                        txtLtr.Text = string.Empty;
                        txtPrice.Text = string.Empty;
                        lblHidden.Text = string.Empty;

                        //txtDiscount.Text = string.Empty;
                        txtValue.Text = string.Empty;

                        txtCrate.Text = string.Empty;
                        txtEnterQty.Text = string.Empty;
                        txtPcs.Text = string.Empty;
                        txtViewTotalPcs.Text = string.Empty;
                        txtViewTotalBox.Text = string.Empty;
                        txtBoxPcs.Text = "";
                        rdExempt.Enabled = false;
                        rdNonExempt.Enabled = false;

                    }// end of checkinh dublicate product
                    #endregion
                }
                #endregion
                return dtLineItems;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
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
            return dtLineItems;
        }

        public void UpdateSecDisc(decimal secdisper, decimal secdisval, decimal taxableamt, decimal tax1, decimal tax2, decimal total, int line)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                conn = obj.GetConnection();
                transaction = conn.BeginTransaction();
                //cmd1 = new SqlCommand("ACX_InsertSaleLinePre");
                cmd = new SqlCommand("ACX_InsertSaleLinePreUpdateSecDisc");
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@statusProcedure", "UPDATESECDIS");
                if (Session["SONO"] != null)
                {
                    cmd.Parameters.AddWithValue("@SO_NO", Session["SONO"]);
                }
                else if (Session["PreSoNO"] != null)
                {
                    cmd.Parameters.AddWithValue("@SO_NO", Session["PreSoNO"]);
                }
                cmd.Parameters.AddWithValue("@LINE_NO", line);
                cmd.Parameters.AddWithValue("@SECDISPER", secdisper);
                cmd.Parameters.AddWithValue("@SECDISVAL", secdisval);
                cmd.Parameters.AddWithValue("@TaxableAmount", taxableamt);
                cmd.Parameters.AddWithValue("@Tax_Amount", tax1);
                cmd.Parameters.AddWithValue("@AddTax_Amount", tax2);
                cmd.Parameters.AddWithValue("@AMOUNT", total);


                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
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

        public void UpdateTdVal(decimal TDValue, decimal TDPer, decimal PE, decimal PE_Per, decimal taxableamount, decimal tax1, decimal tax2, decimal total, decimal line)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                conn = obj.GetConnection();
                transaction = conn.BeginTransaction();
                //cmd1 = new SqlCommand("ACX_InsertSaleLinePre");
                cmd = new SqlCommand("ACX_InsertSaleLinePreUpdateTd");
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@statusProcedure", "UPDATESECDIS");
                if (Session["SONO"] != null)
                {
                    cmd.Parameters.AddWithValue("@SO_NO", Session["SONO"]);
                }
                else if (Session["PreSoNO"] != null)
                {
                    cmd.Parameters.AddWithValue("@SO_NO", Session["PreSoNO"]);
                }
                cmd.Parameters.AddWithValue("@LINE_NO", line);
                cmd.Parameters.AddWithValue("@TDVAL", TDValue);
                cmd.Parameters.AddWithValue("@TD_PER", TDPer);
                cmd.Parameters.AddWithValue("@PE", PE);
                cmd.Parameters.AddWithValue("@PE_PER", PE_Per);
                cmd.Parameters.AddWithValue("@TaxableAmount", taxableamount);
                cmd.Parameters.AddWithValue("@Tax_Amount", tax1);
                cmd.Parameters.AddWithValue("@AddTax_Amount", tax2);
                cmd.Parameters.AddWithValue("@AMOUNT", total);


                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
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

        private void AddColumnInDataTable()
        {
            //dtLineItems = new DataTable();
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            column.ColumnName = "SNO";
            //-----------------------------------------------------------//
            dtLineItems = new DataTable("LineItemTable");
            dtLineItems.Columns.Add(column);
            dtLineItems.Columns.Add("MaterialGroup", typeof(string));
            dtLineItems.Columns.Add("ProductCodeName", typeof(string));
            dtLineItems.Columns.Add("Product_Name", typeof(string));
            dtLineItems.Columns.Add("QtyCrates", typeof(decimal));
            dtLineItems.Columns.Add("QtyBox", typeof(decimal));
            dtLineItems.Columns.Add("BoxPcs", typeof(decimal));
            dtLineItems.Columns.Add("OnlyQtyBox", typeof(decimal));
            dtLineItems.Columns.Add("QtyPcs", typeof(decimal));
            dtLineItems.Columns.Add("QtyLtr", typeof(decimal));
            dtLineItems.Columns.Add("Price", typeof(decimal));
            dtLineItems.Columns.Add("BasePrice", typeof(decimal));
            dtLineItems.Columns.Add("Value", typeof(decimal));
            dtLineItems.Columns.Add("UOM", typeof(string));
            dtLineItems.Columns.Add("DiscType", typeof(string));
            dtLineItems.Columns.Add("Disc", typeof(decimal));
            dtLineItems.Columns.Add("DiscVal", typeof(decimal));
            dtLineItems.Columns.Add("SchemeDisc", typeof(decimal));
            dtLineItems.Columns.Add("SchemeDiscVal", typeof(decimal));
            dtLineItems.Columns.Add("Sec_Disc_Per", typeof(decimal));
            dtLineItems.Columns.Add("SEC_DISC_AMOUNT", typeof(decimal));
            dtLineItems.Columns.Add("TDValue", typeof(decimal));
            dtLineItems.Columns.Add("PE", typeof(decimal));
            //===========Tax==============================
            dtLineItems.Columns.Add("Tax_Code", typeof(decimal));
            dtLineItems.Columns.Add("Tax_Amount", typeof(decimal));
            dtLineItems.Columns.Add("AddTax_Code", typeof(decimal));
            dtLineItems.Columns.Add("AddTax_Amount", typeof(decimal));
            dtLineItems.Columns.Add("TD_Per", typeof(decimal));
            dtLineItems.Columns.Add("PE_Per", typeof(decimal));
            dtLineItems.Columns.Add("MRP", typeof(decimal));
            dtLineItems.Columns.Add("CalculationBase", typeof(string));
            dtLineItems.Columns.Add("Calculationon", typeof(string));   //mrp or price
            dtLineItems.Columns.Add("TaxableAmount", typeof(decimal));
            // New Fields for GST
            dtLineItems.Columns.Add("HSNCode", typeof(string));
            dtLineItems.Columns.Add("TaxComponent", typeof(string));
            dtLineItems.Columns.Add("AddTaxComponent", typeof(string));
            dtLineItems.Columns.Add("ADDSCHDISCPER", typeof(decimal));
            dtLineItems.Columns.Add("ADDSCHDISCVAL", typeof(decimal));
            dtLineItems.Columns.Add("ADDSCHDISCAMT", typeof(decimal));
        }

        private void AddColumnInSchemeDataTable()
        {
            dtSchemeLineItems = new DataTable("SchemeLineItemTable");
            dtSchemeLineItems.Columns.Add("Srno", typeof(int));
            dtSchemeLineItems.Columns.Add("Avail", typeof(bool));
            dtSchemeLineItems.Columns.Add("SchemeCode", typeof(string));
            dtSchemeLineItems.Columns.Add("SchemeName", typeof(string));
            dtSchemeLineItems.Columns.Add("ProductGroup", typeof(string));
            dtSchemeLineItems.Columns.Add("ProductCode", typeof(string));
            dtSchemeLineItems.Columns.Add("ProductName", typeof(string));
            dtSchemeLineItems.Columns.Add("Slab", typeof(int));
            dtSchemeLineItems.Columns.Add("Set", typeof(int));
            dtSchemeLineItems.Columns.Add("FreeBoxQty", typeof(int));
            dtSchemeLineItems.Columns.Add("AvailBoxQty", typeof(int));
            dtSchemeLineItems.Columns.Add("FreePCSQty", typeof(int));
            dtSchemeLineItems.Columns.Add("AvailPCSQty", typeof(int));
            dtSchemeLineItems.Columns.Add("PackSize", typeof(int));
            dtSchemeLineItems.Columns.Add("ConvQty", typeof(decimal));
            dtSchemeLineItems.Columns.Add("Rate", typeof(decimal));
            dtSchemeLineItems.Columns.Add("QtyLtr", typeof(decimal));
            dtSchemeLineItems.Columns.Add("Price", typeof(decimal));
            dtSchemeLineItems.Columns.Add("Value", typeof(decimal));
            dtSchemeLineItems.Columns.Add("UOM", typeof(string));
            //===========Discount=========================
            dtSchemeLineItems.Columns.Add("DiscType", typeof(string));
            dtSchemeLineItems.Columns.Add("Disc", typeof(decimal));
            dtSchemeLineItems.Columns.Add("DiscVal", typeof(decimal));
            dtSchemeLineItems.Columns.Add("SchemeDisc", typeof(decimal));
            dtSchemeLineItems.Columns.Add("SchemeDiscVal", typeof(decimal));
            //===========Tax==============================
            dtSchemeLineItems.Columns.Add("Tax_Code", typeof(decimal));
            dtSchemeLineItems.Columns.Add("Tax_Amount", typeof(decimal));
            dtSchemeLineItems.Columns.Add("AddTax_Code", typeof(decimal));
            dtSchemeLineItems.Columns.Add("AddTax_Amount", typeof(decimal));
            dtSchemeLineItems.Columns.Add("MRP", typeof(decimal));
            dtSchemeLineItems.Columns.Add("CalculationBase", typeof(string));
            dtSchemeLineItems.Columns.Add("Calculationon", typeof(string));   //mrp or price
            dtSchemeLineItems.Columns.Add("TaxableAmount", typeof(decimal));
            // New Fields for GST
            dtSchemeLineItems.Columns.Add("HSNCode", typeof(string));
            dtSchemeLineItems.Columns.Add("TaxComponent", typeof(string));
            dtSchemeLineItems.Columns.Add("AddTaxComponent", typeof(string));
            dtSchemeLineItems.Columns.Add("ADDSCHDISCPER", typeof(decimal));
            dtSchemeLineItems.Columns.Add("ADDSCHDISCVAL", typeof(decimal));
            dtSchemeLineItems.Columns.Add("ADDSCHDISCAMT", typeof(decimal));
        }

        private bool ValidateLineItemAdd()
        {
            bool b = true;

            if (txtBilltoState.Text.Trim().Length == 0 || txtAddress.Text.Trim().Length == 0)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);

                string message = "alert('Select Customers Bill To Address !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                ddlCustomer.Focus();
                b = false;
                return b;
            }

            if (DDLProductGroup.Text == "Select..." || DDLProductGroup.Text == "")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);

                string message = "alert('Select Material Group !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                DDLProductGroup.Focus();
                b = false;
                return b;
            }
            if (ddlPSRName.Visible == true)
            {
                if (ddlPSRName.SelectedItem.Value == "Select..." || ddlPSRName.Text == string.Empty)
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select PSR Name !');", true);
                    string message = "alert('Select PSR Name !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                    ddlPSRName.Focus();
                    b = false;
                    return b;
                }
                if (ddlBeatName.SelectedItem.Value == "Select..." || ddlBeatName.Text == string.Empty)
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Beat Name !');", true);
                    string message = "alert('Select Beat Name !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                    ddlBeatName.Focus();
                    b = false;
                    return b;
                }
            }
            if (DDLMaterialCode.Text == string.Empty || DDLMaterialCode.Text == "Select...")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Product First !');", true);

                string message = "alert('Select Product First !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                DDLMaterialCode.Focus();
                b = false;
                return b;
            }
            // || txtEnterQty.Text == "0"
            if (txtQtyBox.Text == string.Empty)
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Qty cannot be left blank !');", true);
                string message = "alert('Qty cannot be left blank !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                txtQtyBox.Focus();
                return b;
            }
            if (txtQtyBox.Text == string.Empty || txtQtyBox.Text == "0")
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Qty cannot be left blank !');", true);
                string message = "alert('Qty cannot be left blank !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                txtQtyBox.Focus();
                return b;
            }
            if (txtQtyCrates.Text == string.Empty)
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price cannot be left blank !');", true);
                string message = "alert('Price cannot be left blank !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                txtQtyCrates.Focus();
                return b;
            }
            // || txtLtr.Text == "0"
            if (txtLtr.Text == string.Empty)
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('ltr cannot be left blank !');", true);

                string message = "alert('ltr cannot be left blank !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                txtLtr.Focus();
                return b;
            }

            if (txtCrate.Text == "0" && txtEnterQty.Text == "0" && txtPcs.Text == "0")
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('ltr cannot be left blank !');", true);

                string message = "alert('Crate, Box and Pcs cannot be left blank ! Atleast One Quantity is required !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                txtCrate.Focus();
                return b;
            }

            if (txtPrice.Text == string.Empty || txtPrice.Text == "0")
            {
                decimal amount;
                if (!decimal.TryParse(txtPrice.Text, out amount))
                {
                    b = false;
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price is cannot be left blank !');", true);
                    string message = "alert('Price is cannot be left blank !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    txtPrice.Focus();
                    return b;
                }
                else
                {
                    b = true;
                }

            }
            if (rdExempt.Checked == true)
            {
                if (Session["Exempt_CurVal"] != null && Session["Exempt_CurVal"].ToString() != "1")
                {
                    b = false;
                    string message = "alert('Please add Exempted Products only');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    resetlinetextboxes();

                    return b;
                }
            }

            if (rdNonExempt.Checked == true)
            {
                if (Session["Exempt_CurVal"] != null && Session["Exempt_CurVal"].ToString() != "0")
                {
                    b = false;
                    string message = "alert('Please add NonExempted Products only');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    resetlinetextboxes();
                    return b;
                }
            }
            return b;

        }

        public void ReCalculate()
        {

            DataTable dt = new DataTable();
            // if (ViewState["checksecdis"] == null)
            //{
            if (Session["LineItem"] != null)
            {
                decimal SecDiscPer = 0, SecDiscValue = 0;
                if (string.IsNullOrEmpty(txtSDiscPer.Text))
                {
                    txtSDiscPer.Text = "0";
                }
                if (string.IsNullOrEmpty(txtSDiscVal.Text))
                {
                    txtSDiscVal.Text = "0";
                }
                //DataTable dttt = new DataTable();
                //dttt = (DataTable)Session["LineItem"];

                //Session["OriginalData"] = dt;

                //if (ViewState["OriginalData"] != null)
                //{
                //  DataTable dat = (DataTable)ViewState["OriginalData"];
                //}
                //if (ViewState["OriginalData"] == null)
                //{
                //  ViewState["OriginalData"] = dttt.Select("").CopyToDataTable();
                // DataTable dat = (DataTable)ViewState["OriginalData"];
                //}
                dt = (DataTable)Session["LineItem"];

                dt.Columns["Sec_Disc_Per"].ReadOnly = false;
                dt.Columns["SEC_DISC_AMOUNT"].ReadOnly = false;
                dt.Columns["Tax_Amount"].ReadOnly = false;
                dt.Columns["AddTax_Amount"].ReadOnly = false;
                dt.Columns["TaxableAmount"].ReadOnly = false;
                dt.Columns["Value"].ReadOnly = false;

                // dt.Columns["Amount"].ReadOnly = false;
                SecDiscPer = Convert.ToDecimal(txtSDiscPer.Text);
                SecDiscValue = Convert.ToDecimal(txtSDiscVal.Text);


                foreach (DataRow row in dt.Rows)
                {
                    decimal TD, TaxableAmount, Tax1, Tax2, DiscValue, AddSchDiscAmt, PE, SchemeDiscValue, Basic = 0;
                    int Line;

                    Basic = Convert.ToDecimal(row["BoxPcs"]) * Convert.ToDecimal(row["Price"]);
                    if (SecDiscPer != 0)
                    {
                        SecDiscValue = (SecDiscPer * Basic) / 100;
                    }
                    TD = Convert.ToDecimal(row["TDValue"]);
                    AddSchDiscAmt = Convert.ToDecimal(row["ADDSCHDISCAMT"]);
                    PE = Convert.ToDecimal(row["PE"]);
                    Line = Convert.ToInt32(row["SNO"]);
                    DiscValue = Convert.ToDecimal(row["DiscVal"]);
                    SchemeDiscValue = Convert.ToDecimal(row["SchemeDiscVal"]);
                    TaxableAmount = Basic - DiscValue - SecDiscValue - SchemeDiscValue - TD + PE - AddSchDiscAmt;
                    Tax1 = TaxableAmount * Convert.ToDecimal(row["Tax_Code"]) / 100;
                    Tax2 = TaxableAmount * Convert.ToDecimal(row["AddTax_Code"]) / 100;
                    decimal Total = TaxableAmount + Tax1 + Tax2;
                    row["Sec_Disc_Per"] = SecDiscPer;
                    row["SEC_DISC_AMOUNT"] = SecDiscValue;
                    row["TaxableAmount"] = TaxableAmount;
                    row["Tax_Amount"] = Tax1;
                    row["AddTax_Amount"] = Tax2;
                    row["Value"] = Total;

                    //row["Amount"] = TaxableAmount + Tax1 + Tax2;
                    if (TaxableAmount + Tax1 + Tax2 < 0)
                    {
                        if (Basic > 0)
                        {
                            txtSDiscVal.Text = "0.00";
                            txtSDiscPer.Text = "0.00";
                            string message = "alert('Invoice Line value should not be less than zero!! At Line no " + row["SNO"].ToString() + "');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        }

                    }

                    //Update Secondary Discount in SalesLinePre Table
                    UpdateSecDisc(SecDiscPer, SecDiscValue, TaxableAmount, Tax1, Tax2, Total, Line);
                    dt.AcceptChanges();
                }
                Session["LineItem"] = dt;
                BindingGird();
                //gvDetails.DataSource = dt;
                //gvDetails.DataBind();
                //GridViewFooterCalculate(dt);
                DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
                if (dtApplicable.Rows.Count > 0)
                {
                    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                }
                if (intApplicable == 1 || intApplicable == 3)
                {
                    this.SchemeDiscCalculation();
                    BindSchemeGrid();
                }
                this.SchemeDiscCalculation();
            }
            ViewState["checksecdis"] = 1;

            //return dt;
            //}

        }


        public void ReCalculate1()
        {
            DataTable dt = new DataTable();
            if (ViewState["checksecdis"] != null)
            {
                if (Session["LineItem"] != null)
                {
                    //decimal Taxable, Tax1, Tax2, Total, SecDiscPer, SecDiscVal;

                    //int Line;
                    dt = (DataTable)Session["LineItem"];
                    //DataTable dat = (DataTable)ViewState["OriginalData"];
                    // DataRow[] drSec = dt.Select("SEC_DISC_AMOUNT>0");
                    //if (drSec.Length > 0)
                    dt.Columns["Sec_Disc_Per"].ReadOnly = false;
                    dt.Columns["SEC_DISC_AMOUNT"].ReadOnly = false;
                    dt.Columns["Tax_Amount"].ReadOnly = false;
                    dt.Columns["AddTax_Amount"].ReadOnly = false;
                    dt.Columns["TaxableAmount"].ReadOnly = false;
                    dt.Columns["Value"].ReadOnly = false;

                    //foreach (DataRow drs in dt.Rows)
                    //{
                    //    decimal TD, TaxableAmount, Tax1, Tax2, DiscValue, SchemeDiscValue, Basic = 0, SecDiscPer, SecDiscVal;
                    //    int Line;
                    //    Basic = Convert.ToDecimal(drs["BoxPcs"]) * Convert.ToDecimal(drs["Price"]);
                    //    TD = Convert.ToDecimal(drs["TDValue"]);
                    //    Line = Convert.ToInt32(drs["SNO"]);
                    //    DiscValue = Convert.ToDecimal(drs["DiscVal"]);
                    //    SchemeDiscValue = Convert.ToDecimal(drs["SchemeDiscVal"]);
                    //    TaxableAmount = Basic - DiscValue - SchemeDiscValue - TD;
                    //    Tax1 = TaxableAmount * Convert.ToDecimal(drs["Tax_Code"]) / 100;
                    //    Tax2 = TaxableAmount * Convert.ToDecimal(drs["AddTax_Code"]) / 100;
                    //    decimal Total = TaxableAmount + Tax1 + Tax2;
                    //    SecDiscPer = 0;
                    //    SecDiscVal = 0;
                    //    drs["Sec_Disc_Per"] = SecDiscPer;
                    //    drs["SEC_DISC_AMOUNT"] = SecDiscVal;
                    //    drs["TaxableAmount"] = TaxableAmount;
                    //    drs["Tax_Amount"] = Tax1;
                    //    drs["AddTax_Amount"] = Tax2;
                    //    drs["Value"] = Total;
                    //    UpdateSecDisc(SecDiscPer, SecDiscVal, TaxableAmount, Tax1, Tax2, Total, Line);
                    //    dt.AcceptChanges();


                    //}
                    txtSDiscPer.Text = "0";
                    txtSDiscVal.Text = "0";
                    Session["LineItem"] = dt;
                    DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
                    if (dtApplicable.Rows.Count > 0)
                    {
                        intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                    }
                    if (intApplicable == 1 || intApplicable == 3)
                    {
                        this.SchemeDiscCalculation();
                        BindSchemeGrid();
                    }
                    this.SchemeDiscCalculation();

                }
                //return dt;
                ViewState["checksecdis"] = null;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Allowed Secondary Discount/Trade Discount value will be reset due to change in quantity or new product addition')", true);
            }


        }

        protected void lnkbtnDel_Click(object sender, EventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            HiddenField hiddenField = (HiddenField)gvrow.FindControl("HiddenField2");
            LinkButton lnk = sender as LinkButton;
            DataTable dt1 = new DataTable();
            //=================Delete from Pre table=================
            if (Session["SONO"] != null)   // for 1st time when we save the record.
            {
                if (gvDetails.Rows.Count > 0)
                {
                    string query = "Delete From [ax].[ACXSALESLINEPRE] where [SO_NO]='" + Session["SONO"] + "' and [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and [SITEID]='" + Session["SiteCode"].ToString() + "' and  [LINE_NO]=" + hiddenField.Value + "";
                    baseObj.ExecuteCommand(query);
                    //=================Fill Datatable After delete from Pretable========
                    //AddColumnInDataTable();
                    Session["LineItem"] = null;
                    strQuery = "Select A.[LINE_NO] AS SNO,B.[PRODUCT_GROUP] as MaterialGroup,A.[PRODUCT_CODE] as ProductCodeName,A.[PRODUCT_CODE] +'-'+B.Product_Name as Product_Name,A.[CRATES] as QtyCrates,CAST(A.[BOX] AS decimal(9,2)) as QtyBox,FLOOR(A.[BOX]) as OnlyQtyBox,CAST((CAST(A.[BOX] AS decimal(9,2))-CAST(FLOOR(A.[BOX]) AS DECIMAL(9,0)))*B.Product_PackSize AS decimal(9,0)) as QtyPcs,A.[LTR] as QtyLtr,A.[RATE] as Price,A.[AMOUNT] as Value,A.[UOM],case when A.[DiscType]='0' then 'Per' when A.[DiscType]='1' then 'Val' else '2' end as DiscType,A.[Disc]" +
                        ",A.[DiscVal],A.[TAX_CODE],A.[TAX_AMOUNT],A.[ADDTAX_CODE],A.[ADDTAX_AMOUNT],B.Product_MRP as MRP,case A.[DiscCalculationBase] when 0 then 'Price' when 1 then 'MRP' else '2' end  as CalculationBase  ,ISNULL(A.BOXPCS,0.00) AS BOXPCS ,case when A.[DiscType]='0' and A.[DiscCalculationBase]=1 then 'MRP' else 'Price' end as CalculationOn ,Isnull(A.BasePrice,0) AS  BasePrice,isnull(A.TaxableAmount,0) AS TaxableAmount,A.HSNCODE,A.TAXCOMPONENT,A.ADDTAXCOMPONENT,A.Sec_Disc_Per,A.SEC_DISC_AMOUNT,A.TDValue,A.PEValue as PE,A.TD_Per,A.PE_Per,A.SchemeDiscPer as SchemeDisc,A.SchemeDiscValue as SchemeDiscVal,ISNULL(ADDSCHDISCPER,0) ADDSCHDISCPER,ISNULL(ADDSCHDISCVAL,0) ADDSCHDISCVAL,ISNULL(ADDSCHDISCAMT,0) ADDSCHDISCAMT  " +
                        " FROM [ax].[ACXSALESLINEPRE] A " +
                        " INNER JOIN ax.InventTable B on A.[PRODUCT_CODE]=B.ITEMID" +
                        " WHERE A.SO_NO='" + Session["SONO"] + "' and A.SiteId='" + Session["SiteCode"].ToString() + "' and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'" +
                        " ORDER BY A.[LINE_NO] ";
                    Session["LineItem"] = baseObj.GetData(strQuery);
                }
                else
                {
                    try//If no line item
                    {
                        //===============Delete From PreTable===============
                        conn = baseObj.GetConnection();
                        cmd2 = new SqlCommand("ACX_USP_DeleteFromSalesOrderPre");
                        cmd2.Connection = conn;
                        cmd2.Connection = conn;
                        cmd2.Transaction = transaction;
                        cmd2.CommandTimeout = 3600;
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.Clear();
                        cmd2.Parameters.AddWithValue("@Site_Code", Session["SITECODE"].ToString());
                        cmd2.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd2.Parameters.AddWithValue("@SO_NO", Session["SONO"].ToString());
                        cmd2.ExecuteNonQuery();
                        //===================================================

                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else if (Session["PreSoNO"] != null) // when we came from pending SO list.
            {
                if (gvDetails.Rows.Count > 0)
                {
                    string query = "Delete from [ax].[ACXSALESLINEPRE] where [SO_NO]='" + Session["PreSoNO"] + "' and [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and [SITEID]='" + Session["SiteCode"].ToString() + "' and  [LINE_NO]=" + hiddenField.Value + "";
                    baseObj.ExecuteCommand(query);
                    //=================Fill Datatable After delete from Pretable========
                    //AddColumnInDataTable();
                    Session["LineItem"] = null;
                    strQuery = "SELECT A.[LINE_NO] as SNO,B.[PRODUCT_GROUP] as MaterialGroup,A.[PRODUCT_CODE] as ProductCodeName,A.[PRODUCT_CODE] +'-'+B.Product_Name as Product_Name,A.[CRATES] as QtyCrates,CAST(A.[BOX] AS decimal(9,2)) as QtyBox ,FLOOR(A.[BOX]) as OnlyQtyBox,CAST((CAST(A.[BOX] AS decimal(9,2))-CAST(FLOOR(A.[BOX]) AS DECIMAL(9,0)))*B.Product_PackSize AS decimal(9,0)) as QtyPcs,A.[LTR] as QtyLtr,A.[RATE] as Price,A.[AMOUNT] as Value,A.[UOM],case when A.[DiscType]='0' then 'Per' when A.[DiscType]='1' then 'Val' else '2' end as DiscType,A.[Disc]" +
                               ",A.[DiscVal],A.[TAX_CODE],A.[TAX_AMOUNT],A.[ADDTAX_CODE],A.[ADDTAX_AMOUNT],B.Product_MRP AS MRP,case A.[DiscCalculationBase] when 0 then 'Price' when 1 then 'MRP' else '2' end  as CalculationBase ,ISNULL(A.BOXPCS,0.00) AS BOXPCS,case when A.[DiscType]='0' and A.[DiscCalculationBase]=1 then 'MRP' else 'Price' end as CalculationOn ,Isnull(A.BasePrice,0) as  BasePrice,isnull(A.TaxableAmount,0) as TaxableAmount,A.HSNCODE,A.TAXCOMPONENT,A.ADDTAXCOMPONENT,A.Sec_Disc_Per,A.SEC_DISC_AMOUNT,A.TDValue,A.PEValue as PE,A.TD_Per,A.PE_Per,A.SchemeDiscPer as SchemeDisc,A.SchemeDiscValue as SchemeDiscVal,ISNULL(ADDSCHDISCPER,0) ADDSCHDISCPER,ISNULL(ADDSCHDISCVAL,0) ADDSCHDISCVAL,ISNULL(ADDSCHDISCAMT,0) ADDSCHDISCAMT  " +
                               " FROM [ax].[ACXSALESLINEPRE] A " +
                               " INNER JOIN ax.InventTable B on A.[PRODUCT_CODE]=B.ITEMID" +
                               " WHERE A.SO_NO='" + Session["PreSoNO"] + "' and A.SiteId='" + Session["SiteCode"].ToString() + "' and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'" +
                               " ORDER BY A.[LINE_NO] ";
                    Session["LineItem"] = baseObj.GetData(strQuery);
                }
                else  //If no line item
                {
                    try
                    {
                        //===============Delete From PreTable===============
                        conn = baseObj.GetConnection();
                        cmd2 = new SqlCommand("ACX_USP_DeleteFromSalesOrderPre");
                        cmd2.Connection = conn;

                        cmd2.Transaction = transaction;
                        cmd2.CommandTimeout = 3600;
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.Clear();
                        cmd2.Parameters.AddWithValue("@Site_Code", Session["SITECODE"].ToString());
                        cmd2.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd2.Parameters.AddWithValue("@SO_NO", Session["PreSoNO"].ToString());
                        cmd2.ExecuteNonQuery();
                        //===================================================

                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            //=====================
            if (Session["LineItem"] != null)
            {
                DataTable dt = Session["LineItem"] as DataTable;
                dt.AsEnumerable().Where(r => r.Field<int>("SNO") == Convert.ToInt32(hiddenField.Value)).ToList().ForEach(row => row.Delete());
                //gvDetails.DataSource = dt;
                //gvDetails.DataBind();
                //GridViewFooterCalculate(dt);
                Session["LineItem"] = dt;
                BindingGird();
                //TDCalculation();
                if (dt.Rows.Count == 0)
                {
                    ViewState["checksecdis"] = null;
                }
                //BindSchemeGrid();
                //====================new==============
                DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
                if (dtApplicable.Rows.Count > 0)
                {
                    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                }
                if (intApplicable == 1 || intApplicable == 3)
                {
                    BindSchemeGrid();
                    this.SchemeDiscCalculation();
                }

            }
            DataTable dt11 = new DataTable();
            dt11 = (DataTable)Session["LineItem"];

            if (dt11.Rows.Count == 0)
            {
                rdExempt.Enabled = true;
                rdNonExempt.Enabled = true;
                Session["Exempt_CurVal"] = null;
                txtHdnTDValue.Text = "0";
                txtTDValue.Text = "0";
                //Session["Comm_Exem"] = null;
            }
            else if (dt11.Rows.Count > 0)
            {
                btnApply_Click(null, null);
            }
        }

        protected void resetlinetextboxes()
        {
            txtBoxPcs.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtPcs.Text = "";
            txtCrate.Text = "";
            txtEnterQty.Text = "";
            txtQtyBox.Text = "";
            txtQtyCrates.Text = "";
            txtLtr.Text = "";
            txtPrice.Text = "";
            txtValue.Text = "";
        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            DataTable dt1 = new DataTable();
            DataTable dttt = (DataTable)Session["LineItem"];
            Boolean ResetMsg;
            ResetMsg = false;
            ReCalculate1();

            if (txtDeliveryDate.Text.Trim().Length > 0)
            {
                if (!baseObj.IsDate(txtDeliveryDate.Text))
                {
                    string message = "alert('Invalid Delivery Date !!!');";
                    ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                    txtDeliveryDate.Focus();
                    return;
                }
            }

            if (string.IsNullOrEmpty(txtQtyBox.Text.Trim()))
            {
                string message = "alert('Please enter Qty!!');";
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                txtEnterQty.Focus();
                return;
            }
            if (Convert.ToDecimal(txtQtyBox.Text.Trim()) == 0)
            {
                string message = "alert('Please enter Qty!!');";
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                txtEnterQty.Focus();
                return;
            }
            DataTable dtDublicate = Session["LineItem"] as DataTable;
            if (dtDublicate != null)
            {
                if (dtDublicate.Rows.Count > 0)
                {
                    if (dtDublicate.Select("ProductCodeName='" + DDLMaterialCode.SelectedValue.ToString() + "'").Length > 0)
                    {
                        string message = "alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                        txtBoxPcs.Text = "";
                        txtViewTotalBox.Text = "";
                        txtViewTotalPcs.Text = "";
                        txtPcs.Text = "";
                        txtCrate.Text = "";
                        txtEnterQty.Text = "";
                        txtQtyBox.Text = "";
                        txtQtyCrates.Text = "";
                        txtLtr.Text = "";
                        txtPrice.Text = "";
                        txtValue.Text = "";
                        ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                        DDLMaterialCode.Focus();
                        return;
                    }
                }
            }

            
            if (ddlPSRName.Visible == true)
            {
                string stringquery = "select ORDERALLOWED from ax.ACXPSRSITELinkingMaster where Site_code ='" + Session["SiteCode"].ToString() + "' and psrcode = '" + ddlPSRName.SelectedItem.Value + "'";
                string orderallowed = baseObj.GetScalarValue(stringquery);
                if (orderallowed == "1" || orderallowed == "2")
                {
                }
                else
                {
                    resetlinetextboxes();
                    string message = "alert('Order Not Allowed From Portal');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
            }

            //============================
            bool valid = true;
            valid = ValidatePurchaseReturnHeaderData();
            if (valid == true)
            {
                valid = ValidateLineItemAdd();
            }
            if (valid == true)
            {
                DataTable dt = new DataTable();
                DataTable dat1 = new DataTable();
                dt = Session["ItemTable"] as DataTable;
                DataTable dbt = (DataTable)Session["LineItem"];
                dt = AddLineItems();
                dat1 = dt.Copy();

                Session["LineItem"] = dt;
                BindingGird();
                
                if (ResetMsg)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Allowed Secondary Discount value will be reset due to change in quantity or new product addition')", true);
                }
            }
            txtEnterQty.Text = string.Empty;
            btnApply_Click(sender, e);
            DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
            if (dtApplicable.Rows.Count > 0)
            {
                intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
            }
            if (intApplicable == 1 || intApplicable == 3)
            {
                this.SchemeDiscCalculation();
                BindSchemeGrid();
            }
            DDLMaterialCode.Focus();
            this.SchemeDiscCalculation();
            DDLMaterialCode.SelectedIndex = 0;
            //DDLProductGroup.Items.Clear();
            //DDLProductSubCategory.Items.Clear();
            //ProductGroup();
            //FillProductCode();

        }

        private void GridViewFooterCalculate(DataTable dt)
        {
            decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Value"));          //For Total[Sum] Value Show in Footer--//
            decimal tBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyBox"));
            decimal tOnlyBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("OnlyQtyBox"));
            decimal tPcs = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyPcs"));
            //decimal tBoxPcs = dt.AsEnumerable().Sum(row => row.Field<decimal>("BoxPcs"));
            decimal tCrate = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyCrates"));
            decimal tLtr = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyLtr"));
            decimal tTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("Tax_Amount"));

            //===============11-5-2016=====
            decimal tPrice = dt.AsEnumerable().Sum(row => row.Field<decimal>("Price"));
            decimal tAddTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("AddTax_Amount"));
            decimal tDiscValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("DiscVal"));
            decimal tSchemeDiscVal = dt.AsEnumerable().Sum(row => row.Field<decimal>("SchemeDiscVal"));
            decimal tAddSchemeDiscVal = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADDSCHDISCAMT"));
            decimal tmrp = dt.AsEnumerable().Sum(row => row.Field<decimal>("MRP") * row.Field<decimal>("QtyBox"));
            //==============================
            //=============Sec Discount & TD Value====
            decimal tSecDisVal = dt.AsEnumerable().Sum(row => row.Field<decimal>("SEC_DISC_AMOUNT"));
            decimal tTDValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TDValue"));
            ViewState["tdvalue"] = tTDValue;
            //========================================
            if (gvDetails.Rows.Count > 0)

            {
                gvDetails.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[3].Text = "TOTAL";
                gvDetails.FooterRow.Cells[3].Font.Bold = true;

                gvDetails.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[5].Text = tBox.ToString("N2");
                gvDetails.FooterRow.Cells[5].Font.Bold = true;
                TotQty.Text = tBox.ToString("N2");

                //gvDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                //gvDetails.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
                //gvDetails.FooterRow.Cells[6].Text = tBoxPcs.ToString("N2");
                //gvDetails.FooterRow.Cells[6].Font.Bold = true;

                gvDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[7].Text = tOnlyBox.ToString("N2");
                gvDetails.FooterRow.Cells[7].Font.Bold = true;

                txttotltr.Text = tLtr.ToString("N2");
                gvDetails.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[8].Text = tPcs.ToString("N2");
                gvDetails.FooterRow.Cells[8].Font.Bold = true;

                gvDetails.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[10].Text = tCrate.ToString("N2");
                gvDetails.FooterRow.Cells[10].Font.Bold = true;

                gvDetails.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[11].Text = tLtr.ToString("N2");
                gvDetails.FooterRow.Cells[11].Font.Bold = true;


                gvDetails.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[13].Text = tmrp.ToString("N2");
                gvDetails.FooterRow.Cells[13].Font.Bold = true;

                gvDetails.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[18].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[18].Text = tTaxAmount.ToString("N2");
                gvDetails.FooterRow.Cells[18].Font.Bold = true;

                //===============11-5-16=============
                gvDetails.FooterRow.Cells[20].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[20].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[20].Text = tAddTaxAmount.ToString("N2");
                gvDetails.FooterRow.Cells[20].Font.Bold = true;

                gvDetails.FooterRow.Cells[21].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[21].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[21].Text = total.ToString("N2");
                gvDetails.FooterRow.Cells[21].Font.Bold = true;
                InvTot.Text = total.ToString("N2");

                gvDetails.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[16].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[16].Text = tDiscValue.ToString("N2");
                gvDetails.FooterRow.Cells[16].Font.Bold = true;

                gvDetails.FooterRow.Cells[27].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[27].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[27].Text = tSecDisVal.ToString("N2");
                gvDetails.FooterRow.Cells[27].Font.Bold = true;

                gvDetails.FooterRow.Cells[28].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[28].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[28].Text = tTDValue.ToString("N2");
                gvDetails.FooterRow.Cells[28].Font.Bold = true;



                gvDetails.FooterRow.Cells[31].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[31].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[31].Text = tSchemeDiscVal.ToString("N2");
                gvDetails.FooterRow.Cells[31].Font.Bold = true;

                gvDetails.FooterRow.Cells[32].HorizontalAlign = HorizontalAlign.Left;
                gvDetails.FooterRow.Cells[32].ForeColor = System.Drawing.Color.MidnightBlue;
                gvDetails.FooterRow.Cells[32].Text = tAddSchemeDiscVal.ToString("N2");
                gvDetails.FooterRow.Cells[32].Font.Bold = true;
            }
            if (gvDetails.Rows.Count == 0)
            {
                TotQty.Text = "";
                InvTot.Text = "";
            }
        }

        protected void txtQtyBox_TextChanged(object sender, EventArgs e)
        {
            QtyCalcualtion();
            Session["focus"] = 2;
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        private bool Validation()
        {
            bool returnvalue = true;
            decimal totalInvoiceValue = 0;
            string lblTotal;

            // Check For Valid Delivery Date
            if (txtAddress.Text.Trim().Length == 0)
            {

                string message = "alert('Customers Bill To Address Required !!!');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", message, true);
                returnvalue = false;
                ddlCustomer.Focus();
                return returnvalue;
            }

            if (txtBilltoState.Text.Trim().Length == 0)
            {
                string message = "alert('Customers Bill To State Required !!!');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                returnvalue = false;
                ddlCustomer.Focus();
                return returnvalue;
            }

            if (ddlShipToAddress.SelectedValue == null || ddlShipToAddress.SelectedValue.ToString().Trim().Length == 0)
            {
                string message = "alert('Customers Ship To Address Required !!!');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                returnvalue = false;
                ddlCustomer.Focus();
                return returnvalue;
            }

            if (txtDeliveryDate.Text.Trim().Length > 0)
            {
                if (!baseObj.IsDate(txtDeliveryDate.Text))
                {
                    string message = "alert('Invalid Delivery Date !!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                    txtDeliveryDate.Focus();
                    return returnvalue;
                }
            }

            try
            {
                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    lblTotal = gvDetails.Rows[i].Cells[21].Text.ToString();
                    if (lblTotal.Trim().Length == 0) { lblTotal = "0"; }
                    if (Convert.ToDecimal(lblTotal) <= 0)
                    {
                        returnvalue = false;
                        break;
                    }
                    totalInvoiceValue += Convert.ToDecimal(lblTotal);
                }
                if (totalInvoiceValue <= 0 || returnvalue == false)
                {
                    string message = "alert('Sales Order value should not be less than zero!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                    return returnvalue;
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                returnvalue = false;
                return returnvalue;
            }

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            //===============Validation for gridview==============
            bool b = Validation();
            if (!b) { return; }

            int gvRow = gvDetails.Rows.Count;

            if (gvRow > 0)
            {
                //SavePurchaseReturn();
                SaveSaleOrder();
            }
            else
            {
                //=================If no Line Item then delete from [ax].[ACXSALESHEADERPRE] table===========
                try
                {
                    conn = baseObj.GetConnection();
                    cmd2 = new SqlCommand("ACX_USP_DeleteFromSalesOrderPre");
                    transaction = conn.BeginTransaction();
                    cmd2.Connection = conn;
                    cmd2.Transaction = transaction;
                    cmd2.CommandTimeout = 3600;
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.Clear();
                    cmd2.Parameters.AddWithValue("@Site_Code", Session["SITECODE"].ToString());
                    cmd2.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                    cmd2.Parameters.AddWithValue("@SO_NO", Session["PreSoNO"].ToString());
                    cmd2.ExecuteNonQuery();
                    transaction.Commit();

                    string message = " alert('Please Enter a line');";
                    ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);

                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a line');", true);

                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            ddlCustomerGroup.Focus();
        }

        public bool ValidateSchemeQty1(string SelectedShemeCode)
        {
            bool returnType = true;
            if (gvScheme.Rows.Count > 0)
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("SchemeCode", typeof(string));
                dt.Columns.Add("Slab", typeof(int));
                dt.Columns.Add("FreeQty", typeof(int));
                dt.Columns.Add("SetNO", typeof(int));
                dt.Columns.Add("FreeQtyPcs", typeof(int));


                DataRow dr = null;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    dr = dt.NewRow();
                    dr["SchemeCode"] = rw.Cells[1].Text;
                    dr["Slab"] = Convert.ToInt16(rw.Cells[6].Text);
                    dr["SetNo"] = Convert.ToInt16(rw.Cells[7].Text);
                    if (rw.Cells[8].Text == "&nbsp;")
                    { rw.Cells[8].Text = ""; }
                    if (rw.Cells[11].Text == "&nbsp;")
                    { rw.Cells[11].Text = ""; }
                    if (rw.Cells[8].Text != "")
                    {
                        dr["FreeQty"] = Convert.ToInt16(Convert.ToInt16(rw.Cells[8].Text));
                        dr["FreeQtyPcs"] = 0;
                    }
                    if (Convert.ToString(rw.Cells[11].Text) != "")
                    {
                        dr["FreeQtyPcs"] = Convert.ToInt16(Convert.ToInt16(rw.Cells[11].Text));
                        dr["FreeQty"] = 0;
                    }

                    dt.Rows.Add(dr);

                }
                DataView dv = new DataView(dt);
                DataTable distinctTableValue = (DataTable)dv.ToTable(true, "SchemeCode", "Slab", "FreeQty", "FreeQtyPcs", "SetNo");

                DataView dv1 = new DataView(distinctTableValue);
                dv1.RowFilter = "[SchemeCode]='" + SelectedShemeCode + "' and SetNo=0";
                if (dv1.Count > 0)
                {

                    decimal TotalQty = 0, TotalQtyPcs = 0;
                    decimal Slab = 0;
                    decimal FreeQty = 0;
                    decimal FreeQtyPcs = 0;
                    decimal SetNo = 0;
                    string SchemeCode = string.Empty;
                    foreach (DataRowView drow in dv1)
                    {

                        TotalQty = 0;
                        TotalQtyPcs = 0;
                        SchemeCode = drow["SchemeCode"].ToString();
                        Slab = Convert.ToInt16(drow["Slab"]);
                        FreeQty = Convert.ToInt16(drow["FreeQty"]);
                        FreeQtyPcs = Convert.ToInt16(drow["FreeQtyPcs"]);

                        foreach (GridViewRow rw in gvScheme.Rows)
                        {
                            //HiddenField hdnCalculationOn = (HiddenField)rw.FindControl("hdnCalculationOn");                                
                            CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                            if (chkBx.Checked == true)
                            {
                                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                                TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                                txtQty.Text = (txtQty.Text.Trim().Length == 0 ? "0" : txtQty.Text);
                                txtQtyPcs.Text = (txtQtyPcs.Text.Trim().Length == 0 ? "0" : txtQtyPcs.Text);

                                if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab && Convert.ToInt16(rw.Cells[7].Text) == SetNo)
                                {
                                    TotalQty += Convert.ToInt16(txtQty.Text);
                                }
                                if (!string.IsNullOrEmpty(txtQtyPcs.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab && Convert.ToInt16(rw.Cells[7].Text) == SetNo)
                                {
                                    TotalQtyPcs += Convert.ToInt16(txtQtyPcs.Text);
                                }
                            }
                        }

                        if (TotalQty != FreeQty && Slab == Convert.ToInt16(drow["Slab"]) && SetNo == Convert.ToInt16(drow["SetNo"]))
                        {
                            returnType = false;
                        }
                        if (Slab == Convert.ToInt16(drow["Slab"]) && SetNo == Convert.ToInt16(drow["SetNo"]) && TotalQtyPcs != FreeQtyPcs)
                        {
                            returnType = false;
                        }

                    }
                }
                else
                {
                    returnType = false;
                }
            }
            return returnType;
        }

        private void SaveSaleOrder()
        {
            try
            {
                bool b = ValidatePurchaseReturnHeaderData();
                string SelectedSchemeCode = string.Empty;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chk = (CheckBox)rw.FindControl("ChkSelect");
                    if (chk.Checked)
                    {
                        SelectedSchemeCode = rw.Cells[1].Text;
                        break;
                    }
                }
                bool IsSchemeValidate = false;
                intApplicable = 0;
                DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
                if (dtApplicable.Rows.Count > 0)
                {
                    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                }
                if (intApplicable == 1 || intApplicable == 3)
                {
                    IsSchemeValidate = baseObj.ValidateSchemeQty(SelectedSchemeCode, ref gvScheme);
                }
                else
                {
                    IsSchemeValidate = true;
                }
                // bool IsSchemeValidate = ValidateSchemeQty(SelectedSchemeCode);
                if (IsSchemeValidate == true)
                {
                    decimal Total = 0;
                    if (b == true)
                    {
                        if (Session["LineItem"] != null)
                        {
                            string strPsrName = string.Empty;
                            string strBeatName = string.Empty;
                            if (ddlPSRName.Visible == true)
                            {
                                strPsrName = ddlPSRName.SelectedItem.Value;
                                strBeatName = ddlBeatName.SelectedItem.Value;
                            }
                            string RefSchemeCode, RefSchemeLineNo;
                            int RefSchemeType;
                            RefSchemeCode = "";
                            RefSchemeLineNo = "0";
                            RefSchemeType = -1;
                            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                            conn = obj.GetConnection();
                            cmd = new SqlCommand("InsertSaleHeaderNEW");
                            transaction = conn.BeginTransaction();
                            cmd.Connection = conn;
                            cmd.Transaction = transaction;
                            cmd.CommandTimeout = 3600;
                            cmd.CommandType = CommandType.StoredProcedure;
                            //string SO_NO = ddlCustomer.SelectedItem.Value + System.DateTime.Now.ToString("yymmddhhmmss");
                            //--code logic by Amrita--//
                            string st = Session["SiteCode"].ToString();
                            if (st.Length < 6)
                            {
                                int len = st.Length;
                                int plen = 6 - len;
                                for (int j = 0; j < plen; j++)
                                {
                                    st = "0" + st;
                                }
                            }
                            DataTable dt = obj.GetNumSequenceNew(4, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                            string SO_NO = string.Empty;
                            string NUMSEQ = string.Empty;
                            if (dt != null)
                            {
                                SO_NO = dt.Rows[0][0].ToString();
                                NUMSEQ = dt.Rows[0][1].ToString();
                            }
                            else
                            {
                                return;
                            }

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@statusProcedure", "Insert");
                            cmd.Parameters.AddWithValue("@SO_NO", SO_NO);
                            cmd.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@RECID", "");
                            cmd.Parameters.AddWithValue("@TDValue", ViewState["tdvalue"].ToString());
                            cmd.Parameters.AddWithValue("@PSR_CODE", strPsrName);
                            cmd.Parameters.AddWithValue("@DELIVERY_DATE", Convert.ToDateTime(txtDeliveryDate.Text).ToString("dd-MMM-yyyy"));
                            cmd.Parameters.AddWithValue("@SO_DATE", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                            //cmd.Parameters.AddWithValue("@SO_VALUE", "5000");
                            cmd.Parameters.AddWithValue("@PSR_BEAT", strBeatName);
                            if (txtIndentNo.Text == "")
                            {
                                cmd.Parameters.AddWithValue("@CUST_REF_NO", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@CUST_REF_NO", txtIndentNo.Text);
                            }
                            cmd.Parameters.AddWithValue("@REMARK", "");
                            cmd.Parameters.AddWithValue("@APP_SO_NO", "");
                            cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                            cmd.Parameters.AddWithValue("@APP_SO_DATE", "");
                            cmd.Parameters.AddWithValue("@SO_APPROVEDATE", "");
                            cmd.Parameters.AddWithValue("@STATUS", "0");
                            cmd.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                            cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);


                            /* ---------- GST Implementation Start -----------*/
                            cmd.Parameters.AddWithValue("@DISTGSTINNO", Session["SITEGSTINNO"]);

                            if (Session["SITEGSTINREGDATE"] != null && Session["SITEGSTINREGDATE"].ToString().Trim().Length > 0)
                            {
                                cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", Convert.ToDateTime(Session["SITEGSTINREGDATE"]).ToString("dd-MMM-yyyy"));
                            }
                            cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                            cmd.Parameters.AddWithValue("@CUSTGSTINNO", txtGSTtin.Text);
                            if (txtGSTtinRegistration.Text != null && txtGSTtinRegistration.Text.Trim().Length > 0)
                            {
                                cmd.Parameters.AddWithValue("@CUSTGSTINREGDATE", Convert.ToDateTime(txtGSTtinRegistration.Text).ToString("dd-MMM-yyyy"));
                            }
                            cmd.Parameters.AddWithValue("@CUSTCOMPOSITIONSCHEME", (chkCompositionScheme.Checked == true ? 1 : 0));
                            cmd.Parameters.AddWithValue("@BILLTOADDRESS", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@SHIPTOADDRESS", ddlShipToAddress.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@BILLTOSTATE", txtBilltoState.Text);

                            /* ---------- GST Implementation End -----------*/

                            //cmd1 = new SqlCommand("InsertSaleLine");
                            cmd1 = new SqlCommand("InsertSaleLineNew");
                            cmd1.Connection = conn;
                            cmd1.Transaction = transaction;
                            cmd1.CommandTimeout = 3600;
                            cmd1.CommandType = CommandType.StoredProcedure;

                            int i = 0;
                            dtLineItems = (DataTable)Session["LineItem"];
                            for (int j = 0; j < dtLineItems.Rows.Count; j++)
                            {
                                cmd1.Parameters.Clear();
                                #region Sale order Line

                                int intDiscType = 0;
                                int intCalculation = 0;

                                if (dtLineItems.Rows[j]["CalculationBase"].ToString().ToUpper() == "PRICE")
                                {
                                    intCalculation = 0;
                                }
                                else if (dtLineItems.Rows[j]["CalculationBase"].ToString().ToUpper() == "MRP")
                                {
                                    intCalculation = 1;
                                }
                                else
                                {
                                    intCalculation = 2;
                                }
                                if (dtLineItems.Rows[j]["DiscType"].ToString().ToUpper() == "PER")
                                {
                                    intDiscType = 0;
                                }
                                else if (dtLineItems.Rows[j]["DiscType"].ToString().ToUpper() == "VAL")
                                {
                                    intDiscType = 1;
                                }
                                else
                                {
                                    intDiscType = 2;
                                }
                                cmd1.Parameters.AddWithValue("@statusProcedure", "Insert");
                                cmd1.Parameters.AddWithValue("@SO_NO", SO_NO);
                                cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                                cmd1.Parameters.AddWithValue("@RECID", "");
                                //cmd1.Parameters.AddWithValue("@LINE_NO", dtLineItems.Rows[j]["SNO"].ToString());
                                cmd1.Parameters.AddWithValue("@LINE_NO", j + 1);
                                cmd1.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                                cmd1.Parameters.AddWithValue("@PRODUCT_CODE", dtLineItems.Rows[j]["ProductCodeName"].ToString());
                                cmd1.Parameters.AddWithValue("@BOX", dtLineItems.Rows[j]["QtyBox"].ToString());
                                cmd1.Parameters.AddWithValue("@BOXPcs", Convert.ToDecimal(dtLineItems.Rows[j]["BoxPcs"].ToString()));
                                cmd1.Parameters.AddWithValue("@BOXQty", Convert.ToDecimal(dtLineItems.Rows[j]["OnlyQtyBox"].ToString()));
                                cmd1.Parameters.AddWithValue("@PcsQty", Convert.ToDecimal(dtLineItems.Rows[j]["QtyPcs"].ToString()));
                                cmd1.Parameters.AddWithValue("@BasePrice", Convert.ToDecimal(dtLineItems.Rows[j]["BasePrice"].ToString()));
                                cmd1.Parameters.AddWithValue("@TaxableAmount", Convert.ToDecimal(dtLineItems.Rows[j]["TaxableAmount"].ToString()));

                                //=======================================
                                cmd1.Parameters.AddWithValue("@CRATES", Convert.ToDecimal(dtLineItems.Rows[j]["QtyCrates"].ToString()));
                                cmd1.Parameters.AddWithValue("@LTR", Convert.ToDecimal(dtLineItems.Rows[j]["QtyLtr"].ToString()));
                                cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(dtLineItems.Rows[j]["Value"].ToString()));
                                cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(dtLineItems.Rows[j]["Price"].ToString()));

                                cmd1.Parameters.AddWithValue("@DiscType", intDiscType);
                                cmd1.Parameters.AddWithValue("@Disc", Convert.ToDecimal(dtLineItems.Rows[j]["Disc"].ToString()));
                                cmd1.Parameters.AddWithValue("@DiscVal", Convert.ToDecimal(dtLineItems.Rows[j]["DiscVal"].ToString()));
                                cmd1.Parameters.AddWithValue("@SECDISPER", Convert.ToDecimal(dtLineItems.Rows[j]["Sec_Disc_Per"]));
                                cmd1.Parameters.AddWithValue("@SECDISVAL", Convert.ToDecimal(dtLineItems.Rows[j]["SEC_DISC_AMOUNT"]));
                                cmd1.Parameters.AddWithValue("@TDValue", Convert.ToDecimal(dtLineItems.Rows[j]["TDValue"]));
                                cmd1.Parameters.AddWithValue("@TDPer", Convert.ToDecimal(dtLineItems.Rows[j]["TD_Per"]));
                                cmd1.Parameters.AddWithValue("@PE_VAL", Convert.ToDecimal(dtLineItems.Rows[j]["PE"]));
                                cmd1.Parameters.AddWithValue("@PE_PER", Convert.ToDecimal(dtLineItems.Rows[j]["PE_Per"]));

                                //===========For Tax=========
                                cmd1.Parameters.AddWithValue("@Tax_Code", Convert.ToDecimal(dtLineItems.Rows[j]["Tax_Code"].ToString()));
                                cmd1.Parameters.AddWithValue("@Tax_Amount", Convert.ToDecimal(dtLineItems.Rows[j]["Tax_Amount"].ToString()));
                                cmd1.Parameters.AddWithValue("@AddTax_Code", Convert.ToDecimal(dtLineItems.Rows[j]["AddTax_Code"].ToString()));
                                cmd1.Parameters.AddWithValue("@AddTax_Amount", Convert.ToDecimal(dtLineItems.Rows[j]["AddTax_Amount"].ToString()));
                                cmd1.Parameters.AddWithValue("@CalculationBase", intCalculation);
                                //====================

                                /*-------- GST IMPLEMENTATION START --------*/
                                cmd1.Parameters.AddWithValue("@HSNCODE", Convert.ToString(dtLineItems.Rows[j]["HSNCode"].ToString()));
                                cmd1.Parameters.AddWithValue("@COMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                                cmd1.Parameters.AddWithValue("@TAXCOMPONENT", Convert.ToString(dtLineItems.Rows[j]["TaxComponent"].ToString()));
                                cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", Convert.ToString(dtLineItems.Rows[j]["AddTaxComponent"].ToString()));
                                cmd1.Parameters.AddWithValue("@SchemeDiscPer", Convert.ToDecimal(dtLineItems.Rows[j]["SchemeDisc"].ToString()));
                                cmd1.Parameters.AddWithValue("@SchemeDiscVal", Convert.ToDecimal(dtLineItems.Rows[j]["SchemeDiscVal"].ToString()));
                                cmd1.Parameters.AddWithValue("@ADDSCHDISCPER", Convert.ToDecimal(dtLineItems.Rows[j]["ADDSCHDISCPER"].ToString()));
                                cmd1.Parameters.AddWithValue("@ADDSCHDISCVAL", Convert.ToDecimal(dtLineItems.Rows[j]["ADDSCHDISCVAL"].ToString()));
                                cmd1.Parameters.AddWithValue("@ADDSCHDISCAMT", Convert.ToDecimal(dtLineItems.Rows[j]["ADDSCHDISCAMT"].ToString()));
                                /*-------- GST IMPLEMENTATION END ---------*/

                                cmd1.Parameters.AddWithValue("@UOM", Convert.ToString(dtLineItems.Rows[j]["UOM"].ToString()));
                                cmd1.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                                cmd1.Parameters.AddWithValue("@DOCTYPE", 4);

                                cmd1.ExecuteNonQuery();
                                //Total += Convert.ToDecimal(grv.Cells[12].Text);
                                Total += Convert.ToDecimal(dtLineItems.Rows[j]["Value"].ToString());
                                #endregion
                            }
                            i = dtLineItems.Rows.Count;

                            foreach (GridViewRow grv in gvScheme.Rows)
                            {
                                if (((CheckBox)grv.FindControl("chkSelect")).Checked)
                                {
                                    #region Scheme Line Saving
                                    TextBox txtQtyToAvail = (TextBox)grv.FindControl("txtQty");
                                    TextBox txtQtyToAvailPcs = (TextBox)grv.FindControl("txtQtyPcs");
                                    HiddenField hdnSchemeLine = (HiddenField)grv.FindControl("hdnSchemeLine");
                                    HiddenField hdnSchemeType = (HiddenField)grv.FindControl("hdnSchemeType");

                                    txtQtyToAvail.Text = (txtQtyToAvail.Text.Trim().Length == 0 ? "0" : txtQtyToAvail.Text);
                                    txtQtyToAvailPcs.Text = (txtQtyToAvailPcs.Text.Trim().Length == 0 ? "0" : txtQtyToAvailPcs.Text);
                                    RefSchemeCode = grv.Cells[1].Text.ToString();
                                    RefSchemeLineNo = hdnSchemeLine.Value.ToString();
                                    RefSchemeType = Convert.ToInt16(hdnSchemeType.Value);
                                    decimal SchTaxCode1, SchTaxCode2;
                                    SchTaxCode1 = SchTaxCode2 = 0;

                                    using (DataTable dtSchtax = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE,H.ACXADDITIONALTAXBASIS from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + grv.Cells[4].Text.ToString() + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'"))
                                    {
                                        if (dtSchtax.Rows.Count > 0)
                                        {
                                            SchTaxCode1 = Convert.ToDecimal(dtSchtax.Rows[0]["TaxValue"].ToString());
                                            SchTaxCode2 = Convert.ToDecimal(dtSchtax.Rows[0]["ACXADDITIONALTAXVALUE"].ToString());
                                        }
                                    }
                                    decimal packSize = 1, boxqty = 0, pcsQty = 0;
                                    decimal billQty = 0;
                                    string boxPcs = "";
                                    using (DataTable dtFreeItem = baseObj.GetData("Select Cast(ISNULL(Product_PackSize,1) as int) AS Product_PackSize From AX.InventTable Where ItemId='" + grv.Cells[4].Text.ToString() + "'"))
                                    {
                                        if (dtFreeItem.Rows.Count > 0)
                                        {
                                            string strPack = dtFreeItem.Rows[0]["Product_PackSize"].ToString();
                                            packSize = Convert.ToDecimal(strPack);

                                        }
                                    }
                                    if (Convert.ToInt16(txtQtyToAvailPcs.Text) > 0)
                                    {
                                        pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text);
                                    }

                                    else
                                    {
                                        pcsQty = 0;
                                    }
                                    boxqty = (pcsQty >= packSize ? Convert.ToInt32(Math.Floor(pcsQty / packSize)) : 0);
                                    pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text) - (boxqty * packSize);
                                    boxqty = boxqty + Convert.ToInt32(txtQtyToAvail.Text);
                                    billQty = Math.Round((boxqty + (pcsQty / packSize)), 2);// Math.Round(Convert.ToDecimal((boxqty + (pcsQty / packSize))), 2);
                                    boxPcs = boxqty.ToString() + "." + (pcsQty.ToString().Length == 1 ? "0" : "") + pcsQty.ToString();
                                    string[] calValue = baseObj.CalculatePrice1(grv.Cells[4].Text, ddlCustomer.SelectedItem.Value, Convert.ToDecimal(billQty < 0 ? 1 : billQty), "Box", Session["SiteCode"].ToString());
                                    i = i + 1;
                                    cmd1.Parameters.Clear();
                                    if (billQty > 0)
                                    {
                                        #region Scheme Line Saving
                                        cmd1.Parameters.AddWithValue("@statusProcedure", "Insert");
                                        cmd1.Parameters.AddWithValue("@SO_NO", SO_NO);
                                        cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                                        cmd1.Parameters.AddWithValue("@RECID", "");
                                        cmd1.Parameters.AddWithValue("@LINE_NO", i);
                                        cmd1.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                                        cmd1.Parameters.AddWithValue("@PRODUCT_CODE", grv.Cells[4].Text);
                                        cmd1.Parameters.AddWithValue("@BOX", billQty);
                                        cmd1.Parameters.AddWithValue("@CRATES", Convert.ToDecimal(0));
                                        cmd1.Parameters.AddWithValue("@LTR", Convert.ToDecimal(calValue[1].ToString()));
                                        cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(grv.Cells[25].Text));
                                        cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(grv.Cells[16].Text));
                                        cmd1.Parameters.AddWithValue("@UOM", "");
                                        cmd1.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                                        cmd1.Parameters.AddWithValue("@DiscType", 2);
                                        cmd1.Parameters.AddWithValue("@Disc", new decimal(0));
                                        cmd1.Parameters.AddWithValue("@DiscVal", new decimal(0));
                                        cmd1.Parameters.AddWithValue("@SchemeCode", grv.Cells[1].Text);
                                        cmd1.Parameters.AddWithValue("@Slab", grv.Cells[6].Text);
                                        cmd1.Parameters.AddWithValue("@Tax_Code", Convert.ToDecimal(grv.Cells[21].Text));
                                        cmd1.Parameters.AddWithValue("@Tax_Amount", Convert.ToDecimal(grv.Cells[22].Text));
                                        cmd1.Parameters.AddWithValue("@AddTax_Code", Convert.ToDecimal(grv.Cells[23].Text));
                                        cmd1.Parameters.AddWithValue("@AddTax_Amount", Convert.ToDecimal(grv.Cells[24].Text));
                                        cmd1.Parameters.AddWithValue("@CalculationBase", 2);
                                        cmd1.Parameters.AddWithValue("@BasePrice", Convert.ToDecimal(grv.Cells[16].Text));
                                        cmd1.Parameters.AddWithValue("@BOXQty", txtQtyToAvail.Text);
                                        cmd1.Parameters.AddWithValue("@PcsQty", txtQtyToAvailPcs.Text);
                                        cmd1.Parameters.AddWithValue("@BOXPcs", boxPcs);
                                        cmd1.Parameters.AddWithValue("@TaxableAmount", Convert.ToDecimal(grv.Cells[20].Text));
                                        cmd1.Parameters.AddWithValue("@HSNCODE", grv.Cells[26].Text);
                                        cmd1.Parameters.AddWithValue("@COMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                                        cmd1.Parameters.AddWithValue("@TAXCOMPONENT", grv.Cells[27].Text.Replace("&nbsp;", ""));
                                        cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", grv.Cells[28].Text.Replace("&nbsp;", ""));
                                        cmd1.Parameters.AddWithValue("@SchemeDiscPer", Convert.ToDecimal(grv.Cells[18].Text));
                                        cmd1.Parameters.AddWithValue("@SchemeDiscVal", Convert.ToDecimal(grv.Cells[19].Text));
                                        cmd1.Parameters.AddWithValue("@ADDSCHDISCPER", Convert.ToDecimal(0));
                                        cmd1.Parameters.AddWithValue("@ADDSCHDISCVAL", Convert.ToDecimal(0));
                                        cmd1.Parameters.AddWithValue("@ADDSCHDISCAMT", Convert.ToDecimal(0));
                                        cmd1.Parameters.AddWithValue("@DOCTYPE", 4);
                                        cmd1.ExecuteNonQuery();
                                        Total += Convert.ToDecimal(grv.Cells[25].Text);

                                        #endregion
                                    }
                                    #endregion
                                }
                            }

                            //===============Delete From PreTable===============
                            cmd2 = new SqlCommand("ACX_USP_DeleteFromSalesOrderPre");
                            cmd2.Connection = conn;
                            cmd2.Transaction = transaction;
                            cmd2.CommandTimeout = 3600;
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.Clear();
                            cmd2.Parameters.AddWithValue("@Site_Code", Session["SITECODE"].ToString());
                            cmd2.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                            if (Session["SONO"] != null)
                            {
                                cmd2.Parameters.AddWithValue("@SO_NO", Session["SONO"].ToString());
                            }
                            else if (Session["PreSoNO"] != null)
                            {
                                cmd2.Parameters.AddWithValue("@SO_NO", Session["PreSoNO"].ToString());
                            }

                            cmd2.ExecuteNonQuery();
                            //===================================================

                            cmd.Parameters.AddWithValue("@SO_VALUE", Total);
                            cmd.Parameters.AddWithValue("@REFSCHEMECODE", RefSchemeCode);
                            cmd.Parameters.AddWithValue("@SCHREFRECID", RefSchemeLineNo);
                            cmd.Parameters.AddWithValue("@SCHEMETYPE", RefSchemeType);
                            cmd.ExecuteNonQuery();
                            //int a = obj.UpdateLastNumSequence(4, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn,transaction);
                            transaction.Commit();

                            // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Sale Order : " + SO_NO + " Generated Successfully.!');", true);

                            string message = "alert('Sale Order : " + SO_NO + " Generated Successfully.!');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            ResetAllControls();
                        }
                        else
                        {
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Add Line Items First !');", true);
                            string message = "alert('Please Add Line Items First !');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                            return;
                        }
                    }
                }
                else
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Free Quantity should be Equal !');", true);
                    string message = "alert('Free Quantity should be Equal !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                    return;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Error:" + ex.Message + " !');", true);
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('Error:" + ex.Message.Replace("'", "") + " !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                LblMessage1.Text = "";
                lblMessage.Text = "";
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open")
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        public void TDCalculation()
        {
            try
            {
                DataTable dtLineItems = new DataTable();
                if (Session["LineItem"] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session["LineItem"];
                }
                decimal totalBasicValue = 0;

                totalBasicValue = dtLineItems.AsEnumerable().Sum(r => r.Field<decimal>("OnlyQtyBox") * r.Field<decimal>("Price"));
                //=========For calculate TD % ================
                decimal TDPercentage = 0;
                if (totalBasicValue > 0)
                {
                    TDPercentage = Convert.ToDecimal(txtHdnTDValue.Text) / totalBasicValue;
                }
                decimal TDCheck = Convert.ToDecimal(txtHdnTDValue.Text);
                foreach (DataColumn dc in dtLineItems.Columns)
                {
                    dc.ReadOnly = false;
                }
                foreach (DataRow row in dtLineItems.Rows)
                {
                    decimal BasicValue, TD, TaxableAmount, Tax1, Tax2, PE, PE_PER, TOTAL, TOTALTAX;
                    int line;
                    line = Convert.ToInt32(row["SNO"]);
                    BasicValue = (Convert.ToDecimal(row["OnlyQtyBox"]) * Convert.ToDecimal(row["Price"]));
                    TD = (BasicValue * TDPercentage);
                    TOTALTAX = Convert.ToDecimal(row["Tax_Code"]) + Convert.ToDecimal(row["AddTax_Code"]);
                    if (TDCheck > 0)
                    {
                        PE_PER = TOTALTAX / (1 + (TOTALTAX / 100));
                    }
                    else
                    {
                        PE_PER = 0;
                    }
                    //PE_PER = TOTALTAX / (1 + (TOTALTAX / 100));
                    //PE = (TD * Convert.ToDecimal(15.25)/100);
                    PE = TD * PE_PER / 100;


                    TaxableAmount = (Convert.ToDecimal(row["OnlyQtyBox"]) * Convert.ToDecimal(row["Price"])) - Convert.ToDecimal(row["DiscVal"]) - Convert.ToDecimal(row["SEC_DISC_AMOUNT"]) - Convert.ToDecimal(row["SchemeDiscVal"]) - TD + PE - Convert.ToDecimal(row["ADDSCHDISCAMT"]);
                    Tax1 = TaxableAmount * Convert.ToDecimal(row["Tax_Code"]) / 100;
                    Tax2 = TaxableAmount * Convert.ToDecimal(row["AddTax_Code"]) / 100;
                    TOTAL = TaxableAmount + Tax1 + Tax2;
                    row["TD_Per"] = TDPercentage;
                    row["TDValue"] = TD.ToString(".00");
                    row["PE"] = PE;
                    row["PE_Per"] = PE_PER;
                    row["TaxableAmount"] = TaxableAmount;
                    row["Tax_Amount"] = Tax1;
                    row["AddTax_Amount"] = Tax2;
                    row["Value"] = TaxableAmount + Tax1 + Tax2;
                    UpdateTdVal(TD, TDPercentage, PE, PE_PER, TaxableAmount, Tax1, Tax2, TOTAL, line);
                    dtLineItems.AcceptChanges();
                }
                Session["LineItem"] = dtLineItems;
                BindingGird();
                //gvDetails.DataSource = dtLineItems;
                //gvDetails.DataBind();
                //GridViewFooterCalculate(dtLineItems);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "')", true);
            }


        }

        private bool ValidatePurchaseReturnHeaderData()
        {
            bool returnvalue = true;

            try
            {
                if (txtAddress.Text.Trim().Length == 0 || txtBilltoState.Text.Trim().Length == 0)
                {
                    string message = "alert('Select Customers Bill To Address !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    ddlCustomer.Focus();
                    returnvalue = false;
                }

                if (ddlShipToAddress.SelectedValue == null || ddlShipToAddress.SelectedValue.ToString().Trim().Length == 0)
                {
                    string message = "alert('Select Customers Ship To Address !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    ddlCustomer.Focus();
                    returnvalue = false;
                }

                if (ddlCustomerGroup.SelectedItem.Value == "Select...")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer Group');", true);

                    string message = "alert('Select Customer Group');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                }

                if (ddlPSRName.Visible == true)
                {
                    if (ddlPSRName.SelectedItem.Value == "Select..." || ddlPSRName.Text == string.Empty)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select PSR Name !');", true);

                        string message = "alert('Select PSR Name !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                        ddlPSRName.Focus();
                        returnvalue = false;

                    }
                    if (ddlBeatName.Text == string.Empty || ddlBeatName.SelectedIndex == -1 || ddlBeatName.SelectedItem.Text == "Select...")
                    {
                        // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Beat Name !');", true);

                        string message = "alert('Select Beat Name !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                        ddlBeatName.Focus();
                        returnvalue = false;
                    }

                }

                //Customer code
                if (ddlCustomer.SelectedIndex == -1)
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer..');", true);

                    string message = "alert('Select Customer..');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                    returnvalue = false;
                }
                else
                {
                    if (ddlCustomer.SelectedItem.Text == "Select...")
                    {
                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer..');", true);

                        string message = "alert('Select Customer..');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                        returnvalue = false;
                    }
                }
                //Select Date

                if (txtDeliveryDate.Text == "")
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Date... !');", true);
                    string message = "alert('Select Date... !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                    returnvalue = false;
                }


                //if (Session["Exempt_ComVal"] == null || Convert.ToString(Session["Exempt_ComVal"].ToString()) == "")
                //{
                //
                //    Session["Exempt_ComVal"] = Session["Exempt_CurVal"];
                //}
                //
                //if (Convert.ToString(Session["Exempt_CurVal"]) != Convert.ToString(Session["Exempt_ComVal"]))
                //{
                //    txtBoxPcs.Text = "";
                //    txtViewTotalBox.Text = "";
                //    txtViewTotalPcs.Text = "";
                //    txtPcs.Text = "";
                //    txtCrate.Text = "";
                //    txtEnterQty.Text = "";
                //    txtQtyBox.Text = "";
                //    txtQtyCrates.Text = "";
                //    txtLtr.Text = "";
                //    txtPrice.Text = "";
                //    txtValue.Text = "";
                //
                //    string message = "alert('PRODUCT cannot be added because of different Exempt value');";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                //
                //    returnvalue = false;
                //}

                //  check grid item==========
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Error: " + ex.Message + " !');", true);
                string message = "alert('Error: " + ex.Message + " !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                returnvalue = false;
            }
            return returnvalue;
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query3 = "select GSTINNO,GSTREGISTRATIONDATE,COMPOSITIONSCHEME,Address1,STATE,Mobile_No from VW_CUSTOMERGSTINFO where CUSTOMER_CODE='" + ddlCustomer.SelectedValue + "'";

            DataTable dt1 = baseObj.GetData(Query3);
            if (dt1.Rows.Count > 0)
            {
                txtGSTtin.Text = dt1.Rows[0]["GSTINNO"].ToString();
                txtGSTtinRegistration.Text = dt1.Rows[0]["GSTREGISTRATIONDATE"].ToString();
                chkCompositionScheme.Checked = Convert.ToBoolean(dt1.Rows[0]["COMPOSITIONSCHEME"]);
                txtBilltoState.Text = dt1.Rows[0]["STATE"].ToString();
                txtAddress.Text = dt1.Rows[0]["Address1"].ToString();
                txtContactNo.Text = dt1.Rows[0]["Mobile_No"].ToString();

                String qquery = "Exec [dbo].[usp_GetCustomerOutstandingDetail] '" + txtBilltoState.Text + "','" + Session["SITECODE"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + ddlCustomer.SelectedValue + "'";

                DataTable dtt = baseObj.GetData(qquery);
                if (dtt.Rows.Count > 0)
                {
                    txtOutstandingAmount.Text = Convert.ToDecimal(dtt.Rows[0]["OutstandingAmount"].ToString()).ToString("0.00");
                    //lblMessage.Visible = true;
                }
                else
                {
                    txtOutstandingAmount.Text = "0.00";
                    //lblMessage.Visible = false;
                }
            }
            else
            {
                txtGSTtin.Text = "";
                txtGSTtinRegistration.Text = "";
                chkCompositionScheme.Checked = false;
                txtBilltoState.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtContactNo.Text = string.Empty;
            }

            Query2 = "EXEC USP_CUSTSHIPTOADDRESS '" + ddlCustomer.SelectedValue + "'";
            ddlShipToAddress.Items.Clear();
            baseObj.BindToDropDown(ddlShipToAddress, Query2, "SHIPTOADDRESS", "SHIPTOADDRESS");

            if (ddlBeatName.SelectedIndex > 0)
            {
                //strQuery = "Select Mobile_No,Address1 from ax.ACXCUSTMASTER where Customer_Code ='" + ddlCustomer.SelectedValue + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "' ";
                //DataTable dt = baseObj.GetData(strQuery);
                //if (dt.Rows.Count > 0)
                //{
                //    txtContactNo.Text = dt.Rows[0]["Mobile_No"].ToString();
                //    txtAddress.Text = dt.Rows[0]["Address1"].ToString();
                //}
                //else
                //{
                //    txtContactNo.Text = "";
                //    txtAddress.Text = "";
                //}

                ///////Vijay////////////////
                //AddColumnInDataTable();
                Session["LineItem"] = null;
                Session["SchemeLineItem"] = null;
                Session["SoNo"] = null;
                //Session["Exempt_ComVal"] = null;
                Session["Exempt_CurVal"] = null;
                //Session["Comm_Exem"] = null;
                //Session["Curr_Exem"] = null;
                dtLineItems = null;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gvScheme.DataSource = null;
                gvScheme.DataBind();
                //BindSchemeGrid();
            }
            else
            {
                strQuery = @"Select Ac.Mobile_No,Ac.ADDRESS1,Ac.PSR_CODE,AP.PSR_Name,APB.BeatName,APB.BeatCode,Ac.APPLICABLESCHEMEDISCOUNT,case Ac.APPLICABLESCHEMEDISCOUNT when  0 then 'None' when 1 then 'Scheme' when 2 then 'Discount' when 3 then 'Both' end SchemeDiscount "
                         + " from ax.ACXCUSTMASTER Ac "
                         + " left Outer Join [ax].[ACXPSRMaster] AP on Ac.PSR_CODE = AP.PSR_Code "
                         + " left Outer Join [ax].[ACXPSRBeatMaster] APB on AP.PSR_CODE = APB.PSRCode and Ac.PSR_BEAT = APB.BeatCode"
                         + " where Ac.Customer_Code ='" + ddlCustomer.SelectedValue + "' and Ac.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' ";
                //hint
                DataTable dt = baseObj.GetData(strQuery);
                if (dt.Rows.Count > 0)
                {
                    txtContactNo.Text = dt.Rows[0]["Mobile_No"].ToString();
                    txtAddress.Text = dt.Rows[0]["Address1"].ToString();
                    txtSchemeDisc.Text = dt.Rows[0]["SchemeDiscount"].ToString();
                    ddlPSRName.Items.Clear();
                    ddlBeatName.Items.Clear();
                    //txtSchemeDisc.Items.Clear();
                    ddlPSRName.Items.Add(new ListItem(dt.Rows[0]["PSR_CODE"].ToString() + "-" + dt.Rows[0]["PSR_Name"].ToString(), dt.Rows[0]["PSR_CODE"].ToString()));
                    ddlBeatName.Items.Add(new ListItem(dt.Rows[0]["BeatCode"].ToString() + "-" + dt.Rows[0]["BeatName"].ToString(), dt.Rows[0]["BeatCode"].ToString()));
                    //ddlSchemeDisc.Items.Add(new ListItem(dt.Rows[0]["SchemeDiscount"].ToString()));
                    //ddlSchemeDisc.Items.Add(new ListItem(dt.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString() + "-" + dt.Rows[0]["SCHEME_DISCOUNT"].ToString(), dt.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString()));
                }
                else
                {
                    txtContactNo.Text = "";
                    txtAddress.Text = "";
                }
                ///////Vijay////////////////
                //AddColumnInDataTable();
                Session["LineItem"] = null;
                Session["SchemeLineItem"] = null;
                Session["SoNo"] = null;
                //Session["Exempt_ComVal"] = null;
                Session["Exempt_CurVal"] = null;
                //Session["Comm_Exem"] = null;
                //Session["Curr_Exem"] = null;
                dtLineItems = null;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gvScheme.DataSource = null;
                gvScheme.DataBind();
            }
            DDLProductGroup.Focus();
        }

        protected void ddlPSRName_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = @"select BeatCode +'-'+BeatName as BeatName,BeatCode from [ax].[ACXPSRBeatMaster] where PSRCode='" + ddlPSRName.SelectedItem.Value + "'";
            ddlBeatName.Items.Clear();
            ddlBeatName.Items.Add("Select...");
            baseObj.BindToDropDown(ddlBeatName, strQuery, "BeatName", "BeatCode");

        }

        protected void ddlCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomerGroup.SelectedValue == "CG0001")
            {
                strQuery = @"Select PSR_Code +'-'+ PSR_Name as PSRName,PSR_Code from [ax].[ACXPSRMaster] where PSR_Code  " +
                          " in (select A.PSRCode from [ax].[ACXPSRBeatMaster] A  " +
                          " left Join [ax].[ACXPSRSITELinkingMaster] B on A.PSRCode = B.PSRCode " +
                          " where B.Site_code ='" + Session["SiteCode"].ToString() + "')";
                ddlPSRName.Items.Clear();
                ddlPSRName.Items.Add("Select...");
                baseObj.BindToDropDown(ddlPSRName, strQuery, "PSRName", "PSR_Code");
                lblBeatName.Visible = true;
                lblPSRName.Visible = true;
                ddlBeatName.Visible = true;
                ddlPSRName.Visible = true;
                ddlCustomer.Items.Clear();
                //strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'";
                strQuery = "Select distinct Customer_Code + '-' + Customer_Name as Name,Customer_Code from ax.ACXCUSTMASTER CM" +
                                " JOIN ax.ACXPSRSITELinkingMaster PM on CM.SITE_CODE = PM.SITE_CODE and CM.PSR_CODE = PM.PSRCODE" +
                                " where CM.BLOCKED = 0 AND CM.CUST_GROUP = '" + ddlCustomerGroup.SelectedValue + "' AND CM.SITE_CODE = '" + Session["SiteCode"].ToString() + "'" +
                                 " and (PM.ORDERALLOWED = '1' or PM.ORDERALLOWED = '2')";
                ddlCustomer.Items.Clear();
                txtContactNo.Text = "";
                txtAddress.Text = "";
                ddlCustomer.Items.Add("Select...");
                baseObj.BindToDropDown(ddlCustomer, strQuery, "Name", "Customer_Code");
            }
            else
            {
                ddlPSRName.Items.Clear();
                ddlBeatName.Items.Clear();
                ddlBeatName.Visible = false;
                ddlPSRName.Visible = false;
                lblPSRName.Visible = false;
                lblBeatName.Visible = false;
                //strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'"; //--AND SITE_CODE='657546'";
                //if (ddlCustomerGroup.SelectedValue == "CG0004")
                //{
                //    strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'" +
                //            " Union  All " +
                //            " Select Name=(Select A.Customer_Code+'-'+A.Customer_Name as Name  from ax.ACXCUSTMASTER A where A.Blocked = 0 and B.SubDistributor=A.Customer_Code AND A.CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' ),B.SubDistributor as Customer_Code " +
                //            " from ax.ACX_SDLinking B Where B.Other_Site='" + Session["SiteCode"].ToString() + "'";
                //}
                //else
                //{
                //    strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'";
                //
                //}

                //strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'"; //--AND SITE_CODE='657546'";
                //if (ddlCustomerGroup.SelectedValue == "CG0004")
                //{
                strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'" +
                        " Union  All " +
                        "select B.SUBDISTRIBUTOR +'-'+ A.Customer_Name as Name,B.SUBDISTRIBUTOR Customer_Code  from ax.ACX_SDLinking B LEFT OUTER JOIN AX.ACXCUSTMASTER A ON A.CUSTOMER_CODE=B.SUBDISTRIBUTOR WHERE B.Other_Site='" + Session["SiteCode"].ToString() + "' AND A.CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND A.BLOCKED=0 AND B.BLOCKED=0 ";
                //" Select Name=(Select A.Customer_Code+'-'+A.Customer_Name as Name  from ax.ACXCUSTMASTER A where A.Blocked = 0 and B.SubDistributor=A.Customer_Code AND A.CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' ),B.SubDistributor as Customer_Code " +
                //" from ax.ACX_SDLinking B Where B.Other_Site='" + Session["SiteCode"].ToString() + "'";
                //}
                //else
                //{
                //    strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'";

                //}


                ddlCustomer.Items.Clear();
                txtContactNo.Text = "";
                txtAddress.Text = "";
                ddlCustomer.Items.Add("Select...");
                baseObj.BindToDropDown(ddlCustomer, strQuery, "Name", "Customer_Code");

            }
        }

        protected void ddlBuilt_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ResetAllControls()
        {
            Session["SONO"] = null;
            Session["LineItem"] = null;
            Session["SchemeLineItem"] = null;
            Session["PreSoNO"] = null;
            //Session["Exempt_ComVal"] = null;
            Session["Exempt_CurVal"] = null;
            //Session["Comm_Exem"] = null;
            //Session["Curr_Exem"] = null;
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            gvDetails.Visible = false;
            // FillMaterialGroup();
            //DDLMaterialCode.Items.Clear();
            txtBoxPcs.Text = "";
            txtCrate.Text = "";
            txtPcs.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtGSTtin.Text = "";
            txtGSTtinRegistration.Text = "";
            txtSchemeDisc.Text = "";
            TotQty.Text = "";
            InvTot.Text = "";
            txtQtyCrates.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtHdnTDValue.Text = "0";
            fillCustomerGroup();
            ddlCustomer.Items.Clear();
            ddlCustomer.Items.Add("Select...");
            ddlBusinessUnit.SelectedIndex = 0;
            lblPSRName.Visible = false;
            ddlPSRName.Items.Clear();
            ddlPSRName.Visible = false;
            lblBeatName.Visible = false;
            ddlBeatName.Items.Clear();
            ddlBeatName.Visible = false;
            txtContactNo.Text = string.Empty;
            txtAddress.Text = string.Empty;
            rdNonExempt.Enabled = true;
            rdExempt.Enabled = true;
            rdExempt.Checked = false;
            rdNonExempt.Checked = true;
            txtBilltoState.Text = string.Empty;
            ddlShipToAddress.Items.Clear();
            txtDeliveryDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            gvScheme.DataSource = null;
            gvScheme.DataBind();
            txtBoxPcs.Text = "";
            LblMessage1.Text = "";
            lblMessage.Text = "";
            DDLProductSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();
        }

        protected void ddlBeatName_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = @"Select Customer_Code+'-'+Customer_Name as Name,Customer_Code from ax.ACXCUSTMASTER where Blocked = 0 and PSR_CODE ='" + ddlPSRName.SelectedItem.Value + "' and PSR_BEAT='" + ddlBeatName.SelectedItem.Value + "' "
                      + " and SITE_CODE in ('" + Session["SiteCode"].ToString() + "') ";
            ddlCustomer.Items.Clear();
            txtContactNo.Text = "";
            txtAddress.Text = "";
            ddlCustomer.Items.Add("Select...");
            baseObj.BindToDropDown(ddlCustomer, strQuery, "Name", "Customer_Code");
        }

        protected void DDLProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCrate.Text = "0";
            txtPcs.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            txtBoxPcs.Text = "0.00";
            lblMessage.Text = "";
            LblMessage1.Text = "";
            FillProductCode();

            //string strcondition = "";
            //if (DDLProductGroup.SelectedIndex > 0)
            //{ strcondition += " and inv.Product_Group = '" + DDLProductGroup.SelectedValue + "' "; }
            //if (DDLProductSubCategory.SelectedIndex > 0)
            //{ strcondition += " and inv.PRODUCT_SUBCATEGORY = '" + DDLProductSubCategory.SelectedValue + "' "; }
            //strQuery = "SELECT distinct inv.ITEMID+'-'+inv.Product_Name as Name,inv.ITEMID from ax.INVENTTABLE inv where INV.block=0 " + strcondition;
            //if (rdStock.Checked == true)
            //{
            //    strQuery = "SELECT distinct inv.ITEMID + '-' + inv.Product_Name as Name,inv.ITEMID from ax.INVENTTABLE inv" +
            //                " inner join AX.ACXINVENTTRANS TT on inv.itemid = tt.productcode and SITECODE = '" + Session["SiteCode"].ToString() + "'" +
            //                " inner join ax.INVENTSITE i on tt.TransLocation = i.MAINWAREHOUSE and i.SITEID = TT.SiteCode where " +
            //                " INV.block = 0 " + strcondition + " GROUP BY inv.ITEMID,inv.Product_Name having SUM(TT.TransQty) > 0";


            //    //strQuery += " and inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
            //}
            //DDLMaterialCode.DataSource = null;
            //DDLMaterialCode.Items.Clear();
            //baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Name", "ITEMID");
            //DDLMaterialCode.Items.Add("Select...");

            DDLMaterialCode.Enabled = true;
            DDLMaterialCode.Focus();
            //if (DDLMaterialCode.SelectedItem.Text != "Select...")
            //{
            //    DDLMaterialCode_SelectedIndexChanged(null, null);
            //}
            PcsBillingApplicable();
        }

        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCrate.Text = "";
            txtPcs.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            txtBoxPcs.Text = "";
            lblMessage.Text = "";
            LblMessage1.Text = "";
            //===========Fill Product Group and Product Sub Cat========
            DataTable dt = new DataTable();
            string query = "select iTEMID,Product_Group,PRODUCT_SUBCATEGORY,cast(Product_PackSize as decimal(10,2)) as Product_PackSize,Cast(Product_Crate_PackSize as decimal(10,2)) as Product_CrateSize,EXEMPT from ax.INVENTTABLE where ItemId='" + DDLMaterialCode.SelectedItem.Value + "' order by Product_Name";
            dt = baseObj.GetData(query);

            if (dt.Rows.Count > 0)
            {
                DDLProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                if (DDLProductSubCategory.Items.FindByText(dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString()) == null)
                {
                    ddlProductGroup_SelectedIndexChanged(sender, e);
                }
                //ProductSubCategory();
                //=============For Product Sub Cat======
                DDLProductSubCategory.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
                DDLMaterialCode.SelectedValue = dt.Rows[0]["ItemId"].ToString();
                Session["Exempt_CurVal"] = dt.Rows[0]["EXEMPT"];
            }

            txtEnterQty.Focus();
            PcsBillingApplicable();
            //DDLMaterialCode.Items.Clear();
        }

        public void BindSchemeGrid()
        {
            Session["SessionScheme"] = null;
            DataTable dt = GetDatafromSP(@"ACX_SCHEME");
            Session["SessionScheme"] = dt;
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataTable dtscheme = new DataTable();
                    dtscheme = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + row["Free Item Code"].ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "',1," + Convert.ToDecimal(row["Rate"].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                    if (dtscheme.Rows.Count > 0)
                    {
                        dt.Columns["Tax1Per"].ReadOnly = false;
                        dt.Columns["Tax2Per"].ReadOnly = false;
                        dt.Columns["Tax1"].ReadOnly = false;
                        dt.Columns["Tax2"].ReadOnly = false;
                        dt.Columns["Tax1Component"].ReadOnly = false;
                        dt.Columns["Tax2Component"].ReadOnly = false;
                        dt.Columns["Tax2Component"].MaxLength = 20;
                        dt.Columns["Tax1Component"].MaxLength = 20;
                        if (dtscheme.Rows[0]["RETMSG"].ToString().IndexOf("TRUE") >= 0)
                        {
                            row["Tax1Per"] = dtscheme.Rows[0]["TAX_PER"];
                            row["Tax2Per"] = dtscheme.Rows[0]["ADDTAX_PER"];
                            row["Tax1"] = dtscheme.Rows[0]["Tax1"];
                            row["Tax2"] = dtscheme.Rows[0]["Tax2"];
                            row["Tax1Component"] = dtscheme.Rows[0]["TAX1COMPONENT"];
                            row["Tax2Component"] = dtscheme.Rows[0]["TAX2COMPONENT"];
                            dt.AcceptChanges();
                        }
                    }
                }
            }
            Session["SessionSchGrid"] = dt;
            gvScheme.DataSource = dt;
            gvScheme.DataBind();
            //gvScheme.DataSource = dt;
            //gvScheme.DataBind();
        }


        public DataTable GetDatafromSP(string SPName)
        {
            conn = baseObj.GetConnection();
            try
            {
                string ItemGroup = string.Empty;
                string ItemCode = string.Empty;
                foreach (GridViewRow row in gvDetails.Rows)
                {
                    if (row.RowIndex == 0)
                    {
                        ItemCode = "" + row.Cells[3].Text + "";
                    }
                    else
                    {
                        ItemCode += "," + row.Cells[3].Text + "";
                    }
                }

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPName;
                cmd.Parameters.AddWithValue("@PLACESITE", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@PLACESTATE", Session["SCHSTATE"].ToString());
                cmd.Parameters.AddWithValue("@PLACEALL", "");
                cmd.Parameters.AddWithValue("@CUSCODECUS", ddlCustomer.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CUSCODEGROUP", ddlCustomerGroup.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CUSCODEALL", "");
                cmd.Parameters.AddWithValue("@ITEMITEM", ItemCode);
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                //cmd.Parameters.AddWithValue("@ITEMGROUP",DDLProductGroup.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@ITEMALL", "");

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                if (dt.Rows.Count > 0)
                {
                    #region Scheme Found
                    dt.Columns.Add("TotalFreeQty", typeof(System.Int16));
                    dt.Columns.Add("TotalFreeQtyPCS", typeof(System.Int16));
                    dt.Columns.Add("TotalSchemeValueoff", typeof(System.Decimal));
                    DataTable dt1 = dt.Clone();
                    dt1.Clear();

                    Int16 TotalQtyofGroupItem = 0;
                    Int16 TotalPCSQtyofGroupItem = 0;
                    Int16 TotalQtyofItem = 0;
                    Int16 TotalPCSQtyofItem = 0;

                    decimal TotalValueofGroupItem = 0;
                    decimal TotalValueofItem = 0;
                    dt.Columns["SCHBOX"].ReadOnly = false;
                    dt.Columns["SCHPCS"].ReadOnly = false;
                    dt.Columns["SCHVALUE"].ReadOnly = false;
                    dt.Columns["MINBOX"].ReadOnly = false;
                    dt.Columns["MINPCS"].ReadOnly = false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["TotalFreeQty"] = Convert.ToInt16("0");
                        dr["TotalFreeQtyPCS"] = Convert.ToInt16("0");
                        dr["TotalSchemeValueoff"] = Convert.ToDecimal("0.0");
                        #region Minimum Qty
                        if (Convert.ToInt16(dr["MINIMUMQUANTITY"]) > 0)
                        {
                            TotalQtyofGroupItem = GetQtyofGroupItem(dr["Scheme Item group"].ToString(), "BOX");
                            TotalQtyofItem = GetQtyofItem(dr["Scheme Item group"].ToString(), "BOX");
                            dr["SCHBOX"] = TotalQtyofGroupItem + TotalQtyofItem;
                            if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                            {
                                dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                            }
                            if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                            {
                                dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTY"]));
                            }
                            if (dr["Schemetype"].ToString() == "3")
                            {
                                //dr["TotalSchemeValueoff"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));

                                dr["TotalSchemeValueoff"] = Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"])) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]);
                            }
                        }
                        #endregion
                        #region Minimum PCS
                        if (Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]) > 0)
                        {
                            TotalPCSQtyofGroupItem = GetQtyofGroupItem(dr["Scheme Item group"].ToString(), "PCS");
                            TotalPCSQtyofItem = GetQtyofItem(dr["Scheme Item group"].ToString(), "PCS");
                            dr["SCHPCS"] = TotalPCSQtyofGroupItem + TotalPCSQtyofItem;
                            if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                            {
                                dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                            }
                            if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                            {
                                dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(dr["FREEQTY"]));
                            }
                            if (dr["Schemetype"].ToString() == "3")
                            {
                                dr["TotalSchemeValueoff"] = Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"])) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]);
                                //dr["TotalSchemeValueoff"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));
                            }
                        }
                        #endregion
                        #region Minimum Value
                        if (Convert.ToInt16(dr["MINIMUMVALUE"]) > 0)
                        {
                            TotalValueofGroupItem = GetValueofGroupItem(dr["Scheme Item group"].ToString());
                            TotalValueofItem = GetValueofItem(dr["Scheme Item group"].ToString());
                            dr["SCHVALUE"] = TotalValueofGroupItem + TotalValueofItem;
                            if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                            {
                                dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                            }
                            if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                            {
                                dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTY"]));
                            }
                            if (dr["Schemetype"].ToString() == "3")
                            {
                                dr["TotalSchemeValueoff"] = Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"])) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]);
                                //dr["TotalSchemeValueoff"] = Convert.ToDecimal(System.Math.Floor(Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));
                            }
                        }
                        #endregion
                        #region Minimum Purchase Type Check
                        if (Convert.ToInt16(dr["MINIMUMPURCHASEITEMTYPE"]) == 0)
                        {
                            dr["MINBOX"] = "0";
                            dr["MINPCS"] = "0";
                        }
                        if (Convert.ToInt16(dr["MINIMUMPURCHASEBOX"]) > 0 && Convert.ToInt16(dr["MINIMUMPURCHASEITEMTYPE"]) > 0)
                        {
                            TotalQtyofGroupItem = GetQtyofGroupItem(dr["MINIMUMPURCHASEITEMGROUP"].ToString(), "BOX");
                            TotalQtyofItem = GetQtyofItem(dr["MINIMUMPURCHASEITEMGROUP"].ToString(), "BOX");
                            dr["MINBOX"] = TotalQtyofGroupItem + TotalQtyofItem;
                        }
                        if (Convert.ToInt16(dr["MINIMUMPURCHASEPCS"]) > 0 && Convert.ToInt16(dr["MINIMUMPURCHASEITEMTYPE"]) > 0)
                        {
                            TotalPCSQtyofGroupItem = GetQtyofGroupItem(dr["MINIMUMPURCHASEITEMGROUP"].ToString(), "PCS");
                            TotalPCSQtyofItem = GetQtyofItem(dr["MINIMUMPURCHASEITEMGROUP"].ToString(), "PCS");
                            dr["MINPCS"] = TotalPCSQtyofGroupItem + TotalPCSQtyofItem;
                        }
                        #endregion
                        dt.AcceptChanges();
                    }
                    DataRow[] drSchFound = dt.Select("(MINIMUMQUANTITY<=SCHBOX OR MINIMUMVALUE<=SCHVALUE OR MINIMUMQUANTITYPCS<=SCHPCS) AND (MINIMUMPURCHASEBOX<=MINBOX OR MINIMUMPURCHASEPCS<=MINPCS)");
                    if (drSchFound.Length > 0)
                    {
                        dt1 = drSchFound.CopyToDataTable();
                    }
                    else
                    {
                        return null;
                    }
                    //dt1 = dt.Select("(MINIMUMQUANTITY<=SCHBOX OR MINIMUMVALUE<=SCHVALUE OR MINIMUMQUANTITYPCS<=SCHPCS) AND (MINIMUMPURCHASEBOX<=MINBOX OR MINIMUMPURCHASEPCS<=MINPCS)").CopyToDataTable();

                    //dt1 = dt.Select("(MINIMUMPURCHASEBOX<=MINBOX OR MINIMUMPURCHASEPCS<=MINPCS)").CopyToDataTable();
                    DataTable dt3 = dt1.Clone();

                    #region For Qty
                    DataView view = new DataView(dt1);
                    view.RowFilter = "[Scheme Type]=0";
                    DataTable distinctTable = (DataTable)view.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITY");

                    DataView dvSort = new DataView(distinctTable);
                    dvSort.Sort = "SCHEMECODE ASC, MINIMUMQUANTITY DESC";

                    DataView Dv1 = null;
                    int intCalRemFreeQty = 0;
                    string schemeCode = string.Empty;

                    foreach (DataRowView drv in dvSort)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            intCalRemFreeQty = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }

                        if (intCalRemFreeQty == 0)
                        {
                            // Qty Scheme Filter
                            if (Convert.ToInt16(drv.Row["MINIMUMQUANTITY"]) > 0)
                            {
                                #region Scheme Free Lines BOX Free Qty
                                Dv1 = new DataView(dt1);
                                Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY='" + drv.Row["MINIMUMQUANTITY"] + "' AND FREEQTY>0";

                                foreach (DataRowView drv2 in Dv1)
                                {
                                    if (Convert.ToDecimal(drv2["SCHBOX"]) >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                                #endregion

                                #region Scheme Free Lines For PCS Free Qty
                                // PCS Qty Scheme Filter
                                Dv1 = new DataView(dt1);
                                Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY='" + drv.Row["MINIMUMQUANTITY"] + "' AND FREEQTYPCS>0";

                                foreach (DataRowView drv2 in Dv1)
                                {
                                    if (Convert.ToDecimal(drv2["SCHBOX"]) >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                                #endregion
                            }
                            intCalRemFreeQty = +1;
                        }
                    }
                    #endregion

                    #region For PCS Qty

                    view = new DataView(dt1);
                    view.RowFilter = "[Scheme Type]=0";
                    distinctTable = (DataTable)view.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITYPCS");

                    dvSort = new DataView(distinctTable);
                    dvSort.Sort = "SCHEMECODE ASC,MINIMUMQUANTITYPCS DESC";

                    Dv1 = null;
                    intCalRemFreeQty = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSort)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            intCalRemFreeQty = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (intCalRemFreeQty == 0)
                        {
                            #region Scheme Free Lines BOX Free Qty
                            Dv1 = new DataView(dt1);
                            Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS='" + drv.Row["MINIMUMQUANTITYPCS"] + "' AND FREEQTY>0 AND MINIMUMQUANTITYPCS>0";

                            foreach (DataRowView drv2 in Dv1)
                            {
                                if (Convert.ToDecimal(drv2["SCHPCS"]) >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    dt3.ImportRow(drv2.Row);
                                }
                            }
                            #endregion

                            #region Scheme Free Lines PCS Free Qty
                            Dv1 = new DataView(dt1);
                            Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS='" + drv.Row["MINIMUMQUANTITYPCS"] + "' AND FREEQTYPCS>0 AND MINIMUMQUANTITYPCS>0";

                            foreach (DataRowView drv2 in Dv1)
                            {
                                if (Convert.ToDecimal(drv2["SCHPCS"]) >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    dt3.ImportRow(drv2.Row);
                                }
                            }
                            #endregion
                        }
                        intCalRemFreeQty += 1;
                    }

                    #endregion
                    #region for Value

                    DataView viewValue = new DataView(dt1);
                    viewValue.RowFilter = "[Scheme Type]=1";
                    DataTable distinctTableValue = (DataTable)viewValue.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMVALUE");

                    DataView dvSortValue = new DataView(distinctTableValue);
                    dvSortValue.Sort = "SCHEMECODE ASC,MINIMUMVALUE DESC";

                    DataView Dv1Value = null;
                    Int16 IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortValue)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMVALUE >='" + drv.Row["MINIMUMVALUE"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["FREEQTY"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHVALUE"]) >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                                if (Convert.ToInt16(drv2["FREEQTYPCS"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHVALUE"]) >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }

                    #endregion
                    #region Scheme Percentage
                    DataView viewScheme = new DataView(dt1);
                    viewScheme.RowFilter = "[Scheme Type]=2";
                    DataTable distinctTableScheme = (DataTable)viewScheme.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMVALUE");

                    DataView dvSortScheme = new DataView(distinctTableScheme);
                    dvSortScheme.Sort = "SCHEMECODE ASC,MINIMUMVALUE DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortScheme)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMVALUE >='" + drv.Row["MINIMUMVALUE"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMVALUE"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHVALUE"]) >= Convert.ToInt16(drv2["MINIMUMVALUE"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    viewScheme = new DataView(dt1);
                    viewScheme.RowFilter = "[Scheme Type]=2";
                    distinctTableScheme = (DataTable)viewScheme.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITY");

                    dvSortScheme = new DataView(distinctTableScheme);
                    dvSortScheme.Sort = "SCHEMECODE ASC,MINIMUMQUANTITY DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortScheme)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY >='" + drv.Row["MINIMUMQUANTITY"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMQUANTITY"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHBOX"]) >= Convert.ToDecimal(drv2["MINIMUMQUANTITY"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    viewScheme = new DataView(dt1);
                    viewScheme.RowFilter = "[Scheme Type]=2";
                    distinctTableScheme = (DataTable)viewScheme.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITYPCS");

                    dvSortScheme = new DataView(distinctTableScheme);
                    dvSortScheme.Sort = "SCHEMECODE ASC,MINIMUMQUANTITYPCS DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortScheme)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS >='" + drv.Row["MINIMUMQUANTITYPCS"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]) > 0)
                                {

                                    if (Convert.ToDecimal(drv2["SCHPCS"]) >= Convert.ToDecimal(drv2["MINIMUMQUANTITYPCS"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    #endregion
                    #region Scheme Valueoff
                    DataView viewSchemeValueoff = new DataView(dt1);
                    viewSchemeValueoff.RowFilter = "[Scheme Type]=3";
                    DataTable distinctTableSchemeValueOff = (DataTable)viewSchemeValueoff.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMVALUE");

                    DataView dvSortSchemeValueoff = new DataView(distinctTableSchemeValueOff);
                    dvSortSchemeValueoff.Sort = "SCHEMECODE ASC,MINIMUMVALUE DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortSchemeValueoff)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMVALUE >='" + drv.Row["MINIMUMVALUE"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMVALUE"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHVALUE"]) >= Convert.ToInt16(drv2["MINIMUMVALUE"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    viewSchemeValueoff = new DataView(dt1);
                    viewSchemeValueoff.RowFilter = "[Scheme Type]=3";
                    distinctTableSchemeValueOff = (DataTable)viewSchemeValueoff.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITY");

                    dvSortSchemeValueoff = new DataView(distinctTableSchemeValueOff);
                    dvSortSchemeValueoff.Sort = "SCHEMECODE ASC,MINIMUMQUANTITY DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortSchemeValueoff)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY >='" + drv.Row["MINIMUMQUANTITY"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMQUANTITY"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHBOX"]) >= Convert.ToDecimal(drv2["MINIMUMQUANTITY"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    viewSchemeValueoff = new DataView(dt1);
                    viewSchemeValueoff.RowFilter = "[Scheme Type]=3";
                    distinctTableSchemeValueOff = (DataTable)viewSchemeValueoff.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITYPCS");

                    dvSortSchemeValueoff = new DataView(distinctTableSchemeValueOff);
                    dvSortSchemeValueoff.Sort = "SCHEMECODE ASC,MINIMUMQUANTITYPCS DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortSchemeValueoff)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS >='" + drv.Row["MINIMUMQUANTITYPCS"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]) > 0)
                                {

                                    if (Convert.ToDecimal(drv2["SCHPCS"]) >= Convert.ToDecimal(drv2["MINIMUMQUANTITYPCS"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    #endregion
                    // Update Slab Qty For FreeQTYPCS
                    foreach (DataColumn col in dt3.Columns)
                    {
                        if (col.ColumnName == "FREEQTY")
                        {
                            col.ReadOnly = false;
                        }
                    }
                    for (int i = 0; i <= dt3.Rows.Count - 1; i++)
                    {
                        if (Convert.ToDecimal(dt3.Rows[i]["FREEQTYPCS"].ToString()) > 0)
                        {
                            dt3.Rows[i]["FREEQTY"] = Convert.ToInt32(dt3.Rows[i]["FREEQTYPCS"]);
                        }
                    }
                    dt3.AcceptChanges();
                    DataView dv = dt3.DefaultView;
                    dv.Sort = "SCHEMECODE ASC,SetNo ASC, FREEQTY ASC";
                    DataTable sortedDT = dv.ToTable();
                    return sortedDT;
                    #endregion
                }
                else
                {
                    return null;
                }


                #region Comment

                /*

                DataView dv = null;
                DataTable dt2 = new DataTable();
                dt2 = dt1.Clone();
                


               
                var groupedData = from r in dt1.AsEnumerable()
                                  where r.Field<int>("Scheme Type").Equals(0)
                                  group r by new
                                  {
                                      FreeItemCode = r.Field<string>("Free Item Code"),
                                      
                                  } into g
                                  select new
                                  {
                                      g.Key.FreeItemCode,
                                      Max = g.Max(r => r.Field<decimal>("MINIMUMQUANTITY"))

                                  };

                foreach (var grp in groupedData)
                {
                    dv = new DataView(dt1);
                    dv.RowFilter = "[Free Item Code]='" + grp.FreeItemCode + "' and MINIMUMQUANTITY=" + grp.Max + "";
                    foreach (DataRowView drv in dv)
                    {
                        DataRow dr = drv.Row;
                        dt2.ImportRow(dr);
                    }

                }

                 groupedData = from r in dt1.AsEnumerable()
                                  where r.Field<int>("Scheme Type").Equals(1)
                                  group r by new
                                  {
                                      FreeItemCode = r.Field<string>("Free Item Code"),
                    
                                  } into g
                                  select new
                                  {
                                      g.Key.FreeItemCode,
                                      Max = g.Max(r => r.Field<decimal>("MINIMUMVALUE"))

                                  };

                foreach (var grp in groupedData)
                {
                    dv = new DataView(dt1);
                    dv.RowFilter = "[Free Item Code]='" + grp.FreeItemCode + "' and MINIMUMVALUE=" + grp.Max + "";
                    
                    foreach (DataRowView drv in dv)
                    {
                        DataRow dr = drv.Row;
                        dt2.ImportRow(dr);
                    }

                }
                */
                #endregion


            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
            finally
            {

                conn.Close();
                conn.Dispose();
            }
        }

        public DataTable GetDatafromSPDiscount(string SPName, string strItem)
        {
            DataTable dt = new DataTable();
            conn = baseObj.GetConnection();
            try
            {
                string ItemGroup = string.Empty;
                string ItemCode = string.Empty;

                string strSite = Session["SiteCode"].ToString();
                string strLocation = Session["SCHSTATE"].ToString();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPName;
                cmd.Parameters.AddWithValue("@PLACESITE", strSite);
                cmd.Parameters.AddWithValue("@PLACESTATE", strLocation);
                cmd.Parameters.AddWithValue("@PLACEALL", "");
                cmd.Parameters.AddWithValue("@CUSCODECUS", ddlCustomer.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CUSCODEGROUP", ddlCustomerGroup.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CUSCODEALL", "");
                cmd.Parameters.AddWithValue("@ITEMITEM", strItem);
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                //cmd.Parameters.AddWithValue("@ITEMGROUP",DDLProductGroup.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@ITEMALL", "");

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
            }
            return dt;
        }

        public Int16 GetQtyofGroupItem(string Group, string BoxPCS)
        {
            DataTable dt;
            if (Group.Trim().Length == 0 || Group.Trim().ToUpper() == "ALL")
            {
                dt = baseObj.GetData("Select  DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "'");
            }
            else
            {
                dt = baseObj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
            }
            Int16 Qty = 0;

            //foreach (DataRow dtdr in dt.Rows)
            //{
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                DataRow[] dr = dt.Select("ItemId='" + gvrow.Cells[3].Text + "'");
                if (dr.Length > 0)
                //if (dtdr[0].ToString() == gvrow.Cells[3].Text)
                {
                    if (BoxPCS == "BOX")
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(gvrow.Cells[7].Text));
                    }
                    else
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(gvrow.Cells[8].Text));
                    }
                }
            }
            //}
            return Qty;
        }

        public decimal GetValueofGroupItem(string Group)
        {
            DataTable dt;
            if (Group.Trim().Length == 0 || Group.Trim().ToUpper() == "ALL")
            {
                dt = baseObj.GetData("Select ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "'");
            }
            else
            {
                dt = baseObj.GetData("Select ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
            }
            decimal Value = 0;
            //foreach (DataRow dtdr in dt.Rows)
            //{

            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                DataRow[] dr = dt.Select("ItemId='" + gvrow.Cells[3].Text + "'");
                if (dr.Length > 0)
                //if (dtdr[0].ToString() == gvrow.Cells[3].Text)
                {

                    Value += Convert.ToDecimal(gvrow.Cells[21].Text);
                }
            }
            //}
            return Value;
        }

        public Int16 GetQtyofItem(string Item, string BoxPCS)
        {
            Int16 Qty = 0;
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                if (gvrow.Cells[3].Text == Item)
                {
                    if (BoxPCS == "BOX")
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(gvrow.Cells[7].Text));
                    }
                    else
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(gvrow.Cells[8].Text));
                    }
                }
            }
            return Qty;
        }

        public decimal GetValueofItem(string Item)
        {
            decimal Value = 0;
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                if (gvrow.Cells[3].Text == Item)
                {
                    Value = Convert.ToDecimal(gvrow.Cells[21].Text);
                }
            }
            return Value;
        }

        public Int16 GetMaxQtyofItem(string BoxPCS)
        {
            Int16 Qty = 0;
            Int16[] arrQty = new Int16[gvDetails.Rows.Count];
            if (gvDetails.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    if (BoxPCS == "BOX")
                    {
                        arrQty[gvrow.RowIndex] = Convert.ToInt16(Convert.ToDecimal(gvrow.Cells[7].Text));
                    }
                    else
                    {
                        arrQty[gvrow.RowIndex] = Convert.ToInt16(Convert.ToDecimal(gvrow.Cells[8].Text));
                    }
                }
                Qty = arrQty.Max();
            }
            return Qty;
        }

        public decimal GetMaxValueofItem()
        {
            decimal Value = 0;
            decimal[] arrValue = new decimal[gvDetails.Rows.Count];
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                arrValue[gvrow.RowIndex] = Convert.ToDecimal(gvrow.Cells[21].Text);
            }
            if (arrValue.Length > 0)
            {
                Value = arrValue.Max();
            }
            return Value;
        }

        //private bool GetPrevSelection(string SchemeCode,string SetNo, ref GridView gv)
        //{
        //    foreach (GridViewRow rw in gv.Rows)
        //    {
        //        CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
        //        if (chkBx.Checked && rw.Cells[1].Text != SchemeCode )
        //        {
        //            return true;
        //        }
        //        if (chkBx.Checked && rw.Cells[1].Text == SchemeCode && rw.Cells[7].Text != SetNo)
        //        {                    
        //            return true; 
        //        }
        //    }
        //    return false;
        //}

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            GridViewRow row1 = (GridViewRow)activeCheckBox.Parent.Parent;
            String SchemeCode1 = row1.Cells[1].Text;
            String SetNo1 = row1.Cells[7].Text;
            TextBox txtQty = (TextBox)row1.FindControl("txtQty");
            TextBox txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
            HiddenField hdnSchType = (HiddenField)row1.FindControl("hdnSchemeType");
            HiddenField hdnSchSrlNo = (HiddenField)row1.FindControl("hdnSchSrlNo");
            if (App_Code.Global.GetPrevSelection(SchemeCode1, SetNo1, ref gvScheme, hdnSchSrlNo.Value.ToString())) { activeCheckBox.Checked = false; return; }

            if (activeCheckBox.Checked)
            {
                #region For Selection
                GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
                string SchemeCode = row.Cells[1].Text;
                string SetNo = row.Cells[7].Text;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    #region Same Scheme Validation 1st Check
                    //CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    //if (chkBx.Checked)
                    //{
                    //    if (rw.Cells[1].Text != SchemeCode)
                    //    {
                    //        activeCheckBox.Checked = false;

                    //        string message = "alert('You can select only one scheme items !');";
                    //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    //       return;
                    //    }
                    //    else if(rw.Cells[1].Text == SchemeCode && SetNo != rw.Cells[7].Text)
                    //    {
                    //        activeCheckBox.Checked = false;

                    //        string message = "alert('You can select only one scheme items with same set!');";
                    //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    //        return;
                    //    }
                    //}
                    #endregion

                    #region 2nd Check Same scheme and same set setno > 0 Auto select all scheme
                    CheckBox chkBxNew = (CheckBox)rw.FindControl("chkSelect");  //Auto Select All Same Set No
                    if (Convert.ToString(rw.Cells[7].Text) == SetNo && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(SetNo) > 0)
                    {
                        chkBxNew.Checked = true;
                        txtQty = (TextBox)rw.FindControl("txtQty");
                        txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                        HiddenField hdnTotalFreeQty = (HiddenField)rw.FindControl("hdnTotalFreeQty");
                        HiddenField hdnTotalFreeQtyPcs = (HiddenField)rw.FindControl("hdnTotalFreeQtyPcs");
                        txtQty.Text = hdnTotalFreeQty.Value;
                        txtQtyPcs.Text = hdnTotalFreeQtyPcs.Value;
                        txtQty.Enabled = false;
                        txtQtyPcs.Enabled = false;
                    }
                    #endregion
                }

                #region FreeBox Readonly validation
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    HiddenField hdnSchemeType = (HiddenField)rw.FindControl("hdnSchemetype");
                    txtQty = (TextBox)rw.FindControl("txtQty");
                    if (chkBx.Checked)
                    {
                        txtQty.ReadOnly = false;
                    }
                    else
                    {
                        txtQty.Text = string.Empty;
                        txtQty.ReadOnly = true;
                    }
                    if (hdnSchemeType.Value.ToString() == "2" || hdnSchemeType.Value.ToString() == "3")
                    {
                        txtQty.ReadOnly = true;
                        txtQtyPcs.ReadOnly = true;
                    }
                }
                #endregion
                #endregion
                #region  checkbox enable for set no 0 only
                if (SetNo1 == "0")
                {
                    if ((hdnSchType.Value.ToString() == "2" || hdnSchType.Value.ToString() == "3"))
                    {

                    }
                    else
                    {
                        txtQty = (TextBox)row1.FindControl("txtQty");
                        txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
                        HiddenField hdnTotalFreeQty = (HiddenField)row1.FindControl("hdnTotalFreeQty");
                        HiddenField hdnTotalFreeQtyPcs = (HiddenField)row1.FindControl("hdnTotalFreeQtyPcs");

                        if (hdnTotalFreeQty.Value != "")
                        {
                            txtQty.ReadOnly = false;
                        }
                        else
                        {
                            txtQty.ReadOnly = true;
                        }
                        if (Session["ApplicableOnState"].ToString() == "Y")
                        {
                            if (hdnTotalFreeQtyPcs.Value != "")
                            {
                                txtQtyPcs.ReadOnly = false;
                            }
                            else
                            {
                                txtQtyPcs.ReadOnly = true;
                            }
                        }
                        else { txtQtyPcs.ReadOnly = true; }
                    }
                }
                #endregion

            }
            else
            {
                #region  For unchecked
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
                    string SchemeCode = row.Cells[1].Text;
                    string SetNo = row.Cells[7].Text;
                    if (SchemeCode == SchemeCode1 && SetNo == SetNo1 && Convert.ToInt16(SetNo) > 0)
                    {
                        CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                        chkBx.Checked = false;
                        txtQty = (TextBox)rw.FindControl("txtQty");
                        txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                        HiddenField hdnTotalFreeQty = (HiddenField)rw.FindControl("hdnTotalFreeQty");
                        HiddenField hdnTotalFreeQtyPcs = (HiddenField)rw.FindControl("hdnTotalFreeQtyPcs");
                        txtQty.Text = "";
                        txtQtyPcs.Text = "";
                        txtQtyPcs.ReadOnly = true;
                        txtQty.ReadOnly = true;
                    }
                }
                if (SetNo1 == "0" && (hdnSchType.Value.ToString() == "2" || hdnSchType.Value.ToString() == "3"))
                {
                    txtQty = (TextBox)row1.FindControl("txtQty");
                    txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPcs.Text = "";
                }
                #endregion
            }
            if (activeCheckBox.Checked && (hdnSchType.Value.ToString() == "0" || hdnSchType.Value.ToString() == "1"))
            {
                if (Convert.ToInt32(SetNo1) > 0)
                {
                    txtQty.Enabled = false;
                    txtQtyPcs.Enabled = false;
                }
                else
                {
                    txtQty.Enabled = true;
                    txtQtyPcs.Enabled = true;
                }
            }
            else
            {
                txtQty.Enabled = false;
                txtQtyPcs.Enabled = false;
                txtQty.Text = "0";
                txtQtyPcs.Text = "0";
            }

            SchemeDiscCalculation();


        }


        protected decimal SchemeLineCalculation(decimal discPer)
        {
            decimal Rate = 0, Qty = 0, BasicAmount = 0, packSize = 0;
            decimal TotalBasicAmount = 0;
            decimal TaxPer = 0;
            decimal DiscAmt = 0;
            decimal AddTaxPer = 0;
            decimal TaxAmount = 0;
            decimal AddTaxAmount = 0;
            try
            {


                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = (txtQty.Text.Trim().Length == 0 ? "0" : txtQty.Text);
                    txtQtyPCS.Text = (txtQtyPCS.Text.Trim().Length == 0 ? "0" : txtQtyPCS.Text);
                    packSize = (rw.Cells[14].Text == "" ? 0 : Convert.ToDecimal(rw.Cells[14].Text));
                    if (packSize == 0)
                    {
                        packSize = 1;
                    }
                    Qty = Convert.ToDecimal(txtQty.Text) + (Convert.ToDecimal(txtQtyPCS.Text) > 0 ? Convert.ToDecimal(txtQtyPCS.Text) / packSize : 0);
                    Rate = rw.Cells[16].Text == "" ? 0 : Convert.ToDecimal(rw.Cells[16].Text);
                    rw.Cells[15].Text = Convert.ToString(Qty);
                    rw.Cells[17].Text = Convert.ToString(Rate * Qty);
                    BasicAmount = (Rate * Qty);
                    rw.Cells[17].Text = Convert.ToString(Rate * Qty);
                    rw.Cells[18].Text = (discPer * 100).ToString("0.00");
                    DiscAmt = BasicAmount * discPer;
                    rw.Cells[19].Text = DiscAmt.ToString("0.00");
                    rw.Cells[20].Text = (BasicAmount - DiscAmt).ToString("0.00");
                    if (rw.Cells[21].Text.Trim().Length == 0)
                    {
                        TaxPer = 0;
                    }
                    else
                    {
                        TaxPer = Convert.ToDecimal(rw.Cells[21].Text);
                    }
                    if (rw.Cells[23].Text.Trim().Length == 0)
                    {
                        AddTaxPer = 0;
                    }
                    else
                    {
                        AddTaxPer = Convert.ToDecimal(rw.Cells[23].Text);
                    }
                    TaxAmount = Math.Round(((BasicAmount - DiscAmt) * TaxPer) / 100, 2);
                    AddTaxAmount = Math.Round(((BasicAmount - DiscAmt) * AddTaxPer) / 100, 2);
                    rw.Cells[22].Text = TaxAmount.ToString("0.00");
                    rw.Cells[24].Text = AddTaxAmount.ToString("0.00");
                    rw.Cells[25].Text = (BasicAmount + TaxAmount + AddTaxAmount - DiscAmt).ToString("0.00");
                    TotalBasicAmount = TotalBasicAmount + BasicAmount;
                }
                return TotalBasicAmount;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.Replace("'", "''") + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return TotalBasicAmount;
            }
        }

        protected void SchemeDiscCalculation()
        {
            try
            {
                /*@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    Don't change rounding places of any field {decimal number}
                  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ */

                decimal TotalSchemeBasicAmount = 0;
                decimal TotalBasicAmount = 0;
                DataTable dtLineItems;
                decimal DiscPer = 0;

                TotalSchemeBasicAmount = this.SchemeLineCalculation(0);
                dtLineItems = null;
                dtLineItems = (DataTable)Session["LineItem"];
                TotalBasicAmount = dtLineItems.AsEnumerable().Sum(row => row.Field<decimal>("QtyBox") * row.Field<decimal>("Price"));
                TotalBasicAmount = TotalBasicAmount + TotalSchemeBasicAmount;
                if (TotalBasicAmount > 0)
                {
                    DiscPer = Math.Round(TotalSchemeBasicAmount / TotalBasicAmount, 6);
                }
                this.SchemeLineCalculation(DiscPer);

                //#region Update Grid
                //foreach (GridViewRow rw in gvDetails.Rows)
                decimal lineAmount = 0;
                decimal DiscAmount = 0;
                decimal SchemeDiscAmount = 0;
                decimal TaxableAmount = 0;
                decimal TaxPer = 0;
                decimal TaxAmount = 0;
                decimal AddTaxPer = 0;
                decimal AddTaxAmount = 0;
                decimal SecDiscAmount = 0;
                decimal TDAmount = 0;
                decimal PEAmount = 0;
                decimal SchemeDiscPer = 0;
                decimal AddSchemeDiscount = 0;
                decimal AddSchemePerc = 0;
                decimal AddSchemeValue = 0;
                dtLineItems.Columns["Tax_Amount"].ReadOnly = false;
                dtLineItems.Columns["AddTax_Amount"].ReadOnly = false;
                dtLineItems.Columns["TaxableAmount"].ReadOnly = false;
                dtLineItems.Columns["Value"].ReadOnly = false;
                dtLineItems.Columns["ADDSCHDISCPER"].ReadOnly = false;
                dtLineItems.Columns["ADDSCHDISCVAL"].ReadOnly = false;
                dtLineItems.Columns["ADDSCHDISCAMT"].ReadOnly = false;
                dtLineItems.Columns["SEC_DISC_AMOUNT"].ReadOnly = false;
                dtLineItems.Columns["TDValue"].ReadOnly = false;

                Boolean SchemeDiscApply = false, AddSchemeDiscApply = false;
                string SchSrlNo = "0", AddSchemeGroup = "", AddSchSrlNo = "0";
                SchemeDiscApply = AddSchemeDiscApply = false;

                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    HiddenField hdnSchemetype = (HiddenField)rw.FindControl("hdnSchemetype");
                    HiddenField hdnSchSrlNo = (HiddenField)rw.FindControl("hdnSchSrlNo");
                    Label lblSchemeDiscPer = (Label)rw.FindControl("lblSchemeDiscPer");
                    HiddenField hdnAddSchType = (HiddenField)rw.FindControl("hdnAddSchType");
                    Label lblAddSchemePer = (Label)rw.FindControl("lblAddSchemePer");
                    Label lblAddSchemeVal = (Label)rw.FindControl("lblAddSchemeVal");
                    Label lblAddSchemeGroup = (Label)rw.FindControl("lblAddSchemeGroup");
                    HiddenField hdntotSchemeValueoff = (HiddenField)rw.FindControl("hdntotSchemeValueoff");
                    if (chkBx.Checked && hdnAddSchType.Value != "0")
                    {
                        AddSchemeDiscApply = true;
                        AddSchemePerc = Convert.ToDecimal(lblAddSchemePer.Text.Trim() == "" ? 0 : Convert.ToDecimal(lblAddSchemePer.Text));
                        AddSchemeValue = Convert.ToDecimal(lblAddSchemeVal.Text.Trim() == "" ? 0 : Convert.ToDecimal(lblAddSchemeVal.Text));
                        AddSchemeGroup = lblAddSchemeGroup.Text;
                        AddSchSrlNo = hdnSchSrlNo.Value.ToString();
                    }
                    if (chkBx.Checked && hdnSchemetype.Value.ToString() == "2")
                    {
                        SchemeDiscApply = true;
                        DiscPer = lblSchemeDiscPer.Text.Trim() == "" ? 0 : Convert.ToDecimal(lblSchemeDiscPer.Text);
                        DiscPer = DiscPer / 100;
                        SchSrlNo = hdnSchSrlNo.Value.ToString();
                        break;
                    }
                    if (chkBx.Checked && hdnSchemetype.Value.ToString() == "3")
                    {
                        SchemeDiscApply = true;
                        #region Scheme Basic Value Check
                        DataTable dtSchValue = new DataTable();
                        if (Session["SessionScheme"] != null)
                        { dtSchValue = (DataTable)Session["SessionScheme"]; }
                        DiscPer = 0;
                        if (dtSchValue.Rows.Count > 0)
                        {
                            SchSrlNo = hdnSchSrlNo.Value.ToString();
                            DataRow[] dr = dtSchValue.Select("SrNo=" + SchSrlNo);
                            DataTable dt = new DataTable();
                            string[] ItemArray = new string[1];
                            if (dr.Length > 0)
                            {
                                #region Get Scheme Free Product List
                                if (dr[0]["Scheme Item Type"].ToString().ToUpper() == "ALL" && (dr[0]["Scheme Item Group"].ToString().ToUpper() == "ALL" || dr[0]["Scheme Item Group"].ToString().ToUpper() == ""))
                                {
                                    dt = baseObj.GetData("Select  DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "'");
                                }
                                else if (dr[0]["Scheme Item Type"].ToString().ToUpper() == "GROUP")
                                {
                                    dt = baseObj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + dr[0]["Scheme Item Group"].ToString().ToUpper() + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
                                }
                                else
                                {
                                    dt = baseObj.GetData("Select  ITEMID From AX.INVENTTABLE Where ITEMID='" + dr[0]["Scheme Item Group"].ToString().ToUpper() + "'");
                                }
                                #endregion
                                #region Get Basic Value
                                if (dt.Rows.Count > 0)
                                {
                                    lineAmount = 0;
                                    for (int i = 0; i < dtLineItems.Rows.Count; i++)
                                    {
                                        DataRow[] drItem = dt.Select("ItemId='" + dtLineItems.Rows[i]["ProductCodeName"].ToString() + "'");
                                        if (drItem.Length > 0)
                                        {
                                            lineAmount += Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Price"].ToString());
                                        }
                                    }
                                }
                                #endregion
                                DiscPer = Convert.ToDecimal(hdntotSchemeValueoff.Value.Trim() == "" ? 0 : Convert.ToDecimal(hdntotSchemeValueoff.Value)) / lineAmount;
                            }

                            break;
                        }
                        #endregion
                    }
                }
                if (AddSchemeDiscApply == true && (AddSchemePerc > 0 || AddSchemeValue > 0))
                {
                    #region Apply Additional Scheme of Percent or Value Off
                    DataTable dtAddSch = new DataTable();
                    if (Session["SessionScheme"] != null)
                    { dtAddSch = (DataTable)Session["SessionScheme"]; }

                    if (dtAddSch.Rows.Count > 0)
                    {
                        DataRow[] dr = dtAddSch.Select("SrNo=" + AddSchSrlNo);
                        DataTable dt = new DataTable();
                        string[] ItemArray = new string[1];
                        if (dr.Length > 0)
                        {
                            if (dr[0]["ADDITIONDISCOUNTITEMTYPE"].ToString().ToUpper() == "2")
                            {
                                dt = baseObj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + dr[0]["ADDITIONDISCOUNTITEMGROUP"].ToString().ToUpper() + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
                            }
                            else
                            {
                                dt = baseObj.GetData("Select  ITEMID From AX.INVENTTABLE Where ITEMID='" + dr[0]["ADDITIONDISCOUNTITEMGROUP"].ToString().ToUpper() + "'");
                            }

                            if (dt.Rows.Count > 0)
                            {

                                for (int i = 0; i < dtLineItems.Rows.Count; i++)
                                {
                                    #region Update Additional Scheme Discount
                                    DataRow[] drItem = dt.Select("ItemId='" + dtLineItems.Rows[i]["ProductCodeName"].ToString() + "'");
                                    if (drItem.Length > 0)
                                    {
                                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Price"].ToString());
                                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                                        SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SEC_DISC_AMOUNT"].ToString());
                                        dtLineItems.Rows[i]["ADDSCHDISCPER"] = AddSchemePerc;
                                        dtLineItems.Rows[i]["ADDSCHDISCVAL"] = AddSchemeValue;
                                        TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TDValue"].ToString());
                                        PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());
                                        SchemeDiscPer = Convert.ToDecimal(dtLineItems.Rows[i]["SchemeDisc"].ToString());
                                        SchemeDiscAmount = Math.Round((lineAmount * SchemeDiscPer), 6);
                                        if (AddSchemePerc > 0)
                                        { dtLineItems.Rows[i]["ADDSCHDISCAMT"] = Math.Round((lineAmount * (AddSchemePerc / 100)), 6); }
                                        if (AddSchemeValue > 0)
                                        {
                                            dtLineItems.Rows[i]["ADDSCHDISCAMT"] = Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * AddSchemeValue;
                                        }
                                        //dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");

                                        dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString()) - Convert.ToDecimal(dtLineItems.Rows[i]["SchemeDiscVal"]) - SecDiscAmount - TDAmount + PEAmount - Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"])).ToString("0.00");

                                        if (dtLineItems.Rows[i]["Tax_Code"].ToString().Trim().Length == 0)
                                        {
                                            TaxPer = 0;
                                        }
                                        else
                                        {
                                            TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["Tax_Code"]);
                                        }
                                        if (dtLineItems.Rows[i]["AddTax_Code"].ToString().Trim().Length == 0)
                                        {
                                            AddTaxPer = 0;
                                        }
                                        else
                                        {
                                            AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTax_Code"]);
                                        }

                                        TaxableAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TaxableAmount"].ToString());
                                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                                        dtLineItems.Rows[i]["Tax_Amount"] = TaxAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["AddTax_Amount"] = AddTaxAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["Value"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                                    }
                                    #endregion
                                }
                                dtLineItems.AcceptChanges();
                                Session["LineItem"] = dtLineItems;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Remove Additional Scheme of Percent or Value off
                    for (int i = 0; i < dtLineItems.Rows.Count; i++)
                    {
                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Price"].ToString());
                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                        SchemeDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SchemeDiscVal"].ToString());
                        AddSchemeDiscount = 0;
                        SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SEC_DISC_AMOUNT"].ToString());
                        TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TDValue"].ToString());
                        PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());
                        dtLineItems.Rows[i]["ADDSCHDISCPER"] = AddSchemePerc;
                        dtLineItems.Rows[i]["ADDSCHDISCVAL"] = AddSchemeValue;
                        dtLineItems.Rows[i]["ADDSCHDISCAMT"] = AddSchemeDiscount;
                        dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - AddSchemeDiscount).ToString("0.000000");

                        if (dtLineItems.Rows[i]["Tax_Code"].ToString().Trim().Length == 0)
                        {
                            TaxPer = 0;
                        }
                        else
                        {
                            TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["Tax_Code"]);
                        }
                        if (dtLineItems.Rows[i]["AddTax_Code"].ToString().Trim().Length == 0)
                        {
                            AddTaxPer = 0;
                        }
                        else
                        {
                            AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTax_Code"]);
                        }
                        TaxableAmount = lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - AddSchemeDiscount;
                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                        dtLineItems.Rows[i]["Tax_Amount"] = TaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["AddTax_Amount"] = AddTaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["Value"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                    }
                    dtLineItems.AcceptChanges();
                    Session["LineItem"] = dtLineItems;
                    #endregion
                }
                if (SchemeDiscApply == true && DiscPer > 0)
                {
                    #region Apply Scheme of Percent or Value Off
                    DataTable dtSch = new DataTable();
                    if (Session["SessionScheme"] != null)
                    { dtSch = (DataTable)Session["SessionScheme"]; }

                    if (dtSch.Rows.Count > 0)
                    {
                        DataRow[] dr = dtSch.Select("SrNo=" + SchSrlNo);
                        DataTable dt = new DataTable();
                        string[] ItemArray = new string[1];
                        if (dr.Length > 0)
                        {
                            if (dr[0]["Scheme Item Type"].ToString().ToUpper() == "ALL" && (dr[0]["Scheme Item Group"].ToString().ToUpper() == "ALL" || dr[0]["Scheme Item Group"].ToString().ToUpper() == ""))
                            {
                                dt = baseObj.GetData("Select  DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "'");
                            }
                            else if (dr[0]["Scheme Item Type"].ToString().ToUpper() == "GROUP")
                            {
                                dt = baseObj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + dr[0]["Scheme Item Group"].ToString().ToUpper() + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
                            }
                            else
                            {
                                dt = baseObj.GetData("Select  ITEMID From AX.INVENTTABLE Where ITEMID='" + dr[0]["Scheme Item Group"].ToString().ToUpper() + "'");
                            }

                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtLineItems.Rows.Count; i++)
                                {
                                    DataRow[] drItem = dt.Select("ItemId='" + dtLineItems.Rows[i]["ProductCodeName"].ToString() + "'");
                                    if (drItem.Length > 0)
                                    {
                                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Price"].ToString());
                                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                                        SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SEC_DISC_AMOUNT"].ToString());
                                        TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TDValue"].ToString());
                                        PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());
                                        dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                                        SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"])).ToString("0.00");
                                        if (dtLineItems.Rows[i]["Tax_Code"].ToString().Trim().Length == 0)
                                        {
                                            TaxPer = 0;
                                        }
                                        else
                                        {
                                            TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["Tax_Code"]);
                                        }
                                        if (dtLineItems.Rows[i]["AddTax_Code"].ToString().Trim().Length == 0)
                                        {
                                            AddTaxPer = 0;
                                        }
                                        else
                                        {
                                            AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTax_Code"]);
                                        }
                                        TaxableAmount = lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"]);
                                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                                        dtLineItems.Rows[i]["Tax_Amount"] = TaxAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["AddTax_Amount"] = AddTaxAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["Value"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                                    }
                                }
                                dtLineItems.AcceptChanges();
                                Session["LineItem"] = dtLineItems;
                                BindingGird();
                                //gvDetails.DataSource = dtLineItems;
                                //gvDetails.DataBind();
                                //GridViewFooterCalculate(dtLineItems);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    for (int i = 0; i < dtLineItems.Rows.Count; i++)
                    {
                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Price"].ToString());
                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                        SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SEC_DISC_AMOUNT"].ToString());
                        TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TDValue"].ToString());
                        PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());

                        dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                        SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                        AddSchemeDiscount = Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"]);
                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"])).ToString("0.00");
                        if (dtLineItems.Rows[i]["Tax_Code"].ToString().Trim().Length == 0)
                        {
                            TaxPer = 0;
                        }
                        else
                        {
                            TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["Tax_Code"]);
                        }
                        if (dtLineItems.Rows[i]["AddTax_Code"].ToString().Trim().Length == 0)
                        {
                            AddTaxPer = 0;
                        }
                        else
                        {
                            AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTax_Code"]);
                        }
                        TaxableAmount = lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - AddSchemeDiscount;
                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                        dtLineItems.Rows[i]["Tax_Amount"] = TaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["AddTax_Amount"] = AddTaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["Value"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                    }
                    dtLineItems.AcceptChanges();
                    Session["LineItem"] = dtLineItems;
                    BindingGird();
                    //gvDetails.DataSource = dtLineItems;
                    //gvDetails.DataBind();
                    //GridViewFooterCalculate(dtLineItems);
                }
                //#endregion
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.Replace("'", "''") + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
            }
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            int TotalQty = 0;
            GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
            string SchemeCode = row.Cells[1].Text;
            int AvlFreeQty = Convert.ToInt16(row.Cells[8].Text);
            int Slab = Convert.ToInt16(row.Cells[6].Text);
            CheckBox chkBx1 = (CheckBox)row.FindControl("chkSelect");
            TextBox txtQty1 = (TextBox)row.FindControl("txtQty");

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                if (chkBx.Checked == true)
                {
                    //For Box

                    if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab)
                    {
                        TotalQty += Convert.ToInt16(txtQty.Text);
                        if (getBoxPcsPicQty(SchemeCode, Slab, "P") > 0)
                        {
                            txtQty1.Text = "0";
                            chkBx1.Checked = false;
                            txtQty1.ReadOnly = false;
                            this.SchemeDiscCalculation();
                            string message = "alert('Free Qty should not greater than available free qty !');";
                            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                            return;
                        }
                    }

                    if (TotalQty > AvlFreeQty)
                    {
                        txtQty1.Text = "0";
                        chkBx1.Checked = false;
                        txtQty1.ReadOnly = false;
                        this.SchemeDiscCalculation();
                        string message = "alert('Free Qty Box should not greater than available free qty box!');";
                        ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                        return;
                    }
                }
                else
                {
                    txtQty.Text = "0";
                }
            }
            this.SchemeDiscCalculation();
        }

        protected Int32 getBoxPcsPicQty(string SchemeCode, int slab, string BoxPcs)
        {
            Int16 TotalQty = 0;
            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                if (txtQty.Text.Trim().Length == 0)
                {
                    txtQty.Text = "0";
                }
                if (txtQtyPcs.Text.Trim().Length == 0)
                {
                    txtQtyPcs.Text = "0";
                }
                if (chkBx.Checked == true)
                {
                    if (rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[7].Text) == 0) //Convert.ToInt16(rw.Cells[6].Text) == slab &&
                    {
                        if (BoxPcs == "B")
                        {
                            TotalQty += Convert.ToInt16(txtQty.Text);
                        }
                        else
                        {
                            TotalQty += Convert.ToInt16(txtQtyPcs.Text);
                        }
                    }
                }
            }
            return TotalQty;
        }

        protected void txtQtyPcs_TextChanged(object sender, EventArgs e)
        {
            int TotalQtyPcs = 0;
            GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
            string SchemeCode = row.Cells[1].Text;
            int AvlFreeQtyPcs = Convert.ToInt16(row.Cells[11].Text);
            int Slab = Convert.ToInt16(row.Cells[6].Text);
            CheckBox chkBx1 = (CheckBox)row.FindControl("chkSelect");
            TextBox txtQtyPcs1 = (TextBox)row.FindControl("txtQtyPcs");

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                if (chkBx.Checked == true)
                {
                    //For Pcs                    
                    if (!string.IsNullOrEmpty(txtQtyPcs.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab)
                    {
                        TotalQtyPcs += Convert.ToInt16(txtQtyPcs.Text);
                        if (getBoxPcsPicQty(SchemeCode, Slab, "B") > 0)
                        {
                            txtQtyPcs1.Text = "0";
                            chkBx1.Checked = false;
                            txtQtyPcs1.ReadOnly = false;
                            this.SchemeDiscCalculation();
                            string message = "alert('Free Qty Pcs should not greater than available free qty pcs !');";
                            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                            return;
                        }
                    }
                    if (TotalQtyPcs > AvlFreeQtyPcs)
                    {
                        txtQtyPcs1.Text = "0";
                        chkBx1.Checked = false;
                        txtQtyPcs1.ReadOnly = false;
                        this.SchemeDiscCalculation();
                        string message = "alert('Free Qty Pcs should not greater than available free qty pcs !');";
                        ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                        return;
                    }
                }
                else
                {
                    txtQtyPcs.Text = "0";
                }
            }
            this.SchemeDiscCalculation();
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
                txtPcs.Text = "";
                txtViewTotalPcs.Text = "";
                txtViewTotalBox.Text = "";
                txtCrate.Text = "";
                ImDnldTemp.Visible = false;
                txtEnterQty.Text = string.Empty;
                txtQtyBox.Text = string.Empty;
                txtQtyCrates.Text = string.Empty;
                txtQtyCrates.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtValue.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtContactNo.Text = string.Empty;
                rdNonExempt.Enabled = true;
                rdExempt.Enabled = true;
                rdNonExempt.Checked = true;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gvScheme.DataSource = null;
                gvScheme.DataBind();
                lblMessage.Text = string.Empty;
                CalendarExtender1.StartDate = DateTime.Now;
                fillCustomerGroup();
                ddlCustomer.Items.Clear();
                ddlCustomer.Items.Add("Select...");
                ddlPSRName.Items.Clear();
                ddlPSRName.Items.Add("Select...");
                ddlBeatName.Items.Clear();
                ddlBeatName.Items.Add("Select...");
                ProductGroup();
                txtDeliveryDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
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
                LblMessage1.Text = "";
                txtPcs.Text = "";
                txtViewTotalPcs.Text = "";
                txtViewTotalBox.Text = "";
                txtCrate.Text = "";
                rdNonExempt.Enabled = true;
                rdExempt.Enabled = true;
                rdNonExempt.Checked = true;
                ImDnldTemp.Visible = true;
                txtEnterQty.Text = string.Empty;
                txtQtyBox.Text = string.Empty;
                txtQtyCrates.Text = string.Empty;
                txtQtyCrates.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtValue.Text = string.Empty;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gvScheme.DataSource = null;
                gvScheme.DataBind();
                lblMessage.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtContactNo.Text = string.Empty;
                CalendarExtender1.StartDate = DateTime.Now;
                fillCustomerGroup();
                ddlCustomer.Items.Clear();
                ddlCustomer.Items.Add("Select...");
                ddlPSRName.Items.Clear();
                ddlPSRName.Items.Add("Select...");
                ddlBeatName.Items.Clear();
                ddlBeatName.Items.Add("Select...");
                ProductGroup();
                txtDeliveryDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
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

        protected void rdoExcelEntry_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnUplaod_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidatePurchaseReturnHeaderData();
                if (b)
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
                        dtExcelData = CreamBell_DMS_WebApps.App_Code.ExcelUpload.ImportExcelXLSO(Server.MapPath("~/Uploads/" + fileName), true);

                        dtChangeZeroToNull(dtExcelData);

                        DataTable dtForShownUnuploadData = new DataTable();
                        dtForShownUnuploadData.Columns.Add("ProductCode");
                        dtForShownUnuploadData.Columns.Add("Qty");

                        int j = 0;
                        for (int i = 0; i < dtExcelData.Rows.Count; i++)
                        {
                            if (dtExcelData.Rows[i]["ProductCode"].ToString() == "0")
                            {
                                DataRow dr = dtExcelData.Rows[i];
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                //Delete the rows from datatabel 
                                dr.Delete();
                                j += 1;
                            }

                        }
                        dtExcelData.AcceptChanges();


                        //Create a new datatabel with valid datatypes 
                        DataTable Exceltable = new DataTable();
                        Exceltable.Columns.Add("ProductCode", typeof(string));
                        Exceltable.Columns.Add("Qty", typeof(Double));
                        Exceltable.Columns.Add("Crate", typeof(Double));
                        Exceltable.Columns.Add("Pcs", typeof(Double));

                        int p = 0;
                        foreach (DataRow row in dtExcelData.Rows)
                        {
                            //adding row in datatabel 
                            Exceltable.Rows.Add();
                            Exceltable.Rows[p]["ProductCode"] = dtExcelData.Rows[p]["ProductCode"].ToString();
                            Exceltable.Rows[p]["Qty"] = Convert.ToDouble(dtExcelData.Rows[p]["Qty"]);
                            Exceltable.Rows[p]["Crate"] = Convert.ToDouble(dtExcelData.Rows[p]["Crate"]);
                            Exceltable.Rows[p]["Pcs"] = Convert.ToDouble(dtExcelData.Rows[p]["Pcs"]);
                            p++;
                        }
                        //null the dtexcel tabel
                        dtExcelData = null;

                        //===============find duplicate value and merge it ====================
                        var result1 = from row in Exceltable.AsEnumerable()
                                      where row.Field<string>("ProductCode") != null
                                      select row;
                        //null the excelTable tabel
                        Exceltable = null;

                        var result = result1.AsEnumerable()
                                     .GroupBy(r => new
                                     {
                                         ProductCode = r.Field<String>("ProductCode"),
                                         // Qty = r.Field<Double>("Qty"),
                                     })

                                           .Select(g =>
                                           {
                                               var row = g.First();
                                               row.SetField("Qty", g.Sum(r => r.Field<Double>("Qty")));
                                               row.SetField("Crate", g.Sum(r => r.Field<Double>("Crate")));
                                               row.SetField("Pcs", g.Sum(r => r.Field<Double>("Pcs")));
                                               return row;
                                           })
                                            .CopyToDataTable();
                        ///after merging assign the value to dtExcelData
                        dtExcelData = result;
                        if (!baseObj.ExcelDataCheckForPcsApplicability(dtExcelData, Session["SCHSTATE"].ToString()))
                        {
                            string message = "alert('Error in Excel Data! PCS Billing Not Allowed');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            return;
                        }

                        //===============================================================================
                        string strCode = string.Empty;
                        CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                        conn = obj.GetConnection();

                        #region SO  Number Generate

                        string st = Session["SiteCode"].ToString();
                        if (st.Length < 6)
                        {
                            int len = st.Length;
                            int plen = 6 - len;
                            for (int k = 0; k < plen; k++)
                            {
                                st = "0" + st;
                            }
                        }
                        strCurrentN0 = DateTime.Now.ToString("yyMMddHHmmss");
                        PRESO_NO = st.Substring(st.Length - 6).ToString() + strCurrentN0;

                        string NUMSEQ = string.Empty;
                        NUMSEQ = "1";

                        string strPsrName = string.Empty;
                        string strBeatName = string.Empty;

                        if (ddlPSRName.Visible == true)
                        {
                            strPsrName = ddlPSRName.SelectedItem.Value;
                            strBeatName = ddlBeatName.SelectedItem.Value;
                        }

                        #endregion

                        #region Header Insert Data

                        conn = obj.GetConnection();
                        cmd = new SqlCommand("ACX_InsertSaleHeaderPre");
                        transaction = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@statusProcedure", "Insert");
                        cmd.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@RECID", "");
                        cmd.Parameters.AddWithValue("@SO_NO", PRESO_NO);
                        cmd.Parameters.AddWithValue("@PSR_CODE", strPsrName);
                        cmd.Parameters.AddWithValue("@DELIVERY_DATE", Convert.ToDateTime(txtDeliveryDate.Text).ToString("dd-MMM-yyyy"));
                        cmd.Parameters.AddWithValue("@SO_DATE", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                        cmd.Parameters.AddWithValue("@SO_VALUE", "5000");
                        cmd.Parameters.AddWithValue("@PSR_BEAT", strBeatName);
                        cmd.Parameters.AddWithValue("@CUST_REF_NO", "");
                        cmd.Parameters.AddWithValue("@REMARK", "");
                        cmd.Parameters.AddWithValue("@APP_SO_NO", "");
                        cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@APP_SO_DATE", "");
                        cmd.Parameters.AddWithValue("@SO_APPROVEDATE", "");
                        cmd.Parameters.AddWithValue("@STATUS", "0");
                        cmd.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                        /*   ---------- GST Implementation Start ----------- */
                        cmd.Parameters.AddWithValue("@DISTGSTINNO", Session["SITEGSTINNO"]);

                        if (Session["SITEGSTINREGDATE"] != null && Session["SITEGSTINREGDATE"].ToString().Trim().Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", Session["SITEGSTINREGDATE"]);
                        }
                        cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                        cmd.Parameters.AddWithValue("@CUSTGSTINNO", txtGSTtin.Text);
                        if (txtGSTtinRegistration.Text != null && txtGSTtinRegistration.Text.Trim().Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@CUSTGSTINREGDATE", txtGSTtinRegistration.Text);
                        }
                        cmd.Parameters.AddWithValue("@CUSTCOMPOSITIONSCHEME", (chkCompositionScheme.Checked == true ? 1 : 0));
                        cmd.Parameters.AddWithValue("@BILLTOADDRESS", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@SHIPTOADDRESS", ddlShipToAddress.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@BILLTOSTATE", txtBilltoState.Text);
                        /*   ---------- GST Implementation End  ----------- */
                        cmd.ExecuteNonQuery();

                        #endregion


                        #region Line Insert Data on Same PURCH Order Number

                        cmd1 = new SqlCommand("ACX_InsertSaleLinePreNew");
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 0;
                        cmd1.CommandType = CommandType.StoredProcedure;

                        #endregion

                        decimal TotalBoxQty;
                        string boxpcs = "0.00";
                        decimal boxQty = 0;
                        decimal pcsQty = 0;
                        string hsncode = "";
                        string queryRecidLine = "Select Count(RECID) as RECID from [ax].[ACXSALESLINEPRE]";
                        Int64 RecidLine = Convert.ToInt64(obj.GetScalarValue(queryRecidLine));
                        int no = 0;
                        for (int i = 0; i < dtExcelData.Rows.Count; i++)
                        {
                            string sqlstr = "SELECT ItemID,PRODUCT_CRATE_PACKSIZE,PRODUCT_PACKSIZE,HSNCODE,EXEMPT From ax.inventTable WHERE ItemID = '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "'  and block=0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID='" + Convert.ToString(Session["SiteCode"]) + "')";
                            DataTable dtobjcheckproductcode = obj.GetData(sqlstr);
                            if (dtobjcheckproductcode.Rows.Count <= 0)
                            {
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                j += 1;
                                continue;
                            }
                            else
                            {
                                //if(Session["Comm_Exem"] == null || Session["Comm_Exem"].ToString() == "")
                                //{
                                //    Session["Comm_Exem"] = dtobjcheckproductcode.Rows[0]["EXEMPT"].ToString();
                                //}
                                //
                                Session["Exempt_CurVal"] = dtobjcheckproductcode.Rows[0]["EXEMPT"].ToString();
                                //
                                //if(Session["Curr_Exem"].ToString() != Session["Comm_Exem"].ToString())
                                //{
                                //
                                //    string message = "alert('All products does not have have same Exempt value');";
                                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                                //    Session["Comm_Exem"] = null;
                                //    Session["Curr_Exem"] = null;
                                //    if (transaction != null)
                                //    {
                                //        transaction.Rollback();
                                //    }
                                //    LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : 0 Records.";
                                //    return;
                                //}
                                if (rdExempt.Checked == true)
                                {
                                    if (Session["Exempt_CurVal"].ToString() != "1")
                                    {
                                        //b = false;
                                        string message = "alert('All products does not have have same Exempt value');";
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                                        if (transaction != null)
                                        {
                                            transaction.Rollback();
                                        }
                                        LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : 0 Records.";
                                        return;
                                    }
                                }

                                if (rdNonExempt.Checked == true)
                                {
                                    if (Session["Exempt_CurVal"].ToString() != "0")
                                    {
                                        string message = "alert('All products does not have have same Exempt value');";
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                                        if (transaction != null)
                                        {
                                            transaction.Rollback();
                                        }
                                        LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : 0 Records.";
                                        return;
                                    }
                                }

                                hsncode = dtobjcheckproductcode.Rows[0]["HSNCODE"].ToString();
                            }

                            try
                            {
                                if (Convert.ToDecimal(dtobjcheckproductcode.Rows[0]["PRODUCT_PACKSIZE"].ToString()) == 0)
                                {
                                    dtForShownUnuploadData.Rows.Add();
                                    dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                    dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                    j += 1;
                                    continue;
                                }
                                else
                                {
                                    TotalBoxQty = (Convert.ToDecimal(dtExcelData.Rows[i]["Crate"]) * Convert.ToDecimal(dtobjcheckproductcode.Rows[0]["PRODUCT_CRATE_PACKSIZE"]))
                                        + Convert.ToDecimal(dtExcelData.Rows[i]["Qty"])
                                        + (Convert.ToDecimal(dtExcelData.Rows[i]["Pcs"]) / Convert.ToDecimal(dtobjcheckproductcode.Rows[0]["PRODUCT_PACKSIZE"].ToString()));
                                }

                                boxQty = Math.Truncate(TotalBoxQty);                          //Extract Only Box Qty From Total Qty
                                pcsQty = Math.Round((TotalBoxQty - boxQty) * Convert.ToDecimal(dtobjcheckproductcode.Rows[0]["PRODUCT_PACKSIZE"].ToString()));
                                boxpcs = Convert.ToString(boxQty) + '.' + (Convert.ToString(pcsQty).Length == 1 ? "0" : "") + Convert.ToString(pcsQty);
                                if (TotalBoxQty == 0)
                                {
                                    dtForShownUnuploadData.Rows.Add();
                                    dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                    dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                    j += 1;
                                    continue;
                                }
                            }
                            catch(Exception ex)
                            {
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                j += 1;
                                ErrorSignal.FromCurrentContext().Raise(ex);
                                continue;
                            }


                            cmd1.Parameters.Clear();

                            string[] ReturnArray = null;

                            //ReturnArray = obj.CalculatePrice1(dtExcelData.Rows[i]["ProductCode"].ToString(), ddlCustomer.SelectedItem.Value, decimal.Parse(dtExcelData.Rows[i]["Qty"].ToString()), ddlEntryType.SelectedItem.Value.ToString());
                            ReturnArray = obj.CalculatePrice1(dtExcelData.Rows[i]["ProductCode"].ToString(), ddlCustomer.SelectedItem.Value, TotalBoxQty, "Box", Session["SiteCode"].ToString());
                            if (ReturnArray != null && ReturnArray[2].ToString() == "")
                            {
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                j += 1;
                                continue;

                            }
                            else if (ReturnArray != null && ReturnArray[2].ToString() != "")
                            {
                                DataTable dt = new DataTable();
                                //dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalc '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + TotalBoxQty + "," + Convert.ToDecimal(ReturnArray[2].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                                if (txtAddress.Text.Trim().Length == 0)
                                {
                                    string message = "alert('Please select Customers Bill To Address!');";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                                    ddlCustomer.Focus();
                                    return;
                                }
                                if (txtBilltoState.Text.Trim().Length == 0)
                                {
                                    string message = "alert('Please select Customers Bill To State!');";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                                    ddlCustomer.Focus();
                                    return;
                                }
                                dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + TotalBoxQty + "," + Convert.ToDecimal(ReturnArray[2].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                                if (dt.Rows.Count > 0)
                                {
                                    cmd1.Parameters.AddWithValue("@statusProcedure", "Insert");
                                    cmd1.Parameters.AddWithValue("@SO_NO", PRESO_NO);
                                    cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                                    cmd1.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                                    cmd1.Parameters.AddWithValue("@PRODUCT_CODE", dtExcelData.Rows[i]["ProductCode"].ToString());
                                    cmd1.Parameters.AddWithValue("@BOX", TotalBoxQty);
                                    cmd1.Parameters.AddWithValue("@CRATES", ReturnArray[0].ToString());
                                    cmd1.Parameters.AddWithValue("@LTR", ReturnArray[1].ToString());
                                    cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(dt.Rows[0]["VALUE"]));
                                    cmd1.Parameters.AddWithValue("@BasePrice", ReturnArray[2].ToString());
                                    cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(dt.Rows[0]["Rate"]));

                                    cmd1.Parameters.AddWithValue("@BOXPcs", boxpcs);

                                    if (dt.Rows[0]["DISCTYPE"].ToString().ToUpper() == "PER")  // Percentage type
                                    {
                                        cmd1.Parameters.AddWithValue("@DiscType", "0");
                                    }
                                    else if (dt.Rows[0]["DISCTYPE"].ToString().ToUpper() == "VAL")  // Value Type
                                    {
                                        cmd1.Parameters.AddWithValue("@DiscType", "1");
                                    }
                                    else  // None
                                    {
                                        cmd1.Parameters.AddWithValue("@DiscType", "2");
                                    }
                                    cmd1.Parameters.AddWithValue("@Disc", Convert.ToDecimal(dt.Rows[0]["DISC"]));
                                    cmd1.Parameters.AddWithValue("@DiscVal", Convert.ToDecimal(dt.Rows[0]["DISCVAL"]));
                                    cmd1.Parameters.AddWithValue("@SECDISPER", Convert.ToDecimal(dt.Rows[0]["SECDISCPER"]));
                                    cmd1.Parameters.AddWithValue("@SECDISVAL", Convert.ToDecimal(dt.Rows[0]["SECDISCAMT"]));
                                    cmd1.Parameters.AddWithValue("@TDVAL", new decimal(0));
                                    cmd1.Parameters.AddWithValue("@TD_PER", new decimal(0));
                                    cmd1.Parameters.AddWithValue("@PE_VAL", new decimal(0));
                                    cmd1.Parameters.AddWithValue("@PE_PER", new decimal(0));

                                    //===========For Tax=========
                                    cmd1.Parameters.AddWithValue("@Tax_Code", Convert.ToDecimal(dt.Rows[0]["TAX_PER"]));
                                    cmd1.Parameters.AddWithValue("@Tax_Amount", Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]));
                                    cmd1.Parameters.AddWithValue("@AddTax_Code", Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]));
                                    cmd1.Parameters.AddWithValue("@AddTax_Amount", Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]));
                                    //New Field added for GST
                                    cmd1.Parameters.AddWithValue("@TaxComponent", dt.Rows[0]["TAX_CODE"]);
                                    cmd1.Parameters.AddWithValue("@AddTaxComponent", dt.Rows[0]["ADDTAX_CODE"]);
                                    cmd1.Parameters.AddWithValue("@CompositionScheme", (chkCompositionScheme.Checked == true ? 1 : 0));
                                    cmd1.Parameters.AddWithValue("@HSNCode", dt.Rows[0]["HSNCODE"]);

                                    //cmd1.Parameters.AddWithValue("@TaxComponent", dt.Rows[0]["TAX_CODE"]);
                                    //cmd1.Parameters.AddWithValue("@AddTaxComponent", dt.Rows[0]["ADDTAX_CODE"]);
                                    //cmd1.Parameters.AddWithValue("@CompositionScheme", (chkCompositionScheme.Checked == true ? 1 : 0));
                                    //cmd1.Parameters.AddWithValue("@HSNCode", dt.Rows[0]["HSNCODE"]);
                                    //cmd1.Parameters.AddWithValue("@TaxComponent", dt.Rows[0]["TAX_CODE"]);
                                    //cmd1.Parameters.AddWithValue("@AddTaxComponent", dt.Rows[0]["ADDTAX_CODE"]);
                                    //cmd1.Parameters.AddWithValue("@CompositionScheme", (chkCompositionScheme.Checked == true ? 1 : 0));
                                    //cmd1.Parameters.AddWithValue("@HSNCode", dt.Rows[0]["HSNCODE"]);

                                    int intCalculation = 0;

                                    if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "PRICE")
                                    {
                                        intCalculation = 0;
                                    }
                                    else if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "MRP")
                                    {
                                        intCalculation = 1;
                                    }
                                    else
                                    {
                                        intCalculation = 2;
                                    }
                                    cmd1.Parameters.AddWithValue("@CalculationBase", intCalculation);
                                    cmd1.Parameters.AddWithValue("@TaxableAmount", Convert.ToDecimal(dt.Rows[0]["TaxableAmount"]));
                                    //====================
                                    cmd1.Parameters.AddWithValue("@RECID", RecidLine + 1 + i);
                                    cmd1.Parameters.AddWithValue("@UOM", ReturnArray[4].ToString());
                                    cmd1.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                                    cmd1.ExecuteNonQuery();
                                    no += 1;
                                }
                            }
                        }
                        // cmd.ExecuteNonQuery();
                        transaction.Commit();
                        rdExempt.Enabled = false;
                        rdNonExempt.Enabled = false;
                        Session["PreSoNO"] = null;
                        Session["PreSoNO"] = PRESO_NO;
                        BindSODetails(PRESO_NO);

                        LblMessage1.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : " + no + " Records.";

                        if (dtForShownUnuploadData.Rows.Count > 0)
                        {
                            gridviewRecordNotExist.DataSource = dtForShownUnuploadData;
                            gridviewRecordNotExist.DataBind();
                            ModalPopupExtender1.Show();
                        }
                        else
                        {
                            gridviewRecordNotExist.DataSource = null;
                            gridviewRecordNotExist.DataBind();
                        }
                    }
                    else
                    {
                        LblMessage1.Text = "Please try again....!";
                    }
                }

            }
            catch (Exception ex)
            {
                LblMessage1.Text = ex.Message;
                LblMessage1.Visible = true;
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

        public void FillProductCode()
        {
            string strQuery = string.Empty;
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            string query = "USP_DropdownGetProductDesc";
            ilist.Add("@USERCODE"); litem.Add(Session["USERID"].ToString());
            if (ddlBusinessUnit.SelectedItem.Text == "All...")
            {
                ilist.Add("@BU_CODE"); litem.Add("");
            }
            else
            {
                ilist.Add("@BU_CODE"); litem.Add(ddlBusinessUnit.SelectedItem.Value.ToString());
            }
            if (rdStock.Checked == true)
            {
                ilist.Add("@IsStock"); litem.Add("1");
            }
            else
            {
                ilist.Add("@IsStock"); litem.Add("0");
            }
            if (rdExempt.Checked == true)
            {
                ilist.Add("@EXEMPT"); litem.Add("1");
            }
            else
            {
                ilist.Add("@EXEMPT"); litem.Add("0");
            }
            ilist.Add("@Block"); litem.Add("1");
            ilist.Add("@ProductGroup"); litem.Add(DDLProductGroup.SelectedValue.ToString());
            ilist.Add("@ProductSubGroup"); litem.Add(DDLProductSubCategory.SelectedValue.ToString());

            DataTable dtBinddropdown = baseObj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);
            DDLMaterialCode.DataSource = dtBinddropdown;
            DDLMaterialCode.DataTextField = "CODE_DISC";
            DDLMaterialCode.DataValueField = "CODE";
            DDLMaterialCode.DataBind();
            DDLMaterialCode.Items.Insert(0, new ListItem("--Select--", ""));
        }
        //public void FillProductCode()
        //{
        //    if (ddlBusinessUnit.SelectedItem.Text == "All...")
        //    {
        //        DDLMaterialCode.Items.Clear();
        //        // DDLMaterialCode.Items.Add("Select...");
        //        if (DDLProductGroup.Text == "Select..." && DDLProductSubCategory.Text == "Select..." || DDLProductSubCategory.Text == "")
        //        {
        //            strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  INV.block=0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
        //            if (rdStock.Checked == true)
        //            {
        //                strQuery += " AND inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
        //            }
        //            strQuery += "  order by Product_Name";
        //            DDLMaterialCode.Items.Clear();
        //            DDLMaterialCode.Items.Add("Select...");
        //            //DataTable dt = baseObj.GetData(strQuery);

        //            baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Product_Name", "Product_Code");
        //            DDLMaterialCode.Focus();
        //        }
        //    }
        //    else
        //    {
        //        DDLMaterialCode.Items.Clear();
        //        // DDLMaterialCode.Items.Add("Select...");
        //        if (DDLProductGroup.Text == "Select..." && DDLProductSubCategory.Text == "Select..." || DDLProductSubCategory.Text == "")
        //        {
        //            strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  INV.block=0"
        //                + " and BU_CODE in('" + ddlBusinessUnit.SelectedItem.Value.ToString() + "')";
        //            if (rdStock.Checked == true)
        //            {
        //                strQuery += " AND inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
        //            }
        //            strQuery += " order by Product_Name";
        //            DDLMaterialCode.Items.Clear();
        //            DDLMaterialCode.Items.Add("Select...");
        //            //DataTable dt = baseObj.GetData(strQuery);

        //            baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Product_Name", "Product_Code");
        //            DDLMaterialCode.Focus();
        //        }
        //    }
        //}

        protected void rdAll_CheckedChanged(object sender, EventArgs e)
        {
            DDLProductSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();
        }

        protected void txtCrate_TextChanged(object sender, EventArgs e)
        {
            QtyCalcualtion();
            Session["focus"] = 2;
        }

        protected void txtPcs_TextChanged(object sender, EventArgs e)
        {
            QtyCalcualtion();
            Session["focus"] = 2;
        }

        protected void QtyCalcualtion()
        {
            try
            {//===========Validation for zero amount==================


                if (ddlCustomer.SelectedItem.Text == "Select...")
                {
                    string message = "alert('Please select customer name!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    ddlCustomer.Focus();
                    return;
                }
                if (DDLMaterialCode.SelectedItem.Value == "Select...")
                {
                    string message = "alert('Please select Product !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    DDLMaterialCode.Focus();
                    return;
                }

                string query = "EXEC ax.ACX_GetProductRate '" + DDLMaterialCode.SelectedItem.Value + "','" + ddlCustomer.SelectedItem.Value + "','" + Session["SiteCode"].ToString() + "'";

                DataTable dtItem = new DataTable();
                dtItem = baseObj.GetData(query);
                if (dtItem.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_MRP"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Product Price !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Product Pack Size !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Product Crate PackSize !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["LTR"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Ltr !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    decimal TotalQty = 0, crateQty = 0, boxQty = 0, pcsQty = 0, crateSize = 0, pacSize = 0;
                    crateQty = App_Code.Global.ParseDecimal(txtCrate.Text);
                    boxQty = App_Code.Global.ParseDecimal(txtEnterQty.Text);
                    pcsQty = App_Code.Global.ParseDecimal(txtPcs.Text);
                    crateSize = App_Code.Global.ParseDecimal(dtItem.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString());
                    pacSize = App_Code.Global.ParseDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString());
                    TotalQty = crateQty * crateSize + boxQty + (pcsQty / (pacSize == 0 ? 1 : pacSize));  //Total qty (Create+box+pcs) with decimal factor
                    decimal TotalBox = 0, TotalPcs = 0;
                    string BoxPcs = "";
                    
                    TotalBox = Math.Truncate(TotalQty);                     //Extract Only Box Qty From Total Qty
                    TotalPcs = Math.Round((TotalQty - TotalBox) * pacSize); //Extract Only Pcs Qty From Total Qty
                    BoxPcs = Convert.ToString(TotalBox) + '.' + (Convert.ToString(TotalPcs).Length == 1 ? "0" : "") + Convert.ToString(TotalPcs);
                    txtBoxPcs.Text = BoxPcs;
                    txtViewTotalBox.Text = TotalBox.ToString("0.00");
                    txtViewTotalPcs.Text = TotalPcs.ToString("0.00");
                    txtQtyBox.Text = TotalQty.ToString("0.0000");
                    
                    string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, ddlCustomer.SelectedItem.Value, TotalQty, "Box", Session["SiteCode"].ToString());

                    if (calValue[0] != "")
                    {

                        txtQtyCrates.Text = calValue[0];
                        txtLtr.Text = calValue[1];

                        txtPrice.Text = calValue[2];
                        txtValue.Text = calValue[3];
                        lblHidden.Text = calValue[4];
                        if (calValue[4] != "")
                        {
                            BtnAddItem.Focus();
                            BtnAddItem.CausesValidation = false;
                        }
                        else
                        {
                            string message = "alert('');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Price Not Define !";
                        DDLMaterialCode.Focus();
                    }
                }
                else
                {
                    lblMessage.Text = "Price Not Define !";
                    DDLMaterialCode.Focus();
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString().Replace("'", "''") + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
            }

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

                query = "[dbo].[ACX_GETSaleOrder]";

                cmd.Parameters.AddWithValue("@siteid", Session["SiteCode"].ToString());
                if (rdStock.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@IsStock", "1");
                    //ilist.Add("@IsStock"); litem.Add("1");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IsStock", "0");
                    //ilist.Add("@IsStock"); litem.Add("0");
                }
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
                string attachment = "attachment; filename=SaleOrder.xls";
                Response.ClearContent();
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", attachment);

                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            catch (Exception e)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
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

        protected void ImDnldTemp_Click(object sender, ImageClickEventArgs e)
        {
            ShowData_ForExcel();
        }

        protected void btnInvPre_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/frmInvoicePrepration.aspx");
        }

        protected void txtDeliveryDate_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)Session["LineItem"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    decimal tSecDisVal = dt.AsEnumerable().Sum(row => row.Field<decimal>("SEC_DISC_AMOUNT"));
                    if (tSecDisVal > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Secondary Discount gets reset and updated to new on all line items')", true);
                    }

                    ReCalculate();
                    txtSDiscPer.Text = "0";
                    txtSDiscVal.Text = "0";
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "')", true);
            }
        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["LineItem"];
            if (dt != null && dt.Rows.Count > 0)
            {
                if (txtTDValue.Text != "")
                {
                    txtHdnTDValue.Text = txtTDValue.Text;
                    txtTDValue.Text = "";
                    TDCalculation();
                }
                else if (txtHdnTDValue.Text != "")
                {
                    txtTDValue.Text = "";
                    TDCalculation();
                }
                else
                {
                    txtHdnTDValue.Text = "0";
                    txtTDValue.Text = "0";
                    TDCalculation();
                }
            }
            else
            {
                txtTDValue.Text = "";
                txtHdnTDValue.Text = "";
            }
        }

        protected void txtSDiscPer_TextChanged(object sender, EventArgs e)
        {

            Double discper = Double.Parse(txtSDiscPer.Text);
            if (discper > 99.99)
            {
                txtSDiscPer.Text = null;
            }
            else
            {
                if (txtSDiscPer.Text != null)
                {
                    txtSDiscVal.Text = null;
                    Session["focus"] = 1;
                }
            }
            //ScriptManager ScriptManager1 = ScriptManager.GetCurrent(this.Page);
            //ScriptManager1.SetFocus(btnGO);
            //Page.SetFocus(btnGO);

        }

        protected void SecDisc_TextChanged(object sender, EventArgs e)

        {

            TextBox SecDis = (TextBox)sender;
            double secdicper = Convert.ToDouble(SecDis.Text);
            if (secdicper < 100)
            {
                decimal SecDiscPer = 0, SecDiscValue = 0;
                TextBox SDiscPer = (TextBox)sender;
                GridViewRow gv = (GridViewRow)SDiscPer.Parent.Parent;
                if (string.IsNullOrEmpty(SDiscPer.Text))
                {
                    SDiscPer.Text = "0";
                }
                SecDiscPer = Convert.ToDecimal(SDiscPer.Text);
                Label lblLineNo = (Label)gv.FindControl("Line_No");
                TextBox txtSDiscValue = (TextBox)gv.FindControl("SecDiscValue");
                if (string.IsNullOrEmpty(txtSDiscValue.Text))
                {
                    txtSDiscValue.Text = "0";
                }
                if (txtSDiscValue.Text == "0")
                {
                    txtSDiscValue.Text = "0";
                }
                DataTable dt = (DataTable)Session["LineItem"];

                dt.Columns["Sec_Disc_Per"].ReadOnly = false;
                dt.Columns["SEC_DISC_AMOUNT"].ReadOnly = false;
                dt.Columns["Tax_Amount"].ReadOnly = false;
                dt.Columns["AddTax_Amount"].ReadOnly = false;
                dt.Columns["TaxableAmount"].ReadOnly = false;
                dt.Columns["Value"].ReadOnly = false;
                DataRow[] Drrow = dt.Select("SNO =" + lblLineNo.Text);

                SecDiscValue = Convert.ToDecimal(txtSDiscValue.Text);

                decimal TD, TaxableAmount, Tax1, Tax2, DiscValue, SchemeDiscValue, Basic, AddSchDiscAmt, PE;
                int Line;
                Line = Convert.ToInt32(lblLineNo.Text);
                foreach (DataRow row in Drrow)
                {
                    TD = TaxableAmount = Tax1 = Tax2 = DiscValue = SchemeDiscValue = Basic = 0;
                    Basic = Convert.ToDecimal(row["BoxPcs"]) * Convert.ToDecimal(row["Price"]);

                    SecDiscValue = (SecDiscPer * Basic) / 100;
                    TD = Convert.ToDecimal(row["TDValue"]);
                    AddSchDiscAmt = Convert.ToDecimal(row["ADDSCHDISCAMT"]);
                    PE = Convert.ToDecimal(row["PE"]);
                    //Line = Convert.ToInt32(row["SNO"]);
                    DiscValue = Convert.ToDecimal(row["DiscVal"]);

                    SchemeDiscValue = Convert.ToDecimal(row["SchemeDiscVal"]);

                    TaxableAmount = Basic - DiscValue - SecDiscValue - SchemeDiscValue - TD + PE - AddSchDiscAmt;
                    Tax1 = TaxableAmount * Convert.ToDecimal(row["Tax_Code"]) / 100;
                    Tax2 = TaxableAmount * Convert.ToDecimal(row["AddTax_Code"]) / 100;
                    decimal Total = TaxableAmount + Tax1 + Tax2;

                    row["Sec_Disc_Per"] = SecDiscPer;
                    row["SEC_DISC_AMOUNT"] = SecDiscValue;
                    row["TaxableAmount"] = TaxableAmount;
                    row["Tax_Amount"] = Tax1;
                    row["AddTax_Amount"] = Tax2;
                    row["Value"] = Total;

                    UpdateSecDisc(SecDiscPer, SecDiscValue, TaxableAmount, Tax1, Tax2, Total, Line);
                    dt.AcceptChanges();
                }
                Session["LineItem"] = dt;
                //gvDetails.DataSource = dt;
                //gvDetails.DataBind();
                //GridViewFooterCalculate(dt);
                BindingGird();
                this.SchemeDiscCalculation();
                DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
                if (dtApplicable.Rows.Count > 0)
                {
                    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                }
                if (intApplicable == 1 || intApplicable == 3)
                {
                    BindSchemeGrid();
                }
                this.SchemeDiscCalculation();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Enter Valid Secondary Discount!');", true);
                decimal SecDiscPer = 0, SecDiscValue = 0;
                TextBox SDiscPer = (TextBox)sender;
                GridViewRow gv = (GridViewRow)SDiscPer.Parent.Parent;
                Label lblLineNo = (Label)gv.FindControl("Line_No");
                TextBox txtSDiscValue = (TextBox)gv.FindControl("SecDiscValue");
                //DataTable dt = (DataTable)Session["LineItem"];
                //DataRow[] Drrow = dt.Select("SNO =" + lblLineNo.Text);
                //int val;
                //val = 0;
                //foreach (DataRow row in Drrow)
                //{
                //    row["Sec_Disc_Per"] = val;
                //    row["SEC_DISC_AMOUNT"] = val;
                //}
                //Session["LineItem"] = dt;
                //gvDetails.DataSource = dt;
                //gvDetails.DataBind();
                //GridViewFooterCalculate(dt);

                DataTable dt = (DataTable)Session["LineItem"];

                dt.Columns["Sec_Disc_Per"].ReadOnly = false;
                dt.Columns["SEC_DISC_AMOUNT"].ReadOnly = false;
                dt.Columns["Tax_Amount"].ReadOnly = false;
                dt.Columns["AddTax_Amount"].ReadOnly = false;
                dt.Columns["TaxableAmount"].ReadOnly = false;
                dt.Columns["Value"].ReadOnly = false;
                DataRow[] Drrow = dt.Select("SNO =" + lblLineNo.Text);

                SecDiscValue = Convert.ToDecimal(txtSDiscValue.Text);

                decimal TD, TaxableAmount, Tax1, Tax2, DiscValue, SchemeDiscValue, Basic;
                int Line;
                Line = Convert.ToInt32(lblLineNo.Text);
                foreach (DataRow row in Drrow)
                {
                    TD = TaxableAmount = Tax1 = Tax2 = DiscValue = SchemeDiscValue = Basic = 0;
                    Basic = Convert.ToDecimal(row["BoxPcs"]) * Convert.ToDecimal(row["Price"]);

                    SecDiscValue = (SecDiscPer * Basic) / 100;
                    TD = Convert.ToDecimal(row["TDValue"]);
                    //Line = Convert.ToInt32(row["SNO"]);
                    DiscValue = Convert.ToDecimal(row["DiscVal"]);

                    SchemeDiscValue = Convert.ToDecimal(row["SchemeDiscVal"]);

                    TaxableAmount = Basic - DiscValue - SecDiscValue - SchemeDiscValue - TD;
                    Tax1 = TaxableAmount * Convert.ToDecimal(row["Tax_Code"]) / 100;
                    Tax2 = TaxableAmount * Convert.ToDecimal(row["AddTax_Code"]) / 100;
                    decimal Total = TaxableAmount + Tax1 + Tax2;

                    row["Sec_Disc_Per"] = SecDiscPer;
                    row["SEC_DISC_AMOUNT"] = SecDiscValue;
                    row["TaxableAmount"] = TaxableAmount;
                    row["Tax_Amount"] = Tax1;
                    row["AddTax_Amount"] = Tax2;
                    row["Value"] = Total;

                    UpdateSecDisc(SecDiscPer, SecDiscValue, TaxableAmount, Tax1, Tax2, Total, Line);
                    dt.AcceptChanges();
                }
                Session["LineItem"] = dt;
                BindingGird();
                //gvDetails.DataSource = dt;
                //gvDetails.DataBind();
                //GridViewFooterCalculate(dt);


            }
        }

        protected void SecDiscValue_TextChanged(object sender, EventArgs e)
        {

            decimal SecDiscPer = 0, SecDiscValue = 0;

            TextBox SDiscVal = (TextBox)sender;
            GridViewRow gv = (GridViewRow)SDiscVal.Parent.Parent;

            if (string.IsNullOrEmpty(SDiscVal.Text))
            {
                SDiscVal.Text = "0";
            }
            SecDiscValue = Convert.ToDecimal(SDiscVal.Text);

            TextBox SDiscPer = (TextBox)gv.FindControl("SecDisc");

            if (string.IsNullOrEmpty(SDiscPer.Text))
            {
                SDiscPer.Text = "0";
            }
            SDiscPer.Text = "0";
            SecDiscPer = Convert.ToDecimal(SDiscPer.Text);
            Label lblLineNo = (Label)gv.FindControl("Line_No");
            TextBox txtSDiscValue = (TextBox)gv.FindControl("SecDiscValue");
            if (string.IsNullOrEmpty(txtSDiscValue.Text))
            {
                txtSDiscValue.Text = "0";
            }
            DataTable dt = (DataTable)Session["LineItem"];

            dt.Columns["Sec_Disc_Per"].ReadOnly = false;
            dt.Columns["SEC_DISC_AMOUNT"].ReadOnly = false;
            dt.Columns["Tax_Amount"].ReadOnly = false;
            dt.Columns["AddTax_Amount"].ReadOnly = false;
            dt.Columns["TaxableAmount"].ReadOnly = false;
            dt.Columns["Value"].ReadOnly = false;
            DataRow[] Drrow = dt.Select("SNO =" + lblLineNo.Text);

            decimal TD, TaxableAmount, Tax1, Tax2, PE, DiscValue, SchemeDiscValue, Basic, AddSchDiscAmt = 0;
            int Line;
            Line = Convert.ToInt32(lblLineNo.Text);
            foreach (DataRow row in Drrow)
            {
                TD = TaxableAmount = Tax1 = Tax2 = PE = DiscValue = SchemeDiscValue = Basic = AddSchDiscAmt = 0;
                Basic = Convert.ToDecimal(row["BoxPcs"]) * Convert.ToDecimal(row["Price"]);
                if (SecDiscPer != 0)
                {
                    SecDiscValue = (SecDiscPer * Basic) / 100;
                }
                PE = Convert.ToDecimal(row["PE"]);
                AddSchDiscAmt = Convert.ToDecimal(row["ADDSCHDISCAMT"]);
                TD = Convert.ToDecimal(row["TDValue"]);
                //Line = Convert.ToInt32(row["SNO"]);
                DiscValue = Convert.ToDecimal(row["DiscVal"]);

                SchemeDiscValue = Convert.ToDecimal(row["SchemeDiscVal"]);

                TaxableAmount = Basic - DiscValue - SecDiscValue - SchemeDiscValue - TD + PE - AddSchDiscAmt;
                Tax1 = TaxableAmount * Convert.ToDecimal(row["Tax_Code"]) / 100;
                Tax2 = TaxableAmount * Convert.ToDecimal(row["AddTax_Code"]) / 100;
                decimal Total = TaxableAmount + Tax1 + Tax2;

                row["Sec_Disc_Per"] = SecDiscPer;
                row["SEC_DISC_AMOUNT"] = SecDiscValue;
                row["TaxableAmount"] = TaxableAmount;
                row["Tax_Amount"] = Tax1;
                row["AddTax_Amount"] = Tax2;
                row["Value"] = Total;

                if (TaxableAmount + Tax1 + Tax2 < 0)
                {
                    if (Basic > 0)
                    {
                        txtSDiscVal.Text = "0.00";
                        txtSDiscPer.Text = "0.00";
                        string message = "alert('Invoice Line value should not be less than zero!! At Line no " + row["SNO"].ToString() + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    }

                }
                UpdateSecDisc(SecDiscPer, SecDiscValue, TaxableAmount, Tax1, Tax2, Total, Line);
                dt.AcceptChanges();
            }
            Session["LineItem"] = dt;
            BindingGird();
            //gvDetails.DataSource = dt;
            //gvDetails.DataBind();
            //GridViewFooterCalculate(dt);
            this.SchemeDiscCalculation();
            DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
            if (dtApplicable.Rows.Count > 0)
            {
                intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
            }
            if (intApplicable == 1 || intApplicable == 3)
            {
                BindSchemeGrid();
            }
            this.SchemeDiscCalculation();
        }


        protected void txtSDiscVal_TextChanged(object sender, EventArgs e)
        {
            if (txtSDiscVal.Text != null)
            {
                txtSDiscPer.Text = null;
            }
            Session["focus"] = 1;
        }

        protected void txtTDValue_TextChanged(object sender, EventArgs e)
        {
            Session["focus"] = 3;
        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlBusinessUnit.SelectedItem.Text == "All...")
            //{
            //    DDLProductGroup.Items.Clear();
            //    string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>''  and A.block=0 and BU_CODE in(select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Session["SiteCode"].ToString() + "') order by a.Product_Group";
            //    DDLProductGroup.Items.Clear();
            //    DDLProductGroup.Items.Add("Select...");
            //    baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

            //}
            //else
            //{
            //    DDLProductGroup.Items.Clear();
            //    string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>''  and A.block=0 and BU_CODE in ('" + ddlBusinessUnit.SelectedItem.Value.ToString() + "') order by a.Product_Group";
            //    DDLProductGroup.Items.Clear();
            //    DDLProductGroup.Items.Add("Select...");
            //    baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");
            //}
            //if (ddlBusinessUnit.SelectedItem.Text == "All...")
            //{
            //    DDLMaterialCode.Items.Clear();
            //    // DDLMaterialCode.Items.Add("Select...");
            //    if (DDLProductGroup.Text == "Select..." && DDLProductSubCategory.Text == "Select..." || DDLProductSubCategory.Text == "")
            //    {
            //        strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  INV.block=0"
            //            + " and BU_CODE in(select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Session["SiteCode"].ToString() + "')";
            //        if (rdStock.Checked == true)
            //        {
            //            strQuery += " AND inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
            //        }
            //        strQuery += "  order by Product_Name";
            //        DDLMaterialCode.Items.Clear();
            //        DDLMaterialCode.Items.Add("Select...");
            //        //DataTable dt = baseObj.GetData(strQuery);

            //        baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Product_Name", "Product_Code");
            //        DDLMaterialCode.Focus();
            //    }
            //}
            //else
            //{
            //    DDLMaterialCode.Items.Clear();
            //    // DDLMaterialCode.Items.Add("Select...");
            //    if (DDLProductGroup.Text == "Select..." && DDLProductSubCategory.Text == "Select..." || DDLProductSubCategory.Text == "")
            //    {
            //        strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  INV.block=0"
            //            + " and BU_CODE in('" + ddlBusinessUnit.SelectedItem.Value.ToString() + "')";
            //        if (rdStock.Checked == true)
            //        {
            //            strQuery += " AND inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
            //        }
            //        strQuery += "  order by Product_Name";
            //        DDLMaterialCode.Items.Clear();
            //        DDLMaterialCode.Items.Add("Select...");
            //        //DataTable dt = baseObj.GetData(strQuery);

            //        baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Product_Name", "Product_Code");
            //        DDLMaterialCode.Focus();
            //    }
            // }
            DDLProductSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();
        }

        protected void PcsVisibility()
        {
            try
            {
                DataTable dt = new DataTable();
                string query = "";
                dt = baseObj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    lblEnterPcs.Visible = true;
                    lblTotalPcs.Visible = true;
                    txtPcs.Visible = true;
                    txtViewTotalPcs.Visible = true;
                }
                else
                {
                    lblEnterPcs.Visible = false;
                    lblTotalPcs.Visible = false;
                    txtPcs.Visible = false;
                    txtViewTotalPcs.Visible = false;
                }
            }
            catch(Exception ex)
            { ErrorSignal.FromCurrentContext().Raise(ex); }
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

        protected void rdAsc_CheckedChanged(object sender, EventArgs e)
        {
            BindingGird();
        }

        protected void Exempt_CheckedChanged(object sender, EventArgs e)
        {
            DDLProductSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();
        }

        private void BindingGird()
        {

            if (Session["LineItem"] == null)
            {
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gvDetails.Visible = false;
            }
            DataTable dt = (DataTable)Session["LineItem"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    DataView dv = dt.DefaultView;
                    if (rdAsc.Checked == true)
                    {
                        dv.Sort = "SNO " + rdAsc.Text.ToString();
                    }
                    else if (rdDesc.Checked == true)
                    {
                        dv.Sort = "SNO " + rdDesc.Text.ToString();
                    }
                    gvDetails.DataSource = dv.ToTable();
                    gvDetails.DataBind();
                    GridViewFooterCalculate(dt);
                    gvDetails.Visible = true;
                }
                else
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = false;
                }
            }
            else
            {
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gvDetails.Visible = false;
            }
        }
        protected void ddlsorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindingGird();
        }
    }
}

