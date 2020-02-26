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
    public partial class frmSDSaleOrder : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1, cmd2;
        SqlTransaction transaction;
        
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string strQuery = string.Empty;
        int intApplicable = 0;
        string PRESO_NO = string.Empty;
        string strCurrentN0 = string.Empty;
        const int gcAmount = 20;
        string SessionScheme = "SDSchemeGrid";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblMessage.Text = string.Empty;
                CalendarExtender1.StartDate = DateTime.Now;

                fillCustomerGroup();
                ProductGroup();
                FillProductCode();
                txtDeliveryDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                Session["LineItem"] = null;
                Session["PreSoNO"] = null;
                Session["SONO"] = null;
                if (Request.QueryString["Indent_NO"] != null && Request.QueryString["SiteId"] != null)
                {
                    string Indent_NO = Request.QueryString["Indent_NO"].ToString();
                    string CustomerCode = Request.QueryString["SiteId"].ToString();                   
                    BindIndentDetails(Indent_NO, CustomerCode);
                }

            }

            Page.MaintainScrollPositionOnPostBack = true;

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            //===============Validation for gridview==============
            int gvRow = gvDetails.Rows.Count;
            if (gvRow > 0)
            {
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

                    string message = "alert('Please Enter a line');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a line');", true);
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
                                " where A.Customer_Code='" + dt.Rows[0]["CUSTOMER_CODE"].ToString() + "' and( A.Customer_Code=(select SubDistributor from ax.ACX_SDLinking B where Other_Site='" + dt.Rows[0]["SiteId"].ToString() + "' and B.SubDistributor='" + dt.Rows[0]["CUSTOMER_CODE"].ToString() + "') Or A.[SITE_CODE]='" + dt.Rows[0]["SiteId"].ToString() + "' )";
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
            catch(Exception ex)
            {
                //transaction.Rollback();
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('" + ex.Message.ToString().Replace("'","") + "');", true);
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

        public void BindSODetails(string SONO)
        {
            DataTable dt = new DataTable();
            DataTable dtHeader = new DataTable();

            //================Fill Line Item Details==========
            AddColumnInDataTable();
            Session["LineItem"] = null;
            strQuery = "Select A.[LINE_NO] as SNO,B.[PRODUCT_GROUP] as MaterialGroup,A.[PRODUCT_CODE] as ProductCodeName,B.Product_Name,A.[CRATES] as QtyCrates ,A.[BOXQty] as OnlyQtyBox,CAST(A.[PCSQTY] AS decimal(9,2)) as QtyPcs,CAST(A.[BOX] AS decimal(9,2)) as QtyBox,A.[LTR] as QtyLtr,A.[RATE] as Price,A.[Amount] as Value,A.[UOM],case when A.[DiscType]='0' then 'Per' when A.[DiscType]='1' then 'Val' else '2' end as DiscType,A.[Disc]" +
                     ",A.[DiscVal],A.[TAX_CODE],A.[TAX_AMOUNT],A.[ADDTAX_CODE],A.[ADDTAX_AMOUNT],B.Product_MRP as MRP,case A.[DiscCalculationBase] when 0 then 'Price' when 1 then 'MRP' else '2' end  as CalculationBase,A.[PRODUCT_CODE]+'-'+B.Product_Name as Product_Name ,ISNULL(A.BOXPCS,0.00) AS BOXPCS, case when A.[DiscType]='0' and A.[DiscCalculationBase]=1 then 'MRP' else 'Price' end as CalculationOn ,isnull(A.BasePrice,0) as BasePrice,isnull(A.TaxableAmount,0) as TaxableAmount,A.HSNCODE,A.TAXCOMPONENT,A.ADDTAXCOMPONENT,A.SchemeDiscPer as SchemeDisc,A.SchemeDiscValue as SchemeDiscVal ,ISNULL(ADDSCHDISCPER,0) ADDSCHDISCPER,ISNULL(ADDSCHDISCVAL,0) ADDSCHDISCVAL,ISNULL(ADDSCHDISCAMT,0) ADDSCHDISCAMT " +
                     " from [ax].[ACXSALESLINEPRE] A " +
                     " Inner Join ax.InventTable B on A.[PRODUCT_CODE]=B.ITEMID" +
                     " where A.SO_NO='" + SONO + "' and A.SiteId='" + Session["SiteCode"].ToString() + "' and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'" +
                     " Order By A.[LINE_NO] ";
            Session["LineItem"] = baseObj.GetData(strQuery);
            dt = (DataTable)Session["LineItem"];
            if (dt.Rows.Count > 0)
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
                GridViewFooterCalculate(dt);
            }
            else
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
            }
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

                cmd1 = new SqlCommand("ACX_InsertSaleLinePre");
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
                    catch (Exception ex)
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
                    ReturnArray = obj.CalculatePrice1(dtExcelData.Rows[i]["ProductCode"].ToString(), CustomerCode, App_Code.Global.ParseDecimal(dtExcelData.Rows[i]["Qty"].ToString()), "Box");
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
              

        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                        + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "' and block=0";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            FillProductCode();
            DDLProductSubCategory.Focus();

            PcsBillingApplicable();
        }

        public void fillCustomerGroup()
        {
            strQuery = "Select CUSTGROUP_CODE+'-'+CUSTGROUP_NAME as Name,CUSTGROUP_CODE from ax.ACXCUSTGROUPMASTER where DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' and  Blocked = 0";
            ddlCustomerGroup.Items.Clear();
            ddlCustomerGroup.Items.Add("Select...");
            baseObj.BindToDropDown(ddlCustomerGroup, strQuery, "Name", "CUSTGROUP_CODE");
        }

        public void ProductGroup()
        {

            string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' and block=0 order by a.Product_Group";
            DDLProductGroup.Items.Clear();
            DDLProductGroup.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

        }

        private DataTable AddLineItems()
        {
            try
            {
                if (Session["LineItem"] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session["LineItem"];
                }
                DataRow[] dataPerDay = (from myRow in dtLineItems.AsEnumerable()
                                        where myRow.Field<string>("ProductCodeName") == DDLMaterialCode.SelectedValue.ToString()
                                        select myRow).ToArray();
                if (dataPerDay.Count() == 0)
                {
                    #region  Insert into Session
                    DataTable dt = new DataTable();
                    dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + DDLMaterialCode.SelectedValue.ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtQtyBox.Text.Trim()) + "," + Convert.ToDecimal(txtPrice.Text.Trim()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                    string abc = "EXEC USP_ACX_GetSalesLineCalcGST '" + DDLMaterialCode.SelectedValue.ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtQtyBox.Text.Trim()) + "," + Convert.ToDecimal(txtPrice.Text.Trim()) + ",'" + Session["SITELOCATION"].ToString() + "','" + ddlCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'";
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["RETMSG"].ToString().IndexOf("FALSE") >= 0)
                        {
                            string message = "alert('" + dt.Rows[0]["RETMSG"].ToString().Replace("FALSE|", "") + "');";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                            DDLMaterialCode.Focus();
                            return dtLineItems;
                        }
                        DataRow row;
                        row = dtLineItems.NewRow();
                        int count = 1 + dtLineItems.Rows.Count;
                        row["SNO"] = count;
                        row["MaterialGroup"] = DDLProductGroup.SelectedItem.Text.ToString();
                        row["MaterialGroup"] = DDLProductGroup.SelectedItem.Text.ToString();
                        row["ProductCodeName"] = DDLMaterialCode.SelectedValue.ToString();
                        row["Product_Name"] = DDLMaterialCode.SelectedItem.Text.ToString();
                        row["QtyCrates"] = Convert.ToDecimal(txtQtyCrates.Text.Trim().ToString());
                        row["QtyBox"] = decimal.Parse(txtQtyBox.Text.Trim().ToString());
                        row["OnlyQtyBox"] = Convert.ToDecimal(txtViewTotalBox.Text.Trim().ToString());
                        row["BoxPcs"] = Convert.ToDecimal(txtBoxPcs.Text.Trim());
                        row["QtyPcs"] = Convert.ToDecimal(txtViewTotalPcs.Text.Trim().ToString());
                        row["QtyLtr"] = Convert.ToDecimal(txtLtr.Text.Trim().ToString());
                        row["Price"] = Convert.ToDecimal(dt.Rows[0]["Rate"]);
                        row["BasePrice"] = Convert.ToDecimal(txtPrice.Text);
                        row["Value"] = Convert.ToDecimal(dt.Rows[0]["VALUE"]);
                        row["UOM"] = lblHidden.Text;

                        row["DiscType"] = dt.Rows[0]["DISCTYPE"].ToString();
                        row["Disc"] = Convert.ToDecimal(dt.Rows[0]["DISC"]);
                        row["DiscVal"] = Convert.ToDecimal(dt.Rows[0]["DISCVAL"]);
                        //==========For Tax===============
                        row["Tax_Code"] = Convert.ToDecimal(dt.Rows[0]["TAX_PER"]);
                        row["Tax_Amount"] = Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]);
                        row["AddTax_Code"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]);
                        row["AddTax_Amount"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]);
                        row["MRP"] = Convert.ToDecimal(dt.Rows[0]["MRP"]);
                        row["TaxableAmount"] = Convert.ToDecimal(dt.Rows[0]["TaxableAmount"]);
                        /*  ----- GST Implementation Start */
                        row["TaxComponent"] = dt.Rows[0]["TAX_CODE"].ToString();
                        row["AddTaxComponent"] = dt.Rows[0]["ADDTAX_CODE"].ToString();
                        row["HSNCode"] = dt.Rows[0]["HSNCODE"].ToString();
                        row["ADDSCHDISCPER"] = new decimal(0);//sam
                        row["ADDSCHDISCVAL"] = new decimal(0);//sam
                        row["ADDSCHDISCAMT"] = new decimal(0);//sam
                        /*  ----- GST Implementation End */

                        //if (dt.Rows[0]["DISCTYPE"].ToString() == "Per" && dt.Rows[0]["CALCULATIONBASE"].ToString() == "MRP")
                        if (dt.Rows[0]["CALCULATIONBASE"].ToString() == "MRP")
                        {
                            row["CalculationOn"] = "MRP";
                            row["CALCULATIONBASE"] = dt.Rows[0]["CALCULATIONBASE"].ToString();
                        }
                        else
                        {
                            row["CalculationOn"] = "Price";
                            row["CALCULATIONBASE"] = "2";
                        }
                        if (row.Table.Columns.Contains("SchemeDisc"))
                        {
                            row["SchemeDisc"] = 0.00;
                        }
                        if (row.Table.Columns.Contains("SchemeDiscVal"))
                        {
                            row["SchemeDiscVal"] = 0.00;
                        }

                        dtLineItems.Rows.Add(row);
                    }
                    #endregion

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

                        cmd1 = new SqlCommand("ACX_InsertSaleLinePre");
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;

                    }
                    else
                    {
                        conn = obj.GetConnection();
                        transaction = conn.BeginTransaction();
                        cmd1 = new SqlCommand("ACX_InsertSaleLinePre");
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

                    //===========For Tax=========
                    cmd1.Parameters.AddWithValue("@Tax_Code", Convert.ToDecimal(dt.Rows[0]["TAX_PER"]));
                    cmd1.Parameters.AddWithValue("@Tax_Amount", Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]));
                    cmd1.Parameters.AddWithValue("@AddTax_Code", Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]));
                    cmd1.Parameters.AddWithValue("@AddTax_Amount", Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]));
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

                    //cmd1.Parameters.AddWithValue("@RECID", 0);
                    cmd1.Parameters.AddWithValue("@UOM", lblHidden.Text);
                    cmd1.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());

                    /*-------- GST IMPLEMENTATION START --------*/
                    cmd1.Parameters.AddWithValue("@HSNCODE", dt.Rows[0]["HSNCODE"]);
                    cmd1.Parameters.AddWithValue("@COMPOSITIONSCHEME", Convert.ToInt32((chkCompositionScheme.Checked == true ? 1 : 0)));
                    cmd1.Parameters.AddWithValue("@TAXCOMPONENT", dt.Rows[0]["TAX_CODE"].ToString());
                    cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", dt.Rows[0]["ADDTAX_CODE"].ToString());
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

                    txtQtyCrates.Text = string.Empty;
                    //txtEnterQty.Text = string.Empty;
                    txtBoxqty.Text = "";
                    txtPCSQty.Text = string.Empty;
                    txtViewTotalPcs.Text = string.Empty;
                    txtViewTotalBox.Text = string.Empty;
                    txtBoxPcs.Text = "";

                }// end of checkinh dublicate product
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

        private void AddColumnInDataTable()
        {
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
            //===========Tax==============================
            dtLineItems.Columns.Add("Tax_Code", typeof(decimal));
            dtLineItems.Columns.Add("Tax_Amount", typeof(decimal));
            dtLineItems.Columns.Add("AddTax_Code", typeof(decimal));
            dtLineItems.Columns.Add("AddTax_Amount", typeof(decimal));
            dtLineItems.Columns.Add("MRP", typeof(decimal));
            dtLineItems.Columns.Add("CalculationBase", typeof(string));
            dtLineItems.Columns.Add("Calculationon", typeof(string));   //mrp or price
            dtLineItems.Columns.Add("TaxableAmount", typeof(decimal));
            // New Fields for GST
            dtLineItems.Columns.Add("HSNCode", typeof(string));
            dtLineItems.Columns.Add("TaxComponent", typeof(string));
            dtLineItems.Columns.Add("AddTaxComponent", typeof(string));
            dtLineItems.Columns.Add("ADDSCHDISCPER", typeof(decimal));//sam
            dtLineItems.Columns.Add("ADDSCHDISCVAL", typeof(decimal));//sam
            dtLineItems.Columns.Add("ADDSCHDISCAMT", typeof(decimal));//sam
        }

        private bool ValidateLineItemAdd()
        {
            bool b = true;

            if (txtBilltoState.Text.Trim().Length == 0 || txtAddress.Text.Trim().Length == 0)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);

                string message = "alert('Select Customer Bill To Address !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                ddlCustomer.Focus();
                b = false;
                return b;
            }

            if (DDLProductGroup.Text == "Select..." || DDLProductGroup.Text == "")
            {
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);
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
                   // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Beat Name !');", true);

                    string message = "alert('Select Beat Name !');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    ddlBeatName.Focus();
                    b = false;
                    return b;
                }
            }
            if (DDLMaterialCode.Text == string.Empty || DDLMaterialCode.Text == "Select...")
            {
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Product First !');", true);

                string message = "alert('Select Product First !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                DDLMaterialCode.Focus();
                b = false;
                return b;
            }
            if (txtQtyBox.Text == string.Empty || txtQtyBox.Text == "0" || txtQtyBox.Text == "0.00")
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Qty cannot be left blank !');", true);
                string message = "alert('Qty cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (Convert.ToDecimal(txtQtyBox.Text.Trim()) == 0)
            {
                b = false;
                string message = "alert('Please enter Qty!!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (txtQtyCrates.Text == string.Empty)
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price cannot be left blank !');", true);
                string message = "alert('Price cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (txtLtr.Text == string.Empty || txtLtr.Text == "0")
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('ltr cannot be left blank !');", true);
                string message = "alert('ltr cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    return b;
                }
                else
                {
                    b = true;
                }

            }
            return b;

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
                    string query = "Delete from [ax].[ACXSALESLINEPRE] where [SO_NO]='" + Session["SONO"] + "' and [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and [SITEID]='" + Session["SiteCode"].ToString() + "' and  [LINE_NO]=" + hiddenField.Value + "";
                    baseObj.ExecuteCommand(query);
                    //=================Fill Datatable After delete from Pretable========
                    AddColumnInDataTable();
                    Session["LineItem"] = null;
                    strQuery = "Select A.[LINE_NO] as SNO,B.[PRODUCT_GROUP] as MaterialGroup,A.[PRODUCT_CODE] as ProductCodeName,A.[PRODUCT_CODE] +'-'+B.Product_Name as Product_Name,A.[CRATES] as QtyCrates,CAST(A.[BOX] AS Decimal(10,2)) as QtyBox,FLOOR(A.[BOX]) as OnlyQtyBox,CAST((CAST(A.[BOX] AS decimal(9,2))-CAST(FLOOR(A.[BOX]) AS DECIMAL(9,0)))*B.Product_PackSize AS decimal(9,0)) as QtyPcs,A.[LTR] as QtyLtr,A.[RATE] as Price,A.[AMOUNT] as Value,A.[UOM],cast(A.[DiscType] as nvarchar(10)) as DiscType,A.[Disc]" +
                        ",A.[DiscVal],A.[TAX_CODE],A.[TAX_AMOUNT],A.[ADDTAX_CODE],A.[ADDTAX_AMOUNT],B.Product_MRP as MRP,case A.[DiscCalculationBase] when 0 then 'Price' when 1 then 'MRP' else '2' end  as CalculationBase,ISNULL(A.BOXPCS,0.00) AS BOXPCS ,case when A.[DiscType]='0' and A.[DiscCalculationBase]=1 then 'MRP' else 'Price' end as CalculationOn ,Isnull(A.BasePrice,0) as  BasePrice,isnull(A.TaxableAmount,0) as TaxableAmount,A.SchemeDiscPer as SchemeDisc,A.SchemeDiscValue as SchemeDiscVal ISNULL(ADDSCHDISCPER,0) ADDSCHDISCPER,ISNULL(ADDSCHDISCVAL,0) ADDSCHDISCVAL,ISNULL(ADDSCHDISCAMT,0) ADDSCHDISCAMT " +
                        " from [ax].[ACXSALESLINEPRE] A " +
                        " Inner Join ax.InventTable B on A.[PRODUCT_CODE]=B.ITEMID" +
                        " where A.SO_NO='" + Session["SONO"] + "' and A.SiteId='" + Session["SiteCode"].ToString() + "' and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'" +
                        " Order By A.[LINE_NO] ";
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
                    catch (Exception ex)
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
                    //string query1 = "Delete from [ax].[ACXSALESLINEPRE] where [SO_NO]='" + Session["SONO"] + "' and [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and [SITEID]='" + Session["SiteCode"].ToString() + "' and  [LINE_NO]=" + hiddenField.Value + "";
                    baseObj.ExecuteCommand(query);
                    //=================Fill Datatable After delete from Pretable========
                    AddColumnInDataTable();
                    Session["LineItem"] = null;
                    strQuery = "Select A.[LINE_NO] as SNO,B.[PRODUCT_GROUP] as MaterialGroup,A.[PRODUCT_CODE] as ProductCodeName,A.[PRODUCT_CODE] +'-'+B.Product_Name as Product_Name,A.[CRATES] as QtyCrates,CAST(A.[BOX] AS Decimal(10,2)) as QtyBox,FLOOR(A.[BOX]) as OnlyQtyBox,CAST((CAST(A.[BOX] AS decimal(9,2))-CAST(FLOOR(A.[BOX]) AS DECIMAL(9,0)))*B.Product_PackSize AS decimal(9,0)) as QtyPcs,A.[LTR] as QtyLtr,A.[RATE] as Price,A.[AMOUNT] as Value,A.[UOM],cast(A.[DiscType] as nvarchar(10)) as DiscType,A.[Disc]" +
                               ",A.[DiscVal],A.[TAX_CODE],A.[TAX_AMOUNT],A.[ADDTAX_CODE],A.[ADDTAX_AMOUNT],B.Product_MRP as MRP,case A.[DiscCalculationBase] when 0 then 'Price' when 1 then 'MRP' else '2' end as CalculationBase ,ISNULL(A.BOXPCS,0.00) AS BOXPCS ,case when A.[DiscType]='0' and A.[DiscCalculationBase]=1 then 'MRP' else 'Price' end as CalculationOn ,Isnull(A.BasePrice,0) as  BasePrice,isnull(A.TaxableAmount,0) as TaxableAmount,A.HSNCODE,A.TAXCOMPONENT,A.ADDTAXCOMPONENT,A.SchemeDiscPer as SchemeDisc,A.SchemeDiscValue as SchemeDiscVal ,ISNULL(ADDSCHDISCPER,0) ADDSCHDISCPER,ISNULL(ADDSCHDISCVAL,0) ADDSCHDISCVAL,ISNULL(ADDSCHDISCAMT,0) ADDSCHDISCAMT " +
                               "from [ax].[ACXSALESLINEPRE] A " +
                               " Inner Join ax.InventTable B on A.[PRODUCT_CODE]=B.ITEMID" +
                               " where A.SO_NO='" + Session["PreSoNO"] + "' and A.SiteId='" + Session["SiteCode"].ToString() + "' and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'" +
                               " Order By A.[LINE_NO] ";
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
                    catch (Exception ex)
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
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
                GridViewFooterCalculate(dt);
                Session["LineItem"] = dt;
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
                }

            }

        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            //=============Grid Validation (Cant add same item )=================
            foreach (GridViewRow grv in gvDetails.Rows)
            {
                string product = grv.Cells[3].Text;
                if (DDLMaterialCode.SelectedItem.Value == product)
                {
                    txtBoxqty.Text = "";
                    txtPCSQty.Text = "";
                    txtViewTotalBox.Text = "";
                    txtViewTotalPcs.Text = "";

                    //txtEnterQty.Text = "";
                    txtQtyBox.Text = "";
                    txtQtyCrates.Text = "";
                    txtLtr.Text = "";
                    txtPrice.Text = "";
                    txtValue.Text = "";
                    txtCrateQty.Text = string.Empty;
                    txtPCSQty.Text = string.Empty;
                    //txtPack.Text = string.Empty;
                    string message = "alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');", true);
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
                dt = Session["ItemTable"] as DataTable;
                dt = AddLineItems();
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
                    gvDetails.Visible = false;
                }
            }
            //txtEnterQty.Text = string.Empty;
            txtQtyBox.Text = string.Empty;
            txtCrateQty.Text = string.Empty;
            txtPCSQty.Text = string.Empty;
            //txtPack.Text = string.Empty;
            //  AddDataOnPreTable();
            DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + ddlCustomer.SelectedValue.ToString() + "'");
            if (dtApplicable.Rows.Count > 0)
            {
                intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
            }
            if (intApplicable == 1 || intApplicable == 3)
            {
                BindSchemeGrid();
            }
            //if (intApplicable == 1 || intApplicable == 3)
            //{
            //    BindSchemeGrid();
            //}
            DDLMaterialCode.Focus();
        }

        private void GridViewFooterCalculate(DataTable dt)
        {                        
            decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Value"));          //For Total[Sum] Value Show in Footer--//
            decimal tBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyBox"));
            decimal tOnlyBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("OnlyQtyBox"));
            decimal tPcs = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyPcs"));
            decimal tCrate = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyCrates"));
            decimal tLtr = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyLtr"));
            decimal tTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("Tax_Amount"));

            //===============11-5-2016=====
            decimal tPrice = dt.AsEnumerable().Sum(row => row.Field<decimal>("Price"));
            decimal tAddTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("AddTax_Amount"));
            decimal tDiscValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("DiscVal"));
            decimal tSchemeDiscVal = dt.AsEnumerable().Sum(row => row.Field<decimal>("SchemeDiscVal"));
            decimal tAddSchemeDiscVal = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADDSCHDISCAMT"));
            //==============================

            gvDetails.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[3].Text = "TOTAL";
            gvDetails.FooterRow.Cells[3].Font.Bold = true;


            //gvDetails.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Right;
            //gvDetails.FooterRow.Cells[18].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[18].Text = total.ToString("N2");
            //gvDetails.FooterRow.Cells[18].Font.Bold = true;

            gvDetails.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[4].Text = tBox.ToString("N2");
            gvDetails.FooterRow.Cells[4].Font.Bold = true;

            //gvDetails.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            //gvDetails.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[5].Text = tPcs.ToString("N2");
            //gvDetails.FooterRow.Cells[5].Font.Bold = true;
            gvDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[6].Text = tOnlyBox.ToString("N2");
            gvDetails.FooterRow.Cells[6].Font.Bold = true;

            //gvDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            //gvDetails.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[7].Text = tCrate.ToString("N2");
            //gvDetails.FooterRow.Cells[7].Font.Bold = true;

            //gvDetails.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            //gvDetails.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[8].Text = tLtr.ToString("N2");
            //gvDetails.FooterRow.Cells[8].Font.Bold = true;

            gvDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[7].Text = tPcs.ToString("N2");
            gvDetails.FooterRow.Cells[7].Font.Bold = true;

            //gvDetails.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Left;
            //gvDetails.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[9].Text = tPcs.ToString("N2");
            //gvDetails.FooterRow.Cells[9].Font.Bold = true;

            gvDetails.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[9].Text = tCrate.ToString("N2");
            gvDetails.FooterRow.Cells[9].Font.Bold = true;

            gvDetails.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[10].Text = tLtr.ToString("N2");
            gvDetails.FooterRow.Cells[10].Font.Bold = true;

            gvDetails.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[17].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[17].Text = tTaxAmount.ToString("N2");
            gvDetails.FooterRow.Cells[17].Font.Bold = true;

            //===============11-5-16=============
            gvDetails.FooterRow.Cells[20].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[20].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[20].Text = total.ToString("N2");
            gvDetails.FooterRow.Cells[20].Font.Bold = true;

            gvDetails.FooterRow.Cells[19].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[19].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[19].Text = tAddTaxAmount.ToString("N2");
            gvDetails.FooterRow.Cells[19].Font.Bold = true;

            gvDetails.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[15].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[15].Text = tDiscValue.ToString("N2");
            gvDetails.FooterRow.Cells[15].Font.Bold = true;


            gvDetails.FooterRow.Cells[26].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[26].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[26].Text = tSchemeDiscVal.ToString("N2");
            gvDetails.FooterRow.Cells[26].Font.Bold = true;

            //gvDetails.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Right;
            //gvDetails.FooterRow.Cells[15].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[15].Text = tTaxAmount.ToString("N2");
            //gvDetails.FooterRow.Cells[15].Font.Bold = true;

            ////===============11-5-16=============
            //gvDetails.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Right;
            //gvDetails.FooterRow.Cells[17].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[17].Text = tAddTaxAmount.ToString("N2");
            //gvDetails.FooterRow.Cells[17].Font.Bold = true;

            //gvDetails.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
            //gvDetails.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[13].Text = tDiscValue.ToString("N2");
            //gvDetails.FooterRow.Cells[13].Font.Bold = true;

            gvDetails.FooterRow.Cells[30].HorizontalAlign = HorizontalAlign.Left;//sam
            gvDetails.FooterRow.Cells[30].ForeColor = System.Drawing.Color.MidnightBlue;//sam
            gvDetails.FooterRow.Cells[30].Text = tAddSchemeDiscVal.ToString("N2");//sam
            gvDetails.FooterRow.Cells[30].Font.Bold = true;//sam
        }

        protected void txtQtyBox_TextChanged(object sender, EventArgs e)
        {
            try
            {//===========Validation for zero amount==================

//                string query = @"select I.ITEMID,t.AMOUNT,I.PRODUCT_PACKSIZE,I.PRODUCT_CRATE_PACKSIZE,I.LTR from ax.InventTable I
//                                Inner join DBO.PriceDiscTable t on I.ITEMID =t.ITEmRelation
//                                where ACCOUNTRELATION = (select PriceGroup from ax.ACXCUSTMASTER where CUSTOMER_CODE = '" + ddlCustomer.SelectedItem.Value + "') "
//                                  + " and I.ITEMID ='" + DDLMaterialCode.SelectedItem.Value + "'  ";

                string query = "EXEC ax.ACX_GetProductRate '" + DDLMaterialCode.SelectedItem.Value + "','" + ddlCustomer.SelectedItem.Value + "'";
                DataTable dtItem = new DataTable();
                dtItem = baseObj.GetData(query);
                if (dtItem.Rows.Count > 0)
                {
                   // if (Convert.ToDecimal(dtItem.Rows[0]["AMOUNT"].ToString()) <= 0)
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_MRP"].ToString()) <= 0)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Check Product Price !');", true);

                        string message = "alert('Please Check Product Price !');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString()) <= 0)
                    {
                       // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Check Product Pack Size !');", true);

                        string message = "alert('Please Check Product Pack Size !');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString()) <= 0)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Check Product Crate PackSize !');", true);

                        string message = "alert('Please Check Product Crate PackSize !');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["LTR"].ToString()) <= 0)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Check Ltr !');", true);
                        string message = "alert('Please Check Ltr !');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        return;
                    }
                }


                //string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, ddlCustomer.SelectedItem.Value, int.Parse(txtEnterQty.Text), "Box");
                string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, ddlCustomer.SelectedItem.Value, Convert.ToDecimal(txtQtyBox.Text), "Box");
                if (calValue[0] != "")
                {
                    if (calValue[5] == "Box")
                    {
                        //txtQtyBox.Text = txtEnterQty.Text;
                        txtQtyCrates.Text = calValue[0];
                    }
                    if (calValue[5] == "Crate")
                    {
                        //txtQtyCrates.Text = txtEnterQty.Text;
                        txtQtyBox.Text = calValue[0];
                    }
                    txtLtr.Text = calValue[1];

                    txtPrice.Text = calValue[2];
                    txtValue.Text = calValue[3];
                    lblHidden.Text = calValue[4];
                    if (calValue[4] != "")
                    {
                        BtnAddItem.Focus();
                        BtnAddItem.CausesValidation = false;

                        //BtnAddItem.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(BtnAddItem, null) + ";");

                    }

                }
                else
                {
                    lblMessage.Text = "Price Not Define !";
                }
            }
            catch (Exception ex)
            {
                //                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price Not Defined !');", true);
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + ex.Message.ToString() + "');", true);
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

            }
          
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

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
                dt.Columns.Add("FreeQtyPcs", typeof(int));
                dt.Columns.Add("SetNO", typeof(int));


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
                dv1.RowFilter = "[SchemeCode]='" + SelectedShemeCode + "'";
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

                        if (TotalQty != FreeQty && Slab == Convert.ToInt16(drow["Slab"]) && SetNo == Convert.ToInt16(drow["SetNo"]) && TotalQtyPcs != FreeQtyPcs)
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

        //private void AddDataOnPreTable()
        //{
        //    try
        //    {
        //        decimal Total = 0;
        //        string strPsrName = string.Empty;
        //        string strBeatName = string.Empty;
        //        if (ddlPSRName.Visible == true)
        //        {
        //            strPsrName = ddlPSRName.SelectedItem.Value;
        //            strBeatName = ddlBeatName.SelectedItem.Value;
        //        }

        //                           //Total += Convert.ToDecimal(grv.Cells[12].Text);


        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Error:" + ex.Message + " !');", true);
        //    }
        //}

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
                    IsSchemeValidate = baseObj.ValidateSchemeQty(SelectedSchemeCode, ref gvScheme); // ValidateSchemeQty(SelectedSchemeCode);
                }
                else
                {
                    IsSchemeValidate = true;
                }
                //              bool IsSchemeValidate = ValidateSchemeQty(SelectedSchemeCode);
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
                            cmd = new SqlCommand("InsertSaleHeader");
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
                            cmd.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@RECID", "");
                            cmd.Parameters.AddWithValue("@SO_NO", SO_NO);
                            cmd.Parameters.AddWithValue("@PSR_CODE", strPsrName);
                            cmd.Parameters.AddWithValue("@DELIVERY_DATE", Convert.ToDateTime(txtDeliveryDate.Text).ToString("dd-MMM-yyyy"));
                            cmd.Parameters.AddWithValue("@SO_DATE", System.DateTime.Now.ToString("dd-MMM-yyyy"));
                            //cmd.Parameters.AddWithValue("@SO_VALUE", "5000");
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
                            cmd1 = new SqlCommand("InsertSaleLine");
                            cmd1.Connection = conn;
                            cmd1.Transaction = transaction;
                            cmd1.CommandTimeout = 3600;
                            cmd1.CommandType = CommandType.StoredProcedure;

                            int i = 0;
                            dtLineItems = (DataTable)Session["LineItem"];//sam
                            for (int j = 0; j < dtLineItems.Rows.Count; j++)//sam
                            {
                                cmd1.Parameters.Clear();//sam
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
                                cmd1.Parameters.AddWithValue("@LINE_NO", dtLineItems.Rows[j]["SNO"].ToString());
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
                            
                            foreach (GridViewRow grv in gvScheme.Rows)
                            {
                                if (((CheckBox)grv.FindControl("chkSelect")).Checked)
                                {
                            
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

                                    //boxqty = (pcsQty >= packSize ? Convert.ToInt32(pcsQty / packSize) : 0);
                                    boxqty = (pcsQty >= packSize ? Convert.ToInt32(Math.Floor(pcsQty / packSize)) : 0);
                                    pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text) - (boxqty * packSize);

                                    boxqty = boxqty + Convert.ToInt32(txtQtyToAvail.Text);
                                    billQty = Math.Round((boxqty + (pcsQty / packSize)), 2);// Math.Round(Convert.ToDecimal((boxqty + (pcsQty / packSize))), 2);
                                    boxPcs = boxqty.ToString() + "." + (pcsQty.ToString().Length == 1 ? "0" : "") + pcsQty.ToString();


                                    //string[] calValue = baseObj.CalculatePrice1(grv.Cells[4].Text, ddlCustomer.SelectedItem.Value, Global.ParseDecimal(grv.Cells[6].Text), "Box");
                                    string[] calValue = baseObj.CalculatePrice1(grv.Cells[4].Text, ddlCustomer.SelectedItem.Value, Convert.ToDecimal(billQty), "Box");

                                    i = i + 1;
                                    cmd1.Parameters.Clear();

                                    if (billQty > 0)
                                    {
                                        cmd1.Parameters.AddWithValue("@statusProcedure", "Insert");
                                        cmd1.Parameters.AddWithValue("@SO_NO", SO_NO);
                                        cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", ddlCustomer.SelectedItem.Value);
                                        cmd1.Parameters.AddWithValue("@RECID", "");
                                        cmd1.Parameters.AddWithValue("@LINE_NO", i);
                                        cmd1.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                                        cmd1.Parameters.AddWithValue("@PRODUCT_CODE", grv.Cells[4].Text);
                                        // cmd1.Parameters.AddWithValue("@BOX", grv.Cells[6].Text);
                                        //cmd1.Parameters.AddWithValue("@BOX", Convert.ToDecimal(txtQtyToAvail.Text));
                                        cmd1.Parameters.AddWithValue("@BOX", billQty);
                                        cmd1.Parameters.AddWithValue("@CRATES", Convert.ToDecimal(0));
                                        string strLtr = string.Empty;
                                        if (calValue[1] != null)
                                        {
                                            strLtr = calValue[1].ToString();
                                        }
                                        if (strLtr == "")
                                        {
                                            strLtr = "0";
                                        }
                                        //cmd1.Parameters.AddWithValue("@LTR", Convert.ToDecimal(strLtr));
                                        //cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(0));
                                        //cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(0));
                                        cmd1.Parameters.AddWithValue("@LTR", Convert.ToDecimal(calValue[1].ToString()));
                                        cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(grv.Cells[25].Text));
                                        cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(grv.Cells[16].Text));
                                        string strUOM = string.Empty;
                                        if (calValue[4] != null)
                                        {
                                            strUOM = calValue[4].ToString();
                                        }
                                        cmd1.Parameters.AddWithValue("@UOM", strUOM);
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
                                        //cmd1.Parameters.AddWithValue("@TAXCOMPONENT", grv.Cells[26].Text);
                                        //cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", grv.Cells[27].Text);
                                        cmd1.Parameters.AddWithValue("@TAXCOMPONENT", grv.Cells[27].Text.Replace("&nbsp;", ""));
                                        cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", grv.Cells[28].Text.Replace("&nbsp;", ""));

                                        cmd1.Parameters.AddWithValue("@SchemeDiscPer", Convert.ToDecimal(grv.Cells[18].Text));
                                        cmd1.Parameters.AddWithValue("@SchemeDiscVal", Convert.ToDecimal(grv.Cells[19].Text));
                                        cmd1.Parameters.AddWithValue("@DOCTYPE", 4);
                                        cmd1.ExecuteNonQuery();
                                    }
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
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Sale Order : " + SO_NO + " Generated Successfully.!');", true);

                            string message = "alert('Sale Order : " + SO_NO + " Generated Successfully.!');";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                            ResetAllControls();


                        }
                        else
                        {
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Add Line Items First !');", true);

                            string message = "alert('Please Add Line Items First !');";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                            return;
                        }
                    }
                }
                else
                {
                    string message = "alert('Free Quantity should be Equal !');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Free Quantity should be Equal !');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('Error:" + ex.Message + " !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Error:" + ex.Message + " !');", true);
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

        private bool ValidatePurchaseReturnHeaderData()
        {
            bool returnvalue = true;

            try
            {
                if (ddlCustomerGroup.SelectedItem.Value == "Select...")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer Group');", true);

                    string message = "alert('Select Customer Group');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                    returnvalue = false;
                }

                if (ddlPSRName.Visible == true)
                {
                    if (ddlPSRName.SelectedItem.Value == "Select..." || ddlPSRName.Text == string.Empty)
                    {
                       // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select PSR Name !');", true);

                        string message = "alert('Select PSR Name !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                        ddlPSRName.Focus();
                        returnvalue = false;

                    }
                    if (ddlBeatName.Text == string.Empty || ddlBeatName.SelectedIndex == -1 || ddlBeatName.SelectedItem.Text == "Select...")
                    {
                       // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Beat Name !');", true);

                        string message = "alert('Select Beat Name !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        ddlBeatName.Focus();
                        returnvalue = false;
                    }

                }

                //Customer code
                if (ddlCustomer.SelectedIndex == -1)
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer..');", true);
                    string message = "alert('Select Customer..');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                }
                else
                {
                    if (ddlCustomer.SelectedItem.Text == "Select...")
                    {
                       // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer..');", true);
                        string message = "alert('Select Customer..');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        returnvalue = false;
                    }
                }
                //Select Date

                if (txtDeliveryDate.Text == "")
                {
                   // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Date... !');", true);
                    string message = "alert('Select Date... !');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                }
                //  check grid item==========
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Error: " + ex.Message + " !');", true);
                string message = "alert('Error: " + ex.Message + " !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                returnvalue = false;
            }
            return returnvalue;
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
                strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'";
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
              //  strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'"; //--AND SITE_CODE='657546'";
                strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'" +
                           " Union  All " +
                           " Select Name=(Select A.Customer_Code+'-'+A.Customer_Name as Name  from ax.ACXCUSTMASTER A where A.Blocked = 0 and B.SubDistributor=A.Customer_Code AND A.CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' ),B.SubDistributor as Customer_Code " +
                           " from ax.ACX_SDLinking B Where B.Other_Site='" + Session["SiteCode"].ToString() + "'";
                ddlCustomer.Items.Clear();
                txtContactNo.Text = "";
                txtAddress.Text = "";
                ddlCustomer.Items.Add("Select...");
                baseObj.BindToDropDown(ddlCustomer, strQuery, "Name", "Customer_Code");

            }
        }

        protected void ddlPSRName_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = @"select BeatCode +'-'+BeatName as BeatName,BeatCode from [ax].[ACXPSRBeatMaster] where PSRCode='" + ddlPSRName.SelectedItem.Value + "'";
            ddlBeatName.Items.Clear();
            ddlBeatName.Items.Add("Select...");
            baseObj.BindToDropDown(ddlBeatName, strQuery, "BeatName", "BeatCode");
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Query = "select GSTINNO,GSTREGISTRATIONDATE,COMPOSITIONSCHEME,Address1,STATE,Mobile_No from VW_CUSTOMERGSTINFO where CUSTOMER_CODE='" + ddlCustomer.SelectedValue + "'";

            DataTable dt1 = baseObj.GetData(Query);
            if (dt1.Rows.Count > 0)
            {
                txtGSTtin.Text = dt1.Rows[0]["GSTINNO"].ToString();
                txtGSTtinRegistration.Text = dt1.Rows[0]["GSTREGISTRATIONDATE"].ToString();
                chkCompositionScheme.Checked = Convert.ToBoolean(dt1.Rows[0]["COMPOSITIONSCHEME"]);
                txtBilltoState.Text = dt1.Rows[0]["STATE"].ToString();
                txtAddress.Text = dt1.Rows[0]["Address1"].ToString();
                txtContactNo.Text = dt1.Rows[0]["Mobile_No"].ToString();
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

            Query = "EXEC USP_CUSTSHIPTOADDRESS '" + ddlCustomer.SelectedValue + "'";
            ddlShipToAddress.Items.Clear();
            baseObj.BindToDropDown(ddlShipToAddress, Query, "SHIPTOADDRESS", "SHIPTOADDRESS");

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
                Session["SoNo"] = null;
                dtLineItems = null;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gvScheme.DataSource = null;
                gvScheme.DataBind();
                //BindSchemeGrid();
            }
            else
            {
                strQuery = @"Select Ac.Mobile_No,Ac.ADDRESS1,Ac.PSR_CODE,AP.PSR_Name,APB.BeatName,APB.BeatCode "
                         + " from ax.ACXCUSTMASTER Ac "
                         + " left Outer Join [ax].[ACXPSRMaster] AP on Ac.PSR_CODE = AP.PSR_Code "
                         + " left Outer Join [ax].[ACXPSRBeatMaster] APB on AP.PSR_CODE = APB.PSRCode and Ac.PSR_BEAT = APB.BeatCode"
                         + " where Ac.Customer_Code ='" + ddlCustomer.SelectedValue + "' and Ac.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' ";

                DataTable dt = baseObj.GetData(strQuery);
                if (dt.Rows.Count > 0)
                {
                    txtContactNo.Text = dt.Rows[0]["Mobile_No"].ToString();
                    txtAddress.Text = dt.Rows[0]["Address1"].ToString();
                    ddlPSRName.Items.Clear();
                    ddlBeatName.Items.Clear();
                    ddlPSRName.Items.Add(new ListItem(dt.Rows[0]["PSR_CODE"].ToString() + "-" + dt.Rows[0]["PSR_Name"].ToString(), dt.Rows[0]["PSR_CODE"].ToString()));
                    ddlBeatName.Items.Add(new ListItem(dt.Rows[0]["BeatCode"].ToString() + "-" + dt.Rows[0]["BeatName"].ToString(), dt.Rows[0]["BeatCode"].ToString()));
                }
                else
                {
                    txtContactNo.Text = "";
                    txtAddress.Text = "";
                }
                ///////Vijay////////////////

                //AddColumnInDataTable();
                Session["LineItem"] = null;
                dtLineItems = null;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gvScheme.DataSource = null;
                gvScheme.DataBind();
            }
        }

        private void ResetAllControls()
        {
            Session["LineItem"] = null;
            Session["PreSoNO"] = null;
            gvDetails.DataSource = null;
            gvDetails.Visible = false;
            // FillMaterialGroup();
            //DDLMaterialCode.Items.Clear();
            txtQtyCrates.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtCrateQty.Text = string.Empty;
            txtPCSQty.Text = string.Empty;
            //txtCrateSize.Text = string.Empty;
            //txtPack.Text = string.Empty;
            txtValue.Text = string.Empty;

            txtBoxqty.Text = "";
            txtPCSQty.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";

            txtBoxqty.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtQtyBox.Text = "";
            txtBoxPcs.Text = "";
            txtLtr.Text = "";
            txtPrice.Text = "";
            txtValue.Text = "";

            gvScheme.DataSource = null;
            gvScheme.DataBind();

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
            strQuery = "Select distinct P.ITEMID+'-'+P.Product_Name as Name,P.ITEMID from ax.InventTable P where Product_Group='" + DDLProductGroup.SelectedValue + "' and P.PRODUCT_SUBCATEGORY ='" + DDLProductSubCategory.SelectedItem.Value + "' and block=0"; //--AND SITE_CODE='657546'";
            DDLMaterialCode.DataSource = null;
            DDLMaterialCode.Items.Clear();
            //DDLMaterialCode.Items.Add("Select...");
            baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Name", "ITEMID");

            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtCrateQty.Text = string.Empty;
            txtPCSQty.Text = string.Empty;
            //txtCrateSize.Text = string.Empty;
            //txtPack.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            //txtEnterQty.Text = string.Empty;
            DDLMaterialCode.Enabled = true;
            DDLMaterialCode.SelectedIndex = 0;

            //DDLMaterialCode_SelectedIndexChanged(sender, e);
            DDLMaterialCode.Focus();
            //DDLMaterialCode.Focus();
            //DDLMaterialCode.Attributes.Add("onChange", "location.href = this.options[this.selectedIndex].value;");

            PcsBillingApplicable();
        }

        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = sender.ToString();
            string b = e.ToString();
            //txtQtyBox.Focus();
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            //txtEnterQty.Text = string.Empty;
            //txtCrateSize.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtCrateQty.Text = string.Empty;
            txtPCSQty.Text = string.Empty;
            //txtPack.Text = string.Empty;

            txtBoxqty.Text = "";
            txtPCSQty.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            //===========Fill Product Group and Product Sub Cat========
            DataTable dt = new DataTable();
            string query = "select Product_Group,PRODUCT_SUBCATEGORY,cast(Product_PackSize as decimal(10,2)) as Product_PackSize,Cast(Product_Crate_PackSize as decimal(10,2)) as Product_CrateSize  from ax.INVENTTABLE where ItemId='" + DDLMaterialCode.SelectedItem.Value + "' order by Product_Name";
            dt = baseObj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DDLProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                ProductSubCategory();
                //txtPack.Text = dt.Rows[0]["Product_PackSize"].ToString();
                //txtCrateSize.Text = dt.Rows[0]["Product_CrateSize"].ToString();
                //=============For Product Sub Cat======
                DDLProductSubCategory.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
                //DDLProductSubCategory.DataSource = dt;
                //DDLProductSubCategory.DataTextField = "PRODUCT_SUBCATEGORY";
                //DDLProductSubCategory.DataValueField = "PRODUCT_SUBCATEGORY";
                //DDLProductSubCategory.DataBind();
            }

            //txtEnterQty.Focus();

            //DDLMaterialCode.Items.Clear();
            PcsBillingApplicable();
        }

        public void ProductSubCategory()
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                        + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "' and block=0";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            // FillProductCode();
            DDLProductSubCategory.Focus();
        }
        public void BindSchemeGrid()
        {
            Session[SessionScheme] = null;
            DataTable dt = GetDatafromSP(@"ACX_SCHEME");
            Session[SessionScheme] = dt;
            gvScheme.DataSource = dt;
            gvScheme.DataBind();

            
        }

        
        public DataTable GetDatafromSP(string SPName)//replaced
        {
            DataTable dt = new DataTable();
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
                               
                dt.Load(cmd.ExecuteReader());

                if (dt.Rows.Count > 0)
                {
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
                    dt.Columns["SCHBOX"].ReadOnly = false;//sam
                    dt.Columns["SCHPCS"].ReadOnly = false;//sam
                    dt.Columns["SCHVALUE"].ReadOnly = false;//sam
                    dt.Columns["MINBOX"].ReadOnly = false;//sam
                    dt.Columns["MINPCS"].ReadOnly = false;//sam

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["TotalFreeQty"] = Convert.ToInt16("0");
                        dr["TotalFreeQtyPCS"] = Convert.ToInt16("0");
                        dr["TotalSchemeValueoff"] = Convert.ToInt16("0");
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
                                dr["TotalSchemeValueoff"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));
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
                                dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                            }
                            if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                            {
                                dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTY"]));
                            }
                            if (dr["Schemetype"].ToString() == "3")
                            {
                                dr["TotalSchemeValueoff"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));
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
                                dr["TotalSchemeValueoff"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));
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
                    viewSchemeValueoff.RowFilter = "[Scheme Type]=2";
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
            }
            finally
            {

                conn.Close();
                conn.Dispose();
            }
            return dt;
        }
        public DataTable GetDatafromSPDiscount(string SPName, string strItem)
        {
            conn = baseObj.GetConnection();
            DataTable dt = new DataTable();
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

        //public Int16 GetQtyofGroupItem(string Group)
        //{
        //    DataTable dt = baseObj.GetData("select ITEMID from ax.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
        //    Int16 Qty = 0;
        //    foreach (DataRow dtdr in dt.Rows)
        //    {
        //        foreach (GridViewRow gvrow in gvDetails.Rows)
        //        {
        //            if (dtdr[0].ToString() == gvrow.Cells[3].Text)
        //            {
        //                Qty += Convert.ToInt16(gvrow.Cells[4].Text);
        //            }
        //        }
        //    }
        //    return Qty;
        //}

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
            foreach (DataRow dtdr in dt.Rows)
            {
                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    if (dtdr[0].ToString() == gvrow.Cells[3].Text)
                    {
                        if (BoxPCS == "BOX")
                        {
                            Qty += Convert.ToInt16(Convert.ToDouble(gvrow.Cells[6].Text));
                        }
                        else
                        {
                            Qty += Convert.ToInt16(Convert.ToDouble(gvrow.Cells[7].Text));
                        }
                    }
                }
            }
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
            foreach (DataRow dtdr in dt.Rows)
            {
                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    if (dtdr[0].ToString() == gvrow.Cells[3].Text)
                    {
                        Value += Convert.ToDecimal(gvrow.Cells[gcAmount].Text);
                    }
                }
            }
            return Value;
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
        //public Int16 GetQtyofItem(string Item)
        //{
        //    Int16 Qty = 0;
        //    foreach (GridViewRow gvrow in gvDetails.Rows)
        //    {
        //        if (gvrow.Cells[3].Text == Item)
        //        {
        //            Qty = Convert.ToInt16(gvrow.Cells[4].Text);
        //        }
        //    }
        //    return Qty;
        //}

        public Int16 GetQtyofItem(string Item, string BoxPCS)
        {
            Int16 Qty = 0;
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                if (gvrow.Cells[3].Text == Item)
                {
                    if (BoxPCS == "BOX")
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(gvrow.Cells[6].Text));
                    }
                    else
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(gvrow.Cells[7].Text));
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
                    Value = Convert.ToDecimal(gvrow.Cells[gcAmount].Text);
                }
            }
            return Value;
        }

        //public Int16 GetMaxQtyofItem()
        //{
        //    Int16 Qty = 0;
        //    Int16[] arrQty = new Int16[gvDetails.Rows.Count];
        //    foreach (GridViewRow gvrow in gvDetails.Rows)
        //    {
        //        arrQty[gvrow.RowIndex] = Convert.ToInt16(gvrow.Cells[4].Text);
        //    }
        //    Qty = arrQty.Max();
        //    return Qty;
        //}

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
                        arrQty[gvrow.RowIndex] = Convert.ToInt16(Convert.ToDecimal(gvrow.Cells[6].Text));
                    }
                    else
                    {
                        arrQty[gvrow.RowIndex] = Convert.ToInt16(Convert.ToDecimal(gvrow.Cells[7].Text));
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
                arrValue[gvrow.RowIndex] = Convert.ToDecimal(gvrow.Cells[gcAmount].Text);
            }
            if (arrValue.Length > 0)
            { Value = arrValue.Max(); }
            return Value;
        }

        //private bool GetPrevSelection(string SchemeCode, string SetNo)
        //{
        //    foreach (GridViewRow rw in gvScheme.Rows)
        //    {
        //        CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
        //        if (chkBx.Checked && rw.Cells[1].Text != SchemeCode)
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

        //protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox activeCheckBox = sender as CheckBox;
        //    GridViewRow row1 = (GridViewRow)activeCheckBox.Parent.Parent;
        //    String SchemeCode1 = row1.Cells[1].Text;
        //    String SetNo1 = row1.Cells[7].Text;
        //    TextBox txtQty = (TextBox)row1.FindControl("txtQty");
        //    TextBox txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");

        //    if (Global.GetPrevSelection(SchemeCode1, SetNo1, ref gvScheme,"0")) { activeCheckBox.Checked = false; return; }
        //    #region For Selection
        //    if (activeCheckBox.Checked)
        //    {

        //        GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
        //        string SchemeCode = row.Cells[1].Text;
        //        string SetNo = row.Cells[7].Text;
        //        foreach (GridViewRow rw in gvScheme.Rows)
        //        {
        //            #region Same Scheme Validation 1st Check
        //            //CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
        //            //if (chkBx.Checked)
        //            //{
        //            //    if (rw.Cells[1].Text != SchemeCode)
        //            //    {
        //            //        activeCheckBox.Checked = false;

        //            //        string message = "alert('You can select only one scheme items !');";
        //            //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
        //            //        return;
        //            //    }
        //            //    else if (rw.Cells[1].Text == SchemeCode && SetNo != rw.Cells[7].Text)
        //            //    {
        //            //        activeCheckBox.Checked = false;

        //            //        string message = "alert('You can select only one scheme items with same set!');";
        //            //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
        //            //        return;
        //            //    }
        //            //}
        //            #endregion

        //            #region 2nd Check Same scheme and same set setno > 0 Auto select all scheme
        //            CheckBox chkBxNew = (CheckBox)rw.FindControl("chkSelect");  //Auto Select All Same Set No
        //            if (Convert.ToString(rw.Cells[7].Text) == SetNo && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(SetNo) > 0)
        //            {

        //                chkBxNew.Checked = true;
        //                txtQty = (TextBox)rw.FindControl("txtQty");
        //                txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
        //                HiddenField hdnTotalFreeQty = (HiddenField)rw.FindControl("hdnTotalFreeQty");
        //                HiddenField hdnTotalFreeQtyPcs = (HiddenField)rw.FindControl("hdnTotalFreeQtyPcs");
        //                txtQty.Text = hdnTotalFreeQty.Value;
        //                txtQtyPcs.Text = hdnTotalFreeQtyPcs.Value;
        //                txtQty.Enabled = false;
        //                txtQtyPcs.Enabled = false;
        //            }

        //            #endregion
        //        }
        //        foreach (GridViewRow rw in gvScheme.Rows)
        //        {
        //            CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
        //            txtQty = (TextBox)rw.FindControl("txtQty");
        //            if (chkBx.Checked)
        //            {
        //                txtQty.ReadOnly = false;
        //            }
        //            else
        //            {
        //                txtQty.Text = string.Empty;
        //                txtQty.ReadOnly = true;
        //            }
        //        }
        //        #endregion

        //        #region  checkbox enable for set no 0 only

        //        if (SetNo1 == "0")
        //        {
        //            txtQty = (TextBox)row1.FindControl("txtQty");
        //            txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
        //            HiddenField hdnTotalFreeQty = (HiddenField)row1.FindControl("hdnTotalFreeQty");
        //            HiddenField hdnTotalFreeQtyPcs = (HiddenField)row1.FindControl("hdnTotalFreeQtyPcs");

        //            if (hdnTotalFreeQty.Value != "")
        //            { txtQty.ReadOnly = false; }
        //            else { txtQty.ReadOnly = true; }

        //            if (Session["ApplicableOnState"].ToString() == "Y")
        //            {
        //                if (hdnTotalFreeQtyPcs.Value != "")
        //                {
        //                    txtQtyPcs.ReadOnly = false;
        //                }
        //                else
        //                {
        //                    txtQtyPcs.ReadOnly = true;
        //                }
        //            }
        //            else { txtQtyPcs.ReadOnly = true; }
        //        }
        //        #endregion
        //    }

        //    else
        //    {
        //        #region  For unchecked
        //        foreach (GridViewRow rw in gvScheme.Rows)
        //        {
        //            GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
        //            string SchemeCode = row.Cells[1].Text;
        //            string SetNo = row.Cells[7].Text;
        //            if (SchemeCode == SchemeCode1 && SetNo == SetNo1 && Convert.ToInt16(SetNo) > 0)
        //            {

        //                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
        //                chkBx.Checked = false;
        //                txtQty = (TextBox)rw.FindControl("txtQty");
        //                txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
        //                HiddenField hdnTotalFreeQty = (HiddenField)rw.FindControl("hdnTotalFreeQty");
        //                HiddenField hdnTotalFreeQtyPcs = (HiddenField)rw.FindControl("hdnTotalFreeQtyPcs");
        //                txtQty.Text = "";
        //                txtQtyPcs.Text = "";
        //            }
        //        }
        //        if (SetNo1 == "0")
        //        {
        //            txtQty = (TextBox)row1.FindControl("txtQty");
        //            txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
        //            txtQty.Text = "";
        //            txtQtyPcs.Text = "";
        //        }
        //        #endregion
        //    }

        //    if (activeCheckBox.Checked)
        //    {
        //        txtQty.Enabled = true;
        //        txtQtyPcs.Enabled = true;
        //    }
        //    else
        //    {
        //        txtQty.Enabled = false;
        //        txtQtyPcs.Enabled = false;
        //        txtQty.Text = "0";
        //        txtQtyPcs.Text = "0";
        //    }
        //    SchemeDiscCalculation();
        //}

        
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
                    //if (chkBx.Checked == true)
                    //{
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = (txtQty.Text.Trim().Length == 0 ? "0" : txtQty.Text);
                    txtQtyPCS.Text = (txtQtyPCS.Text.Trim().Length == 0 ? "0" : txtQtyPCS.Text);
                    packSize = (rw.Cells[13].Text == "" ? 0 : Convert.ToDecimal(rw.Cells[13].Text));
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
                    //}
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
                decimal AddSchemeDiscount = 0;//sam
                decimal AddSchemePerc = 0;//sam
                decimal AddSchemeValue = 0;//sam

                dtLineItems.Columns["Tax_Amount"].ReadOnly = false;
                dtLineItems.Columns["AddTax_Amount"].ReadOnly = false;
                dtLineItems.Columns["TaxableAmount"].ReadOnly = false;
                dtLineItems.Columns["Value"].ReadOnly = false;

                dtLineItems.Columns["ADDSCHDISCPER"].ReadOnly = false;//sam
                dtLineItems.Columns["ADDSCHDISCVAL"].ReadOnly = false;//sam 
                dtLineItems.Columns["ADDSCHDISCAMT"].ReadOnly = false;//sam


                Boolean SchemeDiscApply = false, AddSchemeDiscApply = false;
                string SchSrlNo = "0", AddSchemeGroup = "", AddSchSrlNo = "0";
                SchemeDiscApply = AddSchemeDiscApply = false;

                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    HiddenField hdnSchemetype = (HiddenField)rw.FindControl("hdnSchemetype");
                    HiddenField hdnSchSrlNo = (HiddenField)rw.FindControl("hdnSchSrlNo");
                    Label lblSchemeDiscPer = (Label)rw.FindControl("lblSchemeDiscPer");
                    HiddenField hdnAddSchType = (HiddenField)rw.FindControl("hdnAddSchType");//sam;
                    Label lblAddSchemePer = (Label)rw.FindControl("lblAddSchemePer");//sam
                    Label lblAddSchemeVal = (Label)rw.FindControl("lblAddSchemeVal");//sam
                    Label lblAddSchemeGroup = (Label)rw.FindControl("lblAddSchemeGroup");//sam
                    HiddenField hdntotSchemeValueoff = (HiddenField)rw.FindControl("hdntotSchemeValueoff");
                    if (chkBx.Checked && hdnAddSchType.Value != "0")//sam
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
                    if (chkBx.Checked && hdnSchemetype.Value.ToString() == "3")//sam
                    {
                        SchemeDiscApply = true;
                        #region Scheme Basic Value Check
                        DataTable dtSchValue = new DataTable();
                        if (Session[SessionScheme] != null)
                        { dtSchValue = (DataTable)Session[SessionScheme]; }
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

                                DiscPer = Convert.ToDecimal(lblSchemeDiscPer.Text.Trim() == "" ? 0 : Convert.ToDecimal(lblSchemeDiscPer.Text)) / lineAmount;
                                //DiscPer = DiscPer * 100;
                            }

                            break;
                        }
                        #endregion
                    }
                }
                if (AddSchemeDiscApply == true && (AddSchemePerc > 0 || AddSchemeValue > 0))//sam
                {
                    #region Apply Additional Scheme of Percent or Value Off
                    DataTable dtAddSch = new DataTable();
                    if (Session[SessionScheme] != null)
                    { dtAddSch = (DataTable)Session[SessionScheme]; }

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

                                        dtLineItems.Rows[i]["ADDSCHDISCPER"] = AddSchemePerc;
                                        dtLineItems.Rows[i]["ADDSCHDISCVAL"] = AddSchemeValue;
                                        if (AddSchemePerc > 0)
                                        { dtLineItems.Rows[i]["ADDSCHDISCAMT"] = Math.Round((lineAmount * (AddSchemePerc / 100)), 6); }
                                        if (AddSchemeValue > 0)
                                        {
                                            dtLineItems.Rows[i]["ADDSCHDISCAMT"] = Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * AddSchemeValue;
                                        }
                                        //dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString()) - Convert.ToDecimal(dtLineItems.Rows[i]["SchemeDiscVal"]) - Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"])).ToString("0.00");
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
                        SchemeDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SchemeDisc"].ToString());
                        AddSchemeDiscount = 0;
                        dtLineItems.Rows[i]["ADDSCHDISCPER"] = AddSchemePerc;//sam
                        dtLineItems.Rows[i]["ADDSCHDISCVAL"] = AddSchemeValue;//sam
                        dtLineItems.Rows[i]["ADDSCHDISCAMT"] = AddSchemeDiscount;//sam
                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - DiscAmount - SchemeDiscAmount - AddSchemeDiscount).ToString("0.000000");
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
                        TaxableAmount = lineAmount - DiscAmount - SchemeDiscAmount - AddSchemeDiscount;
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
                    DataTable dtSch = new DataTable();
                    if (Session[SessionScheme] != null)
                    { dtSch = (DataTable)Session[SessionScheme]; }

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
                                        dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                                        SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - DiscAmount - SchemeDiscAmount - Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"])).ToString("0.00");
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
                                        TaxableAmount = lineAmount - DiscAmount - SchemeDiscAmount - Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"]); ;
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
                                gvDetails.DataSource = dtLineItems;
                                gvDetails.DataBind();
                                GridViewFooterCalculate(dtLineItems);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dtLineItems.Rows.Count; i++)
                    {
                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Price"].ToString());
                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                        dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                        SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                        AddSchemeDiscount = Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"]);//sam
                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - DiscAmount - SchemeDiscAmount - Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"])).ToString("0.00");//sam
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
                        TaxableAmount = lineAmount - DiscAmount - SchemeDiscAmount - AddSchemeDiscount;
                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                        dtLineItems.Rows[i]["Tax_Amount"] = TaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["AddTax_Amount"] = AddTaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["Value"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                    }
                    dtLineItems.AcceptChanges();
                    Session["LineItem"] = dtLineItems;
                    gvDetails.DataSource = dtLineItems;
                    gvDetails.DataBind();
                    GridViewFooterCalculate(dtLineItems);
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
        protected void txtQtyBox_TextChanged1(object sender, EventArgs e)
        {

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

        //protected void txtQty_TextChanged(object sender, EventArgs e)
        //{
        //    int TotalQty = 0;

        //    GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
        //    string SchemeCode = row.Cells[1].Text;
        //    int AvlFreeQty = Convert.ToInt16(row.Cells[8].Text);
        //    int Slab = Convert.ToInt16(row.Cells[6].Text);

        //    foreach (GridViewRow rw in gvScheme.Rows)
        //    {
        //        CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
        //        TextBox txtQty = (TextBox)rw.FindControl("txtQty");
        //        if (chkBx.Checked == true)
        //        {
                    
        //            if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab)
        //            {
        //                TotalQty += Convert.ToInt16(txtQty.Text);
        //            }

        //            if (TotalQty > AvlFreeQty)
        //            {
        //                txtQty.Text = string.Empty;
        //                string message = "alert('Free Qty should not greater than available free qty !');";
        //                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
        //                return;
        //            }

        //        }
        //        else
        //        {
        //            txtQty.Text = "0";
        //        }

        //    }

        //}

        protected void gvScheme_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void FillProductCode()
        {
            DDLMaterialCode.Items.Clear();
            // DDLMaterialCode.Items.Add("Select...");
            if (DDLProductGroup.Text == "Select..." && DDLProductSubCategory.Text == "Select..." || DDLProductSubCategory.Text == "")
            {
                strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE where block=0  order by Product_Name";
                DDLMaterialCode.Items.Clear();
                DDLMaterialCode.Items.Add("Select...");
                baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Product_Name", "Product_Code");
                DDLMaterialCode.Focus();
            }
        }

        public void CalculateQtyAmt(Object sender)
        {
//                string query = @"select I.ITEMID,t.AMOUNT,I.PRODUCT_PACKSIZE,I.PRODUCT_CRATE_PACKSIZE,I.LTR from ax.InventTable I
//                                Inner join DBO.PriceDiscTable t on I.ITEMID =t.ITEmRelation
//                                where ACCOUNTRELATION = (select PriceGroup from ax.ACXCUSTMASTER where CUSTOMER_CODE = '" + ddlCustomer.SelectedItem.Value + "') "
//                                  + " and I.ITEMID ='" + DDLMaterialCode.SelectedItem.Value + "'  ";
                string query = "EXEC ax.ACX_GetProductRate '" + DDLMaterialCode.SelectedItem.Value + "','" + ddlCustomer.SelectedItem.Value + "'";

                DataTable dtItem = new DataTable();
                dtItem = baseObj.GetData(query);
                if (dtItem.Rows.Count > 0)
                {
                    //if (Convert.ToDecimal(dtItem.Rows[0]["AMOUNT"].ToString()) <= 0)
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_MRP"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Product Price !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Product Pack Size !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Product Crate PackSize !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    if (Convert.ToDecimal(dtItem.Rows[0]["LTR"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Ltr !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        return;
                    }

                    decimal dblTotalQty = 0, crateQty = 0, boxQty = 0, pcsQty = 0, crateSize = 0, pacSize = 0;

                    crateQty = App_Code.Global.ParseDecimal(txtCrateQty.Text);
                    boxQty = App_Code.Global.ParseDecimal(txtBoxqty.Text);
                    pcsQty = App_Code.Global.ParseDecimal(txtPCSQty.Text);
                    crateSize = App_Code.Global.ParseDecimal(dtItem.Rows[0]["PRODUCT_CRATE_PACKSIZE"].ToString());
                    pacSize = App_Code.Global.ParseDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString());

                    dblTotalQty = crateQty * crateSize + boxQty + (pcsQty / (pacSize == 0 ? 1 : pacSize));  // crateQty * crateSize + boxQty + (pcsQty / (boxSize == 0 ? 1 : boxSize));
                    //txtEnterQty.Text = dblTotalQty.ToString();
                    txtQtyBox.Text = dblTotalQty.ToString();

                    decimal TotalBox = 0, TotalPcs = 0;
                    string BoxPcs = "";
                    TotalBox = Math.Truncate(dblTotalQty);                     //Extract Only Box Qty From Total Qty
                    TotalPcs = Math.Round((dblTotalQty - TotalBox) * pacSize); //Extract Only Pcs Qty From Total Qty
                    BoxPcs = Convert.ToString(TotalBox) + '.' + (Convert.ToString(TotalPcs).Length == 1 ? "0" : "") + Convert.ToString(TotalPcs);

                try
                {

                    string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, ddlCustomer.SelectedItem.Value, dblTotalQty, "Box");
                    if (calValue[0] != "")
                    {
                        txtQtyCrates.Text = calValue[0];
                        txtLtr.Text = calValue[1];
                        txtPrice.Text = calValue[2];
                        txtValue.Text = calValue[3];
                        lblHidden.Text = calValue[4];
                        txtBoxPcs.Text = BoxPcs;
                        txtViewTotalBox.Text = TotalBox.ToString("0.00");
                        txtViewTotalPcs.Text = TotalPcs.ToString("0.00");
                        txtQtyBox.Text = dblTotalQty.ToString("0.00");

                        SetFocus(BtnAddItem);
                    }
                    else
                    {
                        lblMessage.Text = "Price Not Define !";
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    string message = "alert('" + ex.Message.ToString() + "');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/frmSOCreationIndent.aspx");
        }

        protected void TextCrateQty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
        }

        protected void txtPCSQty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
        }

        protected void gvScheme_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvScheme_RowDataBound1(object sender, GridViewRowEventArgs e)
        {

        }

        protected void txtQtyPcs_TextChanged(object sender, EventArgs e)
        {
            int TotalQtyPcs = 0;

            GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
            string SchemeCode = row.Cells[1].Text;
            int AvlFreeQtyPcs = Convert.ToInt16(row.Cells[11].Text);
            int Slab = Convert.ToInt16(row.Cells[6].Text);

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                if (chkBx.Checked == true)
                {
                    //For Pcs                 
                    if (!string.IsNullOrEmpty(txtQtyPcs.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab)
                    {
                        TotalQtyPcs += Convert.ToInt16(txtQtyPcs.Text);
                    }
                    if (TotalQtyPcs > AvlFreeQtyPcs)
                    {
                        txtQtyPcs.Text = string.Empty;
                        chkBx.Checked = false;
                        txtQtyPcs.ReadOnly = false;
                        string message = "alert('Free Qty Pcs should not greater than available free qty pcs !');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        return;
                    }
                }
                else
                {
                    txtQtyPcs.Text = "0";
                }
            }
        }

        protected void txtBoxqty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
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
                txtPCSQty.Enabled = true;
            }
            else
            {
                txtPCSQty.Enabled = false;
            }
        }

    }
}