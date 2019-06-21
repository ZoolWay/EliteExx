using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.EliteExx.Core
{
    public abstract class UiMessengerMessage
    {
        public class Publish : UiMessengerMessage
        {
            public object Message { get; }

            public Publish(object message)
            {
                this.Message = message;
            }
        }
    }
}
