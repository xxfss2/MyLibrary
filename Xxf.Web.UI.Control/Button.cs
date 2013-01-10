using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI;
using System.Drawing.Design;

namespace Xxf.Web.UI.Control
{
    public enum ButtonType
    {
        Add,Delete,Edit,Back,Select
    }
    public class Button : System.Web.UI.WebControls.Button
    {
        public ButtonType Type { get; set; }

        [DefaultValue("")]
        [Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Themeable(false)]
        [UrlProperty("*.aspx")]
        public string ActionURL { get; set; }

        public override void RenderControl(System.Web.UI.HtmlTextWriter writer)
        {
            switch (Type)
            {
                case ButtonType .Edit :
                    writer.AddAttribute("onclick", "JZ_Edit('" + ActionURL + "');return false;",false);
                    break;
            }
            base.RenderControl(writer);
        }
    }
}
