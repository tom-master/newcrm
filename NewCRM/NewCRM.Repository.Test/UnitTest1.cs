using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Repository.Test
{
    [TestClass]
    public class UnitTest1
    {



        [TestMethod]
        public void TestMethod1()
        {
            GetTypeClass<Desk>();
        }

        public T GetTypeClass<T>() where T : new()
        {
            T Tclass = new T();  //实例化一个 T 类对象
                                 //获取该类类型  
            System.Type t = Tclass.GetType();

            //得到所有属性
            System.Reflection.PropertyInfo[] propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToArray();

            var a = propertyInfos.Where(w => w.GetAccessors(true).Any(ww => ww.IsVirtual));

            //foreach (var propertyInfo in propertyInfos)
            //{
            //    foreach (var methodInfo in propertyInfo.GetAccessors(true))
            //    {
            //        if (methodInfo.IsVirtual)
            //        {
            //            var a = propertyInfo;
            //        }
            //    }
            //}

            return default(T);
        }
    }

    public class AgentClass
    {
        public int agentID
        {
            get;
            private set;
        }
        public int level
        {
            get;
            private set;
        }
        public int blood
        {
            get;
            private set;
        }
        public byte moveSpeed
        {
            get;
            private set;
        }
        public int power
        {
            get;
            private set;
        }
        public float attackInterval
        {
            get;
            private set;
        }
        public byte characterType
        {
            get;
            private set;
        }
        public byte campType
        {
            get;
            private set;
        }
    }


}

