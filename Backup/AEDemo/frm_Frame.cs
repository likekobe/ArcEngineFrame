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
        public frmFrame()
        {
            InitializeComponent();
            axMapControlEagelEye.Visible = false;
            CommFunction.PlayMusic();
        }

        /// <summary>
        /// 按下Esc退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //// ！！！！！！点击Esc退出窗体后，会报错： 尝试读取或写入受保护的内存。这通常指示其他内存已损坏。
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
        // ！！！！！！！！！！！有问题啊，点击否 窗体还是会关闭
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
        /// 保存地图文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OperateFile.SaveDocument(this);
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OperateFile.SaveAsDocument(this);
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
            if (Parameters.g_iLayerCount > 0)
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
            CommFunction.PlayMusic();
        }

        /// <summary>
        /// 图层属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLayerProperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Parameters.g_iLayerCount > 0)
            {
                frmLayerProperty frm = new frmLayerProperty();
                //// ？？？？？并不显示在父窗体的中心，未设置子窗体的父窗体？？
                //// 解答：！！！！ 以ShowDialog()方式显示窗体，才会在父窗体中心显示
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
        /// 浏览属性的字段详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPropertyDetails_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Parameters.g_iLayerCount > 0)
            {
                frmPropertyDetails frm = new frmPropertyDetails();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Owner = this;
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
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //// 鼠标中键按下，实现地图拖动
            if (e.button==4)
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerHand;
                axMapControl1.Pan();  
            }
        }

        //// MapControl中，在使用其它工具后，会使鼠标滚轮缩放失效，可以通过加入MouseDown或MouseUp事件让地图获得焦点，使此功能持续有效。
        //// MapControl1在MouseUp后获得焦点，使滚轮放大缩小总是可用 
        private void axMapControl1_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            axMapControl1.Focus();
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

    }
}