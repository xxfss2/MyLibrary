using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Xxf.Web.UI;
namespace Xxf.Web.UI.Control
{
    public class Repeater : System.Web.UI.WebControls.Repeater, IPostBackEventHandler
    {
        public event AsynBindHandler AsynBind;
        /// <summary>
        /// 是否再次输出HTML，再次输出时，不套DIV
        /// </summary>
        private bool IsReRender = false;
        /// <summary>
        /// 第一次访问页面输出控件HTML时，在外面套一个DIV
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (IsReRender)
                base.RenderControl(writer);
            else
            {
                writer.AddAttribute("id", this.ClientID);
                writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, "15px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "15px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                base.RenderControl(writer);
                writer.RenderEndTag();
            }
        }
 
        public void DataBind(int dataCount)
        {
            IsReRender = true;
            base.DataBind();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter tw = new System.IO.StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(tw);
            this.RenderControl(htw);

            AsyncBindResponse asyncBind = new AsyncBindResponse();
            asyncBind.DataCount = dataCount;
            asyncBind.Html = sb.ToString();
            asyncBind.Success = true;
            asyncBind.AsyncResponse();
        }
        private void OnAsynBind()
        {
            if (AsynBind != null)
            {
                IsReRender = true;
                AsynBind(this,EventArgs .Empty );

            }
        }

        #region IPostBackEventHandler 成员

        public void RaisePostBackEvent(string eventArgument)
        {
            OnAsynBind();
        }

        #endregion
    }
}
