using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmExcelUploadAdj : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblEror1.Text = "";
            lblError.Text = "";
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
               //btnExcelUpload.Attributes.Add("onclick", "javascript: if (Page_ClientValidate() ){" + btnExcelUpload.ClientID + ".disabled=true;}" + ClientScript.GetPostBackEventReference(btnExcelUpload, ""));
            }
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

        protected void btnExcelUpload_Click(object sender, EventArgs e)
        {
            //btnExcelUpload.Attributes.Add("onclick", "javascript: if (Page_ClientValidate() ){" + btnExcelUpload.ClientID + ".disabled=true;}" + ClientScript.GetPostBackEventReference(btnExcelUpload, ""));
            SqlTransaction tran = null;
            string query = string.Empty;
            SqlConnection conn = baseObj.GetConnection();
            SqlCommand cmd = new SqlCommand("USP_EXCELOPENINGSTOCKUPLOAD");
            tran = conn.BeginTransaction();
            cmd.Connection = conn;
            cmd.Transaction = tran;
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            if (conn==null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validate", "alert('Connection lost. Please check connection.');", true);
                return;
            }
            try
            {
                int no = 0 ;
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
                    try {
                        dtExcelData = CreamBell_DMS_WebApps.App_Code.ExcelUpload.ImportExcelXLS(Server.MapPath("~/Uploads/" + fileName), true);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Execl Upload", "alert('" + ex.Message.ToString().Replace("'","''") + "');", true);
                        lblError.Text = ex.Message;
                        lblError.ForeColor = System.Drawing.Color.Red;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        return;
                    }

                    DataSet lds = new DataSet();
                    DataTable dt = new DataTable();
                    //lds.Tables.Add(dtForShownUnuploadData);
                    dtExcelData.TableName = "OpeningStockUpload";
                    lds.Tables.Add(dtExcelData);
                    RemoveTimezoneForDataSet(lds);
                    string ls_xml = lds.GetXml();
                    SqlParameter pvNewId = new SqlParameter();
                    pvNewId.ParameterName = "@SuccessCount";
                    pvNewId.DbType = DbType.Int32;
                    pvNewId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pvNewId);
                    cmd.Parameters.AddWithValue("@XmlData", ls_xml);
                    cmd.Parameters.AddWithValue("@DataAreaId", Session["DATAAREAID"].ToString());
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        gridviewRecordNotExist.DataSource = dt;
                        gridviewRecordNotExist.DataBind();
                        ModalPopupExtender1.Show();
                    }
                    lblUpload.Text = cmd.Parameters["@SuccessCount"].Value.ToString() + " Records Uploaded Successfully...";
                    lblUpload.ForeColor = System.Drawing.Color.Green;
                    tran.Commit();
                    //DataTable dtForShownUnuploadData = new DataTable();
                    //dtForShownUnuploadData.Columns.Add("Site Code");
                    //dtForShownUnuploadData.Columns.Add("Product Code");
                    //dtForShownUnuploadData.Columns.Add("Box Qty");
                    //dtForShownUnuploadData.Columns.Add("Crate Qty");
                    //dtForShownUnuploadData.Columns.Add("Rejected Reason");


                    //string DATAAREAID = Session["DATAAREAID"].ToString();
                    //int TransType = 7;//1 for Adj 
                    //int DocumentType = 7;

                    //string DocumentNo = "";
                    //string Referencedocumentno = "";
                    //string TransLocation = "";

                    //    tran = conn.BeginTransaction();
                    //    cmd.Transaction = tran;
                    //    cmd.Connection = conn;
                    //    int j = 0;
                    //    for (int k = 0; k < dtExcelData.Rows.Count; k++)
                    //    {
                    //        if (dtExcelData.Rows[k]["Site Code"].ToString() == "")
                    //        {
                    //            break;
                    //        }


                    //        //If Same Site , Same Transaction Type for any Item exists, System will not process the Excel Upload activity.
                    //        string sqlstr = "	select top 1 SITECODE from [ax].[ACXINVENTTRANS] where SITECODE = '" + dtExcelData.Rows[k]["Site Code"].ToString().TrimEnd() + "' "
                    //                       + " and TRANSTYPE = '7' and ProductCode ='" + dtExcelData.Rows[k]["Product Code"].ToString() + "' ";
                    //        object objcheck = baseObj.GetScalarValue(sqlstr);


                    //        if (objcheck != null)
                    //        {
                    //            lblEror1.Text = "Record Already Exist,System will not process the Excel Upload activity";
                    //            lblEror1.ForeColor = System.Drawing.Color.Red;
                    //            lblError.Visible = false;
                    //            dtForShownUnuploadData.Rows.Add();
                    //            dtForShownUnuploadData.Rows[j]["Site Code"] = dtExcelData.Rows[k]["Site Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Product Code"] = dtExcelData.Rows[k]["Product Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Box Qty"] = dtExcelData.Rows[k]["Box Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Crate Qty"] = dtExcelData.Rows[k]["Crate Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Rejected Reason"] = "Record Already Exist..";
                    //            j += 1;
                    //        }

                    //        //check for SiteCode 
                    //        sqlstr = "select top 1 SITEID from ax.InventSite where SITEID = '" + dtExcelData.Rows[k]["Site Code"].ToString().TrimEnd() + "' ";
                    //        objcheck = baseObj.GetScalarValue(sqlstr);
                    //        if (objcheck == null)
                    //        {
                    //            dtForShownUnuploadData.Rows.Add();
                    //            dtForShownUnuploadData.Rows[j]["Site Code"] = dtExcelData.Rows[k]["Site Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Product Code"] = dtExcelData.Rows[k]["Product Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Box Qty"] = dtExcelData.Rows[k]["Box Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Crate Qty"] = dtExcelData.Rows[k]["Crate Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Rejected Reason"] = "Site Id Not Exist..";
                    //            j += 1;
                    //            continue;
                    //        }
                    //        //check for Transactionlocation id

                    //        string query1 = "select MainWarehouse from ax.inventsite where siteid='" + dtExcelData.Rows[k]["Site Code"].ToString() + "'";
                    //        object objTranslocation = baseObj.GetScalarValue(query1);

                    //        if (objTranslocation != null)
                    //        {
                    //            TransLocation = objTranslocation.ToString();
                    //        }
                    //        else
                    //        {
                    //            dtForShownUnuploadData.Rows.Add();
                    //            dtForShownUnuploadData.Rows[j]["Site Code"] = dtExcelData.Rows[k]["Site Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Product Code"] = dtExcelData.Rows[k]["Product Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Box Qty"] = dtExcelData.Rows[k]["Box Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Crate Qty"] = dtExcelData.Rows[k]["Crate Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Rejected Reason"] = "TransLocation ID not Found";
                    //            j += 1;
                    //            continue;
                    //        }

                    //        //check for Product Code 
                    //        sqlstr = "select top 1 ItemID from ax.inventTable where ItemID = '" + dtExcelData.Rows[k]["Product Code"].ToString() + "'";
                    //        object objcheckproductcode = baseObj.GetScalarValue(sqlstr);
                    //        if (objcheckproductcode == null)
                    //        {
                    //            dtForShownUnuploadData.Rows.Add();
                    //            dtForShownUnuploadData.Rows[j]["Site Code"] = dtExcelData.Rows[k]["Site Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Product Code"] = dtExcelData.Rows[k]["Product Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Box Qty"] = dtExcelData.Rows[k]["Box Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Crate Qty"] = dtExcelData.Rows[k]["Crate Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Rejected Reason"] = "Product Code Not Exist..";
                    //            j += 1;
                    //            continue;
                    //        }

                    //        //check for Product Code is Blocked
                    //        sqlstr = "select top 1 ItemID from ax.inventTable where  BLOCK=0 and ItemID = '" + dtExcelData.Rows[k]["Product Code"].ToString() + "'";
                    //        object objcheckblockproductcode = baseObj.GetScalarValue(sqlstr);
                    //        if (objcheckblockproductcode == null)
                    //        {
                    //            dtForShownUnuploadData.Rows.Add();
                    //            dtForShownUnuploadData.Rows[j]["Site Code"] = dtExcelData.Rows[k]["Site Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Product Code"] = dtExcelData.Rows[k]["Product Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Box Qty"] = dtExcelData.Rows[k]["Box Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Crate Qty"] = dtExcelData.Rows[k]["Crate Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Rejected Reason"] = "Product Code Is Blocked..";
                    //            j += 1;
                    //            continue;
                    //        }

                    //        sqlstr = "select top 1 ITEMID from ax.inventTable where BU_CODE in (select BU_CODE from ax.acxsitebumapping where SITEID='" + dtExcelData.Rows[k]["Site Code"].ToString() + "') and ITEMID = '" + dtExcelData.Rows[k]["Product Code"].ToString() + "' and BLOCK=0 ";
                    //        object objcheckBUCode = baseObj.GetScalarValue(sqlstr);

                    //        if (objcheckBUCode == null)
                    //        {
                    //            dtForShownUnuploadData.Rows.Add();
                    //            dtForShownUnuploadData.Rows[j]["Site Code"] = dtExcelData.Rows[k]["Site Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Product Code"] = dtExcelData.Rows[k]["Product Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Box Qty"] = dtExcelData.Rows[k]["Box Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Crate Qty"] = dtExcelData.Rows[k]["Crate Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Rejected Reason"] = "Product Code Does Not Belong To BU..";
                    //            j += 1;
                    //            continue;
                    //        }

                    //        string Siteid = dtExcelData.Rows[k]["Site Code"].ToString().TrimEnd();
                    //        string TransId = string.Empty;
                    //        if (Siteid.Length >= 6)
                    //        {
                    //             TransId = Siteid.Substring(Siteid.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                    //        }
                    //        else
                    //        {
                    //             TransId = Siteid + System.DateTime.Now.ToString("yymmddhhmmss");
                    //        }



                    //        string Product = dtExcelData.Rows[k]["Product Code"].ToString().TrimEnd();
                    //        string Box = dtExcelData.Rows[k]["Box Qty"].ToString().TrimEnd();
                    //        string Crate = dtExcelData.Rows[k]["Crate Qty"].ToString().TrimEnd();
                    //        decimal PackSize;
                    //        PackSize = Convert.ToDecimal(baseObj.GetScalarValue("SELECT ISNULL(i.PRODUCT_CRATE_PACKSIZE,1) FROM ax.INVENTTABLE i WHERE i.ITEMID='" + dtExcelData.Rows[k]["Product Code"].ToString().TrimEnd() + "'"));
                    //        decimal TransQty = Convert.ToDecimal(Box) + (Convert.ToDecimal(Crate) * PackSize);
                    //        int REcid = k + 1;

                    //        if (TransQty > 0)
                    //        {
                    //            cmd.CommandText = "USP_OPENSTOCKTOINVENTTRANS";
                    //            cmd.CommandType = CommandType.StoredProcedure;
                    //            cmd.CommandTimeout = 100;

                    //            cmd.Parameters.Clear();
                    //            cmd.Parameters.AddWithValue("@TransId", TransId.ToString());
                    //            cmd.Parameters.AddWithValue("@Siteid", Siteid.ToString());
                    //            cmd.Parameters.AddWithValue("@Dataareaid", DATAAREAID.ToString());
                    //            cmd.Parameters.AddWithValue("@TransType", TransType.ToString());
                    //            cmd.Parameters.AddWithValue("@DocumentType", DocumentType.ToString());
                    //            cmd.Parameters.AddWithValue("@DocumentNo", DocumentNo.ToString());
                    //            cmd.Parameters.AddWithValue("@DocumentDate", System.DateTime.Now.ToString());
                    //            cmd.Parameters.AddWithValue("@ProductCode", Product.ToString());
                    //            cmd.Parameters.AddWithValue("@TransQty", TransQty.ToString());
                    //            cmd.Parameters.AddWithValue("@TransUOM", "BOX");
                    //            cmd.Parameters.AddWithValue("@TransLocation", TransLocation.ToString());
                    //            cmd.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno.ToString());
                    //            cmd.Parameters.AddWithValue("@TransLineNo", k.ToString());
                    //            cmd.ExecuteNonQuery();
                    //            no += 1;

                    //        //string Tquery = " Insert Into ax.AcxinventTrans " +
                    //        //            "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                    //        //            "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno],TransLineNo)" +
                    //        //            " Values ('" + TransId + "','" + Siteid + "','" + DATAAREAID + "',(select coalesce(max(Recid),0)+1 as Recid from  ax.acxinventTrans),getdate()," + TransType + "," + DocumentType + ",'" + DocumentNo + "'," +
                    //        //            " '" + System.DateTime.Now + "','" + Product + "'," + TransQty + ",'BOX','" + TransLocation + "','" + Referencedocumentno + "'," + k + ")";
                    //        //    //obj.ExecuteCommand(query);
                    //        //    cmd.CommandText = Tquery;
                    //        //    cmd.ExecuteNonQuery();

                    //        }
                    //        else
                    //        {
                    //            dtForShownUnuploadData.Rows.Add();
                    //            dtForShownUnuploadData.Rows[j]["Site Code"] = dtExcelData.Rows[k]["Site Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Product Code"] = dtExcelData.Rows[k]["Product Code"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Box Qty"] = dtExcelData.Rows[k]["Box Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Crate Qty"] = dtExcelData.Rows[k]["Crate Qty"].ToString();
                    //            dtForShownUnuploadData.Rows[j]["Rejected Reason"] = "Qty always greater than zero..";
                    //            j += 1;
                    //            continue;
                    //        }


                    //    }

                    //    tran.Commit();
                    //    lblError.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : " + no + " Records.";
                    //    lblError.ForeColor = System.Drawing.Color.Green;
                    //    if (dtForShownUnuploadData.Rows.Count > 0)
                    //    {
                    //        gridviewRecordNotExist.DataSource = dtForShownUnuploadData;
                    //        gridviewRecordNotExist.DataBind();
                    //        ModalPopupExtender1.Show();
                    //    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                tran.Rollback();
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
    }
}