using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.QueryServices.Query;

namespace NewCRM.QueryServices.Test
{
    [TestClass]
    public class UnitTest1
    {
        ////导入无参数方法
        //[Import]
        //public QueryFactory MethodWithoutPara { get; set; }


        [TestMethod]
        public void TestMethod1()
        {
            //UnitTest1 pro = new UnitTest1();

            //pro.Compose();
            //var a = pro.MethodWithoutPara;

        }

        //private void Compose()
        //{
        //    var catalog = new AssemblyCatalog(Assembly.GetAssembly(typeof(DefaultQueryFactory)));
        //    CompositionContainer container = new CompositionContainer(catalog);
        //    container.ComposeParts(this);
        //}
    }


    //[Export]
    //public class MusicBook
    //{
    //    //导出公有方法

    //    public IQuery GetBookName<T>()
    //    {
    //        return null;
    //    }
    //}
}
