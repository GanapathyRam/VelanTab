using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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

                FillLocationMaster();

                FillSubLocationMaster();

                FillEmployeeMaster();

                FillShiftMaster();

                FillOperatorMaster();

                btnSubmit.Visible = false;

                //txtPatrolNumber.Enabled = false;

                //GetPatrolDetails("P1902100001");

                GridViewPopUp.DataSource = new DataTable();
                GridViewPopUp.DataBind();
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
                lblMessage.Text = "Invalid Location and Sub Location";

                txtPatrolNumber.Text = "";
                txtProdOrderNo.Text = "";

                return;
            }

            var OrderExist = ProdOrderNo.Substring(0, 1);

            if (OrderExist == "4")
            {
                DataSet ds1 = new DataSet();

                ds1 = _DBObj.GetProductionReleaseNewForPatrolWithSerial(ProdOrderNo, ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

                if (ds1 != null && ds1.Tables[0].Rows.Count == 0)
                {
                    ds1 = _DBObj.GetProductionReleaseNewForPatrolWithOutSerial(ProdOrderNo, ddlLocation.SelectedValue, ddlSubLocation.SelectedValue);

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

            if (!string.IsNullOrEmpty(txtPatrolNumber.Text.Trim()))
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
                    lblMessage.Text = "Patrol qty is missing";
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
                    Label lblMeetName = (e.Row.FindControl("lblMeets") as Label);

                    DropDownList1.DataSource = ds.Tables[0];

                    DropDownList1.DataTextField = "MeetName";
                    DropDownList1.DataValueField = "MeetCode";
                    DropDownList1.DataBind();

                    DropDownList1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Meets--", "0"));

                    if (!string.IsNullOrEmpty(lblMeetName.Text))
                    {
                        DropDownList1.SelectedValue = (e.Row.FindControl("lblMeets") as Label).Text;
                        DropDownList1.Items.FindByText((e.Row.FindControl("lblMeets") as Label).Text).Selected = true;
                    }
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
                                    txtProdOrderNo.Text, Convert.ToInt32(txtPatrolQty.Text), ddlOperator.SelectedValue, ddlShift.SelectedValue, Convert.ToInt32(ddlInspBy.SelectedValue),
                                    txtRemarks.Text, System.DateTime.UtcNow);

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
                    currentYear = Convert.ToString("P" + currentYear + locationCode + subLocationCode + "1");
                }
                else
                {
                    var workOrderInc = Int32.Parse(model.ToString().Substring(7)) + 1;
                    currentYear = Convert.ToString("P" + currentYear + locationCode + subLocationCode + workOrderInc);
                }
            }
            else
            {
                currentYear = Convert.ToString("P" + currentYear + locationCode + subLocationCode + "1");
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
            bool IsPatrolNumberExist = false;

            var selectedDate = DateTime.ParseExact(txtPatrolDate.Text, "MM/dd/yyyy", null);

            try
            {
                IsPatrolNumberExist = _DBObj.IsPatrolNumberExist(txtPatrolNumber.Text.Trim());

                if (!IsPatrolNumberExist)
                {
                    foreach (GridViewRow row in this.gridPatrolInspection.Rows)
                    {
                        string lblCheckListSerial = ((Label)row.FindControl("lblCheckListSerial")).Text.ToString();
                        string lblCheckListDescription = ((Label)row.FindControl("lblCheckListDescription")).Text.ToString();
                        DropDownList ddlMeets = ((DropDownList)row.FindControl("ddlMeets"));
                        string txtObservation = ((TextBox)row.FindControl("txtObservation")).Text.ToString();

                        //string lblMeetName = ((Label)row.FindControl("lblMeets")).Text.ToString();

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
                    _DBObj.InsertPatrolInspectionMaster(txtPatrolNumber.Text, selectedDate, ddlLocation.SelectedValue, ddlSubLocation.SelectedValue,
                        txtProdOrderNo.Text, Convert.ToInt32(deletedSerialCount), ddlOperator.SelectedValue, ddlShift.SelectedValue, Convert.ToInt32(ddlInspBy.SelectedValue), txtRemarks.Text,
                        System.DateTime.UtcNow);

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
                    lblMessage.Text = "Patrol Inspection added successfully.";

                    txtPatrolNumber.Enabled = true;
                }
                else // Update Patrol Inspection
                {
                    foreach (GridViewRow row in this.gridPatrolInspection.Rows)
                    {
                        var savedImage = string.Empty;
                        string path = "";
                        string image = string.Empty;

                        string lblCheckListSerial = ((Label)row.FindControl("lblCheckListSerial")).Text.ToString();
                        string lblCheckListDescription = ((Label)row.FindControl("lblCheckListDescription")).Text.ToString();
                        DropDownList ddlMeets = ((DropDownList)row.FindControl("ddlMeets"));
                        string txtObservation = ((TextBox)row.FindControl("txtObservation")).Text.ToString();

                        savedImage = ((System.Web.UI.WebControls.Image)row.FindControl("Image1")).ImageUrl;

                        FileUpload FileUpload = (FileUpload)row.FindControl("FileUpload1");

                        if (FileUpload.HasFile)
                        {
                            path += FileUpload.FileName;

                            //save image in folder  
                            if (!string.IsNullOrEmpty(path))
                            {
                                FileUpload.SaveAs(MapPath("~/FileUpload/" + path));
                            }

                            image = FileUpload.FileName;
                        }

                        else if (!string.IsNullOrEmpty(savedImage) && string.IsNullOrEmpty(path))
                        {
                            image = ((System.Web.UI.WebControls.Image)row.FindControl("Image1")).ImageUrl.Substring(13);
                            path = image;
                        }

                        if (!string.IsNullOrEmpty(txtPatrolNumber.Text) && !string.IsNullOrEmpty(txtPatrolQty.Text))
                        {
                            // Patrol Details
                            _DBObj.UpdatePatrolInspectionDetails(txtPatrolNumber.Text, lblCheckListSerial.ToString(), ddlMeets.SelectedValue, txtObservation.ToString(),
                                "", "", image, path);
                        }
                    }

                    var deletedSerialCount = Convert.ToString(ViewState["DeletedSerialNoList"]) == "" ? Convert.ToString(txtPatrolQty.Text) : Convert.ToString(ViewState["DeletedSerialNoList"]);

                    // Patrol Master
                    _DBObj.UpdatePatrolInspectionMaster(txtPatrolNumber.Text, selectedDate,
                        Convert.ToInt32(deletedSerialCount), ddlOperator.SelectedValue, ddlShift.SelectedValue, Convert.ToInt32(ddlInspBy.SelectedValue), txtRemarks.Text, System.DateTime.UtcNow);

                    Clear();

                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Patrol Inspection updated successfully.";

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
                    txtPatrolDate.Text = Convert.ToString(patrolDate.ToString("MM/dd/yyyy"));
                    ddlLocation.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["LocationCode"]);
                    ddlSubLocation.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["SubLocationCode"]);
                    txtProdOrderNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["ProdOrderNo"]);
                    txtPatrolQty.Text = Convert.ToString(ds.Tables[0].Rows[0]["PatrolQty"]);

                    ddlOperator.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["OperatorCode"]);
                    ddlShift.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ShiftCode"]);
                    ddlInspBy.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["EmployeeCode"]);
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
                    lblMessage.Text = "Invalid Patrol Number";

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
                    lblMessage.Text = "Patrol deleted successfully.";

                    Clear();
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Invalid Patrol number.";

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
                    lblMessage.Text = "Select Serial no's to delete";

                    return;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from deleting patrol inspection  details.");
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            var pdfFooter = new PdfFooter();

            try
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 30f, 40f);
                iTextSharp.text.Font NormalFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                bool IsPatrolNumberExist = _DBObj.IsPatrolNumberExist(txtPatrolNumber.Text.Trim());

                if (IsPatrolNumberExist)
                {
                    // Get Patrol Master Details
                    DataSet ds = _DBObj.GetPatrolMaster(txtPatrolNumber.Text.Trim());

                    var OrderExist = txtProdOrderNo.Text.Trim().Substring(0, 1);

                    DataSet ds1 = new DataSet();
                    DataSet ds4 = new DataSet();

                    if (OrderExist == "4")
                    {
                        // Get Misorderstaus Details If Prod Order Has '4' Series
                        ds1 = _DBObj.GetMISOrderStatusForPatrol(txtProdOrderNo.Text.Trim());
                    }
                    else
                    {
                        ds4 = _DBObj.GetWIPAllFromProdOrderNo(txtProdOrderNo.Text.Trim());
                    }

                    // Get Patrol Serial No
                    DataSet ds2 = _DBObj.GetPatrolInspectionSerial(txtPatrolNumber.Text.Trim());

                    // Get Patrol View Details
                    DataSet ds3 = _DBObj.GetPatrolDetails(txtPatrolNumber.Text.Trim());

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                            PdfPCell cell = null;
                            PdfPTable table = null;

                            writer.PageEvent = new PdfFooter();
                            writer.PageEvent = pdfFooter;

                            document.Open();
                            table = new PdfPTable(5);
                            table.HeaderRows = 8;
                            table.TotalWidth = 500f;
                            table.LockedWidth = true;

                            //table.DefaultCell.BorderWidth = 0.5f;
                            cell = ImageCell("../Image/logo_velan.png", 4.5f, PdfPCell.ALIGN_LEFT);
                            cell.FixedHeight = 45f;
                            cell.VerticalAlignment = 0;
                            cell.PaddingLeft = 2f;
                            cell.PaddingTop = 10f;
                            cell.BorderWidth = 0.5f;
                            cell.BorderColor = BaseColor.BLACK;
                            cell.Border = iTextSharp.text.Rectangle.BOX;
                            cell.Colspan = 1;
                            table.AddCell(cell);

                            PdfPCell header = new PdfPCell(new Phrase("PATROL INSPECTION REPORT", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            header.Colspan = 4;
                            header.PaddingTop = 15f;
                            header.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(header);

                            var item = string.Empty;

                            if (OrderExist == "4" && ds1 != null && ds1.Tables[0].Rows.Count > 0)
                            {
                                item = ds1.Tables[0].Rows[0]["Item"].ToString();
                            }
                            else if (OrderExist != "4" && ds4 != null && ds4.Tables[0].Rows.Count > 0)
                            {
                                item = ds4.Tables[0].Rows[0]["ItemNumber"].ToString();
                            }

                            PdfPCell itemNumber = new PdfPCell(new Phrase("Item No                          : " + item + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            itemNumber.Colspan = 3;
                            //itemNumber.FixedHeight = 25f;
                            itemNumber.PaddingTop = 7f;
                            itemNumber.BorderWidthTop = 0f;
                            itemNumber.BorderWidthBottom = 0f;
                            itemNumber.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(itemNumber);

                            PdfPCell patrolNumber = new PdfPCell(new Phrase("Patrol No         : " + ds.Tables[0].Rows[0]["PatrolNumber"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            patrolNumber.Colspan = 2;
                            patrolNumber.PaddingTop = 7f;
                            patrolNumber.BorderWidthLeft = 0f;
                            patrolNumber.BorderWidthTop = 0f;
                            patrolNumber.BorderWidthBottom = 0f;
                            table.AddCell(patrolNumber);

                            var description = string.Empty;

                            if (OrderExist == "4" && ds1 != null && ds1.Tables[0].Rows.Count > 0)
                            {
                                description = ds1.Tables[0].Rows[0]["Description"].ToString();
                            }
                            else if (OrderExist != "4" && ds4 != null && ds4.Tables[0].Rows.Count > 0)
                            {
                                description = ds4.Tables[0].Rows[0]["Description"].ToString();
                            }

                            PdfPCell itemDescription = new PdfPCell(new Phrase("Item Description            : " + description + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            itemDescription.Colspan = 3;
                            //itemDescription.FixedHeight = 25f;
                            itemDescription.PaddingTop = 7f;
                            itemDescription.BorderWidthTop = 0f;
                            itemDescription.BorderWidthBottom = 0f;
                            itemDescription.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(itemDescription);

                            PdfPCell pOrderNo = new PdfPCell(new Phrase("P.Order No      : " + ds.Tables[0].Rows[0]["ProdOrderNo"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            pOrderNo.Colspan = 2;
                            //pOrderNo.FixedHeight = 25f;
                            pOrderNo.BorderWidthTop = 0f;
                            pOrderNo.BorderWidthLeft = 0f;
                            pOrderNo.PaddingTop = 7f;
                            pOrderNo.BorderWidthBottom = 0f;
                            table.AddCell(pOrderNo);

                            var orderNo = string.Empty;
                            var position = string.Empty;

                            if (OrderExist == "4")
                            {
                                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                                {
                                    orderNo = ds1.Tables[0].Rows[0]["OrderNo"].ToString();
                                }
                                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                                {
                                    position = ds1.Tables[0].Rows[0]["Pos"].ToString();
                                }
                            }

                            PdfPCell saleOrderNo = new PdfPCell(new Phrase("Sale Order No + Pos     : " + orderNo + " - " + position + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            saleOrderNo.Colspan = 3;
                            //saleOrderNo.FixedHeight = 25f;
                            saleOrderNo.PaddingTop = 7f;
                            //saleOrderNo.BorderWidthRight = 0f;
                            saleOrderNo.BorderWidthTop = 0f;
                            saleOrderNo.BorderWidthBottom = 0f;
                            table.AddCell(saleOrderNo);

                            PdfPCell location = new PdfPCell(new Phrase("Location           : " + ds.Tables[0].Rows[0]["LocationName"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            location.Colspan = 2;
                            location.PaddingTop = 7f;
                            location.BorderWidthLeft = 0f;
                            location.BorderWidthTop = 0f;
                            location.BorderWidthBottom = 0f;
                            table.AddCell(location);

                            var customerName = string.Empty;

                            if (OrderExist == "4")
                            {
                                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                                {
                                    customerName = ds1.Tables[0].Rows[0]["CustomerName"].ToString();
                                }
                            }

                            PdfPCell customer = new PdfPCell(new Phrase("Customer                       : " + customerName + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            customer.Colspan = 3;
                            customer.PaddingTop = 7f;
                            //customer.PaddingBottom = 3f;
                            customer.BorderWidthTop = 0f;
                            customer.BorderWidthBottom = 0f;
                            customer.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(customer);

                            PdfPCell subLocation = new PdfPCell(new Phrase("Sub Location    : " + ds.Tables[0].Rows[0]["SubLocationName"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            subLocation.Colspan = 2;
                            subLocation.PaddingTop = 7f;
                            subLocation.BorderWidthLeft = 0f;
                            subLocation.BorderWidthTop = 0f;
                            subLocation.BorderWidthBottom = 0f;
                            subLocation.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(subLocation);

                            var customerPoNo = string.Empty;
                            var lineNo = string.Empty;

                            if (OrderExist == "4")
                            {
                                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                                {
                                    customerPoNo = ds1.Tables[0].Rows[0]["CustomerOrderNo"].ToString();
                                }
                                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                                {
                                    lineNo = ds1.Tables[0].Rows[0]["LineNum"].ToString();
                                }
                            }

                            PdfPCell custPoNo = new PdfPCell(new Phrase("Cust.PO.No + Line No   : " + customerPoNo + " - " + lineNo + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            custPoNo.Colspan = 3;
                            custPoNo.PaddingTop = 7f;
                            //custPoNo.BorderWidthRight = 0f;
                            custPoNo.BorderWidthTop = 0f;
                            custPoNo.BorderWidthBottom = 0f;
                            custPoNo.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(custPoNo);

                            PdfPCell pdfOperator = new PdfPCell(new Phrase("Operator           : " + ds.Tables[0].Rows[0]["OperatorName"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            pdfOperator.Colspan = 2;
                            pdfOperator.PaddingTop = 7f;
                            pdfOperator.BorderWidthTop = 0f;
                            pdfOperator.BorderWidthLeft = 0f;
                            pdfOperator.BorderWidthBottom = 0f;
                            pdfOperator.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(pdfOperator);

                            //var savedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PatrolDate"].ToString());

                            //PdfPCell pdfDate = new PdfPCell(new Phrase("Date   : " + savedDate.ToString("MM/dd/yyyy") + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            //pdfDate.Colspan = 1;
                            ////pdfDate.FixedHeight = 25f;
                            //pdfDate.PaddingTop = 20f;
                            //pdfDate.BorderWidthRight = 0f;
                            //pdfDate.BorderWidthLeft = 0f;
                            //pdfDate.BorderWidthTop = 0f;
                            //pdfDate.BorderWidthBottom = 0f;
                            //pdfDate.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(pdfDate);

                            var serialNoList = string.Empty;
                            if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                {
                                    serialNoList += Convert.ToString(ds2.Tables[0].Rows[i]["SerialNo"]);
                                    serialNoList += ',';
                                }

                                //serialNoList.Replace(Convert.ToString(serialNoList.ToCharArray().LastOrDefault()), "");
                                serialNoList = serialNoList.TrimEnd(',');
                            }

                            PdfPCell volumeNo = new PdfPCell(new Phrase("Valve Sl.No                    : " + serialNoList + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            volumeNo.Colspan = 3;
                            volumeNo.PaddingTop = 7f;
                            //volumeNo.BorderWidthLeft = 0f;
                            volumeNo.BorderWidthTop = 0f;
                            volumeNo.PaddingBottom = 7f;
                            volumeNo.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(volumeNo);

                            PdfPCell shift = new PdfPCell(new Phrase("Shift      : " + ds.Tables[0].Rows[0]["ShiftName"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            shift.Colspan = 1;
                            shift.PaddingTop = 7f;
                            shift.BorderWidthRight = 0f;
                            shift.BorderWidthLeft = 0f;
                            shift.BorderWidthTop = 0f;
                            shift.PaddingBottom = 7f;
                            shift.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(shift);

                            //PdfPCell emptyCell = new PdfPCell(new Phrase(""));
                            ////emptyCell.PaddingTop = 7f;
                            //emptyCell.Colspan = 2;
                            //emptyCell.BorderWidthTop = 0f;
                            //emptyCell.BorderWidthLeft = 0f;
                            //emptyCell.BorderWidthBottom = 0f;
                            //table.AddCell(emptyCell);                            

                            PdfPCell pdfQty = new PdfPCell(new Phrase("Qty   : " + ds.Tables[0].Rows[0]["PatrolQty"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            pdfQty.Colspan = 1;
                            pdfQty.PaddingTop = 7f;
                            pdfQty.BorderWidthLeft = 0f;
                            pdfQty.PaddingBottom = 3f;
                            pdfQty.BorderWidthTop = 0f;
                            pdfQty.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(pdfQty);

                            //PdfPTable nested = new PdfPTable(1);
                            PdfPCell gridSlNo = new PdfPCell(new Phrase("Sl.No", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            gridSlNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            gridSlNo.Colspan = 1;
                            gridSlNo.BorderWidthTop = 0f;
                            gridSlNo.BorderWidthBottom = 0f;
                            gridSlNo.PaddingTop = 5f;
                            table.AddCell(gridSlNo);

                            //PdfPCell nesthousing = new PdfPCell(nested);
                            //nesthousing.Padding = 0f;
                            //table.AddCell(nesthousing);

                            PdfPCell gridCheckPoints = new PdfPCell(new Phrase("Check Points", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            gridCheckPoints.Colspan = 2;
                            gridCheckPoints.BorderWidthTop = 0f;
                            gridCheckPoints.BorderWidthLeft = 0f;
                            //gridCheckPoints.BorderWidthRight = 0f;
                            gridCheckPoints.BorderWidthBottom = 0f;
                            gridCheckPoints.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            gridCheckPoints.PaddingTop = 5f;
                            table.AddCell(gridCheckPoints);

                            //PdfPTable nested1 = new PdfPTable(2);
                            PdfPCell gridMeets = new PdfPCell(new Phrase("Meets the Requirements", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            gridMeets.Colspan = 1;
                            gridMeets.PaddingTop = 3f;
                            gridMeets.BorderWidthLeft = 0f;
                            gridMeets.BorderWidthTop = 0f;
                            gridMeets.BorderWidthRight = 0f;
                            gridMeets.BorderWidthBottom = 0f;
                            gridMeets.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(gridMeets);

                            PdfPCell gridComments = new PdfPCell(new Phrase("Comments", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            gridComments.Colspan = 1;
                            gridComments.PaddingTop = 5f;
                            gridComments.BorderWidthTop = 0f;
                            gridComments.BorderWidthBottom = 0f;
                            gridComments.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(gridComments);

                            for (int i = 0; i <= ds3.Tables[0].Rows.Count - 1; i++)
                            {
                                int j = i + 1;
                                //ds3.Tables[0].Rows[i]["CheckListSerial"].ToString()
                                PdfPCell gridSlNoValue = new PdfPCell(new Phrase("" + j + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                gridSlNoValue.Colspan = 1;
                                //gridSlNoValue.BorderWidthTop = 0f;
                                //gridSlNoValue.BorderWidthBottom = 0f;
                                gridSlNoValue.BorderWidthRight = 0f;
                                gridSlNoValue.PaddingTop = 5f;
                                gridSlNoValue.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(gridSlNoValue);

                                //PdfPCell gridDescriptionValue = new PdfPCell(new Phrase("" + ds3.Tables[0].Rows[i]["CheckListDescription"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                //gridDescriptionValue.Colspan = 2;
                                //gridDescriptionValue.Rowspan = 1;
                                //gridDescriptionValue.PaddingTop = 5f;
                                //gridDescriptionValue.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                //table.AddCell(gridDescriptionValue);
                                //PdfPCell cellImage = ImageCell("~/FileUpload/" + ds3.Tables[0].Rows[i]["MeetsImage"].ToString() + "", 4.5f, PdfPCell.ALIGN_LEFT);

                                PdfPTable nested = new PdfPTable(1);
                                nested.DefaultCell.BorderWidth = iTextSharp.text.Rectangle.NO_BORDER;

                                PdfPCell gridDescriptionValue = new PdfPCell(new Phrase("" + ds3.Tables[0].Rows[i]["CheckListDescription"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                gridDescriptionValue.Colspan = 2;
                                gridDescriptionValue.Rowspan = 1;
                                gridDescriptionValue.PaddingLeft = 5f;
                                gridDescriptionValue.BorderWidthTop = 0f;
                                gridDescriptionValue.BorderWidthRight = 0f;
                                gridDescriptionValue.BorderWidthBottom = 0f;
                                gridDescriptionValue.BorderWidthLeft = 0f;
                                nested.AddCell(gridDescriptionValue);

                                if (!string.IsNullOrEmpty(ds3.Tables[0].Rows[i]["PatrolImage"].ToString()) && (Convert.ToString(ds3.Tables[0].Rows[i]["PatrolImage"]) != "NoImageFound_1.jpg"))
                                {
                                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/FileUpload/" + ds3.Tables[0].Rows[i]["PatrolImage"].ToString() + ""));
                                    PdfPCell cellImage = new PdfPCell(img);
                                    cellImage.Colspan = 2;
                                    cellImage.Rowspan = 1;
                                    cellImage.FixedHeight = 200f;
                                    cellImage.PaddingTop = 5f;
                                    cellImage.PaddingBottom = 5f;
                                    cellImage.PaddingLeft = 5f;
                                    cellImage.PaddingRight = 5f;
                                    cellImage.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cellImage.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    cellImage.BorderWidthTop = 0f;
                                    cellImage.BorderWidthBottom = 0f;
                                    cellImage.BorderWidthRight = 0f;
                                    cellImage.BorderWidthLeft = 0f;

                                    img.ScaleAbsolute(210, 210);
                                    nested.AddCell(cellImage);
                                }
                                else
                                {
                                    PdfPCell cellImageEmpty = new PdfPCell();
                                    cellImageEmpty.BorderWidth = 0f;
                                    nested.AddCell(cellImageEmpty);
                                }


                                PdfPCell nesthousing = new PdfPCell(nested);
                                //nesthousing.Padding = 0f;
                                nesthousing.Colspan = 2;
                                table.AddCell(nesthousing);

                                PdfPCell gridMeetsValue = new PdfPCell(new Phrase("" + ds3.Tables[0].Rows[i]["MeetName"].ToString() + "", FontFactory.GetFont("Arial", "", false, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                gridMeetsValue.Colspan = 1;
                                gridMeetsValue.PaddingTop = 5f;
                                gridMeetsValue.PaddingTop = 5f;
                                gridMeetsValue.BorderWidthRight = 0f;
                                //gridMeetsValue.BorderWidthBottom = 0f;
                                gridMeetsValue.BorderWidthLeft = 0f;
                                gridMeetsValue.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(gridMeetsValue);

                                PdfPCell gridCommentsValue = new PdfPCell(new Phrase("" + ds3.Tables[0].Rows[i]["MeetsComments"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                gridCommentsValue.Colspan = 1;
                                //gridCommentsValue.BorderWidthTop = 0f;
                                //gridCommentsValue.BorderWidthLeft = 0f;
                                //gridCommentsValue.BorderWidthBottom = 0f;
                                gridCommentsValue.PaddingTop = 5f;
                                gridCommentsValue.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(gridCommentsValue);
                            }

                            pdfFooter.InspectedBy = ds.Tables[0].Rows[0]["EmployeeName"].ToString();

                            //PdfPCell footer = new PdfPCell(new Phrase("Inspector Sign   : " + ds.Tables[0].Rows[0]["EmployeeName"].ToString() + "", FontFactory.GetFont("Arial", BaseFont.WINANSI, BaseFont.EMBEDDED, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            //footer.Colspan = 5;
                            //footer.PaddingTop = 25f;
                            //footer.FixedHeight = 45f;
                            //footer.VerticalAlignment = 1;
                            //footer.BorderWidth = 0.5f;
                            ////footer.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(footer);

                            document.Add(table);
                            document.Close();
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();
                            Response.Clear();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + txtPatrolNumber.Text.Trim() + ".pdf" + "");
                            Response.Buffer = true;
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.BinaryWrite(bytes);
                            Response.End();
                            Response.Close();
                            writer.Add(table);
                        }
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }

        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }

        private static PdfPCell ImageCell(string path, float scale, int align)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
            image.ScalePercent(scale);
            PdfPCell cell = new PdfPCell(image);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 0f;
            cell.PaddingTop = 0f;
            return cell;
        }

        protected void gridPatrolInspection_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridPatrolInspection.Rows)
            {
                bool isChecked = ((CheckBox)row.FindControl("CheckBox1")).Checked;
                if (isChecked)
                {
                    var savedImage = string.Empty;
                    string path = "";
                    string image = string.Empty;

                    string lblCheckListSerial = ((Label)row.FindControl("lblCheckListSerial")).Text.ToString();
                    string lblCheckListDescription = ((Label)row.FindControl("lblCheckListDescription")).Text.ToString();
                    DropDownList ddlMeets = ((DropDownList)row.FindControl("ddlMeets"));
                    string txtObservation = ((TextBox)row.FindControl("txtObservation")).Text.ToString();

                    savedImage = ((System.Web.UI.WebControls.Image)row.FindControl("Image1")).ImageUrl;

                    FileUpload FileUpload = (FileUpload)row.FindControl("FileUpload1");

                    if (savedImage != "NoImageFound_1.jpg")
                    {
                        // Patrol Details
                        _DBObj.UpdatePatrolInspectionDetails(txtPatrolNumber.Text, lblCheckListSerial.ToString(), ddlMeets.SelectedValue, txtObservation.ToString(),
                            "", "", "", "");
                    }
                }
            }

            txtPatrolNumber_TextChanged(sender, e);
        }
    }
}