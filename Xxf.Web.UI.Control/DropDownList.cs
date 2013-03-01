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
    public class DropDownList : System.Web.UI.WebControls.DropDownList
    {
        public string PostValue
        {
            get
            {
                return HttpContext.Current.Request.Form[this.ClientID];
            }
        }
    }
}
