using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow.HandleJson
{

    public enum FloorNameEnum
    {
        First,
        Second,
        Third,
        Fourth
    }

    public class OfficeList
    {
        /// <summary>
        /// 110004
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 儿科门诊
        /// </summary>
        public string Name { get; set; }
    }

    public class WaitAreaInfo
    {
        /// <summary>
        /// 等待诊区名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// OfficeList
        /// </summary>
        public IReadOnlyList<OfficeList> OfficeList { get; set; }
    }

    public class FloorInfoList
    {
        /// <summary>
        /// WaitAreaInfo
        /// </summary>
        public IReadOnlyList<WaitAreaInfo> WaitAreaInfo { get; set; }
        /// <summary>
        /// 楼层信息
        /// </summary>
        public FloorNameEnum FloorName { get; set; }
    }

    public class AllFloorWaitAreaList
    {
        /// <summary>
        /// FloorInfoList
        /// </summary>
        public IReadOnlyList<FloorInfoList> FloorInfoList { get; set; }
    }
}
