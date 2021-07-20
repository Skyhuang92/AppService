using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AppService.Tools
{
    public class McConfig
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static McConfig _instance = null;
        public static McConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(McConfig))
                    {
                        if (_instance == null)
                        {
                            _instance = new McConfig();
                        }
                    }
                }
                return _instance;
            }
        }

        public McConfig() { }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string getConfig(string key)
        {
            var values = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrWhiteSpace(values))
            {
                return values;
            }
            return string.Empty;
        }

    }
}