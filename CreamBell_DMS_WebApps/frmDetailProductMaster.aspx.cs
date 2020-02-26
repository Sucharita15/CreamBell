using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmDetailProductMaster : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp3, adp2, adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();

        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string Number = Request.QueryString["Data"];
                if (Number != "")
                {
                    CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                    conn = obj.GetConnection();

                    adp1 = new SqlDataAdapter("SELECT * FROM ax.inventtable where ITEMID = '" + Number + "'", conn);
                    //adp1.SelectCommand.CommandTimeout = 0;
                    ds1.Clear();

                    adp1.Fill(ds1, "dtl");

                    if (ds1.Tables["dtl"].Rows.Count != 0)
                    {
                        for (int i = 0; i < ds1.Tables["dtl"].Rows.Count; i++)
                        {
                            txMaterialCode.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["ITEMID"].ToString());
                            txtProductName.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["PRODUCT_NAME"].ToString());
                            txtMaterialGroup.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["PRODUCT_GROUP"].ToString());
                            txtMaterialNickName.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["PRODUCT_NICKNAME"].ToString());
                            txtMaterialMRP.Text = (Math.Round(Convert.ToDecimal((ds1.Tables["dtl"].Rows[i]["PRODUCT_MRP"].ToString())), 2)).ToString();
                            txtMaterialPackSize.Text = (Math.Round(Convert.ToDecimal((ds1.Tables["dtl"].Rows[i]["PRODUCT_PACKSIZE"].ToString())), 2)).ToString();
                            //txtMaterialPackSize.Text = string.Copy(ds1.Tables["dtl"].Rows[i][9].ToString());
                            txtMaterialCratePackSize.Text = (Math.Round(Convert.ToDecimal((ds1.Tables["dtl"].Rows[i]["PRODUCT_CRATE_PACKSIZE"].ToString())), 2)).ToString();
                            //txtMaterialCratePackSize.Text = string.Copy(ds1.Tables["dtl"].Rows[i][10].ToString());
                            txtUOM.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["UOM"].ToString());
                            txtLTR.Text = (Math.Round(Convert.ToDecimal((ds1.Tables["dtl"].Rows[i]["LTR"].ToString())), 2)).ToString();
                            //txtLTR.Text = string.Copy(ds1.Tables["dtl"].Rows[i][12].ToString());
                            //txtGrossWt.Text = string.Copy(ds1.Tables["dtl"].Rows[i][13].ToString());
                            txtGrossWt.Text = (Math.Round(Convert.ToDecimal((ds1.Tables["dtl"].Rows[i]["GROSSWEIGHTINGM"].ToString())), 2)).ToString();
                            //txtNetWt.Text = string.Copy(ds1.Tables["dtl"].Rows[i][14].ToString());
                            txtNetWt.Text = (Math.Round(Convert.ToDecimal((ds1.Tables["dtl"].Rows[i]["NETWEIGHTPCS"].ToString())), 2)).ToString();
                            txtBarcodeNumber.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["BARCODE"].ToString());
                            DefaultWarehouse.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["WAREHOUSE"].ToString());
                            txtMaterialCategory.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["PRODUCT_GROUP"].ToString());
                            txtproductNature.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["PRODUCT_NATURE"].ToString());
                            txtMaterialSubCatoery.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["PRODUCT_SUBCATEGORY"].ToString());
                            txtFlavor.Text = string.Copy(ds1.Tables["dtl"].Rows[i]["FLAVOUR"].ToString());

                            decimal volume, vol, vol1, vol2;

                            vol1 = (Convert.ToDecimal((ds1.Tables["dtl"].Rows[i]["PRODUCT_PACKSIZE"].ToString())));

                            vol2 = (Convert.ToDecimal((ds1.Tables["dtl"].Rows[i]["LTR"].ToString())));

                            vol = vol1 * vol2 / 1000;

                            volume = (Math.Round(vol, 2));

                            //vol = ((Math.Round(Convert.ToDecimal((ds1.Tables["dtl"].Rows[i][9].ToString())), 2)) * (Math.Round(Convert.ToDecimal((ds1.Tables["dtl"].Rows[i][12].ToString())), 2)))/1000;

                            txtVolume.Text = volume.ToString();
                        }

                    }
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/frmProductMaster.aspx");
        }
    }
}