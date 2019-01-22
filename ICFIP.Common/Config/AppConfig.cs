using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICFIP.Common.Config
{
    /// <summary>
    /// classe generale pour lire des variables du fichier web.config
    /// </summary>
    public abstract class AppConfig
    {
        public class Keys
        {
            public const string CURRENT_ENVIRONMENT = "CURRENT_ENVIRONMENT";
            public const string DB_CONNECTION_STRING = "DB_CONNECTION_STRING";
        }

        #region Declaration
        
        /// <summary>
        /// variable static contient la chaine de connection existe dans le fichier Web.config
        /// </summary>
        public static string DbConnexionString
        {
            get
            {
                return ReadSetting(Keys.DB_CONNECTION_STRING);
            }
        }

       
        public new static string CurrentEnvironment
        {
            get
            {
                return ConfigurationManager.AppSettings[Keys.CURRENT_ENVIRONMENT];
            }
        }
        #endregion


        private static string ReadSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            string keyWithEnvironment = string.Format("{0}_{1}", key, CurrentEnvironment);

            if (AppSettingsKeys.Contains(keyWithEnvironment))
            {
                // Variable de configuration associée à l'environnement :
                return ConfigurationManager.AppSettings[keyWithEnvironment];
            }
            else
            {
                // Variable de configuration introuvable avec ou sans le suffixe d'environnement :
                throw new Exception(string.Format("Clé de configuration introuvable : \"{0}\".", key));
            }
           
        }

        private static string[] _AppSettingsKeys;
        protected static string[] AppSettingsKeys
        {
            get
            {
                if (_AppSettingsKeys == null)
                    _AppSettingsKeys = ConfigurationManager.AppSettings.Keys.Cast<string>().ToArray();

                return _AppSettingsKeys;
            }
        }

    }
}
