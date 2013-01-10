using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Xxf.Web.UI.Control
{
    public class TextBox : System.Web.UI.WebControls.TextBox, ISelectParamControl
    {
        #region ISelectParamControl 成员

        public string GetParamQuery()
        {
            return string .Format ("['{0}']=$('#{0}').val();",this.ClientID );
        }

        #endregion
    }
}
