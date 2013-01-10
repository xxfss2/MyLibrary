using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;

namespace Xxf.Web.UI
{
    public class ViewManager<T> where T : UserControl
    {
        private Page _pageHolder;
        public T LoadViewControl(string path)
        {
            this._pageHolder = new Page();
            return (T)_pageHolder.LoadControl(path);
        }

        public string RenderView(T control)
        {
            StringWriter output = new StringWriter();
            _pageHolder.Controls.Add(control);
            HttpContext.Current.Server.Execute(_pageHolder, output, false);
            return output.ToString();
        }
    }
}