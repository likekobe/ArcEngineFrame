﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace AEDemo
{
    public partial class frmPropertyDetails : DevExpress.XtraEditors.XtraForm
    {
        public frmPropertyDetails()
        {
            InitializeComponent();
            TreeListLoadLayer();
            CommFunction.ShowPropertyDetails(this, Parameters.g_pMapControl.get_Layer(0));
        }

        /// <summary>
        /// 在TreeList中加载图层名称
        /// </summary>
        private void TreeListLoadLayer()
        {
            int iLayerCount = Parameters.g_pMapControl.LayerCount;
            for (int i = 0; i < iLayerCount; i++)
            {
                string sLayerName = Parameters.g_pMapControl.get_Layer(i).Name;
                tlLayer.AppendNode(new object[] { sLayerName, i }, null);
            }
        }

        /// <summary>
        /// 所选树节点变化时，触发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlLayer_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = tlLayer.FocusedNode;
            int iLayerIndex = Convert.ToInt32(node.GetDisplayText(1));
            string sLayerName = node.GetDisplayText(0);
            ILayer pLayer = Parameters.g_pMapControl.get_Layer(iLayerIndex);
            //m_pLayer = pLayer;
            labelLayerName.Text = "图层 【" + sLayerName + "】 属性表";
            CommFunction.ShowPropertyDetails(this, pLayer);
        }

        /// <summary>
        /// 单击一行，高亮显示所选要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvFieldInfo_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            CommFunction.FlashShape(this,(frmFrame)this.Owner);
        }


    }
}