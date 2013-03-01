using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xxf.Web.UI
{
    /// <summary>
    /// 异步绑定（如一些控件）返回内容
    /// </summary>
    [Serializable ]
    public class AsyncBindResponse :Response 
    {
        public int DataCount { get; set; }
        public string Html { get; set; }
    }
}
