using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VV.Web.Models;

namespace VV.Web.Views
{
    public partial class PatrolInspection : System.Web.UI.Page
    {
        DBUtil _DBObj = new DBUtil();
        //int globalPatrolQty = 0;
        string selectedSerialNoList = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime currentDate = DateTime.UtcNow;
                txtPatrolDate.Text = Convert.ToString(currentDate.ToString("dd/MM/yyyy"));

                var userDto = UsersDTO.Instance;

                Label lblUserName = (Label)this.Master.FindControl("lblUserName");

                lblUserName.Text = userDto.UserName;

                FillLocationMaster();

                FillSubLocationMaster();

                FillEmployeeMaster();

                FillShiftMaster();

                FillOperatorMaster();

                btnSubmit.Visible = false;

                //txtPatrolNumber.Enabled = false;

                //GetPatrolDetails("P1902100001");
            }
        }

        protected void txtProdOrderNo_TextChanged(object sender, EventArgs e)
        {
            var PatrolNumber = string.Empty;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            int PatrolQtyCount = 0; int ReleaseQtyCount = 0;

            lblMessage.Text = "";

            // Empty Row Setting Up Main Grid
            gridPatrolInspection.DataSource = dt;
            gridPatrolInspection.DataBind();
            btnUpdate.Visible = false;

            var ProdOrderNo = txtProdOrderNo.Text.Trim();

            if (string.IsNullOrEmpty(ProdOrderNo))
            {
                GridViewPopUp.DataSource = dt;
                GridViewPopUp.DataBind();

                return;
            }

            if (ddlLocation.SelectedIndex != 0 && ddlSubLocation.SelectedIndex != 0)
            {
                PatrolNumber = GetPatrolNumber(ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

                if (!string.IsNullOrEmpty(PatrolNumber))
                {
                    lblMessage.Text = "";
                    txtPatrolNumber.Text = PatrolNumber;
                }
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please select the value from location and sub location dropdown.";

                txtPatrolNumber.Text = "";
                txtProdOrderNo.Text = "";

                return;
            }

            var OrderExist = ProdOrderNo.Substring(0, 1);

            if (OrderExist == "4")
            {
                DataSet ds1 = new DataSet();

                ds1 = _DBObj.GetProductionReleaseNewForPatrolWithSerial(ProdOrderNo);

                if (ds1 != null && ds1.Tables[0].Rows.Count == 0)
                {
                    ds1 = _DBObj.GetProductionReleaseNewForPatrolWithOutSerial(ProdOrderNo);

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

                        txtPatrolQty.Enabled = true;

                        txtPatrolQty.Text = "";

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
                    txtPatrolQty.Enabled = false;

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

                txtPatrolNumber.Enabled = false;
                btnDelete.Enabled = false;
            }

            // Prod Order Not Starting From '4'
            if (OrderExist != "4")
            {
                DataSet ds2 = new DataSet();

                ds2 = _DBObj.GetProductionReleaseNewForPatrol(ProdOrderNo, ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

                if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                {
                    txtPatrolQty.Text = "";

                    txtPatrolQty.Enabled = true;

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
                    txtPatrolQty.Text = "";

                    txtPatrolQty.Enabled = true;

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

                txtPatrolNumber.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            lblMessage.Visible = false;

            if (!string.IsNullOrEmpty(txtPatrolNumber.Text))
            {
                ds = _DBObj.GetCheckListMasterDetails(ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

                var serialNoList = ViewState["selectedSerialNoList"];

                if (serialNoList != null)
                {
                    txtPatrolQty.Text = Convert.ToString(ViewState["selectedSerialNoCount"]);

                    txtPatrolQty.Enabled = false;
                }

                if (!string.IsNullOrEmpty(txtPatrolQty.Text.Trim()))
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        gridPatrolInspection.DataSource = ds;
                        gridPatrolInspection.DataBind();

                        btnUpdate.Visible = true;
                    }
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Patrol qty is missing. Please enter the patrol qty or submit the selected serial no to proceed further.";
                }
            }
        }

        protected void gridPatrolInspection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                DataSet ds = _DBObj.GetMeetMaster();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DropDownList DropDownList1 = (e.Row.FindControl("ddlMeets") as DropDownList);

                    DropDownList1.DataSource = ds.Tables[0];

                    DropDownList1.DataTextField = "MeetName";
                    DropDownList1.DataValueField = "MeetCode";
                    DropDownList1.DataBind();

                    DropDownList1.Items.Insert(0, new ListItem("--Select Meets--", "0"));
                    //DropDownList1.SelectedValue = drv["Meets"].ToString();
                }
            }

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{

            //    if (e.Row.RowState.Equals(DataControlRowState.Edit))
            //    {

            //        Button btnUpload = e.Row.FindControl("btnUpload") as Button;
            //        ScriptManager.GetCurrent(this).RegisterPostBackControl(btnUpload);
            //    }
            //}
        }

        //protected void upload_Click(object sender, EventArgs e)
        //{

        //}

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //if (FileUpload1.HasFile)
            //{
            //    FileUpload1.SaveAs(MapPath("~/FileUpload/" + FileUpload1.FileName));
            //    imgViewFile.ImageUrl = "~/FileUpload/" + FileUpload1.FileName;
            //}
        }

        protected void gridPatrolInspection_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            var url_filename = this.gridPatrolInspection.Rows[e.NewSelectedIndex].Cells[1].Text;
            gridPatrolInspection.SelectedIndex = e.NewSelectedIndex;
        }

        protected void gridPatrolInspection_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            //int liRow;
            //if (e.CommandName == "upload")
            //{

            //    liRow = Convert.ToInt32(e.CommandArgument);

            //}


            //Button bts = e.CommandSource as Button;

            //Response.Write(bts.Parent.Parent.GetType().ToString());
            //if (e.CommandName.ToLower() != "upload")
            //{
            //    return;
            //}
            //FileUpload fu = bts.FindControl("FileUpload1") as FileUpload;//here
            //if (fu.HasFile)
            //{
            //    bool upload = true;
            //    string fleUpload = Path.GetExtension(fu.FileName.ToString());
            //    if (fleUpload.Trim() != string.Empty)
            //    {
            //        fu.SaveAs(Server.MapPath("~/FileUpload/" + fu.FileName.ToString()));
            //        string uploadedFile = (Server.MapPath("~/FileUpload/" + fu.FileName.ToString()));

            //        //Someting to do?...
            //    }
            //    else
            //    {
            //        upload = false;
            //        // Something to do?...
            //    }
            //    if (upload)
            //    {
            //        // somthing to do?...
            //    }
            //}
            //else
            //{
            //    //Something to do?...
            //}
        }

        protected void gridPatrolInspection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridPatrolInspection.EditIndex = e.NewEditIndex;
            GetCheckListMasterDetails();
        }

        protected void gridPatrolInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DBUtil _DBObj = new DBUtil();
            //lblMessage.Visible = false;

            try
            {
                foreach (GridViewRow row in gridPatrolInspection.Rows)
                {
                    bool isChecked = ((CheckBox)row.FindControl("chkSelect")).Checked;
                    if (isChecked)
                    {
                        string lblCheckListSerial = ((Label)row.FindControl("lblCheckListSerial")).Text.ToString();
                        string lblCheckListDescription = ((Label)row.FindControl("lblCheckListDescription")).Text.ToString();
                        DropDownList ddlMeets = ((DropDownList)row.FindControl("ddlMeets"));
                        string txtObservation = ((TextBox)row.FindControl("txtObservation")).Text.ToString();

                        //TextBox name = (TextBox)row.FindControl("txt_Name");
                        FileUpload FileUpload = (FileUpload)row.FindControl("FileUpload1");

                        string path = "";

                        if (FileUpload.HasFile)
                        {
                            path += FileUpload.FileName;
                            //save image in folder    
                            FileUpload.SaveAs(MapPath("~/FileUpload/" + path));

                            if (!string.IsNullOrEmpty(txtPatrolNumber.Text) && !string.IsNullOrEmpty(txtPatrolQty.Text))
                            {
                                // Patrol Master
                                _DBObj.InsertPatrolInspectionMaster(txtPatrolNumber.Text, Convert.ToDateTime(txtPatrolDate.Text), ddlLocation.SelectedValue, ddlSubLocation.SelectedValue,
                                    txtProdOrderNo.Text, Convert.ToInt32(txtPatrolQty.Text), ddlOperator.SelectedValue, ddlShift.SelectedValue, Convert.ToInt32(ddlInspBy.SelectedValue), txtRemarks.Text);

                                // Patrol Details
                                _DBObj.InsertPatrolInspectionDetails(txtPatrolNumber.Text, lblCheckListSerial.ToString(), ddlMeets.SelectedValue, txtObservation.ToString(),
                                    "", "", FileUpload.FileName, path);

                                // Patrol Serial

                                var serialNoList = ViewState["selectedSerialNoList"];

                                if (serialNoList != null)
                                {
                                    var split = serialNoList.ToString().Split(',');

                                    for (int i = 0; i < split.Length; i++)
                                    {
                                        if (split[i].Trim() != string.Empty)
                                        {
                                            _DBObj.InsertPatrolInspectionSerials(txtPatrolNumber.Text, split[i].Trim());
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // use previous user image if new image is not changed    
                            //Image img = (Image)gvImage.Rows[e.RowIndex].FindControl("img_user");
                            //path = img.ImageUrl;
                        }
                    }
                }

                gridPatrolInspection.EditIndex = -1;

                Clear();

                GetPatrolDetails(txtPatrolNumber.Text);
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from while updating row from Sale Order Qty Decrement.");
            }
        }

        protected void gridPatrolInspection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridPatrolInspection.EditIndex = -1;
            GetCheckListMasterDetails();
        }

        protected void GridViewPopUp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = (DataSet)Cache["CacheForPatrolSerialNoList"];

            GridViewPopUp.PageIndex = e.NewPageIndex;
            GridViewPopUp.DataSource = ds;
            GridViewPopUp.DataBind();

            //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
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

        protected void txtPatrolQty_TextChanged(object sender, EventArgs e)
        {
            var qtyCount = txtPatrolQty.Text;

            if (Convert.ToInt16(ViewState["CurrentPatrolQtyCount"]) < Convert.ToInt32(qtyCount.Trim()))
            {
                //// Need to show the error message
                lblMessage.Visible = true;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Patrol qty should be less than Balance qty.";
            }
            else
            {
                lblMessage.Visible = false;
            }
        }

        #region Private Methods

        private string GetPatrolNumber(string locationCode, string subLocationCode)
        {
            var currentYear = string.Empty;

            var model = _DBObj.GetPatrolNumber();

            currentYear = Helper.CurrentFiniancialYear();

            if (!string.IsNullOrEmpty(model))
            {
                var savedYear = Convert.ToString(model.ToString().Substring(1, 2));

                if (!savedYear.Equals(currentYear))
                {
                    currentYear = Convert.ToString("P" + currentYear + locationCode + "1000" + subLocationCode);
                }
                else
                {
                    var workOrderInc = Int32.Parse(model.ToString().Substring(5, 4)) + 1;
                    currentYear = Convert.ToString("P" + currentYear + locationCode + workOrderInc + subLocationCode);
                }
            }
            else
            {
                currentYear = Convert.ToString("P" + currentYear + locationCode + "1000" + subLocationCode);
            }

            return currentYear;
        }

        private DataSet FillLocationMaster()
        {

            DBUtil _DBObj = new DBUtil();
            DataSet ds = _DBObj.GetLocationMaster();

            ddlLocation.DataSource = ds;
            ddlLocation.DataBind();

            ddlLocation.Items.Insert(0, "----Please Select----");

            return ds;
        }

        private DataSet FillSubLocationMaster()
        {

            DBUtil _DBObj = new DBUtil();
            DataSet ds = _DBObj.GetSubLocationMaster();

            ddlSubLocation.DataSource = ds;
            ddlSubLocation.DataBind();

            ddlSubLocation.Items.Insert(0, "----Please Select----");

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

        private DataSet FillShiftMaster()
        {

            DBUtil _DBObj = new DBUtil();
            DataSet ds = _DBObj.GetShiftMasterDetails();

            ddlShift.DataSource = ds;
            ddlShift.DataBind();

            ddlShift.Items.Insert(0, "----Please Select----");

            return ds;
        }

        private DataSet FillEmployeeMaster()
        {

            DBUtil _DBObj = new DBUtil();
            DataSet ds = _DBObj.RetriveByEmployeesNameForPatrol();

            ddlInspBy.DataSource = ds;
            ddlInspBy.DataBind();

            ddlInspBy.Items.Insert(0, "----Please Select----");

            return ds;
        }

        private void GetCheckListMasterDetails()
        {

            //DBUtil _DBObj = new DBUtil();
            //DataSet ds = _DBObj.GetCheckListDetailsForPatrol();

            //gridPatrolInspection.DataSource = ds;
            //gridPatrolInspection.DataBind();

            DataSet ds = _DBObj.GetCheckListMasterDetails(ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

            gridPatrolInspection.DataSource = ds;
            gridPatrolInspection.DataBind();
        }

        private void GetPatrolDetails(String PatrolNumber)
        {
            DataSet ds = _DBObj.GetPatrolDetails(PatrolNumber);

            gridPatrolInspection.DataSource = ds;
            gridPatrolInspection.DataBind();
        }

        private void Clear()
        {
            DataTable dt = new DataTable();

            txtPatrolNumber.Text = "";
            txtProdOrderNo.Text = "";
            txtPatrolDate.Text = "";
            ddlLocation.SelectedIndex = 0;
            ddlSubLocation.SelectedIndex = 0;

            txtItemNumber.Text = "";
            txtDescription.Text = "";
            txtCustomer.Text = "";
            txtSaleOrder.Text = "";
            txtPatrolQty.Text = "";
            ddlOperator.SelectedIndex = 0;
            ddlInspBy.SelectedIndex = 0;
            ddlShift.SelectedIndex = 0;
            txtRemarks.Text = "";

            gridPatrolInspection.DataSource = dt;
            gridPatrolInspection.DataBind();

            GridViewPopUp.DataSource = dt;
            GridViewPopUp.DataBind();

            ddlLocation.Enabled = true;
            ddlSubLocation.Enabled = true;
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string PatrolNumber = string.Empty;

            try
            {
                PatrolNumber = GetPatrolNumber(ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

                if (string.IsNullOrWhiteSpace(PatrolNumber))
                {
                    foreach (GridViewRow row in this.gridPatrolInspection.Rows)
                    {
                        string lblCheckListSerial = ((Label)row.FindControl("lblCheckListSerial")).Text.ToString();
                        string lblCheckListDescription = ((Label)row.FindControl("lblCheckListDescription")).Text.ToString();
                        DropDownList ddlMeets = ((DropDownList)row.FindControl("ddlMeets"));
                        string txtObservation = ((TextBox)row.FindControl("txtObservation")).Text.ToString();

                        FileUpload FileUpload = (FileUpload)row.FindControl("FileUpload1");

                        string path = "";

                        path += FileUpload.FileName;
                        //save image in folder  

                        if (!string.IsNullOrEmpty(path))
                        {
                            FileUpload.SaveAs(MapPath("~/FileUpload/" + path));
                        }

                        if (!string.IsNullOrEmpty(txtPatrolNumber.Text) && !string.IsNullOrEmpty(txtPatrolQty.Text))
                        {
                            // Patrol Details
                            _DBObj.InsertPatrolInspectionDetails(txtPatrolNumber.Text, lblCheckListSerial.ToString(), ddlMeets.SelectedValue, txtObservation.ToString(),
                                "", "", FileUpload.FileName, path);
                        }
                    }

                    var deletedSerialCount = Convert.ToString(ViewState["DeletedSerialNoList"]) == "" ? Convert.ToString(txtPatrolQty.Text) : Convert.ToString(ViewState["DeletedSerialNoList"]);

                    // Patrol Master
                    _DBObj.InsertPatrolInspectionMaster(txtPatrolNumber.Text, Convert.ToDateTime(txtPatrolDate.Text), ddlLocation.SelectedValue, ddlSubLocation.SelectedValue,
                        txtProdOrderNo.Text, Convert.ToInt32(deletedSerialCount), ddlOperator.SelectedValue, ddlShift.SelectedValue, Convert.ToInt32(ddlInspBy.SelectedValue), txtRemarks.Text);

                    // Patrol Serial

                    var serialNoList = ViewState["selectedSerialNoList"];

                    if (serialNoList != null)
                    {
                        var split = serialNoList.ToString().Split(',');

                        for (int i = 0; i < split.Length; i++)
                        {
                            if (split[i].Trim() != string.Empty)
                            {
                                _DBObj.InsertPatrolInspectionSerials(txtPatrolNumber.Text, split[i].Trim());
                            }
                        }
                    }

                    //GetPatrolDetails(txtPatrolNumber.Text);

                    Clear();

                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Patrol Inspection details added successfully.";

                    txtPatrolNumber.Enabled = true;
                }
                else // Update Patrol Inspection
                {
                    foreach (GridViewRow row in this.gridPatrolInspection.Rows)
                    {
                        string lblCheckListSerial = ((Label)row.FindControl("lblCheckListSerial")).Text.ToString();
                        string lblCheckListDescription = ((Label)row.FindControl("lblCheckListDescription")).Text.ToString();
                        DropDownList ddlMeets = ((DropDownList)row.FindControl("ddlMeets"));
                        string txtObservation = ((TextBox)row.FindControl("txtObservation")).Text.ToString();

                        FileUpload FileUpload = (FileUpload)row.FindControl("FileUpload1");

                        string path = "";

                        path += FileUpload.FileName;
                        //save image in folder  

                        if (!string.IsNullOrEmpty(path))
                        {
                            FileUpload.SaveAs(MapPath("~/FileUpload/" + path));
                        }

                        if (!string.IsNullOrEmpty(txtPatrolNumber.Text) && !string.IsNullOrEmpty(txtPatrolQty.Text))
                        {
                            // Patrol Details
                            _DBObj.UpdatePatrolInspectionDetails(txtPatrolNumber.Text, lblCheckListSerial.ToString(), ddlMeets.SelectedValue, txtObservation.ToString(),
                                "", "", FileUpload.FileName, path);
                        }
                    }

                    var deletedSerialCount = Convert.ToString(ViewState["DeletedSerialNoList"]) == "" ? Convert.ToString(txtPatrolQty.Text) : Convert.ToString(ViewState["DeletedSerialNoList"]);

                    // Patrol Master
                    _DBObj.UpdatePatrolInspectionMaster(txtPatrolNumber.Text, Convert.ToDateTime(txtPatrolDate.Text), 
                        Convert.ToInt32(deletedSerialCount), ddlOperator.SelectedValue, ddlShift.SelectedValue, Convert.ToInt32(ddlInspBy.SelectedValue), txtRemarks.Text);

                    Clear();

                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Patrol Inspection details updated successfully.";

                    txtPatrolNumber.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from while inserting patrol main grid details");
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            txtPatrolNumber.Visible = true;
            ddlSubLocation.Enabled = false;
        }

        protected void txtPatrolNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Get Patrol Master
                DataSet ds = _DBObj.GetPatrolMaster(txtPatrolNumber.Text.Trim());

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Visible = false;

                    DateTime patrolDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PatrolDate"]);
                    txtPatrolDate.Text = Convert.ToString(patrolDate.ToString("dd/MM/yyyy"));
                    ddlLocation.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["LocationCode"]);
                    ddlSubLocation.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["SubLocationCode"]);
                    txtProdOrderNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["ProdOrderNo"]);
                    txtPatrolQty.Text = Convert.ToString(ds.Tables[0].Rows[0]["PatrolQty"]);

                    ddlOperator.SelectedIndex = Convert.ToInt32(ds.Tables[0].Rows[0]["OperatorCode"]);
                    ddlShift.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ShiftCode"]);
                    ddlInspBy.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["EmployeeCode"]);
                    txtRemarks.Text = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]);

                    ds = _DBObj.GetMISOrderStatusForPatrol(txtProdOrderNo.Text.Trim());

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        txtItemNumber.Text = Convert.ToString(ds.Tables[0].Rows[0]["Item"]);
                        txtDescription.Text = Convert.ToString(ds.Tables[0].Rows[0]["Description"]);
                        txtCustomer.Text = Convert.ToString(ds.Tables[0].Rows[0]["CustomerName"]);
                        txtSaleOrder.Text = Convert.ToString(ds.Tables[0].Rows[0]["OrderNo"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["Pos"]);
                    }

                    // Get Serial No Grid View Details
                    DataSet ds2 = _DBObj.GetPatrolInspectionSerial(txtPatrolNumber.Text.Trim());
                    GridViewPopUp.DataSource = ds2;
                    GridViewPopUp.DataBind();

                    // Get Patrol Details
                    GetPatrolDetails(txtPatrolNumber.Text.Trim());

                    ddlLocation.Enabled = false;
                    ddlSubLocation.Enabled = false;
                    txtProdOrderNo.Enabled = false;
                    txtItemNumber.Enabled = false;
                    txtDescription.Enabled = false;
                    txtCustomer.Enabled = false;
                    txtSaleOrder.Enabled = false;
                    btnGenerate.Enabled = false;
                    btnDelete.Enabled = true;
                    btnPopDelete.Visible = true;
                    btnUpdate.Visible = true;
                    txtPatrolQty.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Entered patrol number does not exist in the system, Please enter valid one or generate new one.";

                    return;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from fetching patrol master details.");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Get Patrol Master
                DataSet ds = _DBObj.GetPatrolMaster(txtPatrolNumber.Text.Trim());

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    _DBObj.DeletePatrolInspection(txtPatrolNumber.Text.Trim());

                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Entered patrol number has been removed successfully from the system.";

                    Clear();
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Entered patrol number does not exist in the system, Please enter valid one or generate new one.";

                    return;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from deleting patrol inspection  details.");
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
                                _DBObj.DeletePatrolInspectionSerials(txtPatrolNumber.Text, split[i].Trim(), 0, 0);
                            }
                        }

                        // Updating the Patrol Qty Count From Patrol Inspection Master
                        _DBObj.DeletePatrolInspectionSerials(txtPatrolNumber.Text, "", Convert.ToInt32(updatedCount), 1);

                        // Get Serial No Grid View Details
                        DataSet ds2 = _DBObj.GetPatrolInspectionSerial(txtPatrolNumber.Text.Trim());
                        GridViewPopUp.DataSource = ds2;
                        GridViewPopUp.DataBind();

                        btnUpdate.Visible = true;

                        ViewState["DeletedSerialNoList"] = Convert.ToString(updatedCount);

                        //lblMessage.Visible = true;
                        //lblMessage.ForeColor = System.Drawing.Color.Green;
                        //lblMessage.Text = "Selected serial no has been removed successfully.";
                    }
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Please select serial no to be removed from the system.";

                    return;
                }

                // Get Patrol Master
                //DataSet ds = _DBObj.GetPatrolMaster(txtPatrolNumber.Text.Trim());

                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    _DBObj.DeletePatrolInspectionSerials(txtPatrolNumber.Text.Trim());

                //    lblMessage.Visible = true;
                //    lblMessage.ForeColor = System.Drawing.Color.Green;
                //    lblMessage.Text = "Selected serial no has been removed successfully.";
                //}
                //else
                //{
                //    lblMessage.Visible = true;
                //    lblMessage.ForeColor = System.Drawing.Color.Red;
                //    lblMessage.Text = "Select serial no to be removed from the system.";

                //    return;
                //}
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from deleting patrol inspection  details.");
            }
        }
    }
}