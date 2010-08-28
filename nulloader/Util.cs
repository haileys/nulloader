using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace nulloader
{
    public static class Util
    {
        public static object CrossCall(this Control obj, string Method, params object[] args)
        {
            return obj.Invoke((MethodInvoker)(() => obj.GetType().GetMethod(Method).Invoke(obj, args)));
        }
        public static FieldBinding Field(this object o, string FieldName)
        {
            return new FieldBinding { o = o, f = o.GetType().GetField(FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) };
        }
        public class FieldBinding
        {
            internal object o;
            internal FieldInfo f;

            internal FieldBinding() { }

            public void Set(object Value)
            {
                f.SetValue(o, Value);
            }

            public object Get()
            {
                return f.GetValue(o);
            }
        }
    }
}
