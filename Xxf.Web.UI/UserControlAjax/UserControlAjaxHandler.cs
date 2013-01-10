using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Xxf.Web.UI
{
    public class UserControlAjaxHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string appRelativePath = context.Request.AppRelativeCurrentExecutionFilePath;
            string controlPath = appRelativePath.ToLower().Replace(".jza", ".ascx");
            var viewManager = new ViewManager<UserControl>();
            var control = viewManager.LoadViewControl(controlPath);
            SelectPropertyMetaDataProc.SetPropertyValues(control, context);
            context.Response.ContentType = "text/html";
            context.Response.Write(viewManager.RenderView(control));

        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

    }
}