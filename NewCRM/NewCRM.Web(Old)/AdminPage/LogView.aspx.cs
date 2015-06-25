using System;
using FineUI;
using NewCRM.Common;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
using System.Globalization;
namespace NewCRM.Web.AdminPage
{
    public partial class LogView : BasePage
    {
        private static readonly ILogViewBll Log = CreatefactoryBll.CreateLogViewBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreLogView; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            Int32 id = GetQueryIntValue("id");
            Entity.Entity.Log logs = Log.FindLog(id);
            if (logs==null)
            {
                Alert.Show("参数错误", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            labDateTime.Text = logs.LogTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            labException.Text = logs.Exception;
            labId.Text = logs.Level;
            labMessage.Text = logs.Message;
            labSource.Text = logs.Logger;
        }
    }
}