using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using AjaxPro;
using NewCRM.Entity.Entity;
using NewCRM.Common;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using Users = NewCRM.Entity.Entity.User;

namespace NewCRM.Web
{
    public partial class Login : Page
    {
        private static readonly ILoginBll UserBll = CreatefactoryBll.CreateLoginBll();
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Login));
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
            }
        }
        /// <summary>
        /// 成功登陆后执行权限的加载
        /// </summary>
        private void LoginAccess(User user)
        {
            //记录当前登陆成功的用户的登陆时间
            UserBll.RegisterOnlineUser(user);
            SessionProvide.PageValue = DateTime.Now;
            string roleIds = string.Empty;
            if (user.Roles.Count > 0)
            {
                roleIds = string.Join(",", user.Roles.Select(role => role.Id).ToArray());
            }
            bool isPersistent = false;
            DateTime date = DateTime.Now.AddMinutes(120);

            CreateFormsVaildateTicket(user.Name, roleIds, isPersistent, date);
           // HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl);
        }
        /// <summary>
        /// / 创建表单验证的票证并存储在客户端Cookie中
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleIds"></param>
        /// <param name="isPersistent"></param>
        /// <param name="date"></param>
        private void CreateFormsVaildateTicket(string userName, string roleIds, bool isPersistent, DateTime date)
        {
            try
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                    userName,
                    DateTime.Now,
                    date,
                    isPersistent,
                    roleIds);
                string hashTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket)
                {
                    HttpOnly = true,
                    Expires = isPersistent ? date : DateTime.MinValue
                };

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [AjaxMethod]
        public int UserLogin(String uName, String uPassword)
        {
            int result = 0;
            String userName = uName;
            String passWord = uPassword;
            Users user = UserBll.Login(userName);
            if (user != null && PasswordUtil.ComparePasswords(user.Password, passWord))
            {
                if (!user.Enabled)
                {
                    result = 0;
                }
                else
                {
                    LoginAccess(user);
                    result = 1;
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }
    }
}