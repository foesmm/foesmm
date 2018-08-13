using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace foesmm.common
{
    public static class ExceptionExtension
    {
        public static StackFrame FirstFrame(this Exception exception)
        {
            var trace = new StackTrace(exception, true);
            return trace.GetFrame(0);
        }

        public static Guid Guid(this Exception exception)
        {
            var frame = exception.FirstFrame();
            var signature = $"{exception.GetType()}{frame.GetFileName()}{frame.GetFileLineNumber()}{exception.Message}";
            return new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(signature)));
        }
    }
}
