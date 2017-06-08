using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Common
{
    public static class ConfigHelper
    {
        public static T Get<T>(string Key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[Key], typeof(T));
        }
    }
}
