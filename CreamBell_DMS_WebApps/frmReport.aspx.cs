using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Reporting;
using Microsoft.Reporting.WebForms;
using System.Net;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmReport : System.Web.UI.Page
    {
        public static string ParameterName = string.Empty;
        HashSet<string> h1 = new HashSet<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadReport(Request.QueryString["Type"].ToString());
            }
        }

        private void ShowReportSaleInvoice(string ReportType, string parameter, string site)
        {
            try
            {
                decimal GlobalTaxAmount = 0;
                App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                App_Code.AmountToWords obj1 = new App_Code.AmountToWords();

                DataTable dtHeader=null;
                DataTable dtLinetest = null;
                DataTable dtLine = new DataTable();
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                DataTable dtAmountWords = null;
                DataTable dtGSTCAL = null;
                string queryHeader = "Acx_SaleInvoiceHeaderReport";
                string queryLine = "Acx_SaleInvoiceLineReport" ;
                string queryGST = "USP_INVOICETAXTRAN"; 

                ilist.Add("@Invoice_No"); litem.Add(parameter);
                ilist.Add("@SiteID"); litem.Add(site);
                



                dtHeader = obj.GetData_New(queryHeader, CommandType.StoredProcedure, ilist, litem);
                dtLinetest = obj.GetData_New(queryLine, CommandType.StoredProcedure, ilist, litem);
                dtGSTCAL = obj.GetData_New(queryGST, CommandType.StoredProcedure, ilist, litem);

                //dtHeader = obj.GetData(queryHeader);
                //dtLinetest = obj.GetData(queryLine);
                decimal TotalAmount = 0;
                if (dtLinetest.Rows.Count > 0)
                {
                    dtLine.Columns.Add("SRNO"); dtLine.Columns.Add("PRODUCT_CODE"); dtLine.Columns.Add("PRODUCT_NAME"); dtLine.Columns.Add("PRODUCT_PACKSIZE");
                    dtLine.Columns.Add("BOX"); dtLine.Columns.Add("BoxConv"); dtLine.Columns.Add("MRP"); dtLine.Columns.Add("RATE"); dtLine.Columns.Add("LTR");
                    dtLine.Columns.Add("TAX_CODE"); dtLine.Columns.Add("CRATES"); dtLine.Columns.Add("AMOUNT");
                    dtLine.Columns.Add("TAX_AMOUNT"); dtLine.Columns.Add("LINEAMOUNT"); dtLine.Columns.Add("DISC_AMOUNT"); dtLine.Columns.Add("SEC_DISC_AMOUNT");
                    dtLine.Columns.Add("ADDTAX_AMOUNT");
                    dtLine.Columns.Add("ADDTAX_CODE"); dtLine.Columns.Add("PEValue");
                    dtLine.Columns.Add("TDValue");
                    dtLine.Columns.Add("TaxableAmt");
                    dtLine.Columns.Add("basic"); dtLine.Columns.Add("TAXCOMPONENT"); dtLine.Columns.Add("ADDTAXCOMPONENT"); dtLine.Columns.Add("HSNCODE");
                    dtLine.Columns.Add("SCHEMEDISCPER"); dtLine.Columns.Add("SCHEMEDISCVALUE");

                    for (int i = 0; i < dtLinetest.Rows.Count; i++)
                    {
                        if (!h1.Contains(dtLinetest.Rows[i]["TAX_CODE"].ToString()))
                        {
                            h1.Add(dtLinetest.Rows[i]["TAX_CODE"].ToString());
                        }
                        dtLine.Rows.Add();
                        dtLine.Rows[i]["SRNO"] = dtLinetest.Rows[i]["SRNO"].ToString();
                        dtLine.Rows[i]["PRODUCT_CODE"] = dtLinetest.Rows[i]["PRODUCT_CODE"].ToString();
                        dtLine.Rows[i]["PRODUCT_NAME"] = dtLinetest.Rows[i]["PRODUCT_NAME"].ToString();
                        dtLine.Rows[i]["PRODUCT_PACKSIZE"] = dtLinetest.Rows[i]["PRODUCT_PACKSIZE"];
                        dtLine.Rows[i]["BOX"] = Convert.ToDecimal(dtLinetest.Rows[i]["BOX"]);
                        dtLine.Rows[i]["BoxConv"] = Convert.ToDecimal(dtLinetest.Rows[i]["BoxConv"]);
                        dtLine.Rows[i]["MRP"] = Convert.ToDecimal(dtLinetest.Rows[i]["MRP"]);
                        dtLine.Rows[i]["RATE"] = Convert.ToDecimal(dtLinetest.Rows[i]["RATE"]);
                        dtLine.Rows[i]["LTR"] = Convert.ToDecimal(dtLinetest.Rows[i]["LTR"]);
                        dtLine.Rows[i]["TAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_CODE"]);
                        dtLine.Rows[i]["CRATES"] = Convert.ToDecimal(dtLinetest.Rows[i]["CRATES"]);
                        dtLine.Rows[i]["AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                        dtLine.Rows[i]["TAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

                        GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

                        dtLine.Rows[i]["LINEAMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["LINEAMOUNT"]);
                        dtLine.Rows[i]["DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["DISC_AMOUNT"]);
                        dtLine.Rows[i]["ADDTAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_AMOUNT"]);
                        dtLine.Rows[i]["SEC_DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["SEC_DISC_AMOUNT"]);
                        dtLine.Rows[i]["ADDTAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_CODE"]);
                        dtLine.Rows[i]["PEValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["PEValue"]);
                        dtLine.Rows[i]["TDValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["TDValue"]);
                        dtLine.Rows[i]["TaxableAmt"] = Convert.ToDecimal(dtLinetest.Rows[i]["TaxableAmt"]);
                        dtLine.Rows[i]["basic"] = Convert.ToDecimal(dtLinetest.Rows[i]["basic"]);
                        dtLine.Rows[i]["TAXCOMPONENT"] = dtLinetest.Rows[i]["TAXCOMPONENT"].ToString();
                        dtLine.Rows[i]["ADDTAXCOMPONENT"] = dtLinetest.Rows[i]["ADDTAXCOMPONENT"].ToString();
                        dtLine.Rows[i]["HSNCODE"] = dtLinetest.Rows[i]["HSNCODE"].ToString();
                        dtLine.Rows[i]["SCHEMEDISCPER"] = Convert.ToDecimal(dtLinetest.Rows[i]["SCHEMEDISCPER"].ToString());
                        dtLine.Rows[i]["SCHEMEDISCVALUE"] = Convert.ToDecimal(dtLinetest.Rows[i]["SCHEMEDISCVALUE"].ToString());
                        TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                    }

                    //decimal HeaderAmount = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());//

                    decimal HeaderAmount = TotalAmount;
                    decimal TotalTaxAmount = GlobalTaxAmount; //dtLine.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));
                    //decimal TotalTaxAmount = 12 ;
                    decimal TotalNetValue = HeaderAmount;     // +TotalTaxAmount;  //Math.Round(HeaderAmount + TotalTaxAmount);

                    //---Calculating Round Off Value for the Sale Invoice Bill---//
                    decimal RoundOffValue = 0;
                    decimal FinalValueForWords = 0;

                    double decimalpoints = Convert.ToDouble(Math.Abs(TotalNetValue - Math.Floor(TotalNetValue)));
                    if (decimalpoints > 0.5)
                    {
                        RoundOffValue = (decimal)Math.Round(TotalNetValue);
                        FinalValueForWords = RoundOffValue;   //+ Convert.ToDecimal(decimalpoints);
                        decimalpoints = 1 - decimalpoints;              // if Rounding Value is greater than 0.50 then plus sign with decimal points.
                    }
                    else
                    {
                        decimalpoints = 0 - decimalpoints;               // if Rounding Value is less than 0.50 then negative sign with decimal points.
                        RoundOffValue = (decimal)Math.Floor(TotalNetValue);
                        FinalValueForWords = RoundOffValue;
                    }

                    if (dtLinetest.Rows.Count > 0)
                    {
                        //h1.Remove("0.00");                                          //----For Finding the different TAX AMOUNT CODE----//
                        int TaxCodeCount = (h1.Count) * 2;                          //--- Count the TAX_CODE rows----//
                        int dtlinecount = dtLine.Rows.Count + 6 + TaxCodeCount;     //---6 is for FOOTER ROWS COUNTS [TOTAL]---//
                        int totalrec = dtlinecount % 41;

                        if ((dtLinetest.Rows.Count > 41 && dtLinetest.Rows.Count < 49) || (totalrec > 0 && totalrec <= 8))
                        {
                        
                            //totalrec = dtlinecount % 32;
                            //int totalcount = 32 - totalrec;                           ///------Total Rows on a single page is 25
                            //if (totalrec != 0)
                            //{
                            //    for (int i = 0; i < totalcount; i++)                        //-For adding the empty rows in a report.
                            //    {
                            //        DataRow dr = dtLine.NewRow();

                            //        dr[0] = ""; dr[1] = ""; dr[2] = "";
                            //        dr[3] = 0;  dr[4] = 0; dr[5] = 0; dr[6] = 0;
                            //        dr[7] = 0;  dr[8] = 0; dr[9] = 0; dr[10] = 0;
                            //        dr[11] = 0; dr[12] = 0; dr[13] = 0;
                            //        dr[14] = 0; dr[15] = 0; dr[16] = 0;
                            //        dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0; 
                            //        dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                            //    }
                           // }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);

                            //string gstquery = "Select HSNCODE,TAXCODE,TAXPER,SUM(TAXAMOUNT) AS TAXAMOUNT from ax.SALESTRANSTAX" +
                            //                 //"  where DOCUMENT_NO = '" +  + "'" +
                            //                 "  GROUP BY TAXCODE,TAXPER,TAXCODE,HSNCODE";

                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.AsyncRendering = true;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGSTCAL);
                            ReportViewer1.LocalReport.DataSources.Add(RDS4);
                            if (Convert.ToInt32(dtHeader.Rows[0]["TRANTYPE"]) == 2)
                            {
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleReturnGST.rdl");
                            }
                            else
                            {
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoiceGST.rdl");
                            }
                            
                            ReportViewer1.ShowPrintButton = true;
                        }
                        else
                        {
                            totalrec = dtlinecount % 41;
                            int totalcount = 41 - totalrec;                           ///------Total Rows on a single page is 25
                            if (totalrec!=0)
                            {
                            //for (int i = 0; i <= totalcount; i++)                        //-For adding the empty rows in a report.
                            //{
                            //    DataRow dr = dtLine.NewRow();

                            //    dr[0] = ""; dr[1] = ""; dr[2] = "";
                            //    dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
                            //    dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
                            //    dr[11] = 0; dr[12] = 0; dr[13] = 0;
                            //    dr[14] = 0; dr[15] = 0; dr[16] = 0;
                            //    dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0; 
                            //    dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                            //}
                           }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);
                            ReportViewer1.AsyncRendering = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGSTCAL);
                            ReportViewer1.LocalReport.DataSources.Add(RDS4);
                            // ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoice1.rdl");
                            if (Convert.ToInt32(dtHeader.Rows[0]["TRANTYPE"]) == 2)
                            {
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleReturnGST.rdl");
                            }
                            else
                            {
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoiceGST.rdl");
                            }
                            ReportViewer1.ShowPrintButton = true;
                        }
                    }
                }

                ReportViewer1.LocalReport.DisplayName = parameter;

                //ReportViewer1.LocalReport.Refresh();
                //UpdatePanel1.Update();

                #region generate PDF of ReportViewer

                string savePath = Server.MapPath("Downloads\\" + ParameterName + ".pdf");
                byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


                // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.        //--Logic Given By Amol The Master Asset of .Net--//
                Response.Buffer = true;
                // Response.Clear();
                Response.ContentType = mimeType;
                string filename1 = ParameterName + "." + extension;
                Response.AddHeader("content-disposition:inline;", "filename=" + filename1);
                Response.BinaryWrite(bytes); // create the file
                //Response.Flush(); // send it to the client to download

             
                ResolveUrl("~/Downloads/" + filename1 + "");

                #endregion

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ShowReportServiceInvoice(string ReportType, string parameter, string site)
        {
            try
            {
                decimal GlobalTaxAmount = 0;
                App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                App_Code.AmountToWords obj1 = new App_Code.AmountToWords();

                DataTable dtHeader = null;
                DataTable dtLinetest = null;
                DataTable dtLine = new DataTable();
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                DataTable dtAmountWords = null;
                DataTable dtGSTCAL = null;
                string queryHeader = "USP_ServiceInvoiceHeaderReport";
                string queryLine = "USP_ServiceInvoiceLineReport";
                string queryGST = "USP_SERVICEINVOICETAXTRAN";

                ilist.Add("@Invoice_No"); litem.Add(parameter);
                ilist.Add("@SiteID"); litem.Add(site);

                dtHeader = obj.GetData_New(queryHeader, CommandType.StoredProcedure, ilist, litem);
                dtLinetest = obj.GetData_New(queryLine, CommandType.StoredProcedure, ilist, litem);
                dtGSTCAL = obj.GetData_New(queryGST, CommandType.StoredProcedure, ilist, litem);

                //dtHeader = obj.GetData(queryHeader);
                //dtLinetest = obj.GetData(queryLine);
                decimal TotalAmount = 0;
                if (dtLinetest.Rows.Count > 0)
                {
                    //dtLine.Columns.Add("SRNO"); dtLine.Columns.Add("PRODUCT_CODE"); dtLine.Columns.Add("PRODUCT_NAME"); dtLine.Columns.Add("PRODUCT_PACKSIZE");
                    //dtLine.Columns.Add("BOX"); dtLine.Columns.Add("BoxConv"); dtLine.Columns.Add("MRP"); dtLine.Columns.Add("RATE"); dtLine.Columns.Add("LTR");
                    //dtLine.Columns.Add("TAX_CODE"); dtLine.Columns.Add("CRATES"); dtLine.Columns.Add("AMOUNT");
                    //dtLine.Columns.Add("TAX_AMOUNT"); dtLine.Columns.Add("LINEAMOUNT"); dtLine.Columns.Add("DISC_AMOUNT"); dtLine.Columns.Add("SEC_DISC_AMOUNT");
                    //dtLine.Columns.Add("ADDTAX_AMOUNT");
                    //dtLine.Columns.Add("ADDTAX_CODE"); dtLine.Columns.Add("PEValue");
                    //dtLine.Columns.Add("TDValue");
                    //dtLine.Columns.Add("TaxableAmt");
                    //dtLine.Columns.Add("basic"); dtLine.Columns.Add("TAXCOMPONENT"); dtLine.Columns.Add("ADDTAXCOMPONENT"); dtLine.Columns.Add("HSNCODE");
                    //dtLine.Columns.Add("SCHEMEDISCPER"); dtLine.Columns.Add("SCHEMEDISCVALUE");

                    //for (int i = 0; i < dtLinetest.Rows.Count; i++)
                    //{
                    //    if (!h1.Contains(dtLinetest.Rows[i]["TAX_CODE"].ToString()))
                    //    {
                    //        h1.Add(dtLinetest.Rows[i]["TAX_CODE"].ToString());
                    //    }
                    //    dtLine.Rows.Add();
                    //    dtLine.Rows[i]["SRNO"] = dtLinetest.Rows[i]["SRNO"].ToString();
                    //    dtLine.Rows[i]["PRODUCT_CODE"] = dtLinetest.Rows[i]["PRODUCT_CODE"].ToString();
                    //    dtLine.Rows[i]["PRODUCT_NAME"] = dtLinetest.Rows[i]["PRODUCT_NAME"].ToString();
                    //    dtLine.Rows[i]["PRODUCT_PACKSIZE"] = dtLinetest.Rows[i]["PRODUCT_PACKSIZE"];
                    //    dtLine.Rows[i]["BOX"] = Convert.ToDecimal(dtLinetest.Rows[i]["BOX"]);
                    //    dtLine.Rows[i]["BoxConv"] = Convert.ToDecimal(dtLinetest.Rows[i]["BoxConv"]);
                    //    dtLine.Rows[i]["MRP"] = Convert.ToDecimal(dtLinetest.Rows[i]["MRP"]);
                    //    dtLine.Rows[i]["RATE"] = Convert.ToDecimal(dtLinetest.Rows[i]["RATE"]);
                    //    dtLine.Rows[i]["LTR"] = Convert.ToDecimal(dtLinetest.Rows[i]["LTR"]);
                    //    dtLine.Rows[i]["TAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_CODE"]);
                    //    dtLine.Rows[i]["CRATES"] = Convert.ToDecimal(dtLinetest.Rows[i]["CRATES"]);
                    //    dtLine.Rows[i]["AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                    //    dtLine.Rows[i]["TAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);
                    //
                    //    GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);
                    //
                    //    dtLine.Rows[i]["LINEAMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["LINEAMOUNT"]);
                    //    dtLine.Rows[i]["DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["DISC_AMOUNT"]);
                    //    dtLine.Rows[i]["ADDTAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_AMOUNT"]);
                    //    dtLine.Rows[i]["SEC_DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["SEC_DISC_AMOUNT"]);
                    //    dtLine.Rows[i]["ADDTAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_CODE"]);
                    //    dtLine.Rows[i]["PEValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["PEValue"]);
                    //    dtLine.Rows[i]["TDValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["TDValue"]);
                    //    dtLine.Rows[i]["TaxableAmt"] = Convert.ToDecimal(dtLinetest.Rows[i]["TaxableAmt"]);
                    //    dtLine.Rows[i]["basic"] = Convert.ToDecimal(dtLinetest.Rows[i]["basic"]);
                    //    dtLine.Rows[i]["TAXCOMPONENT"] = dtLinetest.Rows[i]["TAXCOMPONENT"].ToString();
                    //    dtLine.Rows[i]["ADDTAXCOMPONENT"] = dtLinetest.Rows[i]["ADDTAXCOMPONENT"].ToString();
                    //    dtLine.Rows[i]["HSNCODE"] = dtLinetest.Rows[i]["HSNCODE"].ToString();
                    //    dtLine.Rows[i]["SCHEMEDISCPER"] = Convert.ToDecimal(dtLinetest.Rows[i]["SCHEMEDISCPER"].ToString());
                    //    dtLine.Rows[i]["SCHEMEDISCVALUE"] = Convert.ToDecimal(dtLinetest.Rows[i]["SCHEMEDISCVALUE"].ToString());
                    //    TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                    //}
                    for (int i = 0; i < dtLinetest.Rows.Count; i++)
                    {
                        TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                        GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);
                    }
                    //decimal HeaderAmount = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());//

                    decimal HeaderAmount = TotalAmount;
                    decimal TotalTaxAmount = GlobalTaxAmount; //dtLine.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));
                    //decimal TotalTaxAmount = 12 ;
                    decimal TotalNetValue = HeaderAmount;     // +TotalTaxAmount;  //Math.Round(HeaderAmount + TotalTaxAmount);

                    //---Calculating Round Off Value for the Sale Invoice Bill---//
                    decimal RoundOffValue = 0;
                    decimal FinalValueForWords = 0;

                    double decimalpoints = Convert.ToDouble(Math.Abs(TotalNetValue - Math.Floor(TotalNetValue)));
                    if (decimalpoints > 0.5)
                    {
                        RoundOffValue = (decimal)Math.Round(TotalNetValue);
                        FinalValueForWords = RoundOffValue;   //+ Convert.ToDecimal(decimalpoints);
                        decimalpoints = 1 - decimalpoints;              // if Rounding Value is greater than 0.50 then plus sign with decimal points.
                    }
                    else
                    {
                        decimalpoints = 0 - decimalpoints;               // if Rounding Value is less than 0.50 then negative sign with decimal points.
                        RoundOffValue = (decimal)Math.Floor(TotalNetValue);
                        FinalValueForWords = RoundOffValue;
                    }

                    if (dtLinetest.Rows.Count > 0)
                    {
                        //h1.Remove("0.00");                                          //----For Finding the different TAX AMOUNT CODE----//
                        int TaxCodeCount = (h1.Count) * 2;                          //--- Count the TAX_CODE rows----//
                        int dtlinecount = dtLine.Rows.Count + 6 + TaxCodeCount;     //---6 is for FOOTER ROWS COUNTS [TOTAL]---//
                        int totalrec = dtlinecount % 41;

                        if ((dtLinetest.Rows.Count > 41 && dtLinetest.Rows.Count < 49) || (totalrec > 0 && totalrec <= 8))
                        {

                            //totalrec = dtlinecount % 32;
                            //int totalcount = 32 - totalrec;                           ///------Total Rows on a single page is 25
                            //if (totalrec != 0)
                            //{
                            //    for (int i = 0; i < totalcount; i++)                        //-For adding the empty rows in a report.
                            //    {
                            //        DataRow dr = dtLine.NewRow();

                            //        dr[0] = ""; dr[1] = ""; dr[2] = "";
                            //        dr[3] = 0;  dr[4] = 0; dr[5] = 0; dr[6] = 0;
                            //        dr[7] = 0;  dr[8] = 0; dr[9] = 0; dr[10] = 0;
                            //        dr[11] = 0; dr[12] = 0; dr[13] = 0;
                            //        dr[14] = 0; dr[15] = 0; dr[16] = 0;
                            //        dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0; 
                            //        dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                            //    }
                            // }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.AcxServiceSaleInvheader where INVOICENO='" + parameter + "' and SITEID='" + site.ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);

                            //string gstquery = "Select HSNCODE,TAXCODE,TAXPER,SUM(TAXAMOUNT) AS TAXAMOUNT from ax.SALESTRANSTAX" +
                            //                 //"  where DOCUMENT_NO = '" +  + "'" +
                            //                 "  GROUP BY TAXCODE,TAXPER,TAXCODE,HSNCODE";
                            ReportViewer1.AsyncRendering = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLinetest);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGSTCAL);
                            ReportViewer1.LocalReport.DataSources.Add(RDS4);

                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\ServiceInvoiceGST.rdl");

                            ReportViewer1.ShowPrintButton = true;
                        }
                        else
                        {
                            totalrec = dtlinecount % 41;
                            int totalcount = 41 - totalrec;                           ///------Total Rows on a single page is 25
                            if (totalrec != 0)
                            {
                                //for (int i = 0; i <= totalcount; i++)                        //-For adding the empty rows in a report.
                                //{
                                //    DataRow dr = dtLine.NewRow();

                                //    dr[0] = ""; dr[1] = ""; dr[2] = "";
                                //    dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
                                //    dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
                                //    dr[11] = 0; dr[12] = 0; dr[13] = 0;
                                //    dr[14] = 0; dr[15] = 0; dr[16] = 0;
                                //    dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0; 
                                //    dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                                //}
                            }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.AcxServiceSaleInvheader where INVOICENO='" + parameter + "' and SITEID='" + site.ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);
                            ReportViewer1.AsyncRendering = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLinetest);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGSTCAL);
                            ReportViewer1.LocalReport.DataSources.Add(RDS4);

                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\ServiceInvoiceGST.rdl");

                            ReportViewer1.ShowPrintButton = true;
                        }
                    }
                }

                ReportViewer1.LocalReport.DisplayName = parameter;

                //ReportViewer1.LocalReport.Refresh();
                //UpdatePanel1.Update();

                #region generate PDF of ReportViewer

                string savePath = Server.MapPath("Downloads\\" + ParameterName + ".pdf");
                byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


                // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.        //--Logic Given By Amol The Master Asset of .Net--//
                Response.Buffer = true;
                // Response.Clear();
                Response.ContentType = mimeType;
                string filename1 = ParameterName + "." + extension;
                Response.AddHeader("content-disposition:inline;", "filename=" + filename1);
                Response.BinaryWrite(bytes); // create the file
                                             //Response.Flush(); // send it to the client to download


                ResolveUrl("~/Downloads/" + filename1 + "");

                #endregion

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ShowReportSaleInvoicePreview(string ReportType, string parameter)
        {
            try
            {
                decimal GlobalTaxAmount = 0;
                App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                App_Code.AmountToWords obj1 = new App_Code.AmountToWords();

                DataTable dtHeader = null;
                DataTable dtLinetest = null;
                DataTable dtLine = new DataTable();
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                DataTable dtAmountWords = null;
                DataTable dtGSTCAL = null;
                string queryHeader = "Acx_SaleInvoiceHeaderReportPRV";
                string queryLine = "Acx_SaleInvoiceLineReportPRV";
                string queryGST = "USP_PURCHINVOICETAXTRANPRV";

                //ilist.Add("@Invoice_No"); litem.Add(parameter);
                ilist.Add("@SiteID"); litem.Add(Session["SiteCode"].ToString());




                dtHeader = obj.GetData_New(queryHeader, CommandType.StoredProcedure, ilist, litem);
                dtLinetest = obj.GetData_New(queryLine, CommandType.StoredProcedure, ilist, litem);
                dtGSTCAL = obj.GetData_New(queryGST, CommandType.StoredProcedure, ilist, litem);

                //dtHeader = obj.GetData(queryHeader);
                //dtLinetest = obj.GetData(queryLine);
                decimal TotalAmount = 0;
                if (dtLinetest.Rows.Count > 0)
                {
                    dtLine.Columns.Add("SRNO"); dtLine.Columns.Add("PRODUCT_CODE"); dtLine.Columns.Add("PRODUCT_NAME"); dtLine.Columns.Add("PRODUCT_PACKSIZE");
                    dtLine.Columns.Add("BOX"); dtLine.Columns.Add("BoxConv"); dtLine.Columns.Add("MRP"); dtLine.Columns.Add("RATE"); dtLine.Columns.Add("LTR");
                    dtLine.Columns.Add("TAX_CODE"); dtLine.Columns.Add("CRATES"); dtLine.Columns.Add("AMOUNT");
                    dtLine.Columns.Add("TAX_AMOUNT"); dtLine.Columns.Add("LINEAMOUNT"); dtLine.Columns.Add("DISC_AMOUNT"); dtLine.Columns.Add("SEC_DISC_AMOUNT");
                    dtLine.Columns.Add("ADDTAX_AMOUNT");
                    dtLine.Columns.Add("ADDTAX_CODE"); dtLine.Columns.Add("PEValue");
                    dtLine.Columns.Add("TDValue");
                    dtLine.Columns.Add("TaxableAmt");
                    dtLine.Columns.Add("basic"); dtLine.Columns.Add("TAXCOMPONENT"); dtLine.Columns.Add("ADDTAXCOMPONENT"); dtLine.Columns.Add("HSNCODE");
                    dtLine.Columns.Add("SCHEMEDISCPER"); dtLine.Columns.Add("SCHEMEDISCVALUE");

                    for (int i = 0; i < dtLinetest.Rows.Count; i++)
                    {
                        if (!h1.Contains(dtLinetest.Rows[i]["TAX_CODE"].ToString()))
                        {
                            h1.Add(dtLinetest.Rows[i]["TAX_CODE"].ToString());
                        }
                        dtLine.Rows.Add();
                        dtLine.Rows[i]["SRNO"] = dtLinetest.Rows[i]["SRNO"].ToString();
                        dtLine.Rows[i]["PRODUCT_CODE"] = dtLinetest.Rows[i]["PRODUCT_CODE"].ToString();
                        dtLine.Rows[i]["PRODUCT_NAME"] = dtLinetest.Rows[i]["PRODUCT_NAME"].ToString();
                        dtLine.Rows[i]["PRODUCT_PACKSIZE"] = dtLinetest.Rows[i]["PRODUCT_PACKSIZE"];
                        dtLine.Rows[i]["BOX"] = Convert.ToDecimal(dtLinetest.Rows[i]["BOX"]);
                        dtLine.Rows[i]["BoxConv"] = Convert.ToDecimal(dtLinetest.Rows[i]["BoxConv"]);
                        dtLine.Rows[i]["MRP"] = Convert.ToDecimal(dtLinetest.Rows[i]["MRP"]);
                        dtLine.Rows[i]["RATE"] = Convert.ToDecimal(dtLinetest.Rows[i]["RATE"]);
                        dtLine.Rows[i]["LTR"] = Convert.ToDecimal(dtLinetest.Rows[i]["LTR"]);
                        dtLine.Rows[i]["TAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_CODE"]);
                        dtLine.Rows[i]["CRATES"] = Convert.ToDecimal(dtLinetest.Rows[i]["CRATES"]);
                        dtLine.Rows[i]["AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                        dtLine.Rows[i]["TAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

                        GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

                        dtLine.Rows[i]["LINEAMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["LINEAMOUNT"]);
                        dtLine.Rows[i]["DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["DISC_AMOUNT"]);
                        dtLine.Rows[i]["ADDTAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_AMOUNT"]);
                        dtLine.Rows[i]["SEC_DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["SEC_DISC_AMOUNT"]);
                        dtLine.Rows[i]["ADDTAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_CODE"]);
                        dtLine.Rows[i]["PEValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["PEValue"]);
                        dtLine.Rows[i]["TDValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["TDValue"]);
                        dtLine.Rows[i]["TaxableAmt"] = Convert.ToDecimal(dtLinetest.Rows[i]["TaxableAmt"]);
                        dtLine.Rows[i]["basic"] = Convert.ToDecimal(dtLinetest.Rows[i]["basic"]);
                        dtLine.Rows[i]["TAXCOMPONENT"] = dtLinetest.Rows[i]["TAXCOMPONENT"].ToString();
                        dtLine.Rows[i]["ADDTAXCOMPONENT"] = dtLinetest.Rows[i]["ADDTAXCOMPONENT"].ToString();
                        dtLine.Rows[i]["HSNCODE"] = dtLinetest.Rows[i]["HSNCODE"].ToString();
                        dtLine.Rows[i]["SCHEMEDISCPER"] = Convert.ToDecimal(dtLinetest.Rows[i]["SCHEMEDISCPER"].ToString());
                        dtLine.Rows[i]["SCHEMEDISCVALUE"] = Convert.ToDecimal(dtLinetest.Rows[i]["SCHEMEDISCVALUE"].ToString());
                        TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                    }

                    //decimal HeaderAmount = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());//

                    decimal HeaderAmount = TotalAmount;
                    decimal TotalTaxAmount = GlobalTaxAmount; //dtLine.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));
                    //decimal TotalTaxAmount = 12 ;
                    decimal TotalNetValue = HeaderAmount;     // +TotalTaxAmount;  //Math.Round(HeaderAmount + TotalTaxAmount);

                    //---Calculating Round Off Value for the Sale Invoice Bill---//
                    decimal RoundOffValue = 0;
                    decimal FinalValueForWords = 0;

                    double decimalpoints = Convert.ToDouble(Math.Abs(TotalNetValue - Math.Floor(TotalNetValue)));
                    if (decimalpoints > 0.5)
                    {
                        RoundOffValue = (decimal)Math.Round(TotalNetValue);
                        FinalValueForWords = RoundOffValue;   //+ Convert.ToDecimal(decimalpoints);
                        decimalpoints = 1 - decimalpoints;              // if Rounding Value is greater than 0.50 then plus sign with decimal points.
                    }
                    else
                    {
                        decimalpoints = 0 - decimalpoints;               // if Rounding Value is less than 0.50 then negative sign with decimal points.
                        RoundOffValue = (decimal)Math.Floor(TotalNetValue);
                        FinalValueForWords = RoundOffValue;
                    }

                    if (dtLinetest.Rows.Count > 0)
                    {
                        //h1.Remove("0.00");                                          //----For Finding the different TAX AMOUNT CODE----//
                        int TaxCodeCount = (h1.Count) * 2;                          //--- Count the TAX_CODE rows----//
                        int dtlinecount = dtLine.Rows.Count + 6 + TaxCodeCount;     //---6 is for FOOTER ROWS COUNTS [TOTAL]---//
                        int totalrec = dtlinecount % 41;

                        if ((dtLinetest.Rows.Count > 41 && dtLinetest.Rows.Count < 49) || (totalrec > 0 && totalrec <= 8))
                        {

                            //totalrec = dtlinecount % 32;
                            //int totalcount = 32 - totalrec;                           ///------Total Rows on a single page is 25
                            //if (totalrec != 0)
                            //{
                            //    for (int i = 0; i < totalcount; i++)                        //-For adding the empty rows in a report.
                            //    {
                            //        DataRow dr = dtLine.NewRow();

                            //        dr[0] = ""; dr[1] = ""; dr[2] = "";
                            //        dr[3] = 0;  dr[4] = 0; dr[5] = 0; dr[6] = 0;
                            //        dr[7] = 0;  dr[8] = 0; dr[9] = 0; dr[10] = 0;
                            //        dr[11] = 0; dr[12] = 0; dr[13] = 0;
                            //        dr[14] = 0; dr[15] = 0; dr[16] = 0;
                            //        dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0; 
                            //        dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                            //    }
                            // }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);

                            //string gstquery = "Select HSNCODE,TAXCODE,TAXPER,SUM(TAXAMOUNT) AS TAXAMOUNT from ax.SALESTRANSTAX" +
                            //                 //"  where DOCUMENT_NO = '" +  + "'" +
                            //                 "  GROUP BY TAXCODE,TAXPER,TAXCODE,HSNCODE";
                            ReportViewer1.AsyncRendering = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGSTCAL);
                            ReportViewer1.LocalReport.DataSources.Add(RDS4);
                            if (Convert.ToInt32(dtHeader.Rows[0]["TRANTYPE"]) == 2)
                            {
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleReturnGST.rdl");
                            }
                            else
                            {
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoiceGST.rdl");
                            }

                            ReportViewer1.ShowPrintButton = true;
                        }
                        else
                        {
                            totalrec = dtlinecount % 41;
                            int totalcount = 41 - totalrec;                           ///------Total Rows on a single page is 25
                            if (totalrec != 0)
                            {
                                //for (int i = 0; i <= totalcount; i++)                        //-For adding the empty rows in a report.
                                //{
                                //    DataRow dr = dtLine.NewRow();

                                //    dr[0] = ""; dr[1] = ""; dr[2] = "";
                                //    dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
                                //    dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
                                //    dr[11] = 0; dr[12] = 0; dr[13] = 0;
                                //    dr[14] = 0; dr[15] = 0; dr[16] = 0;
                                //    dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0; 
                                //    dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                                //}
                            }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);
                            ReportViewer1.AsyncRendering = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGSTCAL);
                            ReportViewer1.LocalReport.DataSources.Add(RDS4);
                            // ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoice1.rdl");
                            if (Convert.ToInt32(dtHeader.Rows[0]["TRANTYPE"]) == 2)
                            {
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleReturnGST.rdl");
                            }
                            else
                            {
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoiceGST.rdl");
                            }
                            ReportViewer1.ShowPrintButton = true;
                        }
                    }
                }

                ReportViewer1.LocalReport.DisplayName = parameter;

                //ReportViewer1.LocalReport.Refresh();
                //UpdatePanel1.Update();

                #region generate PDF of ReportViewer

                string savePath = Server.MapPath("Downloads\\" + ParameterName + ".pdf");
                byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


                // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.        //--Logic Given By Amol The Master Asset of .Net--//
                Response.Buffer = true;
                // Response.Clear();
                Response.ContentType = mimeType;
                string filename1 = ParameterName + "." + extension;
                Response.AddHeader("content-disposition:inline;", "filename=" + filename1);
                Response.BinaryWrite(bytes); // create the file
                                             //Response.Flush(); // send it to the client to download


                ResolveUrl("~/Downloads/" + filename1 + "");

                #endregion

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ShowReportLoadSheet(string ReportType, string parameter)
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new CreamBell_DMS_WebApps.App_Code.Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter = null;
             try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                CreamBell_DMS_WebApps.App_Code.AmountToWords obj1 = new App_Code.AmountToWords();
                DataTable dtHeader = null;
                DataTable dtLine = null;
                DataTable dtAmountWords = null;
               
                String queryHeader= "USP_LOADSHEETHEADERREPORT '" + Session["SiteCode"].ToString() + "','" + parameter + "'";
               

                ////string queryHeader = "select LSH.LOADSHEET_NO,  CONVERT(varchar(15),LSH.LOADSHEET_DATE,105) AS LOADSHEET_DATE , " +
                //                     " ( SH.CUSTOMER_CODE +'-' + CUS.CUSTOMER_NAME) as CUSTOMER, SH.SO_NO, CONVERT( varchar(15), SH.SO_DATE,105) AS SO_DATE , " +
                //                     " cast(round(SH.SO_VALUE,2) as numeric(36,2)) as SO_VALUE, SH.SITEID, USM.User_Name,USM.State from ax.ACXLOADSHEETHEADER LSH " +
                //                     " INNER JOIN ax.ACXSALESHEADER SH ON LSH.LOADSHEET_NO = SH.LoadSheet_No  and  LSH.SITEID= SH.SITEID" +  // add LSH.SITEID= SH.SITEID on 7thApr2017
                //                     " INNER JOIN AX.ACXCUSTMASTER CUS ON SH.CUSTOMER_CODE = CUS.CUSTOMER_CODE "+ //deduct on 4th Apr 2017 and cus.Site_Code = LSH.SITEID" +
                //                     " INNER JOIN AX.ACXUSERMASTER USM ON LSH.SITEID= USM.Site_Code " +
                //                     " where LSH.SITEID = '" + Session["SiteCode"].ToString() + "'  and LSH.LOADSHEET_NO='" + parameter + "'";


                //string queryLine = " Select ROW_NUMBER() over (ORDER BY LSH.PRODUCT_CODE) AS SRNO,( LSH.PRODUCT_CODE + '-'+ PM.PRODUCT_NAME) AS PRODUCT, " +
                //              " LSH.BOXQTY as BOX,LSH.PCSQTY as PCS,isnull(LSH.BOXPCS,0) as TotalBoxPCS,LSH.BOX as TotalQtyConv,LSH.LTR, LSH.STOCKQTY_BOX,LSH.STOCKQTY_LTR from ax.ACXLOADSHEETLINE LSH " +
                //            " Inner Join ax.inventtable PM on LSH.PRODUCT_CODE = PM.ITEMID " +
                //          " where SHITEID = '" + Session["SiteCode"].ToString() + "' and LOADSHEET_NO='" + parameter + "'";

                String queryLine = "Exec USP_LOADSHEETLINEREPORT '"+ Session["SiteCode"].ToString() + "','"+ parameter + "'";
                
                dtHeader = obj.GetData(queryHeader);
                dtLine = obj.GetData(queryLine);

                string query = "Select VALUE  from ax.ACXLOADSHEETHEADER where LOADSHEET_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";
                DataTable dt = obj.GetData(query);


                decimal amount = Math.Round(Convert.ToDecimal(dt.Rows[0]["VALUE"].ToString()));
                string Words = obj1.words(Convert.ToInt32(amount));
                string queryAmountWords = "Select VALUE, '" + Words + "' as AMNTWORDS from ax.ACXLOADSHEETHEADER where LOADSHEET_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                dtAmountWords = obj.GetData(queryAmountWords);
                ReportViewer1.AsyncRendering = true;
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(RDS1);
                ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                ReportViewer1.LocalReport.DataSources.Add(RDS2);
                ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                ReportViewer1.LocalReport.DataSources.Add(RDS3);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\LoadSheet.rdl");
                ReportViewer1.ShowPrintButton = true;
                //ReportViewer1.LocalReport.Refresh();
                //UpdatePanel1.Update();

                #region Generate PDF of LoadSheet

                string savePath = Server.MapPath("Downloads\\" + parameter + ".pdf");
                byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.        //--Logic Given By Amol The Master Asset of .Net--//
                Response.Buffer = true;
                // Response.Clear();
                Response.ContentType = mimeType;
                string filename1 = parameter + "." + extension;
                Response.AddHeader("content-disposition:inline;", "filename=" + filename1);
                Response.BinaryWrite(bytes); // create the file
                //Response.Flush(); // send it to the client to download

                ResolveUrl("~/Downloads/" + filename1 + "");

                #endregion

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            
        }

        private void ShowReportDebitNote(string ReportType, string parameter)
        {
            try
            {
                decimal GlobalTaxAmount = 0;
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                CreamBell_DMS_WebApps.App_Code.AmountToWords obj1 = new App_Code.AmountToWords();

                DataTable dtHeader = null;
                DataTable dtLinetest = null;
                DataTable dtLine = new DataTable();
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                List<string> ilist1 = new List<string>();
                List<string> litem1 = new List<string>();
                DataTable dtAmountWords = null;
                DataTable dtGSTCAL = null;
                string queryHeader = "USP_GETDEBITNOTEHEADER";
                string queryLine = "USP_GETDEBITNOTELINE";
                string queryGST = "USP_PURCHINVOICETAXTRAN";

                ilist.Add("@InvoiceNo"); litem.Add(parameter);
                ilist.Add("@SiteID"); litem.Add(Session["SiteCode"].ToString());

                ilist1.Add("@Invoice_No"); litem1.Add(parameter);
                ilist1.Add("@SiteID"); litem1.Add(Session["SiteCode"].ToString());




                dtHeader = obj.GetData_New(queryHeader, CommandType.StoredProcedure, ilist, litem);
                dtLinetest = obj.GetData_New(queryLine, CommandType.StoredProcedure, ilist, litem);
                dtGSTCAL = obj.GetData_New(queryGST, CommandType.StoredProcedure, ilist1, litem1);

                //dtHeader = obj.GetData(queryHeader);
                //dtLinetest = obj.GetData(queryLine);
                decimal TotalAmount = 0;
                if (dtLinetest.Rows.Count > 0)
                {
                    dtLine.Columns.Add("SRNO"); dtLine.Columns.Add("PRODUCT_CODE"); dtLine.Columns.Add("PRODUCT_NAME"); dtLine.Columns.Add("PRODUCT_PACKSIZE");
                    dtLine.Columns.Add("BOX"); dtLine.Columns.Add("BoxConv"); dtLine.Columns.Add("MRP"); dtLine.Columns.Add("RATE"); dtLine.Columns.Add("LTR");
                    dtLine.Columns.Add("TAX"); dtLine.Columns.Add("CRATES"); dtLine.Columns.Add("AMOUNT");
                    dtLine.Columns.Add("TAXAMOUNT");
                    //dtLine.Columns.Add("LINEAMOUNT"); 
                    dtLine.Columns.Add("BASICVALUE");
                    dtLine.Columns.Add("DISC_AMOUNT"); //dtLine.Columns.Add("SEC_DISC_AMOUNT");
                    dtLine.Columns.Add("ADDTAX_AMOUNT");
                    dtLine.Columns.Add("ADDTAX_CODE"); dtLine.Columns.Add("PEValue");
                    dtLine.Columns.Add("TDValue");
                    dtLine.Columns.Add("TaxableAmt");
                    dtLine.Columns.Add("basic");

                    //dtLine.Columns.Add("TAXCOMPONENT");
                    //dtLine.Columns.Add("ADDTAXCOMPONENT"); 
                    dtLine.Columns.Add("HSNCODE");
                    //dtLine.Columns.Add("SCHEMEDISCPER");
                    dtLine.Columns.Add("SEC_DISC_VAL");

                    for (int i = 0; i < dtLinetest.Rows.Count; i++)
                    {
                        if (!h1.Contains(dtLinetest.Rows[i]["TAX"].ToString()))
                        {
                            h1.Add(dtLinetest.Rows[i]["TAX"].ToString());
                        }
                        dtLine.Rows.Add();
                        dtLine.Rows[i]["SRNO"] = dtLinetest.Rows[i]["SRNO"].ToString();
                        dtLine.Rows[i]["PRODUCT_CODE"] = dtLinetest.Rows[i]["PRODUCT_CODE"].ToString();
                        dtLine.Rows[i]["PRODUCT_NAME"] = dtLinetest.Rows[i]["PRODUCT_NAME"].ToString();
                        dtLine.Rows[i]["PRODUCT_PACKSIZE"] = dtLinetest.Rows[i]["PRODUCT_PACKSIZE"];
                        dtLine.Rows[i]["BOX"] = Convert.ToDecimal(dtLinetest.Rows[i]["BOX"]);
                        dtLine.Rows[i]["BoxConv"] = Convert.ToDecimal(dtLinetest.Rows[i]["BoxConv"]);
                        dtLine.Rows[i]["MRP"] = Convert.ToDecimal(dtLinetest.Rows[i]["MRP"]);
                        dtLine.Rows[i]["RATE"] = Convert.ToDecimal(dtLinetest.Rows[i]["RATE"]);
                        dtLine.Rows[i]["LTR"] = Convert.ToDecimal(dtLinetest.Rows[i]["LTR"]);
                        dtLine.Rows[i]["TAX"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX"]);
                        dtLine.Rows[i]["CRATES"] = Convert.ToDecimal(dtLinetest.Rows[i]["CRATES"]);
                        dtLine.Rows[i]["AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                        dtLine.Rows[i]["TAXAMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAXAMOUNT"]);

                        GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAXAMOUNT"]);

                        dtLine.Rows[i]["BASICVALUE"] = Convert.ToDecimal(dtLinetest.Rows[i]["BASICVALUE"]);
                        dtLine.Rows[i]["DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["DISC_AMOUNT"]);
                        dtLine.Rows[i]["ADDTAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_AMOUNT"]);
                        //dtLine.Rows[i]["SEC_DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["SEC_DISC_AMOUNT"]);
                        dtLine.Rows[i]["ADDTAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_CODE"]);
                        dtLine.Rows[i]["PEValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["PEValue"]);
                        dtLine.Rows[i]["TDValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["TDValue"]);
                        dtLine.Rows[i]["TaxableAmt"] = Convert.ToDecimal(dtLinetest.Rows[i]["TaxableAmt"]);
                        dtLine.Rows[i]["basic"] = Convert.ToDecimal(dtLinetest.Rows[i]["basic"]);
                        //dtLine.Rows[i]["TAXCOMPONENT"] = dtLinetest.Rows[i]["TAXCOMPONENT"].ToString();
                        //dtLine.Rows[i]["ADDTAXCOMPONENT"] = dtLinetest.Rows[i]["ADDTAXCOMPONENT"].ToString();
                        dtLine.Rows[i]["HSNCODE"] = dtLinetest.Rows[i]["HSNCODE"].ToString();
                        //dtLine.Rows[i]["SCHEMEDISCPER"] = Convert.ToDecimal(dtLinetest.Rows[i]["SCHEMEDISCPER"].ToString());
                        dtLine.Rows[i]["SEC_DISC_VAL"] = Convert.ToDecimal(dtLinetest.Rows[i]["SEC_DISC_VAL"].ToString());
                        TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                    }

                    //decimal HeaderAmount = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());//

                    decimal HeaderAmount = TotalAmount;
                    decimal TotalTaxAmount = GlobalTaxAmount; //dtLine.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));
                    //decimal TotalTaxAmount = 12 ;
                    decimal TotalNetValue = HeaderAmount;     // +TotalTaxAmount;  //Math.Round(HeaderAmount + TotalTaxAmount);

                    //---Calculating Round Off Value for the Sale Invoice Bill---//
                    decimal RoundOffValue = 0;
                    decimal FinalValueForWords = 0;

                    double decimalpoints = Convert.ToDouble(Math.Abs(TotalNetValue - Math.Floor(TotalNetValue)));
                    if (decimalpoints > 0.5)
                    {
                        RoundOffValue = (decimal)Math.Round(TotalNetValue);
                        FinalValueForWords = RoundOffValue;   //+ Convert.ToDecimal(decimalpoints);
                        decimalpoints = 1 - decimalpoints;              // if Rounding Value is greater than 0.50 then plus sign with decimal points.
                    }
                    else
                    {
                        decimalpoints = 0 - decimalpoints;               // if Rounding Value is less than 0.50 then negative sign with decimal points.
                        RoundOffValue = (decimal)Math.Floor(TotalNetValue);
                        FinalValueForWords = RoundOffValue;
                    }

                    if (dtLinetest.Rows.Count > 0)
                    {
                        //h1.Remove("0.00");                                          //----For Finding the different TAX AMOUNT CODE----//
                        int TaxCodeCount = (h1.Count) * 2;                          //--- Count the TAX_CODE rows----//
                        int dtlinecount = dtLine.Rows.Count + 6 + TaxCodeCount;     //---6 is for FOOTER ROWS COUNTS [TOTAL]---//
                        int totalrec = dtlinecount % 41;

                        if ((dtLinetest.Rows.Count > 41 && dtLinetest.Rows.Count < 49) || (totalrec > 0 && totalrec <= 8))
                        {

                            //totalrec = dtlinecount % 32;
                            //int totalcount = 32 - totalrec;                           ///------Total Rows on a single page is 25
                            //if (totalrec != 0)
                            //{
                            //    for (int i = 0; i < totalcount; i++)                        //-For adding the empty rows in a report.
                            //    {
                            //        DataRow dr = dtLine.NewRow();

                            //        dr[0] = ""; dr[1] = ""; dr[2] = "";
                            //        dr[3] = 0;  dr[4] = 0; dr[5] = 0; dr[6] = 0;
                            //        dr[7] = 0;  dr[8] = 0; dr[9] = 0; dr[10] = 0;
                            //        dr[11] = 0; dr[12] = 0; dr[13] = 0;
                            //        dr[14] = 0; dr[15] = 0; dr[16] = 0;
                            //        dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0; 
                            //        dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                            //    }
                            // }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);

                            //string gstquery = "Select HSNCODE,TAXCODE,TAXPER,SUM(TAXAMOUNT) AS TAXAMOUNT from ax.SALESTRANSTAX" +
                            //                 //"  where DOCUMENT_NO = '" +  + "'" +
                            //                 "  GROUP BY TAXCODE,TAXPER,TAXCODE,HSNCODE";
                            ReportViewer1.AsyncRendering = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGSTCAL);
                            ReportViewer1.LocalReport.DataSources.Add(RDS4);
                            //if (Convert.ToInt32(dtHeader.Rows[0]["TRANTYPE"]) == 2)
                            //{
                            //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleReturnGST.rdl");
                            //}
                            //else
                            //{
                            //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoiceGST.rdl");
                            //}
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PurchaseReturnGST.rdl");
                            ReportViewer1.ShowPrintButton = true;
                        }
                        else
                        {
                            totalrec = dtlinecount % 41;
                            int totalcount = 41 - totalrec;                           ///------Total Rows on a single page is 25
                            if (totalrec != 0)
                            {
                                //for (int i = 0; i <= totalcount; i++)                        //-For adding the empty rows in a report.
                                //{
                                //    DataRow dr = dtLine.NewRow();

                                //    dr[0] = ""; dr[1] = ""; dr[2] = "";
                                //    dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
                                //    dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
                                //    dr[11] = 0; dr[12] = 0; dr[13] = 0;
                                //    dr[14] = 0; dr[15] = 0; dr[16] = 0;
                                //    dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0; 
                                //    dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                                //}
                            }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select RETURN_DOCVALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.[ACXPURCHRETURNHEADER] where PURCH_RETURNNO='" + parameter + "' and SITE_CODE='" + Session["SiteCode"].ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);
                            ReportViewer1.AsyncRendering = true;
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGSTCAL);
                            ReportViewer1.LocalReport.DataSources.Add(RDS4);
                            // ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoice1.rdl");
                            //if (Convert.ToInt32(dtHeader.Rows[0]["TRANTYPE"]) == 2)
                            //{
                            //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleReturnGST.rdl");
                            //}
                            //else
                            //{
                            //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoiceGST.rdl");
                            //}
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PurchaseReturnGST.rdl");
                            ReportViewer1.ShowPrintButton = true;
                        }
                    }
                }

                ReportViewer1.LocalReport.DisplayName = parameter;

                //ReportViewer1.LocalReport.Refresh();
                //UpdatePanel1.Update();

                #region generate PDF of ReportViewer

                string savePath = Server.MapPath("Downloads\\" + ParameterName + ".pdf");
                byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;

                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


                // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.        //--Logic Given By Amol The Master Asset of .Net--//
                Response.Buffer = true;
                // Response.Clear();
                Response.ContentType = mimeType;
                string filename1 = ParameterName + "." + extension;
                Response.AddHeader("content-disposition:inline;", "filename=" + filename1);
                Response.BinaryWrite(bytes); // create the file
                //Response.Flush(); // send it to the client to download


                ResolveUrl("~/Downloads/" + filename1 + "");

                #endregion

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            SavePDF(ReportViewer1, ParameterName);
        }

        private void ShowReportSaleInvoice_Old(string ReportType, string parameter)
        {
            {
                try
                {
                    decimal GlobalTaxAmount = 0;
                    CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                    CreamBell_DMS_WebApps.App_Code.AmountToWords obj1 = new App_Code.AmountToWords();
                    DataTable dtHeader = null;
                    DataTable dtLinetest = null;
                    DataTable dtLine = new DataTable();
                    List<string> ilist = new List<string>();
                    List<string> litem = new List<string>();
                    DataTable dtAmountWords = null;
                    string queryHeader = "Acx_SaleInvoiceHeaderReport";
                    string queryLine = "Acx_SaleInvoiceLineReport";


                    ilist.Add("@Invoice_No"); litem.Add(parameter);
                    ilist.Add("@SiteID"); litem.Add(Session["SiteCode"].ToString());

                    dtHeader = obj.GetData_New(queryHeader, CommandType.StoredProcedure, ilist, litem);
                    dtLinetest = obj.GetData_New(queryLine, CommandType.StoredProcedure, ilist, litem);

                    decimal TotalAmount = 0;
                    if (dtLinetest.Rows.Count > 0)
                    {
                        dtLine.Columns.Add("SRNO"); dtLine.Columns.Add("PRODUCT_CODE"); dtLine.Columns.Add("PRODUCT_NAME"); dtLine.Columns.Add("PRODUCT_PACKSIZE");
                        dtLine.Columns.Add("BOX"); dtLine.Columns.Add("BoxConv"); dtLine.Columns.Add("MRP"); dtLine.Columns.Add("RATE"); dtLine.Columns.Add("LTR");
                        dtLine.Columns.Add("TAX_CODE"); dtLine.Columns.Add("CRATES"); dtLine.Columns.Add("AMOUNT");
                        dtLine.Columns.Add("TAX_AMOUNT"); dtLine.Columns.Add("LINEAMOUNT"); dtLine.Columns.Add("DISC_AMOUNT"); dtLine.Columns.Add("SEC_DISC_AMOUNT");
                        dtLine.Columns.Add("ADDTAX_AMOUNT");
                        dtLine.Columns.Add("ADDTAX_CODE"); dtLine.Columns.Add("PEValue");
                        dtLine.Columns.Add("TDValue");
                        dtLine.Columns.Add("TaxableAmt");
                        dtLine.Columns.Add("basic");

                        for (int i = 0; i < dtLinetest.Rows.Count; i++)
                        {
                            if (!h1.Contains(dtLinetest.Rows[i]["TAX_CODE"].ToString()))
                            {
                                h1.Add(dtLinetest.Rows[i]["TAX_CODE"].ToString());
                            }
                            dtLine.Rows.Add();
                            dtLine.Rows[i]["SRNO"] = dtLinetest.Rows[i]["SRNO"].ToString();
                            dtLine.Rows[i]["PRODUCT_CODE"] = dtLinetest.Rows[i]["PRODUCT_CODE"].ToString();
                            dtLine.Rows[i]["PRODUCT_NAME"] = dtLinetest.Rows[i]["PRODUCT_NAME"].ToString();
                            dtLine.Rows[i]["PRODUCT_PACKSIZE"] = dtLinetest.Rows[i]["PRODUCT_PACKSIZE"];
                            dtLine.Rows[i]["BOX"] = Convert.ToDecimal(dtLinetest.Rows[i]["BOX"]);
                            dtLine.Rows[i]["BoxConv"] = Convert.ToDecimal(dtLinetest.Rows[i]["BoxConv"]);
                            dtLine.Rows[i]["MRP"] = Convert.ToDecimal(dtLinetest.Rows[i]["MRP"]);
                            dtLine.Rows[i]["RATE"] = Convert.ToDecimal(dtLinetest.Rows[i]["RATE"]);
                            dtLine.Rows[i]["LTR"] = Convert.ToDecimal(dtLinetest.Rows[i]["LTR"]);
                            dtLine.Rows[i]["TAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_CODE"]);
                            dtLine.Rows[i]["CRATES"] = Convert.ToDecimal(dtLinetest.Rows[i]["CRATES"]);
                            dtLine.Rows[i]["AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                            dtLine.Rows[i]["TAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

                            GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

                            dtLine.Rows[i]["LINEAMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["LINEAMOUNT"]);
                            dtLine.Rows[i]["DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["DISC_AMOUNT"]);
                            dtLine.Rows[i]["ADDTAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_AMOUNT"]);
                            dtLine.Rows[i]["SEC_DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["SEC_DISC_AMOUNT"]);
                            dtLine.Rows[i]["ADDTAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_CODE"]);
                            dtLine.Rows[i]["PEValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["PEValue"]);
                            dtLine.Rows[i]["TDValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["TDValue"]);
                            dtLine.Rows[i]["TaxableAmt"] = Convert.ToDecimal(dtLinetest.Rows[i]["TaxableAmt"]);
                            dtLine.Rows[i]["basic"] = Convert.ToDecimal(dtLinetest.Rows[i]["basic"]);

                            TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                        }

                        //decimal HeaderAmount = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());//

                        decimal HeaderAmount = TotalAmount;
                        decimal TotalTaxAmount = GlobalTaxAmount; //dtLine.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));
                        //decimal TotalTaxAmount = 12 ;
                        decimal TotalNetValue = HeaderAmount;     // +TotalTaxAmount;  //Math.Round(HeaderAmount + TotalTaxAmount);

                        //---Calculating Round Off Value for the Sale Invoice Bill---//
                        decimal RoundOffValue = 0;
                        decimal FinalValueForWords = 0;

                        double decimalpoints = Convert.ToDouble(Math.Abs(TotalNetValue - Math.Floor(TotalNetValue)));
                        if (decimalpoints > 0.5)
                        {
                            RoundOffValue = (decimal)Math.Round(TotalNetValue);
                            FinalValueForWords = RoundOffValue;   //+ Convert.ToDecimal(decimalpoints);
                            decimalpoints = 1 - decimalpoints;              // if Rounding Value is greater than 0.50 then plus sign with decimal points.
                        }
                        else
                        {
                            decimalpoints = 0 - decimalpoints;               // if Rounding Value is less than 0.50 then negative sign with decimal points.
                            RoundOffValue = (decimal)Math.Floor(TotalNetValue);
                            FinalValueForWords = RoundOffValue;
                        }

                        if (dtLinetest.Rows.Count > 0)
                        {
                            //h1.Remove("0.00");                                          //----For Finding the different TAX AMOUNT CODE----//
                            int TaxCodeCount = (h1.Count) * 2;                          //--- Count the TAX_CODE rows----//
                            int dtlinecount = dtLine.Rows.Count + 6 + TaxCodeCount;     //---6 is for FOOTER ROWS COUNTS [TOTAL]---//
                            int totalrec = dtlinecount % 41;

                            if ((dtLinetest.Rows.Count > 41 && dtLinetest.Rows.Count < 49) || (totalrec > 0 && totalrec <= 8))
                            {

                                totalrec = dtlinecount % 32;
                                int totalcount = 32 - totalrec;                           ///------Total Rows on a single page is 25
                                if (totalrec != 0)
                                {
                                    for (int i = 0; i < totalcount; i++)                        //-For adding the empty rows in a report.
                                    {
                                        DataRow dr = dtLine.NewRow();

                                        dr[0] = ""; dr[1] = ""; dr[2] = "";
                                        dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
                                        dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
                                        dr[11] = 0; dr[12] = 0; dr[13] = 0;
                                        dr[14] = 0; dr[15] = 0; dr[16] = 0;
                                        dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0;
                                        dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                                    }
                                }
                                string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                                string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                                dtAmountWords = obj.GetData(queryAmountWords);
                                ReportViewer1.AsyncRendering = true;
                                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                                ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.LocalReport.DataSources.Add(RDS1);
                                ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                                ReportViewer1.LocalReport.DataSources.Add(RDS2);
                                ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                                ReportViewer1.LocalReport.DataSources.Add(RDS3);
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoice.rdl");
                                ReportViewer1.ShowPrintButton = true;
                            }
                            else
                            {
                                totalrec = dtlinecount % 41;
                                int totalcount = 41 - totalrec;                           ///------Total Rows on a single page is 25
                                if (totalrec != 0)
                                {
                                    for (int i = 0; i <= totalcount; i++)                        //-For adding the empty rows in a report.
                                    {
                                        DataRow dr = dtLine.NewRow();

                                        dr[0] = ""; dr[1] = ""; dr[2] = "";
                                        dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
                                        dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
                                        dr[11] = 0; dr[12] = 0; dr[13] = 0;
                                        dr[14] = 0; dr[15] = 0; dr[16] = 0;
                                        dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0;
                                        dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                                    }
                                }
                                string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                                string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + parameter + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                                dtAmountWords = obj.GetData(queryAmountWords);
                                ReportViewer1.AsyncRendering = true;
                                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                                ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.LocalReport.DataSources.Add(RDS1);
                                ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                                ReportViewer1.LocalReport.DataSources.Add(RDS2);
                                ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                                ReportViewer1.LocalReport.DataSources.Add(RDS3);
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoice1.rdl");
                                ReportViewer1.ShowPrintButton = true;
                            }
                        }
                    }

                    ReportViewer1.LocalReport.DisplayName = parameter;

                    //ReportViewer1.LocalReport.Refresh();
                    //UpdatePanel1.Update();

                    #region generate PDF of ReportViewer

                    string savePath = Server.MapPath("Downloads\\" + ParameterName + ".pdf");
                    byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


                    // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.        //--Logic Given By Amol The Master Asset of .Net--//
                    Response.Buffer = true;
                    // Response.Clear();
                    Response.ContentType = mimeType;
                    string filename1 = ParameterName + "." + extension;
                    Response.AddHeader("content-disposition:inline;", "filename=" + filename1);
                    Response.BinaryWrite(bytes); // create the file
                    //Response.Flush(); // send it to the client to download


                    ResolveUrl("~/Downloads/" + filename1 + "");

                    #endregion

                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }

        private void LoadReport(string Type)
        {
            if (Request.QueryString["Type"].ToString() != string.Empty && Request.QueryString["Type"].ToString() == "SaleInvoice")
            {
                if (Request.QueryString["SaleInvoiceNo"].ToString() != string.Empty)
                {
                    string SaleInvParameter = Request.QueryString["SaleInvoiceNo"].ToString();
                    string ReportType = Request.QueryString["Type"].ToString();
                    string site = Request.QueryString["Site"].ToString();
                    ParameterName = SaleInvParameter;

                    ShowReportSaleInvoice(ReportType, SaleInvParameter,site);
                }
            }

            if (Request.QueryString["Type"].ToString() != string.Empty && Request.QueryString["Type"].ToString() == "ServiceInvoice")
            {
                if (Request.QueryString["ServiceInvoiceNo"].ToString() != string.Empty)
                {
                    string SaleInvParameter = Request.QueryString["ServiceInvoiceNo"].ToString();
                    string ReportType = Request.QueryString["Type"].ToString();
                    string site = Request.QueryString["Site"].ToString();
                    ParameterName = SaleInvParameter;

                    ShowReportServiceInvoice(ReportType, SaleInvParameter, site);
                }
            }

            if (Request.QueryString["Type"].ToString() != string.Empty && Request.QueryString["Type"].ToString() == "SaleInvoiceOld")
            {
                if (Request.QueryString["SaleInvoiceNo"].ToString() != string.Empty)
                {
                    string SaleInvParameter = Request.QueryString["SaleInvoiceNo"].ToString();
                    string ReportType = Request.QueryString["Type"].ToString();
                    ParameterName = SaleInvParameter;

                    ShowReportSaleInvoice_Old(ReportType, SaleInvParameter);
                }
            }


            if (Request.QueryString["Type"].ToString() != string.Empty && Request.QueryString["Type"].ToString() == "LoadSheet")
            {
                if (Request.QueryString["LaodSheetNo"].ToString() != string.Empty)
                {
                    string LoadSheetParameter = Request.QueryString["LaodSheetNo"].ToString();
                    string ReportType = Request.QueryString["Type"].ToString();
                    ParameterName = LoadSheetParameter;

                    ShowReportLoadSheet(ReportType, LoadSheetParameter);
                }
            }

            if (Request.QueryString["Type"].ToString() != string.Empty && Request.QueryString["Type"].ToString() == "DebitNote")
            {
                if (Request.QueryString["SaleInvoiceNo"].ToString() != string.Empty)
                {
                    string SaleInvParameter = Request.QueryString["SaleInvoiceNo"].ToString();
                    string ReportType = Request.QueryString["Type"].ToString();
                    ParameterName = SaleInvParameter;

                    ShowReportDebitNote(ReportType, SaleInvParameter);
                }
            }

            if (Request.QueryString["Type"].ToString() != string.Empty && Request.QueryString["Type"].ToString() == "SaleInvoicePreview")
            {
                if (Request.QueryString["SaleInvoiceNo"].ToString() != string.Empty)
                {
                    string SaleInvParameter = Request.QueryString["SaleInvoiceNo"].ToString();
                    string ReportType = Request.QueryString["Type"].ToString();
                    ParameterName = SaleInvParameter;

                    ShowReportSaleInvoicePreview(ReportType, SaleInvParameter);
                }
            }
        }

        public void SavePDF(ReportViewer viewer, string filename)
        {
            string savePath = Server.MapPath("Downloads\\"+filename+".pdf");
            byte[] Bytes = viewer.LocalReport.Render(format: "PDF", deviceInfo: "");

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.        //--Logic Given By Amol The Master Asset of .Net--//
            Response.Buffer = true;
            // Response.Clear();
            Response.ContentType = mimeType;
            string filename1 = filename + "." + extension;
            Response.AddHeader("content-disposition:inline;","filename=" + filename1);
            Response.BinaryWrite(bytes); // create the file
           
            //Response.Flush(); // send it to the client to download
            
             ResolveUrl("~/Downloads/"+filename1+"");
        }

    }

}