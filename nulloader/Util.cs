using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace nulloader
{
    public static class Util
    {
        [DllImport("user32.dll")]
        extern static bool LockWindowUpdate(IntPtr hWndLock);

        public static bool Lock(this Control ctl)
        {
            return LockWindowUpdate(ctl.Handle);
        }
        public static bool Unlock(this Control ctl)
        {
            return LockWindowUpdate(IntPtr.Zero);
        }

        public static Type GetNullsType(string FullName)
        {
            return Assembly.GetAssembly(typeof(A.A))
                // public delegate in A.ZD (internal class)
                .GetType(FullName);
        }
        public static object CrossCall(this Control obj, string Method, params object[] args)
        {
            return obj.Invoke((MethodInvoker)(() => obj.GetType().GetMethod(Method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, args)));
        }
        public static FieldBinding Field(this object o, string FieldName)
        {
            FieldInfo fi = null;
            for (var t = o.GetType(); t != typeof(object) && fi == null; t = t.BaseType)
                fi = t.GetField(FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return new FieldBinding { o = o, f = fi };
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
