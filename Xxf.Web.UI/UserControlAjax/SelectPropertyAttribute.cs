using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xxf.Web.UI
{

    public enum PropertyValueSource
    {
        Form, QueryString
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class SelectPropertyAttribute : Attribute
    {
        public string Key { get; set; }
        public PropertyValueSource Source { get; set; }
    }
}