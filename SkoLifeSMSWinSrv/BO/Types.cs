using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkoLifeSMSWinSrv.BO
{
    public class Types
    {
        public enum MsgStatus
        {
            Draft = 0,
            Canceled = 1,
            ReadyToSend = 2,
            Faild = 3,
            Success = 4,


        }
    }
}
