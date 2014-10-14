﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Reflection;

namespace AEDemo
{
    class Parameters
    {
        /// <summary>
        /// 地图控件
        /// </summary>
        public static IMapControl2 g_pMapControl;


        /// <summary>
        /// 地图文档
        /// </summary>
        public static IMapDocument g_pMapDoc;
   

        /// <summary>
        /// 地图文档的路径
        /// </summary>
        public static string g_sDocPath;
    

        /// <summary>
        /// 图层个数
        /// </summary>
        public static int g_iLayerCount;
 

        /// <summary>
        /// exe可执行文件路径
        /// </summary>
        public static string g_sProjectPath
        {
            get
            {
                return Assembly.GetExecutingAssembly().Location;
            }
        }

        /// <summary>
        /// 日志文件夹路径
        /// </summary>
        public static string g_sLogPath
        {
            get
            {
                return Assembly.GetExecutingAssembly().Location + "\\..\\..\\log\\";
            }
        }

        /// <summary>
        /// 背景音乐文件夹路径
        /// </summary>
        public static string g_sBgmPath
        {
            get
            {
                return Assembly.GetExecutingAssembly().Location + "\\..\\..\\bgm\\";
            }
        }

        /// <summary>
        /// 音乐播放状态标识符
        /// </summary>
        public static bool g_bPlayMusic = false;
   

        /// <summary>
        /// 系统时间
        /// </summary>
        public static DateTime g_DateTime
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
