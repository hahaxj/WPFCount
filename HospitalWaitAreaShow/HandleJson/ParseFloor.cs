using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow.HandleJson
{


    public sealed class ParseFloor
    {
        
        private static readonly ParseFloor _singleton = new ParseFloor();
        private static AllFloorWaitAreaList _floorList;

        private ParseFloor()
        {
            GetJson();
        }

        //public static ParseJson JsonInstance
        //{
        //    get
        //    {
        //        return _singleton;
        //    }

        //}

        public static IReadOnlyList<FloorInfoList> FloorWaitAreaList
        {
            get;
            private set;
        }

        private void GetJson()
        {
            log4net.ILog logger = LogHelper.logerror;
            //读取json文件  
            using (StreamReader sr = new StreamReader("Asset/WaitAreaList.json"))
            {
                try
                {                   

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    serializer.NullValueHandling = NullValueHandling.Ignore;

                    //构建Json.net的读取流  
                    JsonReader reader = new JsonTextReader(sr);
                    //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                    _floorList = serializer.Deserialize<AllFloorWaitAreaList>(reader);

                    if (null != _floorList && null != _floorList.FloorInfoList && _floorList.FloorInfoList.Count >0 )
                    {
                        FloorWaitAreaList = _floorList.FloorInfoList;
                    }
                    else
                    {
                        
                        logger.Error("请在WaitAreaList.json中输入正确的科室-楼层对应关系");
                    }
                }
                catch (Exception ex)
                {

                    logger.Error(ex);
                }
            }
        }

        //public static ParseJson GetSingleton()
        //{
        //    if (null == _singleton)
        //    {
        //        lock (lockobj)
        //        {
        //            if (null == _singleton)
        //            {
        //                _singleton = new ParseJson();
        //            }
        //        }
        //    }

        //    return _singleton;
        //}

    }
}
