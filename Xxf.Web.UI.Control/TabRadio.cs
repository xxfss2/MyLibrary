using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Xxf.Web.UI.Control
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TabRadio runat=server></{0}:TabRadio>")]
    public class TabRadio : RadioButton ,IClientScriptControl 
    {
        /// <summary>
        /// 与该分页控件关联的表格控件
        /// </summary>
        [TypeConverter(typeof(ControlIDConverter))]
        public string PageBreakControl
        {
            get;
            set;
        }
        private string MyClientId;
        protected override void OnLoad(EventArgs e)
        {
            MyClientId = "_" + this.ClientID;
            base.OnLoad(e);
        }

        [TypeConverter(typeof(ControlIDConverter))]
        public string DivId { get; set; }

        /// <summary>
        /// 是否隐藏，用于在客户端隐藏，但输出控件，与VISABLE不同
        /// </summary>
        [TypeConverter(typeof(BooleanConverter))]
        [DefaultValue (true )]
        public bool Hidden
        {
            get;
            set;
        }


        public override void RenderControl(HtmlTextWriter writer)
        {
            this.InputAttributes .Add ("onclick", MyClientId + ".show()");
            if (Hidden == false)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            var div = this.Page.FindControl(DivId);
            if (div == null)
            {
                throw new InvalidOperationException("DIV id错误");
            }
            (div as IAttributeAccessor).SetAttribute("style", "display:none;");
            base.RenderControl(writer);
        }

       
        #region IClientScriptControl 成员

        public string GetScript()
        {
            var div=this.Page.FindControl(DivId) ;
            if(div ==null )
            {
                throw new InvalidOperationException ("DIV id错误");
            }

            StringBuilder sb=new StringBuilder ();
            sb.AppendFormat("var {0} = new Xxf.Tab(\"{1}\");", MyClientId, DivId);
            var asynControls = div.Controls;
            foreach (var item in asynControls )
            {
                var scriptControl = item as IClientScriptControl;
                if (scriptControl == null)
                    continue;
                else
                    sb.Append(scriptControl.GetScript());
                var asynControl=item as IAsynBindControl;
                if (asynControl == null)
                    continue;
                else
                    sb.AppendFormat("{0}.subscribe({1});", MyClientId, asynControl.GetBindScript());
            }
            if (this.Checked)
                sb.AppendFormat("$(document).ready(function(){{{0}.show();}});", MyClientId);
            return sb.ToString ();
        }

        #endregion
    }
}
