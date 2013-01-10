using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xxf.Web.UI.Control
{
    /// <summary>
    /// 实现该接口的控件表示需要对客户端输出脚本
    /// </summary>
    public  interface IClientScriptControl
    {
         string GetScript();
    }
}
