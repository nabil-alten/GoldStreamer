namespace FeederCapturer
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
            this.GlobalPricesCapturerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.GlobalPricesCapturerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // GlobalPricesCapturerProcessInstaller
            // 
            this.GlobalPricesCapturerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.GlobalPricesCapturerProcessInstaller.Password = null;
            this.GlobalPricesCapturerProcessInstaller.Username = null;
            // 
            // GlobalPricesCapturerInstaller
            // 
            this.GlobalPricesCapturerInstaller.ServiceName = "GlobalPricesCapturer";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.GlobalPricesCapturerProcessInstaller,
            this.GlobalPricesCapturerInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller GlobalPricesCapturerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller GlobalPricesCapturerInstaller;
    }
}