using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Timers;



namespace SkoLifeSMSWinSrv.BO
{
    public class TaskManager
    {
        public static TaskManager DefaultInstance { get; set; }
        private static Timer _timner;

        public TaskManager()
        {
            
            Tasks = new Queue<Task>();
            _timner = new Timer(1000 * 2);
            _timner.Elapsed += _timner_Run;
        }

        public  Queue<Task> Tasks { get; set; }
        public bool IsBusy { get; set; }

        private void _timner_Run(object sender, ElapsedEventArgs e)
        {
            if (!IsBusy && Tasks.Count > 0)
            {
                RunAsync();
            }   
        }
        private System.Threading.Tasks.Task RunAsync()
        {
            IsBusy = true;
           return System.Threading.Tasks.Task.Run(() =>
             {
                 Task tsk = Tasks.Dequeue();
                 tsk.Execute();
                 IsBusy = false;
             });
        }

        public void GetTasks()
        {
            try
            {
                SqlConnection con = new SqlConnection(Properties.Settings.Default.SkoLifeDBConnection);
                SqlCommand cmd = new SqlCommand(
                    "select d.sms_msg_detail_id, d.sms_msg, d.sms_target_number, g.modem_ip, g.modem_user, g.modem_pass from sms_message s inner join sms_message_detail d " +
                    "on s.sms_msg_id = d.sms_msg_id inner join sms_config g on s.sms_config_id = g.sms_config_id where s.sms_status = 2 AND d.sms_status = 2 order by d.sms_msg_detail_id"
                    , con) {CommandTimeout = int.MaxValue};
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                
                adp.Fill(dt);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    Task tsk = new Task(
                        Convert.ToInt32(row["sms_msg_detail_id"]),
                        row["sms_msg"].ToString(),
                        row["sms_target_number"].ToString(),
                        row["modem_ip"].ToString(),
                        row["modem_user"].ToString(),
                        row["modem_pass"].ToString()
                        );
                    Tasks.Enqueue(tsk);
                }

                dt.Dispose(); con.Close(); con.Dispose(); cmd.Dispose(); adp.Dispose();
            }
            catch (Exception ex)
            {
                LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Error, ex.Message, typeof(TaskManager));
            }
        }
        public void Start()
        { _timner.Start(); }
        public void Stop()
        {
            _timner.Stop();
            //should put code to logout.
        }

    }
}
