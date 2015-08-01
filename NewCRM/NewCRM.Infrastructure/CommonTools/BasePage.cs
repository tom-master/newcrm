//using NewCRM.Models;

namespace NewCRM.Infrastructure.CommonTools
{
    //public class BasePage : Page
    //{
    //    /// <summary>
    //    /// 当前页面的浏览权限，为空的话 就是不受权限控制
    //    /// </summary>
    //    protected virtual string PagePowers
    //    {
    //        get
    //        {
    //            return string.Empty;
    //        }
    //    }
    //    /// <summary>
    //    /// 获取用户脚色
    //    /// </summary>
    //    public static string GetIndentityName
    //    {
    //        get
    //        {
    //            return new Page().User.Identity.IsAuthenticated ? new Page().User.Identity.Name : string.Empty;
    //        }
    //    }
    //    /// <summary>
    //    /// 获取选择的数据的个数
    //    /// </summary>
    //    /// <returns></returns>
    //    protected Int32 GetSelectDataKeyId(Grid grid)
    //    {
    //        Int32 id = -1;
    //        Int32 rowIndex = grid.SelectedRowIndex;
    //        if (rowIndex >= 0)
    //        {
    //            id = Convert.ToInt32(grid.DataKeys[rowIndex][0]);
    //        }
    //        return id;
    //    }
    //    protected string GetSelectedDataKey(Grid grid, int dataIndex)
    //    {
    //        string data = String.Empty;
    //        int rowIndex = grid.SelectedRowIndex;
    //        if (rowIndex >= 0)
    //        {
    //            data = grid.DataKeys[rowIndex][dataIndex].ToString();
    //        }
    //        return data;
    //    }

    //    /// <summary>
    //    /// 获取表格选中项DataKeys的第一个值，并转化为整型列表
    //    /// </summary>
    //    /// <param name="grid"></param>
    //    /// <returns></returns>
    //    protected List<Int32> GetSelectedDataKeyIDs(Grid grid)
    //    {
    //        return grid.SelectedRowIndexArray.Select(rowIndex => Convert.ToInt32(grid.DataKeys[rowIndex][0])).ToList();
    //    }

    //    /// <summary>
    //    /// 获取查询字符串的值
    //    /// </summary>
    //    /// <param name="queryValue"></param>
    //    /// <returns></returns>
    //    protected Int32 GetQueryIntValue(String queryValue)
    //    {
    //        Int32 id = -1;
    //        try
    //        {
    //            id = Convert.ToInt32(Request.QueryString[queryValue], CultureInfo.InvariantCulture);
    //        }
    //        catch (Exception)
    //        {
    //            //TODO
    //        }
    //        return id;

    //    }

    //    protected String GetQueryStrValue(String queryValue)
    //    {
    //        return Request.QueryString[queryValue];
    //    }

    //    /// <summary>
    //    /// 为删除Grid中选中项的按钮添加提示信息
    //    /// </summary>
    //    /// <param name="btn"></param>
    //    /// <param name="grid"></param>
    //    protected void ResolveDeleteButtonForGrid(Button btn, Grid grid)
    //    {
    //        ResolveDeleteButtonForGrid(btn, grid, "确定要删除选中的{0}项记录吗？");
    //    }

    //    protected void ResolveDeleteButtonForGrid(Button btn, Grid grid, string confirmTemplate)
    //    {
    //        ResolveDeleteButtonForGrid(btn, grid, "请至少应该选择一项记录！", confirmTemplate);
    //    }

    //    protected void ResolveDeleteButtonForGrid(Button btn, Grid grid, string noSelectionMessage, string confirmTemplate)
    //    {
    //        // 点击删除按钮时，至少选中一项
    //        btn.OnClientClick = grid.GetNoSelectionAlertInParentReference(noSelectionMessage);
    //        btn.ConfirmText = String.Format(confirmTemplate, "&nbsp;<span class=\"highlight\"><script>" + grid.GetSelectedCountReference() + "</script></span>&nbsp;");
    //        btn.ConfirmTarget = Target.Top;
    //    }

    //    protected void ResolveEnableStatusButtonForGrid(MenuButton menuBtn, Grid grid, bool enabled)
    //    {
    //        String enabledValue = "启用";
    //        if (!enabled)
    //        {
    //            enabledValue = "禁用";
    //        }
    //        menuBtn.OnClientClick = grid.GetNoSelectionAlertInParentReference("请至少选择一项数据!");
    //        menuBtn.ConfirmText = String.Format("确定要{1}选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", grid.GetSelectedCountReference(), enabledValue);
    //        menuBtn.ConfirmTarget = Target.Top;
    //    }

    //    /// <summary>
    //    /// 表格排序
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="q"></param>
    //    /// <param name="grid"></param>
    //    /// <returns></returns>
    //    protected IQueryable<T> Sort<T>(IQueryable<T> q, Grid grid)
    //    {
    //        return q.SortBy(grid.SortField + " " + grid.SortDirection);
    //    }

    //    // 排序后分页
    //    protected IQueryable<T> SortAndPage<T>(IQueryable<T> q, Grid grid)
    //    {
    //        return Sort(q, grid).Skip(grid.PageIndex * grid.PageSize).Take(grid.PageSize);
    //    }
    //    /// <summary>
    //    /// 跨页保持选中项 - 根据隐藏字段的数据更新表格当前页面的选中行
    //    /// </summary>
    //    /// <param name="hiddendIds"></param>
    //    /// <param name="grid"></param>
    //    protected void UpdateSelectedIdRowIndexArray(HiddenField hiddendIds, Grid grid)
    //    {
    //        List<int> ids = GetSelectedIdsFormHiddent(hiddendIds);

    //        List<int> nextSelectedRowIndexArray = new List<int>();
    //        if (grid.IsDatabasePaging)
    //        {
    //            for (int i = 0, count = Math.Min(grid.PageSize, (grid.RecordCount - grid.PageIndex * grid.PageSize)); i < count; i++)
    //            {
    //                int id = Convert.ToInt32(grid.DataKeys[i][0]);
    //                if (ids.Contains(id))
    //                {
    //                    nextSelectedRowIndexArray.Add(i);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            int nextStartPageIndex = grid.PageIndex * grid.PageSize;
    //            for (int i = nextStartPageIndex, count = Math.Min(nextStartPageIndex + grid.PageSize, grid.RecordCount); i < count; i++)
    //            {
    //                int id = Convert.ToInt32(grid.DataKeys[i][0]);
    //                if (ids.Contains(id))
    //                {
    //                    nextSelectedRowIndexArray.Add(i - nextStartPageIndex);
    //                }
    //            }
    //        }
    //        grid.SelectedRowIndexArray = nextSelectedRowIndexArray.ToArray();
    //    }
    //    /// <summary>
    //    /// 从隐藏字段中获取选择的全部ID列表
    //    /// </summary>
    //    /// <param name="hiddentField"></param>
    //    /// <returns></returns>
    //    protected List<Int32> GetSelectedIdsFormHiddent(HiddenField hiddentField)
    //    {
    //        JArray array = new JArray();
    //        String currentIds = hiddentField.Text.Trim();
    //        array = !String.IsNullOrEmpty(currentIds) ? JArray.Parse(currentIds) : new JArray();
    //        return new List<Int32>(array.ToObject<Int32[]>());
    //    }

    //    /// <summary>
    //    /// 跨页保持选中项 - 将表格当前页面选中行对应的数据同步到隐藏字段中
    //    /// </summary>
    //    /// <param name="hiddendIds"></param>
    //    /// <param name="grid"></param>
    //    protected void SyncSelectedRowIndexArrayToHiddenField(HiddenField hiddendIds, Grid grid)
    //    {
    //        List<int> ids = GetSelectedIdsFormHiddent(hiddendIds);

    //        List<int> selectedRows = new List<int>();
    //        if (grid.SelectedRowIndexArray != null && grid.SelectedRowIndexArray.Length > 0)
    //        {
    //            selectedRows = new List<int>(grid.SelectedRowIndexArray);
    //        }

    //        if (grid.IsDatabasePaging)
    //        {
    //            for (int i = 0, count = Math.Min(grid.PageSize, (grid.RecordCount - grid.PageIndex * grid.PageSize)); i < count; i++)
    //            {
    //                int id = Convert.ToInt32(grid.DataKeys[i][0]);
    //                if (selectedRows.Contains(i))
    //                {
    //                    if (!ids.Contains(id))
    //                    {
    //                        ids.Add(id);
    //                    }
    //                }
    //                else
    //                {
    //                    if (ids.Contains(id))
    //                    {
    //                        ids.Remove(id);
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            int startPageIndex = grid.PageIndex * grid.PageSize;
    //            for (int i = startPageIndex, count = Math.Min(startPageIndex + grid.PageSize, grid.RecordCount); i < count; i++)
    //            {
    //                int id = Convert.ToInt32(grid.DataKeys[i][0]);
    //                if (selectedRows.Contains(i - startPageIndex))
    //                {
    //                    if (!ids.Contains(id))
    //                    {
    //                        ids.Add(id);
    //                    }
    //                }
    //                else
    //                {
    //                    if (ids.Contains(id))
    //                    {
    //                        ids.Remove(id);
    //                    }
    //                }
    //            }
    //        }
    //        hiddendIds.Text = new JArray(ids).ToString(Formatting.None);
    //    }

    //    protected void ReplaceEntities<T>(ICollection<T> existEntities, int[] newEntityIDs, T tEntity) where T : class,  IKeyId, new()
    //    {
    //        if (newEntityIDs.Length == 0)
    //        {
    //            existEntities.Clear();
    //        }
    //        else
    //        {
    //            int[] tobeAdded = newEntityIDs.Except(existEntities.Select(x => x.Id)).ToArray();
    //            int[] tobeRemoved = existEntities.Select(x => x.Id).Except(newEntityIDs).ToArray();

    //            AddEntities(existEntities, tobeAdded, tEntity);

    //            existEntities.Where(x => tobeRemoved.Contains(x.Id)).ToList().ForEach(e => existEntities.Remove(e));
    //        }
    //    }
    //    // 向现有实体集合中添加新项
    //    protected void AddEntities<T>(ICollection<T> existItems, int[] newItemId, T tEntitiy) where T : class,  IKeyId, new()
    //    {
    //        foreach (T t in newItemId.Select(roleId => tEntitiy))
    //        {
    //            existItems.Add(t);
    //        }
    //    }
    //}
}
