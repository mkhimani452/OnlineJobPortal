using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal.Admin
{
    public partial class ContactList : System.Web.UI.Page
    {
        private ContactData contactData;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
                return;
            }
            contactData = new ContactData(); 
            if (!IsPostBack)
            {
                contactData = new ContactData();
                LoadContacts();
            }
        }

        private void LoadContacts()
        {
            try
            {
                DataTable dt = contactData.GetContacts();
                ViewState["ContactData"] = dt;

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
            LoadContacts();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int contactId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                bool isDeleted = contactData.DeleteContact(contactId);

                if (isDeleted)
                {
                    llbMsg.Text = "Contact deleted successfully!";
                    llbMsg.CssClass = "alert alert-success";
                }
                else
                {
                    llbMsg.Text = "Cannot delete this record!";
                    llbMsg.CssClass = "alert alert-danger";
                }

                LoadContacts();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}
