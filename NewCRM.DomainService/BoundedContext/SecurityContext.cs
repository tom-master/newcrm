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
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Roles AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = dataStore.FindSingleValue<Int32>(sql, new List<SqlParameter> { new SqlParameter("@name", role.Name) });
                    if (result > 0)
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
                                  @RoleIdentity , -- RoleIdentity - nvarchar(20)
                                  @Remark , -- Remark - nvarchar(50)
                                  0 , -- IsDeleted - bit
                                  GETDATE() , -- AddTime - datetime
                                  GETDATE()  -- LastModifyTime - datetime
                                )";
                    dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@name", role.Name), new SqlParameter("@RoleIdentity", role.RoleIdentity), new SqlParameter("@Remark", role.Remark) });
                }
                #endregion
            }
        }

        public void AddPowerToCurrentRole(int roleId, IEnumerable<int> powerIds)
        {
            ValidateParameter.Validate(roleId).Validate(powerIds);
            if (!powerIds.Any())
            {
                throw new BusinessException("权限列表为空");
            }

            using (var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    #region 移除之前的角色权限
                    {
                        var sql = $@"UPDATE dbo.RolePowers SET IsDeleted=1 WHERE RoleId=@RoleId";
                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@RoleId",roleId)
                        };
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    #region 添加角色权限
                    {
                        var sqlBuilder = new StringBuilder();
                        foreach (var item in powerIds)
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
                        dataStore.SqlExecute(sqlBuilder.ToString());
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch (Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public bool CheckPermissions(int accessAppId, params int[] roleIds)
        {
            ValidateParameter.Validate(accessAppId).Validate(roleIds);
            using (var dataStore = new DataStore())
            {

                #region 检查app是否为系统app
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Apps AS a WHERE a.Id=@Id AND a.IsDeleted=0 AND a.IsSystem=1";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Id",accessAppId)
                    };
                    var result = dataStore.FindSingleValue<Int32>(sql, parameters);
                    if (result <= 0)
                    {
                        return true;
                    }
                }
                #endregion

                {
                    var sql = $@"SELECT a.AppId FROM dbo.RolePowers AS a WHERE a.RoleId IN({String.Join(",", roleIds)}) AND a.IsDeleted=0";
                    var result = dataStore.Find<RolePower>(sql);
                    return result.Any(a => a.AppId == accessAppId);
                }
            }
        }

        public IList<RolePower> GetPowers()
        {
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a.RoleId,
                            a.AppId
                            FROM dbo.RolePowers AS a 
                            WHERE a.IsDeleted=0";
                return dataStore.Find<RolePower>(sql);
            }
        }

        public Role GetRole(int roleId)
        {
            ValidateParameter.Validate(roleId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a.Name,
                            a.RoleIdentity,
                            a.Remark
                            FROM dbo.Roles AS a 
                            WHERE a.Id=@Id AND a.IsDeleted=0";
                var parameters = new List<SqlParameter> { new SqlParameter("@Id", roleId) };
                return dataStore.FindOne<Role>(sql, parameters);
            }
        }

        public List<Role> GetRoles(string roleName, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dataStore = new DataStore())
            {
                var where = new StringBuilder();
                var parameters = new List<SqlParameter>();
                if (!String.IsNullOrEmpty(roleName))
                {
                    parameters.Add(new SqlParameter("@roleName", $@"%{roleName}%"));
                    where.Append($@" AND a.Name LIKE @roleName");
                }
                #region totalCount
                {
                    var sql = $@"SELECT
	                            COUNT(*)
	                            FROM dbo.Roles AS a 
                                WHERE 1=1 {where} AND a.IsDeleted=0";

                    totalCount = dataStore.FindSingleValue<Int32>(sql, parameters);
                }
                #endregion

                #region sql
                {
                    var sql = $@"SELECT TOP (@pageSize) * FROM 
                                (
	                                 SELECT
	                                ROW_NUMBER() OVER(ORDER BY a.Id DESC) AS rownumber,
	                                a.Name,
	                                a.RoleIdentity,
	                                a.Remark,
                                    a.Id
	                                FROM dbo.Roles AS a WHERE 1=1 {where} AND a.IsDeleted=0
                                ) AS aa WHERE aa.rownumber>@pageSize*(@pageIndex-1)";
                    parameters.Add(new SqlParameter("@pageIndex", pageIndex));
                    parameters.Add(new SqlParameter("@pageSize", pageSize));
                    return dataStore.Find<Role>(sql, parameters);
                }
                #endregion
            }
        }

        public void ModifyRole(Role role)
        {
            ValidateParameter.Validate(role);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Roles AS a WHERE a.Name=@name AND a.IsDeleted=0";
                    var result = dataStore.FindSingleValue<Int32>(sql, new List<SqlParameter> { new SqlParameter("@name", role.Name) });
                    if (result > 0)
                    {
                        throw new BusinessException($@"角色:{role.Name} 已经存在");
                    }
                }
                #endregion

                #region 修改角色
                {
                    var sql = $@"UPDATE dbo.Roles SET Name=@name,RoleIdentity=@identity WHERE Id=@Id AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@name",role.Name),
                        new SqlParameter("@identity",role.RoleIdentity),
                        new SqlParameter("@Id",role.Id)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
                #endregion
            }
        }

        public void RemoveRole(int roleId)
        {
            ValidateParameter.Validate(roleId);
            using (var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@roleId",roleId)
                    };
                    #region 前置条件验证
                    {
                        var sql = $@"SELECT COUNT(*) FROM dbo.AccountRoles AS a WHERE a.RoleId=@roleId AND a.IsDeleted=0";
                        var result = dataStore.FindSingleValue<Int32>(sql, parameters);
                        if (result > 0)
                        {
                            throw new BusinessException("当前角色已绑定了账户，无法删除");
                        }
                    }
                    #endregion

                    #region 删除角色
                    {
                        var sql = $@"UPDATE dbo.Roles SET IsDeleted=1 WHERE Id=@roleId AND IsDeleted=0";
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    #region 移除权限
                    {
                        var sql = $@"UPDATE dbo.RolePowers SET IsDeleted=1 WHERE RoleId=@roleId AND IsDeleted=0";
                        dataStore.SqlExecute(sql, parameters);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch (Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }
    }
}
