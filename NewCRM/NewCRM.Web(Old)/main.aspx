<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="NewCRM.Web.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>首页</title>
    <link href="HoorayUI/js/HoorayLibs/hooraylibs.css" rel="stylesheet" />
    <link href="HoorayUI/css/index.css" rel="stylesheet" />
</head>
<body>
    <div class="loading"></div>
    <div id="desktop">
	<div id="zoom-tip"><div><i>​</i>​<span></span></div><a href="javascript:;" class="close" onClick="HROS.zoom.close();">×</a></div>
	<div id="desk">
		<div id="desk-1" class="desktop-container">
			<div class="desktop-apps-container"></div>
			<div class="scrollbar scrollbar-x"></div><div class="scrollbar scrollbar-y"></div>
		</div>
		<div id="desk-2" class="desktop-container">
			<div class="desktop-apps-container"></div>
			<div class="scrollbar scrollbar-x"></div><div class="scrollbar scrollbar-y"></div>
		</div>
		<div id="desk-3" class="desktop-container">
			<div class="desktop-apps-container"></div>
			<div class="scrollbar scrollbar-x"></div><div class="scrollbar scrollbar-y"></div>
		</div>
		<div id="desk-4" class="desktop-container">
			<div class="desktop-apps-container"></div>
			<div class="scrollbar scrollbar-x"></div><div class="scrollbar scrollbar-y"></div>
		</div>
		<div id="desk-5" class="desktop-container">
			<div class="desktop-apps-container"></div>
			<div class="scrollbar scrollbar-x"></div><div class="scrollbar scrollbar-y"></div>
		</div>
		<div id="dock-bar">
			<div id="dock-container">
				<div class="dock-middle">
					<div class="dock-applist"></div>
					<div class="dock-tools-container">
						<div class="dock-tools">
							<a href="javascript:;" class="dock-tool-setting" title="桌面设置"></a>
							<a href="javascript:;" class="dock-tool-style" title="主题设置"></a>
						</div>
						<div class="dock-tools">
							<a href="javascript:;" class="dock-tool-appmanage" title="全局视图，快捷键：Ctrl + F"></a>
							<a href="javascript:;" class="dock-tool-search" title="搜索，Ctrl + F"></a>
						</div>
						<div class="dock-startbtn">
							<a href="javascript:;" class="dock-tool-start" title="点击这里开始"></a>
						</div>
					</div>
					<div class="dock-pagination">
						<a class="pagination pagination-1" href="javascript:;" index="1" title="切换至桌面1，快捷键：Ctrl + 1">
							<span class="pagination-icon-bg"></span>
							<span class="pagination-icon pagination-icon-1">1</span>
						</a>
						<a class="pagination pagination-2" href="javascript:;" index="2" title="切换至桌面2，快捷键：Ctrl + 2">
							<span class="pagination-icon-bg"></span>
							<span class="pagination-icon pagination-icon-2">2</span>
						</a>
						<a class="pagination pagination-3" href="javascript:;" index="3" title="切换至桌面3，快捷键：Ctrl + 3">
							<span class="pagination-icon-bg"></span>
							<span class="pagination-icon pagination-icon-3">3</span>
						</a>
						<a class="pagination pagination-4" href="javascript:;" index="4" title="切换至桌面4，快捷键：Ctrl + 4">
							<span class="pagination-icon-bg"></span>
							<span class="pagination-icon pagination-icon-4">4</span>
						</a>
						<a class="pagination pagination-5" href="javascript:;" index="5" title="切换至桌面5，快捷键：Ctrl + 5">
							<span class="pagination-icon-bg"></span>
							<span class="pagination-icon pagination-icon-5">5</span>
						</a>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div id="startmenu-container">
		<div class="startmenu-selfinfo">
			<a href="javascript:;" class="startmenu-feedback" title="反馈"></a>
			<a href="javascript:;" class="startmenu-lock" title="锁定，快捷键：Ctrl + L"></a>
			<div class="startmenu-avatar"><img src="HoorayUI/img/ui/loading_24.gif" /></div>
			<div class="startmenu-nick">
			<%--	<?php if(checkLogin()){ ?>
				<a href="javascript:;" title="编辑个人资料"><?php echo $db->get('tb_member', 'username', array('tbid' => session('member_id'))); ?></a>
				<?php }else{ ?>
				<a href="javascript:;">请登录</a>
				<?php } ?>--%>
			</div>
		</div>
		<ul class="startmenu">
			<li><a href="javascript:;" class="help">新手指导</a></li>
			<li><a href="javascript:;" class="about">关于 HoorayOS</a></li>
		</ul>
		<div class="startmenu-exit"><a href="javascript:;" title="注销当前用户"></a></div>
	</div>
	<div id="task-bar-bg1"></div>
	<div id="task-bar-bg2"></div>
	<div id="task-bar">
		<div id="task-next"><a href="javascript:;" id="task-next-btn" hidefocus="true"></a></div>
		<div id="task-content">
			<div id="task-content-inner"></div>
		</div>
		<div id="task-pre"><a href="javascript:;" id="task-pre-btn" hidefocus="true"></a></div>
	</div>
	<div id="search-bar">
		<input id="pageletSearchInput" class="mousetrap" placeholder="搜索应用...">
		<input type="button" value="" id="pageletSearchButton" title="搜索...">
	</div>
	<div id="search-suggest">
		<ul class="resultBox"></ul>
		<div class="resultList openAppMarket"><a href="javascript:;"><div>去应用市场搜搜...</div></a></div>
	</div>
</div>
<!-- 全局视图 -->
<div id="appmanage">
	<a class="amg_close" href="javascript:;"></a>
	<div id="amg_dock_container"></div>
	<div class="amg_line_x"></div>
	<div id="amg_folder_container">
		<div class="folderItem">
			<div class="folder_bg folder_bg1"></div>
			<div class="folderOuter">
				<div class="folderInner" desk="1"></div>
				<div class="scrollBar"></div>
			</div>
		</div>
		<div class="folderItem">
			<div class="folder_bg folder_bg2"></div>
			<div class="folderOuter">
				<div class="folderInner" desk="2"></div>
				<div class="scrollBar"></div>
			</div>
			<div class="amg_line_y"></div>
		</div>
		<div class="folderItem">
			<div class="folder_bg folder_bg3"></div>
			<div class="folderOuter">
				<div class="folderInner" desk="3"></div>
				<div class="scrollBar"></div>
			</div>
			<div class="amg_line_y"></div>
		</div>
		<div class="folderItem">
			<div class="folder_bg folder_bg4"></div>
			<div class="folderOuter">
				<div class="folderInner" desk="4"></div>
				<div class="scrollBar"></div>
			</div>
			<div class="amg_line_y"></div>
		</div>
		<div class="folderItem">
			<div class="folder_bg folder_bg5"></div>
			<div class="folderOuter">
				<div class="folderInner" desk="5"></div>
				<div class="scrollBar"></div>
			</div>
			<div class="amg_line_y"></div>
		</div>
	</div>
</div>
<div id="copyright">
	<a href="javascript:;" class="close" title="关闭"></a>
	<div class="title">HoorayOS</div>
	<div class="body">
		<p>这是一款备受好评的 Web 桌面应用框架，你可以用它二次开发出类似 Q+Web 这类的桌面应用网站，也可以开发出适合各种项目的桌面管理系统。</p>
		<p>官网：<a href="http://hoorayos.com" target="_blank">http://hoorayos.com</a></p>
		<p>购买或定制请联系 QQ：<a href="http://wpa.qq.com/msgrd?v=3&uin=304327508&site=qq&menu=yes" target="_blank">304327508</a></p>
	</div>
</div>
    <script src="HoorayUI/js/jquery-1.8.3.min.js"></script>
    <script src="HoorayUI/js/HoorayLibs/hooraylibs.js"></script>
    <script src="HoorayUI/js/Validform_v5.3.2/Validform_v5.3.2_min.js"></script>
    <script src="HoorayUI/js/sugar/sugar-1.4.1.min.js"></script>
    <script src="HoorayUI/js/hros.core.js"></script>
    <script src="HoorayUI/js/hros.app.js"></script>
    <script src="HoorayUI/js/hros.appmanage.js"></script>
    <script src="HoorayUI/js/hros.base.js"></script>
    <script src="HoorayUI/js/hros.copyright.js"></script>
    <script src="HoorayUI/js/hros.desktop.js"></script>
    <script src="HoorayUI/js/hros.dock.js"></script>
    <script src="HoorayUI/js/hros.folderView.js"></script>
    <script src="HoorayUI/js/hros.grid.js"></script>
    <script src="HoorayUI/js/hros.hotkey.js"></script>
    <script src="HoorayUI/js/hros.lock.js"></script>
    <script src="HoorayUI/js/hros.maskBox.js"></script>
    <script src="HoorayUI/js/hros.popupMenu.js"></script>
    <script src="HoorayUI/js/hros.searchbar.js"></script>
    <script src="HoorayUI/js/hros.startmenu.js"></script>
    <script src="HoorayUI/js/hros.taskbar.js"></script>
    <script src="HoorayUI/js/hros.templates.js"></script>
    <script src="HoorayUI/js/hros.wallpaper.js"></script>
    <script src="HoorayUI/js/hros.widget.js"></script>
    <script src="HoorayUI/js/hros.window.js"></script>
    <script src="HoorayUI/js/hros.zoom.js"></script>
    <script src="HoorayUI/js/artDialog4.1.7/jquery.artDialog.js?skin=default"></script>
    <script src="HoorayUI/js/artDialog4.1.7/plugins/iframeTools.js"></script>
</body>
</html>
