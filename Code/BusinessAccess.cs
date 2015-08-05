using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.IO;

namespace DCTSetting
{
    class BusinessAccess
    {
        
        DataAccess objAccess = new DataAccess();
        Utility objUti = new Utility();

        public IList<Models_Dtc_List> DCT_LIST()
        {
            return objAccess.DCT_LIST();
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

        public int Run_Slave_Command(SerialPort objport,string command)
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

        public void ResetForm(Form1 objform)
        {
            


        }
        public string SystemStatus(int status)
        {
            switch (status)
            {
                case 1:
                    return "Stand By";
                case 2:
                    return "RC Mode";
                case 3:
                    return "FL Mode";
                case 4:
                    return "OA Mode";
                default:
                    return "Unknown";

            }
        }



      
    }
}
