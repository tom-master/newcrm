F.ready(function() {

    var treeMenu = F(DATA.treeMenu),
        regionPanel = F(DATA.regionPanel),
        regionTop = F(DATA.regionTop),
        btnShowHideHeader = F(DATA.btnShowHideHeader),
        mainTabStrip = F(DATA.mainTabStrip),
        txtUser = F(DATA.txtUser),
        txtOnlineUserCount = F(DATA.txtOnlineUserCount),
        txtCurrentTime = F(DATA.txtCurrentTime),
        btnRefresh = F(DATA.btnRefresh);


    // 欢迎信息和在线用户数
    txtUser.setText('欢迎您：<span class="highlight">' + DATA.userName + '</span>&nbsp;&nbsp;[' + DATA.userIP + ']');
    txtOnlineUserCount.setText('在线用户：' + DATA.onlineUserCount);

    // 点击刷新按钮
    btnRefresh.on('click', function () {
        var iframe = Ext.DomQuery.selectNode('iframe', mainTabStrip.getActiveTab().body.dom);
        iframe.src = iframe.src;
    });

    // 显示 / 隐藏标题
    btnShowHideHeader.on('click', function () {
        if (regionTop.getInnerHeight() > 0) {
            regionTop.setHeight(0);
            btnShowHideHeader.setTooltip('显示标题栏');
            btnShowHideHeader.setIcon('res.axd?icon=SectionCollapsed');
        } else {
            regionTop.setHeight(60);
            btnShowHideHeader.setTooltip('隐藏标题栏');
            btnShowHideHeader.setIcon('res.axd?icon=SectionExpanded');
        }
        regionPanel.doLayout();
    });


    // 设置当前时间
    function setCurrentTime() {
        var today = new Date();
        year = today.getFullYear().toString();
        month = String.leftPad(today.getMonth() + 1, '2', '0');
        date = String.leftPad(today.getDate(), '2', '0');
        hour = String.leftPad(today.getHours(), '2', '0');
        minute = String.leftPad(today.getMinutes(), '2', '0');
        //second = string.leftPad(today.getSeconds(), '2', '0');
        txtCurrentTime.setText('当前时间：' + year + '-' + month + '-' + date + ' ' + hour + ':' + minute);
    }
    // 当前时间并定时更新
    setCurrentTime();
    window.setInterval(setCurrentTime, 30 * 1000);




    // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
    // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
    // mainTabStrip： 选项卡实例
    // addTabCallback： 创建选项卡前的回调函数（接受tabConfig参数）
    // updateLocationHash: 切换Tab时，是否更新地址栏Hash值
    // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
    // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame
    F.util.initTreeTabStrip(treeMenu, mainTabStrip, null, true, false, false);



    // 公开添加示例标签页的方法
    window.addExampleTab = function (id, url, text, icon, refreshWhenExist) {
        // 动态添加一个标签页
        // mainTabStrip： 选项卡实例
        // id： 选项卡ID
        // url: 选项卡IFrame地址 
        // text： 选项卡标题
        // icon： 选项卡图标
        // addTabCallback： 创建选项卡前的回调函数（接受tabConfig参数）
        // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
        F.util.addMainTab(mainTabStrip, id, url, text, icon, null, refreshWhenExist);
    };

    window.removeActiveTab = function () {
        var activeTab = mainTabStrip.getActiveTab();
        mainTabStrip.removeTab(activeTab.id);
    };

});