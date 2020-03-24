using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Net.Mime;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SkoLifeSMSWinSrv.BO;

namespace SkoLifeSMSWinSrv
{
    public partial class SkoLifeSMSWinService : ServiceBase
    {
        private static Timer _timner;

        public SkoLifeSMSWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Log, "Service Started...", typeof(ServiceBase));
            TaskManager.DefaultInstance = new TaskManager();
            TaskManager.DefaultInstance.Start();
            //execute every xH
            _timner = new Timer(Properties.Settings.Default.ExecuteInterval.TotalMilliseconds);
            _timner.Elapsed += _timner_Elapsed;
            _timner.Enabled = true;
            _timner_Elapsed(_timner, null);

        }

        private void _timner_Elapsed(object sender, ElapsedEventArgs e)
        {
            TaskManager.DefaultInstance.GetTasks();
            LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Log, $"GetTasks Executed - {TaskManager.DefaultInstance.Tasks.Count} Tasks In-queue...", typeof(ServiceBase));
        }

        protected override void OnStop()
        {
            _timner.Stop();
            TaskManager.DefaultInstance.Stop();
        }
    }
}
