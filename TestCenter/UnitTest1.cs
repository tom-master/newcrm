using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using Nito.AsyncEx;

namespace TestCenter
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var include = $@"data[*].is_normal,admin_closed_comment,reward_info,is_collapsed,annotation_action,annotation_detail,collapse_reason,is_sticky,collapsed_by,suggest_edit,comment_count,can_comment,content,editable_content,voteup_count,reshipment_settings,comment_permission,created_time,updated_time,review_info,question,excerpt,relationship.is_authorized,is_author,voting,is_thanked,is_nothelp,upvoted_followees;data[*].mark_infos[*].url;data[*].author.follower_count,badge[?(type=best_answerer)].topics";
            var url = $@"https://www.zhihu.com/api/v4/questions/34243513/answers?{include}&offset=2&limit=20";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("authorization", "oauth c3cef7c66a1843f8b3a9e6a1e3160e20");




            var response = AsyncContext.Run(() => httpClient.GetAsync(url));
            var message = AsyncContext.Run(() => response.Content.ReadAsStringAsync());
        }
    }
}
