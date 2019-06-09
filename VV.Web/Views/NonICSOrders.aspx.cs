using Libraries.Entity;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VV.Web.Models;

namespace VV.Web.Views
{
    public partial class NonICSOrders : System.Web.UI.Page
    {
        public object ProdOrderNo { get; private set; }

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
                    txtSalesOrderNo.Text = OrderNo + " - " + Pos;
                    txtCustomerName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                    txtFigureNumber.Text = ds.Tables[0].Rows[0]["Item"].ToString();
                }

                GridView1.DataSource = ds;
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from get ICSOrders from serial no.");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        string ProductionOrderNo = Convert.ToString(ProdOrderNumber.Text.Trim());
                        string SerialNo = ((TextBox)row.FindControl("txtSerialNo")).Text.ToString().Trim();
                        string BodyHeatNo = ((TextBox)row.FindControl("txtBodyHeatNo")).Text.ToString().Trim();
                        string BodyRTNo = ((TextBox)row.FindControl("txtBodyRTNo")).Text.ToString().Trim();
                        string BonnetHeatNo = ((TextBox)row.FindControl("txtBonnetHeatNo")).Text.ToString().Trim();
                        string BonnetRTNo = ((TextBox)row.FindControl("txtBonnetRTNo")).Text.ToString().Trim();
                        string WedgeHeatNo = ((TextBox)row.FindControl("txtWedgeHeatNo")).Text.ToString().Trim();
                        string WedgeRTNo = ((TextBox)row.FindControl("txtWedgeRTNo")).Text.ToString().Trim();
                        string PipeHeatNo = ((TextBox)row.FindControl("txtPipeHeatNo")).Text.ToString().Trim();
                        string PipeRTNo = ((TextBox)row.FindControl("txtPipeRTNo")).Text.ToString().Trim();
                        string Bonnet2HeatNo = ((TextBox)row.FindControl("txtBonnet2HeatNo")).Text.ToString().Trim();
                        string Bonnet2RTNo = ((TextBox)row.FindControl("txtBonnet2RTNo")).Text.ToString().Trim();
                        string ActuatorSerialNo = ((TextBox)row.FindControl("txtActuatorSerialNo")).Text.ToString().Trim();

                        DBUtil _dbObj = new DBUtil();

                        _dbObj.UpdateNonICSOrdersDetails(ProductionOrderNo, SerialNo, BodyHeatNo, BodyRTNo, BonnetHeatNo, BonnetRTNo, WedgeHeatNo, WedgeRTNo, PipeHeatNo, PipeRTNo, Bonnet2HeatNo, Bonnet2RTNo, ActuatorSerialNo);

                        ErrorMessage.Visible = true;
                        FailureText.Text = "<span style='color:green'>Record Updated Successfully.<span>";
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from update NonICSOrders screen");
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