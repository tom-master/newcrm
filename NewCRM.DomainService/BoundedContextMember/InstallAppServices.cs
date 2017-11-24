using System;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{

    public sealed class InstallAppServices : BaseServiceContext, IInstallAppServices
    {
        public Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.Id FROM dbo.Apps AS a WHERE a.AccountId={accountId} AND a.IsDeleted=0";
                var result = dataStore.SqlGetDataTable(sql).AsList<App>();
                return new Tuple<int, int>(result.Count, result.Count(a => a.AppReleaseState == AppReleaseState.UnRelease));
            }
        }

        public void Install(Int32 accountId, Int32 appId, Int32 deskNum)
        {
            ValidateParameter.Validate(accountId).Validate(appId).Validate(deskNum);

            using(var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    App app = null;
                    #region 获取app
                    {
                        var sql = $@"SELECT
                                a.Name,
                                a.IconUrl,
                                a.AppUrl,
                                a.Id,
                                a.Width,
                                a.Height,
                                a.IsLock,
                                a.IsMax,
                                a.IsFull,
                                a.IsSetbar,
                                a.IsOpenMax,
                                a.IsFlash,
                                a.IsDraw
                                FROM  dbo.Apps AS a WHERE a.AppAuditState={AppAuditState.Pass} AND a.AppReleaseState={AppReleaseState.Release} AND a.IsDeleted=0 AND a.Id={appId}";
                        app = dataStore.SqlGetDataTable(sql).AsSignal<App>();
                    }
                    #endregion

                    if(app == null)
                    {
                        throw new BusinessException($"应用添加失败，请刷新重试");
                    }

                    #region 添加桌面成员
                    {
                        var newMember = new Member(app.Name, app.IconUrl, app.AppUrl, app.Id, app.Width, app.Height, app.IsLock, app.IsMax, app.IsFull, app.IsSetbar, app.IsOpenMax, app.IsFlash, app.IsDraw, app.IsResize);

                        var sql = $@"INSERT dbo.Members
                            ( AppId ,
                              Width ,
                              Height ,
                              FolderId ,
                              Name ,
                              IconUrl ,
                              AppUrl ,
                              IsOnDock ,
                              IsMax ,
                              IsFull ,
                              IsSetbar ,
                              IsOpenMax ,
                              IsLock ,
                              IsFlash ,
                              IsDraw ,
                              IsResize ,
                              MemberType ,
                              IsDeleted ,
                              AddTime ,
                              LastModifyTime ,
                              AccountId ,
                              DeskIndex
                            )
                    VALUES  ( {newMember.AppId} , -- AppId - int
                              {newMember.Width} , -- Width - int
                              {newMember.Height} , -- Height - int
                              0 , -- FolderId - int
                              N'{newMember.Name}' , -- Name - nvarchar(6)
                              N'{newMember.IconUrl}' , -- IconUrl - nvarchar(max)
                              N'{newMember.AppUrl}' , -- AppUrl - nvarchar(max)
                              0 , -- IsOnDock - bit
                              {newMember.IsMax} , -- IsMax - bit
                              {newMember.IsFull} , -- IsFull - bit
                              {newMember.IsSetbar} , -- IsSetbar - bit
                              {newMember.IsOpenMax} , -- IsOpenMax - bit
                              {newMember.IsLock} , -- IsLock - bit
                              {newMember.IsFlash} , -- IsFlash - bit
                              {newMember.IsDraw} , -- IsDraw - bit
                              {newMember.IsResize} , -- IsResize - bit
                              {newMember.MemberType} , -- MemberType - int
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE() , -- LastModifyTime - datetime
                              {accountId} , -- AccountId - int
                              {deskNum}  -- DeskIndex - int
                            )";
                    }
                    #endregion

                    #region 更改app使用数量
                    {
                        var sql = $@"UPDATE dbo.Apps SET UseCount=UseCount+1 WHERE Id={app.Id} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch(Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }
    }
}
