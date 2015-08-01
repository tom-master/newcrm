using System;
using System.Web.UI;
namespace NewCRM.Web.AdminPage
{
    public partial  class AdminIndex :Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }
        private void LoadData() 
        {
            
        }
    }
}