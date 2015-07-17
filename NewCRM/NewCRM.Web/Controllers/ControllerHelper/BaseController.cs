using System;
using System.Text;
using System.Web.Mvc;
using NewCRM.Dto.Dto;

namespace NewCRM.Web.Controllers.ControllerHelper
{
    public class BaseController : Controller
    {
        private Int32 _index = 0;
        /// <summary>
        /// 当前页面的浏览权限，为空的话 就是不受权限控制
        /// </summary>
        protected virtual string PagePowers
        {
            get
            {
                return string.Empty;
            }
        }

        #region 提供对临时数据的常用方法的管理
        /// <summary>
        /// 设置临时数据
        /// </summary>
        /// <param name="dataKey">键</param>
        /// <param name="dataValue">值</param>
        protected void SetTempData(String dataKey, object dataValue)
        {
            if (!TempData.Keys.Contains(dataKey))
            {
                TempData.Add(dataKey, dataValue);
            }
            else
            {
                // throw new 
            }

        }
        /// <summary>
        /// 获取设置的临时数据
        /// </summary>
        /// <param name="dataKey">键</param>
        /// <returns></returns>
        protected object GetTempData(String dataKey)
        {
            return TempData[dataKey];
        }
        /// <summary>
        /// 移除临时数据
        /// </summary>
        /// <param name="dataName">键</param>
        protected void RemoveTempData(String dataName)
        {
            TempData.Remove(dataName);
        }

        /// <summary>
        /// 清空所有临时数据
        /// </summary>
        protected void ClearAllTempData()
        {
            TempData.Clear();
        }

        protected Int32 GetTempDataCount
        {
            get { return TempData.Count; }
        }

        #endregion


        /// <summary>
        /// 获取保存的用户实体
        /// </summary>
        protected UserDto UserEntity
        {
            get { return Session["userEntity"] as UserDto; }
        }



        #region error code
        //protected void SyncSelectedRowIndexArrayToHiddenField(HiddenField hiddendIds, Grid grid)
        //{
        //    List<int> ids = GetSelectedIdsFormHiddent(hiddendIds);

        //    List<int> selectedRows = new List<int>();
        //    if (grid.SelectedRowIndexArray != null && grid.SelectedRowIndexArray.Length > 0)
        //    {
        //        selectedRows = new List<int>(grid.SelectedRowIndexArray);
        //    }

        //    if (grid.IsDatabasePaging)
        //    {
        //        for (int i = 0, count = Math.Min(grid.PageSize, (grid.RecordCount - grid.PageIndex * grid.PageSize)); i < count; i++)
        //        {
        //            int id = Convert.ToInt32(grid.DataKeys[i][0]);
        //            if (selectedRows.Contains(i))
        //            {
        //                if (!ids.Contains(id))
        //                {
        //                    ids.Add(id);
        //                }
        //            }
        //            else
        //            {
        //                if (ids.Contains(id))
        //                {
        //                    ids.Remove(id);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        int startPageIndex = grid.PageIndex * grid.PageSize;
        //        for (int i = startPageIndex, count = Math.Min(startPageIndex + grid.PageSize, grid.RecordCount); i < count; i++)
        //        {
        //            int id = Convert.ToInt32(grid.DataKeys[i][0]);
        //            if (selectedRows.Contains(i - startPageIndex))
        //            {
        //                if (!ids.Contains(id))
        //                {
        //                    ids.Add(id);
        //                }
        //            }
        //            else
        //            {
        //                if (ids.Contains(id))
        //                {
        //                    ids.Remove(id);
        //                }
        //            }
        //        }
        //    }
        //    hiddendIds.Text = new JArray(ids).ToString(Formatting.None);
        //}

        //protected void ReplaceEntities<T>(ICollection<T> existEntities, int[] newEntityIDs, T tEntity) where T : class,  IKeyId, new()
        //{
        //    if (newEntityIDs.Length == 0)
        //    {
        //        existEntities.Clear();
        //    }
        //    else
        //    {
        //        int[] tobeAdded = newEntityIDs.Except(existEntities.Select(x => x.Id)).ToArray();
        //        int[] tobeRemoved = existEntities.Select(x => x.Id).Except(newEntityIDs).ToArray();

        //        AddEntities(existEntities, tobeAdded, tEntity);

        //        existEntities.Where(x => tobeRemoved.Contains(x.Id)).ToList().ForEach(e => existEntities.Remove(e));
        //    }
        //}
        //// 向现有实体集合中添加新项
        //protected void AddEntities<T>(ICollection<T> existItems, int[] newItemId, T tEntitiy) where T : class,  IKeyId, new()
        //{
        //    foreach (T t in newItemId.Select(roleId => tEntitiy))
        //    {
        //        existItems.Add(t);
        //    }
        //}
        #endregion

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new Jsons
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

    }
}
