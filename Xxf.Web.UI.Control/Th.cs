using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System .Web .UI ;
using System.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace Xxf.Web.UI.Control
{
    public class Th : System.Web.UI.Control, INamingContainer
    {
         /// <summary>
         /// 字段显示在客户端的名称
         /// </summary>
         public string FieldClientCode { get; set; }

         private string _FieldSortCode;
         [Bindable(true)]
         [Localizable(true)]
         [Editor(typeof(SortPropertyEditor), typeof(UITypeEditor))]
         public string FieldSortCode
         {
             get
             {
                 return _FieldSortCode;
             }
             set
             {
                 _FieldSortCode = value;
             }
         }

         public override void RenderControl(HtmlTextWriter writer)
         {
             if (FieldSortCode != null)
                 writer.AddAttribute("field", FieldSortCode);
             writer.RenderBeginTag(HtmlTextWriterTag.Th);
             writer.Write(FieldClientCode);
             writer.RenderEndTag();
         }

    }
}
