using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xxf.Web.UI.Control
{
    /// <summary>
    /// 实现该接口的控件表示提供异步绑定数据方法
    /// </summary>
    public interface IAsynBindControl
    {
        string GetBindScript();
    }
}
