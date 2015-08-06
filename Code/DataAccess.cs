using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using Microsoft.ApplicationBlocks.Data;
using MySql.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.IO;




namespace DCTSetting
{
    
    class DataAccess
    {
       
        public static string Conn  = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlconnectionString"].ToString();
       
        Utility objUti = new Utility();
        static string strSql;
        public string ErrorMsg = "";


        public static Models_CarParement Car_Parement(string[] strData)
        {

            Models_CarParement group = new Models_CarParement();

            group.carStatus = Convert.ToInt32(strData[0]);
            group.sysVoltage = Convert.ToDouble(strData[1]) / 1000d;
            group.sysErr = Convert.ToInt32(strData[2]);
            group.expectedV = Convert.ToDouble(strData[3]) / 1000d;
            group.expectedW = Convert.ToInt32(strData[4]);
            group.actualV = Convert.ToDouble(strData[5]) / 1000d;
            group.actualW = Convert.ToInt32(strData[6]);
            group.actualAL = Convert.ToDouble(strData[7]) / 1000d;
            group.actualAR = Convert.ToDouble(strData[8]) / 1000d;
            group.pitch = Convert.ToInt32(strData[9]);
            group.roll = Convert.ToInt32(strData[10]);

            group.IEEE_HS = Convert.ToInt32(strData[11]);
            group.RSSI_HS = Convert.ToInt32(strData[12]);
            group.soundWidthL_HS = Convert.ToInt32(strData[13]);
            group.soundWidthR_HS = Convert.ToInt32(strData[14]);
            group.distance_HS = Convert.ToDouble(strData[15]) / 1000d;
            group.angle_HS = Convert.ToInt32(strData[16]);

            group.IEEE_OA = Convert.ToInt32(strData[17]);
            group.RSSI_OA = Convert.ToInt32(strData[18]);
            group.obsticleDistance = Convert.ToDouble(strData[19]) / 1000d;
            group.obsticleAngle = Convert.ToInt32(strData[20]);
            group.soundWidthL_OA = Convert.ToInt32(strData[21]);
            group.soundWidthR_OA = Convert.ToInt32(strData[22]);
            group.cnt_connected = 20;

            return group;
        }
        public static Models_Slave get_slave_status(string[] strData)
        {

            Models_Slave group = new Models_Slave();
            if (strData[0] == "CMD_flags")
            {
                if ((Convert.ToInt32(strData[1]) & 0x01) == 0x01)
                {
                    group.hold = true;
                    group.release = false;
                }
                else
                {
                    group.hold = false;
                    group.release = true;
                }

                if ((Convert.ToInt32(strData[1]) & 0x02) == 0x02)
                {
                    group.up = true;
                    group.down = false;
                }
                else
                {
                    group.up = false;
                    group.down = true;
                }

                if ((Convert.ToInt32(strData[1]) & 0x04) == 0x04) //“现在测试大电流负载”
                {
                    group.short_circuit = true;
                    group.open_circuit = false;
                }
                else
                {
                    group.short_circuit = false;
                    group.open_circuit = true;
                }
                group.cnt_Connected = 20;
            }
            else
            {
                group.hold = false;
                group.release = false;
                group.up = false;
                group.down = false;
                group.short_circuit = false;
                group.open_circuit = false;
            }


            return group;
        }


        public IList<Models_Dtc_List> DCT_LIST()
        {
            strSql = "SELECT c.categoryname, d.id, d.line, d.dct_name, d.dct_ip, d.sortby FROM sfis_base_category c " +
                "left join sfis_base_dtcname d on c.id=d.category_id "+
                "where d.active=1 order by d.category_id, d.sortby";

            MySqlDataReader sdr = MySqlHelper.ExecuteReader(Conn, strSql);
            IList<Models_Dtc_List> list = new List<Models_Dtc_List>();
            while (sdr.Read())
            {
                Models_Dtc_List group = new Models_Dtc_List();

                group.categoryname = sdr["categoryname"].ToString().Trim();
                group.id = int.Parse(sdr["id"].ToString().Trim());
                //group.category_id = int.Parse(sdr["category_id"].ToString().Trim());
                group.line = short.Parse(sdr["line"].ToString().Trim());
                group.dct_name = sdr["dct_name"].ToString().Trim();
                group.dct_ip = sdr["dct_ip"].ToString().Trim();
                group.sortby = short.Parse(sdr["sortby"].ToString().Trim());
                list.Add(group);
            }
            
            sdr.Close();
            sdr.Dispose();
            return list;
        }

        public string CheckUser_access(string user_account, int stationid)
        {
            strSql = "SELECT u.id, u.username, p.item FROM sfis_base_workstation_premission p left join sfis_member u on p.p_id = u.id where u.username = '" + user_account + "' " +
                "and p.val=1 and p.productid=1";
            MySqlDataReader sdr = MySqlHelper.ExecuteReader(Conn, strSql);
            string result = "";
            while (sdr.Read())
            {
                if (sdr["item"].ToString().Trim().IndexOf(stationid.ToString()) > -1)
                    result = sdr["id"].ToString();



               // group.gunno = sdr["option_value"].ToString().Trim();


               
            }
            sdr.Close();
            sdr.Dispose();

            return result;
        }

        public static IList<Models_Process_name> ProcessInformation()
        {
            IList<Models_Process_name> list = new List<Models_Process_name>();
          

            using (StreamReader sr = new StreamReader("process.txt", Encoding.GetEncoding("GBK")))
            {
                string sLine = "";
                int inta = 0;
                while (!sr.EndOfStream) //读到结尾退出
                {

                    sLine = sr.ReadLine();
                    if (sLine.IndexOf("//") == -1 && (sLine != null && !sLine.Equals("")))
                    {
                        Models_Process_name group = new Models_Process_name();
                        string[] sp = sLine.Split(',');
                        group.id = inta;
                        group.processname = sp[0].ToString();
                        group.processname_desc = sp[1].ToString();
                        group.run_process = int.Parse(sp[2].ToString());
                        group.perid = int.Parse(sp[3].ToString());
                        group.is_auto = int.Parse(sp[4].ToString());
                        group.is_show = int.Parse(sp[5].ToString());

                        group.maxDV_actualV_expected = double.Parse(sp[6].ToString());
                        group.maxDV_actualW_expected = double.Parse(sp[7].ToString());
                        group.min_actualAL = double.Parse(sp[8].ToString());
                        group.max_actualAL = double.Parse(sp[9].ToString());
                        group.min_actualAR = double.Parse(sp[10].ToString());
                        group.max_actualAR = double.Parse(sp[11].ToString());
                        group.min_pitch = double.Parse(sp[12].ToString());
                        group.max_pitch = double.Parse(sp[13].ToString());
                        group.min_roll = double.Parse(sp[14].ToString());
                        group.max_roll = double.Parse(sp[15].ToString());
                        group.min_distance = double.Parse(sp[16].ToString());
                        group.max_distance = double.Parse(sp[17].ToString());
                        group.min_angle = double.Parse(sp[18].ToString());
                        group.max_angle = double.Parse(sp[19].ToString());
                        group.min_RSSI = double.Parse(sp[20].ToString());
                        group.max_RSSI = double.Parse(sp[21].ToString());
                        group.directory = sp[22].ToString();
                        group.checkbox_name = sp[23].ToString();
                        list.Add(group);
                        inta++;
                    }


                }
               


                return list;
            }

        }

        public int Save_Car_Parameters(string barcode, Models_CarParement_db group_db)
        {
            strSql = "Insert into sfis_platform_test_ct (barcode, " +
                "rc_l_actualV , rc_l_actualW , rc_l_actualAL , rc_l_actualAR , " +
                "rc_r_actualV , rc_r_actualW , rc_r_actualAL , rc_r_actualAR , " +
                "rc_b_actualV , rc_b_actualW , rc_b_actualAL , rc_b_actualAR , " +
                "rc_f_actualV , rc_f_actualW , rc_f_actualAL , rc_f_actualAR , " +
                "fl_3f_actualV , fl_3f_actualW , fl_3f_actualAL , fl_3f_actualAR , fl_3f_ieee , fl_3f_rssi , fl_3f_distance , fl_3f_angle , " +
                "fl_3l_actualV , fl_3l_actualW , fl_3l_actualAL , fl_3l_actualAR , fl_3l_ieee , fl_3l_rssi , fl_3l_distance , fl_3l_angle , " +
                "fl_3r_actualV , fl_3r_actualW , fl_3r_actualAL , fl_3r_actualAR , fl_3r_ieee , fl_3r_rssi , fl_3r_distance , fl_3r_angle , " +
                "fl_5f_actualV , fl_5f_actualW , fl_5f_actualAL , fl_5f_actualAR , fl_5f_ieee , fl_5f_rssi , fl_5f_distance , fl_5f_angle , " +
                "fl_5l_actualV , fl_5l_actualW , fl_5l_actualAL , fl_5l_actualAR , fl_5l_ieee , fl_5l_rssi , fl_5l_distance , fl_5l_angle , " +
                "fl_5r_actualV , fl_5r_actualW , fl_5r_actualAL , fl_5r_actualAR , fl_5r_ieee , fl_5r_rssi , fl_5r_distance , fl_5r_angle , " +
                "pitch , roll, active ) values ('" + barcode + "', " +
                group_db.rc_l_actualV + "," + group_db.rc_l_actualW + "," + group_db.rc_l_actualAL + "," + group_db.rc_l_actualAR + "," +
                group_db.rc_r_actualV + "," + group_db.rc_r_actualW + "," + group_db.rc_r_actualAL + "," + group_db.rc_r_actualAR + "," +
                group_db.rc_b_actualV + "," + group_db.rc_b_actualW + "," + group_db.rc_b_actualAL + "," + group_db.rc_b_actualAR + "," +
                group_db.rc_f_actualV + "," + group_db.rc_f_actualW + "," + group_db.rc_f_actualAL + "," + group_db.rc_f_actualAR + "," +
                group_db.fl_3f_actualV + "," + group_db.fl_3f_actualW + "," + group_db.fl_3f_actualAL + "," + group_db.fl_3f_actualAR + "," + group_db.fl_3f_ieee + "," + group_db.fl_3f_rssi + "," + group_db.fl_3f_distance + "," + group_db.fl_3f_angle + "," +
                group_db.fl_3l_actualV + "," + group_db.fl_3l_actualW + "," + group_db.fl_3l_actualAL + "," + group_db.fl_3l_actualAR + "," + group_db.fl_3l_ieee + "," + group_db.fl_3l_rssi + "," + group_db.fl_3l_distance + "," + group_db.fl_3l_angle + "," +
                group_db.fl_3r_actualV + "," + group_db.fl_3r_actualW + "," + group_db.fl_3r_actualAL + "," + group_db.fl_3r_actualAR + "," + group_db.fl_3r_ieee + "," + group_db.fl_3r_rssi + "," + group_db.fl_3r_distance + "," + group_db.fl_3r_angle + "," +
                group_db.fl_5f_actualV + "," + group_db.fl_5f_actualW + "," + group_db.fl_5f_actualAL + "," + group_db.fl_5f_actualAR + "," + group_db.fl_5f_ieee + "," + group_db.fl_5f_rssi + "," + group_db.fl_5f_distance + "," + group_db.fl_5f_angle + "," +
                group_db.fl_5l_actualV + "," + group_db.fl_5l_actualW + "," + group_db.fl_5l_actualAL + "," + group_db.fl_5l_actualAR + "," + group_db.fl_5l_ieee + "," + group_db.fl_5l_rssi + "," + group_db.fl_5l_distance + "," + group_db.fl_5l_angle + "," +
                group_db.fl_5r_actualV + "," + group_db.fl_5r_actualW + "," + group_db.fl_5r_actualAL + "," + group_db.fl_5r_actualAR + "," + group_db.fl_5r_ieee + "," + group_db.fl_5r_rssi + "," + group_db.fl_5r_distance + "," + group_db.fl_5r_angle + "," +
                group_db.pitch + "," + group_db.roll + "," + group_db.active + ")";
            try
            {
                
                Update(strSql);
                return 1;
            }
            catch
            {
                return 0;
                // get error or null value

            }
        }

        public static IList<Models_User_Info> CheckUser_Information(int userid)
        {
            strSql = "SELECT id, username ,fullname, picture, lastlogintime FROM sfis_member where id=" + userid;
            MySqlDataReader sdr = MySqlHelper.ExecuteReader(Conn, strSql);
            IList<Models_User_Info> list = new List<Models_User_Info>();
          
            while (sdr.Read())
            {
                Models_User_Info group = new Models_User_Info();

                group.id = int.Parse(sdr["id"].ToString());
                group.usrname = sdr["username"].ToString().Trim();
                group.fullname = sdr["fullname"].ToString().Trim();
                group.picture = sdr["picture"].ToString().Trim();
                group.lastlogintime = sdr["lastlogintime"].ToString().Trim();
               


                // group.gunno = sdr["option_value"].ToString().Trim();

                list.Add(group);
            }
            sdr.Close();
            sdr.Dispose();
            return list;
        }

        public string Update(string _strSql)
        {
            ErrorMsg = "";
            try
            {
                strSql = _strSql;
                SQLHelper.ExecuteNonQuery(Conn, CommandType.Text, strSql);
                return "";
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message + " " + strSql;
                return "error";
            }

        }

        public string ReturnSingleField(string _strsql)
        {
            try
            {
                return SQLHelper.ExecuteScalar(Conn, CommandType.Text, _strsql).ToString();
            }
            catch 
            {
                return ""; //ex..Message;
            }
        }

    }
}
