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
    public static class DelegateUtility
    {
        public static T Cast<T>(Delegate source) where T : class
        {
            return Cast(source, typeof(T)) as T;
        }

        public static Delegate Cast(Delegate source, Type type)
        {
            if (source == null)
                return null;

            Delegate[] delegates = source.GetInvocationList();
            if (delegates.Length == 1)
                return Delegate.CreateDelegate(type,
                    delegates[0].Target, delegates[0].Method);

            Delegate[] delegatesDest = new Delegate[delegates.Length];
            for (int nDelegate = 0; nDelegate < delegates.Length; nDelegate++)
                delegatesDest[nDelegate] = Delegate.CreateDelegate(type,
                    delegates[nDelegate].Target, delegates[nDelegate].Method);
            return Delegate.Combine(delegatesDest);
        }
    }
}
