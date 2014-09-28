﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using DevExpress.XtraTreeList.Nodes;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using System.Data;

namespace AEDemo
{
    /// <summary>
    /// 公共方法的类
    /// </summary>
    class CommFunction
    {
        private static string m_sProjectPath = Assembly.GetExecutingAssembly().Location;
        private static string m_sLogPath = m_sProjectPath + "\\..\\..\\log\\";

        //private static uint SND_ASYNC = 0x0001;
        //private static uint SND_FILENAME = 0x00020000;
        /// <summary>
        /// mciSendString用于播放音乐
        /// </summary>
        /// <param name="lpstrCommand">要发送的命令字符串。字符串结构是:[命令][设备别名][命令参数]</param>
        /// <param name="lpstrReturnString">返回信息的缓冲区,为一指定了大小的字符串变量.</param>
        /// <param name="uReturnLength">缓冲区的大小,就是字符变量的长度.</param>
        /// <param name="hWndCallback">回调方式，一般设为零</param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        private static extern uint mciSendString(string lpstrCommand, string lpstrReturnString, uint uReturnLength, uint hWndCallback);

        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="LogName">日志名</param>
        /// <param name="LogContent">日志内容</param>
        /// <returns></returns>
        public static bool WriteLog(string LogName, string LogContent)
        {
            bool bResult = false;
            try
            {
                DateTime dateTime = System.DateTime.Now;
                string sTime = dateTime.GetDateTimeFormats('s')[0].ToString();
                sTime = sTime.Replace(":", "").ToString();
                LogName = LogName + "(" + sTime + ").log";
                string sPath = m_sLogPath + LogName;

                StreamWriter streamWrite = null;
                streamWrite = new StreamWriter(sPath, false);
                streamWrite.WriteLine(LogContent);
                streamWrite.Close();        ////    一定要写这一行，不然写入不了
                bResult = true;
            }
            catch (Exception ex)
            {
                WriteLog("写入日志失败", ex.ToString());
                bResult = false;
            }

            return bResult;

        }

        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="Path">日志文件夹路径</param>
        /// <returns></returns>
        public static bool ShowLog(frmShowLog frm)
        {
            bool bResult = false;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(m_sLogPath);
                FileInfo[] files = dir.GetFiles();
                FileAttributes fileAttributes;

                //// 遍历所有文件
                foreach (FileInfo file in files)
                {
                    if (file.Extension == ".log")
                    {
                        //// 忽略隐藏文件
                        fileAttributes = file.Attributes & FileAttributes.Hidden;
                        if (fileAttributes != FileAttributes.Hidden)
                        {
                            FileStream fs = new FileStream(file.FullName, FileMode.Open);
                            StreamReader reader = new StreamReader(fs, Encoding.UTF8);

                            //// 循环读取文件
                            string sSum = string.Empty;
                            while (true)
                            {
                                string str = reader.ReadLine();
                                if (str == null)
                                {
                                    break;
                                }
                                else if (str.Trim() == "")
                                {
                                    continue;
                                }
                                else
                                {
                                    //// 这里就是每行数据了  你可以进行处理 取出符合要求的行
                                    sSum = sSum + str;
                                }
                            }

                            frm.treeListShowLog.AppendNode(new object[] { file, sSum }, null);
                        }
                    }
                }

                bResult = true;
            }
            catch (Exception ex)
            {
                WriteLog("显示日志失败", ex.ToString());
                bResult = false;
            }
            return bResult;

        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="Play">是否正在播放的标识</param>
        public static void PlayMusic(bool Play)
        {
            string sMusicPath = m_sProjectPath + "\\..\\..\\bgm\\安妮的仙境(annie's w'onderland).mp3";
            mciSendString("close temp_alias", null, 0, 0);
            //mciSendString(sCmd, null, 0, 0);

            //// ？？？？？怎么设置相对路径啊，格式总写不对
            mciSendString(@"open ""E:\LIKE\AEDemo\AEDemo\bin\x86\bgm\安妮的仙境(annie's w'onderland).mp3"" alias temp_alias", null, 0, 0);
            if (Play == false)
            {
                mciSendString("play temp_alias repeat", null, 0, 0);
                frmFrame.g_bPlayMusic = true;
            }
            else if (Play == true)
            {
                mciSendString("close temp_alias ", null, 0, 0);
                frmFrame.g_bPlayMusic = false;
            }
        }

        /// <summary>
        /// 显示图层属性
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        public static bool ShowLayerProperty(frmLayerProperty frm, ILayer Layer)
        {
            bool bResult = false;
            try
            {
                IFeatureLayer pFeaLayer = Layer as IFeatureLayer;
                IFeatureClass pFeaClass = pFeaLayer.FeatureClass;
                //// 获取字段个数
                int iFieldCount = pFeaClass.Fields.FieldCount;

                frm.listData.Items.Clear();
                frm.listData.Items.Add("图层别名：    " + pFeaClass.AliasName);
                frm.listData.Items.Add(" ");
                frm.listData.Items.Add("几何类型：    " + pFeaClass.ShapeType);
                frm.listData.Items.Add(" ");
                frm.listData.Items.Add("要素类型：    " + pFeaClass.FeatureType);
                frm.listData.Items.Add(" ");
                frm.listData.Items.Add("要素个数：    " + pFeaClass.FeatureCount(null));
                frm.listData.Items.Add(" ");
                frm.listData.Items.Add("字段个数：    " + pFeaClass.Fields.FieldCount);
                frm.listData.Items.Add(" ");
                frm.listData.Items.Add("OID字段名：   " + pFeaClass.OIDFieldName.ToString());
                frm.listData.Items.Add(" ");

                DataTable dt = new DataTable();
                dt.Columns.Add("名称：");
                dt.Columns.Add("别名：");
                dt.Columns.Add("类型：");
                dt.Columns.Add("长度：");
                dt.Columns.Add("精度：");

                for (int i = 0; i < iFieldCount;i++ )
                {
                    DataRow dr = dt.NewRow();
                    dr["名称："]=pFeaClass.Fields.get_Field(i).Name;
                    dr["别名："]=pFeaClass.Fields.get_Field(i).AliasName;
                    dr["类型："]=pFeaClass.Fields.get_Field(i).Type;
                    dr["长度："]=pFeaClass.Fields.get_Field(i).Length;
                    dr["精度："] = pFeaClass.Fields.get_Field(i).Precision;
                    dt.Rows.Add(dr);
                }
                frm.gcField.DataSource = dt;
                frm.gvField.BestFitColumns();

                bResult = true;
            }
            catch (Exception ex)
            {
                CommFunction.WriteLog("显示图层属性失败", ex.ToString());
                bResult = false;
            }
            finally
            {

            }
            return bResult;
        }

        /// <summary>
        /// 显示属性的详细信息
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        public static bool ShowPropertyDetails(frmPropertyDetails frm, ILayer Layer)
        {
            bool bResult = false;
            try
            {
                bResult = true;
            }
            catch (Exception ex)
            {
                bResult = false;
            }
            finally
            {

            }
            return bResult;
        }
    }
}
