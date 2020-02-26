using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using Elmah;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html;
//using iTextSharp.text.html.simpleparser;


namespace CreamBell_DMS_WebApps
{
    public partial class frmTargetMaster : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        string query = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SiteCode"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (rdoOption.SelectedIndex==0)
                {
                    gvSKUDetails.Visible = false;
                    gvOtherDetails.Visible = true;
                    ShowTargetMaster();
                }
                else
                {
                    gvSKUDetails.Visible = true;
                    gvOtherDetails.Visible = false;
                    ShowSKUTargetMaster();
                }
                GetObjectType();
            }
        }

        private void GetObjectType()
        {
            try
            {               
                query = "select [TARGETOBJECT_TYPE],[TARGETOBJECTTYPE_NAME] from [ax].[ACXTARGETOBJECTMASTER] where [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'";             
                DataTable dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    drpTargetObject.DataSource = dt;
                    drpTargetObject.DataTextField = "TARGETOBJECTTYPE_NAME";
                    drpTargetObject.DataValueField = "TARGETOBJECT_TYPE";
                    drpTargetObject.DataBind();
                    drpTargetObject.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ShowTargetMaster()
        {
            query = " Select A.[TARGETOBJECT_TYPE],B.[TARGETOBJECTTYPE_NAME],A.[TARGET_OBJECT],A.[TARGETOBJECT_NAME],A.[TARGET_TYPE],D.[TARGETTYPE_NAME],A.[TARGET],CONVERT(nvarchar(20), A.[VALIDFROM],106) as [VALIDFROM],CONVERT(nvarchar(20), A.[VALIDTO],106) as [VALIDTO] " +
                    " from [ax].[ACXTARGETMASTER] A "+
                    " inner join  [ax].[ACXTARGETOBJECTMASTER] B on B.[TARGETOBJECT_TYPE]=A.[TARGETOBJECT_TYPE] and B.[DATAAREAID]=A.[DATAAREAID]" +
                    " inner join  [ax].[ACXTARGETTYPEMASTER] D on D.[TARGET_TYPE]=A.[TARGET_TYPE] and D.[DATAAREAID]=A.[DATAAREAID] " +
                    " Where A.SiteID='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + " ' "+                  
                    " Order By  A.[TARGETOBJECT_TYPE],A.[TARGET_OBJECT],A.[TARGETOBJECT_NAME]";

            DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                gvOtherDetails.DataSource = dt;
                gvOtherDetails.DataBind();
                LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();                
            }
            else
            {
                gvOtherDetails.DataSource = dt;
                gvOtherDetails.DataBind();
                LblMessage.Text = string.Empty;
            }
        }

        private void ShowSKUTargetMaster()
        {
            query = "select A.[TARGETOBJECT_TYPE],C.[TARGETOBJECTTYPE_NAME],A.[TARGET_OBJECT],A.[TARGET_TYPE],D.[TARGETTYPE_NAME]" +
                    "  ,A.[PRODUCT_GROUP],A.[PRODUCT_CATEGORY],B.[PRODUCT_SUBCATEGORY],A.[PRODUCT_CODE],A.[PRODUCT_CODE]+'-'+B.[PRODUCT_NAME] as Product " +
                    " ,A.[TARGET], B.[PRODUCT_GROUP],(A.[TARGET]*B.Product_PackSize*B.Ltr)/1000 as Ltr  ,CONVERT(nvarchar(20), A.[VALIDFROM],106) as [VALIDFROM],CONVERT(nvarchar(20), A.[VALIDTO],106) as [VALIDTO]  " +                    
                     " from [ax].[ACXTARGETMASTERSKUWISE] A "+
                     " Inner Join [ax].[INVENTTABLE] B on B.[ITEMID]=A.[PRODUCT_CODE] " +
                     " inner join  [ax].[ACXTARGETOBJECTMASTER] C on C.[TARGETOBJECT_TYPE]=A.[TARGETOBJECT_TYPE] and C.[DATAAREAID]=A.[DATAAREAID] " +
                     " inner join  [ax].[ACXTARGETTYPEMASTER] D on D.[TARGET_TYPE]=A.[TARGET_TYPE] and D.[DATAAREAID]=A.[DATAAREAID]  " +
                     " Where A.SiteID='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + " ' "+
                     " Order By A.[TARGETOBJECT_TYPE],D.[TARGETTYPE_NAME],B.[PRODUCT_GROUP],B.[PRODUCT_SUBCATEGORY],Product";

            DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                gvSKUDetails.DataSource = dt;
                gvSKUDetails.DataBind();
                LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
            }
            else
            {
                gvSKUDetails.DataSource = dt;
                gvSKUDetails.DataBind();
                LblMessage.Text = string.Empty;
            }
        }

        protected void rdoOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoOption.SelectedIndex == 0)
            {
                gvSKUDetails.Visible = false;
                gvOtherDetails.Visible = true;
                ShowTargetMaster();
            }
            else
            {
                gvSKUDetails.Visible = true;
                gvOtherDetails.Visible = false;
                ShowSKUTargetMaster();
            }
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (rdoOption.SelectedIndex == 0)
            {
                if (gvOtherDetails.Rows.Count > 0)
                {
                    ExportToExcelOther();
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
                }
                
            }
            else 
            {
                if (gvSKUDetails.Rows.Count > 0)
                {
                    ExportToExcelSKU();
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
                }
            }
           
        }

        private void ExportToExcelOther()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "TargetMaster" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gvOtherDetails.GridLines = GridLines.Both;
            gvOtherDetails.HeaderStyle.Font.Bold = true;
            gvOtherDetails.RenderControl(htmltextwrtter);
            if (drpTargetObject.SelectedItem.Text != "--Select--")
            {
                Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td> <td></td> <td><b>Target Object: " + drpTargetObject.SelectedItem.Text + "</b></td></tr></table>");
            }
            else
            {
                Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
            }            
            Response.Write(strwritter.ToString());
            Response.End();
        }

        private void ExportToExcelSKU()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "TargetSKUMaster" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gvSKUDetails.GridLines = GridLines.Both;
            gvSKUDetails.HeaderStyle.Font.Bold = true;
            gvSKUDetails.RenderControl(htmltextwrtter);
            if (drpTargetObject.SelectedItem.Text != "--Select--")
            {
                Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td> <td></td> <td><b>Target Object: " + drpTargetObject.SelectedItem.Text + "</b></td></tr></table>");
            }
            else
            {
                Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
            }
            

            Response.Write(strwritter.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private bool ValidateSearch()
        {
            bool value = false;
            if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The Date Range Parameter !');", true);
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The TO Date Range Parameter !');", true);
            }
            if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                value = true;
            }
            return value;
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            bool b = ValidateSearch();
            if (b == true)
            {
                if (rdoOption.SelectedIndex == 0)
                {
                    gvSKUDetails.Visible = false;
                    gvOtherDetails.Visible = true;
                    ShowTargetMaster_Search();
                }
                else
                {
                    gvSKUDetails.Visible = true;
                    gvOtherDetails.Visible = false;
                    ShowSKUTargetMaster_Search();
                }
            }
        }

        private void ShowTargetMaster_Search()
        {
            string FromDate = txtFromDate.Text;           
            string ToDate = txtToDate.Text;

            query = " Select A.[TARGETOBJECT_TYPE],B.[TARGETOBJECTTYPE_NAME],A.[TARGET_OBJECT],A.[TARGETOBJECT_NAME],A.[TARGET_TYPE],D.[TARGETTYPE_NAME],A.[TARGET],CONVERT(nvarchar(20), A.[VALIDFROM],106) as [VALIDFROM],CONVERT(nvarchar(20), A.[VALIDTO],106) as [VALIDTO] " +
                    " from [ax].[ACXTARGETMASTER] A " +
                    " inner join  [ax].[ACXTARGETOBJECTMASTER] B on B.[TARGETOBJECT_TYPE]=A.[TARGETOBJECT_TYPE] and B.[DATAAREAID]=A.[DATAAREAID]" +
                    " inner join  [ax].[ACXTARGETTYPEMASTER] D on D.[TARGET_TYPE]=A.[TARGET_TYPE] and D.[DATAAREAID]=A.[DATAAREAID] " +
                     " Where A.SiteID='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + " ' " +
                     " and (A.[VALIDFROM]>='" + txtFromDate.Text + "' and A.[VALIDTO]<='" + txtToDate.Text + "')";

            if (drpTargetObject.SelectedItem.Text!="--Select--")
            {
                query = query + " and A.[TARGETOBJECT_TYPE]='"+ drpTargetObject.SelectedItem.Value +"' ";
            }
                  
                query=query+ " Order By  A.[TARGETOBJECT_TYPE],A.[TARGET_OBJECT],A.[TARGETOBJECT_NAME]";

            DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                gvOtherDetails.DataSource = dt;
                gvOtherDetails.DataBind();
                LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
            }
            else
            {
                gvOtherDetails.DataSource = dt;
                gvOtherDetails.DataBind();
                LblMessage.Text = string.Empty;
            }
        }

        private void ShowSKUTargetMaster_Search()
        {
            string FromDate = txtFromDate.Text;
            string ToDate = txtToDate.Text;

            query = "select A.[TARGETOBJECT_TYPE],C.[TARGETOBJECTTYPE_NAME],A.[TARGET_OBJECT],A.[TARGET_TYPE],D.[TARGETTYPE_NAME]" +
                    "  ,A.[PRODUCT_GROUP],A.[PRODUCT_CATEGORY],B.[PRODUCT_SUBCATEGORY],A.[PRODUCT_CODE],A.[PRODUCT_CODE]+'-'+B.[PRODUCT_NAME] as Product " +
                    " ,A.[TARGET], B.[PRODUCT_GROUP],(A.[TARGET]*B.Product_PackSize*B.Ltr)/1000 as Ltr  ,CONVERT(nvarchar(20), A.[VALIDFROM],106) as [VALIDFROM],CONVERT(nvarchar(20), A.[VALIDTO],106) as [VALIDTO]  " +
                     " from [ax].[ACXTARGETMASTERSKUWISE] A " +
                     " Inner Join [ax].[INVENTTABLE] B on B.[ITEMID]=A.[PRODUCT_CODE] " +
                     " inner join  [ax].[ACXTARGETOBJECTMASTER] C on C.[TARGETOBJECT_TYPE]=A.[TARGETOBJECT_TYPE] and C.[DATAAREAID]=A.[DATAAREAID] " +
                     " inner join  [ax].[ACXTARGETTYPEMASTER] D on D.[TARGET_TYPE]=A.[TARGET_TYPE] and D.[DATAAREAID]=A.[DATAAREAID]  " +
                     " Where A.SiteID='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + " ' " +
                     " and (A.[VALIDFROM]>='" + txtFromDate.Text + "' and A.[VALIDTO]<='" + txtToDate.Text + "')";
            
            if (drpTargetObject.SelectedItem.Text!="--Select--")
            {
                query = query + " and A.[TARGETOBJECT_TYPE]='"+ drpTargetObject.SelectedItem.Value +"' ";
            }
                
             query=query+ " Order By A.[TARGETOBJECT_TYPE],D.[TARGETTYPE_NAME],B.[PRODUCT_GROUP],B.[PRODUCT_SUBCATEGORY],Product";

            DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                gvSKUDetails.DataSource = dt;
                gvSKUDetails.DataBind();
                LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
            }
            else
            {
                gvSKUDetails.DataSource = dt;
                gvSKUDetails.DataBind();
                LblMessage.Text = string.Empty;
            }
        }
    }
}