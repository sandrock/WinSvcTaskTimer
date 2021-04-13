
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;

    /// <summary>
    /// Windows Service configurable installer configuration.
    /// </summary>
    public class LocalServiceInstallerConfiguration
    {
        /// <summary>
        /// Gets or sets the type of account under which to run this service application.
        /// </summary>
        /// <value>
        /// the type of account under which to run this service application.
        /// </value>
        public ServiceAccount ServiceAccount { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the user account under which the service application will run.
        /// </summary>
        /// <value>
        /// the user account under which the service application will run.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password associated with the user account under which the service application runs.
        /// </summary>
        /// <value>
        /// the password associated with the user account under which the service application runs.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the services that must be running for this service to run.
        /// </summary>
        /// <value>
        /// the services that must be running for this service to run.
        /// </value>
        public string[] ServicesDependedOn { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the service should be delayed from starting until other automatically started services are running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the service should be delayed from starting until other automatically started services are running; otherwise, <c>false</c>.
        /// </value>
        public bool DelayedAutoStart { get; set; }

        /// <summary>
        /// Gets or sets how and when this service is started.
        /// </summary>
        /// <value>
        /// how and when this service is started.
        /// </value>
        public ServiceStartMode StartType { get; set; }

        /// <summary>
        /// Creates from appSettings.
        /// </summary>
        /// <returns>the configuration</returns>
        public static LocalServiceInstallerConfiguration CreateFromConfiguration()
        {
            var prefix = "ServiceInstaller/";
            var appSettingsResolver = new Func<string, string>(key => ConfigurationManager.AppSettings[prefix + key]);

            return CreateFromConfiguration(appSettingsResolver);
        }
        
        /// <summary>
        /// Creates from specified configuration (uses appSettings).
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns>the configuration</returns>
        public static LocalServiceInstallerConfiguration CreateFromConfiguration(System.Configuration.Configuration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            var prefix = "ServiceInstaller/";
            var appSettingsResolver = new Func<string, string>(key => GetAppSettingsKey(config.AppSettings, prefix + key));

            return CreateFromConfiguration(appSettingsResolver);
        }

        private static LocalServiceInstallerConfiguration CreateFromConfiguration(Func<string, string> appSettings)
        {
            var cfg = new LocalServiceInstallerConfiguration();

            var serviceAccountValue = appSettings("ServiceAccount");
            if (string.IsNullOrEmpty(serviceAccountValue))
            {
                cfg.ServiceAccount = ServiceAccount.NetworkService;
            }
            else
            {
                cfg.ServiceAccount = (ServiceAccount)Enum.Parse(typeof(ServiceAccount), serviceAccountValue);

                if (cfg.ServiceAccount == ServiceAccount.User)
                {
                    cfg.Username = appSettings("Username");
                    cfg.Password = appSettings("Password");
                }
            }

            var startTypeValue = appSettings("StartType");
            if (string.IsNullOrEmpty(startTypeValue))
            {
                cfg.StartType = ServiceStartMode.Automatic;
            }
            else
            {
                cfg.StartType = (ServiceStartMode)Enum.Parse(typeof(ServiceStartMode), startTypeValue);
            }

            var delayedAutoStartValue = appSettings("DelayedAutoStart");
            if (string.IsNullOrEmpty(delayedAutoStartValue))
            {
                cfg.DelayedAutoStart = false;
            }
            else
            {
                cfg.DelayedAutoStart = bool.Parse(delayedAutoStartValue);
            }

            var dependenciesValue = appSettings("ServicesDependedOn");
            if (!string.IsNullOrWhiteSpace(dependenciesValue))
            {
                cfg.ServicesDependedOn = dependenciesValue
                    .Split(new string[] { ",", ";", " ", }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => e.Trim())
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .ToArray();
            }
            else
            {
                cfg.ServicesDependedOn = new string[0];
            }

            cfg.ServiceName = appSettings("ServiceName");
            cfg.DisplayName = appSettings("DisplayName");
            cfg.Description = appSettings("Description");

            return cfg;
        }

        private static string GetAppSettingsKey(AppSettingsSection section, string key)
        {
            if (section != null && section.Settings != null)
            {
                var entry = section.Settings[key];
                return entry != null ? entry.Value : null;
            }
            else
            {
                return null;
            }
        }
    }
}
