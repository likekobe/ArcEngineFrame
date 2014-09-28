using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System.IO;
using System.Reflection;
using DevExpress.XtraTreeList.Nodes;

namespace AEDemo
{
    /// <summary>
    /// 文件操作的类，入打开、保存、添加数据等操作
    /// </summary>
    class OperateFile
    {
        private static string m_sProjectPath = Assembly.GetExecutingAssembly().Location;
        private static string m_sLogPath = m_sProjectPath + "\\..\\..\\log\\";

        /// <summary>
        /// 打开Mxd文件
        /// </summary>
        /// <param name="frm">主窗体</param>
        /// <returns></returns>
        public static bool OpenFile(frmFrame frm)
        {
            bool bResult = false;
            OpenFileDialog OpenDlg = new OpenFileDialog();
            OpenDlg.Title = "打开地图文档";
            OpenDlg.Filter = "map documents(*.mxd)|*.mxd";
            if (OpenDlg.ShowDialog() == DialogResult.OK)
            {
                string sFilePath = OpenDlg.FileName;
                if (frm.axMapControl1.CheckMxFile(sFilePath))
                {
                    //// 设置鼠标指针的显示样式  ？？？？？但为什么设置两次
                    frm.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrowHourglass;
                    frm.axMapControl1.LoadMxFile(sFilePath, 0, Type.Missing);
                    frm.axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
                    LoadEagleEye(frm, sFilePath);
                    //// 全屏显示
                    frm.axMapControl1.Extent = frm.axMapControl1.FullExtent;
                    CommFunction.WriteLog(OpenDlg.Title, "打开地图文档成功。 文档路径：" + sFilePath);
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

        /// <summary>
        /// 显示鹰眼功能
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool LoadEagleEye(frmFrame frm, string FilePath)
        {
            bool bResult = false;
            if (frm.axMapControlEagelEye.CheckMxFile(FilePath))
            {
                frm.axMapControlEagelEye.MousePointer = esriControlsMousePointer.esriPointerArrowHourglass;
                frm.axMapControlEagelEye.LoadMxFile(FilePath, 0, Type.Missing);
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
            frm.axMapControlEagelEye.MapScale = frm.axMapControl1.MapScale * 4.0;
            IPoint pPoint = new PointClass();
            pPoint.X = (frm.axMapControl1.Extent.XMax + frm.axMapControl1.Extent.XMin) / 2;
            pPoint.Y = (frm.axMapControl1.Extent.YMax + frm.axMapControl1.Extent.YMin) / 2;

            //// 不显示水平、垂直滚动条
            frm.axMapControlEagelEye.ShowScrollbars = false;
            frm.axMapControlEagelEye.CenterAt(pPoint);
            frm.axMapControlEagelEye.Refresh();
        }

      
    }
}
