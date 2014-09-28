using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Controls;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;

namespace AEDemo
{
    public partial class frmFrame : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 是否播放背景音乐的判断
        /// </summary>
        public static bool g_bPlayMusic;
        public static IMapControl2 g_pMapControl;
        public frmFrame()
        {
            InitializeComponent();
            axMapControlEagelEye.Visible = false;
            g_bPlayMusic = false;
            g_pMapControl = (IMapControl2)axMapControl1.Object ;
            CommFunction.PlayMusic(g_bPlayMusic);
        }

        /// <summary>
        /// 按下Esc退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFrame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ShowCloseFormTips();
            }
        }

        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEsc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCloseFormTips();
        }

        /// <summary>
        /// 主界面关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        //// ！！！！！！！！！！！有问题啊，点击否 窗体还是会关闭
        //private void frmFrame_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    ShowCloseFormTips();
        //}

        /// <summary>
        /// 关闭主界面给出提示
        /// </summary>
        private void ShowCloseFormTips()
        {
            if (MessageBox.Show("     确定要退出系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 打开Mxd文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenMxd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OperateFile.OpenFile(this);
        }

        /// <summary>
        /// 打开或关闭鹰眼界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEagleEye_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (axMapControlEagelEye.Visible == false)
            {
                axMapControlEagelEye.Visible = true;
            }
            else
            {
                axMapControlEagelEye.Visible = false;
            }
        }

        /// <summary>
        /// 地图移动时，鹰眼地图同步移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            OperateFile.SetLoadEagle(this);
        }

        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (axMapControl1.LayerCount > 0)
            {
                frmShowLog frm = new frmShowLog();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
            else
            {
                if (MessageBox.Show("未加载地图文档，是否加载地图文档？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnOpenMxd_ItemClick(null, null);
                }
            }
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlayMusic_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CommFunction.PlayMusic(g_bPlayMusic);
        }

        /// <summary>
        /// 图层属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLayerProperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (axMapControl1.LayerCount > 0)
            {
                frmLayerProperty frm = new frmLayerProperty();
                //// ？？？？？并不显示在父窗体的中心，未设置子窗体的父窗体
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Show();
            }
            else
            {
                if (MessageBox.Show("未加载地图文档，是否加载地图文档？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnOpenMxd_ItemClick(null, null);
                }
            }
        }

        /// <summary>
        /// 浏览属性的字段详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPropertyDetails_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (axMapControl1.LayerCount > 0)
            {
                frmPropertyDetails frm = new frmPropertyDetails();
                //// ？？？？？并不显示在父窗体的中心，未设置子窗体的父窗体
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Show();
            }
            else
            {
                if (MessageBox.Show("未加载地图文档，是否加载地图文档？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnOpenMxd_ItemClick(null, null);
                }
            }
        }
    }
}