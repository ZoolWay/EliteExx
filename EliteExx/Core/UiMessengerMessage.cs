using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.EliteExx.Core
{
    /// <summary>
    /// Messages for UI messenging.
    /// </summary>
    public abstract class UiMessengerMessage
    {
        /// <summary>
        /// Publishes an immutable messenge to the UI (EventAggregator).
        /// Sent by: Any actor.
        /// Received by: UiMessenger.
        /// Rate: Unlimited.
        /// </summary>
        public class Publish : UiMessengerMessage
        {
            public object Message { get; }

            public Publish(object message)
            {
                this.Message = message;
            }
        }

        /// <summary>
        /// Publishes an error message from the actor system to the UI.
        /// Sent by: Any actor.
        /// Received by: UiMessenger.
        /// Rate: Unlimited (please never).
        /// </summary>
        public class Error : UiMessengerMessage
        {
            public string Message { get; }

            public Error(string message)
            {
                this.Message = message;
            }
        }
    }
}
