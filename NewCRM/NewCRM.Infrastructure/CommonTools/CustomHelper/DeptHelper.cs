
namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    //public sealed class DeptHelper
    //{
    //    /// <summary>
    //    /// 注入部门对象
    //    /// </summary>
    //    private static readonly IDeptDal Dept;

    //    static DeptHelper()
    //    {
    //        Dept = CreatefactoryDal.CreateDal<IDeptDal>("DeptDal");
    //    }

    //    private static List<Depts> _depts;
    //    public static List<Depts> Depts
    //    {
    //        get
    //        {
    //            if (_depts == null)
    //            {
    //                InitDepts();
    //            }
    //            return _depts;
    //        }
    //    }
    //    private static void InitDepts()
    //    {
    //        _depts = new List<Depts>();
    //        List<Depts> dbDepts = Dept.InitDept();
    //        ResloveDeptCollection(dbDepts, null, 0);
    //    }
    //    private static Int32 ResloveDeptCollection(IEnumerable<Depts> dbDepts, Depts parentDept, Int32 level)
    //    {
    //        Int32 count = 0;
    //        IEnumerable<Depts> enumerable = dbDepts as IList<Depts> ?? dbDepts.ToList();
    //        foreach (Depts item in enumerable.Where(d => d.Parent == parentDept))
    //        {
    //            count++;
    //            _depts.Add(item);
    //            item.TreeLevel = level;
    //            item.IsTreeLeaf = true;
    //            item.Enabled = true;
    //            level++;
    //            Int32 childCount = ResloveDeptCollection(enumerable, item, level);
    //            if (childCount != 0)
    //            {
    //                item.IsTreeLeaf = false;
    //            }
    //            level--;
    //        }
    //        return count;
    //    }
    //    public static void Reload() { _depts = null; }
    //}
}
