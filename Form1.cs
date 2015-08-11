using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using CCWin;
using System.Threading;
using System.Net.Sockets;

namespace DCTSetting
{
    public partial class Form1 : CCSkinMain
    {
        private IList<Models_Dtc_List> objDtc_List;
        private delegate void AddTextBoxHandler(TextBox objbox, string value);
        private delegate void AddListItemHandler(ListBox objList, string value);
        private delegate void RemoveListItemHandler(ListBox objList, string value);
        private delegate void RunText(TextBox objbox, ListBox objList);
        Thread threadWatch = null; // 负责监听客户端连接请求的 线程；  
        Socket socketWatch = null;
        Utility objUti = new Utility();
        Dictionary<string, Socket> dict = new Dictionary<string, Socket>();
        Dictionary<string, Thread> dictThread = new Dictionary<string, Thread>();
        BusinessAccess objAccess;
        public Form1()
        {
            InitializeComponent();
            // objDtc_List = objAccess.DCT_LIST();
            Run();
        }

        private void Run()
        {

            ListItem item_type1 = new ListItem("@", "訊息");
            ListItem item_type2 = new ListItem("#", "Parse");
            ListItem item_type3 = new ListItem("~", "電位");
            cbxtype.Items.Add(item_type1);
            cbxtype.Items.Add(item_type2);
            cbxtype.Items.Add(item_type3);
            cbxtype.SelectedIndex = 0;
            cbxtype.SelectedItem = item_type2;
            ListItem item_success1 = new ListItem("0A", "成功");
            ListItem item_success2 = new ListItem("14", "提示");
            ListItem item_success3 = new ListItem("1E", "錯誤");
            cbxsuccess.Items.Add(item_success1);
            cbxsuccess.Items.Add(item_success2);
            cbxsuccess.Items.Add(item_success3);
            cbxsuccess.SelectedIndex = 0;
            cbxsuccess.SelectedItem = item_success1;
            cbxtype.DisplayMember = this.cbxsuccess.DisplayMember = "Name";
            cbxtype.ValueMember = this.cbxsuccess.ValueMember = "ID";

            ListItem item_cbxvoice1 = new ListItem("1", "不响");
            ListItem item_cbxvoice2 = new ListItem("2", "長音");
            ListItem item_cbxvoice3 = new ListItem("3", "短音");
            cbxvoice.Items.Add(item_cbxvoice1);
            cbxvoice.Items.Add(item_cbxvoice2);
            cbxvoice.Items.Add(item_cbxvoice3);
            cbxvoice.SelectedIndex = 0;
            cbxvoice.SelectedItem = item_cbxvoice2;
            cbxvoice.DisplayMember = "Name";
            cbxvoice.DisplayMember = "ID";

            ListItem item_light1 = new ListItem("1", "不亮");
            ListItem item_light2 = new ListItem("2", "長亮");
            ListItem item_light3 = new ListItem("3", "短亮");
            cbxlight.Items.Add(item_light1);
            cbxlight.Items.Add(item_light2);
            cbxlight.Items.Add(item_light3);
            cbxlight.SelectedIndex = 0;
            cbxlight.SelectedItem = item_light2;
            cbxlight.DisplayMember = "Name";
            cbxlight.DisplayMember = "ID";



        }


        private void CheckIP(CCWin.SkinControl.SkinTextBox objtext, string text)
        {
            try
            {
                IPAddress IP_end;
                IP_end = IPAddress.Parse(objtext.Text);
            }
            catch
            {
                MessageBox.Show(text);
                objtext.Focus();
                return;
            }
        }

        private void CheckInteger(CCWin.SkinControl.SkinTextBox objtext, string text)
        {
            try
            {
                int a = Int16.Parse(objtext.Text);
            }
            catch
            {
                MessageBox.Show(text);
                objtext.Focus();
                return;
            }
        }



        private void txtsrvip_Leave(object sender, EventArgs e)
        {
            CheckIP(txtsrvip, "Server IP Address Format Error");
        }

        private void txtdctip_Leave_1(object sender, EventArgs e)
        {
            CheckIP(txtdctip, "DCT IP Address Format Error");
        }

        private void txtgeteway_Leave(object sender, EventArgs e)
        {
            CheckIP(txtgeteway, "GateWay IP Address Format Error");
        }

        private void txtnetmask_Leave(object sender, EventArgs e)
        {
            CheckIP(txtnetmask, "Net Mask IP Address Format Error");
        }

        private void txtport_Leave(object sender, EventArgs e)
        {
            CheckInteger(txtport, "Port Format Error");
        }

        private bool CheckAll_Field()
        {
            if (txtdctname.Text.Trim().Length == 0 || txtdctip.Text.Trim().Length == 0 ||
                txtgeteway.Text.Trim().Length == 0 || txtnetmask.Text.Trim().Length == 0 ||
                txtport.Text.Trim().Length == 0 || txtsrvip.Text.Trim().Length == 0)
            {
                MessageBox.Show("All field must input.");
                return false;
            }
            else
                return true;
        }

        public byte[] dct_content = new byte[36];
        private void btnsubmit_Click(object sender, EventArgs e)
        {

            string result = "";
            dct_content[0] = 0x6B;
            dct_content[1] = 0xFF;
            dct_content[2] = 0x20;

            if (CheckAll_Field()) //do write DCT
            {
                byte[] array = System.Text.Encoding.ASCII.GetBytes(txtdctname.Text.Trim());  //数组array为对应的ASCII数组     
                string ASCIIstr2 = null;
                for (int i = 0; i < array.Length; i++)
                {
                    dct_content[3 + i] = array[i];
                    int asciicode = (int)(array[i]);
                    ASCIIstr2 += Convert.ToString(asciicode);//字符串ASCIIstr2 为对应的ASCII字符串
                }
                ParseIP2Byte(txtdctip.Text, 11, ref dct_content);// DCT IP
                ParseIP2Byte(txtgeteway.Text, 15, ref dct_content);// geteway IP
                ParseIP2Byte(txtnetmask.Text, 19, ref dct_content);// netmask IP
                ParseIP2Byte(txtsrvip.Text, 23, ref dct_content);// Server IP
                string port16 = Convert.ToString(int.Parse(txtport.Text), 16);
                ParsePort2Byte(port16.Substring(0, 2), 27, ref dct_content);//port
                ParsePort2Byte(port16.Substring(2, 2), 28, ref dct_content); //port                
                ParsePort2Byte(port16.Substring(0, 2), 29, ref dct_content); //port
                ParsePort2Byte(port16.Substring(2, 2), 30, ref dct_content); //port
                ParseIP2Byte("0.0.0.0", 31, ref dct_content); //netaddress

                Byte xor = 0;
                for (Byte ii = 0; ii < 35; ii++)
                {
                    xor ^= dct_content[ii];
                }
                dct_content[35] = xor;


                for (int i = 0; i < dct_content.Length; i++)
                {
                    result += " " + dct_content[i].ToString("X2");
                    //a.ToString("x")
                    //if (i==20)
                    //    result += "\n" + dct_content[i].ToString();
                    //else
                    //    result += " " + dct_content[i].ToString();

                }
            }
            textBox1.Text = txtsend.Text = result.Trim();

        }

        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 4];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 4), 16);
            return returnBytes;
        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        private void ParseIP2Byte(string ipaddress, int intstart, ref byte[] dct_content)
        {
            string[] ss = ipaddress.Split('.');
            for (int i = 0; i < ss.Length; i++)
            {
                dct_content[intstart + i] = Convert.ToByte(String.Format("{0:X}", ss[i]));
            }

            // (byte)ss[i];//
        }

        private void ParsePort2Byte(string port, int intstart, ref byte[] dct_content)
        {
            string str = "0x" + port;
            int x = Convert.ToInt32(str, 16);

            dct_content[intstart] = (byte)x;
            //for (int i = 0; i < ss.Length; i++)
            //{
            //    dct_content[intstart + i] = System.Text.Encoding.Default.GetBytes("ff");// byte.Parse("1");// Convert.ToByte(String.Format("{0:X}", Convert.ToInt32(ss[i]), 16));
            //}
        }




        private void txtdctname_Leave(object sender, EventArgs e)
        {
            if (!(txtdctname.Text.Trim().Length == 8))
            {
                MessageBox.Show("Field must input 8 char.");
                txtdctname.Focus();
            }
        }


        private void btnstop_Click(object sender, EventArgs e)
        {
            socketWatch.Close();
            threadWatch.Interrupt();
            threadWatch.Abort();
            dict.Clear();
            BusinessAccess.Socket = dict;
            dictThread.Clear();
            ShowMsg("服务器 Stop！");
        }
        private void btnstartserver_Click(object sender, EventArgs e)
        {
            // 创建负责监听的套接字，注意其中的参数；  
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 获得文本框中的IP对象；  
            IPAddress address = IPAddress.Parse(txtrealip.Text.Trim());
            // 创建包含ip和端口号的网络节点对象；  
            IPEndPoint endPoint = new IPEndPoint(address, int.Parse(txtrealport.Text.Trim()));
            try
            {
                // 将负责监听的套接字绑定到唯一的ip和端口上；  
                socketWatch.Bind(endPoint);
            }
            catch (SocketException se)
            {
                MessageBox.Show("异常：" + se.Message);
                return;
            }
            // 设置监听队列的长度；  
            socketWatch.Listen(10);
            // 创建负责监听的线程；  
            threadWatch = new Thread(WatchConnecting);
            threadWatch.IsBackground = true;
            threadWatch.Start();
            ShowMsg("服务器启动监听成功！");
            //}  


            //MyIP = System.Net.IPAddress.Parse(txtrealip.Text.Trim());
            //listener = new TcpListener(MyIP, int.Parse(txtrealport.Text.Trim()));
            //start = new ThreadStart(startListen);
            //listenThread = new Thread(start);   
        }




        private void AddListItem_Messages(ListBox objList, string value)
        {
            objList.Items.Insert(0, value);
            //objList.Items.Insert(0, objList.Items.Count);
            //if (objList.Items.Count >= 8)
            //    objList.Items.RemoveAt(7); //objList.Items.Clear(); 

        }
        private void RemoveListItem_Messages(ListBox objList, string value)
        {
            objList.Items.Remove(value);
            //objList.Items.Insert(0, objList.Items.Count);
            //if (objList.Items.Count >= 8)
            //    objList.Items.RemoveAt(7); //objList.Items.Clear(); 

        }
        /// <summary>  
        /// 监听客户端请求的方法；  
        /// </summary>  
        void WatchConnecting()
        {
            while (true)  // 持续不断的监听客户端的连接请求；  
            {
                // 开始监听客户端连接请求，Accept方法会阻断当前的线程；  
                Socket sokConnection = socketWatch.Accept(); // 一旦监听到一个客户端的请求，就返回一个与该客户端通信的 套接字；  
                // 想列表控件中添加客户端的IP信息；  

                this.Invoke(new AddListItemHandler(this.AddListItem_Messages), listBox1, sokConnection.RemoteEndPoint.ToString());
                //listBox1.Items.Add(sokConnection.RemoteEndPoint.ToString());
                // 将与客户端连接的 套接字 对象添加到集合中；  
                dict.Add(sokConnection.RemoteEndPoint.ToString(), sokConnection);
                BusinessAccess.Socket = dict;
                ShowMsg("客户端连接成功！");
                Thread thr = new Thread(RecMsg);
                thr.IsBackground = true;
                thr.Start(sokConnection);
                dictThread.Add(sokConnection.RemoteEndPoint.ToString(), thr);  //  将新建的线程 添加 到线程的集合中去。  
            }
        }

        void RecMsg(object sokConnectionparn)
        {
            Socket sokClient = sokConnectionparn as Socket;
            while (true)
            {
                // 定义一个2M的缓存区；  
                byte[] arrMsgRec = new byte[1024 * 1024 * 2];
                // 将接受到的数据存入到输入  arrMsgRec中；  
                int length = -1;
                try
                {
                    length = sokClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度；  
                    if (length == 0)
                        throw new Exception("Empty Send");
                    //string strMsg = System.Text.Encoding.UTF8.GetString(arrMsgRec, 0, arrMsgRec.Length);// 将接受到的字节数据转化成字符串；  
                    //ShowMsg(strMsg);
                }
                catch (SocketException se)
                {
                    ShowMsg("异常：" + se.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；  
                    dict.Remove(sokClient.RemoteEndPoint.ToString());
                    BusinessAccess.Socket = dict;
                    // 从通信线程集合中删除被中断连接的通信线程对象；  
                    dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从列表中移除被中断的连接IP  
                    this.Invoke(new RemoveListItemHandler(this.RemoveListItem_Messages), listBox1, sokClient.RemoteEndPoint.ToString());
                    //listBox1.Items.Remove(sokClient.RemoteEndPoint.ToString());
                    break;
                }
                catch (Exception e)
                {
                    ShowMsg("异常：" + e.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；  
                    dict.Remove(sokClient.RemoteEndPoint.ToString());
                    BusinessAccess.Socket = dict;
                    // 从通信线程集合中删除被中断连接的通信线程对象；  
                    dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从列表中移除被中断的连接IP  
                    this.Invoke(new RemoveListItemHandler(this.RemoveListItem_Messages), listBox1, sokClient.RemoteEndPoint.ToString());
                    //listBox1.Items.Remove(sokClient.RemoteEndPoint.ToString());
                    break;
                }

                try
                {
                    objAccess = new BusinessAccess();
                    if (objAccess.Send2DTC_Return(arrMsgRec, length, sokClient.RemoteEndPoint.ToString()) == 1)
                    {

                        ShowMsg(objAccess.DTC_Name + ":" + objAccess.DTC_Message);
                        this.Invoke(new RunText(this.RunTextBox), txtsend, listBox1);
                        //Send2DCT(txtsend.Text.Trim(), listBox1.Text.Trim());
                    }
                }
                catch { }
                
            }
        }

        private void AddTextBox_Messages(TextBox objList, string value)
        {
            objList.AppendText(value + "\r\n");
            //objList.Items.Insert(0, objList.Items.Count);
            //if (objList.Items.Count >= 8)
            //    objList.Items.RemoveAt(7); //objList.Items.Clear(); 

        }
        void ShowMsg(string str)
        {
            this.Invoke(new AddTextBoxHandler(this.AddTextBox_Messages), txtMsg, "(" + DateTime.Now.Millisecond + ") " + str);
            // txtMsg.AppendText(str + "\r\n");
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            Send2DCT(txtsend.Text.Trim(), listBox1.Text.Trim());
            //string strKey = "";
            //strKey = listBox1.Text.Trim();
            //byte[] byte_temp = byte_temp = Utility.Parse2Byte(txtsend.Text.Trim(), 0);
            //Int16 int16 =  BusinessAccess.Send2DTC(dict, strKey, byte_temp);
            //ShowMsg(strKey);
            //txtsendhex.Text = Utility.byte2string(byte_temp);
            //if (int16 == 0)
            //{
            //    ShowMsg("Error");

            //}


        }

        private void Send2DCT(string strcontent, string strip)
        {
            if (strcontent.Trim().Length > 0)
            {
                byte[] byte_temp = byte_temp = Utility.Parse2Byte(strcontent, 0);
                Int16 int16 = BusinessAccess.Send2DTC(ref dict, strip, byte_temp);
                // ShowMsg(strKey);
                txtsendhex.Text = Utility.byte2string(byte_temp);
                if (int16 == 0)
                {
                    ShowMsg("Error");

                }
            }
            //else
            //    ShowMsg("No Content");
        }

        private void RunTextBox(TextBox a, ListBox b)
        {
            Send2DCT(a.Text.Trim(), b.Text.Trim());
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            // string aaa = Convert.ToString(txtport.Text, 2); 
            string strMsg = txtsend.Text.Trim();// +"\r\n";
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg); // 将要发送的字符串转换成Utf-8字节数组；

            byte[] arrSendMsg = new byte[arrMsg.Length + 1];
            arrSendMsg[0] = 0; // 表示发送的是消息数据
            Buffer.BlockCopy(arrMsg, 0, arrSendMsg, 0, arrMsg.Length);

            foreach (Socket s in dict.Values)
            {
                s.Send(arrSendMsg);
            }
            ShowMsg(strMsg);
            // txtMsgSend.Clear();
            ShowMsg(" 群发完毕～～～");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // byte[] returnBytes = byteToHexStr(txtport.Text);

            //  label1.Text = returnBytes.ToString();

        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            ListItem listItem_T = cbxtype.SelectedItem as ListItem;
            ListItem listItem_C = cbxsuccess.SelectedItem as ListItem;
            ListItem listItem_V = cbxvoice.SelectedItem as ListItem;
            ListItem listItem_L = cbxlight.SelectedItem as ListItem;
            string strmsg = "";
            if (listItem_T.ID == "@")
            {

                strmsg = txtmmsg.Text;
               
            }

            string a = "$DCT," + listItem_T.ID + listItem_C.ID + strmsg + "," + listItem_V.ID + "," + listItem_L.ID + "," + txtsu2.Text.Trim() + "," + txtsu1.Text.Trim() + "," + txtcontent.Text + ",*";
            txtsend2dtc.Text = a;
            //byte[] temp = { 0x6B ,0xFF,0x20};
            byte[] temp = new byte[0];


            string aa = txthex.Text = objUti.Parse2ASCII(a);
            txtsend.Text = a + aa.Substring(2 > aa.Length ? 0 : aa.Length - 2);
            //
        }

        private void txtgeteway_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbxsuccess_SelectedIndexChanged(object sender, EventArgs e)
        {

            txt2DTC();
        }

        private void cbxvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt2DTC();
        }

        private void cbxlight_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt2DTC();
        }

        private void txtsu1_Paint(object sender, PaintEventArgs e)
        {
            txt2DTC();
        }

        private void txtsu2_Paint(object sender, PaintEventArgs e)
        {
            txt2DTC();
        }

        private void txt2DTC()
        {

        }

        private void btnonline_Click(object sender, EventArgs e)
        {
           
           
            string strKey = listBox1.Text.Trim();
            Int16 int16 = BusinessAccess.Send2DTC1(strKey, BusinessAccess.byte_online);
            //byte[] byte_temp = { 0x6b, 0xfe, 0x00, 0x95 };
            //Int16 int16 = BusinessAccess.Send2DTC(ref dict, strKey, byte_temp);
            ShowMsg(strKey + " Send OnLine");
            if (int16 == 0)
                ShowMsg("Error");
        }

        private void btndtcinfo_Click(object sender, EventArgs e)
        {
            string strKey = listBox1.Text.Trim();
            Int16 int16 = BusinessAccess.Send2DTC1(strKey, BusinessAccess.byte_dtc_info);
            //byte[] byte_temp = { 0x6b, 0xfd, 0x00, 0x96 };
            //Int16 int16 = BusinessAccess.Send2DTC(ref dict, strKey, byte_temp);
            ShowMsg(strKey + " Send Info");
            if (int16 == 0)
                ShowMsg("Error");
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            txtMsg.Text = "";
        }

        private void cbxtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem listItem_T = cbxtype.SelectedItem as ListItem;
            if (listItem_T.ID == "@")
            {
                txtmmsg.Enabled = true;
                txtmmsg.Text = "";
                txtmmsg.Focus();
            }
            else
                txtmmsg.Enabled = false;
        }






    }
}
