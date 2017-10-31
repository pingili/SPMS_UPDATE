using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4DB
{
    public interface ILog
    {
        /// <summary>
        /// Informationals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void informational(string message);

        /// <summary>
        /// Informationals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        void informational(string message, Exception innerException);

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void debug(string message);

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        void debug(string message, Exception innerException);

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void error(string message);

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        void error(string message, Exception innerException);

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void warning(string message);

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        void warning(string message, Exception innerException);
    }
}
