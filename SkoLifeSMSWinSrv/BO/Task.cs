using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Net.Configuration;

namespace SkoLifeSMSWinSrv.BO
{
    public class Task
    {
        public Task()
        {
        }
        public Task(int sms_msg_detail_id, string sms_msg, string sms_target_number, string modem_ip, string modem_user, string modem_pass)
        {
            this.sms_msg_detail_id = sms_msg_detail_id;
            this.sms_msg = sms_msg;
            this.sms_target_number = sms_target_number;
            this.modem_ip = modem_ip;
            this.modem_user = modem_user;
            this.modem_pass = modem_pass;
        }

        public int sms_msg_detail_id { get; set; }
        public string sms_msg { get; set; }
        public string sms_log { get; set; }
        public Types.MsgStatus sms_status { get; set; }
        public string sms_target_number { get; set; }
        public DateTime sms_sent_date { get; set; }

        public string modem_ip { get; set; }
        public string modem_user { get; set; }
        public string modem_pass { get; set; }

        public void UpdateMsg()
        {
            try
            {
                SqlConnection con = new SqlConnection(Properties.Settings.Default.SkoLifeDBConnection);
                SqlCommand cmd = new SqlCommand("update sms_message_detail set sms_status = @sms_status, sms_sent_date = GETDATE() " +
                    "where sms_msg_detail_id = @sms_msg_detail_id"
                    , con);
                SqlParameter Psms_status = new SqlParameter("@sms_status", System.Data.SqlDbType.Int) { Value = (int)sms_status };
                SqlParameter Psms_msg_detail_id = new SqlParameter("@sms_msg_detail_id", System.Data.SqlDbType.Int) { Value = sms_msg_detail_id };
                cmd.Parameters.AddRange(new SqlParameter[] { Psms_status, Psms_msg_detail_id }); 

                cmd.CommandTimeout = int.MaxValue;

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close(); cmd.Dispose(); con.Dispose();
            }
            catch (Exception ex)
            {
                LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Error, ex.Message, typeof(Task));
            }
        }

        public void AddMsgLog()
        {
            try
            {
                SqlConnection con = new SqlConnection(Properties.Settings.Default.SkoLifeDBConnection);
                SqlCommand cmd = new SqlCommand("INSERT INTO [sms_msg_detail_log] ([sms_msg_detail_id],[sms_log],[sms_log_date]) VALUES " +
                    "(@sms_msg_detail_id, @sms_log, GETDATE())"
                    , con);
                SqlParameter Psms_msg_detail_id = new SqlParameter("@sms_msg_detail_id", System.Data.SqlDbType.Int) { Value = sms_msg_detail_id };
                SqlParameter Psms_log = new SqlParameter("@sms_log", System.Data.SqlDbType.NVarChar) { Value = sms_log };
                cmd.Parameters.AddRange(new SqlParameter[] { Psms_msg_detail_id, Psms_log });

                cmd.CommandTimeout = int.MaxValue;

                con.Open();
                cmd.ExecuteNonQuery();

                con.Close(); cmd.Dispose(); con.Dispose();
            }
            catch (Exception ex)
            {
                LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Error, ex.Message, typeof(Task));
            }
        }

        public bool Execute()
        {
            try
            {
                
                sms_sent_date = DateTime.Now;
                if (!HuaweiAPI.LoginState(modem_ip))
                {
                    if (!HuaweiAPI.UserLogin(modem_ip, modem_user, modem_user, out string loginLog))
                    {
                        sms_log += "\r\n" + loginLog;
                        UpdateMsg();
                        AddMsgLog();
                        return false;
                    }
                    sms_log += "\r\n" + loginLog;
                    
                }

                if (HuaweiAPI.SendSMS(modem_ip, sms_target_number, sms_msg, out string sendLog))
                {
                    sms_status = Types.MsgStatus.Success;
                    sms_log += "\r\n" + sendLog;
                }
                else
                {
                    sms_status = Types.MsgStatus.Faild;
                    sms_log += "\r\n" + sendLog;
                }
                

                UpdateMsg();
                AddMsgLog();
                return true;
            }
            catch (Exception ex)
            {
                LogsManager.DefaultInstance.LogMsg(LogsManager.LogType.Error, ex.Message, typeof(Task));
                return false;
            }
            
            
        }
       

    }
}
