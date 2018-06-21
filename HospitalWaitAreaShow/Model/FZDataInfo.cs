using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow.Model
{
    public class FZDataInfo
    {
        /// <summary>
        /// name's FZKS
        /// </summary>
        public string FZKS;

        /// <summary>
        /// state of FZ,0 is for waiting ;2 is for called
        /// </summary>
        public string HJBZ;

        /// <summary>
        /// FZ'S TIME
        /// </summary>
        public DateTime? FZSJ;

        /// <summary>
        /// doctor's calling time
        /// </summary>
        public DateTime? CALLTIME;

        /// <summary>
        /// 0 is for normal, else is for export type
        /// </summary>
        public string expert_flag;
    }
}
