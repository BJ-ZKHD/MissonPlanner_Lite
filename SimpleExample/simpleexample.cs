using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleExample
{
    public partial class simpleexample : Form
    {
        MAVLink.MavlinkParse mavlink = new MAVLink.MavlinkParse();
        bool armed = false;
        // locking to prevent multiple reads on serial port
        object readlock = new object();
        // our target sysid
        byte sysid;
        // our target compid
        byte compid;

        public simpleexample()
        {
            InitializeComponent();

            but_connect.Text = "启动连接";
            label1.Text = "当前状态：未连接...";
        }

        private void but_connect_Click(object sender, EventArgs e)
        {
            // if the port is open close it
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                but_connect.Text = "启动连接";
                label1.Text = "当前状态：未连接...";
                return;
            }

            // set the comport options
            serialPort1.PortName = CMB_comport.Text;
            serialPort1.BaudRate = int.Parse(cmb_baudrate.Text);

            // open the comport
            serialPort1.Open();

            // set timeout to 2 seconds
            serialPort1.ReadTimeout = 2000;

            BackgroundWorker bgw = new BackgroundWorker();

            bgw.DoWork += bgw_DoWork;

            bgw.RunWorkerAsync();

            //but_connect.Text = "连接成功";
            //but_connect.Enabled = false;
            but_connect.Text = "断开连接";
            label1.Text = "当前状态： 已连接成功";
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (serialPort1.IsOpen)
            {
                try
                {
                    MAVLink.MAVLinkMessage packet;
                    lock (readlock)
                    {
                        // read any valid packet from the port
                        packet = mavlink.ReadPacketObj(serialPort1.BaseStream);

                        // check its valid
                        if (packet == null || packet.data == null)
                            continue;
                    }

                    // check to see if its a hb packet from the comport
                    if (packet.data.GetType() == typeof(MAVLink.mavlink_heartbeat_t))
                    {
                        var hb = (MAVLink.mavlink_heartbeat_t)packet.data;

                        // save the sysid and compid of the seen MAV
                        sysid = packet.sysid;
                        compid = packet.compid;

                        // request streams at 2 hz
                        mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.REQUEST_DATA_STREAM,
                            new MAVLink.mavlink_request_data_stream_t()
                            {
                                req_message_rate = 2,
                                req_stream_id = (byte)MAVLink.MAV_DATA_STREAM.ALL,
                                start_stop = 1,
                                target_component = compid,
                                target_system = sysid
                            });
                    }

                    Console.WriteLine("MSGID:" + packet.msgid + " MESSID:" + packet.messid);

                    // from here we should check the the message is addressed to us
                    if (sysid != packet.sysid || compid != packet.compid)
                        continue;


                    Console.WriteLine("MSGID:"+packet.msgid+" MESSID:"+packet.messid);


                    if (packet.messid == (byte)MAVLink.MAVLINK_MSG_ID.ATTITUDE)
                    //or
                    //if (packet.data.GetType() == typeof(MAVLink.mavlink_attitude_t))
                    {
                        var att = (MAVLink.mavlink_attitude_t)packet.data;

                        Console.WriteLine(att.pitch * 57.2958 + " " + att.roll * 57.2958);
                    }
                    
                }
                catch
                {
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        T readsomedata<T>(byte sysid, byte compid, int timeout = 2000)
        {
            DateTime deadline = DateTime.Now.AddMilliseconds(timeout);
            lock (readlock)
            {
                // read the current buffered bytes  读取当前缓冲字节
                while (DateTime.Now < deadline)
                {
                    var packet = mavlink.ReadPacketObj(serialPort1.BaseStream);

                    // check its not null, and its addressed to us
                    if (packet == null)//|| sysid != packet.sysid || compid != packet.compid)
                        continue;

                    Console.WriteLine(packet);

                    //if (packet.data.GetType() == typeof(T))
                    //{
                    //    return (T)packet.data;
                    //}
                }
            }

            throw new Exception("No packet match found");//没有找到匹配的数据包
        }

        private void but_armdisarm_Click(object sender, EventArgs e)
        {
            MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t();

            req.target_system = 1;
            req.target_component = 1;

            req.command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM;

            req.param1 = armed ? 0 : 1;
            armed = !armed;

            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);

            //如果串口没打开，则返回
            if (!serialPort1.IsOpen)
                return;

            //发送
            serialPort1.Write(packet, 0, packet.Length);

            //尝试读取响应消息
            try
            {
                var ack = readsomedata<MAVLink.mavlink_command_ack_t>(sysid, compid);
                if (ack.result == (byte)MAVLink.MAV_RESULT.ACCEPTED)
                {

                }
            }
            catch
            {
            }
        }

        private void CMB_comport_Click(object sender, EventArgs e)
        {
            CMB_comport.DataSource = SerialPort.GetPortNames();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }




        //ARHS_GPS_USE
        private void button1_Click(object sender, EventArgs e)
        {
            MAVLink.mavlink_param_value_t  req = new MAVLink.mavlink_param_value_t();

            req.param_value = 0;
          
            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);

            //如果串口没打开，则返回
            if (!serialPort1.IsOpen)
                return;

            //发送
            serialPort1.Write(packet, 0, packet.Length);

            //尝试读取响应消息
            try
            {
                var ack = readsomedata<MAVLink.mavlink_command_ack_t>(sysid, compid);
                if (ack.result == (byte)MAVLink.MAV_RESULT.ACCEPTED)
                {

                }
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MAVLink.mavlink_param_request_read_t req = new MAVLink.mavlink_param_request_read_t();

            req.target_system = 1;
            req.target_component = 1;
            req.param_index = 1;  //读取第一个参数
            
            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.PARAM_REQUEST_READ, req);

            //如果串口没打开，则返回
            if (!serialPort1.IsOpen)
                return;

            //发送
            serialPort1.Write(packet, 0, packet.Length);

            //尝试读取响应消息
            try
            {
                var ack = readsomedata<MAVLink.mavlink_command_ack_t>(sysid, compid);
                if (ack.result == (byte)MAVLink.MAV_RESULT.ACCEPTED)
                {

                }
            }
            catch
            {
            }


        }
    }
}
