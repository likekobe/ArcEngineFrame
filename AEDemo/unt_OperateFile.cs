﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System.IO;
using System.Reflection;
using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.Carto;

namespace AEDemo
{
    /// <summary>
    /// 文件操作的类，入打开、保存、添加数据等操作
    /// </summary>
    class OperateFile
    {
        /// <summary>
        /// 打开Mxd文件
        /// </summary>
        /// <param name="frm">主窗体</param>
        /// <returns></returns>
        public static bool OpenFile(frmFrame frm)
        {

            IMapDocument pMapDoc = new MapDocumentClass();
            bool bResult = false;
            OpenFileDialog OpenDlg = new OpenFileDialog();
            OpenDlg.Title = "打开地图文档";
            OpenDlg.Filter = "map documents(*.mxd)|*.mxd";
            if (OpenDlg.ShowDialog() == DialogResult.OK)
            {
                string sFilePath = OpenDlg.FileName;
                if (frm.axMapControl1.CheckMxFile(sFilePath))
                {
                    ////// 设置鼠标指针的显示样式  ？？？？？但为什么设置两次
                    //frm.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrowHourglass;
                    //frm.axMapControl1.LoadMxFile(sFilePath, 0, Type.Missing);
                    //frm.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
                    //LoadEagleEye(frm, sFilePath);
                    ////// 全屏显示
                    //frm.axMapControl1.Extent = frm.axMapControl1.FullExtent;

                    pMapDoc.Open(sFilePath, "");
                    for (int i = 0; i < pMapDoc.MapCount; i++)
                    {
                        frm.axMapControl1.Map = pMapDoc.get_Map(i);
                    }

                    //// 把需要用到的地图参数都传入Parameters类中，便于读取
                    Parameters.g_pMapControl = (IMapControl2)frm.axMapControl1.Object;
                    Parameters.g_pMapDoc = pMapDoc;
                    Parameters.g_sDocPath = sFilePath;
                    Parameters.g_iLayerCount = frm.axMapControl1.LayerCount;
                    
                    //// 显示鹰眼
                    LoadEagleEye(frm);
                    frm.axMapControl1.Refresh();
                    CommFunction.WriteLog(OpenDlg.Title, "打开地图文档成功。 地图文档路径：" + sFilePath);
                    bResult = true;
                }
                else
                {
                    MessageBox.Show(sFilePath + "不是有效的地图文档。");
                    CommFunction.WriteLog(OpenDlg.Title, "打开地图文档失败，不是有效的地图文档。");
                    bResult = false;
                }
            }

            return bResult;
        }

        ///// <summary>
        ///// 拉框放大
        ///// </summary>
        //public static void ZoomIn(frmFrame frm)
        //{

        //    Parameters.g_pMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
        //    IEnvelope pEnv;
        //    pEnv = Parameters.g_pMapControl.TrackRectangle();
        //    frm.axMapControl1.Extent = pEnv;
        //    frm.axMapControl1.Refresh();
            
        //}

        /// <summary>
        /// 显示鹰眼功能
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool LoadEagleEye(frmFrame frm)
        {
            bool bResult = false;
            if (frm.axMapControlEagelEye.CheckMxFile(Parameters.g_sDocPath))
            {
                frm.axMapControlEagelEye.MousePointer = esriControlsMousePointer.esriPointerArrowHourglass;
                frm.axMapControlEagelEye.LoadMxFile(Parameters.g_sDocPath, 0, Type.Missing);
                frm.axMapControlEagelEye.MousePointer = esriControlsMousePointer.esriPointerDefault;
                SetLoadEagle(frm);
                bResult = true;
            }
            else
            {
                bResult = false;
            }
            return bResult;

        }

        /// <summary>
        /// 设置鹰眼显示中心点
        /// </summary>
        /// <param name="frm"></param>
        public static void SetLoadEagle(frmFrame frm)
        {
            frm.axMapControlEagelEye.MapScale = Parameters.g_pMapControl.MapScale * 4.0;
            IPoint pPoint = new PointClass();
            pPoint.X = (Parameters.g_pMapControl.Extent.XMax + Parameters.g_pMapControl.Extent.XMin) / 2;
            pPoint.Y = (Parameters.g_pMapControl.Extent.YMax + Parameters.g_pMapControl.Extent.YMin) / 2;

            //// 不显示水平、垂直滚动条
            frm.axMapControlEagelEye.ShowScrollbars = false;
            frm.axMapControlEagelEye.CenterAt(pPoint);
            frm.axMapControlEagelEye.Refresh();
        }

        /// <summary>
        /// 保存地图文档
        /// </summary>
        /// <param name="frm"></param>
        public static void SaveDocument(frmFrame frm)
        {
            try
            {
                if (Parameters.g_pMapDoc == null)
                {
                    MessageBox.Show("地图文档对象为空，请先加载地图文档。");
                }
                else
                {
                    if (Parameters.g_pMapDoc.get_IsReadOnly(Parameters.g_pMapDoc.DocumentFilename) == true)
                    {
                        MessageBox.Show("地图文档是只读的，无法保存。");
                    }
                    else
                    {
                        Parameters.g_pMapDoc.Save(Parameters.g_pMapDoc.UsesRelativePaths, true);
                        CommFunction.WriteLog("保存地图文档成功", "保存地图文档成功。 地图文档路径："+Parameters.g_sDocPath);
                        MessageBox.Show("保存地图文档成功。");
                    }
                }
            }
            catch (Exception ex)
            {
                CommFunction.WriteLog("保存地图文档失败", ex.ToString());
            }
        }

        /// <summary>
        /// 地图文档另存为
        /// </summary>
        /// <param name="frm"></param>
        public static void SaveAsDocument(frmFrame frm)
        {
            try
            {
                if (Parameters.g_pMapDoc == null)
                {
                    MessageBox.Show("地图文档对象为空，请先加载地图文档。");
                }
                else
                {
                    if (Parameters.g_pMapDoc.get_IsReadOnly(Parameters.g_pMapDoc.DocumentFilename) == true)
                    {
                        MessageBox.Show("地图文档是只读的，无法保存。");
                    }
                    else
                    {
                        SaveFileDialog SaveDlg = new SaveFileDialog();
                        SaveDlg.Filter = "Map Document(*mxd)|*mxd";
                        SaveDlg.Title = "另存为地图文档";

                        if (SaveDlg.ShowDialog() == DialogResult.OK)
                        {
                            string sSavePath = SaveDlg.FileName+".mxd";
                            if (string.IsNullOrEmpty(sSavePath))
                            {
                                return;
                            }
                            else if (sSavePath.Equals(Parameters.g_pMapDoc.UsesRelativePaths))
                            {
                                SaveDocument(frm);
                            }
                            else
                            {
                                Parameters.g_pMapDoc.SaveAs(sSavePath, true, true);
                                CommFunction.WriteLog("另存为地图文档成功", "另存为地图文档成功。 地图文档另存为路径：" + sSavePath);
                                MessageBox.Show("另存为地图文档成功。");
                            }
                        }

                    } 
 
                }
            }
            catch (Exception ex)
            {
                CommFunction.WriteLog("保存地图文档失败", ex.ToString());

            }
        }

    


    }
}
