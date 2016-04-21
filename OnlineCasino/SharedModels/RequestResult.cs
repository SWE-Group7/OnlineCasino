using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedModels
{

    public class RequestResult
    {
        private bool hasValue;
        private object value;

        public RequestResult()
        {
            this.hasValue = false;
        }

        public object GetValue()
        {
                return value;
        }
    
        public bool SetValue(object value)
        {
            if (!hasValue)
            {
                lock (this.value)
                    this.value = value;
                
                hasValue = true;
                return true;
            }
            
            return false;
        }

        public bool HasValue()
        {
            return hasValue;
        }

    }
}
