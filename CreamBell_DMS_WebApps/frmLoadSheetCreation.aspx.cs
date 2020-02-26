using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmLoadSheetCreation : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
       
        SqlConnection conn = null;
        SqlDataAdapter adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();
        decimal TotalQtyConv = 0,PcsQty=0,BoxQty=0;
        decimal Crates = 0;
        decimal ltr = 0;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Visible = false;
                if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                string sitecode1;

                sitecode1 = Session["SiteCode"].ToString();

                if (!Page.IsPostBack)
                {
//                    string str = @"Select h.SO_No,h.[DELIVERY_DATE],C.CUST_GROUP,h.Customer_Code,C.CUSTOMER_NAME,C.ADDRESS1,APB.BeatName ,C.PSR_BEAT,h.SO_DATE,SO_Value,
//                              psr.PSR_Name ,h.PSR_CODE,case when h.App_SO_NO!='' then 'MOBILE' else 'MANUAL' end as SOTYPE
//                             from [ax].[ACXSALESHEADER] h left Join 
//                             [ax].[ACXCUSTMASTER] C on h.Customer_Code = C.Customer_Code and C.SITE_CODE = h.SITEID                                   
//                             left join [ax].[ACXPSRBeatMaster] APB on c.PSR_BEAT = APB.BeatCode and h.PSR_CODE = APB.PSRCode
//							 left join [ax].[ACXPSRMaster] psr on  h.PSR_CODE = psr.PSR_Code  
//                             where loadsheet_Status = 0 and h.siteid = '" + sitecode1 + "' order by h.[DELIVERY_DATE] desc,h.SO_DATE desc,h.SO_No desc";

                    string str = @"Select h.SO_No,h.[DELIVERY_DATE],C.CUST_GROUP,h.Customer_Code,C.CUSTOMER_NAME,C.ADDRESS1,APB.BeatName ,C.PSR_BEAT,h.SO_DATE,SO_Value,
                              psr.PSR_Name ,h.PSR_CODE,case when h.App_SO_NO!='' then 'MOBILE' else 'MANUAL' end as SOTYPE
                             from [ax].[ACXSALESHEADER] h left Join 
                             [ax].[ACXCUSTMASTER] C on h.Customer_Code = C.Customer_Code                                  
                             left join [ax].[ACXPSRBeatMaster] APB on c.PSR_BEAT = APB.BeatCode and h.PSR_CODE = APB.PSRCode
							 left join [ax].[ACXPSRMaster] psr on  h.PSR_CODE = psr.PSR_Code  
                             where loadsheet_Status = 0 and h.siteid = '" + sitecode1 + "' order by h.[DELIVERY_DATE] desc,h.SO_DATE desc,h.SO_No desc";

                    BindGrid(str);                          //Bind GridView Method which Take String Param
                    CalendarExtender1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblMsg.Text = "Error :" + ex.Message;
                lblMsg.Visible = true;
            }
        }
        protected void BindGrid(string sqlstr)
        {
            DataTable dt =new DataTable();
            dt = baseObj.GetData(sqlstr);    //callign a Global Class getData 
            if(dt.Rows.Count>0)
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();     //Bind Sale Header
            } 
        }
        protected void txtQtyBox_TextChanged(object sender, EventArgs e)                //This for calculate Qty value,Crate and Ltr for sale Line
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((TextBox)sender)).NamingContainer;
                TextBox txtBox = (TextBox)gvrow.FindControl("txtBox");
                TextBox txtPCS = (TextBox)gvrow.FindControl("txtPCS");
                TextBox txtQty = (TextBox)gvrow.FindControl("txtqty"); //total Qty conv
                TextBox txtBoxPcs = (TextBox)gvrow.FindControl("txtBoxPcs"); //total Qty conv
                HiddenField productPack = (HiddenField)gvrow.FindControl("HiddenField2");
                if (txtBox.Text == "")
                { txtBox.Text = "0"; }
                if (txtPCS.Text == "")
                { txtPCS.Text = "0"; }


                if (Convert.ToDecimal(txtPCS.Text) > Convert.ToDecimal(productPack.Value))
                {
                    int pcs = Convert.ToInt32(Convert.ToDecimal(txtPCS.Text));
                    int pac = Convert.ToInt32(Convert.ToDecimal(productPack.Value));
                    int addbox = pcs / pac;
                    int remainder = pcs % pac;
                    if (remainder == 0)
                    {
                        txtPCS.Text = "0";
                    }
                    else
                    {
                        txtPCS.Text = Convert.ToString(remainder);
                    }
                    txtBox.Text = Convert.ToString(Convert.ToInt32(Convert.ToDouble(txtBox.Text)) + addbox);
                }

                if (txtBox.Text != "" && txtPCS.Text != "")
                {
                    string str = (Convert.ToInt32(txtBox.Text) + (Convert.ToDecimal(txtPCS.Text) / Convert.ToDecimal(productPack.Value))).ToString("N6");
                    txtQty.Text = str;
                }

                DataTable dt = new DataTable();
                dt = baseObj.GetData("SELECT BOXPRINT FROM [dbo].[udf_GetPCSDetails](" + App_Code.Global.ConvertToDecimal(txtQty.Text).ToString() + "," + Convert.ToDecimal(productPack.Value) + ")");
                if (dt.Rows.Count > 0)
                {
                    txtBoxPcs.Text = dt.Rows[0][0].ToString();
                }

                string[] calValue = baseObj.CalculatePrice1(gvrow.Cells[1].Text, string.Empty, decimal.Parse(txtQty.Text), "Box");
                if (calValue.Length > 0)
                {
                    gvrow.Cells[3].Text = calValue[0];                   //Crate
                    gvrow.Cells[8].Text = calValue[1];                   //ltr
                }
                if (Convert.ToDecimal(txtQty.Text) > Convert.ToDecimal(gvrow.Cells[9].Text))
                {
                    gvrow.BackColor = System.Drawing.Color.Red;
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please check stock Qty for ITEM:" + gvrow.Cells[1].Text + " !');", true);
                    txtQty.Text = "0";
                    gvrow.Cells[3].Text = "0";
                    gvrow.Cells[8].Text = "0";
                }
                else
                {
                    gvrow.BackColor = System.Drawing.Color.White;
                }

                ltr = 0;
                Crates = 0;
                BoxQty = 0;
                TotalQtyConv = 0;
                PcsQty = 0;

                foreach (GridViewRow grv in GridView2.Rows)
                {
                    string str = grv.Cells[3].Text;
                    Crates += Convert.ToDecimal(grv.Cells[3].Text);
                    TextBox txtQtyConv = (TextBox)grv.Cells[6].FindControl("txtqty");
                    TotalQtyConv += Convert.ToDecimal(txtQtyConv.Text);
                    TextBox txtBoxQty = (TextBox)grv.Cells[4].FindControl("txtBox");
                    BoxQty += Convert.ToDecimal(txtBoxQty.Text);
                    TextBox txtPcsQty = (TextBox)grv.Cells[5].FindControl("txtPCS");
                    PcsQty += Convert.ToDecimal(txtPcsQty.Text);
                    ltr += Convert.ToDecimal(grv.Cells[8].Text);
                }

                GridView2.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                GridView2.FooterRow.Cells[2].ForeColor = System.Drawing.Color.MidnightBlue;
                GridView2.FooterRow.Cells[2].Text = "TOTAL : ";
                GridView2.FooterRow.Cells[2].Font.Bold = true;

                GridView2.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                GridView2.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
                GridView2.FooterRow.Cells[3].Text = Crates.ToString();
                GridView2.FooterRow.Cells[3].Font.Bold = true;

                GridView2.FooterRow.Cells[4].Text = BoxQty.ToString();
                GridView2.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                GridView2.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
                GridView2.FooterRow.Cells[4].Font.Bold = true;

                GridView2.FooterRow.Cells[5].Text = PcsQty.ToString();
                GridView2.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                GridView2.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
                GridView2.FooterRow.Cells[5].Font.Bold = true;

                GridView2.FooterRow.Cells[6].Text = TotalQtyConv.ToString();
                GridView2.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                GridView2.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
                GridView2.FooterRow.Cells[6].Font.Bold = true;

                GridView2.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                GridView2.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
                GridView2.FooterRow.Cells[8].Text = ltr.ToString();
                GridView2.FooterRow.Cells[8].Font.Bold = true;

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Input String was not in Correct format !');", true);
            }
            //UppnalegridDetails.Update();
        }
        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)                           //sale Header Grid View for Filling the Sale Line
        {
            try 
            {
                string strSONO = string.Empty;
                DataTable dt = new DataTable();
                foreach (GridViewRow row in gvDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkStatus") as CheckBox);
                        if (chkRow.Checked)
                        {
                            //strSONO += "'" + chkRow.Text + "',";                 //getting all So_No those are checked by User 
                            strSONO += "" + chkRow.Text + ",";
                        }
                    }
                }
                //==============For Warehouse Loacion 11-5-16===========
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string TransLocation = "";

                string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
                }
                //====================================
                if (strSONO != string.Empty)
                {
                    strSONO = strSONO.Remove(strSONO.Length - 1);

                    //==============11-5-16========
                    //string strSaleLine = "Select AP.PRODUCT_GROUP,AL.PRODUCT_CODE,AP.PRODUCT_NAME,Sum(AL.CRATES) as CRATES,cast(Sum(AL.BOX) as integer) As BOX,cast(Sum(AL.BOX) as decimal(9,2)) As TotalQty,Sum(AL.LTR) AS LTR ,Cast(Round((cast(Sum(AL.BOX) as decimal(9,2))-cast(Sum(AL.BOX) as integer))*AP.Product_PackSize,2) as integer) as PCS,AP.Product_PackSize" +
                    //                      ",StockBox=(Select coalesce(cast(sum(F.TransQty) as decimal(10,2)),0) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]=AL.SITEID and F.[ProductCode]=AL.Product_COde and F.[TransLocation]='" + TransLocation + "')" +
                    //                      ",StockLtr =(Select coalesce(cast((sum(F.TransQty)*AP.Product_PackSize*AP.LTR)/1000 as decimal(10,2)),0) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]=AL.SITEID and F.[ProductCode]=AL.Product_COde and F.[TransLocation]='" + TransLocation + "')" +
                    //                      " from [ax].[ACXSALESLINE] AL Inner Join [ax].[InventTable] AP on AL.PRODUCT_CODE = AP.ItemId " +
                    //                      " where SO_No In (" + strSONO + ") and  AL.SiteId='" + Session["SiteCode"].ToString() + "' group by AL.SITEID,AP.PRODUCT_GROUP,AL.PRODUCT_CODE,AP.PRODUCT_NAME,AP.Product_PackSize,AP.LTR";

                    string strSaleLine = "EXEC ACX_GETLOADSHEET '" + Session["SiteCode"].ToString() + "','" + TransLocation + "','" + strSONO + "'";

                    dt = baseObj.GetData(strSaleLine);
                    if (dt.Rows.Count > 0)
                    {
                        GridView2.Visible = true;
                        GridView2.DataSource = dt;
                        GridView2.DataBind();

                        GridView2.FooterRow.Cells[2].Text = "Total :";
                        GridView2.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                        GridView2.FooterRow.Cells[2].ForeColor = System.Drawing.Color.MidnightBlue;
                        GridView2.FooterRow.Cells[2].Font.Bold = true;

                        decimal Crates = dt.AsEnumerable().Sum(row => row.Field<decimal>("CRATES"));
                        GridView2.FooterRow.Cells[3].Text = Crates.ToString("N2");
                        GridView2.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                        GridView2.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
                        GridView2.FooterRow.Cells[3].Font.Bold = true;

                        int BoxQty = dt.AsEnumerable().Sum(row => row.Field<int>("BOX"));
                        GridView2.FooterRow.Cells[4].Text = BoxQty.ToString();
                        GridView2.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                        GridView2.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
                        GridView2.FooterRow.Cells[4].Font.Bold = true;

                        int PCS = dt.AsEnumerable().Sum(row => row.Field<int>("PCS"));
                        GridView2.FooterRow.Cells[5].Text = PCS.ToString();
                        GridView2.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        GridView2.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
                        GridView2.FooterRow.Cells[5].Font.Bold = true;
                        

                        decimal TotalQtyConv = dt.AsEnumerable().Sum(row => row.Field<decimal>("TotalQty"));
                        GridView2.FooterRow.Cells[6].Text = TotalQtyConv.ToString("N2");
                        GridView2.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                        GridView2.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
                        GridView2.FooterRow.Cells[6].Font.Bold = true;
                        

                        decimal ltr = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTR"));
                        GridView2.FooterRow.Cells[8].Text = ltr.ToString("N2");
                        GridView2.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                        GridView2.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
                        GridView2.FooterRow.Cells[8].Font.Bold = true;
                       
                     
                    }
                    else
                    {
                        dt = null;
                        GridView2.DataSource = dt;
                        GridView2.DataBind();
                    }
                }
                else
                {
                    dt = null;
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
                //UppnalegridDetails.Update();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
          
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Savegrid();
            Session["LSNO"] = null;

        }
        
        protected Boolean Savegrid()
        {
        
        string sitecode1;
            sitecode1 = Session["SiteCode"].ToString();

            Boolean ValidateStockQty = true;

            string strItem = "";
            decimal lsQty = 0; //
            foreach (GridViewRow gvr in GridView2.Rows)
            {
                TextBox txtBox = ((TextBox)gvr.Cells[0].FindControl("txtqty"));
                string stockBox = gvr.Cells[9].Text;
                lsQty += (txtBox.Text.Trim().Length == 0 ? 0 : Convert.ToDecimal(txtBox.Text)); //k
                if (Convert.ToDecimal(txtBox.Text) < 0 || Convert.ToDecimal(txtBox.Text) > Convert.ToDecimal(stockBox))
                {
                    ValidateStockQty = false;

                    if (strItem == "")
                    {
                        strItem = gvr.Cells[1].Text;
                    }
                    else
                    {
                        strItem = strItem + "," + gvr.Cells[1].Text; ;
                    }

                    // ScriptManager.RegisterStartupScript(this,this.GetType(), "Alert", " alert(' ITEM No:- " + gvr.Cells[1].Text + " can't be more than stock qty !!');", true);
                    //return;
                }

            }
            if (lsQty == 0)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please fill Qty !');", true);
                return false;
            }

            if (ValidateStockQty == true)
            {

                // checkItem();
                Addloadsheetheader();
                // Addloadsheetline();
                gvDetails.DataSource = null;
                gvDetails.DataBind();

//                string str = @"SELECT h.SO_No,h.[DELIVERY_DATE],C.CUST_GROUP,h.Customer_Code,C.CUSTOMER_NAME,C.ADDRESS1,
//                    APB.BeatName ,C.PSR_BEAT,h.SO_DATE,SO_Value,psr.PSR_Name ,h.PSR_CODE,
//                    CASE WHEN h.App_SO_NO!='' THEN 'MOBILE' ELSE 'MANUAL' END AS SOTYPE
//                    FROM [AX].[ACXSALESHEADER] h 
//                    INNER JOIN [AX].[ACXCUSTMASTER] C on h.Customer_Code = C.Customer_Code AND C.SITE_CODE = h.SITEID       
//                    LEFT JOIN [AX].[ACXPSRBeatMaster] APB ON c.PSR_BEAT = APB.BeatCode AND h.PSR_CODE = APB.PSRCode
//                    LEFT JOIN [AX].[ACXPSRMaster] PSR ON  h.PSR_CODE = psr.PSR_Code                             
//                    WHERE LoadSheet_Status = 0 AND H.siteid = '" + sitecode1 + "' ORDER BY h.[DELIVERY_DATE] ASC,h.SO_DATE ASC";

                // Chamged on 7th Apr 2017
                string str = @"Select h.SO_No,h.[DELIVERY_DATE],C.CUST_GROUP,h.Customer_Code,C.CUSTOMER_NAME,C.ADDRESS1
                             ,APB.BeatName ,C.PSR_BEAT,h.SO_DATE,SO_Value,psr.PSR_Name ,h.PSR_CODE
                             ,case when h.App_SO_NO!='' then 'MOBILE' else 'MANUAL' end as SOTYPE
                             from [ax].[ACXSALESHEADER] h 
                             left Join [ax].[ACXCUSTMASTER] C on h.Customer_Code = C.Customer_Code                                  
                             left join [ax].[ACXPSRBeatMaster] APB on c.PSR_BEAT = APB.BeatCode and h.PSR_CODE = APB.PSRCode
							 left join [ax].[ACXPSRMaster] psr on  h.PSR_CODE = psr.PSR_Code  
                             where loadsheet_Status = 0 and h.siteid = '" + sitecode1 + "' order by h.[DELIVERY_DATE] desc,h.SO_DATE desc,h.SO_No desc";

                BindGrid(str);
                GridView2.DataSource = null;
                GridView2.DataBind();
                return true;
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please check stock Qty for ITEM:" + strItem + " !');", true);
                return false;
            }
          

        }


        private void checkItem()
        {
            foreach(GridViewRow gvr in GridView2.Rows)
            {
                TextBox txtBox = ((TextBox)gvr.Cells[0].FindControl("txtqty"));                           
                string stockBox = gvr.Cells[6].Text;

                if (Convert.ToDecimal(txtBox.Text) <= 0 || Convert.ToDecimal(txtBox.Text) > Convert.ToDecimal(stockBox))
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert(' ITEM No:- " + gvr.Cells[1].Text + " can't be more than stock qty !!');", true);
                    return;
                }

            }
        }
        private void Addloadsheetheader()
        {
           
            string productcode1;
            int recid;

            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            conn = obj.GetConnection();

            adp1 = new SqlDataAdapter("SELECT Top (1) recid FROM ax.ACXLOADSHEETHEADER order by recid desc ", conn);
            ds2.Clear();
            adp1.Fill(ds2, "dtl");

            if (ds2.Tables["dtl"].Rows.Count != 0)
            {
                productcode1 = string.Copy(ds2.Tables["dtl"].Rows[0][0].ToString());
                recid = Convert.ToInt32((productcode1).ToString()) + 1;
            }
            else
            {
                recid = 1;
            }
            SqlTransaction transaction;
            
            transaction = conn.BeginTransaction();

            int k = 0;

            try
            {
                SqlCommand comm = new SqlCommand();
                SqlCommand comm1 = new SqlCommand();
                SqlCommand comm2 = new SqlCommand();
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataTable dt = new DataTable();
                string sono = "";

                //string strLoadSheetNo = obj.GetNumSequence(5, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());

                DataTable dtNumSeq = obj.GetNumSequenceNew(5, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                string strLoadSheetNo = string.Empty;
                string NUMSEQ = string.Empty;
                if (dtNumSeq != null)
                {
                    strLoadSheetNo = dtNumSeq.Rows[0][0].ToString();
                    NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                }
                else
                {
                    return;
                }

                bool flag = false;
                string lodNO = string.Empty;
                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gvDetails.Rows[i].FindControl("chkStatus")).Checked)
                    {
                        sono = ((CheckBox)gvDetails.Rows[i].FindControl("chkStatus")).Text;
                        
                        string sodate = gvDetails.Rows[i].Cells[1].Text;
                        string customergroup = gvDetails.Rows[i].Cells[2].Text;
                        string customercode = gvDetails.Rows[i].Cells[3].Text;
                        string psrname = gvDetails.Rows[i].Cells[6].Text;
                        string beatname = gvDetails.Rows[i].Cells[8].Text;
                        string deliverydate = gvDetails.Rows[i].Cells[10].Text;
                        string value = gvDetails.Rows[i].Cells[11].Text;
                        

                        string sitecode1, DataAreaId;

                        sitecode1 = Session["SiteCode"].ToString();
                        DataAreaId = Session["DATAAREAID"].ToString();
                        decimal value1;

                        value1 = Convert.ToDecimal(value);

                        comm = new SqlCommand("ax.ACX_ACXLOADSHEETHEADER", conn, transaction);

                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandTimeout = 0;
                        comm.Parameters.AddWithValue("@SITEID", sitecode1);
                        comm.Parameters.AddWithValue("@DATAAREAID", DataAreaId);
                        comm.Parameters.AddWithValue("@RECID", i + recid );
                        comm.Parameters.AddWithValue("@CUSTOMER_CODE", customercode);
                        comm.Parameters.AddWithValue("@LOADSHEET_NO", strLoadSheetNo);
                        lodNO = strLoadSheetNo;

                        comm.Parameters.AddWithValue("@SO_NO", sono);
                        comm.Parameters.AddWithValue("@SO_DATE", sodate);
                        comm.Parameters.AddWithValue("@PSR_CODE", psrname);
                        comm.Parameters.AddWithValue("@PSR_BEAT", beatname);
                        comm.Parameters.AddWithValue("@INVOICE_DATE", deliverydate);
                        comm.Parameters.AddWithValue("@DELIVERY_DATE", deliverydate);
                        comm.Parameters.AddWithValue("@VALUE", value1);
                        comm.Parameters.AddWithValue("@RATE", value1);
                        comm.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);

                        int a1 = comm.ExecuteNonQuery();

                        string box = ""; decimal  box2;

                        for (int j = 0; j < GridView2.Rows.Count; j++)
                        {
                            string productgroup = GridView2.Rows[j].Cells[0].Text;
                            string productcode = GridView2.Rows[j].Cells[1].Text;
                            string crate = GridView2.Rows[j].Cells[3].Text;
                        
                            TextBox txtBox = ((TextBox)GridView2.Rows[j].FindControl("txtqty")); //with conv
                            TextBox txtPcs = ((TextBox)GridView2.Rows[j].FindControl("txtPCS")); //pcs
                            TextBox txtBoxQty = ((TextBox)GridView2.Rows[j].FindControl("txtBox")); //box qty
                            TextBox txtBoxPcs = ((TextBox)GridView2.Rows[j].FindControl("txtBoxPcs"));// boxpcs
                            
                            box = txtBox.Text;
                            string Ltr = GridView2.Rows[j].Cells[8].Text;
                            string stockBox = GridView2.Rows[j].Cells[9].Text;
                            string StockLtr = GridView2.Rows[j].Cells[10].Text;
                          
                            string sitecode2, DataAreaId2, uom;
                            uom = "";
                            sitecode2 = Session["SiteCode"].ToString();
                            DataAreaId2 = Session["DATAAREAID"].ToString();

                            decimal crate1, ltr1, stockBox1, StockLtr1;
                            
                            stockBox1 = Convert.ToDecimal(stockBox);
                            StockLtr1 = Convert.ToDecimal(StockLtr);
                            crate1 = Convert.ToDecimal(crate);
                            box2 = Convert.ToDecimal(box);
                            ltr1 = Convert.ToDecimal(Ltr);

                            if (Convert.ToDecimal(txtBox.Text) <= Convert.ToDecimal(stockBox) && Convert.ToDecimal(txtBox.Text)>0)
                            {
                                comm1 = new SqlCommand("ax.ACX_ACXLOADSHEETLINE", conn, transaction);

                                comm1.CommandType = CommandType.StoredProcedure;
                                comm1.CommandTimeout = 0;
                                comm1.Parameters.AddWithValue("@SHITEID", sitecode2);
                                comm1.Parameters.AddWithValue("@DATAAREAID", DataAreaId2);
                                comm1.Parameters.AddWithValue("@RECID", j + 1 + recid);
                                comm1.Parameters.AddWithValue("@CUSTOMER_CODE", customercode);
                                comm1.Parameters.AddWithValue("@LOADSHEET_NO", strLoadSheetNo);
                                comm1.Parameters.AddWithValue("@LINE_NO", j + 1);
                                comm1.Parameters.AddWithValue("@UOM", uom);
                                comm1.Parameters.AddWithValue("@STOCKQTY_BOX", stockBox1);
                                comm1.Parameters.AddWithValue("@STOCKQTY_LTR", StockLtr1);
                                comm1.Parameters.AddWithValue("@BOX", box2);
                                comm1.Parameters.AddWithValue("@CRATES", crate1);
                                comm1.Parameters.AddWithValue("@LINENUM", j + 1);
                                comm1.Parameters.AddWithValue("@LTR", ltr1);
                                comm1.Parameters.AddWithValue("@PRODUCT_CODE", productcode);
                                comm1.Parameters.AddWithValue("@BOXQty", Convert.ToDecimal(txtBoxQty.Text.Trim()));
                                comm1.Parameters.AddWithValue("@PcsQty", Convert.ToDecimal(txtPcs.Text.Trim()));
                                comm1.Parameters.AddWithValue("@BOXPcs", txtBoxPcs.Text.Trim());

                                int a = comm1.ExecuteNonQuery();
                            }
                        }

                        for (k = 0; k < gvDetails.Rows.Count; k++)
                        {
                            if (((CheckBox)gvDetails.Rows[k].FindControl("chkStatus")).Checked)
                            {
                                sono = ((CheckBox)gvDetails.Rows[k].FindControl("chkStatus")).Text;

                                sitecode1 = Session["SiteCode"].ToString();
                                DataAreaId = Session["DATAAREAID"].ToString();

                                comm2 = new SqlCommand("ax.ACX_ACXLOADSHEETHEADERUPDATE_LOADNO", conn, transaction);

                                comm2.CommandType = CommandType.StoredProcedure;

                                comm2.Parameters.AddWithValue("@LOADSHEET_NO", strLoadSheetNo);
                                comm2.Parameters.AddWithValue("@SO_NO", sono);
                                comm2.Parameters.AddWithValue("@SiteCode", sitecode1);
                                comm2.Parameters.AddWithValue("@DATAAREAID", DataAreaId);

                                int a2 = comm2.ExecuteNonQuery();
                            }
                        }

                        //obj.UpdateLastNumSequence(5, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);

                        transaction.Commit();
                        
                        flag = true;
                        break;
                    }
                    
                }
                if (flag == true)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Load Sheet N0:" + lodNO + " generated Suucessfull!');", true);
                    Session["LSNO"] = lodNO;
                }

            }
            catch(Exception ex)
            {
                transaction.Rollback();
                conn.Close();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void btn2_Click(object sender, EventArgs e)
        {
            string sitecode1;

            sitecode1 = Session["SiteCode"].ToString();

            gvDetails.DataSource = null;
            gvDetails.DataBind();

            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            conn = obj.GetConnection();
            try
            {
                if (drpSearch.SelectedItem.Text == "SO Number")
                {
                    adp1 = new SqlDataAdapter("Select h.SO_No,h.[DELIVERY_DATE],C.CUST_GROUP,h.Customer_Code" +
                                ",C.CUSTOMER_NAME,C.ADDRESS1,APB.BeatName ,C.PSR_BEAT,h.SO_DATE,SO_Value,psr.PSR_Name , h.PSR_CODE,h.PSR_CODE,case when h.App_SO_NO!='' then 'MOBILE' else 'MANUAL' end as SOTYPE " +
                                " from [ax].[ACXSALESHEADER] h Inner Join " +
                                " [ax].[ACXCUSTMASTER] C on h.Customer_Code = C.Customer_Code" +
                                //" and C.[DATAAREAID] = h.[DATAAREAID] " +
                                " left join [ax].[ACXPSRBeatMaster] APB on c.PSR_BEAT = APB.BeatCode and h.PSR_CODE = APB.PSRCode " +
							    " left join [ax].[ACXPSRMaster] psr on  h.PSR_CODE = psr.PSR_Code "  +                          
                                " where loadsheet_Status = 0 and h.siteid = '" + sitecode1 + "' " +
                                " and  h.SO_NO like '%" + txtSerch.Text + "%' " +
                                " order by h.[DELIVERY_DATE] asc,h.SO_DATE asc", conn);
                }
                else if (drpSearch.SelectedItem.Text == "Date")
                {
                    adp1 = new SqlDataAdapter("Select h.SO_No,h.[DELIVERY_DATE],C.CUST_GROUP,h.Customer_Code" +
                                ",C.CUSTOMER_NAME,C.ADDRESS1,APB.BeatName ,C.PSR_BEAT,h.SO_DATE,SO_Value,psr.PSR_Name ,h.PSR_CODE,case when h.App_SO_NO!='' then 'MOBILE' else 'MANUAL' end as SOTYPE " +
                                " from [ax].[ACXSALESHEADER] h Inner Join " +
                                " [ax].[ACXCUSTMASTER] C on h.Customer_Code = C.Customer_Code" +
                                //" and C.[DATAAREAID] = h.[DATAAREAID] " +
                                " left join [ax].[ACXPSRBeatMaster] APB on c.PSR_BEAT = APB.BeatCode and h.PSR_CODE = APB.PSRCode " +
                                " left join [ax].[ACXPSRMaster] psr on  h.PSR_CODE = psr.PSR_Code " +                          
                                " where loadsheet_Status = 0 and h.siteid = '" + sitecode1 + "' " +
                                " and  h.SO_DATE = '" + Convert.ToDateTime(txtSerch.Text).ToString("dd-MMM-yyyy") + "' " +
                                " order by h.[DELIVERY_DATE] asc,h.SO_DATE asc", conn);
                }
                else if (drpSearch.SelectedItem.Text == "Customer Group")
                {
                    adp1 = new SqlDataAdapter("Select h.SO_No,h.[DELIVERY_DATE],C.CUST_GROUP,h.Customer_Code" +
                                ",C.CUSTOMER_NAME,C.ADDRESS1,APB.BeatName ,C.PSR_BEAT,h.SO_DATE,SO_Value,psr.PSR_Name ,h.PSR_CODE,case when h.App_SO_NO!='' then 'MOBILE' else 'MANUAL' end as SOTYPE " +
                                " from [ax].[ACXSALESHEADER] h Inner Join " +
                                " [ax].[ACXCUSTMASTER] C on h.Customer_Code = C.Customer_Code" +
                               // " and C.[DATAAREAID] = h.[DATAAREAID] " +
                                " left join [ax].[ACXPSRBeatMaster] APB on c.PSR_BEAT = APB.BeatCode and h.PSR_CODE = APB.PSRCode " +
                                " left join [ax].[ACXPSRMaster] psr on  h.PSR_CODE = psr.PSR_Code " +                          
                                " where loadsheet_Status = 0 and h.siteid = '" + sitecode1 + "' " +
                                " and  c.CUST_GROUP like '%" + txtSerch.Text + "%' " +
                                " order by h.[DELIVERY_DATE] asc,h.SO_DATE asc", conn);
                }
                else
                {
                    adp1 = new SqlDataAdapter("Select h.SO_No,h.[DELIVERY_DATE],C.CUST_GROUP,h.Customer_Code" +
                                ",C.CUSTOMER_NAME,C.ADDRESS1,APB.BeatName ,C.PSR_BEAT,h.SO_DATE,SO_Value,psr.PSR_Name,h.PSR_CODE,case when h.App_SO_NO!='' then 'MOBILE' else 'MANUAL' end as SOTYPE " +
                                " from [ax].[ACXSALESHEADER] h Inner Join " +
                                " [ax].[ACXCUSTMASTER] C on h.Customer_Code = C.Customer_Code" +
                                //" and C.[DATAAREAID] = h.[DATAAREAID] " +
                                 " left join [ax].[ACXPSRBeatMaster] APB on c.PSR_BEAT = APB.BeatCode and h.PSR_CODE = APB.PSRCode " +
                                " left join [ax].[ACXPSRMaster] psr on  h.PSR_CODE = psr.PSR_Code " +                          
                                " where loadsheet_Status = 0  and h.siteid = '" + sitecode1 + "'" +
                                " and  h.PSR_CODE like '%" + txtSerch.Text + "%' " +
                                " order by h.[DELIVERY_DATE] asc,h.SO_DATE asc", conn);
                }
                ds2.Clear();
                adp1.Fill(ds2, "dtl");
                if (ds2.Tables["dtl"].Rows.Count != 0)
                {
                      gvDetails.DataSource = ds2 ;
                      gvDetails.DataBind();                       
                }

                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView2.Visible = false;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }        
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string lblPrGroup = e.Row.Cells[0].Text;
                TextBox txtQty = (TextBox)e.Row.FindControl("txtqty"); //total qty conv
                TextBox txtBoxQty = (TextBox)e.Row.FindControl("txtBox");//box
                TextBox txtPcsQty = (TextBox)e.Row.FindControl("txtPcs");//pcs qty

                TotalQtyConv += Convert.ToDecimal(txtQty.Text);  // Total Box with Pcs
                BoxQty += Convert.ToDecimal(txtBoxQty.Text);
                PcsQty += Convert.ToDecimal(txtPcsQty.Text);

                if (Convert.ToDecimal(txtQty.Text) > Convert.ToDecimal(e.Row.Cells[9].Text))
                {
                    e.Row.BackColor = System.Drawing.Color.Tomato;
                }

                Crates += Convert.ToDecimal(e.Row.Cells[3].Text);
                ltr += Convert.ToDecimal(e.Row.Cells[8].Text);

                #region Pcs Billing Applicability
                DataTable dt = baseObj.GetPcsBillingApplicability(Session["SCHSTATE"].ToString(), lblPrGroup);
                string ProductGroupApplicable = string.Empty;

                if (dt.Rows.Count > 0)
                {
                    ProductGroupApplicable = dt.Rows[0][1].ToString();
                }
                if (ProductGroupApplicable == "Y")
                {
                    txtPcsQty.Enabled = true;
                }
                else
                {
                    txtPcsQty.Enabled = false;
                }
                #endregion
            }
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    //e.Row.Cells[2].Text = "TOTAL :";
            //    //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            //    //e.Row.Cells[2].ForeColor = System.Drawing.Color.MidnightBlue;
            //    //e.Row.Cells[2].Font.Bold = true;

            //    e.Row.Cells[3].Text = "Total:"+ Crates.ToString();
            //    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            //    e.Row.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            //    e.Row.Cells[3].Font.Bold = true;

            //    e.Row.Cells[4].Text = "Total:" + BoxQty.ToString();
            //    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            //    e.Row.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            //    e.Row.Cells[4].Font.Bold = true;

            //    e.Row.Cells[5].Text = "Total:" + PcsQty.ToString();
            //    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            //    e.Row.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            //    e.Row.Cells[5].Font.Bold = true;

            //    e.Row.Cells[6].Text = "Total:" + TotalQtyConv.ToString();
            //    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            //    e.Row.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            //    e.Row.Cells[6].Font.Bold = true;

            //    e.Row.Cells[8].Text = "Total:" + ltr.ToString();
            //    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            //    e.Row.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            //    e.Row.Cells[8].Font.Bold = true;
               
            //}
        }
        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            string strSONO = string.Empty;
            DataTable dt = new DataTable();

            foreach (GridViewRow row in gvDetails.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkStatus") as CheckBox);
                    if (chkRow.Checked)
                    {
                        //strSONO += "'" + chkRow.Text + "',";                 //getting all So_No those are checked by User 
                        strSONO += "" + chkRow.Text + ",";
                    }
                }
            }
            //==============For Warehouse Loacion 11-5-16===========
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            string TransLocation = "";

            string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
            dt = new DataTable();
            dt = obj.GetData(query1);
            if (dt.Rows.Count > 0)
            {
                TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
            }
            //====================================
            if (strSONO != string.Empty)
            {
                strSONO = strSONO.Remove(strSONO.Length - 1);

                //==============11-5-16========
                //string strSaleLine = "Select AP.PRODUCT_GROUP,AL.PRODUCT_CODE,AP.PRODUCT_NAME,Sum(AL.CRATES) as CRATES,Sum(AL.BOX) AS  BOX,Sum(AL.LTR) AS LTR " +
                //                      ",StockBox=(Select coalesce(cast(sum(F.TransQty) as decimal(10,2)),0) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]=AL.SITEID and F.[ProductCode]=AL.Product_COde and F.[TransLocation]='" + TransLocation + "')" +
                //                      ",StockLtr =(Select coalesce(cast((sum(F.TransQty)*AP.Product_PackSize*AP.LTR)/1000 as decimal(10,2)),0) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]=AL.SITEID and F.[ProductCode]=AL.Product_COde and F.[TransLocation]='" + TransLocation + "')" +
                //                      " from [ax].[ACXSALESLINE] AL Inner Join [ax].[InventTable] AP on AL.PRODUCT_CODE = AP.ItemId " +
                //                      " where SO_No In (" + strSONO + ") and  AL.SiteId='" + Session["SiteCode"].ToString() + "' group by AL.SITEID,AP.PRODUCT_GROUP,AL.PRODUCT_CODE,AP.PRODUCT_NAME,AP.Product_PackSize,AP.LTR";

                string strSaleLine = "EXEC ACX_GETLOADSHEET '" + Session["SiteCode"].ToString() + "','" + TransLocation + "','" + strSONO + "'";

                dt = baseObj.GetData(strSaleLine);
                if (dt.Rows.Count > 0)
                {
                    GridView2.Visible = true;
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
                else
                {
                    dt = null;
                    GridView2.Visible = false;
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
            }
            else
            {
                dt = null;
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
            //UppnalegridDetails.Update();
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[12].Text == "MOBILE")
                {
                    e.Row.BackColor = System.Drawing.Color.LightCyan;
                }              
            }
        }

        protected void btnSavePrint_Click(object sender, EventArgs e)
        {

            if (Savegrid())
            {
                String strRetVal2 = "LoadSheet";
                //string message = "debugger;" +
                //                       " var printWindow = window.open('frmReport.aspx?LaodSheetNo=" + Session["LSNO"] + "&Type=" + strRetVal2 + "','_newtab')";
                string message = " var printWindow = window.open('frmReport.aspx?LaodSheetNo=" + Session["LSNO"] + "&Type=" + strRetVal2 + "','_newtab')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                Session["LSNO"] = null;
            }
        }

        protected void drpSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpSearch.SelectedItem.Text == "Date")
            {
                CalendarExtender1.Enabled = true;
                txtSerch.Text = "";
            }
            else
            {
                CalendarExtender1.Enabled = false;
            }
        }

        protected void txtPcs_TextChanged(object sender, EventArgs e)
        {
            txtQtyBox_TextChanged(sender, e);
        }
    }
}