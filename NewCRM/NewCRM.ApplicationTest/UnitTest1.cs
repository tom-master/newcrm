using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.ApplicationService;
using NewCRM.ApplicationService.IApplicationService;

namespace NewCRM.ApplicationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IUserApplicationService _userApplicationService = new UserApplicationService();
            _userApplicationService.SetWebWallPaper("http://h.hiphotos.baidu.com/image/w%3D310/sign=d518951a60d0f703e6b293dd38fa5148/359b033b5bb5c9ea4ac5c0eed739b6003af3b38d.jpg");
        }
    }
}
