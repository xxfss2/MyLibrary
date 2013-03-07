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
    public class DropDownList : System.Web.UI.WebControls.DropDownList, ISelectParamControl
    {
        public string PostValue
        {
            get
            {
                return HttpContext.Current.Request.Form[this.ClientID];
            }
        }

        public string GetParamQuery()
        {
            return string.Format("['{0}']=$('#{0}').val();", this.ClientID);
        }
    }
}
