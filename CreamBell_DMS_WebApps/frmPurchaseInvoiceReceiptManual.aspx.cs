using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPurchaseInvoiceReceiptManual : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        public DataTable dtLineItems;

        SqlConnection conn = null;
        SqlCommand cmd, cmd1, cmd2;
        SqlTransaction transaction;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();
        static string SessionPendingInv = "SessionPendingInv";
        SqlDataAdapter da = new SqlDataAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {
            LblMessage.Text = "";
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!Page.IsPostBack)
            {
                Session["PreDocument_No"] = null;
                DrpSalesInvoice.Visible = false;
                txtInvoiceNo.Visible = true;
                //txtInvoiceNo.ReadOnly = true;
                string datestring = DateTime.Now.ToString("yyyy-MM-dd");
                txtReceiptDate.Text = datestring;
                txtInvoiceDate.Text = datestring;
                //CalendarExtender3.StartDate = DateTime.Now;
                imgBtnGetInvoiceData.Visible = false;
                Session[SessionPendingInv] = null;
                FillIndentNo();
                FillPendingSalesInvoice();
                Session["LineItem"] = null;
                BtnUpdateHeader.Visible = false;

                txtIndentDate.Attributes.Add("readonly", "readonly");
                txtInvoiceDate.Attributes.Add("readonly", "readonly");

                FillState();
                FillOrderType();

                if (rdoSAPFetchData.Checked != true)
                {
                    imgBtnGetInvoiceData.Visible = false;
                }

                if (Request.QueryString["PreNo"] != null)
                {
                    GetUnPostedReferenceData(Request.QueryString["PreNo"]);
                }
                rdoManualEntry_CheckedChanged(null, null);
            }


        }

        private void FillOrderType()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string strQuery = string.Empty;
            DataTable dt = new DataTable();
            //if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            //{
            strQuery = "SELECT  B.USERCODE,A.ORDERTYPE, (A.ORDERTYPE + ' - ' + A.ORDERDESCRIPTION) AS ORDERTYPEDESCRIPTION FROM ax.ACX_ORDERPLANTMAPPING A "
                        + "Left JOIN AX.Acx_PACUserCreation B ON A.ORDERTYPE = B.ORDERTYPE ";
            //+ " INNER JOIN AX.ACXUSERMASTER C ON C.USER_CODE = B.USERCODE";
            //+ " WHERE C.user_code = '" + Session["USERID"].ToString() + "'";
            //}
            //else
            // {
            //     strQuery = "SELECT ORDERTYPE, (ORDERTYPE + ' - ' + ORDERDESCRIPTION) AS ORDERTYPEDESCRIPTION FROM AX.ACX_ORDERPLANTMAPPING ORDER BY ORDERTYPE";

            // }
            dt = obj.GetData(strQuery);
            drpOrderType.Items.Clear();
            DataTable dtunique = new DataTable();
            dtunique = dt.DefaultView.ToTable(true, "ORDERTYPEDESCRIPTION", "ORDERTYPE");
            drpOrderType.DataSource = dtunique;
            drpOrderType.DataTextField = "ORDERTYPEDESCRIPTION";
            drpOrderType.DataValueField = "ORDERTYPE";
            drpOrderType.DataBind();
            DataRow[] dr = dt.Select("USERCODE = '" + Session["USERID"].ToString() + "'");
            if (dr.Length > 0)
            {
                drpOrderType.SelectedValue = dr[0]["ORDERTYPE"].ToString();
            }

            //drpOrderType.Items.Add("-Select-");
            //obj.BindToDropDown(drpOrderType, strQuery, "ORDERTYPEDESCRIPTION", "ORDERTYPE");

            FillPlantByOrderType();

        }
        private void FillPlantByOrderType()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string strQuery = string.Empty;
            DataTable dt = new DataTable();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                strQuery = "SELECT P.USERCODE USER_CODE,O.PLANTCODE,(O.PLANTCODE+'-'+O.PLANTDESCRIPTION) AS PLANTDESCRIPTION FROM AX.ACX_ORDERPLANTMAPPING O "
                              + " LEFT JOIN AX.Acx_PACUserCreation P ON O.PLANTCODE=P.PLANTCODE "
                            // + " INNER JOIN AX.ACXUSERMASTER A ON A.USER_CODE=P.USERCODE"
                            //+ " WHERE A.User_Code='" + Session["USERID"].ToString() + "'";
                            + " Where O.ORDERTYPE='" + drpOrderType.SelectedItem.Value.ToString() + "'";
            }
            else
            {
                strQuery = "EXEC ACX_ORDERTYPE_PLANT '" + drpOrderType.SelectedItem.Value.ToString() + "'";

            }

            dt = obj.GetData(strQuery);
            DrpPlant.Items.Clear();
            DataTable dtunique = new DataTable();
            dtunique = dt.DefaultView.ToTable(true, "PLANTDESCRIPTION", "PLANTCODE");
            DrpPlant.DataSource = dtunique;
            DrpPlant.DataTextField = "PLANTDESCRIPTION";
            DrpPlant.DataValueField = "PLANTCODE";
            DrpPlant.DataBind();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                DataRow[] dr = dt.Select("USER_CODE = '" + Session["USERID"].ToString() + "'");
                if (dr.Length > 0)
                {
                    DrpPlant.SelectedValue = dr[0]["PLANTCODE"].ToString();
                }
            }

            //DrpPlant.Items.Clear();
            //DrpPlant.Items.Add("-Select-");
            // obj.BindToDropDown(DrpPlant, strQuery, "PLANTDESCRIPTION", "PLANTCODE");
            DrpPlant.Focus();
        }
        protected void drpOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPlantByOrderType();
        }

        protected void FillState()
        {
            DrpState.Items.Clear();
            //string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
            string sqlstr11 = "Select Distinct LS.STATECODE_IT Code,LS.Name from [ax].[LOGISTICSADDRESSSTATE] LS WHERE LS.STATECODE_IT<>''  ";
            DataTable dt = baseObj.GetData(sqlstr11);
            DrpState.Items.Add("-Select-");
            //DrpState.Items.Add("All...");
            baseObj.BindToDropDown(DrpState, sqlstr11, "Name", "Code");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]


        public static List<string> GetProductDescription(string prefixText)
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string query = "Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY from ax.INVENTTABLE where " +
                           "replace(replace(ITEMID, char(9), ''), char(13) + char(10), '') Like @ProductCode+'%'";

            SqlConnection conn = obj.GetConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductCode", prefixText);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> ProductDetails = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductDetails.Add(dt.Rows[i]["ITEMID"].ToString());
            }
            return ProductDetails;
        }

        protected void BtnGetProductDetails_Click(object sender, EventArgs e)
        {
            if (txtProductCode.Text != string.Empty)
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                string queryFillProductDetails = "Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY," +
                                                 " CAST(ROUND(PRODUCT_MRP,2) as NUMERIC(10,2)) as PRODUCT_MRP ,CAST(ROUND(LTR,2) as NUMERIC(10,2)) as LTR, " +
                                                 " CAST(ROUND(NETWEIGHTPCS,2) as NUMERIC(10,2)) as NETWEIGHTPCS " +
                                                 "from ax.INVENTTABLE where replace(replace(ITEMID, char(9), ''), char(13) + char(10), '')= '" + txtProductCode.Text.Trim().ToString() + "'";

                DataTable dtProductDetails = obj.GetData(queryFillProductDetails);

                //string taxdata = "[USP_ACX_GetProductGSTRate] " + "'" + "'"

                ResetProductdetails();
                if (dtProductDetails.Rows.Count > 0 && dtProductDetails.Rows.Count == 1)
                {
                    txtProductDesc.Text = dtProductDetails.Rows[0]["PRODUCT_NAME"].ToString();
                    txtMRP.Text = dtProductDetails.Rows[0]["PRODUCT_MRP"].ToString();
                    txtWeight.Text = dtProductDetails.Rows[0]["NETWEIGHTPCS"].ToString();
                    txtVolume.Text = dtProductDetails.Rows[0]["LTR"].ToString();
                    txtRate.Text = obj.GetScalarValue("select dbo.ACXSTATEPRICE('" + Session["SiteCode"].ToString() + "'," + txtProductCode.Text.Trim().ToString() + ",0)");
                    Session["Weight"] = dtProductDetails.Rows[0]["NETWEIGHTPCS"].ToString();
                    Session["Volume"] = dtProductDetails.Rows[0]["LTR"].ToString();
                    DrpState_SelectedIndexChanged(null, null);
                    DataTable dtTax = obj.GetData("[USP_ACX_GetProductGSTRate] '" + txtProductCode.Text.Trim() + "', '" + DrpState.SelectedValue.ToString() + "', '" + Session["SITELOCATION"] + "'");
                    if (dtTax.Rows.Count > 0)
                    {
                        #region Tax Compenent
                        if (dtTax.Rows[0]["RETMSG"].ToString().IndexOf("TRUE") >= 0)
                        {
                            if (dtTax.Rows[0]["Exempt"].ToString() == "1" || txtGSTNNumber.Text.Trim().ToString() == "" || DrpCompScheme.SelectedValue == "1")
                            {
                                txtCGSTPerc.Text = txtIGSTPerc.Text = txtUGSTPerc.Text = txtSGSTPerc.Text = "0";
                                txtCGSTValue.Text = txtIGSTValue.Text = txtUGSTValue.Text = txtSGSTValue.Text = "0";
                            }
                            else if (dtTax.Rows[0]["Tax1Component"].ToString() == "IGST")
                            {
                                txtIGSTPerc.Text = dtTax.Rows[0]["Tax1_PER"].ToString();
                                txtCGSTPerc.Text = txtUGSTPerc.Text = txtSGSTPerc.Text = "0";
                                txtCGSTValue.Text = txtUGSTValue.Text = txtSGSTValue.Text = "0";
                            }
                            else if (dtTax.Rows[0]["Tax1Component"].ToString() == "UGST")
                            {
                                txtUGSTPerc.Text = dtTax.Rows[0]["Tax1_PER"].ToString();
                                txtCGSTPerc.Text = dtTax.Rows[0]["Tax2_PER"].ToString();
                                txtIGSTPerc.Text = txtSGSTPerc.Text = "0";
                                txtIGSTValue.Text = txtSGSTValue.Text = "0";
                            }
                            else if (dtTax.Rows[0]["Tax1Component"].ToString() == "SGST")
                            {
                                txtSGSTPerc.Text = dtTax.Rows[0]["Tax1_PER"].ToString();
                                txtCGSTPerc.Text = dtTax.Rows[0]["Tax2_PER"].ToString();
                                txtIGSTPerc.Text = txtUGSTPerc.Text = "0";
                                txtIGSTValue.Text = txtUGSTValue.Text = "0";
                            }
                            else
                            {
                                txtCGSTPerc.Text = txtIGSTPerc.Text = txtUGSTPerc.Text = txtSGSTPerc.Text = "0";
                                txtCGSTValue.Text = txtIGSTValue.Text = txtUGSTValue.Text = txtSGSTValue.Text = "0";
                            }
                        }
                        else
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + dtTax.Rows[0]["RETMSG"].ToString() + " !');", true);
                            LblMessage.Text = dtTax.Rows[0]["RETMSG"].ToString();
                            ResetProductdetails();
                        }
                        #endregion
                    }
                }

                if (dtProductDetails.Rows.Count > 1)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Product Code Issue: We Have Duplicate Records for this Product Code !');", true);

                    //string message = "alert('Product Code Issue: We Have Duplicate Records for this Product Code !');";
                    //ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                }
                if (dtProductDetails.Rows.Count == 0)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Product Code Issue: No Such Produt Code Exist !');", true);
                    //string message = "alert('Product Code Issue: No Such Produt Code Exist !');";
                    // ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                }

            }
            else
            {
                txtProductCode.Focus();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Product Code Here !');", true);

                //string message = "alert('Please Provide Product Code Here !');";
                //ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
            }
        }

        private void ResetProductdetails()
        {
            txtTRDDiscPerc.Text = txtUGSTPerc.Text = txtIGSTPerc.Text = txtSGSTPerc.Text = txtCGSTPerc.Text = txtSDPerc.Text = "0";
            txtTRDpercValue.Text = txtUGSTValue.Text = txtIGSTValue.Text = txtSGSTValue.Text = txtCGSTValue.Text = txtSDValue.Text = "0";
            txtRate.Text = txtPriceEqualValue.Text = txtGrossRate.Text = txtTotalValue.Text = txtMRP.Text = txtEntryValue.Text = txtWeight.Text = txtVolume.Text = "0";
            txtRemark.Text = "";
        }
        private bool ValidateManualEntry()
        {
            bool b = false;

            //if (DrpIndentNo.Text == "-Select-")
            //{
            //    LblMessage.Text = "► Please Select Indent No.";
            //    DrpIndentNo.Focus();
            //    b = false;
            //    return b;
            //}
            ////if (txtIndentDate.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Select Indent Date.";
            //    txtIndentDate.Focus();
            //    b = false;
            //    return b;
            //}

            if (rdoSAPFetchData.Checked)
            {
                if (DrpSalesInvoice.SelectedValue == string.Empty)
                {
                    LblMessage.Text = "► Please Provide Invoice No.";
                    DrpSalesInvoice.Focus();
                    b = false;
                    return b;
                }
            }
            else
            {
                if (txtInvoiceNo.Text == "")
                {
                    LblMessage.Text = "► Please Provide Invoice No.";
                    txtInvoiceNo.Focus();
                    b = false;
                    return b;
                }
            }
            if (txtInvoiceDate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice Date.";
                txtInvoiceDate.Focus();
                b = false;
                return b;
            }
            if (DrpState.SelectedItem.Text == "-Select-")
            {
                LblMessage.Text = "► Please Select State.";
                DrpState.Focus();
                b = false;
                return b;
            }
            if (txtvehicleNo.Text == string.Empty)
            {
                LblMessage.Text = "► Please Enter Vehicle No.";
                txtvehicleNo.Focus();
                b = false;
                return b;
            }
            if (txtAddr.Text == string.Empty)
            {
                LblMessage.Text = "► Please Enter Plant Address.";
                txtAddr.Focus();
                b = false;
                return b;
            }
            //if(DrpCompScheme.SelectedItem.Text == "--Select--")
            //{
            //    LblMessage.Text = "► Please Select Composition Scheme.";
            //    DrpCompScheme.Focus();
            //    b = false;
            //    return b;
            //}
            //if(txtGSTRegistrationDate.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide GST Registration Date.";
            //    txtGSTRegistrationDate.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtTransporterName.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide Transporter Name.";
            //    txtTransporterName.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txttransporterNo.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide Driver Number.";
            //    txttransporterNo.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtvehicleNo.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide Vehicle Number.";
            //    txtvehicleNo.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtReceiptValue.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide Receipt Value.";
            //    txtReceiptValue.Focus();
            //    b = false;
            //    return b;
            //}
            //if (Convert.ToDecimal(txtReceiptValue.Text) != Convert.ToDecimal(Session["TotalAmount"].ToString()))
            //{
            //    LblMessage.Text = "► Total Grid Items Net Value Not Matches With the Receipt Value Amount.";
            //    txtReceiptValue.Focus();
            //    b = false;
            //    return b;
            //}
            if (txtProductCode.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Product Code:";
                txtProductCode.Focus();
                b = false;
                return b;
            }

            if (txtProductDesc.Text == string.Empty)
            {
                LblMessage.Text = "► Product Description Not Available:";
                txtProductDesc.Focus();
                b = false;
                return b;
            }
            if (txtMRP.Text == string.Empty)
            {
                LblMessage.Text = "► Product MRP Not Available:";
                txtMRP.Focus();
                b = false;
                return b;
            }

            if (txtEntryValue.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide " + LTEntryType.Text + " Value :";
                txtEntryValue.Focus();
                //txtEntryValue.BorderColor = Color.Red;
                b = false;
                return b;
            }
            if (txtWeight.Text == string.Empty)
            {
                LblMessage.Text = "► Weight Cannot be empty or 0.";
                txtWeight.Focus();
                b = false;
                return b;
            }
            if (txtVolume.Text == string.Empty)
            {
                LblMessage.Text = "► Volume Cannot be empty or 0:";
                txtVolume.Focus();
                b = false;
                return b;
            }
            if (txtRate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Rate :";
                txtRate.Focus();
                b = false;
                return b;
            }
            if (txtValueRate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Rate To Calculate Value :";
                txtValueRate.Focus();
                b = false;
                return b;
            }
            //if (txtTRDDiscPerc.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide TRD % :";
            //    txtTRDDiscPerc.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtTRDpercValue.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide TRD Disc Value :";
            //    txtTRDDiscPerc.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtPriceEqualValue.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide Price Equal Value :";
            //    txtPriceEqualValue.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtVATAddTAXPerc.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide VAT Add TAX % :";
            //    txtVATAddTAXPerc.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtVATAddTAXValue.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide VAT Add TAX % to Calculate Vat Value:";
            //    txtVATAddTAXPerc.Focus();
            //    b = false;
            //    return b;
            //}

            if (IGST.Visible == true)
            {
                if (txtIGSTPerc.Text == string.Empty)
                {
                    LblMessage.Text = "► Please Provide IGST % :";
                    txtIGSTPerc.Focus();
                    b = false;
                    return b;
                }
                if (txtIGSTValue.Text == string.Empty)
                {
                    LblMessage.Text = "► Please Provide IGST % to Calculate IGST Value:";
                    txtIGSTValue.Focus();
                    b = false;
                    return b;
                }
            }


            if (SGST.Visible == true)
            {
                if (txtSGSTPerc.Text == string.Empty)
                {
                    LblMessage.Text = "► Please Provide SGST % :";
                    txtSGSTPerc.Focus();
                    b = false;
                    return b;
                }
                if (txtSGSTValue.Text == string.Empty)
                {
                    LblMessage.Text = "► Please Provide SGST % to Calculate SGST Value:";
                    txtSGSTValue.Focus();
                    b = false;
                    return b;
                }
            }

            if (UGST.Visible == true)
            {
                if (txtUGSTPerc.Text == string.Empty)
                {
                    LblMessage.Text = "► Please Provide UGST % :";
                    txtUGSTPerc.Focus();
                    b = false;
                    return b;
                }
                if (txtUGSTValue.Text == string.Empty)
                {
                    LblMessage.Text = "► Please Provide UGST % to Calculate UGST Value:";
                    txtUGSTValue.Focus();
                    b = false;
                    return b;
                }
            }


            if (CGST.Visible == true)
            {
                if (txtCGSTPerc.Text == string.Empty)
                {
                    LblMessage.Text = "► Please Provide CGST % :";
                    txtCGSTPerc.Focus();
                    b = false;
                    return b;
                }
                if (txtCGSTValue.Text == string.Empty)
                {
                    LblMessage.Text = "► Please Provide CGST % to Calculate CGST Value:";
                    txtCGSTValue.Focus();
                    b = false;
                    return b;
                }
            }


            //if (txtTax1Value.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide TAX1 % to Calculate TAX1 Value:";
            //    txtTax1Value.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtTax2Perc.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide TAX2 % :";
            //    txtTax2Perc.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtTax2Value.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide TAX2 % to Calculate TAX2 Value:";
            //    txtTax2Value.Focus();
            //    b = false;
            //    return b;
            //}

            if (txtGrossRate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Gross Amount:";
                txtGrossRate.Focus();
                b = false;
                return b;
            }
            if (txtTotalValue.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Gross Value First:";
                txtGrossRate.Focus();
                b = false;
                return b;
            }
            else
            {
                LblMessage.Text = string.Empty;
                b = true;
            }
            return b;

        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            //bool valid = ValidateManualEntry();

            //if(valid)
            //{
            //   DataTable dt =  AddLineItems();
            //   if (dt.Rows.Count > 0)
            //   {
            //       GridPurchItems.DataSource = dt;
            //       GridPurchItems.DataBind();
            //       GridViewFooterTotalShow(dt);
            //   }
            //}

        }

        protected void DDLEntryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLEntryType.SelectedItem.Text == "Box")
            {
                LTEntryType.Text = "Box Qty";
            }
            if (DDLEntryType.SelectedItem.Text == "Crate")
            {
                LTEntryType.Text = "Crate Qty";
            }
        }

        private string[] CalculateManualEntryMethod(decimal RateAmount, decimal EntryValueQty)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string[] returnResult = null;

            try
            {
                if (DDLEntryType.SelectedItem.Text.ToString() == "Crate")
                {
                    string[] ReturnArray = obj.CalculatePrice1(txtProductCode.Text, string.Empty, Convert.ToDecimal(txtEntryValue.Text), DDLEntryType.SelectedItem.Text.ToString());

                    EntryValueQty = Convert.ToDecimal(ReturnArray[0].ToString());
                }

                decimal RateValue = 0;
                decimal TRDPerc = Convert.ToDecimal(txtTRDDiscPerc.Text);
                decimal TRDValue = 0;
                decimal PriceEqualValue;
                if (txtPriceEqualValue.Text == "")
                {
                    PriceEqualValue = 0.00m;
                }
                else
                {
                    PriceEqualValue = Convert.ToDecimal(txtPriceEqualValue.Text);
                }
                decimal SDValue;
                if (txtSDValue.Text == "")
                {
                    SDValue = 0.00m;
                }
                else
                {
                    SDValue = Convert.ToDecimal(txtSDValue.Text);
                }
                //decimal SDValue = Convert.ToDecimal(txtSDValue.Text);
                //decimal VATPerc = Convert.ToDecimal(txtVATAddTAXPerc.Text);
                decimal IGSTPerc = Convert.ToDecimal(txtIGSTPerc.Text);
                //decimal VATValue = 0;
                decimal IGSTValue = 0;
                decimal SGSTPerc = Convert.ToDecimal(txtSGSTPerc.Text);
                decimal SGSTValue = 0;
                decimal UGSTPerc = Convert.ToDecimal(txtUGSTPerc.Text);
                decimal UGSTValue = 0;
                decimal CGSTPerc = Convert.ToDecimal(txtCGSTPerc.Text);
                decimal CGSTValue = 0;
                decimal VATPerc = IGSTPerc + SGSTPerc + UGSTPerc + CGSTPerc;
                decimal VATValue = 0;
                decimal GrossRate = Convert.ToDecimal(txtGrossRate.Text);
                decimal NetValue = 0;
                decimal Weight = 0;
                decimal Ltr = 0;

                RateValue = Math.Round(RateAmount * EntryValueQty, 2);

                TRDValue = Math.Round((RateValue * TRDPerc) / 100, 2);

                //VATValue = Math.Round((((RateValue - TRDValue) + PriceEqualValue) * VATPerc) / 100, 2);
                IGSTValue = Math.Round((((RateValue - TRDValue - SDValue) + PriceEqualValue) * IGSTPerc) / 100, 2);

                SGSTValue = Math.Round((((RateValue - TRDValue - SDValue) + PriceEqualValue) * SGSTPerc) / 100, 2);

                UGSTValue = Math.Round((((RateValue - TRDValue - SDValue) + PriceEqualValue) * UGSTPerc) / 100, 2);

                CGSTValue = Math.Round((((RateValue - TRDValue - SDValue) + PriceEqualValue) * CGSTPerc) / 100, 2);

                VATValue = IGSTValue + SGSTValue + UGSTValue + CGSTValue;

                NetValue = Math.Round(GrossRate * EntryValueQty, 2);

                Weight = Math.Round((Convert.ToDecimal(txtWeight.Text) * EntryValueQty) / 1000, 2);

                Ltr = Math.Round((Convert.ToDecimal(txtVolume.Text) * EntryValueQty) / 1000, 2);

                returnResult = new string[10];
                returnResult[0] = RateValue.ToString();
                returnResult[1] = TRDValue.ToString();
                //returnResult[2] = VATValue.ToString();
                returnResult[2] = IGSTValue.ToString();
                returnResult[3] = SGSTValue.ToString();
                returnResult[4] = UGSTValue.ToString();
                returnResult[5] = CGSTValue.ToString();
                returnResult[6] = VATValue.ToString();
                returnResult[7] = NetValue.ToString();
                returnResult[8] = Weight.ToString();
                returnResult[9] = Ltr.ToString();
                return returnResult;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return returnResult;
            }

        }

        private string[] GetBOXCRATELITRECalculatedValueFromGlobal()
        {
            string[] ReturnArray = null;
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            try
            {
                ReturnArray = obj.CalculatePrice1(txtProductCode.Text, string.Empty, Convert.ToDecimal(txtEntryValue.Text), DDLEntryType.SelectedItem.Text.ToString());

                return ReturnArray;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
                return ReturnArray;
            }
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            ResetPageUnPostedData();
        }

        private void ResetPageUnpostedDate1()
        {
            txtInvoiceNo.ReadOnly = false;

            LblMessage.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtProductDesc.Text = string.Empty;
            txtMRP.Text = string.Empty;
            DDLEntryType.SelectedIndex = 0;
            txtEntryValue.Text = string.Empty;
            //BtnCalculate.Visible = true;
            //BtnAddItem.Visible = false;
            txtRate.Text = string.Empty;
            txtValueRate.Text = string.Empty;
            txtTRDDiscPerc.Text = string.Empty;
            txtTRDpercValue.Text = string.Empty;
            txtPriceEqualValue.Text = string.Empty;
            txtSDPerc.Text = string.Empty;
            txtSDValue.Text = string.Empty;
            //txtVATAddTAXPerc.Text = string.Empty;
            //txtVATAddTAXValue.Text = string.Empty;
            //txtTax1Perc.Text = string.Empty;
            //txtTax1Value.Text = string.Empty;
            //txtTax2Perc.Text = string.Empty;
            //txtTax2Value.Text = string.Empty;
            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    DrpState.Enabled = false;
            //}
            //else
            //{
            //    DrpState.Enabled = true;
            //}
            txtIGSTPerc.Text = string.Empty;
            txtIGSTValue.Text = string.Empty;
            txtSGSTPerc.Text = string.Empty;
            txtSGSTValue.Text = string.Empty;
            txtUGSTPerc.Text = string.Empty;
            txtUGSTValue.Text = string.Empty;
            txtCGSTPerc.Text = string.Empty;
            txtCGSTValue.Text = string.Empty;
            txtGrossRate.Text = string.Empty;
            txtTotalValue.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtVolume.Text = string.Empty;

        }

        private void ResetPageUnPostedData()
        {
            txtInvoiceDate.ReadOnly = false;
            txtReceiptValue.ReadOnly = true;
            //txtReceiptDate.ReadOnly = false;
            Session["PreDocument_No"] = null;
            txtInvoiceNo.ReadOnly = false;

            LblMessage.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtProductDesc.Text = string.Empty;
            txtMRP.Text = string.Empty;
            DDLEntryType.SelectedIndex = 0;
            txtEntryValue.Text = string.Empty;
            //BtnCalculate.Visible = true;
            //BtnAddItem.Visible = false;
            txtRate.Text = string.Empty;
            txtValueRate.Text = string.Empty;
            txtTRDDiscPerc.Text = string.Empty;
            txtTRDpercValue.Text = string.Empty;
            txtPriceEqualValue.Text = string.Empty;
            //txtVATAddTAXPerc.Text = string.Empty;
            //txtVATAddTAXValue.Text = string.Empty;
            //txtTax1Perc.Text = string.Empty;
            //txtTax1Value.Text = string.Empty;
            //txtTax2Perc.Text = string.Empty;
            //txtTax2Value.Text = string.Empty;
            txtIGSTPerc.Text = string.Empty;
            txtIGSTValue.Text = string.Empty;
            txtSGSTPerc.Text = string.Empty;
            txtSGSTValue.Text = string.Empty;
            txtUGSTPerc.Text = string.Empty;
            txtUGSTValue.Text = string.Empty;
            txtCGSTPerc.Text = string.Empty;
            txtCGSTValue.Text = string.Empty;
            txtGrossRate.Text = string.Empty;
            txtTotalValue.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtVolume.Text = string.Empty;

            //free the Header Textbox
            txtIndentDate.Text = string.Empty;
            txtTransporterName.Text = string.Empty;
            txttransporterNo.Text = string.Empty;
            txtvehicleNo.Text = string.Empty;
            DrpSalesInvoice.SelectedIndex = 0;
            txtInvoiceDate.Text = string.Empty;
            txtVehicleType.Text = string.Empty;
            txtReceiptValue.Text = string.Empty;
            GridPurchItems.DataSource = null;
            GridPurchItems.Visible = false;
            txtPurchDocumentNo.Text = string.Empty;
            //Emtry grid view data
            GridPurchItems.DataSource = null;
            GridPurchItems.DataBind();
            //
            //txtPlantName.Text = "";
            txtPostalCode.Text = "";
            txtAddr.Text = "";
            txtPlantCity.Text = "";

        }

        private void FillIndentNo()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string strQuery = " select distinct INDENT_NO as INDENT_NO from [ax].[ACXPURCHINDENTHEADER] PINDH where  NOT EXISTS (Select PURCH_INDENTNO " +
                              " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH where PINDH.INDENT_NO=PIH.PURCH_INDENTNO AND PINDH.SITEID = PIH.SITE_CODE) and  PINDH.SITEID='" + Session["SiteCode"].ToString() +
                              "' and PINDH.STATUS=1  order by INDENT_NO desc";

            DrpIndentNo.Items.Clear();
            DrpIndentNo.Items.Add("-Select-");
            obj.BindToDropDown(DrpIndentNo, strQuery, "INDENT_NO", "INDENT_NO");

        }

        protected void DrpIndentNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIndentNo.Text = DrpIndentNo.Text;
            if (DrpIndentNo.Text != "-Select-")
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                string strQuery = " select indent_date,invoice_no from [ax].[ACXPURCHINDENTHEADER] where indent_no = '" + DrpIndentNo.SelectedValue.ToString() + "'";

                DataTable dt = obj.GetData(strQuery);
                if (dt.Rows.Count > 0)
                {
                    txtIndentDate.Text = Convert.ToDateTime(dt.Rows[0]["indent_date"]).ToString("dd-MMM-yyyy");
                    txtInvoiceNo.Text = dt.Rows[0]["invoice_no"].ToString();
                    // DrpSalesInvoice.Items.FindByValue(dt.Rows[0]["invoice_no"].ToString()).Selected = true;
                }
            }
        }

        private bool ValidateCompleteHeader()
        {
            bool b = false;

            //if (DrpIndentNo.Text == "-Select-")
            //{
            //    LblMessage.Text = "► Please Select Indent No.";
            //    DrpIndentNo.Focus();
            //    b = false;
            //    return b;
            //}
            //if (txtIndentDate.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Select Indent Date.";
            //    txtIndentDate.Focus();
            //    b = false;
            //    return b;
            //}
            if (rdoSAPFetchData.Checked)
            {
                if (DrpSalesInvoice.SelectedValue == string.Empty)
                {
                    LblMessage.Text = "► Please Provide Invoice No.";
                    DrpSalesInvoice.Focus();
                    b = false;
                    return b;
                }
            }
            else
            {
                if (txtInvoiceNo.Text == "")
                {
                    LblMessage.Text = "► Please Provide Invoice No.";
                    txtInvoiceNo.Focus();
                    b = false;
                    return b;
                }
            }
            if (txtInvoiceDate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice Date.";
                txtInvoiceDate.Focus();
                b = false;
                return b;
            }
            if (txtTransporterName.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Transporter Name.";
                txtTransporterName.Focus();
                b = false;
                return b;
            }
            if (txttransporterNo.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Driver Number.";
                txttransporterNo.Focus();
                b = false;
                return b;
            }
            if (txtvehicleNo.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Vehicle Number.";
                txtvehicleNo.Focus();
                b = false;
                return b;
            }
            if (txtAddr.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Plant_Add.";
                txtAddr.Focus();
                b = false;
                return b;
            }
            if (txtReceiptValue.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Receipt Value.";
                txtReceiptValue.Focus();
                b = false;
                return b;
            }
            if (Convert.ToDecimal(txtReceiptValue.Text) != Convert.ToDecimal(Session["TotalAmount"].ToString()))
            {
                LblMessage.Text = "► Total Grid Items Net Value Not Matches With the Receipt Value Amount.";
                txtReceiptValue.Focus();
                b = false;
                return b;
            }
            else
            {
                LblMessage.Text = string.Empty;
                b = true;
            }
            return b;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //=================Validaion======Check InvoiceNo=================
            DataTable dtChk = new DataTable();
            string SavingInvoiceNo = string.Empty;
            if (rdoSAPFetchData.Checked)
            {
                SavingInvoiceNo = DrpSalesInvoice.SelectedValue;
            }
            else
            {
                SavingInvoiceNo = txtInvoiceNo.Text;
            }
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string chkInvoice = "select * from [ax].[ACXPURCHINVRECIEPTHEADER] where Sale_InvoiceNo='" + SavingInvoiceNo + "' and DataAreaid='" + Session["DATAAREAID"].ToString() + "' and Site_Code='" + Session["SiteCode"].ToString() + "'";
            dtChk = obj.GetData(chkInvoice);
            if (dtChk.Rows.Count > 0)
            {

                string message = "alert('InvoiceNo: " + SavingInvoiceNo + " Already exists  !');";
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                return;
            }
            //==================Check Date=======================

            DateTime InvoiceDate, ReceiptDate;
            if (string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
            {
                showMsg("Invoice Date should not be empty !");
                return;
            }
            InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
            ReceiptDate = Convert.ToDateTime(txtReceiptDate.Text);
            if (InvoiceDate > ReceiptDate)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Invoice Date can't grater than Receipt Date !');", true);
                showMsg("Invoice Date can't greater than Receipt Date !");
                return;
            }
            if (InvoiceDate > DateTime.Now.Date)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Invoice Date can't grater than Receipt Date !');", true);
                showMsg("Invoice Date can't greater than current date !");
                return;
            }
            //================================
            bool b = ValidateManualEntry();
            if (b)
            {
                PRESavePurchaseInvoiceReceiptToDB();
                ResetProductdetails();
            }

        }

        private void PRESavePurchaseInvoiceReceiptToDB()
        {
            string SavingInvoiceNo = "";
            if (rdoSAPFetchData.Checked)
            {
                SavingInvoiceNo = DrpSalesInvoice.SelectedValue;
            }
            else
            {
                SavingInvoiceNo = txtInvoiceNo.Text;
            }
            try
            {
                string strCode = string.Empty;

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                if (txtPurchDocumentNo.Text == string.Empty)
                {
                    #region PO  Number Generate

                    string _query = "SELECT ISNULL(MAX(CAST(RIGHT(DOCUMENT_NO,7) AS INT)),0)+1 FROM [ax].[ACXPURCHINVRECIEPTHEADERPRE] where SITE_CODE='" + Session["SiteCode"].ToString() + "'";

                    cmd = new SqlCommand(_query);
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.Text;
                    object vc = cmd.ExecuteScalar();


                    DataTable dtNumSeq = obj.GetNumSequenceNew(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                    string NUMSEQ = string.Empty;
                    if (dtNumSeq != null)
                    {
                        strCode = dtNumSeq.Rows[0][0].ToString();
                        NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                    }
                    else
                    {
                        return;
                    }

                    strCode = obj.GetNumSequence(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());
                    Session["PreDocument_No"] = null;
                    Session["PreDocument_No"] = strCode;

                    #endregion

                    #region Header Insert Data

                    Random rnd = new Random();
                    int SO_NO = rnd.Next(40000, 1000000);                           //these all are temporary values to insert--Kinldy remove it after clearance..[Rahul]


                    string queryRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTHEADERPRE]";
                    Int64 Recid = Convert.ToInt64(obj.GetScalarValue(queryRecID));
                    cmd.CommandText = string.Empty;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[ACX_ACXPURCHINVRECIEPTHEADERPRE]";
                    cmd.Parameters.Clear();
                    string indentno = string.Empty;
                    if (DrpIndentNo.SelectedItem.Text.ToString() == "-Select-")
                    {
                        indentno = "";
                    }
                    else
                    {
                        indentno = DrpIndentNo.SelectedItem.Text.ToString();
                    }
                    cmd.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                    cmd.Parameters.AddWithValue("@Purchase_Indent_No", indentno);
                    cmd.Parameters.AddWithValue("@Purchase_Indent_Date", txtIndentDate.Text);
                    cmd.Parameters.AddWithValue("@Transporter_Code", txtTransporterName.Text);
                    cmd.Parameters.AddWithValue("@Document_No", strCode);
                    cmd.Parameters.AddWithValue("@DocumentDate", txtReceiptDate.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_No", txtvehicleNo.Text);
                    cmd.Parameters.AddWithValue("@Purchase_Reciept_No", strCode);
                    cmd.Parameters.AddWithValue("@Sale_InvoiceNo", SavingInvoiceNo);
                    cmd.Parameters.AddWithValue("@Sale_InvoiceDate", txtInvoiceDate.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_Type", txtVehicleType.Text);
                    cmd.Parameters.AddWithValue("@SO_No", SO_NO);
                    cmd.Parameters.AddWithValue("@Material_Value", "0.00");
                    cmd.Parameters.AddWithValue("@recid", Recid + 1);
                    cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                    cmd.Parameters.AddWithValue("@STATUS", 0);                  // for UnPosted Purchase Invoice Receipt Status//
                    cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                    cmd.Parameters.AddWithValue("@DRIVERNAME", txttransporterNo.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@PlantName", DrpPlant.SelectedItem.Value.ToString());
                    cmd.Parameters.AddWithValue("@PlantAddress", txtAddr.Text);
                    cmd.Parameters.AddWithValue("@PlantCity", txtPlantCity.Text);
                    cmd.Parameters.AddWithValue("@PlantPostcode", txtPostalCode.Text);
                    cmd.Parameters.AddWithValue("@OrderType", drpOrderType.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@PLANT_STATECODE", DrpState.SelectedValue.ToString());
                    if (txtGSTNNumber.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@PlantGSTNNumber", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PlantGSTNNumber", txtGSTNNumber.Text);
                    }
                    if (DrpCompScheme.SelectedItem.Text == "--Select--")
                    {
                        cmd.Parameters.AddWithValue("@CompositionScheme", "0");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CompositionScheme", DrpCompScheme.SelectedValue.ToString());
                    }
                    if (txtGSTRegistrationDate.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@GSTRegistrationDate", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@GSTRegistrationDate", txtGSTRegistrationDate.Text);
                    }
                    cmd.ExecuteNonQuery();
                    txtPurchDocumentNo.Text = strCode;
                    //transaction.Commit();
                    #endregion


                }

                #region Line Insert Data on Same PURCH Order Number

                cmd1 = new SqlCommand("[ACX_ACXPURCHINVRECIEPTLINEPRE]");
                //transaction = conn.BeginTransaction();
                cmd1.Connection = conn;
                if (transaction == null)
                {
                    transaction = conn.BeginTransaction();
                }
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.StoredProcedure;

                string queryRecidLine = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTLINEPRE]";
                Int64 RecidLine = Convert.ToInt64(obj.GetScalarValue(queryRecidLine));


                #endregion

                string productNameCode = txtProductDesc.Text;
                string[] str = productNameCode.Split('-');
                string productCode = str[0].ToString();

                strCode = txtPurchDocumentNo.Text;

                string[] ReturnArray = null;
                ReturnArray = obj.CalculatePrice1(txtProductCode.Text, Session["SiteCode"].ToString(), Convert.ToDecimal(txtEntryValue.Text), DDLEntryType.SelectedItem.Text.ToString());
                if (ReturnArray != null)
                {
                    if (txtIGSTPerc.Text == "")
                    {
                        txtIGSTPerc.Text = "0.00";
                    }
                    if (txtSGSTPerc.Text == "")
                    {
                        txtSGSTPerc.Text = "0.00";
                    }
                    if (txtUGSTPerc.Text == "")
                    {
                        txtUGSTPerc.Text = "0.00";
                    }
                    if (txtCGSTPerc.Text == "")
                    {
                        txtCGSTPerc.Text = "0.00";
                    }
                    if (txtIGSTValue.Text == "")
                    {
                        txtIGSTValue.Text = "0.00";
                    }
                    if (txtSGSTValue.Text == "")
                    {
                        txtSGSTValue.Text = "0.00";
                    }
                    if (txtUGSTValue.Text == "")
                    {
                        txtUGSTValue.Text = "0.00";
                    }
                    if (txtCGSTValue.Text == "")
                    {
                        txtCGSTValue.Text = "0.00";
                    }
                    cmd1.Parameters.AddWithValue("@RECID", RecidLine + 1);
                    cmd1.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                    cmd1.Parameters.AddWithValue("@Purchase_Reciept_No", strCode);
                    cmd1.Parameters.AddWithValue("@PRODUCT_CODE", productCode);
                    cmd1.Parameters.AddWithValue("@LINE_NO", RecidLine + 1);
                    cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());

                    #region Crate Box Data Insert

                    if (ReturnArray[5].ToString() == "Crate")
                    {
                        decimal VatIncPerc = Convert.ToDecimal(txtIGSTPerc.Text) + Convert.ToDecimal(txtSGSTPerc.Text) + Convert.ToDecimal(txtUGSTPerc.Text) + Convert.ToDecimal(txtCGSTPerc.Text);
                        decimal VatIncPercValue = Convert.ToDecimal(txtIGSTValue.Text) + Convert.ToDecimal(txtSGSTValue.Text) + Convert.ToDecimal(txtUGSTValue.Text) + Convert.ToDecimal(txtCGSTValue.Text);
                        cmd1.Parameters.AddWithValue("@BOX", ReturnArray[0].ToString());
                        cmd1.Parameters.AddWithValue("@CRATES", txtEntryValue.Text.Trim().ToString());
                        cmd1.Parameters.AddWithValue("@LTR", ReturnArray[1].ToString());
                        cmd1.Parameters.AddWithValue("@RATE", txtRate.Text);
                        cmd1.Parameters.AddWithValue("@UOM", ReturnArray[4].ToString());
                        cmd1.Parameters.AddWithValue("@BASICVALUE", txtValueRate.Text);
                        if (txtTRDDiscPerc.Text == "")
                        {
                            cmd1.Parameters.AddWithValue("@TRDDISCPERC", "0.00");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@TRDDISCPERC", txtTRDDiscPerc.Text);
                        }
                        //cmd1.Parameters.AddWithValue("@TRDDISCPERC", txtTRDDiscPerc.Text);
                        cmd1.Parameters.AddWithValue("@TRDDISCVALUE", txtTRDpercValue.Text);
                        cmd1.Parameters.AddWithValue("@PRICEEQUALVALUE", txtPriceEqualValue.Text);
                        if (IGST.Visible == true)
                        {
                            cmd1.Parameters.AddWithValue("@TAX", txtIGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", txtIGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAX", "0.00");
                            cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", "0.00");
                            cmd1.Parameters.AddWithValue("@TaxComponent", "IGST");
                            cmd1.Parameters.AddWithValue("@ADDTaxComponent", "");
                        }
                        if (UGST.Visible == true && CGST.Visible == true)
                        {
                            cmd1.Parameters.AddWithValue("@TAX", txtUGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", txtUGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAX", txtCGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", txtCGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@TaxComponent", "UGST");
                            cmd1.Parameters.AddWithValue("@ADDTaxComponent", "CGST");
                        }
                        if (SGST.Visible == true && CGST.Visible == true)
                        {
                            cmd1.Parameters.AddWithValue("@TAX", txtSGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", txtSGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAX", txtCGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", txtCGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@TaxComponent", "SGST");
                            cmd1.Parameters.AddWithValue("@ADDTaxComponent", "CGST");
                        }
                        //cmd1.Parameters.AddWithValue("@TAX", txtTax1Perc.Text);
                        //cmd1.Parameters.AddWithValue("@TAXAMOUNT", txtTax1Value.Text);
                        //cmd1.Parameters.AddWithValue("@ADDTAX", txtTax2Perc.Text);
                        //cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", txtTax2Value.Text);
                        cmd1.Parameters.AddWithValue("@VAT_INC_PERC", VatIncPerc.ToString());
                        cmd1.Parameters.AddWithValue("@VAT_INC_PERC_VALUE", VatIncPercValue.ToString());
                        cmd1.Parameters.AddWithValue("@GROSSAMOUNT", txtGrossRate.Text);
                        cmd1.Parameters.AddWithValue("@DISCOUNT", 0);
                        //cmd1.Parameters.AddWithValue("@TAX", 0);
                        //cmd1.Parameters.AddWithValue("@TAXAMOUNT", 0);
                        cmd1.Parameters.AddWithValue("@AMOUNT", txtTotalValue.Text);
                        cmd1.Parameters.AddWithValue("@Remark", txtRemark.Text);

                        cmd1.ExecuteNonQuery();
                    }
                    if (ReturnArray[5].ToString() == "Box")
                    {
                        decimal VatIncPerc = Convert.ToDecimal(txtIGSTPerc.Text) + Convert.ToDecimal(txtSGSTPerc.Text) + Convert.ToDecimal(txtUGSTPerc.Text) + Convert.ToDecimal(txtCGSTPerc.Text);
                        decimal VatIncPercValue = Convert.ToDecimal(txtIGSTValue.Text) + Convert.ToDecimal(txtSGSTValue.Text) + Convert.ToDecimal(txtUGSTValue.Text) + Convert.ToDecimal(txtCGSTValue.Text);
                        cmd1.Parameters.AddWithValue("@BOX", txtEntryValue.Text.Trim().ToString());
                        cmd1.Parameters.AddWithValue("@CRATES", ReturnArray[0].ToString());
                        cmd1.Parameters.AddWithValue("@LTR", ReturnArray[1].ToString());
                        if (txtTRDDiscPerc.Text == "")
                        {
                            txtTRDDiscPerc.Text = "0.00";
                        }
                        cmd1.Parameters.AddWithValue("@RATE", txtRate.Text);
                        cmd1.Parameters.AddWithValue("@UOM", ReturnArray[4].ToString());
                        cmd1.Parameters.AddWithValue("@BASICVALUE", txtValueRate.Text);
                        if (txtTRDDiscPerc.Text == "")
                        {
                            cmd1.Parameters.AddWithValue("@TRDDISCPERC", "0.00");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@TRDDISCPERC", txtTRDDiscPerc.Text);
                        }
                        //cmd1.Parameters.AddWithValue("@TRDDISCPERC", txtTRDDiscPerc.Text);
                        cmd1.Parameters.AddWithValue("@TRDDISCVALUE", txtTRDpercValue.Text);
                        cmd1.Parameters.AddWithValue("@PRICEEQUALVALUE", txtPriceEqualValue.Text);
                        if (IGST.Visible == true)
                        {
                            cmd1.Parameters.AddWithValue("@TAX", txtIGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", txtIGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAX", "0.00");
                            cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", "0.00");
                            cmd1.Parameters.AddWithValue("@TaxComponent", "IGST");
                            cmd1.Parameters.AddWithValue("@ADDTaxComponent", "");
                        }
                        if (UGST.Visible == true && CGST.Visible == true)
                        {
                            cmd1.Parameters.AddWithValue("@TAX", txtUGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", txtUGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAX", txtCGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", txtCGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@TaxComponent", "UGST");
                            cmd1.Parameters.AddWithValue("@ADDTaxComponent", "CGST");
                        }
                        if (SGST.Visible == true && CGST.Visible == true)
                        {
                            cmd1.Parameters.AddWithValue("@TAX", txtSGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", txtSGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAX", txtCGSTPerc.Text);
                            cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", txtCGSTValue.Text);
                            cmd1.Parameters.AddWithValue("@TaxComponent", "SGST");
                            cmd1.Parameters.AddWithValue("@ADDTaxComponent", "CGST");
                        }
                        //cmd1.Parameters.AddWithValue("@TAX", txtTax1Perc.Text);
                        //cmd1.Parameters.AddWithValue("@TAXAMOUNT", txtTax1Value.Text);
                        //cmd1.Parameters.AddWithValue("@ADDTAX", txtTax2Perc.Text);
                        //cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", txtTax2Value.Text);
                        cmd1.Parameters.AddWithValue("@VAT_INC_PERC", VatIncPerc.ToString());
                        cmd1.Parameters.AddWithValue("@VAT_INC_PERC_VALUE", VatIncPercValue.ToString());
                        cmd1.Parameters.AddWithValue("@GROSSAMOUNT", txtGrossRate.Text);
                        cmd1.Parameters.AddWithValue("@DISCOUNT", 0);
                        //cmd1.Parameters.AddWithValue("@TAX", 0);
                        //cmd1.Parameters.AddWithValue("@TAXAMOUNT", 0);
                        cmd1.Parameters.AddWithValue("@AMOUNT", txtTotalValue.Text);
                        cmd1.Parameters.AddWithValue("@Remark", txtRemark.Text);
                        if (txtSDPerc.Text == "")
                        {
                            cmd1.Parameters.AddWithValue("@SpecialDiscPerc", "0.00");
                        }
                        else
                        {
                            cmd1.Parameters.AddWithValue("@SpecialDiscPerc", txtSDPerc.Text);
                        }
                        //cmd1.Parameters.AddWithValue("@SpecialDiscPerc", txtSDPerc.Text);
                        cmd1.Parameters.AddWithValue("@SpecialDiscValue", txtSDValue.Text);
                        cmd1.ExecuteNonQuery();
                    }

                    //int a = obj.UpdateLastNumSequence(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);

                    transaction.Commit();
                    txtPurchDocumentNo.Text = strCode;
                    ShowRecords(txtPurchDocumentNo.Text.ToString());
                    UpdateAndShowTotalMaterialValue(txtPurchDocumentNo.Text.ToString());
                    ResetPageUnpostedDate1();
                    BtnUpdateHeader.Visible = true;


                    #endregion

                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
            }

        }

        private void GridViewFooterCalculate(DataTable dt)
        {
            decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("PRODUCT_MRP"));          //For Total[Sum] Value Show in Footer--//
            decimal tQty = dt.AsEnumerable().Sum(row => row.Field<decimal>("BOX"));
            decimal RATE = dt.AsEnumerable().Sum(row => row.Field<decimal>("RATE"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASICVALUE"));
            decimal TRDDISCVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("TRDDISCVALUE"));
            //decimal DiscVal = dt.AsEnumerable().Sum(row => row.Field<decimal>("DiscVal"));
            decimal Tax_Amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));
            decimal AddTax_Amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADD_TAX_AMOUNT"));
            decimal TOTALTAX = dt.AsEnumerable().Sum(row => row.Field<decimal>("VAT_INC_PERCVALUE"));
            decimal NetValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("AMOUNT"));
            decimal SDValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("SPECIALDISCV"));

            //==============================

            GridPurchItems.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            GridPurchItems.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[3].Text = "TOTAL";
            GridPurchItems.FooterRow.Cells[3].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[4].Text = total.ToString("N2");
            GridPurchItems.FooterRow.Cells[4].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[5].Text = tQty.ToString("N2");
            GridPurchItems.FooterRow.Cells[5].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[9].Text = RATE.ToString("N2");
            GridPurchItems.FooterRow.Cells[9].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[10].Text = BASICVALUE.ToString("N2");
            GridPurchItems.FooterRow.Cells[10].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[12].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[12].Text = TRDDISCVALUE.ToString("N2");
            GridPurchItems.FooterRow.Cells[12].Font.Bold = true;

            //GridPurchItems.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            //GridPurchItems.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            //GridPurchItems.FooterRow.Cells[11].Text = DiscVal.ToString("N2");
            //GridPurchItems.FooterRow.Cells[11].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[16].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[16].Text = Tax_Amount.ToString("N2");
            GridPurchItems.FooterRow.Cells[16].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[19].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[19].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[19].Text = AddTax_Amount.ToString("N2");
            GridPurchItems.FooterRow.Cells[19].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[21].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[21].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[21].Text = TOTALTAX.ToString("N2");
            GridPurchItems.FooterRow.Cells[21].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[22].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[22].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[22].Text = NetValue.ToString("N2");
            GridPurchItems.FooterRow.Cells[22].Font.Bold = true;

            GridPurchItems.FooterRow.Cells[25].HorizontalAlign = HorizontalAlign.Left;
            GridPurchItems.FooterRow.Cells[25].ForeColor = System.Drawing.Color.MidnightBlue;
            GridPurchItems.FooterRow.Cells[25].Text = SDValue.ToString("N2");
            GridPurchItems.FooterRow.Cells[25].Font.Bold = true;

            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    txtReceiptValue.Text = NetValue.ToString("N2");
            //}
            //else
            //{
            //    txtReceiptValue.Text = "0.00";
            //}


        }

        private void UpdateAndShowTotalMaterialValue(string PurchNo)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            try
            {
                string query = "Select * from [ax].[ACXPURCHINVRECIEPTLINEPRE] WHERE PURCH_RECIEPTNO='" + PurchNo + "' " +
                               "and SITEID='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";


                DataTable dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    decimal totalvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("AMOUNT"));
                    txtReceiptValue.Text = Math.Round(totalvalue, 2).ToString();

                    string UpdateMaterialValue = "UPDATE [ax].[ACXPURCHINVRECIEPTHEADERPRE] SET MATERIAL_VALUE = " + totalvalue + " where PURCH_RECIEPTNO='" + PurchNo + "' " +
                                                " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    obj.ExecuteCommand(UpdateMaterialValue);
                    DrpState.Enabled = false;
                    txtGSTNNumber.Enabled = false;
                    DrpCompScheme.Enabled = false;

                }
                else
                {
                    txtReceiptValue.Text = "";
                    string UpdateMaterialValue = "UPDATE [ax].[ACXPURCHINVRECIEPTHEADERPRE] SET MATERIAL_VALUE = '0.00' where PURCH_RECIEPTNO='" + PurchNo + "' " +
                                                " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    obj.ExecuteCommand(UpdateMaterialValue);

                    DrpState.Enabled = true;
                    txtGSTNNumber.Enabled = true;
                    DrpCompScheme.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }
        }

        private void ShowRecords(string PurchReceiptNumber)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dtHeader = null;
            DataTable dtLine = null;
            try
            {
                string queryHeader = "Select * from [ax].[ACXPURCHINVRECIEPTHEADERPRE] where PURCH_RECIEPTNO='" + PurchReceiptNumber + "' " +
                                     " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                string queryLine = "Select PURCH_RECIEPTNO, LINE_NO,PRODUCT_CODE, BOX,CRATES,A.LTR,A.UOM,RATE,BASICVALUE,TRDDISCPERC,TRDDISCVALUE,PRICE_EQUALVALUE," +
                                   "TAX as TAX_PERC,TAXCOMPONENT,TAXAMOUNT as TAX_AMOUNT,ADD_TAX_PERC,ADDTAXCOMPONENT,ADD_TAX_AMOUNT,VAT_INC_PERC,VAT_INC_PERCVALUE,GROSSRATE,AMOUNT,(PRODUCT_CODE+'-'+ PRODUCT_NAME) AS PRODUCTDESC,PRODUCT_MRP,SPECIALDISCPERC as SPECIALDISCP,SPECIALDISCVALUE as SPECIALDISCV,YEXP,YEXPPER,YCS3,YCS3PER,Remark  from [ax].[ACXPURCHINVRECIEPTLINEPRE] A " +
                                   " INNER JOIN AX.INVENTTABLE B  ON A.PRODUCT_CODE = B.ITEMID   WHERE PURCH_RECIEPTNO='" + PurchReceiptNumber + "' " +
                                   " and SITEID='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";
                //"dd-MMM-yyyy"



                dtHeader = obj.GetData(queryHeader);
                DrpIndentNo.SelectedItem.Text = dtHeader.Rows[0]["PURCH_INDENTNO"].ToString();
                DateTime IndentDate = Convert.ToDateTime(dtHeader.Rows[0]["PURCH_INDENTDATE"].ToString());
                txtIndentDate.Text = IndentDate.ToString("dd-MMM-yyyy");
                txtTransporterName.Text = dtHeader.Rows[0]["TRANSPORTER_CODE"].ToString();
                txtvehicleNo.Text = dtHeader.Rows[0]["VEHICAL_NO"].ToString();
                DateTime ReceiptDate = Convert.ToDateTime(dtHeader.Rows[0]["DOCUMENT_DATE"].ToString());
                txtReceiptDate.Text = ReceiptDate.ToString("dd-MMM-yyyy");
                if (rdoSAPFetchData.Checked)
                {
                    DrpSalesInvoice.Items.FindByValue(dtHeader.Rows[0]["SALE_INVOICENO"].ToString()).Selected = true;
                }
                else
                {
                    txtInvoiceNo.Text = dtHeader.Rows[0]["SALE_INVOICENO"].ToString();
                }
                DateTime InvcDate = Convert.ToDateTime(dtHeader.Rows[0]["SALE_INVOICEDATE"].ToString());
                txtInvoiceDate.Text = InvcDate.ToString("dd-MMM-yyyy");
                txttransporterNo.Text = dtHeader.Rows[0]["DRIVERNAME"].ToString();
                txtVehicleType.Text = dtHeader.Rows[0]["VEHICAL_TYPE"].ToString();
                decimal RecptValue = Convert.ToDecimal(dtHeader.Rows[0]["MATERIAL_VALUE"].ToString());
                txtReceiptValue.Text = (Math.Round(RecptValue, 2)).ToString();
                txtPurchDocumentNo.Text = PurchReceiptNumber;

                if (string.IsNullOrEmpty(dtHeader.Rows[0]["Plant_Name"].ToString()))
                {
                    //txtPlantName.Text = "";
                }
                else
                {
                    //txtPlantName.Text = dtHeader.Rows[0]["Plant_Name"].ToString();
                }
                if (string.IsNullOrEmpty(dtHeader.Rows[0]["Plant_Address"].ToString()))
                {
                    txtAddr.Text = "";
                }
                else
                {
                    txtAddr.Text = dtHeader.Rows[0]["Plant_Address"].ToString();
                }
                if (string.IsNullOrEmpty(dtHeader.Rows[0]["Plant_City"].ToString()))
                {
                    txtPlantCity.Text = "";
                }
                else
                {
                    txtPlantCity.Text = dtHeader.Rows[0]["Plant_City"].ToString();
                }
                if (string.IsNullOrEmpty(dtHeader.Rows[0]["Plant_PostCode"].ToString()))
                {
                    txtPostalCode.Text = "";
                }
                else
                {
                    txtPostalCode.Text = dtHeader.Rows[0]["Plant_PostCode"].ToString();
                }
                txtGSTRegistrationDate.Text = dtHeader.Rows[0]["VENDGSTINREGDATE"].ToString();
                txtGSTNNumber.Text = dtHeader.Rows[0]["VENDGSTINNO"].ToString();

                if (DrpState.Items.Contains(DrpState.Items.FindByValue(dtHeader.Rows[0]["PLANT_STATECODE"].ToString())))
                    DrpState.SelectedValue = dtHeader.Rows[0]["PLANT_STATECODE"].ToString();
                DrpCompScheme.SelectedValue = Convert.ToBoolean(dtHeader.Rows[0]["VENDCOMPOSITIONSCHEME"]) == true ? "1" : "0";
                dtLine = obj.GetData(queryLine);

                if (dtLine.Rows.Count > 0)
                {
                    GridPurchItems.DataSource = dtLine;
                    GridPurchItems.DataBind();
                    GridPurchItems.Visible = true;
                    GridViewFooterCalculate(dtLine);
                    //if(txtReceiptValue.Text != string.Empty || txtReceiptValue.Text != "")
                    //{
                    //    string query = "update [ax].[ACXPURCHINVRECIEPTHEADERPRE] set MATERIAL_VALUE = '"+ Convert.ToDecimal(txtReceiptValue.Text) + "'where PURCH_RECIEPTNO = '" + txtPurchDocumentNo.Text + "' " +
                    //                     " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";
                    //    obj.ExecuteCommand(query);
                    //}
                    //else
                    //{
                    //    string query = "update [ax].[ACXPURCHINVRECIEPTHEADERPRE] set MATERIAL_VALUE = '0.00' where PURCH_RECIEPTNO = '" + txtPurchDocumentNo.Text + "' " +
                    //                     " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";
                    //    obj.ExecuteCommand(query);
                    //}
                }
                else
                {
                    GridPurchItems.DataSource = dtLine;
                    GridPurchItems.DataBind();
                    LblMessage.Text = "No Line Items Exist";
                    BtnUpdateHeader.Visible = false;
                    //string query = "update [ax].[ACXPURCHINVRECIEPTHEADERPRE] set MATERIAL_VALUE = '0.00' where PURCH_RECIEPTNO = '" + txtPurchDocumentNo.Text + "' " +
                    //                     " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";
                    //obj.ExecuteCommand(query);
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }
        }

        protected void txtEntryValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                decimal EntryValueQty = 0;

                EntryValueQty = Convert.ToDecimal(txtEntryValue.Text);
                if (EntryValueQty != 0)
                {
                    txtWeight.Text = Convert.ToString(Math.Round((Convert.ToDecimal(Session["Weight"].ToString()) * EntryValueQty) / 1000, 2));
                    txtVolume.Text = Convert.ToString(Math.Round((Convert.ToDecimal(Session["Volume"].ToString()) * EntryValueQty) / 1000, 2));
                    txtRate.Focus();
                    CalculationLineValue();
                }
                else
                {
                    LblMessage.Text = "Entered Quantity always greater than zero..!!";
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }

        }

        protected void txtRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtEntryValue.Text == string.Empty)
                {
                    txtEntryValue.Focus();
                    showMsg("► Please Provide Qty Value");
                    return;
                }
                if (txtRate.Text.Trim() == "")
                {
                    txtRate.Text = "0";
                    CalculationLineValue();
                    txtRate.Focus();
                    showMsg("► Please Provide Rate.");
                    return;
                }
                CalculationLineValue();
                txtTRDDiscPerc.Focus();
            }

            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private Boolean CalculationLineValue()
        {
            try
            {
                decimal EntryValueQty = 0;
                if (DDLEntryType.SelectedItem.Text.ToString() == "Crate")
                {
                    string[] ReturnArray = baseObj.CalculatePrice1(txtProductCode.Text, string.Empty, Convert.ToDecimal(txtEntryValue.Text), DDLEntryType.SelectedItem.Text.ToString());

                    EntryValueQty = Convert.ToDecimal(ReturnArray[0].ToString());
                }
                else
                {
                    EntryValueQty = Convert.ToDecimal(txtEntryValue.Text);
                }
                if (txtRate.Text.Trim() == "") { txtRate.Text = "0"; }
                decimal decBasicValue, decTaxable, decNetValue, SDPerc, TRDPerc, TRDVALUE, SDValue, IGSTPerc, IGSTValue, SGSTPerc, SGSTValue, CGSTPerc, CGSTValue, UGSTPerc, UGSTValue;
                txtValueRate.Text = Convert.ToString(Math.Round(Convert.ToDecimal(txtRate.Text) * EntryValueQty, 2));
                decBasicValue = Convert.ToDecimal(txtValueRate.Text);

                if (txtTRDDiscPerc.Text != "")
                { TRDPerc = Convert.ToDecimal(txtTRDDiscPerc.Text); }
                else
                { TRDPerc = 0.00m; }
                TRDVALUE = Math.Round(decBasicValue * TRDPerc / 100, 2);
                txtTRDpercValue.Text = Convert.ToString(TRDVALUE);

                if (txtSDPerc.Text != "")
                { SDPerc = Convert.ToDecimal(txtSDPerc.Text); }
                else
                { SDPerc = 0.00m; }
                SDValue = decBasicValue * SDPerc / 100;
                txtSDValue.Text = SDValue.ToString("0.00");
                if (txtIGSTPerc.Text != "")
                { IGSTPerc = Convert.ToDecimal(txtIGSTPerc.Text); }
                else
                { IGSTPerc = 0.00m; }
                if (txtUGSTPerc.Text != "")
                { UGSTPerc = Convert.ToDecimal(txtUGSTPerc.Text); }
                else
                { UGSTPerc = 0.00m; }
                if (txtSGSTPerc.Text != "")
                { SGSTPerc = Convert.ToDecimal(txtSGSTPerc.Text); }
                else
                { SGSTPerc = 0.00m; }
                if (txtCGSTPerc.Text != "")
                { CGSTPerc = Convert.ToDecimal(txtCGSTPerc.Text); }
                else
                { CGSTPerc = 0.00m; }

                decimal PriceEqualValue, decPEPerc;
                decPEPerc = (IGSTPerc + SGSTPerc + UGSTPerc + CGSTPerc) / (1 + (IGSTPerc + SGSTPerc + UGSTPerc + CGSTPerc) / 100);
                PriceEqualValue = TRDVALUE * decPEPerc / 100;
                txtPriceEqualValue.Text = PriceEqualValue.ToString("0.00");

                decTaxable = decBasicValue - TRDVALUE - SDValue + PriceEqualValue;
                IGSTValue = decTaxable * IGSTPerc / 100;
                txtIGSTValue.Text = IGSTValue.ToString("0.00");

                CGSTValue = decTaxable * CGSTPerc / 100;
                txtCGSTValue.Text = CGSTValue.ToString("0.00");

                SGSTValue = decTaxable * SGSTPerc / 100;
                txtSGSTValue.Text = SGSTValue.ToString("0.00");

                UGSTValue = decTaxable * UGSTPerc / 100;
                txtUGSTValue.Text = UGSTValue.ToString("0.00");
                decNetValue = decTaxable + IGSTValue + UGSTValue + CGSTValue + SGSTValue;
                txtGrossRate.Text = (decTaxable / EntryValueQty).ToString("0.00");
                txtTotalValue.Text = decNetValue.ToString("0.00");
                return true;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }
        protected void txtTRDDiscPerc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculationLineValue();
                txtPriceEqualValue.Focus();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtSpecialDiscountPerc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculationLineValue();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void txtGrossRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculationLineValue();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void BtnUpdateHeader_Click(object sender, EventArgs e)
        {
            UpdateHeader(txtPurchDocumentNo.Text);
        }

        private void UpdateHeader(string PurchInvcNo)
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();
            string SavingInvoiceNo = string.Empty;
            if (rdoSAPFetchData.Checked)
            {
                SavingInvoiceNo = DrpSalesInvoice.SelectedValue;
            }
            else
            {
                SavingInvoiceNo = txtInvoiceNo.Text;
            }
            try
            {
                if (txtPurchDocumentNo.Text != string.Empty)
                {
                    #region Update Header

                    string QueryUpdate = " UPDATE [ax].[ACXPURCHINVRECIEPTHEADERPRE] SET PURCH_INDENTNO= @PurchIndentNo," +
                                         " PURCH_INDENTDATE= @PURCH_INDENTDATE, SALE_INVOICEDATE=@SALE_INVOICEDATE, SALE_INVOICENO = @SALE_INVOICENO," +
                                         " TRANSPORTER_CODE= @TRANSPORTER_CODE, VEHICAL_NO = @VEHICAL_NO, VEHICAL_TYPE= @VEHICAL_TYPE , DRIVERNAME = @DRIVERNAME " +
                                         " where PURCH_RECIEPTNO='" + txtPurchDocumentNo.Text + "' " +
                                         " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    cmd = new SqlCommand(QueryUpdate);
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.Text;

                    string strindentNo = string.Empty;
                    if (DrpIndentNo.SelectedItem.Text == "-Select-")
                    {
                        strindentNo = "";
                    }
                    else
                    {
                        strindentNo = DrpIndentNo.SelectedItem.Text;
                    }

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@PurchIndentNo", strindentNo);
                    cmd.Parameters.AddWithValue("@PURCH_INDENTDATE", txtIndentDate.Text);
                    cmd.Parameters.AddWithValue("@SALE_INVOICEDATE", txtInvoiceDate.Text);
                    cmd.Parameters.AddWithValue("@SALE_INVOICENO", SavingInvoiceNo);
                    cmd.Parameters.AddWithValue("@TRANSPORTER_CODE", txtTransporterName.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_NO", txtvehicleNo.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_TYPE", txtVehicleType.Text);
                    cmd.Parameters.AddWithValue("@DRIVERNAME", txttransporterNo.Text);
                    cmd.ExecuteNonQuery();
                    transaction.Commit();

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        protected void GridPurchItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            HiddenField hiddenfield = (HiddenField)GridPurchItems.Rows[e.RowIndex].FindControl("HiddenValueLineNo");

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string query = "Delete from [ax].[ACXPURCHINVRECIEPTLINEPRE] where PURCH_RECIEPTNO='" + txtPurchDocumentNo.Text + "' " +
                           " and LINE_NO = " + hiddenfield.Value + " and SITEID='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

            obj.ExecuteCommand(query);
            ShowRecords(txtPurchDocumentNo.Text);
            UpdateAndShowTotalMaterialValue(txtPurchDocumentNo.Text);
            //if(GridPurchItems.Rows.Count > 0)
            //{
            //    DrpState.Enabled = false;
            //}
            //else
            //{
            //    DrpState.Enabled = true;
            //}
        }

        protected void btnPostPurchaseInvoice_Click(object sender, EventArgs e)
        {
            // POSTPurchaseInvoiceReceipt(txtPurchDocumentNo.Text);
            //===============
            //=================Validaion======Check InvoiceNo=================
            DataTable dtChk = new DataTable();
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string SavingInvoiceNo = "";
            if (rdoSAPFetchData.Checked)
            {
                SavingInvoiceNo = DrpSalesInvoice.SelectedValue;
            }
            else
            {
                SavingInvoiceNo = txtInvoiceNo.Text;
            }
            string chkInvoice = "select * from [ax].[ACXPURCHINVRECIEPTHEADER] where Sale_InvoiceNo='" + SavingInvoiceNo + "' and DataAreaid='" + Session["DATAAREAID"].ToString() + "' and Site_Code='" + Session["SiteCode"].ToString() + "'";
            dtChk = obj.GetData(chkInvoice);
            if (dtChk.Rows.Count > 0)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('InvoiceNo: " + SavingInvoiceNo + " already exists  !');", true);

                // string message = "alert('InvoiceNo: " + txtInvoiceNo.Text + " already exists  !');";
                //ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                return;
            }
            //==================Check Date=======================

            DateTime InvoiceDate, ReceiptDate;
            if (txtInvoiceDate.Text == "")
            {
                return;
            }
            if (txtReceiptDate.Text == "")
            {
                return;
            }
            InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
            ReceiptDate = Convert.ToDateTime(txtReceiptDate.Text);
            if (InvoiceDate > ReceiptDate)
            {
                string message = "alert('Invoice Date cannot not greater than Receipt Date   !');";
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Invoice Date cant not greated than Receipt Date !');", true);
                // string message = "alert('Invoice Date cant not greated than Receipt Date !');";
                //ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                return;
            }
            //================================
            if (rdoSAPFetchData.Checked != true)
            {
                POSTPurchaseInvoiceReceipt(txtPurchDocumentNo.Text);
            }
            if (rdoSAPFetchData.Checked == true && ViewState["BAPI_STAGINGDATA"] != null || Convert.ToString(ViewState["BAPI_STAGINGDATA"]) != string.Empty)
            {
                bool b = ValidateSAPInvoicePosting();
                if (b)
                {
                    DataTable dtSAPLineItems = ViewState["BAPI_STAGINGDATA"] as DataTable;
                    POSTSAPInvoiceData(dtSAPLineItems);
                }
            }
            if (rdoSAPFetchData.Checked)
            {
                FillPendingSalesInvoice();
            }
            //rdoManualEntry_CheckedChanged(null, null);
        }

        private void POSTPurchaseInvoiceReceipt(string PrePurchReceiptNo)
        {
            try
            {
                string PostDocumentNo = string.Empty;

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                if (txtPurchDocumentNo.Text != string.Empty)
                {
                    #region Get PreSaved Data Header and Line

                    string queryPreHeader = "Select * from [ax].[ACXPURCHINVRECIEPTHEADERPRE] where PURCH_RECIEPTNO='" + PrePurchReceiptNo + "' " +
                                            " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    string queryPreLine = "Select * from [ax].[ACXPURCHINVRECIEPTLINEPRE] WHERE PURCH_RECIEPTNO='" + PrePurchReceiptNo + "' " +
                                          " and SITEID='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    DataTable dtHeaderPre = obj.GetData(queryPreHeader);
                    DataTable dtLinePre = obj.GetData(queryPreLine);

                    #endregion

                    if (dtHeaderPre.Rows.Count > 0 && dtLinePre.Rows.Count > 0)
                    {

                        #region Generate New Posted Invoice Number

                        string _query = "SELECT ISNULL(MAX(CAST(RIGHT(DOCUMENT_NO,7) AS INT)),0)+1 FROM [ax].[ACXPURCHINVRECIEPTHEADER] where SITE_CODE='" + Session["SiteCode"].ToString() + "'";

                        cmd = new SqlCommand(_query);
                        transaction = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.Text;
                        object vc = cmd.ExecuteScalar();

                        //PostDocumentNo = "PI-" + ((int)vc).ToString("0000000");                  // Unique Purchase Invoice Receipt Number Generate //
                        DataTable dtNumSeq = obj.GetNumSequenceNew(2, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                        string NUMSEQ = string.Empty;
                        if (dtNumSeq != null)
                        {
                            PostDocumentNo = dtNumSeq.Rows[0][0].ToString();
                            NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                        }
                        else
                        {
                            return;
                        }

                        //Session["PreDocument_No"] = null;
                        //Session["PreDocument_No"] = PostDocumentNo;

                        #endregion

                        string queryRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTHEADER]";
                        Int64 Recid = Convert.ToInt64(obj.GetScalarValue(queryRecID));

                        cmd.CommandText = string.Empty;
                        cmd.CommandType = CommandType.StoredProcedure;

                        //cmd.CommandText = " INSERT INTO [ax].[ACXPURCHINVRECIEPTHEADER] ( Document_No, DATAAREAID, RECID, Document_Date, Purch_IndentNo, Purch_IndentDate, " +
                        //          " SO_No,SO_Date,STATUS,Material_Value,Purch_RecieptNo,Sale_InvoiceDate,Sale_InvoiceNo,Site_Code,Transporter_Code,VEHICAL_No," +
                        //          " VEHICAL_Type,PREDOCUMENT_NO,NUMSEQ,DRIVERNAME,Plant_Name,Plant_Address,Plant_City,Plant_PostCode,OrderTypeCode,DISTGSTINNO," +
                        //          " DISTGSTINREGDATE,DISTCOMPOSITIONSCHEME,VENDGSTINNO,VENDGSTINREGDATE,VENDCOMPOSITIONSCHEME) values ( @Document_No,@DATAAREAID,@RECID," +
                        //          " @Document_Date,@Purch_IndentNo,@Purch_IndentDate,@SO_No,@SO_Date,@STATUS,@Material_Value,@Purch_RecieptNo,@Sale_InvoiceDate,@Sale_InvoiceNo," +
                        //          " @Site_Code,@Transporter_Code,@VEHICAL_No,@VEHICAL_Type,@PREDOCUMENT_NO,@NUMSEQ,@DRIVERNAME,@Plant_Name,@Plant_Address,@Plant_City," +
                        //          " @Plant_PostCode,@OrderTypeCode,@DISTGSTINNO,@DISTGSTINREGDATE,@DISTCOMPOSITIONSCHEME,@VENDGSTINNO,@VENDGSTINREGDATE,@VENDCOMPOSITIONSCHEME)";

                        cmd.CommandText = "[ACX_ACXPURCHINVRECIEPTHEADER]";


                        string queryInsert2 = "insert into [ax].[ACXVENDORTRANS](Document_No,SiteCode,Dealer_Code,DATAAREAID,RECID,Amount,DocumentType,RemainingAmount,Document_Date," +
                                         "Transaction_Date) values(@Document_No,@Site_Code,@Plant_Name,@DATAAREAID,@RECID,@Material_Value,1,@Material_Value,@Sale_InvoiceDate,@Sale_InvoiceDate)";
                        cmd2 = new SqlCommand(queryInsert2);
                        cmd2.Connection = conn;
                        if (transaction == null)
                        {
                            transaction = conn.BeginTransaction();
                        }
                        cmd2.Transaction = transaction;
                        cmd2.CommandTimeout = 3600;
                        cmd2.CommandType = CommandType.Text;

                        #region Insert Header

                        for (int i = 0; i < dtHeaderPre.Rows.Count; i++)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Document_No", PostDocumentNo);
                            cmd.Parameters.AddWithValue("@DATAAREAID", dtHeaderPre.Rows[0]["DATAAREAID"].ToString());
                            cmd.Parameters.AddWithValue("@RECID", Recid + 1);
                            string DATE = DateTime.Now.ToString();
                            cmd.Parameters.AddWithValue("@Document_Date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Purch_IndentNo", dtHeaderPre.Rows[0]["Purch_IndentNo"].ToString());
                            string abcd = Convert.ToDateTime(dtHeaderPre.Rows[0]["Purch_IndentDate"].ToString()).ToString();
                            cmd.Parameters.AddWithValue("@Purch_IndentDate", Convert.ToDateTime(dtHeaderPre.Rows[0]["Purch_IndentDate"].ToString()));
                            cmd.Parameters.AddWithValue("@SO_No", dtHeaderPre.Rows[0]["SO_No"].ToString());
                            string hello = Convert.ToDateTime(dtHeaderPre.Rows[0]["SO_Date"].ToString()).ToString();
                            cmd.Parameters.AddWithValue("@SO_Date", Convert.ToDateTime(dtHeaderPre.Rows[0]["SO_Date"].ToString()));
                            cmd.Parameters.AddWithValue("@STATUS", 1);
                            string vhb = Convert.ToDecimal(dtHeaderPre.Rows[0]["Material_Value"].ToString()).ToString();
                            cmd.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(dtHeaderPre.Rows[0]["Material_Value"].ToString()));
                            cmd.Parameters.AddWithValue("@Purch_RecieptNo", PostDocumentNo);
                            string hjsdf = Convert.ToDateTime(dtHeaderPre.Rows[0]["Sale_InvoiceDate"].ToString()).ToString();
                            cmd.Parameters.AddWithValue("@Sale_InvoiceDate", Convert.ToDateTime(dtHeaderPre.Rows[0]["Sale_InvoiceDate"].ToString()));
                            cmd.Parameters.AddWithValue("@Sale_InvoiceNo", dtHeaderPre.Rows[0]["Sale_InvoiceNo"].ToString());
                            cmd.Parameters.AddWithValue("@Site_Code", dtHeaderPre.Rows[0]["Site_Code"].ToString());
                            cmd.Parameters.AddWithValue("@Transporter_Code", dtHeaderPre.Rows[0]["Transporter_Code"].ToString());
                            cmd.Parameters.AddWithValue("@VEHICAL_No", dtHeaderPre.Rows[0]["VEHICAL_No"].ToString());
                            cmd.Parameters.AddWithValue("@VEHICAL_Type", dtHeaderPre.Rows[0]["VEHICAL_Type"].ToString());
                            cmd.Parameters.AddWithValue("@PREDOCUMENT_NO", PrePurchReceiptNo);
                            cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                            cmd.Parameters.AddWithValue("@DRIVERNAME", dtHeaderPre.Rows[0]["DRIVERNAME"].ToString());
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["Plant_Name"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@Plant_Name", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Plant_Name", dtHeaderPre.Rows[0]["Plant_Name"].ToString());
                            }
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["Plant_Address"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@Plant_Address", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Plant_Address", dtHeaderPre.Rows[0]["Plant_Address"].ToString());
                            }
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["Plant_City"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@Plant_City", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Plant_City", dtHeaderPre.Rows[0]["Plant_City"].ToString());
                            }
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["Plant_PostCode"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@Plant_PostCode", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Plant_PostCode", dtHeaderPre.Rows[0]["Plant_PostCode"].ToString());
                            }
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["DISTGSTINNO"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@DISTGSTINNO", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@DISTGSTINNO", dtHeaderPre.Rows[0]["DISTGSTINNO"].ToString());
                            }
                            cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", "1900-01-01");

                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["DISTCOMPOSITIONSCHEME"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", "0");
                            }
                            else
                            {
                                if (dtHeaderPre.Rows[0]["DISTCOMPOSITIONSCHEME"].ToString() == "false")
                                {
                                    cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", "0");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", "1");
                                }
                            }
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["VENDGSTINNO"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@VENDGSTINNO", "");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@VENDGSTINNO", dtHeaderPre.Rows[0]["VENDGSTINNO"].ToString());
                            }
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["VENDGSTINREGDATE"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@VENDGSTINREGDATE", "1900-01-01");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@VENDGSTINREGDATE", Convert.ToDateTime(dtHeaderPre.Rows[0]["VENDGSTINREGDATE"]));
                            }
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["VENDCOMPOSITIONSCHEME"].ToString()))
                            {
                                cmd.Parameters.AddWithValue("@VENDCOMPOSITIONSCHEME", "0");
                            }
                            else
                            {
                                if (dtHeaderPre.Rows[0]["VENDCOMPOSITIONSCHEME"].ToString() == "false")
                                {
                                    cmd.Parameters.AddWithValue("@VENDCOMPOSITIONSCHEME", '0');
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@VENDCOMPOSITIONSCHEME", '1');
                                }
                            }

                            //cmd.Parameters.AddWithValue("@OrderTypeCode", txtOrderType.Text.Trim().ToString());
                            cmd.Parameters.AddWithValue("@OrderTypeCode", drpOrderType.SelectedValue.ToString());

                            cmd.ExecuteNonQuery();

                            //------------Trans----Cdoe By Savita
                            cmd2.Parameters.AddWithValue("@Document_No", PostDocumentNo);
                            cmd2.Parameters.AddWithValue("@Site_Code", dtHeaderPre.Rows[0]["Site_Code"].ToString());
                            cmd2.Parameters.AddWithValue("@DATAAREAID", dtHeaderPre.Rows[0]["DATAAREAID"].ToString());
                            cmd2.Parameters.AddWithValue("@RECID", Recid + 1);
                            cmd2.Parameters.AddWithValue("@Document_Date", DateTime.Now);
                            if (string.IsNullOrEmpty(dtHeaderPre.Rows[0]["Plant_Name"].ToString()))
                            {
                                cmd2.Parameters.AddWithValue("@Plant_Name", "");
                            }
                            else
                            {
                                cmd2.Parameters.AddWithValue("@Plant_Name", dtHeaderPre.Rows[0]["Plant_Name"].ToString());
                            }
                            cmd2.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(dtHeaderPre.Rows[0]["Material_Value"].ToString()));
                            cmd2.Parameters.AddWithValue("@Sale_InvoiceDate", Convert.ToDateTime(dtHeaderPre.Rows[0]["Sale_InvoiceDate"].ToString()));
                            cmd2.ExecuteNonQuery();
                        }

                        #endregion

                        cmd1 = new SqlCommand();
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;

                        string queryLineRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTLINE]";
                        Int64 recid = Convert.ToInt64(obj.GetScalarValue(queryLineRecID));

                        cmd1.CommandText = string.Empty;
                        //cmd1.CommandText = " INSERT INTO [ax].[ACXPURCHINVRECIEPTLINE] ( Purch_RecieptNo,DATAAREAID,RECID,Amount,Box,Crates,Discount,Line_No, " +
                        //                   " Ltr,Product_Code,Rate,	Siteid,TAX,TAXAMOUNT,UOM,BASICVALUE,TRDDISCPERC,TRDDISCVALUE,PRICE_EQUALVALUE,VAT_INC_PERC, " +
                        //                   " VAT_INC_PERCVALUE,GROSSRATE,Remark,ADD_TAX_PERC,ADD_TAX_AMOUNT,TAXCOMPONENT,ADDTAXCOMPONENT,SCH_DISC_PERC,SCH_DISC_VAL) Values (@Purch_RecieptNo,@DATAAREAID,@RECID,@Amount,@Box,@Crates,@Discount,@Line_No," +
                        //                   " @Ltr,@Product_Code,@Rate, @Siteid,@TAX,@TAXAMOUNT,@UOM,@BASICVALUE,@TRDDISCPERC,@TRDDISCVALUE,@PRICE_EQUALVALUE, " +
                        //                   " @VAT_INC_PERC,@VAT_INC_PERCVALUE,@GROSSRATE,@Remark,@ADD_TAX_PERC,@ADD_TAX_AMOUNT,@TAXCOMPONENT,@ADDTAXCOMPONENT,@SPECIALDISCPERC,@SPECIALDISCVALUE)";
                        cmd1.CommandText = "[dbo].[ACX_ACXPURCHINVRECIEPTLINE]";
                        #region Line Insert

                        for (int p = 0; p < dtLinePre.Rows.Count; p++)
                        {
                            cmd1.Parameters.Clear();

                            cmd1.Parameters.AddWithValue("@Purch_RecieptNo", PostDocumentNo);
                            cmd1.Parameters.AddWithValue("@DATAAREAID", dtLinePre.Rows[p]["DATAAREAID"].ToString());
                            cmd1.Parameters.AddWithValue("@RECID", p + recid + 1);
                            cmd1.Parameters.AddWithValue("@Amount", Convert.ToDecimal(dtLinePre.Rows[p]["Amount"].ToString()));
                            cmd1.Parameters.AddWithValue("@Box", Convert.ToDecimal(dtLinePre.Rows[p]["Box"].ToString()));
                            cmd1.Parameters.AddWithValue("@Crates", Convert.ToDecimal(dtLinePre.Rows[p]["Crates"].ToString()));
                            cmd1.Parameters.AddWithValue("@Discount", Convert.ToDecimal(dtLinePre.Rows[p]["Discount"].ToString()));
                            cmd1.Parameters.AddWithValue("@Line_No", p + recid + 1);
                            cmd1.Parameters.AddWithValue("@Ltr", Convert.ToDecimal(dtLinePre.Rows[p]["Ltr"].ToString()));
                            cmd1.Parameters.AddWithValue("@Product_Code", dtLinePre.Rows[p]["Product_Code"].ToString());
                            cmd1.Parameters.AddWithValue("@Rate", Convert.ToDecimal(dtLinePre.Rows[p]["Rate"].ToString()));
                            cmd1.Parameters.AddWithValue("@Siteid", dtLinePre.Rows[p]["Siteid"].ToString());
                            cmd1.Parameters.AddWithValue("@TAX", Convert.ToDecimal(dtLinePre.Rows[p]["TAX"].ToString()));
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", Convert.ToDecimal(dtLinePre.Rows[p]["TAXAMOUNT"].ToString()));
                            //cmd1.Parameters.AddWithValue("@TAX", Convert.ToDecimal(dtLinePre.Rows[p]["TAX"].ToString()));
                            //cmd1.Parameters.AddWithValue("@TAXAMOUNT", Convert.ToDecimal(dtLinePre.Rows[p]["TAXAMOUNT"].ToString()));
                            cmd1.Parameters.AddWithValue("@UOM", dtLinePre.Rows[p]["UOM"].ToString());
                            cmd1.Parameters.AddWithValue("@BASICVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["BASICVALUE"].ToString()));
                            cmd1.Parameters.AddWithValue("@TRDDISCPERC", Convert.ToDecimal(dtLinePre.Rows[p]["TRDDISCPERC"].ToString()));
                            cmd1.Parameters.AddWithValue("@TRDDISCVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["TRDDISCVALUE"].ToString()));
                            cmd1.Parameters.AddWithValue("@PRICE_EQUALVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["PRICE_EQUALVALUE"].ToString()));
                            cmd1.Parameters.AddWithValue("@VAT_INC_PERC", Convert.ToDecimal(dtLinePre.Rows[p]["VAT_INC_PERC"].ToString()));
                            cmd1.Parameters.AddWithValue("@VAT_INC_PERCVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["VAT_INC_PERCVALUE"].ToString()));
                            cmd1.Parameters.AddWithValue("@GROSSRATE", Convert.ToDecimal(dtLinePre.Rows[p]["GROSSRATE"].ToString()));
                            cmd1.Parameters.AddWithValue("@Remark", dtLinePre.Rows[p]["Remark"].ToString());
                            cmd1.Parameters.AddWithValue("@ADD_TAX_PERC", Convert.ToDecimal(dtLinePre.Rows[p]["ADD_TAX_PERC"].ToString()));
                            cmd1.Parameters.AddWithValue("@ADD_TAX_AMOUNT", Convert.ToDecimal(dtLinePre.Rows[p]["ADD_TAX_AMOUNT"].ToString()));
                            cmd1.Parameters.AddWithValue("@TAXCOMPONENT", dtLinePre.Rows[p]["TAXCOMPONENT"].ToString());
                            cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", dtLinePre.Rows[p]["ADDTAXCOMPONENT"].ToString());
                            cmd1.Parameters.AddWithValue("@SPECIALDISCPERC", Convert.ToDecimal(dtLinePre.Rows[p]["SPECIALDISCPERC"].ToString()));
                            cmd1.Parameters.AddWithValue("@SPECIALDISCVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["SPECIALDISCVALUE"].ToString()));


                            cmd1.ExecuteNonQuery();

                        }

                        #endregion

                        //int a = obj.UpdateLastNumSequence(2, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);


                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Purchase Invoice Posted Successfully !  Document Number : " + PostDocumentNo + " ');", true);

                        //string message = "alert('Purchase Invoice Posted Successfully !  Document Number : " + PostDocumentNo + " ');";
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                        UpdatePostedStatusInPreTable();
                        if (rdoExcelEntry.Checked == true || rdoManualEntry.Checked)
                        {
                            ExcelUpdateTransTable(PostDocumentNo, transaction, conn);
                        }
                        else
                        {
                            UpdateTransTable(PostDocumentNo, transaction, conn);
                        }


                        transaction.Commit();
                        txtPurchDocumentNo.Text = string.Empty;
                        RefreshCompletePage();


                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' No Data to Save. Please Add Line Items and Header Details First !! ');", true);
                    //string message = "alert(' No Data to Save. Please Add Line Items and Header Details First !! ');";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void RefreshCompletePage()
        {
            Session["PreDocument_No"] = null;
            txtInvoiceNo.ReadOnly = false;
            txtInvoiceDate.ReadOnly = false;
            txtReceiptValue.ReadOnly = false;
            // txtReceiptDate.ReadOnly = false;
            txtGSTNNumber.Text = txtGSTRegistrationDate.Text = string.Empty;
            DrpCompScheme.SelectedIndex = 0;
            txtInvoiceNo.Text = string.Empty;
            LblMessage.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtProductDesc.Text = string.Empty;
            txtMRP.Text = string.Empty;
            DDLEntryType.SelectedIndex = 0;
            txtEntryValue.Text = string.Empty;
            txtRate.Text = string.Empty;
            txtValueRate.Text = string.Empty;
            txtTRDDiscPerc.Text = string.Empty;
            txtTRDpercValue.Text = string.Empty;
            txtPriceEqualValue.Text = string.Empty;
            txtIGSTPerc.Text = string.Empty;
            txtIGSTValue.Text = string.Empty;
            txtSGSTPerc.Text = string.Empty;
            txtSGSTValue.Text = string.Empty;
            txtUGSTPerc.Text = string.Empty;
            txtUGSTValue.Text = string.Empty;
            txtCGSTPerc.Text = string.Empty;
            txtCGSTValue.Text = string.Empty;
            //txtVATAddTAXPerc.Text = string.Empty;
            //txtVATAddTAXValue.Text = string.Empty;
            txtGrossRate.Text = string.Empty;
            txtTotalValue.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtVolume.Text = string.Empty;
            BtnUpdateHeader.Visible = false;
            DrpIndentNo.SelectedIndex = 0;
            FillIndentNo();
            txtIndentDate.Text = string.Empty;
            txtTransporterName.Text = string.Empty;
            txttransporterNo.Text = string.Empty;
            txtvehicleNo.Text = string.Empty;
            DrpSalesInvoice.SelectedIndex = 0;
            txtInvoiceDate.Text = string.Empty;
            txtVehicleType.Text = string.Empty;
            txtReceiptValue.Text = string.Empty;
            ordertype.Text = string.Empty;
            Plant.Text = string.Empty;
            txtState.Text = string.Empty;
            GridPurchItems.DataSource = null;
            GridPurchItems.Visible = false;
            txtPurchDocumentNo.Text = string.Empty;
            DrpState.Enabled = true;
            DrpState.SelectedIndex = 0;
            if (rdoExcelEntry.Checked == true)
            {
                DrpCompScheme.Enabled = txtGSTNNumber.Enabled = true;
            }
            else
            {
                DrpCompScheme.Enabled = txtGSTNNumber.Enabled = false;
            }
            //txtPlantName.Text = "";
            txtPostalCode.Text = "";
            txtAddr.Text = "";
            txtPlantCity.Text = "";
        }

        private void UpdatePostedStatusInPreTable()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            try
            {
                if (txtPurchDocumentNo.Text != string.Empty)
                {
                    #region Update Pre Header Status

                    string QueryUpdate = " UPDATE [ax].[ACXPURCHINVRECIEPTHEADERPRE] SET STATUS = 1  where PURCH_RECIEPTNO='" + txtPurchDocumentNo.Text + "' " +
                                         " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    obj.ExecuteCommand(QueryUpdate);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        //public void UpdateTransTable(string PostedDocumentNo)
        //{
        //    try
        //    {
        //        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        //        conn = obj.GetConnection();
        //        string TransLocation = "";

        //        string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
        //        DataTable dt = new DataTable();
        //        dt = obj.GetData(query1);
        //        if (dt.Rows.Count > 0)
        //        {
        //            TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
        //        }

        //        string st = Session["SiteCode"].ToString();
        //        string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");

        //        cmd = new SqlCommand();
        //        cmd.Connection = conn;
        //        cmd.CommandTimeout = 100;
        //        cmd.CommandText = string.Empty;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "[ACX_PURCHINVC_UPDATEINVENTTRANS]";

        //        cmd.Parameters.Clear();

        //        string strSite = Session["SiteCode"].ToString();
        //        string strDAtaArea = Session["DATAAREAID"].ToString();

        //        cmd.Parameters.AddWithValue("@SITECODE", strSite);
        //        cmd.Parameters.AddWithValue("@DOCUMENTPURCHRECEIPTNUMBER", PostedDocumentNo);
        //        cmd.Parameters.AddWithValue("@DATAAREAID", strDAtaArea);
        //        cmd.Parameters.AddWithValue("@TRANSID", TransId);
        //        cmd.Parameters.AddWithValue("@WAREHOUSE", TransLocation);
        //        cmd.Parameters.AddWithValue("@TRANSTYPE", 1);

        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = "Error: Inventory Update Issue - " + ex.Message.ToString();
        //    }
        //}

        //



        //Excel

        public void ExcelUpdateTransTable(string PostedDocumentNo, SqlTransaction trans, SqlConnection conn)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string TransLocation = "";
                string TransId = string.Empty;

                string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
                }

                string queryInsert = " Insert Into ax.acxinventTrans " +
                                 "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                 "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno],[OrderTypeCode])" +
                                 " Values (@TransId,@SiteCode,@DATAAREAID,@RECID,@InventTransDate,@TransType,@DocumentType,@DocumentNo,@DocumentDate, " +
                                 " @ProductCode,@TransQty,@TransUOM,@TransLocation,@Referencedocumentno,@OrderTypeCode)";


                cmd = new SqlCommand(queryInsert);
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                string st = Session["SiteCode"].ToString();
                if (st.Length <= 6)
                {
                    TransId = st + System.DateTime.Now.ToString("yymmddhhmmss");
                }
                else
                {
                    TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                }

                //==================by Rahul==============Old Code
                //for (int p = 0; p < GridPurchItems.Rows.Count; p++)
                //{
                //    string Siteid = Session["SiteCode"].ToString();
                //    string DATAAREAID = Session["DATAAREAID"].ToString();
                //    int TransType = 1;                                          // Type 1 for Purchase Invoice Receipt
                //    int DocumentType = 1;
                //    string DocumentNo = PostedDocumentNo;
                //    string productNameCode = GridPurchItems.Rows[p].Cells[3].Text;
                //    string[] str = productNameCode.Split('-');
                //    string ProductCode = str[0].ToString();
                //    string box = GridPurchItems.Rows[p].Cells[5].Text;

                //    decimal TransQty = Convert.ToDecimal(box) * 1;
                //    string UOM = GridPurchItems.Rows[p].Cells[7].Text;
                //    string Referencedocumentno = PostedDocumentNo;
                //    int REcid = p + 1;

                //    cmd.Parameters.Clear();
                //    cmd.Parameters.AddWithValue("@TransId", TransId);
                //    cmd.Parameters.AddWithValue("@SiteCode", Siteid);
                //    cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                //    cmd.Parameters.AddWithValue("@RECID", p + 1);
                //    cmd.Parameters.AddWithValue("@InventTransDate", DateTime.Now);
                //    cmd.Parameters.AddWithValue("@TransType", TransType);
                //    cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                //    cmd.Parameters.AddWithValue("@DocumentNo", DocumentNo);
                //    cmd.Parameters.AddWithValue("@DocumentDate", DateTime.Now);
                //    cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                //    cmd.Parameters.AddWithValue("@TransQty", TransQty);
                //    cmd.Parameters.AddWithValue("@TransUOM", UOM);
                //    cmd.Parameters.AddWithValue("@TransLocation", TransLocation);
                //    cmd.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno);

                //    int i = cmd.ExecuteNonQuery();
                //}

                //===============By Amrita =========6jun2016==========
                DataTable dtLinePre = new DataTable();
                string queryLinePre = "";
                if (Request.QueryString["PreNo"] != null && Session["PreDocument_No"] == null)
                {
                    //GetUnPostedReferenceData(Request.QueryString["PreNo"]);
                    queryLinePre = "select Product_Code,UOM,sum(Box) as Box from [ax].[ACXPURCHINVRECIEPTLINEPRE] where siteid='" + Session["SiteCode"].ToString() + "' and Purch_recieptno='" + Request.QueryString["PreNo"] + "' group by Product_Code,UOM ";
                }
                else if (Session["PreDocument_No"].ToString() != null)
                {
                    queryLinePre = "select Product_Code,UOM,sum(Box) as Box from [ax].[ACXPURCHINVRECIEPTLINEPRE] where siteid='" + Session["SiteCode"].ToString() + "' and Purch_recieptno='" + Session["PreDocument_No"].ToString() + "' group by Product_Code,UOM ";
                }

                dtLinePre = obj.GetData(queryLinePre);
                if (dtLinePre.Rows.Count > 0)
                {
                    int count = dtLinePre.Rows.Count;
                    // for (int p = 0; p < GridPurchItems.Rows.Count; p++)
                    for (int p = 0; p < count; p++)
                    {
                        string Siteid = Session["SiteCode"].ToString();
                        string DATAAREAID = Session["DATAAREAID"].ToString();
                        int TransType = 1;                                          // Type 1 for Purchase Invoice Receipt
                        int DocumentType = 1;
                        string DocumentNo = PostedDocumentNo;
                        //string productNameCode = GridPurchItems.Rows[p].Cells[3].Text;
                        //string[] str = productNameCode.Split('-');
                        string ProductCode = dtLinePre.Rows[p]["Product_Code"].ToString();// str[0].ToString();
                        string box = dtLinePre.Rows[p]["Box"].ToString(); //GridPurchItems.Rows[p].Cells[5].Text;

                        decimal TransQty = Convert.ToDecimal(box) * 1;
                        string UOM = dtLinePre.Rows[p]["UOM"].ToString(); //GridPurchItems.Rows[p].Cells[7].Text;
                        string Referencedocumentno = PostedDocumentNo;
                        int REcid = p + 1;

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@TransId", TransId);
                        cmd.Parameters.AddWithValue("@SiteCode", Siteid);
                        cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                        cmd.Parameters.AddWithValue("@RECID", p + 1);
                        cmd.Parameters.AddWithValue("@InventTransDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TransType", TransType);
                        cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                        cmd.Parameters.AddWithValue("@DocumentNo", DocumentNo);
                        cmd.Parameters.AddWithValue("@DocumentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                        cmd.Parameters.AddWithValue("@TransQty", TransQty);
                        cmd.Parameters.AddWithValue("@TransUOM", UOM);
                        cmd.Parameters.AddWithValue("@TransLocation", TransLocation);
                        cmd.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno);
                        //cmd.Parameters.AddWithValue("@OrderTypeCode", txtOrderType.Text.Trim().ToString());
                        cmd.Parameters.AddWithValue("@OrderTypeCode", drpOrderType.SelectedValue.ToString());

                        int i = cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
        //

        //Rahul 
        public void UpdateTransTable(string PostedDocumentNo, SqlTransaction trans, SqlConnection conn)
        {
            string SavingInvoiceNo = "";
            if (rdoSAPFetchData.Checked)
            {
                SavingInvoiceNo = DrpSalesInvoice.SelectedValue;
            }
            else
            {
                SavingInvoiceNo = txtInvoiceNo.Text;
            }
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string TransLocation = "";
                string TransId = string.Empty;

                string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
                }

                string queryInsert = " Insert Into ax.acxinventTrans " +
                                 "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                 "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno],[OrderTypeCode])" +
                                 " Values (@TransId,@SiteCode,@DATAAREAID,@RECID,@InventTransDate,@TransType,@DocumentType,@DocumentNo,@DocumentDate" +
                                 " ,@ProductCode,@TransQty,@TransUOM,@TransLocation,@Referencedocumentno,@OrderTypeCode)";


                cmd = new SqlCommand(queryInsert);
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                string st = Session["SiteCode"].ToString();
                if (st.Length <= 6)
                {
                    TransId = st + System.DateTime.Now.ToString("yymmddhhmmss");
                }
                else
                {
                    TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                }
                DataTable dtsapline = new DataTable();
                string queryLine = " Select  SSI.MATERIAL_CODE AS PRODUCTCODE,  sum(QTY_BOX) AS BOX, " +
                                  " SSI.UOM " +
                                  " from [ax].[ACX_STAGINGSALESINVOICE] SSI " +
                                  " INNER JOIN AX.INVENTSITE INVST ON INVST.SITEID = SSI.CUSTOMER_CODE AND INVST.DATAAREAID = SSI.DATAAREAID " +
                                  " Inner JOIN AX.INVENTTABLE INVT ON SSI.MATERIAL_CODE = INVT.ITEMID WHERE cast(SSI.INVOICE_NO as Bigint) ='" + SavingInvoiceNo + "' AND " +
                                  " CUSTOMER_CODE = '" + Session["SiteCode"].ToString() + "' and SSI.DATAAREAID='" + Session["DATAAREAID"].ToString() + "'" +
                                  " Group By SSI.MATERIAL_CODE,SSI.UOM";

                dtsapline = obj.GetData(queryLine);
                if (dtsapline.Rows.Count > 0)
                {
                    int count = dtsapline.Rows.Count;
                    // for (int p = 0; p < GridPurchItems.Rows.Count; p++)
                    for (int p = 0; p < count; p++)
                    //for (int p = 0; p < GridPurchItems.Rows.Count; p++)
                    {
                        string Siteid = Session["SiteCode"].ToString();
                        string DATAAREAID = Session["DATAAREAID"].ToString();
                        int TransType = 1;                                          // Type 1 for Purchase Invoice Receipt
                        int DocumentType = 1;
                        string DocumentNo = PostedDocumentNo;
                        //string productNameCode = GridPurchItems.Rows[p].Cells[2].Text;
                        //string[] str = productNameCode.Split('-');
                        string ProductCode = dtsapline.Rows[p]["PRODUCTCODE"].ToString();//str[0].ToString();
                        string box = dtsapline.Rows[p]["Box"].ToString();//GridPurchItems.Rows[p].Cells[4].Text;

                        decimal TransQty = Convert.ToDecimal(box) * 1;
                        string UOM = dtsapline.Rows[p]["UOM"].ToString();//GridPurchItems.Rows[p].Cells[6].Text;
                        string Referencedocumentno = PostedDocumentNo;
                        int REcid = p + 1;

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@TransId", TransId);
                        cmd.Parameters.AddWithValue("@SiteCode", Siteid);
                        cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                        cmd.Parameters.AddWithValue("@RECID", p + 1);
                        cmd.Parameters.AddWithValue("@InventTransDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TransType", TransType);
                        cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                        cmd.Parameters.AddWithValue("@DocumentNo", DocumentNo);
                        cmd.Parameters.AddWithValue("@DocumentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                        cmd.Parameters.AddWithValue("@TransQty", TransQty);
                        cmd.Parameters.AddWithValue("@TransUOM", UOM);
                        cmd.Parameters.AddWithValue("@TransLocation", TransLocation);
                        cmd.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno);
                        //cmd.Parameters.AddWithValue("@OrderTypeCode", txtOrderType.Text.Trim().ToString());
                        cmd.Parameters.AddWithValue("@OrderTypeCode", drpOrderType.SelectedValue.ToString());
                        int i = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            #region old Code PROCEDURE ISSUE

            //try
            //{
            //    CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            //    conn = obj.GetConnection();
            //    string TransLocation = "";
            //    string TransId = string.Empty;

            //    string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
            //    DataTable dt = new DataTable();
            //    dt = obj.GetData(query1);
            //    if (dt.Rows.Count > 0)
            //    {
            //        TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
            //    }

            //    string st = Session["SiteCode"].ToString();
            //    if (st.Length <= 6)
            //    {
            //        TransId = st + System.DateTime.Now.ToString("yymmddhhmmss");
            //    }
            //    else
            //    {
            //        TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
            //    }


            //    cmd = new SqlCommand();
            //    cmd.Connection = conn;
            //    cmd.CommandTimeout = 100;
            //    cmd.CommandText = string.Empty;
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "[ACX_PURCHINVC_UPDATEINVENTTRANS]";

            //    cmd.Parameters.Clear();

            //    string strSite = Session["SiteCode"].ToString();
            //    string strDAtaArea = Session["DATAAREAID"].ToString();

            //    cmd.Parameters.AddWithValue("@SITECODE", strSite);
            //    cmd.Parameters.AddWithValue("@DOCUMENTPURCHRECEIPTNUMBER", PostedDocumentNo);
            //    cmd.Parameters.AddWithValue("@DATAAREAID", strDAtaArea);
            //    cmd.Parameters.AddWithValue("@TRANSID", TransId);
            //    cmd.Parameters.AddWithValue("@WAREHOUSE", TransLocation);
            //    cmd.Parameters.AddWithValue("@TRANSTYPE", 1);

            //    int i = cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    LblMessage.Text = "Error: Inventory Update Issue - " + ex.Message.ToString();
            //}
            //finally
            //{
            //    if (conn != null)
            //    {
            //        if (conn.State == ConnectionState.Open)
            //        {
            //            conn.Close();
            //        }
            //    }
            //}

            #endregion
        }
        //

        private void GetUnPostedReferenceData(string queryString)
        {
            if (Request.QueryString["PreNo"].ToString() != string.Empty)
            {
                string ReferenceNo = Request.QueryString["PreNo"].ToString();

                bool b = CheckPostedStatus(ReferenceNo);
                if (b == false)
                {
                    ShowRecords(ReferenceNo);
                }
            }

        }

        private bool CheckPostedStatus(string ReferenceNo)
        {
            bool checkStatus = false;
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                string query = " Select  [STATUS] from [ax].[ACXPURCHINVRECIEPTHEADERPRE] WHERE SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "' AND DOCUMENT_NO='" + ReferenceNo + "' ";

                object status = obj.GetScalarValue(query);
                if (status != null || Convert.ToString(status) != string.Empty)
                {
                    string str = status.ToString();
                    if (str == "1")
                    {
                        checkStatus = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' Reference Number Not Found !! Redirecting back to previous page ...');", true);

                        //string message = "alert(' Reference Number Not Found !! Redirecting back to previous page ...');";
                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                        Response.Redirect("frmPurchUnPostList.aspx");
                        return checkStatus;
                    }
                }
                else
                {
                    checkStatus = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' Reference Status Not Exists !! Redirecting back to previous page ...');", true);

                    //string message = "alert(' Reference Status Not Exists !! Redirecting back to previous page ...');";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    Response.Redirect("frmPurchUnPostList.aspx");
                    return checkStatus;
                }
                return checkStatus;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
                return checkStatus;
            }
        }

        public void calcualteGrossRate()
        {
            if (txtValueRate.Text != "" && txtIGSTValue.Text != "" && txtPriceEqualValue.Text != "" && txtSDValue.Text != "" && txtTRDpercValue.Text != "" && txtEntryValue.Text != "")
            {
                //Basicvalue+Priceequalvalue+vatincaddtax-trddiscvalue/qty
                txtGrossRate.Text = ((Convert.ToDecimal(txtValueRate.Text) + Convert.ToDecimal(txtIGSTValue.Text) + Convert.ToDecimal(txtPriceEqualValue.Text) - Convert.ToDecimal(txtTRDpercValue.Text) - Convert.ToDecimal(txtSDValue.Text)) / Convert.ToDecimal(txtEntryValue.Text)).ToString();
                txtGrossRate_TextChanged(null, null);
            }
            if (txtValueRate.Text != "" && txtSGSTValue.Text != "" && txtCGSTValue.Text != "" && txtPriceEqualValue.Text != "" && txtSDValue.Text != "" && txtTRDpercValue.Text != "" && txtEntryValue.Text != "")
            {
                //Basicvalue+Priceequalvalue+vatincaddtax-trddiscvalue/qty
                txtGrossRate.Text = ((Convert.ToDecimal(txtValueRate.Text) + Convert.ToDecimal(txtSGSTValue.Text) + Convert.ToDecimal(txtCGSTValue.Text) + Convert.ToDecimal(txtPriceEqualValue.Text) - Convert.ToDecimal(txtTRDpercValue.Text) - Convert.ToDecimal(txtSDValue.Text)) / Convert.ToDecimal(txtEntryValue.Text)).ToString();
                txtGrossRate_TextChanged(null, null);
            }
            if (txtValueRate.Text != "" && txtUGSTValue.Text != "" && txtCGSTValue.Text != "" && txtPriceEqualValue.Text != "" && txtSDValue.Text != "" && txtTRDpercValue.Text != "" && txtEntryValue.Text != "")
            {
                //Basicvalue+Priceequalvalue+vatincaddtax-trddiscvalue/qty
                txtGrossRate.Text = ((Convert.ToDecimal(txtValueRate.Text) + Convert.ToDecimal(txtUGSTValue.Text) + Convert.ToDecimal(txtCGSTValue.Text) + Convert.ToDecimal(txtPriceEqualValue.Text) - Convert.ToDecimal(txtTRDpercValue.Text) - Convert.ToDecimal(txtSDValue.Text)) / Convert.ToDecimal(txtEntryValue.Text)).ToString();
                txtGrossRate_TextChanged(null, null);
            }
        }

        private bool ValidateManualEntryExcel()
        {
            bool b = true;

            if (txtInvoiceNo.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice No.";
                txtInvoiceNo.Focus();
                b = false;
                return b;
            }
            if (txtInvoiceDate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice Date.";
                txtInvoiceDate.Focus();
                b = false;
                return b;
            }
            //if (txtReceiptValue.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide Receipt Value.";
            //    txtReceiptValue.Focus();
            //    b = false;
            //    return b;
            //}
            if (DrpState.SelectedItem.Text == "-Select-")
            {
                LblMessage.Text = "► Please Select State.";
                DrpState.Focus();
                b = false;
                return b;
            }
            if (txtvehicleNo.Text == string.Empty)
            {
                LblMessage.Text = "► Please Enter Vehicle No.";
                txtvehicleNo.Focus();
                b = false;
                return b;
            }
            if (txtAddr.Text == string.Empty)
            {
                LblMessage.Text = "► Please Enter Plant Address.";
                txtAddr.Focus();
                b = false;
                return b;
            }

            return b;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //=================Validaion======Check InvoiceNo=================
                DataTable dtChk = new DataTable();


                CreamBell_DMS_WebApps.App_Code.Global obj1 = new Global();
                string chkInvoice = "select * from [ax].[ACXPURCHINVRECIEPTHEADER] where Sale_InvoiceNo='" + txtInvoiceNo.Text + "' and DataAreaid='" + Session["DATAAREAID"].ToString() + "' and Site_Code='" + Session["SiteCode"].ToString() + "'";
                dtChk = obj1.GetData(chkInvoice);
                if (dtChk.Rows.Count > 0)
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('InvoiceNo: " + txtInvoiceNo.Text + " already exists  !');", true);
                    LblMessage.Text = "InvoiceNo: " + txtInvoiceNo.Text + " already exists  !";
                    //string message = "alert('InvoiceNo: " + txtInvoiceNo.Text + " already exists  !');";
                    // ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);

                    return;
                }
                //==================Check Date=======================

                DateTime InvoiceDate, ReceiptDate;

                //Code By Savita
                if (string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
                {

                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Invoice Date can't empty please select  !');", true);
                    LblMessage.Text = "Invoice Date can't empty please select  !";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Invoice Date can't empty please select  !')", true);
                    //string message = "alert('Invoice Date can't empty please select  !');";
                    //ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);

                    return;
                }

                InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
                ReceiptDate = Convert.ToDateTime(txtReceiptDate.Text);

                if (InvoiceDate > ReceiptDate)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Invoice Date cannot grater than Receipt Date !');", true);

                    // string message = "alert('Invoice Date can't grater than Receipt Date !');";
                    //ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);

                    return;
                }
                bool b = ValidateManualEntryExcel();
                if (b)
                {
                    if (AsyncFileUpload1.HasFile)
                    {
                        //  #region
                        string fileName = System.IO.Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                        AsyncFileUpload1.PostedFile.SaveAs(Server.MapPath("~/Uploads/" + fileName));
                        string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                        string conString = string.Empty;
                        string extension = Path.GetExtension(AsyncFileUpload1.PostedFile.FileName);
                        DataTable dtExcelData = new DataTable();

                        //excel upload
                        dtExcelData = CreamBell_DMS_WebApps.App_Code.ExcelUpload.ImportExcelXLS(Server.MapPath("~/Uploads/" + fileName), true);
                        string strCode = string.Empty;
                        CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                        conn = obj.GetConnection();

                        #region PO  Number Generate
                        string _query = "SELECT ISNULL(MAX(CAST(RIGHT(DOCUMENT_NO,7) AS INT)),0)+1 FROM [ax].[ACXPURCHINVRECIEPTHEADERPRE] where SITE_CODE='" + Session["SiteCode"].ToString() + "'";
                        cmd = new SqlCommand(_query);
                        transaction = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.Text;
                        object vc = cmd.ExecuteScalar();
                        DataTable dtNumSeq = obj.GetNumSequenceNew(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                        string NUMSEQ = string.Empty;
                        if (dtNumSeq != null)
                        {
                            strCode = dtNumSeq.Rows[0][0].ToString();
                            NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                        }
                        else
                        {
                            return;
                        }

                        strCode = obj.GetNumSequence(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());
                        Session["PreDocument_No"] = null;
                        Session["PreDocument_No"] = strCode;
                        cmd.CommandText = string.Empty;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[ACX_ACXPURCHINVRECIEPTHEADERPRE]";

                        #endregion

                        #region Header Insert Data

                        Random rnd = new Random();
                        int SO_NO = rnd.Next(40000, 1000000);                           //these all are temporary values to insert--Kinldy remove it after clearance..[Rahul]


                        //DataTable dt = Session["LineItem"] as DataTable;
                        //decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Value"));                  //For Header ReturnDocValue

                        string queryRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTHEADERPRE]";
                        Int64 Recid = Convert.ToInt64(obj.GetScalarValue(queryRecID));

                        cmd.Parameters.Clear();
                        string indentno = string.Empty;
                        if (DrpIndentNo.SelectedItem.Text.ToString() == "-Select-")
                        {
                            indentno = "";
                        }
                        else
                        {
                            indentno = DrpIndentNo.SelectedItem.Text.ToString();
                        }
                        cmd.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@Purchase_Indent_No", indentno);
                        cmd.Parameters.AddWithValue("@Purchase_Indent_Date", txtIndentDate.Text);
                        cmd.Parameters.AddWithValue("@Transporter_Code", txtTransporterName.Text);
                        cmd.Parameters.AddWithValue("@Document_No", strCode);
                        cmd.Parameters.AddWithValue("@DocumentDate", txtReceiptDate.Text);
                        cmd.Parameters.AddWithValue("@VEHICAL_No", txtvehicleNo.Text);
                        cmd.Parameters.AddWithValue("@Purchase_Reciept_No", strCode);
                        cmd.Parameters.AddWithValue("@Sale_InvoiceNo", txtInvoiceNo.Text);
                        cmd.Parameters.AddWithValue("@Sale_InvoiceDate", txtInvoiceDate.Text);
                        cmd.Parameters.AddWithValue("@VEHICAL_Type", txtVehicleType.Text);
                        cmd.Parameters.AddWithValue("@SO_No", string.Empty);
                        cmd.Parameters.AddWithValue("@Material_Value", "0");
                        //cmd.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(txtTotalValue.Text));
                        cmd.Parameters.AddWithValue("@recid", Recid + 1);
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@STATUS", 0);                  // for UnPosted Purchase Invoice Receipt Status//
                        cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                        cmd.Parameters.AddWithValue("@DRIVERNAME", txttransporterNo.Text.Trim().ToString());
                        cmd.Parameters.AddWithValue("@PlantName", DrpPlant.SelectedItem.Value.ToString());
                        cmd.Parameters.AddWithValue("@PlantAddress", txtAddr.Text);
                        cmd.Parameters.AddWithValue("@PlantCity", txtPlantCity.Text);
                        cmd.Parameters.AddWithValue("@PlantPostcode", txtPostalCode.Text);
                        cmd.Parameters.AddWithValue("@OrderType", drpOrderType.SelectedItem.Text.ToString());
                        cmd.Parameters.AddWithValue("@PLANT_STATECODE", DrpState.SelectedValue.ToString());
                        if (txtGSTNNumber.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@PlantGSTNNumber", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@PlantGSTNNumber", txtGSTNNumber.Text);
                        }
                        if (DrpCompScheme.SelectedItem.Text == "--Select--")
                        {
                            cmd.Parameters.AddWithValue("@CompositionScheme", "0");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CompositionScheme", DrpCompScheme.SelectedValue.ToString());
                        }
                        if (txtGSTRegistrationDate.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@GSTRegistrationDate", "1900-01-01");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@GSTRegistrationDate", Convert.ToDateTime(txtGSTRegistrationDate.Text));
                        }

                        //txtPurchDocumentNo.Text = strCode;
                        //transaction.Commit();
                        #endregion


                        #region Line Insert Data on Same PURCH Order Number

                        cmd1 = new SqlCommand("[ACX_ACXPURCHINVRECIEPTLINEPRE]");
                        //transaction = conn.BeginTransaction();
                        cmd1.Connection = conn;
                        if (transaction == null)
                        {
                            transaction = conn.BeginTransaction();
                        }
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;
                        string queryRecidLine = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTLINEPRE]";
                        Int64 RecidLine = Convert.ToInt64(obj.GetScalarValue(queryRecidLine));
                        #endregion

                        string productNameCode = txtProductDesc.Text;
                        string[] str = productNameCode.Split('-');
                        string productCode = str[0].ToString();
                        //strCode = txtPurchDocumentNo.Text;

                        DataTable dtForShownUnuploadData = new DataTable();
                        dtForShownUnuploadData.Columns.Add("ProductCode");
                        dtForShownUnuploadData.Columns.Add("Qty");
                        dtForShownUnuploadData.Columns.Add("Rate");
                        dtForShownUnuploadData.Columns.Add("Trade_Disc_Per");
                        dtForShownUnuploadData.Columns.Add("Trade_Disc_Val");
                        dtForShownUnuploadData.Columns.Add("PE_Val");
                        dtForShownUnuploadData.Columns.Add("Spl_Disc_Per");
                        dtForShownUnuploadData.Columns.Add("Spl_Disc_Val");
                        dtForShownUnuploadData.Columns.Add("SGST_Per");
                        dtForShownUnuploadData.Columns.Add("UGST_Per");
                        dtForShownUnuploadData.Columns.Add("CGST_Per");
                        dtForShownUnuploadData.Columns.Add("IGST_Per");
                        dtForShownUnuploadData.Columns.Add("Remark");
                        int j = 0;
                        int no = 0;
                        Boolean TaxMissMatch;
                        for (int i = 0; i < dtExcelData.Rows.Count; i++)
                        {
                            TaxMissMatch = false;
                            string sqlstr = "select ItemID from ax.inventTable where ItemID = '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "' and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
                            object objcheckproductcode = obj.GetScalarValue(sqlstr);
                            decimal Qty = Convert.ToDecimal((dtExcelData.Rows[i]["Qty"].ToString() == "" ? "0" : dtExcelData.Rows[i]["Qty"].ToString()));
                            decimal Rate = Convert.ToDecimal((dtExcelData.Rows[i]["Rate"].ToString() == "" ? "0" : dtExcelData.Rows[i]["Rate"].ToString()));
                            string ProductCode = dtExcelData.Rows[i]["ProductCode"].ToString();
                            decimal decBasicValue, decTdPer, decTdValue, decSpPer, decSpValue, decIgst, decUgst, decCgst, decSgst, decPeValue, decTax1, decTax2, decLineValue, decTaxable;
                            string strTax1Component, strTax2Component;
                            strTax1Component = strTax2Component = "";
                            decBasicValue = Qty * Rate;
                            decTdPer = Convert.ToDecimal((dtExcelData.Rows[i]["Trade_Disc_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["Trade_Disc_Per"].ToString()));
                            decTdValue = Convert.ToDecimal((dtExcelData.Rows[i]["Trade_Disc_Val"].ToString() == "" ? "0" : dtExcelData.Rows[i]["Trade_Disc_Val"].ToString()));
                            decPeValue = Convert.ToDecimal((dtExcelData.Rows[i]["PE_Val"].ToString() == "" ? "0" : dtExcelData.Rows[i]["PE_Val"].ToString()));
                            decSpPer = Convert.ToDecimal((dtExcelData.Rows[i]["Spl_Disc_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["Spl_Disc_Per"].ToString()));
                            decSpValue = Convert.ToDecimal((dtExcelData.Rows[i]["Spl_Disc_Val"].ToString() == "" ? "0" : dtExcelData.Rows[i]["Spl_Disc_Val"].ToString()));
                            decSgst = Convert.ToDecimal((dtExcelData.Rows[i]["SGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["SGST_Per"].ToString()));
                            decUgst = Convert.ToDecimal((dtExcelData.Rows[i]["UGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["UGST_Per"].ToString()));
                            decCgst = Convert.ToDecimal((dtExcelData.Rows[i]["CGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["CGST_Per"].ToString()));
                            decIgst = Convert.ToDecimal((dtExcelData.Rows[i]["IGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["IGST_Per"].ToString()));
                            if (objcheckproductcode == null)
                            {
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = Qty;
                                dtForShownUnuploadData.Rows[j]["Rate"] = Rate;
                                dtForShownUnuploadData.Rows[j]["Trade_Disc_Per"] = decTdPer;
                                dtForShownUnuploadData.Rows[j]["Trade_Disc_Val"] = decTdValue;
                                dtForShownUnuploadData.Rows[j]["PE_Val"] = decPeValue;
                                dtForShownUnuploadData.Rows[j]["Spl_Disc_Per"] = decSpPer;
                                dtForShownUnuploadData.Rows[j]["Spl_Disc_Val"] = decSpValue;
                                dtForShownUnuploadData.Rows[j]["SGST_Per"] = decSgst;
                                dtForShownUnuploadData.Rows[j]["UGST_Per"] = decUgst;
                                dtForShownUnuploadData.Rows[j]["CGST_Per"] = decCgst;
                                dtForShownUnuploadData.Rows[j]["IGST_Per"] = decIgst;
                                dtForShownUnuploadData.Rows[j]["Remark"] = "Product Code Not Present in BU";
                                j += 1;
                                continue;
                            }

                            try
                            {
                                if (Qty == 0 || Rate == 0)
                                {
                                    dtForShownUnuploadData.Rows.Add();
                                    dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                    dtForShownUnuploadData.Rows[j]["Qty"] = Qty;
                                    dtForShownUnuploadData.Rows[j]["Rate"] = Rate;
                                    dtForShownUnuploadData.Rows[j]["Trade_Disc_Per"] = decTdPer;
                                    dtForShownUnuploadData.Rows[j]["Trade_Disc_Val"] = decTdValue;
                                    dtForShownUnuploadData.Rows[j]["PE_Val"] = decPeValue;
                                    dtForShownUnuploadData.Rows[j]["Spl_Disc_Per"] = decSpPer;
                                    dtForShownUnuploadData.Rows[j]["Spl_Disc_Val"] = decSpValue;
                                    dtForShownUnuploadData.Rows[j]["SGST_Per"] = decSgst;
                                    dtForShownUnuploadData.Rows[j]["UGST_Per"] = decUgst;
                                    dtForShownUnuploadData.Rows[j]["CGST_Per"] = decCgst;
                                    dtForShownUnuploadData.Rows[j]["IGST_Per"] = decIgst;
                                    dtForShownUnuploadData.Rows[j]["Remark"] = "Rate or Quantity not defined.";
                                    j += 1;
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = Qty;
                                dtForShownUnuploadData.Rows[j]["Rate"] = Rate;
                                dtForShownUnuploadData.Rows[j]["Trade_Disc_Per"] = decTdPer;
                                dtForShownUnuploadData.Rows[j]["Trade_Disc_Val"] = decTdValue;
                                dtForShownUnuploadData.Rows[j]["PE_Val"] = decPeValue;
                                dtForShownUnuploadData.Rows[j]["Spl_Disc_Per"] = decSpPer;
                                dtForShownUnuploadData.Rows[j]["Spl_Disc_Val"] = decSpValue;
                                dtForShownUnuploadData.Rows[j]["SGST_Per"] = decSgst;
                                dtForShownUnuploadData.Rows[j]["UGST_Per"] = decUgst;
                                dtForShownUnuploadData.Rows[j]["CGST_Per"] = decCgst;
                                dtForShownUnuploadData.Rows[j]["IGST_Per"] = decIgst;
                                dtForShownUnuploadData.Rows[j]["Remark"] = ex.Message.ToString();
                                j += 1;
                                ErrorSignal.FromCurrentContext().Raise(ex);
                                continue;
                            }
                            DataTable dtTax = obj.GetData("[USP_ACX_GetProductGSTRate] '" + objcheckproductcode + "', '" + DrpState.SelectedValue.ToString() + "', '" + Session["SITELOCATION"] + "'");

                            if (dtTax.Rows.Count > 0)
                            {
                                #region Tax Compenent
                                if (dtTax.Rows[0]["RETMSG"].ToString().IndexOf("TRUE") >= 0)
                                {
                                    if (dtTax.Rows[0]["Exempt"].ToString() == "1" || txtGSTNNumber.Text.ToString().Trim() == "" || DrpCompScheme.SelectedValue == "1")
                                    {
                                        decCgst = decSgst = decIgst = decUgst = 0;
                                    }
                                    else if (dtTax.Rows[0]["Tax1Component"].ToString() == "IGST")
                                    {
                                        if (decIgst <= 0)
                                        {
                                            decIgst = Convert.ToDecimal(dtTax.Rows[0]["Tax1_PER"].ToString());
                                        }
                                        if (Convert.ToDecimal(dtTax.Rows[0]["Tax1_PER"].ToString()) != Convert.ToDecimal((dtExcelData.Rows[i]["IGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["IGST_Per"].ToString())))
                                        {
                                            TaxMissMatch = true;
                                            goto TaxMissMatch;
                                        }
                                        decCgst = decSgst = decUgst = 0;
                                        strTax1Component = dtTax.Rows[0]["Tax1Component"].ToString(); strTax2Component = dtTax.Rows[0]["Tax2Component"].ToString();
                                    }
                                    else if (dtTax.Rows[0]["Tax1Component"].ToString() == "SGST")
                                    {
                                        if (decSgst <= 0)
                                        {
                                            decSgst = Convert.ToDecimal(dtTax.Rows[0]["Tax1_PER"].ToString());
                                        }
                                        if (decCgst <= 0)
                                        {
                                            decCgst = Convert.ToDecimal(dtTax.Rows[0]["Tax2_PER"].ToString());
                                        }
                                        if (Convert.ToDecimal(dtTax.Rows[0]["Tax1_PER"].ToString()) != Convert.ToDecimal((dtExcelData.Rows[i]["SGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["SGST_Per"].ToString())))
                                        {
                                            TaxMissMatch = true;
                                            goto TaxMissMatch;
                                        }
                                        if (Convert.ToDecimal(dtTax.Rows[0]["Tax2_PER"].ToString()) != Convert.ToDecimal((dtExcelData.Rows[i]["CGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["CGST_Per"].ToString())))
                                        {
                                            TaxMissMatch = true;
                                            goto TaxMissMatch;
                                        }
                                        strTax1Component = dtTax.Rows[0]["Tax1Component"].ToString(); strTax2Component = dtTax.Rows[0]["Tax2Component"].ToString();
                                        decIgst = decUgst = 0;
                                    }
                                    else if (dtTax.Rows[0]["Tax1Component"].ToString() == "UGST")
                                    {
                                        if (decUgst <= 0)
                                        {
                                            decUgst = Convert.ToDecimal(dtTax.Rows[0]["Tax1_PER"].ToString());
                                        }
                                        if (decCgst <= 0)
                                        {
                                            decCgst = Convert.ToDecimal(dtTax.Rows[0]["Tax2_PER"].ToString());
                                        }
                                        if (Convert.ToDecimal(dtTax.Rows[0]["Tax1_PER"].ToString()) != Convert.ToDecimal((dtExcelData.Rows[i]["UGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["UGST_Per"].ToString())))
                                        {
                                            TaxMissMatch = true;
                                            goto TaxMissMatch;
                                        }
                                        if (Convert.ToDecimal(dtTax.Rows[0]["Tax2_PER"].ToString()) != Convert.ToDecimal((dtExcelData.Rows[i]["CGST_Per"].ToString() == "" ? "0" : dtExcelData.Rows[i]["CGST_Per"].ToString())))
                                        {
                                            TaxMissMatch = true;
                                            goto TaxMissMatch;
                                        }
                                        decIgst = decSgst = 0;
                                        strTax1Component = dtTax.Rows[0]["Tax1Component"].ToString(); strTax2Component = dtTax.Rows[0]["Tax2Component"].ToString();
                                    }
                                    else
                                    {
                                        dtForShownUnuploadData.Rows.Add();
                                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                        dtForShownUnuploadData.Rows[j]["Qty"] = Qty;
                                        dtForShownUnuploadData.Rows[j]["Rate"] = Rate;
                                        dtForShownUnuploadData.Rows[j]["Trade_Disc_Per"] = decTdPer;
                                        dtForShownUnuploadData.Rows[j]["Trade_Disc_Val"] = decTdValue;
                                        dtForShownUnuploadData.Rows[j]["PE_Val"] = decPeValue;
                                        dtForShownUnuploadData.Rows[j]["Spl_Disc_Per"] = decSpPer;
                                        dtForShownUnuploadData.Rows[j]["Spl_Disc_Val"] = decSpValue;
                                        dtForShownUnuploadData.Rows[j]["SGST_Per"] = decSgst;
                                        dtForShownUnuploadData.Rows[j]["UGST_Per"] = decUgst;
                                        dtForShownUnuploadData.Rows[j]["CGST_Per"] = decCgst;
                                        dtForShownUnuploadData.Rows[j]["IGST_Per"] = decIgst;
                                        dtForShownUnuploadData.Rows[j]["Remark"] = "Tax Setup not defined";
                                        j += 1;
                                        continue;
                                    }

                                }
                                else
                                {
                                    dtForShownUnuploadData.Rows.Add();
                                    dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                    dtForShownUnuploadData.Rows[j]["Qty"] = Qty;
                                    dtForShownUnuploadData.Rows[j]["Rate"] = Rate;
                                    dtForShownUnuploadData.Rows[j]["Trade_Disc_Per"] = decTdPer;
                                    dtForShownUnuploadData.Rows[j]["Trade_Disc_Val"] = decTdValue;
                                    dtForShownUnuploadData.Rows[j]["PE_Val"] = decPeValue;
                                    dtForShownUnuploadData.Rows[j]["Spl_Disc_Per"] = decSpPer;
                                    dtForShownUnuploadData.Rows[j]["Spl_Disc_Val"] = decSpValue;
                                    dtForShownUnuploadData.Rows[j]["SGST_Per"] = decSgst;
                                    dtForShownUnuploadData.Rows[j]["UGST_Per"] = decUgst;
                                    dtForShownUnuploadData.Rows[j]["CGST_Per"] = decCgst;
                                    dtForShownUnuploadData.Rows[j]["IGST_Per"] = decIgst;
                                    dtForShownUnuploadData.Rows[j]["Remark"] = dtTax.Rows[0]["RETMSG"].ToString();
                                    j += 1;
                                    continue;
                                }
                                #endregion
                            }
                            cmd1.Parameters.Clear();
                            if (strTax1Component == "" && dtTax.Rows[0]["Exempt"].ToString() == "0" && txtGSTNNumber.Text.ToString().Trim() != "" && DrpCompScheme.SelectedValue != "1")
                            {
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = Qty;
                                dtForShownUnuploadData.Rows[j]["Rate"] = Rate;
                                dtForShownUnuploadData.Rows[j]["Trade_Disc_Per"] = decTdPer;
                                dtForShownUnuploadData.Rows[j]["Trade_Disc_Val"] = decTdValue;
                                dtForShownUnuploadData.Rows[j]["PE_Val"] = decPeValue;
                                dtForShownUnuploadData.Rows[j]["Spl_Disc_Per"] = decSpPer;
                                dtForShownUnuploadData.Rows[j]["Spl_Disc_Val"] = decSpValue;
                                dtForShownUnuploadData.Rows[j]["SGST_Per"] = decSgst;
                                dtForShownUnuploadData.Rows[j]["UGST_Per"] = decUgst;
                                dtForShownUnuploadData.Rows[j]["CGST_Per"] = decCgst;
                                dtForShownUnuploadData.Rows[j]["IGST_Per"] = decIgst;
                                dtForShownUnuploadData.Rows[j]["Remark"] = "Tax Not defined. ";
                                j += 1;
                                continue;
                            }
                            TaxMissMatch:
                            if (TaxMissMatch)
                            {

                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = Qty;
                                dtForShownUnuploadData.Rows[j]["Rate"] = Rate;
                                dtForShownUnuploadData.Rows[j]["Trade_Disc_Per"] = decTdPer;
                                dtForShownUnuploadData.Rows[j]["Trade_Disc_Val"] = decTdValue;
                                dtForShownUnuploadData.Rows[j]["PE_Val"] = decPeValue;
                                dtForShownUnuploadData.Rows[j]["Spl_Disc_Per"] = decSpPer;
                                dtForShownUnuploadData.Rows[j]["Spl_Disc_Val"] = decSpValue;
                                dtForShownUnuploadData.Rows[j]["SGST_Per"] = decSgst;
                                dtForShownUnuploadData.Rows[j]["UGST_Per"] = decUgst;
                                dtForShownUnuploadData.Rows[j]["CGST_Per"] = decCgst;
                                dtForShownUnuploadData.Rows[j]["IGST_Per"] = decIgst;
                                dtForShownUnuploadData.Rows[j]["Remark"] = "Input Tax % Missmatch as per System.";
                                j += 1;
                                continue;
                            }
                            string[] ReturnArray = null;
                            ReturnArray = obj.CalculatePrice1(dtExcelData.Rows[i]["ProductCode"].ToString(), string.Empty, Convert.ToDecimal(dtExcelData.Rows[i]["Qty"].ToString()), DDLEntryType.SelectedItem.Text.ToString());
                            if (ReturnArray != null)
                            {
                                cmd1.Parameters.AddWithValue("@RECID", RecidLine + 1 + i);
                                cmd1.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                                cmd1.Parameters.AddWithValue("@Purchase_Reciept_No", strCode);
                                cmd1.Parameters.AddWithValue("@PRODUCT_CODE", dtExcelData.Rows[i]["ProductCode"].ToString());
                                cmd1.Parameters.AddWithValue("@LINE_NO", RecidLine + 1 + i);
                                cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());

                                #region Crate Box Data Insert

                                if (ReturnArray[5].ToString() == "Box")
                                {
                                    cmd1.Parameters.AddWithValue("@BOX", Qty);
                                    cmd1.Parameters.AddWithValue("@CRATES", ReturnArray[0].ToString());
                                    cmd1.Parameters.AddWithValue("@LTR", ReturnArray[1].ToString());
                                    cmd1.Parameters.AddWithValue("@RATE", Rate);
                                    cmd1.Parameters.AddWithValue("@UOM", ReturnArray[4].ToString());
                                    cmd1.Parameters.AddWithValue("@BASICVALUE", decBasicValue);
                                    if (decTdPer > 0)
                                    {
                                        decTdValue = decBasicValue * decTdPer / 100;
                                    }
                                    else if (decTdValue > 0)
                                    {
                                        decTdPer = (decTdValue / decBasicValue) * 100;
                                    }
                                    if (decSpPer > 0)
                                    {
                                        decSpValue = decBasicValue * decSpPer / 100;
                                    }
                                    else if (decSpValue > 0)
                                    {
                                        decSpPer = (decSpValue / decBasicValue) * 100;
                                    }
                                    decTaxable = decBasicValue - decTdValue - decSpValue + decPeValue;
                                    decTax1 = decTaxable * (decIgst + decSgst + decUgst) / 100;
                                    decTax2 = decTaxable * (decCgst) / 100;
                                    decLineValue = decTaxable + decTax1 + decTax2;
                                    cmd1.Parameters.AddWithValue("@TRDDISCPERC", decTdPer);
                                    cmd1.Parameters.AddWithValue("@SpecialDiscPerc", decSpPer);


                                    cmd1.Parameters.AddWithValue("@TRDDISCVALUE", decTdValue);
                                    cmd1.Parameters.AddWithValue("@SpecialDiscValue", decSpValue);
                                    cmd1.Parameters.AddWithValue("@PRICEEQUALVALUE", decPeValue);

                                    decimal VatIncPerc = decSgst + decIgst + decUgst + decCgst;
                                    cmd1.Parameters.AddWithValue("@VAT_INC_PERC", VatIncPerc.ToString());
                                    cmd1.Parameters.AddWithValue("@VAT_INC_PERC_VALUE", decTax1 + decTax2);
                                    cmd1.Parameters.AddWithValue("@GROSSAMOUNT", decTaxable);
                                    cmd1.Parameters.AddWithValue("@DISCOUNT", 0);
                                    cmd1.Parameters.AddWithValue("@TAX", decIgst + decSgst + decUgst);
                                    cmd1.Parameters.AddWithValue("@TAXAMOUNT", decTax1);
                                    cmd1.Parameters.AddWithValue("@ADDTAX", decCgst);
                                    cmd1.Parameters.AddWithValue("@ADDTAXAMOUNT", decTax2);
                                    cmd1.Parameters.AddWithValue("@TaxComponent", strTax1Component);
                                    cmd1.Parameters.AddWithValue("@ADDTaxComponent", strTax2Component);
                                    cmd1.Parameters.AddWithValue("@AMOUNT", decLineValue);
                                    cmd1.Parameters.AddWithValue("@Remark", dtExcelData.Rows[i]["Remark"].ToString());
                                    cmd1.ExecuteNonQuery();
                                    no += 1;
                                }
                                #endregion
                            }

                        }

                        //cmd.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(txtTotalValue.Text));
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        ShowRecords(strCode);
                        UpdateAndShowTotalMaterialValue(strCode);
                        //ResetPageUnPostedData();
                        BtnUpdateHeader.Visible = true;
                        LblMessage.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : " + no + " Records.";

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
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
                transaction.Rollback();
            }
        }

        protected void rdoManualEntry_CheckedChanged(object sender, EventArgs e)
        {
            imgBtnGetInvoiceData.Enabled = false;
            lblIndno.Text = "Indent No";
            DrpIndentNo.Visible = true;
            drpGstInvoice.Visible = false;
            {
                if (rdoManualEntry.Checked == true)
                {
                    txtInvoiceDate.Enabled = true;
                    DrpSalesInvoice.Visible = false;
                    txtInvoiceNo.Visible = true;

                    #region Manual Entry

                    panelAddLine.Visible = true;
                    PanelManaulEntry.Visible = true;
                    AsyncFileUpload1.Visible = false;
                    btnUplaod.Visible = false;
                    LblMessage.Text = "";
                    imgBtnGetInvoiceData.Visible = false;

                    txtIndentDate.Text = string.Empty;
                    txtTransporterName.Text = string.Empty;
                    txttransporterNo.Text = string.Empty;
                    txtvehicleNo.Text = string.Empty;
                    txtInvoiceNo.Text = string.Empty;
                    //txtInvoiceNo.ReadOnly = true;
                    DrpSalesInvoice.SelectedIndex = 0;
                    //txtInvoiceDate.Text = string.Empty;
                    txtVehicleType.Text = string.Empty;
                    txtReceiptValue.Text = string.Empty;
                    GridPurchItems.DataSource = null;
                    GridPurchItems.Visible = false;
                    txtPurchDocumentNo.Text = string.Empty;

                    //txtPlantName.Text = "";
                    txtPostalCode.Text = "";
                    txtAddr.Text = "";
                    txtPlantCity.Text = "";
                    DrpState.Enabled = true;

                    GridPurchItems.DataSource = null;
                    GridPurchItems.DataBind();
                    txtPurchDocumentNo.Text = "";

                    #endregion
                }
                else if (rdoExcelEntry.Checked == true)
                {
                    txtInvoiceDate.Enabled = true;
                    DrpSalesInvoice.Visible = false;
                    drpOrderType.Visible = true;
                    DrpPlant.Visible = true;
                    Plant.Visible = false;
                    ordertype.Visible = false;
                    txtInvoiceNo.Visible = true;
                    txtState.Visible = false;
                    DrpState.Visible = true;
                    #region Excel Entry

                    panelAddLine.Visible = false;
                    PanelManaulEntry.Visible = false;
                    AsyncFileUpload1.Visible = true;
                    btnUplaod.Visible = true;
                    LblMessage.Text = "";
                    imgBtnGetInvoiceData.Visible = false;

                    txtIndentDate.Text = string.Empty;
                    txtTransporterName.Text = string.Empty;
                    txttransporterNo.Text = string.Empty;
                    txtvehicleNo.Text = string.Empty;
                    txtInvoiceNo.Text = string.Empty;
                    DrpSalesInvoice.SelectedIndex = 0;
                    //txtInvoiceDate.Text = string.Empty;
                    txtVehicleType.Text = string.Empty;
                    txtReceiptValue.Text = string.Empty;
                    GridPurchItems.DataSource = null;
                    GridPurchItems.Visible = false;
                    txtPurchDocumentNo.Text = string.Empty;
                    txtPurchDocumentNo.Text = "";
                    DrpState.Enabled = true;
                    txtGSTNNumber.Text = string.Empty;
                    DrpCompScheme.SelectedIndex = 0;
                    txtGSTRegistrationDate.Text = string.Empty;

                    txtvehicleNo.Enabled = true;
                    txtAddr.Enabled = true;
                    txtGSTNNumber.Enabled = true;
                    DrpCompScheme.Enabled = true;
                    txtTransporterName.Enabled = true;
                    txttransporterNo.Enabled = true;
                    txtVehicleType.Enabled = true;
                    txtPlantCity.Enabled = true;
                    txtPostalCode.Enabled = true;
                    txtReceiptDate.Enabled = true;
                    txtReceiptValue.Enabled = true;
                    txtIndentDate.Enabled = true;
                    txtGSTNNumber.Enabled = true;
                    DrpCompScheme.Enabled = true;
                    //txtPlantName.Text = "";
                    txtPostalCode.Text = "";
                    txtAddr.Text = "";
                    txtPlantCity.Text = "";

                    GridPurchItems.DataSource = null;
                    GridPurchItems.DataBind();

                    #endregion

                }
                else if (rdoSAPFetchData.Checked == true)
                {
                    txtInvoiceDate.Enabled = false;
                    DrpSalesInvoice.Visible = true;
                    drpOrderType.Visible = false;
                    DrpPlant.Visible = false;
                    Plant.Visible = true;
                    ordertype.Visible = true;
                    txtInvoiceNo.Visible = false;
                    txtState.Visible = true;
                    DrpState.Visible = false;
                    #region SAP

                    panelAddLine.Visible = false;
                    PanelManaulEntry.Visible = false;
                    AsyncFileUpload1.Visible = false;
                    btnUplaod.Visible = false;
                    LblMessage.Text = "";
                    imgBtnGetInvoiceData.Visible = false;
                    BtnUpdateHeader.Visible = false;
                    lblIndno.Text = "GST Invoice No";
                    DrpIndentNo.Visible = false;
                    drpGstInvoice.Visible = true;

                    txtIndentDate.Text = string.Empty;
                    txtTransporterName.Text = string.Empty;
                    txttransporterNo.Text = string.Empty;
                    txtvehicleNo.Text = string.Empty;
                    txtInvoiceNo.ReadOnly = false;
                    txtInvoiceNo.Text = string.Empty;
                    ordertype.Text = string.Empty;
                    Plant.Text = string.Empty;
                    txtGSTNNumber.Text = string.Empty;
                    DrpCompScheme.SelectedIndex = 0;
                    txtState.Text = string.Empty;

                    DrpSalesInvoice.SelectedIndex = 0;
                    drpGstInvoice.SelectedIndex = 0;
                    //txtInvoiceDate.Text = string.Empty;
                    txtVehicleType.Text = string.Empty;
                    txtReceiptValue.Text = string.Empty;
                    GridPurchItems.DataSource = null;
                    GridPurchItems.Visible = false;
                    txtPurchDocumentNo.Text = string.Empty;
                    txtPurchDocumentNo.Text = "";

                    txtvehicleNo.Enabled = false;
                    txtAddr.Enabled = false;
                    txtGSTNNumber.Enabled = false;
                    DrpCompScheme.Enabled = false;
                    txtTransporterName.Enabled = false;
                    txttransporterNo.Enabled = false;
                    txtVehicleType.Enabled = false;
                    txtPlantCity.Enabled = false;
                    txtPostalCode.Enabled = false;
                    txtReceiptDate.Enabled = false;
                    txtReceiptValue.Enabled = false;
                    txtIndentDate.Enabled = false;
                    txtGSTNNumber.Enabled = false;
                    DrpCompScheme.Enabled = false;
                    txtPostalCode.Text = "";
                    txtAddr.Text = "";
                    txtPlantCity.Text = "";

                    GridPurchItems.DataSource = null;
                    GridPurchItems.DataBind();
                    txtInvoiceNo.Focus();

                    #endregion
                }
            }
        }

        protected void imgBtnGetInvoiceData_Click(object sender, ImageClickEventArgs e)
        {
            //=================Validaion======Check InvoiceNo=================
            string SavingInvoiceNo = "";
            if (rdoSAPFetchData.Checked)
            {
                SavingInvoiceNo = DrpSalesInvoice.SelectedValue;
            }
            else
            {
                SavingInvoiceNo = txtInvoiceNo.Text;
            }
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string chkInvoice = "select Top 1 Sale_InvoiceNo from [ax].[ACXPURCHINVRECIEPTHEADER] where Sale_InvoiceNo ='" + SavingInvoiceNo + "' and DataAreaid='" + Session["DATAAREAID"].ToString() + "' and Site_Code='" + Session["SiteCode"].ToString() + "'";
            object objChk = obj.GetScalarValue(chkInvoice);

            if (objChk != null)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('InvoiceNo: " + txtInvoiceNo.Text + " Already exists  !');", true);
                string message = "alert('InvoiceNo: " + SavingInvoiceNo + " Already exists  !');";
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                txtInvoiceDate.ReadOnly = false;
                txtReceiptValue.ReadOnly = false;
                return;
            }
            else
            {
                bool b = ValidateSAPInvoice();
                if (b)
                {
                    bool dataExistOrNot = GetDataFromSAPInvoice();
                    if (dataExistOrNot)
                    {

                        //  CalendarExtender2.StartDate = Convert.ToDateTime(txtInvoiceDate.Text);
                        //  CalendarExtender2.EndDate = Convert.ToDateTime(txtInvoiceDate.Text);
                        txtReceiptValue.ReadOnly = true;
                        //txtReceiptDate.ReadOnly = true;
                        txtReceiptDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);// DateTime.Today.ToString();
                        //CalendarExtender3.StartDate = DateTime.Now;
                    }
                }
            }
            //================================================================

        }

        private bool ValidateSAPInvoice()
        {
            bool b = false;


            if (DrpSalesInvoice.SelectedValue == string.Empty)
            {
                LblMessage.Text = "► Please Provide the SAP Reference Invoice Number :-";
                txtInvoiceNo.Focus();
                b = false;
                return b;
            }
            else
            {
                LblMessage.Text = string.Empty;
                b = true;
            }
            return b;
        }
        private void FillPendingSalesInvoice()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                string strQuery = "EXEC usp_GetPendingSalesInvoice '" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.FillDataTable(strQuery);
                //DataRow dr = dt.NewRow();
                //dr[0] = "-Select-";
                //dr[1] = "-Select-";
                //DrpSalesInvoice.DataSource = dt;
                //drpGstInvoice.DataSource = dt;
                //drpGstInvoice.DataMember = "OdnNo";
                //drpGstInvoice.DataValueField = "OdnNo";
                //DrpSalesInvoice.DataValueField = "INVOICE_NO";
                //DrpSalesInvoice.DataMember = "INVOICE_NO";
                DrpSalesInvoice.Items.Clear();
                DrpSalesInvoice.Items.Add("-Select-");
                obj.BindToDropDown(DrpSalesInvoice, strQuery, "INVOICE_NO", "INVOICE_NO");
                drpGstInvoice.Items.Clear();
                drpGstInvoice.Items.Add("-Select-");
                obj.BindToDropDown(drpGstInvoice, strQuery, "OdnNo", "OdnNo");
                Session[SessionPendingInv] = dt;
            }

            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
        private bool GetDataFromSAPInvoice()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dtHeader = null;
            DataTable dtLine = null;
            DataTable dtLineItems = null;
            string SavingInvoiceNo = "";
            if (rdoSAPFetchData.Checked)
            {
                SavingInvoiceNo = DrpSalesInvoice.SelectedValue;
            }
            else
            {
                SavingInvoiceNo = txtInvoiceNo.Text;
            }

            try
            {
                #region Query




                string queryHeader = " Select SO_NO,SO_DATE, CUSTOMER_INDENT_NO, TRANSPORTORCODE,VEHICLENO, VEHICLETYPE, INVOICE_DATE, INVOICE_VALUE,DRIVERNAME,Plant_code " +
                                       ",PLANT_NAME,PLANT_ADD1 + '' + PLANT_ADD2 as Addr , PLANT_CITY, PLANT_POSTCODE, CASE WHEN LEN(ISNULL(ORDERTYPECODE,''))>0 THEN ORDERTYPECODE ELSE 'YGSO' END ORDERTYPECODE, ODNNO, SSI.GSTINNO, " +
                                       " cast(cast(SSI.GSTREGISTRATIONDATE as date) as varchar(20)) GSTREGISTRATIONDATE, SSI.COMPOSITIONSCHEME,  " +
                                       " PLANT_STATE_CODE" +
                                       " from [ax].[ACX_STAGINGSALESINVOICE] SSI " +
                                     " INNER JOIN AX.INVENTSITE INVST ON INVST.SITEID = SSI.CUSTOMER_CODE " +
                                     " WHERE cast(INVOICE_NO as Bigint) ='" + SavingInvoiceNo + "' AND CUSTOMER_CODE = '" + Session["SiteCode"].ToString() + "'";

                //string queryHeader = " Select Sum(Line_Amount) as RecptValue,SO_NO,SO_DATE, CUSTOMER_INDENT_NO, TRANSPORTORCODE,VEHICLENO, VEHICLETYPE, INVOICE_DATE, INVOICE_VALUE,DRIVERNAME " +
                //                      ", PLANT_NAME,PLANT_ADD1 + '' + PLANT_ADD2 as Addr , PLANT_CITY, PLANT_POSTCODE " +
                //                      " from [ax].[ACX_STAGINGSALESINVOICE] SSI " +
                //                    " INNER JOIN AX.INVENTSITE INVST ON INVST.SITEID = SSI.CUSTOMER_CODE AND INVST.DATAAREAID = SSI.DATAAREAID " +
                //                    " WHERE cast(INVOICE_NO as Bigint) ='" + txtInvoiceNo.Text.Trim() + "' AND CUSTOMER_CODE = '" + Session["SiteCode"].ToString() + "'" +
                //                    " and SSI.DATAAREAID='" + Session["DATAAREAID"].ToString() + "'"+
                //                    "  group by INVOICE_NO,SO_NO,SO_DATE, CUSTOMER_INDENT_NO, TRANSPORTORCODE,VEHICLENO, VEHICLETYPE, INVOICE_DATE, INVOICE_VALUE,DRIVERNAME , PLANT_NAME,PLANT_ADD1, PLANT_ADD2, PLANT_CITY, PLANT_POSTCODE ";

                /* string queryLine = " Select SSI.RECID as LINE_NO, MATERIAL_CODE AS PRODUCTCODE, ISNULL((MATERIAL_CODE+'-'+ PRODUCT_NAME),'') AS PRODUCTDESC, QTY_BOX AS BOX, " +
                                    " QTY_CRATE AS CRATES, QTY_LTR AS LTR,SSI.UOM, MRP as PRODUCT_MRP, RATE, PRICE_EQUALIZATION_VALUE as PRICE_EQUALVALUE, " +
                                    " ADD_TAX_PERC,ADD_TAX_AMOUNT, SURCHARGE_PERC, SURCHARGE_AMOUNT,   TAX_PERC, " +
                                    " TAX_AMOUNT , DISC_PERC AS TRDDISCPERC,DISC_AMOUNT AS TRDDISCVALUE,LINE_AMOUNT AS GROSSRATE, " +
                                    " INVOICE_VALUE as AMOUNT ,YEXPPER,YEXP,YCS3PER,YCS3 from [ax].[ACX_STAGINGSALESINVOICE] SSI " +
                                    " INNER JOIN AX.INVENTSITE INVST ON INVST.SITEID = SSI.CUSTOMER_CODE " +
                                    " LEFT JOIN AX.INVENTTABLE INVT ON SSI.MATERIAL_CODE = INVT.ITEMID WHERE cast(SSI.INVOICE_NO as Bigint) ='" + txtInvoiceNo.Text.Trim() + "' AND " +
                                    " CUSTOMER_CODE = '" + Session["SiteCode"].ToString() + "'";*/

                string queryLine = "Exec ACX_USP_GETSAPINVOICEDETAIL '" + Session["SiteCode"].ToString() + "','" + SavingInvoiceNo + "'";
                dtHeader = obj.GetData(queryHeader);
                dtLine = obj.GetData(queryLine);

                #endregion

                #region Fill Header and Line from SAP Data

                if (dtHeader.Rows.Count > 0 && dtLine.Rows.Count > 0)
                {
                    txtIndentDate.Text = string.Empty;

                    string TransporterName = dtHeader.Rows[0]["TRANSPORTORCODE"].ToString();
                    txtTransporterName.Text = TransporterName;
                    txtvehicleNo.Text = dtHeader.Rows[0]["VEHICLENO"].ToString();
                    DateTime InvcDate = Convert.ToDateTime(dtHeader.Rows[0]["INVOICE_DATE"].ToString());
                    txtInvoiceDate.Text = InvcDate.ToString("dd-MMM-yyyy");
                    txttransporterNo.Text = dtHeader.Rows[0]["DRIVERNAME"].ToString();
                    txtGSTNNumber.Text = dtHeader.Rows[0]["GSTINNO"].ToString();
                    if (dtHeader.Rows[0]["COMPOSITIONSCHEME"].ToString() == "0")
                    {
                        DrpCompScheme.SelectedIndex = 2;
                    }
                    else
                    {
                        DrpCompScheme.SelectedIndex = 1;
                    }
                    if (dtHeader.Rows[0]["PLANT_STATE_CODE"].ToString() == null || dtHeader.Rows[0]["PLANT_STATE_CODE"].ToString() == "")
                    {
                        txtState.Enabled = false;
                        txtState.Text = "";
                    }
                    else
                    {
                        txtState.Enabled = false;
                        txtState.Text = dtHeader.Rows[0]["PLANT_STATE_CODE"].ToString();
                    }
                    ordertype.Text = dtHeader.Rows[0]["ORDERTYPECODE"].ToString();
                    Plant.Text = dtHeader.Rows[0]["Plant_code"].ToString();
                    txtGSTRegistrationDate.Text = dtHeader.Rows[0]["GSTREGISTRATIONDATE"].ToString();
                    //txtPlantName.Text = dtHeader.Rows[0]["PLANT_NAME"].ToString();
                    txtAddr.Text = dtHeader.Rows[0]["Addr"].ToString();
                    txtPlantCity.Text = dtHeader.Rows[0]["PLANT_CITY"].ToString();
                    txtPostalCode.Text = dtHeader.Rows[0]["PLANT_POSTCODE"].ToString();
                    //txtOrderType.Text = dtHeader.Rows[0]["ORDERTYPECODE"].ToString();
                    txtVehicleType.Text = dtHeader.Rows[0]["VEHICLETYPE"].ToString();
                    decimal RecptValue = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());
                    txtReceiptValue.Text = (Math.Round(RecptValue, 2)).ToString();
                    if (Convert.ToString(dtHeader.Rows[0]["ODNNO"].ToString()).Length > 0)
                    {
                        if (drpGstInvoice.Items.Contains(drpGstInvoice.Items.FindByValue(dtHeader.Rows[0]["ODNNO"].ToString())))
                        { drpGstInvoice.SelectedValue = dtHeader.Rows[0]["ODNNO"].ToString(); }
                    }
                    //txtPurchDocumentNo.Text = PurchReceiptNumber;
                    dtLineItems = dtLine.Copy();
                    CalculateSAPLineItems(dtLineItems);

                    if (Convert.ToInt16(GridPurchItems.Rows.Count) > 0)
                    {
                        txtInvoiceNo.ReadOnly = true;
                    }
                    else
                    {
                        txtInvoiceNo.ReadOnly = false;
                    }
                    return true;

                }
                else
                {
                    txtInvoiceNo.ReadOnly = false;
                    LblMessage.Text = "No Data Found for Invoice Number : " + SavingInvoiceNo + "";
                    txtInvoiceNo.Focus();
                    return false;
                }

                #endregion

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        public void showMsg(string msg)
        {
            string message = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
        }

        private void CalculateSAPLineItems(DataTable dt)
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable _dtLine = dt.Clone();
            _dtLine.Columns.Add("BASICVALUE");
            _dtLine.Columns.Add("VAT_INC_PERC");
            _dtLine.Columns.Add("VAT_INC_PERCVALUE");
            _dtLine.Columns.Add("Remark");
            _dtLine.Columns.Add("TAXCOMPONENT");
            _dtLine.Columns.Add("ADDTAXCOMPONENT");
            DataRow row;

            DataTable dtForShownUnuploadData = new DataTable();
            dtForShownUnuploadData.Columns.Add("ProductCode");

            try
            {
                int j = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region to check SAP product Code exits or not in ProductTable

                    string sqlstr = "select ItemID from ax.inventTable where ItemID = '" + dt.Rows[i]["ProductCode"].ToString() + "'";
                    object objcheckproductcode = obj.GetScalarValue(sqlstr);
                    if (objcheckproductcode == null)
                    {
                        dtForShownUnuploadData.Rows.Add();
                        dtForShownUnuploadData.Rows[j]["ProductCode"] = dt.Rows[i]["ProductCode"].ToString();

                        j += 1;
                        continue;
                    }
                    #endregion

                    //string[] ReturnArray = null;
                    //ReturnArray = obj.CalculatePrice1(dt.Rows[i]["PRODUCTCODE"].ToString(), string.Empty, Convert.ToDecimal(dt.Rows[i]["BOX"].ToString()), "Box");
                    //if (ReturnArray != null)
                    {
                        #region SAP Data Caluclation

                        decimal BoxQty = Math.Round(Convert.ToDecimal(dt.Rows[i]["BOX"].ToString()), 2);
                        int Box = Convert.ToInt32(BoxQty);
                        decimal Crates = Convert.ToDecimal(dt.Rows[i]["NewCrates"].ToString());// Convert.ToDecimal(ReturnArray[0].ToString());
                        decimal Ltr = Convert.ToDecimal(dt.Rows[i]["NewLtr"].ToString());//Convert.ToDecimal(ReturnArray[1].ToString());
                        decimal MRP = Convert.ToDecimal(dt.Rows[i]["Product_MRP"].ToString());//Convert.ToDecimal(ReturnArray[2].ToString());
                        string UOM = dt.Rows[i]["UOM"].ToString();//ReturnArray[4].ToString();


                        //=================By Amrita === 2ndjun2016===============
                        //===============By Amrita on 02-06-2016===================

                        decimal Rate = Convert.ToDecimal(dt.Rows[i]["Rate"].ToString());// Convert.ToDecimal(dt.Rows[i]["Rate"].ToString());
                        decimal RateValue = (Math.Round(Rate * Box, 2));            //BASIC VALUE


                        decimal TRDDiscPerc = Convert.ToDecimal(dt.Rows[i]["TRDDISCPERC"].ToString());
                        decimal TRDDiscValue = Math.Round((RateValue * TRDDiscPerc) / 100, 2);

                        decimal PriceEqualValue = Convert.ToDecimal(dt.Rows[i]["PRICE_EQUALVALUE"].ToString());

                        decimal AddTAxPerc = Convert.ToDecimal(dt.Rows[i]["ADD_TAX_PERC"].ToString()) + Convert.ToDecimal(dt.Rows[i]["CGSTPer"].ToString());
                        decimal AddTaxAmount = Convert.ToDecimal(dt.Rows[i]["ADD_TAX_AMOUNT"].ToString()) + Convert.ToDecimal(dt.Rows[i]["CGSTValue"].ToString());

                        decimal TaxPerc = Convert.ToDecimal(dt.Rows[i]["TAX_PERC"].ToString()) + Convert.ToDecimal(dt.Rows[i]["SGSTPer"].ToString()) + Convert.ToDecimal(dt.Rows[i]["UGSTPer"].ToString()) + Convert.ToDecimal(dt.Rows[i]["IGSTPer"].ToString());
                        decimal TaxAmount = Convert.ToDecimal(dt.Rows[i]["TAX_AMOUNT"].ToString()) + Convert.ToDecimal(dt.Rows[i]["SGSTValue"].ToString()) + Convert.ToDecimal(dt.Rows[i]["UGSTValue"].ToString()) + Convert.ToDecimal(dt.Rows[i]["IGSTValue"].ToString());

                        decimal SurchargePerc = Convert.ToDecimal(dt.Rows[i]["SURCHARGE_PERC"].ToString());
                        decimal SurchargeAmount = Convert.ToDecimal(dt.Rows[i]["SURCHARGE_AMOUNT"].ToString());

                        decimal GSTTaxPer = Convert.ToDecimal(dt.Rows[i]["IGSTPer"].ToString()) //+ Convert.ToDecimal(dt.Rows[i]["CGSTPer"].ToString())
                            + Convert.ToDecimal(dt.Rows[i]["SGSTPer"].ToString()) + Convert.ToDecimal(dt.Rows[i]["UGSTPer"].ToString());

                        decimal GSTTaxAmt = Convert.ToDecimal(dt.Rows[i]["IGSTValue"].ToString()) //+ Convert.ToDecimal(dt.Rows[i]["CGSTValue"].ToString())
                            + Convert.ToDecimal(dt.Rows[i]["SGSTValue"].ToString()) + Convert.ToDecimal(dt.Rows[i]["UGSTValue"].ToString());

                        //decimal VAT_inc_perc = Math.Round((TaxPerc / 100 + (TaxPerc / 100 * SurchargePerc / 100) + AddTAxPerc / 100) * 100, 2);
                        //decimal VAT_inc_value = Math.Round(TaxAmount + AddTaxAmount + SurchargeAmount, 2);

                        decimal VAT_inc_perc = Math.Round((TaxPerc / 100 + (TaxPerc / 100 * SurchargePerc / 100) + AddTAxPerc / 100) * 100, 2);
                        //VAT_inc_perc += GSTTaxPer;
                        decimal VAT_inc_value = Math.Round(TaxAmount + AddTaxAmount + SurchargeAmount, 2);
                        //VAT_inc_value += GSTTaxAmt;

                        //if (Convert.ToString(Session["SITEGSTINNO"]).Length == 0 || Convert.ToBoolean(Session["SITECOMPOSITIONSCHEME"]) == true)
                        //{
                        //    if (GSTTaxPer>0)
                        //    {

                        //    }
                        //}
                        decimal GrossRate = Math.Round((RateValue - TRDDiscValue + PriceEqualValue + VAT_inc_value) / Box, 2);
                        decimal NetValue = Convert.ToDecimal(dt.Rows[i]["GROSSRATE"].ToString());
                        decimal YEXPPER = Convert.ToDecimal(dt.Rows[i]["YEXPPER"].ToString());
                        decimal YEXP = Convert.ToDecimal(dt.Rows[i]["YEXP"].ToString());
                        decimal YCS3PER = Convert.ToDecimal(dt.Rows[i]["YCS3PER"].ToString());
                        decimal YCS3 = Convert.ToDecimal(dt.Rows[i]["YCS3"].ToString());

                        #endregion

                        row = _dtLine.NewRow();


                        #region Add New Row in Datatable

                        row["LINE_NO"] = dt.Rows[i]["LINE_NO"].ToString();
                        row["PRODUCTCODE"] = dt.Rows[i]["PRODUCTCODE"].ToString();
                        row["PRODUCTDESC"] = dt.Rows[i]["PRODUCTDESC"].ToString();
                        row["COMPOSITIONSCHEME"] = dt.Rows[i]["COMPOSITIONSCHEME"].ToString();
                        row["PRODUCT_MRP"] = MRP.ToString();
                        row["BOX"] = Box.ToString();
                        row["CRATES"] = Crates.ToString();
                        row["UOM"] = UOM;
                        row["LTR"] = Ltr.ToString();
                        row["RATE"] = Rate.ToString();
                        row["BASICVALUE"] = RateValue.ToString();
                        row["PRICE_EQUALVALUE"] = PriceEqualValue.ToString();
                        row["TRDDISCPERC"] = TRDDiscPerc.ToString();
                        row["TRDDISCVALUE"] = TRDDiscValue.ToString();
                        row["TAX_PERC"] = TaxPerc.ToString();
                        row["TAX_AMOUNT"] = TaxAmount.ToString();
                        row["ADD_TAX_PERC"] = AddTAxPerc.ToString();
                        row["ADD_TAX_AMOUNT"] = AddTaxAmount.ToString();
                        row["SURCHARGE_PERC"] = SurchargePerc.ToString();
                        row["SURCHARGE_AMOUNT"] = SurchargeAmount.ToString();
                        row["VAT_INC_PERC"] = VAT_inc_perc.ToString("0.00");
                        row["VAT_INC_PERCVALUE"] = VAT_inc_value.ToString("0.00");
                        row["GROSSRATE"] = GrossRate.ToString();
                        row["AMOUNT"] = NetValue.ToString();
                        row["YEXPPER"] = YEXPPER.ToString();
                        row["YEXP"] = YEXP.ToString();
                        row["YCS3PER"] = YCS3PER.ToString();
                        row["YCS3"] = YCS3.ToString();
                        row["IGSTPer"] = dt.Rows[i]["IGSTPer"].ToString();
                        row["IGSTValue"] = dt.Rows[i]["IGSTValue"].ToString();
                        row["CGSTPer"] = dt.Rows[i]["CGSTPer"].ToString();
                        row["CGSTValue"] = dt.Rows[i]["CGSTValue"].ToString();
                        row["SGSTPer"] = dt.Rows[i]["SGSTPer"].ToString();
                        row["SGSTValue"] = dt.Rows[i]["SGSTValue"].ToString();
                        row["UGSTPer"] = dt.Rows[i]["UGSTPer"].ToString();
                        row["UGSTValue"] = dt.Rows[i]["UGSTValue"].ToString();
                        row["CessPer"] = dt.Rows[i]["CessPer"].ToString();
                        row["CessValue"] = dt.Rows[i]["CessValue"].ToString();
                        row["SPECIALDISCV"] = dt.Rows[i]["SPECIALDISCV"].ToString();
                        row["SPECIALDISCP"] = dt.Rows[i]["SPECIALDISCP"].ToString();
                        row["HSNCode"] = dt.Rows[i]["HSNCode"].ToString();
                        row["Plant_Code"] = dt.Rows[i]["Plant_Code"].ToString();
                        row["PLANT_STATE"] = dt.Rows[i]["PLANT_STATE"].ToString();

                        #endregion

                        //row["LINE_NO"] = dt.Rows[i]["LINE_NO"].ToString();
                        //row["PRODUCTCODE"] = dt.Rows[i]["PRODUCTCODE"].ToString();
                        //row["PRODUCTDESC"] = dt.Rows[i]["PRODUCTDESC"].ToString();
                        //row["PRODUCT_MRP"] = MRP.ToString();
                        //row["BOX"] = Box.ToString();
                        //row["CRATES"] = Crates.ToString();
                        //row["UOM"] = UOM;
                        //row["LTR"] = Ltr.ToString();
                        //row["RATE"] = Rate.ToString();
                        //row["BASICVALUE"] = RateValue.ToString();
                        //row["TRDDISCPERC"] = TRDDiscPerc.ToString();
                        //row["TRDDISCVALUE"] = TRDDiscValue.ToString();
                        //row["PRICE_EQUALVALUE"] = PriceEqualValue.ToString();
                        //row["VAT_INC_PERC"] = VAT_inc_perc.ToString();
                        //row["VAT_INC_PERCVALUE"] = VAT_inc_value.ToString();
                        //row["GROSSRATE"] = GrossRate.ToString();
                        //row["AMOUNT"] = NetValue.ToString();

                        _dtLine.Rows.Add(row);


                    }
                }

                decimal totalvalue = _dtLine.AsEnumerable().Sum(rows => rows.Field<decimal>("AMOUNT"));

                txtReceiptValue.Text = Math.Round(totalvalue, 2).ToString();

                #region Modal Popup Show for Un-uploaded Products

                if (dtForShownUnuploadData.Rows.Count > 0)
                {
                    gridviewRecordNotExist.DataSource = dtForShownUnuploadData;
                    gridviewRecordNotExist.DataBind();
                    ModalPopupExtender1.Show();
                    _dtLine = null;
                    GridPurchItems.DataSource = _dtLine;
                    GridPurchItems.DataBind();
                    LblMessage.Text = "Please Check Product Master.";
                }
                else
                {
                    #region Bind Grid After Calculation

                    if (_dtLine.Rows.Count > 0)
                    {
                        GridPurchItems.AutoGenerateDeleteButton = false;
                        GridPurchItems.DataSource = _dtLine;
                        GridPurchItems.DataBind();
                        GridPurchItems.Visible = true;
                        ViewState["BAPI_STAGINGDATA"] = _dtLine;                //to store all values of Staging Table line items and save in purch line table
                    }
                    LblMessage.Text = "Records Fetched From SAP Successfully. Total Records : " + dt.Rows.Count + ". Added : " + (dt.Rows.Count - j) + " Records.";
                    #endregion
                    gridviewRecordNotExist.DataSource = null;
                    gridviewRecordNotExist.DataBind();
                }

                #endregion

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private string PurchaseTransTax(string PostDocumentNo, int LineNO, string HsnCode, string ProductCode, string TaxCode, decimal TaxPer, decimal TaxAmount)
        {
            try
            {
                SqlCommand cmd3 = new SqlCommand();
                cmd3.Connection = conn;
                cmd3.Transaction = transaction;
                cmd3.CommandTimeout = 3600;
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "USP_PURCHASETRANSTAX";
                cmd3.Parameters.Clear();
                cmd3.Parameters.AddWithValue("@DOCTYPE", "2");
                cmd3.Parameters.AddWithValue("@DOCUMENT_NO", PostDocumentNo);
                cmd3.Parameters.AddWithValue("@LINE_NO", LineNO);
                cmd3.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd3.Parameters.AddWithValue("@SITE_CODE", Session["SiteCode"].ToString());
                cmd3.Parameters.AddWithValue("@HSNCODE", HsnCode);
                cmd3.Parameters.AddWithValue("@ITEMCODE", ProductCode);
                cmd3.Parameters.AddWithValue("@TAXCODE", TaxCode);
                cmd3.Parameters.AddWithValue("@TAXPER", TaxPer);
                cmd3.Parameters.AddWithValue("@TAXAMOUNT", TaxAmount);
                if (TaxPer != 0)
                {
                    cmd3.Parameters.AddWithValue("@TAXABLEAMOUNT", (TaxAmount / TaxPer * 100));
                }
                else
                {
                    cmd3.Parameters.AddWithValue("@TAXABLEAMOUNT", "0");
                }
                cmd3.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return ex.Message.ToString();
            }
        }
        private void POSTSAPInvoiceData(DataTable dtBAPI)
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();
            string PostDocumentNo = string.Empty;
            string SavingInvoiceNo = "";
            if (rdoSAPFetchData.Checked)
            {
                SavingInvoiceNo = DrpSalesInvoice.SelectedValue;
            }
            else
            {
                SavingInvoiceNo = txtInvoiceNo.Text;
            }
            try
            {
                #region Generate New Posted Invoice Number

                cmd = new SqlCommand();
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                DataTable dtNumSeq = obj.GetNumSequenceNew(2, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());
                string NUMSEQ = string.Empty;
                if (dtNumSeq != null)
                {
                    PostDocumentNo = dtNumSeq.Rows[0][0].ToString();
                    NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                }
                else
                {
                    return;
                }
                #endregion

                string queryRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTHEADER]";
                Int64 Recid = Convert.ToInt64(obj.GetScalarValue(queryRecID));

                cmd.CommandText = string.Empty;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = " INSERT INTO [ax].[ACXPURCHINVRECIEPTHEADER] ( Document_No, DATAAREAID, RECID, Document_Date, Purch_IndentNo, Purch_IndentDate, " +
                                  " SO_No,SO_Date,STATUS,Material_Value,Purch_RecieptNo,Sale_InvoiceDate,Sale_InvoiceNo,Site_Code,Transporter_Code,VEHICAL_No," +
                                  " VEHICAL_Type,PREDOCUMENT_NO,NUMSEQ,DRIVERNAME,Plant_Name,Plant_Address,Plant_City,Plant_PostCode,OrderTypeCode,DISTGSTINNO," +
                                  " DISTGSTINREGDATE,DISTCOMPOSITIONSCHEME,VENDGSTINNO,VENDGSTINREGDATE,VENDCOMPOSITIONSCHEME,GSTINVOICENO,Plant_Code,Supplier_code,PLANT_STATE)" +
                                  " values ( @Document_No,@DATAAREAID,@RECID,@Document_Date,@Purch_IndentNo,@Purch_IndentDate, @SO_No,@SO_Date,@STATUS,@Material_Value," +
                                  " @Purch_RecieptNo,@Sale_InvoiceDate,@Sale_InvoiceNo,@Site_Code,@Transporter_Code,@VEHICAL_No, @VEHICAL_Type,@PREDOCUMENT_NO," +
                                  " @NUMSEQ,@DRIVERNAME,@Plant_Name,@Plant_Address,@Plant_City,@Plant_PostCode,@OrderTypeCode,@DISTGSTINNO,@DISTGSTINREGDATE," +
                                  " @DISTCOMPOSITIONSCHEME,@VENDGSTINNO,@VENDGSTINREGDATE,@VENDCOMPOSITIONSCHEME,@GSTINVOICENO,@Plant_Code,@Supplier_Code,@PLANT_STATE)";

                string queryInsert2 = "insert into [ax].[ACXVENDORTRANS](Document_No,SiteCode,Dealer_Code,DATAAREAID,RECID,Amount,DocumentType,RemainingAmount,Document_Date," +
                                       "Transaction_Date,RefNo_DocumentNo) values(@Document_No,@Site_Code,@Plant_Name,@DATAAREAID,@RECID,@Material_Value,1,@Material_Value,@Sale_InvoiceDate,@Sale_InvoiceDate,@RefNo_DocumentNo)";
                cmd2 = new SqlCommand(queryInsert2);
                cmd2.Connection = conn;
                if (transaction == null)
                {
                    transaction = conn.BeginTransaction();
                }
                cmd2.Transaction = transaction;
                cmd2.CommandTimeout = 3600;
                cmd2.CommandType = CommandType.Text;


                #region Insert Header
                string GstRegDate;
                GstRegDate = Convert.ToString(Session["SITEGSTINREGDATE"]) == "" ? null : Convert.ToDateTime(Session["SITEGSTINREGDATE"]).ToString("yyyy-MM-dd");

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_No", PostDocumentNo);
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@RECID", Recid + 1);
                cmd.Parameters.AddWithValue("@Document_Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Purch_IndentNo", string.Empty);
                cmd.Parameters.AddWithValue("@Purch_IndentDate", txtIndentDate.Text);
                cmd.Parameters.AddWithValue("@SO_No", string.Empty);
                cmd.Parameters.AddWithValue("@SO_Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@STATUS", 1);
                cmd.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(txtReceiptValue.Text));
                cmd.Parameters.AddWithValue("@Purch_RecieptNo", PostDocumentNo);
                cmd.Parameters.AddWithValue("@Sale_InvoiceDate", Convert.ToDateTime(txtInvoiceDate.Text.Trim()));
                cmd.Parameters.AddWithValue("@Sale_InvoiceNo", SavingInvoiceNo);
                cmd.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@Transporter_Code", txtTransporterName.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@VEHICAL_No", txtvehicleNo.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@VEHICAL_Type", txtVehicleType.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@PREDOCUMENT_NO", string.Empty);
                cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                cmd.Parameters.AddWithValue("@DRIVERNAME", txttransporterNo.Text.Trim().ToString());
                //cmd.Parameters.AddWithValue("@Plant_Name", txtPlantName.Text.Trim().ToString());
                //cmd.Parameters.AddWithValue("@Plant_Name", DrpPlant.SelectedItem.Value.ToString());
                cmd.Parameters.AddWithValue("@Plant_Name", Plant.Text.ToString());
                cmd.Parameters.AddWithValue("@Plant_Address", txtAddr.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@Plant_City", txtPlantCity.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@Plant_PostCode", txtPostalCode.Text.Trim().ToString());
                //cmd.Parameters.AddWithValue("@OrderTypeCode", drpOrderType.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@OrderTypeCode", ordertype.Text.ToString());
                cmd.Parameters.AddWithValue("@DISTGSTINNO", Convert.ToString(Session["SITEGSTINNO"]));
                //cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", GstRegDate);
                cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", txtGSTRegistrationDate.Text.ToString());
                cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", (Convert.ToBoolean(Session["SITECOMPOSITIONSCHEME"]) == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@VENDGSTINNO", Convert.ToString(dtBAPI.Rows[0]["GSTINNo"]));
                cmd.Parameters.AddWithValue("@VENDGSTINREGDATE", Convert.ToString(dtBAPI.Rows[0]["GSTREGISTRATIONDATE"]));
                cmd.Parameters.AddWithValue("@VENDCOMPOSITIONSCHEME", (Convert.ToBoolean(dtBAPI.Rows[0]["COMPOSITIONSCHEME"]) == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@GSTINVOICENO", drpGstInvoice.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Plant_Code", Convert.ToString(dtBAPI.Rows[0]["PLANT_CODE"]));
                //cmd.Parameters.AddWithValue("@PLANT_STATE", Convert.ToString(dtBAPI.Rows[0]["PLANT_STATE"]));
                cmd.Parameters.AddWithValue("@PLANT_STATE", txtState.Text);

                cmd.Parameters.AddWithValue("@Supplier_Code", "7200");
                cmd.ExecuteNonQuery();

                //------------Trans----Cdoe By Savita
                cmd2.Parameters.AddWithValue("@Document_No", PostDocumentNo);
                cmd2.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                cmd2.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd2.Parameters.AddWithValue("@RECID", Recid + 1);
                cmd2.Parameters.AddWithValue("@Document_Date", DateTime.Now);


                //if (string.IsNullOrEmpty(txtPlantName.Text.Trim().ToString()))
                //{
                cmd2.Parameters.AddWithValue("@Plant_Name", "7200");
                //}
                //else
                //{
                //    cmd2.Parameters.AddWithValue("@Plant_Name", txtPlantName.Text.Trim().ToString());
                //}
                cmd2.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(txtReceiptValue.Text.Trim()));
                cmd2.Parameters.AddWithValue("@Sale_InvoiceDate", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd").Trim()));
                cmd2.Parameters.AddWithValue("@RefNo_DocumentNo", drpGstInvoice.Text.ToString());

                cmd2.ExecuteNonQuery();

                #endregion

                #region Line Items Query

                SqlCommand cmd3 = new SqlCommand();
                cmd3.Connection = conn;
                cmd3.Transaction = transaction;
                cmd3.CommandTimeout = 3600;
                cmd3.CommandType = CommandType.Text;



                cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.Text;

                string queryLineRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTLINE]";
                Int64 recid = Convert.ToInt64(obj.GetScalarValue(queryLineRecID));

                cmd1.CommandText = string.Empty;
                cmd1.CommandText = " INSERT INTO [ax].[ACXPURCHINVRECIEPTLINE] ( Purch_RecieptNo, DATAAREAID, RECID, Amount, Box,Crates, Discount, Line_No, " +
                                    " Ltr, Product_Code, Rate, Siteid, TAX, TAXAMOUNT, UOM, BASICVALUE,TRDDISCPERC,TRDDISCVALUE,PRICE_EQUALVALUE, VAT_INC_PERC, " +
                                    " VAT_INC_PERCVALUE, GROSSRATE, Remark, ADD_TAX_PERC,ADD_TAX_AMOUNT,SURCHARGE_PERC,SURCHARGE_AMOUNT,YEXPPER,YEXP,YCS3PER,YCS3," +
                                    " HSNCODE,COMPOSITIONSCHEME,TAXCOMPONENT,ADDTAXCOMPONENT,SCH_DISC_PERC,SCH_DISC_VAL,EXPTAXVALUE) Values " +
                                    " ( @Purch_RecieptNo,@DATAAREAID,@RECID,@Amount,@Box,@Crates,@Discount,@Line_No," +
                                    " @Ltr,@Product_Code,@Rate, @Siteid,@TAX,@TAXAMOUNT,@UOM,@BASICVALUE,@TRDDISCPERC,@TRDDISCVALUE,@PRICE_EQUALVALUE, " +
                                    " @VAT_INC_PERC,@VAT_INC_PERCVALUE,@GROSSRATE,@Remark, @ADD_TAX_PERC, @ADD_TAX_AMOUNT, @SURCHARGE_PERC, @SURCHARGE_AMOUNT," +
                                    " @YEXPPER,@YEXP,@YCS3PER,@YCS3,@HSNCODE,@COMPOSITIONSCHEME,@TAXCOMPONENT,@ADDTAXCOMPONENT,@SPECIALDISCP,@SPECIALDISCV,@EXPTAXVALUE)";

                cmd3.CommandText = string.Empty;
                cmd3.CommandText = "INSERT INTO [ax].[PURCHASETRANSTAX] ([DOCTYPE],[DOCUMENT_NO],[LINE_NO],[DATAAREAID],[SITE_CODE],[HSNCODE],[ITEMCODE],[TAXCODE]" +
                            ",[TAXPER],[TAXABLEAMOUNT],[TAXAMOUNT]) VALUES (@DOCTYPE,@DOCUMENT_NO,@LINE_NO,@DATAAREAID,@SITE_CODE,@HSNCODE,@ITEMCODE,@TAXCODE," +
                            "@TAXPER,@TAXABLEAMOUNT,@TAXAMOUNT)";
                #endregion

                string TAXCOMPONENT, ADDTAXCOMPONENT, ResultMsg;


                #region Loop through Grid and get data to Insert into PurchInv Line Table

                for (int p = 0; p < dtBAPI.Rows.Count; p++)
                {
                    TAXCOMPONENT = ADDTAXCOMPONENT = "";
                    string remark = "SAP BAPI";
                    if (Convert.ToDecimal(dtBAPI.Rows[p]["SGSTPer"].ToString()) > 0)
                    {
                        ResultMsg = PurchaseTransTax(PostDocumentNo, p + 1, dtBAPI.Rows[p]["HSNCode"].ToString(), dtBAPI.Rows[p]["PRODUCTCODE"].ToString(), "SGST", Convert.ToDecimal(dtBAPI.Rows[p]["SGSTPer"]), Convert.ToDecimal(dtBAPI.Rows[p]["SGSTValue"]));
                        if (ResultMsg != "Success")
                        {
                            transaction.Rollback();
                            LblMessage.Text = ResultMsg;
                            return;
                        }
                        TAXCOMPONENT = "SGST";
                    }
                    if (Convert.ToDecimal(dtBAPI.Rows[p]["UGSTPer"].ToString()) > 0)
                    {
                        ResultMsg = PurchaseTransTax(PostDocumentNo, p + 1, dtBAPI.Rows[p]["HSNCode"].ToString(), dtBAPI.Rows[p]["PRODUCTCODE"].ToString(), "UGST", Convert.ToDecimal(dtBAPI.Rows[p]["UGSTPer"]), Convert.ToDecimal(dtBAPI.Rows[p]["UGSTValue"]));
                        if (ResultMsg != "Success")
                        {
                            transaction.Rollback();
                            LblMessage.Text = ResultMsg;
                            return;
                        }
                        TAXCOMPONENT = "UGST";
                    }
                    if (Convert.ToDecimal(dtBAPI.Rows[p]["IGSTPer"].ToString()) > 0)
                    {
                        ResultMsg = PurchaseTransTax(PostDocumentNo, p + 1, dtBAPI.Rows[p]["HSNCode"].ToString(), dtBAPI.Rows[p]["PRODUCTCODE"].ToString(), "IGST", Convert.ToDecimal(dtBAPI.Rows[p]["IGSTPer"]), Convert.ToDecimal(dtBAPI.Rows[p]["IGSTValue"]));
                        if (ResultMsg != "Success")
                        {
                            transaction.Rollback();
                            LblMessage.Text = ResultMsg;
                            return;
                        }
                        TAXCOMPONENT = "IGST";
                    }
                    if (Convert.ToDecimal(dtBAPI.Rows[p]["CGSTPer"].ToString()) > 0)
                    {
                        ResultMsg = PurchaseTransTax(PostDocumentNo, p + 1, dtBAPI.Rows[p]["HSNCode"].ToString(), dtBAPI.Rows[p]["PRODUCTCODE"].ToString(), "CGST", Convert.ToDecimal(dtBAPI.Rows[p]["CGSTPer"]), Convert.ToDecimal(dtBAPI.Rows[p]["CGSTValue"]));
                        if (ResultMsg != "Success")
                        {
                            transaction.Rollback();
                            LblMessage.Text = ResultMsg;
                            return;
                        }
                        ADDTAXCOMPONENT = "CGST";
                    }

                    if (TAXCOMPONENT == "")
                    {
                        ResultMsg = PurchaseTransTax(PostDocumentNo, p + 1, dtBAPI.Rows[p]["HSNCode"].ToString(), dtBAPI.Rows[p]["PRODUCTCODE"].ToString(), "VAT", Convert.ToDecimal(dtBAPI.Rows[p]["Tax_Perc"]), Convert.ToDecimal(dtBAPI.Rows[p]["Tax_Amount"]));
                        if (ResultMsg != "Success")
                        {
                            transaction.Rollback();
                            LblMessage.Text = ResultMsg;
                            return;
                        }
                        TAXCOMPONENT = "VAT";
                        if (Convert.ToDecimal(dtBAPI.Rows[p]["Add_Tax_Perc"]) > 0)
                        {
                            ResultMsg = PurchaseTransTax(PostDocumentNo, p + 1, dtBAPI.Rows[p]["HSNCode"].ToString(), dtBAPI.Rows[p]["PRODUCTCODE"].ToString(), "VAT", Convert.ToDecimal(dtBAPI.Rows[p]["Tax_Perc"]), Convert.ToDecimal(dtBAPI.Rows[p]["Tax_Amount"]));
                            if (ResultMsg != "Success")
                            {
                                transaction.Rollback();
                                LblMessage.Text = ResultMsg;
                                return;
                            }
                            ResultMsg = PurchaseTransTax(PostDocumentNo, p + 1, dtBAPI.Rows[p]["HSNCode"].ToString(), dtBAPI.Rows[p]["PRODUCTCODE"].ToString(), "ADD VAT", Convert.ToDecimal(dtBAPI.Rows[p]["Add_Tax_Perc"]), Convert.ToDecimal(dtBAPI.Rows[p]["Add_Tax_Amount"]));
                            if (ResultMsg != "Success")
                            {
                                transaction.Rollback();
                                LblMessage.Text = ResultMsg;
                                return;
                            }
                            ADDTAXCOMPONENT = "ADD VAT";
                        }
                    }

                    if (GridPurchItems.Rows.Count > 0)
                    {
                        remark = ((TextBox)GridPurchItems.Rows[p].FindControl("txtRemark")).Text;
                    }

                    cmd1.Parameters.Clear();

                    cmd1.Parameters.AddWithValue("@TAXCOMPONENT", TAXCOMPONENT);
                    cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", ADDTAXCOMPONENT);
                    cmd1.Parameters.AddWithValue("@Purch_RecieptNo", PostDocumentNo);
                    cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                    cmd1.Parameters.AddWithValue("@RECID", p + recid + 1);
                    cmd1.Parameters.AddWithValue("@Siteid", Session["SiteCode"].ToString());

                    cmd1.Parameters.AddWithValue("@Box", Convert.ToDecimal(dtBAPI.Rows[p]["BOX"].ToString()));
                    cmd1.Parameters.AddWithValue("@Crates", Convert.ToDecimal(dtBAPI.Rows[p]["CRATES"].ToString()));
                    cmd1.Parameters.AddWithValue("@Discount", 0);
                    cmd1.Parameters.AddWithValue("@Line_No", p + 1);
                    cmd1.Parameters.AddWithValue("@Ltr", Convert.ToDecimal(dtBAPI.Rows[p]["LTR"].ToString()));
                    cmd1.Parameters.AddWithValue("@Product_Code", dtBAPI.Rows[p]["PRODUCTCODE"].ToString());
                    cmd1.Parameters.AddWithValue("@UOM", dtBAPI.Rows[p]["UOM"].ToString());

                    cmd1.Parameters.AddWithValue("@Rate", Convert.ToDecimal(dtBAPI.Rows[p]["RATE"].ToString()));
                    cmd1.Parameters.AddWithValue("@BASICVALUE", Convert.ToDecimal(dtBAPI.Rows[p]["BASICVALUE"].ToString()));

                    //cmd1.Parameters.AddWithValue("@TAX", Convert.ToDecimal(dtBAPI.Rows[i]["TAX_PERC"].ToString()));
                    //cmd1.Parameters.AddWithValue("@TAXAMOUNT", Convert.ToDecimal(dtBAPI.Rows[i]["TAX_AMOUNT"].ToString()));

                    cmd1.Parameters.AddWithValue("@TAX", Convert.ToDecimal(dtBAPI.Rows[p]["TAX_PERC"].ToString()));
                    cmd1.Parameters.AddWithValue("@TAXAMOUNT", Convert.ToDecimal(dtBAPI.Rows[p]["TAX_AMOUNT"].ToString()));

                    cmd1.Parameters.AddWithValue("@ADD_TAX_PERC", Convert.ToDecimal(dtBAPI.Rows[p]["ADD_TAX_PERC"].ToString()));
                    cmd1.Parameters.AddWithValue("@ADD_TAX_AMOUNT", Convert.ToDecimal(dtBAPI.Rows[p]["ADD_TAX_AMOUNT"].ToString()));

                    cmd1.Parameters.AddWithValue("@SURCHARGE_PERC", Convert.ToDecimal(dtBAPI.Rows[p]["SURCHARGE_PERC"].ToString()));
                    cmd1.Parameters.AddWithValue("@SURCHARGE_AMOUNT", Convert.ToDecimal(dtBAPI.Rows[p]["SURCHARGE_AMOUNT"].ToString()));

                    cmd1.Parameters.AddWithValue("@TRDDISCPERC", Convert.ToDecimal(dtBAPI.Rows[p]["TRDDISCPERC"].ToString()));
                    cmd1.Parameters.AddWithValue("@TRDDISCVALUE", Convert.ToDecimal(dtBAPI.Rows[p]["TRDDISCVALUE"].ToString()));

                    cmd1.Parameters.AddWithValue("@PRICE_EQUALVALUE", Convert.ToDecimal(dtBAPI.Rows[p]["PRICE_EQUALVALUE"].ToString()));

                    cmd1.Parameters.AddWithValue("@VAT_INC_PERC", Convert.ToDecimal(dtBAPI.Rows[p]["VAT_INC_PERC"].ToString()));
                    cmd1.Parameters.AddWithValue("@VAT_INC_PERCVALUE", Convert.ToDecimal(dtBAPI.Rows[p]["VAT_INC_PERCVALUE"].ToString()));

                    cmd1.Parameters.AddWithValue("@GROSSRATE", Convert.ToDecimal(dtBAPI.Rows[p]["GROSSRATE"].ToString()));
                    cmd1.Parameters.AddWithValue("@Amount", Convert.ToDecimal(dtBAPI.Rows[p]["AMOUNT"].ToString()));
                    cmd1.Parameters.AddWithValue("@Remark", remark);

                    cmd1.Parameters.AddWithValue("@YEXPPER", Convert.ToDecimal(dtBAPI.Rows[p]["YEXPPER"].ToString()));
                    cmd1.Parameters.AddWithValue("@YEXP", Convert.ToDecimal(dtBAPI.Rows[p]["YEXP"].ToString()));
                    cmd1.Parameters.AddWithValue("@YCS3PER", Convert.ToDecimal(dtBAPI.Rows[p]["YCS3PER"].ToString()));
                    cmd1.Parameters.AddWithValue("@YCS3", Convert.ToDecimal(dtBAPI.Rows[p]["YCS3"].ToString()));

                    cmd1.Parameters.AddWithValue("@HSNCODE", Convert.ToString(dtBAPI.Rows[p]["HSNCode"].ToString()));
                    cmd1.Parameters.AddWithValue("@SPECIALDISCP", Convert.ToString(dtBAPI.Rows[p]["SPECIALDISCP"].ToString()));
                    cmd1.Parameters.AddWithValue("@SPECIALDISCV", Convert.ToString(dtBAPI.Rows[p]["SPECIALDISCV"].ToString()));

                    cmd1.Parameters.AddWithValue("@COMPOSITIONSCHEME", (Convert.ToBoolean(dtBAPI.Rows[0]["COMPOSITIONSCHEME"]) == true ? 1 : 0));
                    Decimal ExpTaxValue = Convert.ToDecimal(dtBAPI.Rows[p]["AMOUNT"].ToString()) - Convert.ToDecimal(dtBAPI.Rows[p]["TAX_AMOUNT"].ToString()) - Convert.ToDecimal(dtBAPI.Rows[p]["ADD_TAX_AMOUNT"].ToString());
                    cmd1.Parameters.AddWithValue("@EXPTAXVALUE", ExpTaxValue);
                    cmd1.ExecuteNonQuery();
                }


                #endregion

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('SAP Invoice Posted Successfully !  Document Number : " + PostDocumentNo + " ');", true);

                // string message = "alert('SAP Invoice Posted Successfully !  Document Number : " + PostDocumentNo + " ');";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                UpdateTransTable(PostDocumentNo, transaction, conn);
                transaction.Commit();
                txtPurchDocumentNo.Text = string.Empty;
                RefreshCompletePage();
            }

            catch (Exception ex)
            {
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateSAPInvoicePosting()
        {
            bool b = true;
            if (rdoSAPFetchData.Checked)
            {
                if (DrpSalesInvoice.SelectedValue == string.Empty)
                {
                    LblMessage.Text = "► Please Provide Invoice No.";
                    DrpSalesInvoice.Focus();
                    b = false;
                    return b;
                }
            }
            else
            {
                if (txtInvoiceNo.Text == "")
                {
                    LblMessage.Text = "► Please Provide Invoice No.";
                    txtInvoiceNo.Focus();
                    b = false;
                    return b;
                }
            }

            if (txtInvoiceDate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice Date.";
                txtInvoiceDate.Focus();
                b = false;
                return b;
            }
            if (txtReceiptValue.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Receipt Value.";
                txtReceiptValue.Focus();
                b = false;
                return b;
            }
            if (txtState.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide State.";
                txtState.Focus();
                b = false;
                return b;
            }
            if (ordertype.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Order Type.";
                ordertype.Focus();
                b = false;
                return b;
            }
            if (Plant.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Plant Name.";
                Plant.Focus();
                b = false;
                return b;
            }
            if (txtAddr.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Plant Address.";
                txtAddr.Focus();
                b = false;
                return b;
            }
            //if (txtIndentDate.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide Indent Date.";
            //    txtAddr.Focus();
            //    b = false;
            //    return b;
            //}
            if (GridPurchItems.Rows.Count <= 0)
            {
                LblMessage.Text = "► No Items to Post. Cannot Post data.";
                txtInvoiceNo.Focus();
                b = false;
                return b;
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }

            return b;
        }

        protected void drpGstInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session[SessionPendingInv];
            if (dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select("ODNNO='" + drpGstInvoice.SelectedValue + "'");
                if (dr.Length > 0)
                {
                    DrpSalesInvoice.SelectedValue = dr[0][0].ToString();
                    //DrpSalesInvoice_SelectedIndexChanged(sender, e);
                }
                else
                    DrpSalesInvoice.SelectedIndex = 0;
            }
            else
                DrpSalesInvoice.SelectedIndex = 0;
            //DrpSalesInvoice.SelectedIndex = drpGstInvoice.SelectedIndex;
            DrpSalesInvoice_SelectedIndexChanged(sender, e);
        }

        protected void DrpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            IGST.Visible = CGST.Visible = UGST.Visible = SGST.Visible = false;
            txtUGSTPerc.Text = txtCGSTPerc.Text = txtIGSTPerc.Text = txtSGSTPerc.Text = "0";
            txtUGSTValue.Text = txtCGSTValue.Text = txtIGSTValue.Text = txtSGSTValue.Text = "0";

            if (DrpState.Text != "-Select-")
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                string state = Session["SITELOCATION"].ToString();
                if (DrpState.SelectedValue.ToString() == state.ToString())
                {
                    string query = "select unionterritory from AX.LOGISTICSADDRESSSTATE where stateid = '" + DrpState.SelectedValue.ToString() + "'";
                    DataTable dt1 = obj.GetData(query);
                    if (dt1.Rows.Count > 0)
                    {
                        int result = Convert.ToInt32(dt1.Rows[0]["unionterritory"].ToString());
                        if (result == 1)
                        {
                            UGST.Visible = true;
                            CGST.Visible = true;
                        }
                        else
                        {
                            SGST.Visible = true;
                            CGST.Visible = true;
                        }
                    }
                    else
                    {
                        SGST.Visible = true;
                        CGST.Visible = true;
                    }
                }
                else
                {
                    IGST.Visible = true;
                }
            }
        }

        protected void txtPriceEqualValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculationLineValue();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtIGSTPerc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculationLineValue();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtSGSTPerc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculationLineValue();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtUGSTPerc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculationLineValue();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtCGSTPerc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculationLineValue();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtInvoiceDate_TextChanged(object sender, EventArgs e)
        {
            ////==================Check Date=======================

            //DateTime InvoiceDate, ReceiptDate;
            //InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
            //ReceiptDate = Convert.ToDateTime(txtReceiptDate.Text);
            //if (InvoiceDate > ReceiptDate)
            //{
            //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Invoice Date cant not greated than Receipt Date !');", true);
            //    txtInvoiceDate.Text = txtReceiptDate.Text;
            //}
        }

        protected void DrpSalesInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //=================Validaion======Check InvoiceNo=================

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string chkInvoice = "select Top 1 Sale_InvoiceNo from [ax].[ACXPURCHINVRECIEPTHEADER] where Sale_InvoiceNo ='" + DrpSalesInvoice.SelectedValue + "' and DataAreaid='" + Session["DATAAREAID"].ToString() + "' and Site_Code='" + Session["SiteCode"].ToString() + "'";
            object objChk = obj.GetScalarValue(chkInvoice);

            if (objChk != null)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('InvoiceNo: " + txtInvoiceNo.Text + " Already exists  !');", true);
                string message = "alert('InvoiceNo: " + DrpSalesInvoice.SelectedValue + " Already exists  !');";
                ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                txtInvoiceDate.ReadOnly = false;
                txtReceiptValue.ReadOnly = false;
                return;
            }
            else
            {
                bool b = ValidateSAPInvoice();
                if (b)
                {
                    bool dataExistOrNot = GetDataFromSAPInvoice();
                    if (dataExistOrNot)
                    {

                        //  CalendarExtender2.StartDate = Convert.ToDateTime(txtInvoiceDate.Text);
                        //  CalendarExtender2.EndDate = Convert.ToDateTime(txtInvoiceDate.Text);
                        txtReceiptValue.ReadOnly = true;
                        //txtReceiptDate.ReadOnly = true;
                        txtReceiptDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);// DateTime.Today.ToString();
                        //txtOrderType.ReadOnly = true;
                        //CalendarExtender3.StartDate = DateTime.Now;
                    }
                }
            }
            //================================================================
        }
    }
}