/*
*Xxf模块，该模块以JQUERY为基础
*/
var Xxf;
if (Xxf) { throw new Error('Xxf命名空间与全局变量冲突'); }
//Xxf顶级命名空间
Xxf = {};

Xxf.ErrorHtml = "";
Xxf.AjaxError = function () {
    var errorPage = window.open('');
    errorPage.document.write(Xxf.ErrorHtml);
}

//Xxf.Common命名空间，保存一些公共的工具对象和函数
Xxf.Common = {};
//StringBuilder类
Xxf.Common.StringBuilder = function () {
    this._strings = new Array();
}
Xxf.Common.StringBuilder.prototype.append = function (str) {
    this._strings.push(str);
}
Xxf.Common.StringBuilder.prototype.toString = function () {
    return this._strings.join("");
};



//Xxf.ListPage命名空间，保存用于在列表页面中用到的变量和常用函数
Xxf.ListPage = {};
//保存选中的行的主键（Id）
Xxf.ListPage.selectedIds = new Array();
//保存最近选中的控件的对象
Xxf.ListPage.currChk;


 Xxf.Checks = function(tbId) {
     this.chks = [];
     this.tb = $("#" + tbId);
     this.currChk = null;
 }
 Xxf.Checks.prototype.multiCheck = function(id,obj) {
     var cb = $(obj);
     if (cb.attr("checked") == true) {
         this.chks.push(id);
         this.currChk = cb;
     }
     else {
         for (i = 0; i < this.chks.length; i++) {
             if (this.chks[i] == id) {
                 this.chks.splice(i, 1);
             }
         }
     }
 }

 //用于Radio的单选
 Xxf.Checks.prototype.singleCheck = function (id, obj) {
     var cb = $( obj);
     if (cb.attr("checked") == true) {
         this.chks[0] = id;
         this.currChk = cb;
     }
 }

 //全选　全不选
 Xxf.Checks.prototype.checkAll = function(chk) {
    var ddl = $(chk);
     if (ddl.attr("checked") == true) {
         $('input:checkbox').each(function() {
             if ($(this).attr("checked") == false) {
                 $(this).click();
             }
         });
     }
     else {
         $('input:checkbox[checked]').attr("checked", false);
         this.chks.length = 0;
     }
 }

 Xxf.Checks.prototype.getValues = function () {
     this.chks.length = 0;
     var list = this.chks;
     this.tb.find('input').each(function () {
        var ck=$(this);
         if (ck.attr("checked") == true ) {
             if (ck.attr("key") != undefined)
                list.push(ck.attr("key"));
         }
     });
 }


 Xxf.Tab = function(div) {
     this.isLoaded = false;
     this.divObj = $("#" + div);
     //
     this.fns = [];
     this.fnObjs = [];
 }
 Xxf.Tab.CurrDiv = null;
 Xxf.Tab.prototype = {
     show: function() {
         if (this.divObj.is(":visible") == true)
             return;
         this.divObj.show();
         if (Xxf.Tab.CurrDiv != null) {
             Xxf.Tab.CurrDiv.hide();
         }
         Xxf.Tab.CurrDiv = this.divObj;
         if (this.isLoaded == false) {
             this.update();
             this.isLoaded = true;
         }

     },
     subscribe: function(fn, obj) {
         this.fns.push(fn);
         this.fnObjs.push(obj);
     },
     update: function(o, thisObj) {
         var scope = thisObj || window;
         for (var i = 0; i < this.fns.length; i++) {
             this.fns[i].call(this.fnObjs[i], o);
         }
     }
 }


 Xxf.PageBreak = function (id, size, index, count, repeaterId) {
     this.pageSizeCode = "xxf_pagesize";
     this.pageIndexCode = "xxf_pageindex";
     this.sortFieldCode = "xxf_sortfield";
     this.sortRuleCode = "xxf_sortrule";
     this.clientId = id;
     this.pageSize = 10; //-----
     this.pageIndex = index;
     this.dataCount = count;
     this.pageCount = Math.ceil(this.dataCount / this.pageSize);
     //排序URL
     this.sortQuery = "";
     //上一次排序的字段名称
     this.currSortField = "";
     //排序规格，降序OR升序
     this.sortRule = 0;
     this.repeaterId = repeaterId;
     this.pageName = window.location.href;
     this.param = {};
     this.param["__EVENTTARGET"] = this.repeaterId;
     this.param[this.repeaterId] = "";

     this.hide = true;
     this.simpleWidth = 0;
     this.detailWidth = 0;
 }

 Xxf.PageBreak.prototype.dataBind = function () {
     var obj = this;
     this.param[this.pageSizeCode] = this.pageSize;
     this.param[this.pageIndexCode] = this.pageIndex;
     this.param[this.sortFieldCode] = this.currSortField;
     this.param[this.sortRuleCode] = this.sortRule;
     $.ajax({
         type: "POST",
         url: obj.pageName,
         dataType: "json",
         cache: false,
         data: obj.param,
         success: function (data) {
             var respon = eval(data);
             var div = $(respon.Html);
             if (obj.hide) {
                 div.find('.hide').hide();
                 if (obj.simpleWidth != 0)
                     div.find('table').attr('width', obj.simpleWidth);
                 $("#" + obj.repeaterId).empty().append(div);
             }
             else {
                 if (obj.detailWidth != 0)
                     div.find('table').attr('width', obj.detailWidth);
                 $("#" + obj.repeaterId).empty().append(div);
             }

             obj.dataCount = respon.DataCount;
             obj.pageCount = Math.ceil(obj.dataCount / obj.pageSize);
             obj.init();
             //隔行变色、鼠标移过变色
             obj.tableInit();
             obj.sortInit();
         },
         error: function (XMLHttpRequest, textStatus, errorThrown) { alert("error: " + XMLHttpRequest.status + ' ' + textStatus); Xxf.ErrorHtml = XMLHttpRequest.responseText; ; }
     });
     return false;
 }
 Xxf.PageBreak.prototype.init = function () {
     $("#" + this.clientId + "_currpage").val(1 + this.pageIndex);
     $("#" + this.clientId + "_pagesize").val(this.pageSize);
     $("#" + this.clientId + "_datacount").html(this.dataCount);
     $("#" + this.clientId + "_pagecount").html(this.pageCount);
     var pb = $("#"+this.clientId);
     var first = pb.find(".first");
     pb.find(".dynamic").remove();
     var actIndex = this.pageIndex + 1;
     var current = $('<span class="cpb dynamic" style="margin-right:5px;">' + actIndex + '</span>');
     first.after(current);
     if (this.pageIndex + 1 < this.pageCount) {
         var next = this.pageIndex + 1;
         var shit = next + 1;
         var next1 = $('<a class="paginator dynamic" style="margin-right: 5px;" href="javascript:pb1.setPageIndex(' + next + ')">' + shit + '</a>');
         current.after(next1);
         if (this.pageIndex + 2 < this.pageCount) {
             var next = this.pageIndex + 2;
             var shit = next + 1;
             next1.after('<a class="paginator dynamic" style="margin-right: 5px;" href="javascript:pb1.setPageIndex(' + next + ')">' + shit + '</a>');
         }
     }
     if (this.pageIndex - 1 >= 0) {
         var prev = this.pageIndex - 1;
         var shit = prev + 1;
         var prev1 = $('<a class="paginator dynamic" style="margin-right: 5px;" href="javascript:pb1.setPageIndex(' + prev + ')">' + shit + '</a>');
         first.after(prev1);
         if (this.pageIndex - 2 >= 0) {
             var prev = this.pageIndex - 2;
             var shit = prev + 1;
             var prev1 = $('<a class="paginator dynamic" style="margin-right: 5px;" href="javascript:pb1.setPageIndex(' + prev + ')">' + shit + '</a>');
             first.after(prev1);
         }
     }
 }
 Xxf.PageBreak.prototype.firstPage = function () {
     if (this.pageIndex == 0)
         return;
     this.pageIndex = 0;
     this.dataBind(this.selectQuery);
 }
 Xxf.PageBreak.prototype.nextPage = function () {
     if (this.pageIndex == (this.pageCount - 1))
         return;
     this.pageIndex++;
     this.dataBind();
 }
 Xxf.PageBreak.prototype.prevPage = function () {
     if (this.pageIndex == 0)
         return;
     this.pageIndex--;
     this.dataBind();
 }
 Xxf.PageBreak.prototype.lastPage = function () {
     if (this.pageIndex == (this.pageCount - 1))
         return;
     this.pageIndex = this.pageCount - 1;
     this.dataBind();
 }
 Xxf.PageBreak.prototype.setPageSize = function (obj) {
     this.pageSize = $("#" + this.clientId + "_pagesize").val();
     this.dataBind();
 }
 Xxf.PageBreak.prototype.setPageIndex = function (index) {
     this.pageIndex = index;
     this.dataBind();
 }
 Xxf.PageBreak.prototype.sort = function (field) {
     if (field == this.currSortField) {
         if (this.sortRule == 1)
             this.sortRule = 0;
         else
             this.sortRule = 1;
     }
     this.currSortField = field;
     this.dataBind();
 }

 Xxf.PageBreak.prototype.tableInit = function () {
     var index = 0;
     $("#" + this.repeaterId).find("tr").each(function () {
         index++;
         if (index == 1)
             return true;
         var cls = "in2";
         if (index % 2 == 0)
             cls = "in3";
         $(this).attr("class", cls).hover(function () {
             $(this).addClass('JZhr');
         }, function () {
             $(this).removeClass('JZhr');
         }); ;
     });
 }

 Xxf.PageBreak.prototype.sortInit = function () {
     var control = $("#" + this.repeaterId).children().eq(0);
     var obj = this;
     if (control.length <= 0)
         return;
     var thRow = control.children().eq(0).find("th[field]");
     thRow.each(function () {
         var th = $(this);
         th.css({ 'cursor': 'hand' });
         var field = th.attr("field");
         $(this).bind("click", function () {
             obj.sort(field);
         });
     });
 }

 Xxf.PageBreak.prototype.showDetail = function (width) {
     $("#" + this.repeaterId).find('.hide').show();
     $("#" + this.repeaterId).find('table').attr('width', width);
     this.detailWidth = width;
 }
 Xxf.PageBreak.prototype.showSimple = function (width) {
     $("#" + this.repeaterId).find('.hide').hide();
     $("#" + this.repeaterId).find('table').attr('width', width);
     this.simpleWidth = width;
 }


 //--------------------------------------------------
 //credit project

 Xxf.fromShowError = function (obj, message) {
     if (obj.next().length==0) {
         obj.parent().append("<span style ='color :Red' >" + message + "</span>");
     }
     obj.bind("click", function () {
         obj.next().remove();
         obj.unbind("click");
     });
 }

 Xxf.radioValidate = function (objId,message) {
     var radio = $("#" + objId);
     var items = radio.find('input[name='+objId+']');
     for (var i = 0; i < items.length; i++) {
         if ($(items[i]).attr("checked") == true) {
             return true;
         }
     }
     Xxf.fromShowError(radio, message);
     return false;
 }