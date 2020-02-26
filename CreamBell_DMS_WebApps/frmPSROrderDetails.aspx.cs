using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using System.IO;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPSROrderDetails : System.Web.UI.Page
    {

        string strQuery = string.Empty;
        CreamBell_DMS_WebApps.App_Code.Global Obj = new App_Code.Global();
        public DataTable dtData = null;
        SqlConnection conn = null;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {

               // LblMessage.Text = string.Empty;
                fillPSRDetails();
                fillProductGrp();
               // Session["LineItem"] = null;
                txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
                txtToDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today);
           
            }

        }

        protected void DrpPSRDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void fillPSRDetails()
        {
            strQuery = "select distinct([PSR_Code]),[PSR_Code]+'-'+[PSR_Name] as PSR_Name from [ax].[ACXPSRMaster]";
            DrpPSRDetails.Items.Clear();
            DrpPSRDetails.Items.Add("Select...");
            Obj.BindToDropDown(DrpPSRDetails, strQuery, "PSR_Name", "PSR_Code");
        }

        protected void DrpProductGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DrpProductGrp.SelectedIndex > 0)
            //{
            //    fillProductGrp();
            //}
            //else
            //{
            //    DrpProductGrp.Items.Clear();
            //    DrpProductGrp.Items.Add("Select...");

            //}
        }

        public void fillProductGrp()
        {
            strQuery = "select distinct([Product_Group])  from [ax].[INVENTTABLE] ";
            DrpProductGrp.Items.Clear();
            DrpProductGrp.Items.Add("Select...");
            Obj.BindToDropDown(DrpProductGrp, strQuery, "Product_Group", "Product_Group");
        }

        private bool ValidateReport()
        {
            bool ValidateReport = false;
            if ((txtFromDate.Text == string.Empty) && (txtToDate.Text == string.Empty))
            {
                LblMessage.Text = "► Please Select Date Range ! ";
                ValidateReport = false;
                return ValidateReport ;
            }
            if (DrpPSRDetails.Text == "Select...")
            {
                LblMessage.Text = "► Please Select PSR !";
                ValidateReport = false;
                DrpPSRDetails.Focus();
                return ValidateReport;
            }
            else
            {
                ValidateReport = true;
            }
            return ValidateReport;
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = ValidateReport();
            if (b)
            {
                ShowReport();
            }
        }


        private void ShowReport()
        {
            try
            {
                conn = Obj.GetConnection();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Empty;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACX_PSRORDERDETAILS";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITECODE", Session["USERID"].ToString());
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@STARTDATE", this.txtFromDate.Text);
                cmd.Parameters.AddWithValue("@ENDDATE", this.txtToDate.Text);
                cmd.Parameters.AddWithValue("@PSRCODE", this.DrpPSRDetails.SelectedValue.Trim().ToString());
                if(DrpProductGrp.Text== "Select...")
                {
                    cmd.Parameters.AddWithValue("@PRODUCTGROUP", string.Empty);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PRODUCTGROUP", this.DrpProductGrp.Text.Trim().ToString());
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dtData = new DataTable();
                da.Fill(dtData);
                if (dtData.Rows.Count > 0)
                {
                    LoadDataInReportViewer(dtData);
                }
                else
                {
                    LblMessage.Text = "► No Records Exist !!";
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LoadDataInReportViewer(DataTable dtData)
        {
            try
            {
                if (dtData.Rows.Count > 0)
                {
                    #region Show Report in Report Viewer with parameters

                    DataTable DSetParameter = new DataTable();
                    DSetParameter.Columns.Add("SITECODE");
                    DSetParameter.Columns.Add("DATAAREAID");
                    DSetParameter.Columns.Add("STARTDATE");
                    DSetParameter.Columns.Add("ENDDATE");
                    DSetParameter.Columns.Add("PSRCODE");
                    DSetParameter.Columns.Add("PRODUCTGROUP");
                    DSetParameter.Rows.Add();
                    DSetParameter.Rows[0]["SITECODE"] = Session["USERID"].ToString();
                    DSetParameter.Rows[0]["DATAAREAID"] = Session["DATAAREAID"].ToString();
                    DSetParameter.Rows[0]["STARTDATE"] = this.txtFromDate.Text.Trim().ToString();
                    DSetParameter.Rows[0]["ENDDATE"] = this.txtToDate.Text.Trim().ToString();
                    DSetParameter.Rows[0]["PSRCODE"] = this.DrpPSRDetails.SelectedValue.Trim().ToString();
                    if (DrpProductGrp.Text == "Select...")
                    {
                        DSetParameter.Rows[0]["PRODUCTGROUP"] = string.Empty;
                    }
                    else
                    {
                        DSetParameter.Rows[0]["PRODUCTGROUP"] = DrpProductGrp.SelectedItem.Text.Trim().ToString();
                    }

                    ReportDataSource RDS = new ReportDataSource("DSetPSROrderDetails", dtData);
                    rptViewer.AsyncRendering = true;
                    rptViewer.LocalReport.DataSources.Clear();
                    rptViewer.LocalReport.DataSources.Add(RDS);
                    ReportDataSource RDS1 = new ReportDataSource("DataPSRPrm", DSetParameter);
                    rptViewer.LocalReport.DataSources.Add(RDS1);

                    rptViewer.LocalReport.ReportPath = Server.MapPath("Reports\\PSROrderDetails.rdl");
                    rptViewer.LocalReport.Refresh();
                    LblMessage.Text = string.Empty;
                    rptViewer.Visible = true;

                    #endregion
                }
                else
                {
                    LblMessage.Text = "No Records Exists !!";
                    rptViewer.Visible = false;
                }
                
                

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

    }
}