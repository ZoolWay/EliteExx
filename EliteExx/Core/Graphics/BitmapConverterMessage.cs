using System;

namespace Zw.EliteExx.Core.Graphics
{
    public abstract class BitmapConverterMessage
    {
        /// <summary>
        /// Message to initialize a BitmapConverter.
        /// Sent by: BitmapProcessor.
        /// Received by: BitmapConverter.
        /// Rate: Only once.
        /// </summary>
        public class Init : BitmapConverterMessage
        {
        }

        /// <summary>
        /// Message to make a BitmapConverter convert existing matching files.
        /// Sent by: BitmapConverter.
        /// Received by: BitmapConverter.
        /// Rate: Unlimited.
        /// </summary>
        public class Convert : BitmapConverterMessage
        {
        }

        /// <summary>
        /// Message to notify about the fact that failures happened in a BitmapConverter.
        /// Sent by: BitmapConverter.
        /// Received by: BitmapProcessor.
        /// Rate: Unlimited but hopefully none.
        /// </summary>
        public class FailureNotification : BitmapConverterMessage
        {
            public string FailedFilename { get; }
            public string ErrorMessage { get; }

            public FailureNotification(string errorMessage, string failedFilename)
            {
                this.FailedFilename = failedFilename;
                this.ErrorMessage = errorMessage;
            }
        }
    }
}
