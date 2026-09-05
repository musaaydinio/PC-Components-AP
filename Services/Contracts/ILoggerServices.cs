using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    // API genelinde hata yönetimi ve sistem bilgi kayıtlarını tuttuğumuz arayüzümüz.
    public interface ILoggerServices
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogDebug(string message);
    }
}
