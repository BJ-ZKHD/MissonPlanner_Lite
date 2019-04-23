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

            string[] Ports = SerialPort.GetPortNames();
            if (Ports.Length > 0)
            { 
                CMB_comport.Text = Ports[0];
            }
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

                    Console.WriteLine("Message Type:" + packet.data.GetType() );

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
                    //电池状态
                    else if (packet.data.GetType() == typeof(MAVLink.mavlink_battery_status_t))
                    {

                    }
                    //GPS状态
                    else if (packet.data.GetType() == typeof(MAVLink.mavlink_gps_status_t))
                    {

                    }
                    //参数列表
                    else if (packet.data.GetType() == typeof(MAVLink.mavlink_param_request_list_t))
                    {


                    }
                    //参数
                    else if (packet.data.GetType() == typeof(MAVLink.mavlink_param_value_t))
                    {
                        Console.WriteLine("Message value:" + packet.data.ToString());
                        for (int i = 0; i < packet.buffer.Length; i++)
                        {
                            Console.WriteLine("[" + i + "]: 0x"+ packet.buffer[i].ToString("X2"));
                        }
                    }                    
                    //系统状态
                    else if (packet.data.GetType() == typeof(MAVLink.mavlink_sys_status_t))
                    {

                    }
                    //IMU状态
                    else if (packet.data.GetType() == typeof(MAVLink.mavlink_raw_imu_t))
                    {

                    }
                    else if (packet.data.GetType() == typeof(MAVLink.mavlink_attitude_t))
                    //or// else if (packet.messid == (byte)MAVLink.MAVLINK_MSG_ID.ATTITUDE)                    
                    {
                        var att = (MAVLink.mavlink_attitude_t)packet.data;

                        Console.WriteLine(att.pitch * 57.2958 + " " + att.roll * 57.2958);
                    }

                    Console.WriteLine(   " MSGID:" + packet.msgid + " MESSID:" + packet.messid   
                                        +" SYSID: "+ packet.sysid + " COMPID: " + packet.compid );

                    // from here we should check the the message is addressed to us
                    if (sysid != packet.sysid || compid != packet.compid)
                        continue;


                    //Console.WriteLine("MSGID:"+packet.msgid+" MESSID:"+packet.messid);                     
                    
                }
                catch
                {
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        T readsomedata<T>(byte sysid, byte compid, int timeout = 2000)
        {
            Console.WriteLine("enter: readsomedata");

            DateTime deadline = DateTime.Now.AddMilliseconds(timeout);
            lock (readlock)
            {
                // read the current buffered bytes  读取当前缓冲字节
                while (DateTime.Now < deadline)
                {
                    var packet = mavlink.ReadPacketObj(serialPort1.BaseStream);

                    // check its not null, and its addressed to us
                    if (packet == null)   //|| sysid != packet.sysid || compid != packet.compid)
                        continue;

                    Console.WriteLine(packet);
                }
            }

            throw new Exception("No packet match found");//没有找到匹配的数据包
        }

        //锁定/解锁
        private void but_armdisarm_Click(object sender, EventArgs e)
        {
            MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t();

            req.target_system = 1;
            req.target_component = 1;

            req.command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM;

            req.param1 = armed ? 0 : 1;
            armed = !armed;

            //生成数据包，发送
            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);
            ComSend(packet);

            //尝试读取响应消息
            //try
            //{
            //    var ack = readsomedata<MAVLink.mavlink_command_ack_t>(sysid, compid);
            //    if (ack.result == (byte)MAVLink.MAV_RESULT.ACCEPTED)
            //    {

            //    }
            //}
            //catch
            //{
            //}
        }

        //串口发送打包好的数据
        private bool ComSend(byte[] packet)
        {
            //如果串口没打开，则返回
            if (!serialPort1.IsOpen)
                return false;

            //发送
            serialPort1.Write(packet, 0, packet.Length);
            return true;
        }

        private void CMB_comport_Click(object sender, EventArgs e)
        {
            CMB_comport.DataSource = SerialPort.GetPortNames();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //读取参数命令
        private void btn_getPara_Click(object sender, EventArgs e)
        {
            var req = new MAVLink.mavlink_param_request_list_t
            {
                target_system = sysid,
                target_component = compid              
            };
                        
            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.PARAM_REQUEST_LIST, req);

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

        /// <summary>
        /// used to prevent comport access for exclusive use
        /// </summary>
        public bool giveComport
        {
            get { return _giveComport; }
            set { _giveComport = value; }
        }
        volatile bool _giveComport = false;

        //解析类对象
        MAVLink.MavlinkParse mavParse = new MAVLink.MavlinkParse();

        private void generatePacket(MAVLink.MAVLINK_MSG_ID messageType, object indata)
        {
            mavParse.GenerateMAVLinkPacket(messageType, indata);
        }


        public void GetALL_PARA()
        {
            // create new list so if canceled we use the old list
            MAVLink.MAVLinkParamList newparamlist = new MAVLink.MAVLinkParamList();

            int param_total = 1;

            MAVLink.mavlink_param_request_list_t req = new MAVLink.mavlink_param_request_list_t();
            req.target_system = sysid;
            req.target_component = compid;

            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.PARAM_REQUEST_LIST, req);



        }

        private void simpleexample_Load(object sender, EventArgs e)
        {

        }

        private void simpleexample_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1 != null)
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
            }
        }


        private void btn_setAHRS_GPS_USE_Click(object sender, EventArgs e)
        {
            var req = new MAVLink.mavlink_param_set_t
            {
                target_system = sysid,
                target_component = compid,
                param_type = 9  ///real32  MAV_PARAM_TYPE
            };

            byte[] temp = Encoding.ASCII.GetBytes("AHRS_GPS_USE");
            Array.Resize(ref temp, 16);
            req.param_id = temp;

            req.param_value = Single.Parse(tb_GPS_USE_val.Text); //解析设置值

            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.PARAM_REQUEST_READ, req);

            //如果串口没打开，则返回
            if (!serialPort1.IsOpen)
                return;

            //发送、

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

        private void btn_getAHRS_GPS_USE_Click(object sender, EventArgs e)
        {
            var req = new MAVLink.mavlink_param_request_read_t
            {
                target_system = sysid,
                target_component = compid,
                param_index = -1  // use param_index
            };

            //通过名称查询ID
            byte[] temp = Encoding.ASCII.GetBytes("AHRS_GPS_USE");
            Array.Resize(ref temp, 16);
            req.param_id = temp;

            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.PARAM_REQUEST_READ, req);

            //如果串口没打开，则返回
            if (!serialPort1.IsOpen)
                return;

            //发送、

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

        private void btn_heartbeat_Click(object sender, EventArgs e)
        {
            start_heartbeat();
        }

        //启动心跳包
        private void start_heartbeat()
        {
            MAVLink.mavlink_heartbeat_t hb = new MAVLink.mavlink_heartbeat_t()
            {
                autopilot = 1,
                base_mode = 2,
                custom_mode = 3,
                mavlink_version = 2,
                system_status = 6,
                type = 7
            };

            //生成数据包，发送
            byte[] packet = mavlink.GenerateMAVLinkPacket(MAVLink.MAVLINK_MSG_ID.HEARTBEAT, hb);
            ComSend(packet);

        }


    }
}
