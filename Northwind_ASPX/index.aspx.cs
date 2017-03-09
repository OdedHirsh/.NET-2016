using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Northwind_ASPX
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void btnChoose_Click(object sender, EventArgs e)
        {
            GridViewOrders.DataBind();
            lblMSG.Text = "Select a Customer";

        }

        protected void ddListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMSG.Text = "Press Choose to select the customers orders list";
            
        }
    }
}