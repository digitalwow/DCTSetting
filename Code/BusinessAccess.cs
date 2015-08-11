using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.IO;
using System.Net.Sockets;

namespace DCTSetting
{
    class BusinessAccess
    {
        public static Dictionary<string, Socket> Socket;
        public string DTC_Message , DTC_Name , DTC_Error = "" ;
       // public string DTC_Name = "";
        DataAccess objAccess = new DataAccess();
        Utility objUti = new Utility();

        public static byte[] byte_online = { 0x6b, 0xfe, 0x00, 0x95 };
        public static byte[] byte_dtc_info = { 0x6b, 0xfd, 0x00, 0x96 };
        //public socket_info()
        //{

        //    obj_socket = _obj_socket;
        //}

        //public static Dictionary<string, Socket> Socket
        //{
        //    get
        //    {
        //        return obj_socket;
        //    }
        //    set
        //    {
        //        obj_socket = value;
        //    }
        //}

        public IList<Models_Dtc_List> DCT_LIST()
        {
            return objAccess.DCT_LIST();
        }

        public static Int16 Send2DTC1(string strkey, byte[] obj_byte)
        {
            try
            {
                Socket[strkey].Send(obj_byte);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public static Int16 Send2DTC(ref Dictionary<string, Socket> obj_socket, string strkey, byte[] obj_byte)
        {
            try
            {
                obj_socket[strkey].Send(obj_byte);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public Int16 Send2DTC_Return(byte[] _objReceiver, int length, string strip)
        {
            string strmsg = System.Text.Encoding.UTF8.GetString(_objReceiver, 0, length);
            string[] strresult = strmsg.Split(',');
            try
            {
                if (strresult[1].ToString().Equals("10")) // DTC 中傳來訊息
                {
                    DTC_Name = strresult[0].ToString();
                    DTC_Message = strresult[2].ToString();
                    //CheckUser();
                    //CheckIQC();
                    //CheckPrev();
                    //CheckMyself();
                    //CheckComponent();
                    return 1;
                }
                else if (strresult[1].ToString().Equals("90"))
                    return 1;
                
                else
                    return 1;
            }
            catch
            {
                if (strresult[0].IndexOf("MY MAC") > -1)
                {
                    return Send2DTC1(strip, byte_online);
                    // Int16 int16 = Send2DTC(dict, strip, byte_temp);

                }
                return 0;
            }

        }

        static public string Parse2String(string strmsg, string strip)
        {
            // int step = workflow_setup[index_step];
            string[] strresult = strmsg.Split(',');
            if (strresult[1].ToString().Equals("10")) // DTC 中傳來訊息
            {
                //CheckUser();
                //CheckIQC();
                //CheckPrev();
                //CheckMyself();
                //CheckComponent();
            }
            else if (strresult[0].IndexOf("MY MAC") > -1)
            {
                Send2DTC1(strip, byte_online);
                // Int16 int16 = Send2DTC(dict, strip, byte_temp);
            }


            return "";
        }


        public int Run_Comport(SerialPort objport, string com)
        {
            try
            {
                if (objport.IsOpen)
                    return 1;


                objport.PortName = com; // com_t.Option_Value;
                objport.Parity = Parity.None;
                objport.DataBits = 8;
                objport.StopBits = StopBits.One;
                objport.BaudRate = 115200;
                objport.ReadTimeout = 0;
                objport.WriteTimeout = 1;
                objport.Open();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int Run_Con_car(SerialPort objport, byte[] command)
        {
            try
            {
                objport.Write(command, 0, 2);
                objport.Write(command, 0, 2);
                return 1;

            }
            catch
            {
                return 0;
            }
        }

        public int Run_Slave_Command(SerialPort objport, string command)
        {
            try
            {
                objport.Write(command);

                return 1;

            }
            catch
            {
                return 0;
            }
        }

        public string CheckUser(string user_account, int stationid)
        {
            string is_exit = "";
            is_exit = objAccess.CheckUser_access(user_account, stationid);



            return is_exit;

        }







    }
}
