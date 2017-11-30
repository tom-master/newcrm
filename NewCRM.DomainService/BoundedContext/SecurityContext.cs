using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Services.Interface;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class SecurityContext : BaseServiceContext, ISecurityContext
    {
        public void AddNewRole(Role role)
        {
            ValidateParameter.Validate(role);
            using(var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"
SELECT COUNT(*) FROM dbo.Roles AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlScalar(sql, new List<SqlParameter> { new SqlParameter("@name", role.Name) });
                    if(result > 0)
                    {
                        throw new BusinessException($@"角色:{role.Name} 已经存在");
                    }
                }
                #endregion

                #region 添加角色
                {
                    var sql = $@"INSERT dbo.Roles
                                ( Name ,
                                  RoleIdentity ,
                                  Remark ,
                                  IsDeleted ,
                                  AddTime ,
                                  LastModifyTime
                                )
                        VALUES  ( @name , -- Name - nvarchar(6)
                                  N'{role.RoleIdentity}' , -- RoleIdentity - nvarchar(20)
                                  N'{role.Remark}' , -- Remark - nvarchar(50)
                                  0 , -- IsDeleted - bit
                                  GETDATE() , -- AddTime - datetime
                                  GETDATE()  -- LastModifyTime - datetime
                                )";
                    dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@name", role.Name) });
                }
                #endregion
            }
        }

        public void AddPowerToCurrentRole(int roleId, IEnumerable<int> powerIds)
        {
            ValidateParameter.Validate(roleId).Validate(powerIds);
            if(!powerIds.Any())
            {
                throw new BusinessException("权限列表为空");
            }

            using(var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    #region 移除之前的角色权限
                    {
                        var sql = $@"
UPDATE dbo.RolePowers SET IsDeleted=1 WHERE RoleId={roleId}";
                    }
                    #endregion

                    #region 添加角色权限
                    {
                        var sqlBuilder = new StringBuilder();
                        foreach(var item in powerIds)
                        {
                            sqlBuilder.Append($@"INSERT dbo.RolePowers
                                                    ( RoleId ,
                                                      AppId ,
                                                      IsDeleted ,
                                                      AddTime ,
                                                      LastModifyTime
                                                    )
                                            VALUES  ( {roleId} , -- RoleId - int
                                                      {item}, -- AppId - int
                                                      0 , -- IsDeleted - bit
                                                      GETDATE() , -- AddTime - datetime
                                                      GETDATE()  -- LastModifyTime - datetime
                                                    )");
                        }


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

        public bool CheckPermissions(int accessAppId, params int[] roleIds)
        {
            ValidateParameter.Validate(accessAppId).Validate(roleIds);
            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.AppId FROM dbo.RolePowers AS a WHERE a.RoleId IN({String.Join(",", roleIds)}) AND a.IsDeleted=0";
                var result = dataStore.SqlGetDataTable(sql).AsList<RolePower>().ToList();
                return result.Any(a => a.AppId == accessAppId);
            }
        }

        public IList<RolePower> GetPowers()
        {
            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a.RoleId,
                            a.AppId
                            FROM dbo.RolePowers AS a 
                            WHERE a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsList<RolePower>().ToList();
            }
        }

        public Role GetRole(int roleId)
        {
            ValidateParameter.Validate(roleId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a.Name,
                            a.RoleIdentity,
                            a.Remark
                            FROM dbo.Roles AS a 
                            WHERE a.Id={roleId} AND a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsSignal<Role>();
            }
        }

        public List<Role> GetRoles(string roleName, int pageIndex, int pageSize, out int totalCount)
        {
            using(var dataStore = new DataStore())
            {
                var where = new StringBuilder();
                if(!String.IsNullOrEmpty(roleName))
                {
                    where.Append($@" AND a.Name LIKE '%{roleName}%'");
                }
                #region totalCount
                {
                    var sql = $@"SELECT
	                            COUNT(*)
	                            FROM dbo.Roles AS a 
                                WHERE 1=1 {where} AND a.IsDeleted=0";

                    totalCount = (Int32)dataStore.SqlScalar(sql);
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP {pageSize} * FROM 
                                (
	                                 SELECT
	                                ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber,
	                                a.Name,
	                                a.RoleIdentity,
	                                a.Remark
	                                FROM dbo.Roles AS a WHERE 1=1 {where} AND a.IsDeleted=0
                                ) AS aa WHERE aa.rownumber>{pageSize}*({pageIndex}-1)";

                    return dataStore.SqlGetDataTable(sql).AsList<Role>().ToList();
                }
                #endregion
            }
        }

        public void ModifyRole(Role role)
        {
            ValidateParameter.Validate(role);
            using(var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"
SELECT COUNT(*) FROM dbo.Roles AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlScalar(sql, new List<SqlParameter> { new SqlParameter("@name", role.Name) });
                    if(result > 0)
                    {
                        throw new BusinessException($@"角色:{role.Name} 已经存在");
                    }
                }
                #endregion

                #region 修改角色
                {
                    var sql = $@"
UPDATE dbo.Roles SET Name=@name,RoleIdentity=@identity WHERE Id={role.Id} AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@name",role.Name),
                        new SqlParameter("@identity",role.RoleIdentity)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
                #endregion
            }
        }

        public void RemoveRole(int roleId)
        {
            ValidateParameter.Validate(roleId);
            using(var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    #region 前置条件验证
                    {
                        var sql = $@"SELECT COUNT(*) FROM dbo.AccountRoles AS a WHERE a.RoleId={roleId} AND a.IsDeleted=0";
                        var result = (Int32)dataStore.SqlScalar(sql);
                        if(result > 0)
                        {
                            throw new BusinessException("当前角色已绑定了账户，无法删除");
                        }
                    }
                    #endregion

                    #region 删除角色
                    {
                        var sql = $@"UPDATE dbo.Roles SET IsDeleted=1 WHERE Id={roleId} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    #region 移除权限
                    {
                        var sql = $@"UPDATE dbo.RolePowers SET IsDeleted=1 WHERE RoleId={roleId} AND IsDeleted=0";
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
