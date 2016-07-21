using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IAppServices))]
    public class AppServices : IAppServices
    {

        [Import]
        private IUserRepository _userRepository;

        [Import]
        private IDeskRepository _deskRepository;

        [Import]
        private IAppRepository _appRepository;


        public IDictionary<Int32, IList<dynamic>> GetApp(Int32 userId)
        {
            var userConfig = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).Config;

            IDictionary<Int32, IList<dynamic>> desks = new Dictionary<Int32, IList<dynamic>>();



            foreach (var desk in userConfig.Desks)
            {
                IList<dynamic> deskMembers = new List<dynamic>();
                foreach (var member in desk.Members)
                {
                    if (member.MemberType == MemberType.Folder)
                    {
                        deskMembers.Add(new
                        {
                            type = member.MemberType.ToString().ToLower(),
                            memberId = member.Id,
                            appId = member.AppId,
                            name = member.Name,
                            icon = member.IconUrl,
                            width= member.Width,
                            height = member.Height,
                            isOnDock = member.IsOnDock,
                            isDraw = member.IsDraw,
                            isOpenMax = member.IsOpenMax,
                            isSetbar = member.IsSetbar,
                            apps = desk.Members.Where(m => m.FolderId == member.Id).Select(app => new
                            {
                                type = app.MemberType.ToString().ToLower(),
                                memberId = app.Id,
                                appId = app.AppId,
                                name = app.Name,
                                icon = app.IconUrl,
                                width = app.Width,
                                height = app.Height,
                                isOnDock = app.IsOnDock,
                                isDraw = app.IsDraw,
                                isOpenMax = app.IsOpenMax,
                                isSetbar = app.IsSetbar,
                            })
                        });
                    }
                    else
                    {
                        if (member.FolderId == 0)
                        {
                            var internalType = member.MemberType.ToString().ToLower();
                            deskMembers.Add(new
                            {
                                type = internalType,
                                memberId = member.Id,
                                appId = member.AppId,
                                name = member.Name,
                                icon = member.IconUrl,
                                width = member.Width,
                                height = member.Height,
                                isOnDock = member.IsOnDock,
                                isDraw = member.IsDraw,
                                isOpenMax = member.IsOpenMax,
                                isSetbar = member.IsSetbar
                            });
                        }
                    }
                }
                desks.Add(new KeyValuePair<Int32, IList<dynamic>>(desk.DeskNumber, deskMembers));
            }

            return desks;
        }

        public void ModifyAppDirection(Int32 userId, String direction)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            if (direction.ToLower() == "x")
            {
                userResult.Config.ModifyAppDirectionToX();
            }
            else if (direction.ToLower() == "y")
            {
                userResult.Config.ModifyAppDirectionToY();
            }
            else
            {
                throw new BusinessException($"未能识别的App排列方向:{direction.ToLower()}");
            }


            _userRepository.Update(userResult);
        }

        public void ModifyAppIconSize(Int32 userId, Int32 newSize)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            userResult.Config.ModifyDisplayIconLength(newSize);
            _userRepository.Update(userResult);
        }

        public void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            userResult.Config.ModifyAppVerticalSpacingLength(newSize);
            _userRepository.Update(userResult);
        }

        public void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            userResult.Config.ModifyAppHorizontalSpacingLength(newSize);
            _userRepository.Update(userResult);
        }
    }
}
