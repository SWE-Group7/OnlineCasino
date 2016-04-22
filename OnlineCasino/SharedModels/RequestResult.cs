using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedModels
{

    public class RequestResult
    {
        private bool hasReturned = false;
        private bool success = false;
        private object value = new object();

        public RequestResult()
        {
        }

        public bool GetValue<T>(out T obj)
        {
            if (hasReturned && success)
            {
                obj = (T) value;
                return true;
            }
            else
            {
                obj = default(T);
                return false;
            }
        }

        public bool WaitForReturn<T>(long maxWaitTimeMillis, out T obj)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while(sw.ElapsedMilliseconds < maxWaitTimeMillis)
            {
                if (hasReturned)
                    break;

                Thread.Sleep(5);
            }

            return GetValue<T>(out obj);
        }
    
        public bool SetValue(bool success, object value)
        {
            if (!hasReturned)
            {
                lock (this.value)
                {
                    this.value = value;
                    this.success = success;
                    this.hasReturned = true;
                }
                return true;
            }

            return false;
        }

        public bool HasReturned()
        {
            return hasReturned;
        }

        

    }
}
