﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO.Ports;
using System.IO;
using System.Xml;


namespace HeThongChoThueXe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceive);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ComList = SerialPort.GetPortNames();

            int[] ComNumberList = new int[ComList.Length];
            for (int i = 0; i < ComList.Length; i++)
            {
                ComNumberList[i] = int.Parse(ComList[i].Substring(3));
            }

            Array.Sort(ComNumberList);
            foreach (int ComNumber in ComNumberList)
            {
                cbxComList.Items.Add("COM" + ComNumber.ToString());
            }         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbxComList.Text == "")
            {
                MessageBox.Show("Vui lòng chọn cổng COM", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (serialPort.IsOpen)
            {
                serialPort.Close();
                button1.Text = "Kết nối";
                cbxComList.Enabled = true;
            }
            else
            {
                try
                {
                    serialPort.PortName = cbxComList.Text;
                    serialPort.Open();
                    button1.Text = "Ngắt kết nối";
                    cbxComList.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("Không thể mở cổng " + serialPort.PortName, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        
        Action<string> serialPortReceivedAction;

        private void DataReceive(object obj, SerialDataReceivedEventArgs e)
        {

            serialPortReceivedAction = serialPortReceiver;
            try
            {
                this.BeginInvoke(serialPortReceivedAction, serialPort.ReadLine());
            }
            catch { }

        }
        private void serialPortReceiver(string input)
        {
            string x1m = "X1M";
            string x1t = "X1T";
            string x2m = "X2M";
            string x2t = "X2T";
            string dataInput = input.Remove(3);
            if (string.Compare(dataInput, x1m, true) == 0)
            {
                tt1.Text = "Đã cho thuê";
                checkXe1.Checked = true;
                DateTime tg = DateTime.Now;
                time1m.Text = tg.ToString("HH:mm:ss");
                day1m.Text = tg.ToString("dd/M/yyyy");
                time1t.Text = "..........";
                day1t.Text = "..........";
                count1 = 1;
                timer1.Start();
                t1.Text = "0";
            }
            else if (string.Compare(dataInput, x1t, true) == 0)
            {
                tt1.Text = "Đã trả, đang tại bến";
                checkXe1.Checked = false;
                DateTime tg = DateTime.Now;
                time1t.Text = tg.ToString("HH:mm:ss");
                day1t.Text = tg.ToString("dd/M/yyyy");
                timer1.Stop();
                int tienx1 = count1 * 10000;
                t1.Text = tienx1.ToString();
            }
            else if (string.Compare(dataInput, x2m, true) == 0)
            {
                tt2.Text = "Đã cho thuê";
                checkXe2.Checked = true;
                DateTime tg = DateTime.Now;
                time2m.Text = tg.ToString("HH:mm:ss");
                day2m.Text = tg.ToString("dd/M/yyyy");
                time2t.Text = "..........";
                day2t.Text = "..........";
                count2 = 1;
                timer2.Start();
                t2.Text = "0";
            }
            else if (string.Compare(dataInput, x2t, true) == 0)
            {
                tt2.Text = "Đã trả, đang tại bến";
                checkXe2.Checked = false;
                DateTime tg = DateTime.Now;
                time2t.Text = tg.ToString("HH:mm:ss");
                day2t.Text = tg.ToString("dd/M/yyyy");
                timer2.Stop();
                int tienx2 = count2 * 10000;
                t2.Text = tienx2.ToString();
            }
        }

        int count1 = 1;
        int count2 = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            count1++;
        }

        
        private void timer2_Tick(object sender, EventArgs e)
        {
            count2++;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            string[] ComList = SerialPort.GetPortNames();

            int[] ComNumberList = new int[ComList.Length];
            for (int i = 0; i < ComList.Length; i++)
            {
                ComNumberList[i] = int.Parse(ComList[i].Substring(3));
            }

            Array.Sort(ComNumberList);
            foreach (int ComNumber in ComNumberList)
            {
                cbxComList.Items.Add("COM" + ComNumber.ToString());
                timer3.Stop();
            }
        }
    }
}
