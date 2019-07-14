using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VV.Web.Models;

namespace VV.Web.Views
{
    public partial class FindPatrolNumber : System.Web.UI.Page
    {
        DBUtil _DBObj = new DBUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var userDto = UsersDTO.Instance;

                Label lblUserName = (Label)this.Master.FindControl("lblUserName");

                lblUserName.Text = userDto.UserName;

                FillLocationMaster();
            }
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Get Serial No Grid View Details
            DataSet ds = _DBObj.GetPatrolNumberFromProdOrderNo(txtProdOrderNo.Text.Trim(), Convert.ToString(ddlLocation.SelectedValue));

            Cache["CacheForPatrolNumberList"] = ds;

            GridViewFindPatrol.DataSource = ds;
            GridViewFindPatrol.DataBind();
        }

        protected void GridViewFindPatrol_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = (DataSet)Cache["CacheForPatrolNumberList"];

            GridViewFindPatrol.PageIndex = e.NewPageIndex;
            GridViewFindPatrol.DataSource = ds;
            GridViewFindPatrol.DataBind();
        }
    }
}