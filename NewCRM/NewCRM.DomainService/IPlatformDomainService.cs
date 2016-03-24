using System;
using System.Collections.Generic;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.DomainService
{
    public interface IPlatformDomainService
    {
        User VaildateUser(String userName, String passWord);

        dynamic UserApp(Int32 userId);

        Boolean SetDefaultDesk(Int32 userId, Int32 deskId);

        Boolean SetAppDirection(Int32 userId, String direction);

        Boolean SetAppSize(Int32 userId, Int32 appSize);

        Boolean SetAppVertical(Int32 userId, Int32 appVertical);

        Boolean SetAppHorizontal(Int32 userId, Int32 appHorizontal);

        Boolean SetDockPosition(Int32 userId, String pos, Int32 deskNum);
        Boolean SetSkin(Int32 userId, String skin);

        Boolean SetWebWallpaper(Int32 userId, dynamic imageValue);

        IDictionary<Int32, Tuple<String, String>> GetWallpapers();
    }
}
