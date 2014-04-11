using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Collections.Specialized;

namespace Xxf.Web.UI.Control
{

    public class Params : NameValueCollection
    {
        public new string this [string name]
        {
            get 
            {
                return HttpContext.Current.Request.Form[name].ToString();
            }
            set 
            {
                base[name] = value;
            }
        }
        internal new string this[int i]
        {
            get
            {
                return base[i];
            }
        }
    }

    [ToolboxData("<{0}:AutoComplete runat=server></{0}:AutoComplete>")]
    public class AutoComplete : TextBox
    {
        public new event AutoCompleteHandler TextChanged;

        private Params _params=new Params ();
        public Params Params
        {
            get
            {
                return _params;
            }
        }

        [TypeConverter (typeof (uint ))]
        public string MinStart { get; set; }

        [TypeConverter(typeof(bool))]
        public bool IsBuildFunc { get; set; } 

        protected virtual void OnAutoCompleteChanged(EventArgs e)
        {
            if (TextChanged != null)
            {

                List<string> result = TextChanged(this, EventArgs.Empty);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                string json = ser.Serialize(result);
                Context.Response.ContentType = "text/plain";
                Context.Response.Write(json);
                Context.Response.End();
            }
        }

        protected override void RaisePostDataChangedEvent()
        {
            OnAutoCompleteChanged(EventArgs.Empty);
          //  base.RaisePostDataChangedEvent();
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            StringBuilder sb=new StringBuilder ();
            for (int i = 0; i < Params.Count; i++)
            {
                sb.AppendFormat("{0}:{1}", Params .Keys [i], Params [i]);
            }
            if(IsBuildFunc)
              writer.AddAttribute("onkeyup", "AutoComplate(this,"+MinStart+",{" + sb.ToString () + "})");
            base.RenderControl(writer);
        }


    }
}
