
namespace WinSvcTaskTimer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Configuration.Install;
    using System.Linq;
    using System.ServiceProcess;

    [RunInstaller(true)]
    public class LocalServiceInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public LocalServiceInstaller()
        {
            var config = ConfigurationManager.OpenExeConfiguration(typeof(LocalServiceInstaller).Assembly.CodeBase.Replace("file:///", string.Empty));
            if (config == null)
            {
                throw new InvalidOperationException("Configuration file is missing. ");
            }

            var conf = LocalServiceInstallerConfiguration.CreateFromConfiguration(config);
            if (conf == null)
            {
                throw new InvalidOperationException("Configuration entries are missing. ");
            }

            // configure ServiceProcessInstaller
            this.processInstaller = new ServiceProcessInstaller()
            {
                Account = conf.ServiceAccount,
            };
            if (conf.ServiceAccount == ServiceAccount.User)
            {
                this.processInstaller.Username = conf.Username;
                this.processInstaller.Password = conf.Password;
            }

            this.Installers.Add(this.processInstaller);

            // configure ServiceInstaller
            this.serviceInstaller = new ServiceInstaller()
            {
                DisplayName = conf.DisplayName,
                ServiceName = conf.ServiceName,
                StartType = conf.StartType,
                DelayedAutoStart = conf.DelayedAutoStart,
                ServicesDependedOn = conf.ServicesDependedOn, // don't let it be null!
                Description = conf.Description,
            };
            this.Installers.Add(this.serviceInstaller);
        }
    }
}
