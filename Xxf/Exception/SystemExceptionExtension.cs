using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xxf
{
    public static class SystemExceptionExtension
    {
        /// <summary>
        /// 获取异常及内部异常（一层）的Message，并反序化Exception示例保存至程序ExcepitonLog目录(未完成)
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string Messages(this SystemException ex)
        {
            string msg = ex.Message;
            if (ex.InnerException != null)
                msg += Environment.NewLine + ex.InnerException.Message;
            return msg;
        }
    }
}
