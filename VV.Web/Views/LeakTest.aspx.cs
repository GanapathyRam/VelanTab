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
    public partial class LeakTest : System.Web.UI.Page
    {
        DBUtil _DBObj = new DBUtil();
        string selectedSerialNoList = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime currentDate = DateTime.UtcNow;
                txtPatrolDate.Text = Convert.ToString(currentDate.ToString("MM/dd/yyyy"));

                var userDto = UsersDTO.Instance;

                Label lblUserName = (Label)this.Master.FindControl("lblUserName");

                lblUserName.Text = userDto.UserName;

                FillSubLocationMaster();

                FillOperatorMaster();

                btnSubmit.Visible = false;

                GridViewPopUp.DataSource = new DataTable();
                GridViewPopUp.DataBind();
            }
        }


        #region Private Method

        private string GetLeakTestNumber()
        {
            var currentYear = string.Empty;

            var model = _DBObj.GetLeakTestNumber();

            currentYear = Helper.CurrentFiniancialYear();

            if (!string.IsNullOrEmpty(model))
            {
                var savedYear = Convert.ToString(model.ToString().Substring(1, 2));

                if (!savedYear.Equals(currentYear))
                {
                    currentYear = Convert.ToString("L" + currentYear + "0001");
                }
                else
                {
                    var currentValue = model.ToString().Substring(3);

                    //string input = "001";
                    string output = (int.Parse(currentValue) + 1).ToString().PadLeft(currentValue.Length, '0');

                    //var workOrderInc = Int32.Parse(model.ToString().Substring(3)) + 1;
                    currentYear = Convert.ToString("L" + currentYear + output);
                }
            }
            else
            {
                currentYear = Convert.ToString("L" + currentYear + "0001");
            }

            return currentYear;
        }

        private DataSet FillSubLocationMaster()
        {

            DBUtil _DBObj = new DBUtil();
            DataSet ds = _DBObj.GetSubLocationMaster();

            ddlType.DataSource = ds;
            ddlType.DataBind();

            ddlType.Items.Insert(0, "----Please Select----");

            return ds;
        }

        private DataSet FillOperatorMaster()
        {

            DBUtil _DBObj = new DBUtil();
            DataSet ds = _DBObj.GetOperatorMasterDetails();

            ddlOperator.DataSource = ds;
            ddlOperator.DataBind();

            ddlOperator.Items.Insert(0, "----Please Select----");

            return ds;
        }

        private void GetLeakTestMasterDetails()
        {
            //DataSet ds = _DBObj.GetCheckListMasterDetails(ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

            //gridPatrolInspection.DataSource = ds;
            //gridPatrolInspection.DataBind();
        }

        private void GetLeakTestDetails(String LeakTestNumber)
        {
            DataSet ds = _DBObj.GetLeakTestDetails(LeakTestNumber);

            gridLeakTest.DataSource = ds;
            gridLeakTest.DataBind();
        }

        private void SetInitialRow(object SerialNoList, int From)
        {
            DataTable dt = new DataTable();
            List<string> serialNoList = new List<string>();

            DataRow dr = null;

            if (From == 0 && SerialNoList != null)
            {
                 serialNoList = SerialNoList.ToString().Split(',').Where(s => !string.IsNullOrEmpty(s)).ToList();
            }
            else
            {
                serialNoList = new List<string>();
                serialNoList.Add("");
            }

            dt.Columns.Add(new DataColumn("SerialNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ShellLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("HighPressureSeatLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("LowPressureSeatLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("BackSeatLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("WeldLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("JointLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("TrapShellLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("TrapWeldLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("TrapJointLeak", typeof(int)));
            dt.Columns.Add(new DataColumn("TrapShutLeak", typeof(int)));

            foreach (var serialNo in serialNoList)
            {
                dr = dt.NewRow();

                dr["SerialNo"] = serialNo;
                dr["ShellLeak"] = 0;
                dr["HighPressureSeatLeak"] = 0;
                dr["LowPressureSeatLeak"] = 0;
                dr["BackSeatLeak"] = 0;
                dr["WeldLeak"] = 0;
                dr["JointLeak"] = 0;
                dr["TrapShellLeak"] = 0;
                dr["TrapWeldLeak"] = 0;
                dr["TrapJointLeak"] = 0;
                dr["TrapShutLeak"] = 0;

                dt.Rows.Add(dr);
            }

            //dr = dt.NewRow();

            //Store the DataTable in ViewState

            ViewState["CurrentTable"] = dt;

            gridLeakTest.DataSource = dt;

            gridLeakTest.DataBind();

            btnUpdate.Visible = true;

        }

        private void Clear()
        {
            DataTable dt = new DataTable();

            txtLeakTestNo.Text = "";
            txtProdOrderNo.Text = "";
            txtPatrolDate.Text = "";
            ddlType.SelectedIndex = 0;

            txtItemNumber.Text = "";
            txtDescription.Text = "";
            txtCustomer.Text = "";
            txtSaleOrder.Text = "";
            txtQtyTested.Text = "";
            ddlOperator.SelectedIndex = 0;
            txtRemarks.Text = "";

            gridLeakTest.DataSource = dt;
            gridLeakTest.DataBind();

            GridViewPopUp.DataSource = dt;
            GridViewPopUp.DataBind();

            ddlType.Enabled = true;
            txtProdOrderNo.Enabled = true;
            txtItemNumber.Enabled = true;
            txtDescription.Enabled = true;
            txtCustomer.Enabled = true;
            txtSaleOrder.Enabled = true;
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

        #endregion

        protected void txtProdOrderNo_TextChanged(object sender, EventArgs e)
        {
            var LeakTestNumber = string.Empty;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            int PatrolQtyCount = 0; int ReleaseQtyCount = 0;

            lblMessage.Text = "";

            // Empty Row Setting Up Main Grid
            gridLeakTest.DataSource = dt;
            gridLeakTest.DataBind();
            btnUpdate.Visible = false;

            var ProdOrderNo = txtProdOrderNo.Text.Trim();

            if (string.IsNullOrEmpty(ProdOrderNo))
            {
                GridViewPopUp.DataSource = dt;
                GridViewPopUp.DataBind();

                return;
            }

            if (ddlType.SelectedIndex != 0)
            {
                LeakTestNumber = GetLeakTestNumber();

                if (!string.IsNullOrEmpty(LeakTestNumber))
                {
                    lblMessage.Text = "";
                    txtLeakTestNo.Text = LeakTestNumber;
                }
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Invalid Type Selection";

                txtLeakTestNo.Text = "";
                txtProdOrderNo.Text = "";

                return;
            }

            var OrderExist = ProdOrderNo.Substring(0, 1);

            if (OrderExist == "4")
            {
                DataSet ds1 = new DataSet();

                ds1 = _DBObj.GetProductionReleaseNewForLeakTestWithSerial(ProdOrderNo, ddlType.SelectedValue);

                if (ds1 != null && ds1.Tables[0].Rows.Count == 0)
                {
                    ds1 = _DBObj.GetProductionReleaseNewForLeakTestWithOutSerial(ProdOrderNo, ddlType.SelectedValue);

                    if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["PatrolQty"].ToString() != string.Empty)
                        {
                            PatrolQtyCount = Convert.ToInt32(ds1.Tables[0].Rows[0]["PatrolQty"].ToString());
                        }

                        if (ds1.Tables[0].Rows[0]["ProdReleaseqty"].ToString() != string.Empty)
                        {
                            ReleaseQtyCount = Convert.ToInt32(ds1.Tables[0].Rows[0]["ProdReleaseqty"].ToString());
                        }

                        var globalPatrolQty = Convert.ToInt16(ReleaseQtyCount - PatrolQtyCount);

                        ViewState["CurrentPatrolQtyCount"] = globalPatrolQty;

                        txtQtyTested.Enabled = true;

                        txtQtyTested.Text = "";

                        GridViewPopUp.DataSource = dt;
                        GridViewPopUp.DataBind();

                        //btnGenerate.Enabled = true;

                        //if (globalPatrolQty > Convert.ToInt32(txtPatrolQty.Text.Trim()))
                        //{
                        //    //// Need to show the error message
                        //}
                    }
                    else
                    {
                        GridViewPopUp.DataSource = dt;
                        GridViewPopUp.DataBind();
                    }
                }
                else
                {
                    txtQtyTested.Enabled = false;

                    Cache["CacheForPatrolSerialNoList"] = ds1;

                    ViewState["CurrentPatrolQtyCount"] = 0;

                    GridViewPopUp.DataSource = ds1;
                    GridViewPopUp.DataBind();

                    btnSubmit.Visible = true;

                    //btnGenerate.Enabled = false;
                }

                ds = _DBObj.GetMISOrderStatusForPatrol(txtProdOrderNo.Text.Trim());

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    txtItemNumber.Text = Convert.ToString(ds.Tables[0].Rows[0]["Item"]);
                    txtDescription.Text = Convert.ToString(ds.Tables[0].Rows[0]["Description"]);
                    txtCustomer.Text = Convert.ToString(ds.Tables[0].Rows[0]["CustomerName"]);
                    txtSaleOrder.Text = Convert.ToString(ds.Tables[0].Rows[0]["OrderNo"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["Pos"]);
                }
                else
                {
                    txtItemNumber.Text = "";
                    txtDescription.Text = "";
                    txtCustomer.Text = "";
                    txtSaleOrder.Text = "";
                }

                txtLeakTestNo.Enabled = false;
                btnDelete.Enabled = false;
            }

            // Prod Order Not Starting From '4'
            if (OrderExist != "4")
            {
                DataSet ds2 = new DataSet();
                btnSubmit.Visible = false;

                ds2 = _DBObj.GetProductionReleaseNewForLeakTest(ProdOrderNo, ddlType.SelectedValue);

                if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                {
                    txtQtyTested.Text = "";

                    txtQtyTested.Enabled = true;

                    //btnGenerate.Enabled = true;

                    if (ds2.Tables[0].Rows[0]["PatrolQty"].ToString() != string.Empty)
                    {
                        PatrolQtyCount = Convert.ToInt32(ds2.Tables[0].Rows[0]["PatrolQty"].ToString());
                    }

                    if (ds2.Tables[0].Rows[0]["ProdReleaseqty"].ToString() != string.Empty)
                    {
                        ReleaseQtyCount = Convert.ToInt32(ds2.Tables[0].Rows[0]["ProdReleaseqty"].ToString());
                    }

                    var globalPatrolQty = Convert.ToInt16(ReleaseQtyCount - PatrolQtyCount);

                    ViewState["CurrentPatrolQtyCount"] = globalPatrolQty;

                    GridViewPopUp.DataSource = dt;
                    GridViewPopUp.DataBind();
                }
                else
                {
                    txtQtyTested.Text = "";

                    txtQtyTested.Enabled = true;

                    ViewState["CurrentPatrolQtyCount"] = 0;

                    GridViewPopUp.DataSource = dt;
                    GridViewPopUp.DataBind();

                    btnSubmit.Visible = false;
                }

                ds = _DBObj.GetWIPAllFromProdOrderNo(ProdOrderNo);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    txtItemNumber.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemNumber"]);
                    txtDescription.Text = Convert.ToString(ds.Tables[0].Rows[0]["Description"]);
                    txtCustomer.Text = "";
                    txtSaleOrder.Text = "";
                }
                else
                {
                    txtItemNumber.Text = "";
                    txtDescription.Text = "";
                    txtCustomer.Text = "";
                    txtSaleOrder.Text = "";
                }

                txtLeakTestNo.Enabled = false;
                btnDelete.Enabled = false;  //todo
            }
        }

        protected void GridViewPopUp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = (DataSet)Cache["CacheForPatrolSerialNoList"];

            GridViewPopUp.PageIndex = e.NewPageIndex;
            GridViewPopUp.DataSource = ds;
            GridViewPopUp.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            int Count = 0;
            foreach (GridViewRow row in GridViewPopUp.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;

                    if (isChecked)
                    {
                        string serialNo = (row.Cells[0].FindControl("lblCheckListSerial") as Label).Text;

                        selectedSerialNoList = selectedSerialNoList + serialNo + ",";

                        Count = Count + 1;
                    }
                }
            }

            ViewState["selectedSerialNoList"] = selectedSerialNoList.ToString();

            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["selectedSerialNoList"])) && Count > 0)
            {
                ViewState["selectedSerialNoCount"] = Convert.ToString(Count);
            }
        }

        protected void btnPopDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                int Count = 0;

                foreach (GridViewRow row in GridViewPopUp.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;

                        if (isChecked)
                        {
                            string serialNo = (row.Cells[0].FindControl("lblCheckListSerial") as Label).Text;

                            selectedSerialNoList = selectedSerialNoList + serialNo + ",";

                            Count = Count + 1;
                        }
                    }
                }

                var updatedCount = Convert.ToUInt32(GridViewPopUp.Rows.Count - Count);

                ViewState["selectedSerialNoList"] = selectedSerialNoList.ToString();

                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["selectedSerialNoList"])) && updatedCount >= 0)
                {
                    ViewState["selectedSerialNoCount"] = Convert.ToString(updatedCount);

                    // Patrol Serial
                    if (selectedSerialNoList != null)
                    {
                        var split = selectedSerialNoList.ToString().Split(',');

                        for (int i = 0; i < split.Length; i++)
                        {
                            if (split[i].Trim() != string.Empty)
                            {
                                _DBObj.DeleteLeakTestSerials(txtLeakTestNo.Text, split[i].Trim(), 0, 0);
                            }
                        }

                        // Updating the Patrol Qty Count From Patrol Inspection Master
                        _DBObj.DeleteLeakTestSerials(txtLeakTestNo.Text, "", Convert.ToInt32(updatedCount), 1);

                        // Get Serial No Grid View Details
                        DataSet ds2 = _DBObj.GetLeakTestSerial(txtLeakTestNo.Text.Trim());
                        GridViewPopUp.DataSource = ds2;
                        GridViewPopUp.DataBind();

                        btnUpdate.Visible = true; // todo

                        ViewState["DeletedSerialNoList"] = Convert.ToString(updatedCount);

                        //SetInitialRow(selectedSerialNoList, 0); // 0 means Prod Order Starting with '4' digits

                        //lblMessage.Visible = true;
                        //lblMessage.ForeColor = System.Drawing.Color.Green;
                        //lblMessage.Text = "Selected serial no has been removed successfully.";
                    }
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Select Serial no's to delete";

                    return;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from deleting leak test details.");
            }

            //txtLeakTestNo_TextChanged(sender, e);

            //// Get Serial No Grid View Details
            //_DBObj.GetLeakTestSerial(txtLeakTestNo.Text.Trim());
        }

        protected void gridLeakTest_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gridLeakTest_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gridLeakTest_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gridLeakTest_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gridLeakTest_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridLeakTest_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gridLeakTest_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            bool IsLeakTestNumberExist = false;

            var selectedDate = DateTime.ParseExact(txtPatrolDate.Text, "MM/dd/yyyy", null);

            try
            {
                IsLeakTestNumberExist = _DBObj.IsLeakTestNumberExist(txtLeakTestNo.Text.Trim());

                if (!IsLeakTestNumberExist)
                {
                    foreach (GridViewRow row in this.gridLeakTest.Rows)
                    {
                        string lblSerialNo = ((Label)row.FindControl("lblSerialNo")).Text.ToString();
                       
                        string txtObservation = ((TextBox)row.FindControl("txtShellLeak")).Text.ToString();
                        string txtHighPressureSeatLeak = ((TextBox)row.FindControl("txtHighPressureSeatLeak")).Text.ToString();
                        string txtLowPressureSeatLeak = ((TextBox)row.FindControl("txtLowPressureSeatLeak")).Text.ToString();
                        string txtBackSeatLeak = ((TextBox)row.FindControl("txtBackSeatLeak")).Text.ToString();
                        string txtWeldLeak = ((TextBox)row.FindControl("txtWeldLeak")).Text.ToString();
                        string txtJointLeak = ((TextBox)row.FindControl("txtJointLeak")).Text.ToString();
                        string txtTrapShellLeak = ((TextBox)row.FindControl("txtTrapShellLeak")).Text.ToString();
                        string txtTrapWeldLeak = ((TextBox)row.FindControl("txtTrapWeldLeak")).Text.ToString();
                        string txtTrapJointLeak = ((TextBox)row.FindControl("txtTrapJointLeak")).Text.ToString();
                        string txtTrapShutLeak = ((TextBox)row.FindControl("txtTrapShutLeak")).Text.ToString();


                        if (!string.IsNullOrEmpty(txtLeakTestNo.Text.Trim()) && !string.IsNullOrEmpty(txtQtyTested.Text.Trim()))
                        {
                            // Leak Test Details
                            _DBObj.InsertLeakTestDetails(txtLeakTestNo.Text.Trim(), lblSerialNo.ToString(), 
                                Convert.ToInt16(txtObservation), Convert.ToInt16(txtHighPressureSeatLeak), Convert.ToInt16(txtLowPressureSeatLeak), Convert.ToInt16(txtBackSeatLeak)
                                , Convert.ToInt16(txtWeldLeak), Convert.ToInt16(txtJointLeak), Convert.ToInt16(txtTrapShellLeak), Convert.ToInt16(txtTrapWeldLeak),
                                Convert.ToInt16(txtTrapJointLeak), Convert.ToInt16(txtTrapShutLeak));
                        }
                    }

                    var deletedSerialCount = Convert.ToString(ViewState["DeletedSerialNoList"]) == "" ? Convert.ToString(txtQtyTested.Text) : Convert.ToString(ViewState["DeletedSerialNoList"]);

                    // Leak Test Master
                    _DBObj.InsertLeakTestMaster(txtLeakTestNo.Text.Trim(), selectedDate, txtProdOrderNo.Text, ddlType.SelectedValue,
                        Convert.ToInt32(deletedSerialCount), ddlOperator.SelectedValue, txtRemarks.Text,
                        System.DateTime.UtcNow);

                    Clear(); // todo

                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Leak Test added successfully.";

                    txtLeakTestNo.Enabled = true;
                }
                else // Update Patrol Inspection
                {
                    foreach (GridViewRow row in this.gridLeakTest.Rows)
                    {
                        string lblSerialNo = ((Label)row.FindControl("lblSerialNo")).Text.ToString();

                        string txtObservation = ((TextBox)row.FindControl("txtShellLeak")).Text.ToString();
                        string txtHighPressureSeatLeak = ((TextBox)row.FindControl("txtHighPressureSeatLeak")).Text.ToString();
                        string txtLowPressureSeatLeak = ((TextBox)row.FindControl("txtLowPressureSeatLeak")).Text.ToString();
                        string txtBackSeatLeak = ((TextBox)row.FindControl("txtBackSeatLeak")).Text.ToString();
                        string txtWeldLeak = ((TextBox)row.FindControl("txtWeldLeak")).Text.ToString();
                        string txtJointLeak = ((TextBox)row.FindControl("txtJointLeak")).Text.ToString();
                        string txtTrapShellLeak = ((TextBox)row.FindControl("txtTrapShellLeak")).Text.ToString();
                        string txtTrapWeldLeak = ((TextBox)row.FindControl("txtTrapWeldLeak")).Text.ToString();
                        string txtTrapJointLeak = ((TextBox)row.FindControl("txtTrapJointLeak")).Text.ToString();
                        string txtTrapShutLeak = ((TextBox)row.FindControl("txtTrapShutLeak")).Text.ToString();

                        if (!string.IsNullOrEmpty(txtLeakTestNo.Text.Trim()) && !string.IsNullOrEmpty(txtQtyTested.Text.Trim()))
                        {
                            // Leak Test Details - Update

                            _DBObj.UpdateLeakTestDetails(txtLeakTestNo.Text.Trim(), lblSerialNo.ToString(),
                                Convert.ToInt16(txtObservation), Convert.ToInt16(txtHighPressureSeatLeak), Convert.ToInt16(txtLowPressureSeatLeak), Convert.ToInt16(txtBackSeatLeak)
                                , Convert.ToInt16(txtWeldLeak), Convert.ToInt16(txtJointLeak), Convert.ToInt16(txtTrapShellLeak), Convert.ToInt16(txtTrapWeldLeak),
                                Convert.ToInt16(txtTrapJointLeak), Convert.ToInt16(txtTrapShutLeak));
                        }
                    }

                    var deletedSerialCount = Convert.ToString(ViewState["DeletedSerialNoList"]) == "" ? Convert.ToString(txtQtyTested.Text) : Convert.ToString(ViewState["DeletedSerialNoList"]);

                    // Leak Master - Update 
                    _DBObj.UpdateLeakTestMaster(txtLeakTestNo.Text.Trim(), selectedDate, txtProdOrderNo.Text, Convert.ToInt16(deletedSerialCount)
                        ,ddlOperator.SelectedValue, txtRemarks.Text, System.DateTime.UtcNow);

                    Clear();

                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Leak Test updated successfully.";

                    txtLeakTestNo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from while inserting leak test main grid details");
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            lblMessage.Visible = false;

            var OrderExist = txtProdOrderNo.Text.Trim().Substring(0, 1);

            if (OrderExist == "4")
            {
                if (!string.IsNullOrEmpty(txtLeakTestNo.Text.Trim()))
                {
                    //ds = _DBObj.GetCheckListMasterDetails(ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

                    var serialNoList = ViewState["selectedSerialNoList"];

                    if (serialNoList != null)
                    {
                        txtQtyTested.Text = Convert.ToString(ViewState["selectedSerialNoCount"]);

                        txtQtyTested.Enabled = false;
                    }

                    if (!string.IsNullOrEmpty(txtQtyTested.Text.Trim()))
                    {
                        //if (ds != null && ds.Tables[0].Rows.Count > 0)
                        //{
                        //    gridPatrolInspection.DataSource = ds;
                        //    gridPatrolInspection.DataBind();

                        //    btnUpdate.Visible = true;
                        //}

                        SetInitialRow(serialNoList, 0); // 0 means Prod Order Starting with '4' digits
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Leak Test qty is missing";
                    }
                }
            }
            else if (OrderExist != "4")
            {
                ViewState["selectedSerialNoCount"] = "0";

                txtQtyTested.Text = "0";

                txtQtyTested.Enabled = true;

                SetInitialRow("", 1); // 1 means Prod Order Not Starting with '4' digits
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Get Leak Master and details
                DataSet ds = _DBObj.GetLeakTestMaster(txtLeakTestNo.Text.Trim());

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    _DBObj.DeleteLeakTestMasterAndDetails(txtLeakTestNo.Text.Trim());

                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Leak test details deleted successfully.";

                    Clear();
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Invalid Leak test number.";

                    return;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from deleting leak test  details.");
            }
        }

        protected void txtLeakTestNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Get Patrol Master
                DataSet ds = _DBObj.GetLeakTestMaster(txtLeakTestNo.Text.Trim());

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Visible = false;

                    DateTime patrolDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["LeakTestDate"]);
                    txtPatrolDate.Text = Convert.ToString(patrolDate.ToString("MM/dd/yyyy"));
                    ddlType.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["SubLocationCode"]);
                    txtProdOrderNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["ProdOrderNo"]);
                    txtQtyTested.Text = Convert.ToString(ds.Tables[0].Rows[0]["LeakTestQty"]);

                    ddlOperator.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["OperatorCode"]);
                    txtRemarks.Text = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]);

                    var OrderExist = txtProdOrderNo.Text.Trim().Substring(0, 1);

                    if (OrderExist == "4")
                    {
                        ds = _DBObj.GetMISOrderStatusForPatrol(txtProdOrderNo.Text.Trim());

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            txtItemNumber.Text = Convert.ToString(ds.Tables[0].Rows[0]["Item"]);
                            txtDescription.Text = Convert.ToString(ds.Tables[0].Rows[0]["Description"]);
                            txtCustomer.Text = Convert.ToString(ds.Tables[0].Rows[0]["CustomerName"]);
                            txtSaleOrder.Text = Convert.ToString(ds.Tables[0].Rows[0]["OrderNo"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["Pos"]);
                        }
                    }
                    else if (OrderExist != "4")
                    {
                        DataSet ds1 = _DBObj.GetWIPAllFromProdOrderNo(txtProdOrderNo.Text.Trim());

                        if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                        {
                            txtItemNumber.Text = Convert.ToString(ds1.Tables[0].Rows[0]["ItemNumber"]);
                            txtDescription.Text = Convert.ToString(ds1.Tables[0].Rows[0]["Description"]);
                            txtCustomer.Text = "";
                            txtSaleOrder.Text = "";
                        }
                    }

                    // Get Serial No Grid View Details
                    DataSet ds2 = _DBObj.GetLeakTestSerial(txtLeakTestNo.Text.Trim());

                    var query = ds2.Tables[0].AsEnumerable();  // Or datatable.Select();

                    var sorted = query
                                 .Where(u => (string)u["SerialNo"] != "")
                                 .OrderBy(u => u["SerialNo"]).ToArray();

                    if (sorted != null && sorted.ToList().Count > 0)
                    {
                        GridViewPopUp.DataSource = ds2;
                        GridViewPopUp.DataBind();
                    }

                    // Get Leak Test Details
                    GetLeakTestDetails(txtLeakTestNo.Text.Trim());// todo

                    ddlType.Enabled = false;
                    txtProdOrderNo.Enabled = false;
                    txtItemNumber.Enabled = false;
                    txtDescription.Enabled = false;
                    txtCustomer.Enabled = false;
                    txtSaleOrder.Enabled = false;
                    btnGenerate.Enabled = true;
                    btnDelete.Enabled = true;
                    btnPopDelete.Visible = true;
                    btnUpdate.Visible = true;
                    txtQtyTested.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Invalid Leak Test Number!";

                    return;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from fetching leak test details.");
            }
        }
    }
}