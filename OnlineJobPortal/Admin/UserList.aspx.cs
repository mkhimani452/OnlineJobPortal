using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal.Admin
{
    public partial class UserList : System.Web.UI.Page
    {
        private UserData userData = new UserData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                userData = new UserData();
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            try
            {
                DataTable dt = userData.GetUserList();
                ViewState["UserData"] = dt;

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            if (ViewState["UserData"] != null)
            {
                GridView1.DataSource = ViewState["UserData"];
                GridView1.DataBind();
            }
            else
            {
                LoadUsers();
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                bool isDeleted = userData.DeleteUser(userId);

                if (isDeleted)
                {
                    llbMsg.Text = "User deleted successfully!";
                    llbMsg.CssClass = "alert alert-success";
                }
                else
                {
                    llbMsg.Text = "Cannot delete this record!";
                    llbMsg.CssClass = "alert alert-danger";
                }

                LoadUsers();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}
