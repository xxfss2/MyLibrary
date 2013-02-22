using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Xxf.Web.UI
{
    /// <summary>
    /// 简化页面中数据绑定的扩展函数
    /// </summary>
    public static class BindExtension
    {
        /// <summary>
        /// 用于页面中的数据绑定(Repeater)控件
        /// </summary>
        /// <typeparam name="IView"></typeparam>
        /// <param name="page"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static object Bind<IView>(this Page page, Func<IView, object> func)
        {
            return func((IView)page.GetDataItem());
        }

        /// <summary>
        /// 用于用户控件中的Repeater控件
        /// </summary>
        /// <typeparam name="IView"></typeparam>
        /// <param name="item"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static object Bind<IView>(this RepeaterItem item, Func<IView, object> func)
        {
            return func((IView)item.DataItem);
        }
    }
}
