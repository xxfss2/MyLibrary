/*
*Xxf模块，该模块以JQUERY为基础
*/
var Xxf;
if (Xxf) { throw new Error('Xxf命名空间与全局变量冲突'); }
//Xxf顶级命名空间
Xxf = {};



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
 Xxf.Checks.prototype.multiCheck = function(id, cbId) {
     var cb = $("#" + cbId);
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
 Xxf.Checks.prototype.singleCheck = function (id, cbId) {
     var cb = $("#" + cbId);
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
     this.pageSize = size;
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
 }

 Xxf.PageBreak.prototype.dataBind = function() {
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
         success: function(data) {
             $("#" + obj.repeaterId).html(data[0]);
             obj.dataCount = data[1];
             obj.pageCount = Math.ceil(obj.dataCount / obj.pageSize);
             $("#" + obj.clientId + "_currpage").val(1 + obj.pageIndex);
             $("#" + obj.clientId + "_pagesize").val(obj.pageSize);
             $("#" + obj.clientId + "_datacount").html(obj.dataCount);
             $("#" + obj.clientId + "_pagecount").html(obj.pageCount);
             //隔行变色、鼠标移过变色
             obj.tableInit();
             obj.sortInit();
         },
         error: function(XMLHttpRequest, textStatus, errorThrown) { alert(XMLHttpRequest); }
     });
     return false;
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
 Xxf.PageBreak.prototype.setPageIndex = function() {
 this.pageIndex = parseInt($("#" + this.clientId + "_currpage").val()) - 1;
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