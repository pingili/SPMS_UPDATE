using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Log4DB
{
    public class ErrorLog : ILog
{
    log4net.ILog _Log;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorLog"/> class.
    /// </summary>
    /// <param name="log">The log.</param>
    public ErrorLog(log4net.ILog log) : this()
    {
        _Log = log;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorLog"/> class.
    /// </summary>
    public ErrorLog()
    {
        _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


    }

    /// <summary>
    /// Informationals the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void informational(string message)
    {
        _Log.Info(message);
    }

    /// <summary>
    /// Informationals the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The innerException.</param>
    public void informational(string message, Exception innerException)
    {
        _Log.Info(message, innerException);
    }

    /// <summary>
    /// Debugs the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void debug(string message)
    {
        _Log.Debug(message);
    }

    /// <summary>
    /// Debugs the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The innerException.</param>
    public void debug(string message, Exception innerException)
    {
        _Log.Debug(message, innerException);
    }

    /// <summary>
    /// Errors the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void error(string message)
    {
        _Log.Error(message);
    }

    /// <summary>
    /// Errors the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The innerException.</param>
    public void error(string message, Exception innerException)
    {
        _Log.Error(message, innerException);
    }

    /// <summary>
    /// Warnings the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void warning(string message)
    {
        _Log.Warn(message);
    }

    /// <summary>
    /// Warnings the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The innerException.</param>
    public void warning(string message, Exception innerException)
    {
        _Log.Warn(message, innerException);
    }
}
}
