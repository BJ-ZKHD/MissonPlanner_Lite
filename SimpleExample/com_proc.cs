using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;

namespace SimpleExample
{
    class com_proc:MAVLink
    {
        SerialPort _port = null;
        int timeout_ms_set = 0;
        Thread thr_read = null;
        bool read_work = false;  //启动读


        //init
        public com_proc()
        {

        }
        public com_proc(SerialPort port)
        {
            _port = port;
        }

        //发送
        public int send(byte[] send_data, int offset, int length)
        {
            if (_port == null)
            {
                return 0;
            }

            _port.Write(send_data, offset, length);

            //等待：如果超时未到达 或 数据没发送完成
            DateTime start = DateTime.Now;
            DateTime curr = DateTime.Now;

            int timeout_ms = 0;
            while (_port.BytesToWrite > 0 && (timeout_ms < timeout_ms_set))
            {
                curr = DateTime.Now;
                timeout_ms = DateTime.Compare(start, curr);
            }

            return _port.BytesToWrite;
        }

        //接收
        public int read()
        {
            if (thr_read == null)
            {
                thr_read = new Thread(new ThreadStart(read_proc));
                thr_read.Start();
            }

            return 0;
        }

        const int READ_BUFF_LEN = 2000;
        byte[][] para_list = new byte[0x100][];
        UInt32 para_list_index = 0 ; //限制在 0x100之内使用即可

        void read_proc()
        {
            byte[] read_buff = new byte[READ_BUFF_LEN];
            int read_num = 0;
            while (read_work)
            {
                do
                {
                    read_num = _port.Read(read_buff, 0, READ_BUFF_LEN);
                    if (read_num > 0)
                    {                       
                        for (int i = 0; i<read_num; i++)
                        {
                            //找到起始信息 可能有误
                            if (    (read_buff[i] == MAVLink.MAVLINK_STX)
                                 && ((i + 1) < read_num)  //确保长度数据有效
                                )
                            {
                                if ((i + read_buff[i + 1] + 8) < read_num) //总长度没有溢出有效buffer
                                {
                                    para_list[para_list_index] = new byte[read_buff[i + 1] + 8];
                                    Buffer.BlockCopy(read_buff,i, para_list[para_list_index],0, read_buff[i + 1] + 8);

                                    para_list_index++;
                                    if (para_list_index > 0x100) { para_list_index = 0; }
                                }                            

                            }
                        }

                    }
                } while (read_num == READ_BUFF_LEN); //未读完继续

            }
        }
        
        ~com_proc()
        {
            read_work = false;   
        }



    }
}
