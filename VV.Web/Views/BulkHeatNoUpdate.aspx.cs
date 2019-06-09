using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using VV.Web.Models;

namespace VV.Web.Views
{
    public partial class BulkHeatNoUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var userDto = UsersDTO.Instance;

                Label lblUserName = (Label)this.Master.FindControl("lblUserName");

                lblUserName.Text = userDto.UserName;
            }
        }

        protected void Submit(object sender, EventArgs e)
        {
            ErrorMessage.Visible = false;

            try
            {
                string ProductionOrderNo_Search = Convert.ToString(ProdOrderNumber.Text.Trim());

                DBUtil _dbObj = new DBUtil();

                DataSet ds = _dbObj.GetNonICSOrdersDetails(ProductionOrderNo_Search);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var OrderNo = ds.Tables[0].Rows[0]["OrderNo"].ToString();
                    var Pos = ds.Tables[0].Rows[0]["Pos"].ToString();
                }

                GridView1.DataSource = ds;
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from fetch Bulk update body and bonnet heat no.");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ErrorMessage.Visible = false;

            DBUtil _DBObj = new DBUtil();
            DataSet ds;

            string BodyHeatNo = Convert.ToString(txtBodyHeatNo.Text.Trim());
            string BonnetHeatNo = Convert.ToString(txtBonnetHeatNo.Text.Trim());

            try
            {
                if (BodyHeatNo.ToString() != string.Empty || BonnetHeatNo.ToString() != string.Empty)
                {
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        ds = new DataSet();

                        bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;
                        if (isChecked)
                        {
                            string ProductionOrderNo = Convert.ToString(ProdOrderNumber.Text.Trim());
                            String SerialNo1 = ((System.Web.UI.HtmlControls.HtmlInputHidden)row.FindControl("hiddenSerialNo")).Value.ToString();
                            string SerialNo = ((TextBox)row.FindControl("txtSerialNo")).Text.ToString().Trim();

                            DBUtil _dbObj = new DBUtil();

                            _dbObj.UpdateBulkUpdateBodyBonnetHeatNo(ProductionOrderNo, SerialNo, BodyHeatNo, BonnetHeatNo);

                            ErrorMessage.Visible = true;
                            FailureText.Text = "<span style='color:green'>Record Updated Successfully.<span>";

                            txtBodyHeatNo.Text = "";
                            txtBonnetHeatNo.Text = "";

                            DataSet dataSet = _dbObj.GetNonICSOrdersDetails(ProductionOrderNo);

                            GridView1.DataSource = dataSet;
                            GridView1.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from get Bulk Update Body and Bonnet Heat No from serial no.");
            }
        }

        private void LogError(Exception ex, string section)
        {

            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += "Exception from SQLConnectionOpen" + "-" + section;
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string path = Server.MapPath("~/ErrorLog.txt");
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }
}