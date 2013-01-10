using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

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
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                base.RenderControl(writer);
                writer.RenderEndTag();
            }
        }
 
        public void DataBind(int dataCount)
        {
            IsReRender = true;
            base.DataBind();
            HttpContext.Current.Response.Clear();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.StringWriter tw = new System.IO.StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(tw);

            this.RenderControl(htw);
            string[] result = new string[2];
            result[0] = sb.ToString();
            result[1] = dataCount.ToString();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string resultStr = serializer.Serialize(result);
            Context.Response.ContentType = "application/json;charset=UTF-8";
            Context.Response.Write(resultStr);
            Context.Response.End();
        }
        private void OnAsynBind()
        {
            if (AsynBind != null)
            {
                IsReRender = true;
                AsynBind(this,EventArgs .Empty );
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //System.IO.StringWriter tw = new System.IO.StringWriter(sb);
                //HtmlTextWriter htw = new HtmlTextWriter(tw);
                //this.RenderControl(htw);
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //string resultStr = serializer.Serialize(sb.ToString ());
                //Context.Response.ContentType = "application/json;charset=UTF-8";
                //Context.Response.Write(resultStr);
                //Context.Response.End();
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
