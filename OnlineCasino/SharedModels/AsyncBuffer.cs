using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedModels
{
    public class AsyncBuffer
    {
        private bool requested;
        private bool updated;
        private object value;
        private int writeThreadId;

        public AsyncBuffer()
        {
            this.writeThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void Request()
        {
            updated = false;
            requested = true;
        }

        public bool SetValue(object obj)
        {
            if (Thread.CurrentThread.ManagedThreadId == writeThreadId && requested && !updated)
            {
                lock (value)
                {
                    value = obj;
                    updated = true;
                    requested = false;
                }
                return true;
            }
            return false;
        }

        public bool HasValue()
        {
            return updated;
        }

        public object GetValue()
        {
            if (updated)
            {
                lock (value)
                {
                    return value;
                }
            }

            return null;
        }

    }
}
