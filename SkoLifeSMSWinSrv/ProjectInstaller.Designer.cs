namespace SkoLifeSMSWinSrv
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstallerX = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerX = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerX
            // 
            this.serviceProcessInstallerX.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstallerX.Password = null;
            this.serviceProcessInstallerX.Username = null;
            // 
            // serviceInstallerX
            // 
            this.serviceInstallerX.DisplayName = "SkoLifeSMSWinService";
            this.serviceInstallerX.ServiceName = "SkoLifeSMSWinService";
            this.serviceInstallerX.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerX,
            this.serviceInstallerX});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerX;
        private System.ServiceProcess.ServiceInstaller serviceInstallerX;
    }
}