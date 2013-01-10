using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;
using System.Web.UI;

namespace Xxf.Web.UI
{
    public class SelectPropertyMetaDataProc
    {

        private static Dictionary<Type, Dictionary<PropertyInfo, List<SelectPropertyAttribute>>> s_metadataCache = new Dictionary<Type, Dictionary<PropertyInfo, List<SelectPropertyAttribute>>>();
        private static Dictionary<PropertyInfo, object> s_defaultValueCache = new Dictionary<PropertyInfo, object>();
        private static object s_mutex = new object();

        private static Dictionary<PropertyInfo, List<SelectPropertyAttribute>> GetMetadata(Type type)
        {
            if (!s_metadataCache.ContainsKey(type))
            {
                lock (s_mutex)
                {
                    if (!s_metadataCache.ContainsKey(type))
                    {
                        s_metadataCache[type] = LoadMetadata(type);
                    }
                }
            }

            return s_metadataCache[type];
        }

        private static Dictionary<PropertyInfo, List<SelectPropertyAttribute>> LoadMetadata(Type type)
        {
            var result = new Dictionary<PropertyInfo, List<SelectPropertyAttribute>>();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            foreach (var p in properties)
            {
                var attributes = p.GetCustomAttributes(typeof(SelectPropertyAttribute), true);
                if (attributes.Length > 0)
                {
                    result[p] = new List<SelectPropertyAttribute>(attributes.Cast<SelectPropertyAttribute>());
                }
            }

            return result;
        }

        private static object GetDefaultValue(PropertyInfo property)
        {
            if (!s_defaultValueCache.ContainsKey(property))
            {
                lock (s_mutex)
                {
                    if (!s_defaultValueCache.ContainsKey(property))
                    {
                        var attributes = property.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                        object value = attributes.Length > 0 ?
                            ((DefaultValueAttribute)attributes[0]).Value : null;
                        s_defaultValueCache[property] = value;
                    }
                }
            }

            return s_defaultValueCache[property];
        }

        public static void SetPropertyValues(UserControl control, HttpContext context)
        {
            var metadata = GetMetadata(control.GetType());
            foreach (var property in metadata.Keys)
            {
                object value = GetValue(metadata[property], context) ?? GetDefaultValue(property);
                if (value != null)
                {
                    property.SetValue(control, Convert.ChangeType(value, property.PropertyType), null);
                }
            }
        }

        private static object GetValue(IEnumerable<SelectPropertyAttribute> metadata, HttpContext context)
        {
            foreach (var att in metadata)
            {
                var collection = (att.Source == PropertyValueSource.QueryString) ? context.Request.QueryString : context.Request.Params;
                object value = collection[att.Key];

                if (value != null) return value;
            }

            return null;
        }



    }

}