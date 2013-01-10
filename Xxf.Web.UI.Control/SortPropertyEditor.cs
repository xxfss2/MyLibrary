using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace Xxf.Web.UI.Control
{
    public class SortPropertyEditor:UITypeEditor 
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
                return null ;
            SetSortPropertyFrm dlg = new SetSortPropertyFrm();
          if(service.ShowDialog(dlg) == DialogResult.OK)
                return dlg.SelectedProperty;
            return value;
        }
    }
}
