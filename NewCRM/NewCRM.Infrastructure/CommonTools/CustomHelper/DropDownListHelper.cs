
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using NewCRM.Models;


//namespace NewCRM.CommonTools
//{
//    /// <summary>
//    /// 下拉列表
//    /// </summary>
//    public sealed class DropDownListHelper
//    {
//        public sealed class ResolveDropDownList
//        {
//            public static List<T> ResloveDdl<T>(IEnumerable<T> list) where T : class, ICustomTree, ICloneable, IKeyId, new()
//            {
//                return ResloveDdl(list, -1, true);
//            }
//            public static List<T> ResloveDdl<T>(IEnumerable<T> list, Int32 currentId) where T : class,ICustomTree, ICloneable, IKeyId, new()
//            {
//                return ResloveDdl(list, currentId, true);
//            }
//            private static List<T> ResloveDdl<T>(IEnumerable<T> source, Int32 currentId, bool addRootNode) where T : class, ICustomTree, ICloneable, IKeyId, new()
//            {
//                List<T> result = new List<T>();
//                if (addRootNode)
//                {
//                    T root = new T
//                    {
//                        Name = "--根节点--",
//                        Id = -1,
//                        TreeLevel = 0,
//                        Enabled = true
//                    };
//                    result.Add(root);
//                }
//                foreach (T newT in source.Select(item => (T)item.Clone()))
//                {
//                    result.Add(newT);
//                    if (addRootNode)
//                    {
//                        newT.TreeLevel++;
//                    }
//                }
//                if (currentId == -1) return result;
//                bool startChileNode = false;
//                Int32 startTreeLeve = 0;
//                foreach (T tItem in result)
//                {
//                    if (tItem.Id == currentId)
//                    {
//                        startTreeLeve = tItem.TreeLevel;
//                        tItem.Enabled = false;
//                        startChileNode = true;
//                    }
//                    else
//                    {
//                        if (!startChileNode) continue;
//                        if (tItem.TreeLevel > startTreeLeve)
//                        {
//                            tItem.Enabled = false;
//                        }
//                        else
//                        {
//                            startChileNode = false;
//                        }
//                    }
//                }
//                return result;
//            }
//        }
//    }
//}
