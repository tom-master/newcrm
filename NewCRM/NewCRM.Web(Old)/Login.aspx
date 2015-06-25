<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NewCRM.Web.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>NewCRM</title>
    <%--  <link href="HoorayUI/img/ui/index.css" rel="stylesheet" />--%>
    <link href="HoorayUI/css/index2.css" rel="stylesheet" />
</head>  
<body>
    <form id="form1"  runat="server">
    <div class="loginmask"></div>
    <div class="loading"></div>
    <div class="login">
        <div class="logo">NewCRM</div>
        <div class="input">
            <div class="log">
                <div class="name">
                    <input type="text" id="username" placeholder="用户名" name="username" tabindex="1" />
                </div>
                <div class="pwd">
                    <input type="password" id="password" placeholder="密码" name="password" tabindex="2" /><input type="button" class="submit" tabindex="3"  /><div class="check"></div>
                </div>
                <div class="tip"></div>
            </div>
        </div>
    </div>
   </form>
    <script src="HoorayUI/js/jquery-1.8.1.js"></script>
    <%--<script src="HoorayUI/js/HoorayLibs/hooraylibs.js"></script>--%>
    <script>
        $(function () {
            setTimeout(function () { $('#username').val('').focus(); }, 500);
            //IE6升级提示
            if ($.browser.msie && $.browser.version < 8) {
                if ($.browser.version < 7) {
                    DD_belatedPNG.fix('.update_browser .browser');
                }
                $('.login').html('<div class="update_browser">' +
                    '<div class="subtitle">您正在使用的IE浏览器版本过低，<br>我们建议您升级或者更换浏览器，以便体验顺畅、兼容、安全的互联网。</div>' +
                    '<div class="title">选择一款<span>新</span>浏览器吧</div>' +
                    '<div class="browser">' +
                    '<a href="http://windows.microsoft.com/zh-CN/internet-explorer/downloads/ie" class="ie" target="_blank" title="ie浏览器">ie浏览器</a>' +
                    '<a href="http://www.google.cn/chrome/intl/zh-CN/landing_chrome.html" class="chrome" target="_blank" title="谷歌浏览器">谷歌浏览器</a>' +
                    '<a href="http://www.firefox.com.cn" class="firefox" target="_blank" title="火狐浏览器">火狐浏览器</a>' +
                    '<a href="http://www.opera.com" class="opera" target="_blank" title="opera浏览器">opera浏览器</a>' +
                    '<a href="http://www.apple.com.cn/safari" class="safari" target="_blank" title="safari浏览器">safari浏览器</a>' +
                    '</div>' +
                    '<div class="bottomtitle">[&nbsp;<a href="http://www.theie6countdown.cn" target="_blank">对IE6说再见</a>&nbsp;]</div>' +
                    '</div>');
            }

            $(".input .log").bind("keyup", function (e) {
                if (e.keyCode == 13) {
                    $('.log .submit').click();
                }
            });
            $('.log .submit').click(function () {
                if ($('#username').val() != "" && $('#password').val() != "") {
                    $('.log .submit').hide();
                    $('.log .check').show();
                    $('.log .tip').text('').hide();
                    var result = window.NewCRM.Web.Login.UserLogin($('#username').val(), $('#password').val());
                    if (result.value==1) {
                        $('.loading').hide();
                        $('.loginmask').fadeIn(500, function () {
                            location.href = 'main.aspx';
                        });
                    } else {
                        $('.log .tip').text('用户名或密码错误').show();
                    }
                }
            });
            $('.loading').fadeOut(500);
            $('.login').show();
        });
    </script>
</body>
</html>

