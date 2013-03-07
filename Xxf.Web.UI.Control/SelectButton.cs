using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Xxf.Web.UI.Control
{
    public class SelectButton : System.Web.UI.WebControls.Button, IClientScriptControl
    {
        [TypeConverter (typeof (ControlIDConverter ))]
        public string PageBreakControl { get; set; }
        /// <summary>
        /// 暂时指定为tab页的容器控件
        /// </summary>
        [TypeConverter(typeof(ControlIDConverter))]
        public string ContainerControl { get; set; }

        #region IClientScriptControl 成员

        public string GetScript()
        {
            var pbControl = this.Page.FindControl(PageBreakControl);
            if (pbControl == null)
            {
                throw new ArgumentNullException("请指定正确的pagebreak控件ID");
            }
            var container = this.Page.FindControl(ContainerControl);
            if (container == null)
            {
                throw new ArgumentNullException("请指定正确的container控件ID");
            }
            string pbid = pbControl.ClientID;
            StringBuilder sb = new StringBuilder();
            //生成脚本
            sb.AppendFormat("function {0}(){{", GetClientFuncName());
            foreach (var control in container.Controls)
            {
                ISelectParamControl pc=control  as ISelectParamControl ;
                if (pc!=null )
                {
                    sb.AppendFormat ("{0}.param{1}",pbid,pc.GetParamQuery() );
                }
            }
            sb.AppendFormat("{0}.setPageIndex(0);}}", pbid);
            return sb.ToString();
        }

        #endregion
        private string GetClientFuncName()
        {
            return this.ClientID + "_select";
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, GetClientFuncName ()+"();return false;");
            base.RenderControl(writer);
        }
    }
}
