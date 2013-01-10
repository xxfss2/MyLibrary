using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Xxf.Web.UI.Control
{
    public class SelectButton : System.Web.UI.WebControls.LinkButton, IClientScriptControl
    {
        [TypeConverter (typeof (ControlIDConverter ))]
        public string PageBreakControl { get; set; }

        #region IClientScriptControl 成员

        public string GetScript()
        {
            var pbControl = this.Page.FindControl(PageBreakControl);
            if (pbControl == null)
            {
                throw new ArgumentNullException("请指定正确的pagebreak控件ID");
            }
            string pbid = pbControl.ClientID;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("function {0}(){{", GetClientFuncName());
            foreach (var control in this.Parent.Controls)
            {
                ISelectParamControl pc=control  as ISelectParamControl ;
                if (pc!=null )
                {
                    sb.AppendFormat ("{0}.param{1}",pbid,pc.GetParamQuery() );
                }
            }
            sb.AppendFormat("{0}.dataBind();}}", pbid);
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
