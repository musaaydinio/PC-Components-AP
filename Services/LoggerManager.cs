using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NLog;
using Services.Contracts;
using System;
using System.Collections.Generic;


namespace Services
{
    // NLog kütüphanesini kullanarak API genelindeki tüm log alma işlemlerini yaptğımız sınıfımız.
    public class LoggerManager : ILoggerServices
    {
        private static NLog.ILogger logger =LogManager.GetCurrentClassLogger();

        public void LogDebug(string message)=>logger.Debug(message);
        

        public void LogError(string message)=>logger?.Error(message);
        

        public void LogInfo(string message)=>logger.Info(message);
       

        public void LogWarning(string message)=>logger.Warn(message);
       
    }
}
