// ==UserScript==
// @name        搜索编程问题自动排除csdn的结果
// @namespace   Violentmonkey Scripts
// @match       *://*/*
// @grant       none
// @version     1.0.2
// @author      自来也
// @description 它会在你的搜索关键词的后面自动拼接上 -csdn，来达到过滤效果
// @require   http://code.jquery.com/jquery-migrate-1.2.1.min.js
// @match      https://www.baidu.com/*
// @license MIT
// ==/UserScript==
(function(){
  'use strict';
  let url = location.href
  if(url.indexOf('baidu') > -1){
    $(":submit").click(function(){
      let oldValue = $('#kw').val()
      if(oldValue){
        oldValue = oldValue.replace(' -csdn','')
        $('#kw').val(oldValue+' -csdn')
      }
    })
  }
})();
