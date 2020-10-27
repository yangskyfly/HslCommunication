﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Profinet;
using HslCommunication;
using HslCommunication.ModBus;
using System.Threading;

namespace HslCommunicationDemo
{
    public partial class FormS7Server : HslFormContent
    {
        public FormS7Server( )
        {
            InitializeComponent( );
        }

        private void FormSiemens_Load( object sender, EventArgs e )
        {
            panel2.Enabled = false;


            if(Program.Language == 2)
            {
                Text = "S7 Virtual Server [data support i,q,m,db block read and write, db block only one, whether it is DB1.1 or DB100.1 refers to the same]";
                label3.Text = "port:";
                button1.Text = "Start Server";
                button11.Text = "Close Server";
                label11.Text = "This server is not a strict S7 protocol and only supports perfect communication with HSL components.";
            }
        }
        
        private void FormSiemens_FormClosing( object sender, FormClosingEventArgs e )
        {
            s7NetServer?.ServerClose( );
        }

        #region Server Start


        private HslCommunication.Profinet.Siemens.SiemensS7Server s7NetServer;

        private void button1_Click( object sender, EventArgs e )
        {
            if (!int.TryParse( textBox2.Text, out int port ))
            {
                MessageBox.Show( DemoUtils.PortInputWrong );
                return;
            }

            try
            {

                s7NetServer = new HslCommunication.Profinet.Siemens.SiemensS7Server( );                       // 实例化对象
                s7NetServer.ActiveTimeSpan = TimeSpan.FromHours( 1 );
                s7NetServer.OnDataReceived += BusTcpServer_OnDataReceived;
                
                s7NetServer.ServerStart( port );

                userControlReadWriteServer1.SetReadWriteServer( s7NetServer, "M100" );
                button1.Enabled = false;
                panel2.Enabled = true;
                button11.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }


        private void button11_Click( object sender, EventArgs e )
        {
            // 停止服务
            s7NetServer?.ServerClose( );
            button1.Enabled = true;
            button11.Enabled = false;
        }

        private void BusTcpServer_OnDataReceived( object sender, byte[] receive )
        {
            // 可以对报文二次处理
        }

        #endregion
    }
}
