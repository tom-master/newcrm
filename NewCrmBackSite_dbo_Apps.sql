USE NewCrmBackSite

SELECT TOP 100 * FROM Apps

DELETE FROM Apps




SET IDENTITY_INSERT Apps ON 

INSERT INTO dbo.Apps (Id, Name, IconUrl, AppUrl, Remark, Width, Height, UseCount, IsMax, IsFull, IsSetbar, IsOpenMax, IsLock, IsSystem, IsFlash, IsDraw, IsResize, AccountId, AppTypeId, IsRecommand, AppAuditState, AppReleaseState, AppStyle, IsDeleted, AddTime, LastModifyTime) VALUES (1, N'账户管理', '/Scripts/HoorayUI/img/ui/system-gear.png', '/AccountManager/Index', '', 800, 600, 1, '0', '0', '0', '0', '0', '0', '0', '0', '1', 4, 1, '0', 2, 1, 1, '0', '2016-12-15 11:51:50.593', '2016-12-25 16:57:34.283');
INSERT INTO dbo.Apps (Id, Name, IconUrl, AppUrl, Remark, Width, Height, UseCount, IsMax, IsFull, IsSetbar, IsOpenMax, IsLock, IsSystem, IsFlash, IsDraw, IsResize, AccountId, AppTypeId, IsRecommand, AppAuditState, AppReleaseState, AppStyle, IsDeleted, AddTime, LastModifyTime) VALUES (2, N'应用类型管理', '/Scripts/HoorayUI/img/ui/system-gear.png', '/AppTypes/Index', '', 800, 600, 1, '0', '0', '0', '0', '0', '0', '0', '0', '1', 4, 1, '0', 2, 1, 1, '0', '2016-12-15 11:52:50.937', '2016-12-25 16:57:34.283');
INSERT INTO dbo.Apps (Id, Name, IconUrl, AppUrl, Remark, Width, Height, UseCount, IsMax, IsFull, IsSetbar, IsOpenMax, IsLock, IsSystem, IsFlash, IsDraw, IsResize, AccountId, AppTypeId, IsRecommand, AppAuditState, AppReleaseState, AppStyle, IsDeleted, AddTime, LastModifyTime) VALUES (3, N'角色管理', '/Scripts/HoorayUI/img/ui/system-gear.png', '/security/RoleManage', '', 800, 600, 1, '0', '0', '0', '0', '0', '0', '0', '0', '1', 4, 1, '0', 2, 1, 1, '0', '2016-12-15 11:53:53.757', '2016-12-25 16:57:34.283');
INSERT INTO dbo.Apps (Id, Name, IconUrl, AppUrl, Remark, Width, Height, UseCount, IsMax, IsFull, IsSetbar, IsOpenMax, IsLock, IsSystem, IsFlash, IsDraw, IsResize, AccountId, AppTypeId, IsRecommand, AppAuditState, AppReleaseState, AppStyle, IsDeleted, AddTime, LastModifyTime) VALUES (4, N'权限管理', '/Scripts/HoorayUI/img/ui/system-gear.png', '/security/PowerManage', '', 800, 600, 1, '0', '0', '0', '0', '0', '1', '0', '0', '1', 4, 1, '0', 2, 1, 1, '0', '2016-12-15 11:54:21.000', '2016-12-25 16:57:34.283');
INSERT INTO dbo.Apps (Id, Name, IconUrl, AppUrl, Remark, Width, Height, UseCount, IsMax, IsFull, IsSetbar, IsOpenMax, IsLock, IsSystem, IsFlash, IsDraw, IsResize, AccountId, AppTypeId, IsRecommand, AppAuditState, AppReleaseState, AppStyle, IsDeleted, AddTime, LastModifyTime) VALUES (5, N'应用管理', '/Scripts/HoorayUI/img/ui/system-gear.png', '/AppManager/index', '', 800, 600, 1, '0', '0', '0', '0', '0', '0', '0', '0', '1', 4, 1, '1', 2, 1, 1, '0', '2016-12-16 17:52:10.773', '2016-12-25 16:57:34.283');
INSERT INTO dbo.Apps (Id, Name, IconUrl, AppUrl, Remark, Width, Height, UseCount, IsMax, IsFull, IsSetbar, IsOpenMax, IsLock, IsSystem, IsFlash, IsDraw, IsResize, AccountId, AppTypeId, IsRecommand, AppAuditState, AppReleaseState, AppStyle, IsDeleted, AddTime, LastModifyTime) VALUES (8, N'测试', '/Scripts/HoorayUI/img/ui/system-gear.png', '/test/index', '', 800, 600, 1, '0', '0', '0', '0', '0', '1', '0', '0', '1', 4, 3, '0', 2, 1, 1, '0', '2016-12-18 15:30:13.000', '2016-12-25 20:53:31.370');

SET IDENTITY_INSERT Apps OFF 