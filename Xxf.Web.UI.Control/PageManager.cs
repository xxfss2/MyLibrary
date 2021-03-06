﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Xxf.Web.UI.Control
{
    /// <summary>
    /// 查询表格页面的客户端框架管理控件
    /// 改控件统一操作分页、查询等控件
    /// </summary>
    public class PageManager : System.Web.UI.Control
    {

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //判断是否有缓存
            string key = Page.Request.FilePath;
            string script = ScriptCache.GetScript(key);
            if (string.IsNullOrEmpty(script))
            {
                StringBuilder sbAllScript = new StringBuilder();

                //查找在页面中需要向客户端输出脚本的控件,并统一输出所有脚本
                this.GetChildControlScript(sbAllScript, this.Page.Form);
                script = sbAllScript.ToString();
                this.Page.ClientScript.RegisterStartupScript(Page.GetType(), this.ClientID, script, true);

                ScriptCache.AddScript(key, script);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(Page.GetType(), this.ClientID, script, true);
            }
        }

        private void GetChildControlScript(StringBuilder sb, System.Web.UI.Control control)
        {
            var controls = control.Controls;
            if (controls.Count > 0)
            {
                StringBuilder sb2 = new StringBuilder();
                foreach (var child in controls)
                {
                    this.GetChildControlScript(sb, child as System.Web.UI.Control);
                }
                return;
            }
            TabRadio scc = control as TabRadio;
            if (scc != null)
            {
                sb.AppendLine(scc.GetScript());
            }
            ISelectButtonControl selectButton = control as ISelectButtonControl;
            if (selectButton != null)
                sb.AppendLine(selectButton.GetScript());
            return;
        }
    }
}
