using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Xxf.Web.UI
{
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public virtual void AsyncResponse()
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(this);
            response.ContentType = "application/json;charset=UTF-8";
            response.Write(json);
            response.End();
        }
    }

}
