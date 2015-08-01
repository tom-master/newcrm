//F.ready(function() {

//    var treeMenu = F(DATA.treeMenu),
//        regionPanel = F(DATA.regionPanel),
//        regionTop = F(DATA.regionTop),
//        btnShowHideHeader = F(DATA.btnShowHideHeader),
//        mainTabStrip = F(DATA.mainTabStrip),
//        txtUser = F(DATA.txtUser),
//        txtOnlineUserCount = F(DATA.txtOnlineUserCount),
//        txtCurrentTime = F(DATA.txtCurrentTime),
//        btnRefresh = F(DATA.btnRefresh);


//    // 欢迎信息和在线用户数
//    txtUser.setText('欢迎您：<span class="highlight">' + DATA.userName + '</span>&nbsp;&nbsp;[' + DATA.userIP + ']');
//    txtOnlineUserCount.setText('在线用户：' + DATA.onlineUserCount);

//    // 点击刷新按钮
//    btnRefresh.on('click', function () {
//        var iframe = Ext.DomQuery.selectNode('iframe', mainTabStrip.getActiveTab().body.dom);
//        iframe.src = iframe.src;
//    });

//    // 显示 / 隐藏标题
//    btnShowHideHeader.on('click', function () {
//        if (regionTop.getInnerHeight() > 0) {
//            regionTop.setHeight(0);
//            btnShowHideHeader.setTooltip('显示标题栏');
//            btnShowHideHeader.setIcon('res.axd?icon=SectionCollapsed');
//        } else {
//            regionTop.setHeight(60);
//            btnShowHideHeader.setTooltip('隐藏标题栏');
//            btnShowHideHeader.setIcon('res.axd?icon=SectionExpanded');
//        }
//        regionPanel.doLayout();
//    });


//    // 设置当前时间
//    function setCurrentTime() {
//        var today = new Date();
//        var year = today.getFullYear().toString();
//        var month = leftPadTime(today.getMonth() + 1);
//        var date = leftPadTime(today.getDate());
//        var hour = leftPadTime(today.getHours());
//        var minute = leftPadTime(today.getMinutes());
//        var second = leftPadTime(today.getSeconds());
//        txtCurrentTime.setText('<span class="label">当前时间：</span>' + year + '-' + month + '-' + date + ' ' + hour + ':' + minute + ':' + second);
//    }
//    // 当前时间并定时更新
//    setCurrentTime();
//    window.setInterval(setCurrentTime, 30 * 1000);




//    // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
//    // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
//    // mainTabStrip： 选项卡实例
//    // addTabCallback： 创建选项卡前的回调函数（接受tabConfig参数）
//    // updateLocationHash: 切换Tab时，是否更新地址栏Hash值
//    // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
//    // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame
//    F.util.initTreeTabStrip(treeMenu, mainTabStrip, null, true, false, false);



//    // 公开添加示例标签页的方法
//    window.addExampleTab = function (id, url, text, icon, refreshWhenExist) {
//        // 动态添加一个标签页
//        // mainTabStrip： 选项卡实例
//        // id： 选项卡ID
//        // url: 选项卡IFrame地址 
//        // text： 选项卡标题
//        // icon： 选项卡图标
//        // addTabCallback： 创建选项卡前的回调函数（接受tabConfig参数）
//        // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
//        F.util.addMainTab(mainTabStrip, id, url, text, icon, null, refreshWhenExist);
//    };

//    window.removeActiveTab = function () {
//        var activeTab = mainTabStrip.getActiveTab();
//        mainTabStrip.removeTab(activeTab.id);
//    };

//});

F.ready(function () {

    var treeMenu = Ext.getCmp(DATA.treeMenu),
        regionPanel = Ext.getCmp(DATA.regionPanel),
        regionTop = Ext.getCmp(DATA.regionTop),
        mainTabStrip = Ext.getCmp(DATA.mainTabStrip),
        txtUser = Ext.getCmp(DATA.txtUser),
        txtOnlineUserCount = Ext.getCmp(DATA.txtOnlineUserCount),
        txtCurrentTime = Ext.getCmp(DATA.txtCurrentTime),
        btnRefresh = Ext.getCmp(DATA.btnRefresh);


    // 欢迎信息和在线用户数
    txtUser.setText('<span class="label">欢迎您：</span><span>' + DATA.userName + '</span>&nbsp;&nbsp;[' + DATA.userIP + ']');
    txtOnlineUserCount.setText('<span class="label">在线用户：</span>' + DATA.onlineUserCount);

    // 点击刷新按钮
    btnRefresh.on('click', function () {
        var iframe = Ext.DomQuery.selectNode('iframe', mainTabStrip.getActiveTab().body.dom);
        iframe.contentWindow.location.reload(false);
    });

    function leftPad(source, count, prefix) {
        source += '';
        if (source.length < count) {
            for (var i = source.length; i < count; i++) {
                source = prefix + source;
            }
        }
        return source;
    }

    function leftPadTime(source) {
        return leftPad(source, '2', '0');
    }

    // 设置当前时间
    function setCurrentTime() {
        var today = new Date();
        var year = today.getFullYear().toString();
        var month = leftPadTime(today.getMonth() + 1);
        var date = leftPadTime(today.getDate());
        var hour = leftPadTime(today.getHours());
        var minute = leftPadTime(today.getMinutes());
        var second = leftPadTime(today.getSeconds());
        txtCurrentTime.setText('<span class="label">当前时间：</span>' + year + '-' + month + '-' + date + ' ' + hour + ':' + minute + ':' + second);
    }
    // 当前时间并定时更新
    setCurrentTime();
    window.setInterval(setCurrentTime, 5000);




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