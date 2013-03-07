using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xxf.Web.UI.Control
{
    /// <summary>
    /// 管理控件生成脚本时的缓存处理
    /// </summary>
    public class ScriptCache
    {
        private static IDictionary<string, string> scripts = new Dictionary<string, string>();

        internal static void AddScript(string page, string script)
        {
            //debug 下不作缓存，方便调试修改
            #if DEBUG

            return;

            #endif
            lock (scripts)
            {
                if (scripts.ContainsKey(page))
                {
                    throw new InvalidOperationException("缓存保存，线程同步错误");
                }
                scripts.Add(page, script);
            }
        }

        internal static string GetScript(string page)
        {
            if (scripts.ContainsKey(page))
            {
                return scripts[page];
            }
            return null ;
        }

        public static void Clear()
        {
            lock (scripts)
            {
                scripts.Clear();
            }
        }
    }
}
