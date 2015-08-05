using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCTSetting
{

    class Models_Dtc_List
    {
        public int id { get; set; }
        public string categoryname { get; set; }
        public int category_id { get; set; }
        public Int16 line { get; set; }
        public string dct_name { get; set; }
        public string dct_ip { get; set; }
        public Int16 sortby { get; set; }
    }

    static class Form_par 
    {

        public static int Userid { get; set; }
        public static int Is_Socket { get; set; }
        public static int StationID { get; set; }
        public static string Session_name { get; set; }
    }

    class Models_Receiver_data
    {
        public int is_code { get; set; }
        public int is_OK { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string ParentBarcode { get; set; }
        public string ScanBarcode { get; set; }
    }


  
    class Models_Platform
    {

        public int userid { get; set; }
        public int stationid { get; set; }
        public string stationName { get; set; }
        public string sessionid_user { get; set; }
    }


    class Models_Process_name
    {

        public int id { get; set; }
        public string processname_desc { get; set; }
        public string processname { get; set; }
        public int run_process { get; set; }
        public int perid { get; set; }
        public int is_auto { get; set; }
        public int is_show { get; set; }
        public double maxDV_actualV_expected { get; set; }
        public double maxDV_actualW_expected { get; set; }
        public double max_actualAL { get; set; }
        public double min_actualAL { get; set; }
        public double max_actualAR { get; set; }
        public double min_actualAR { get; set; }
        public double min_pitch { get; set; }
        public double max_pitch { get; set; }
        public double min_roll { get; set; }
        public double max_roll { get; set; }
        public double min_distance { get; set; }
        public double max_distance { get; set; }
        public double min_angle { get; set; }
        public double max_angle { get; set; }
        public double min_RSSI { get; set; }
        public double max_RSSI { get; set; }
        public string checkbox_name { get; set; }
        public string directory { get; set; }
    }


    class Models_CarParement
    {
        public int carStatus { get; set; }
        public double sysVoltage { get; set; }
        public int sysErr { get; set; }
        public double expectedV { get; set; }
        public double expectedW { get; set; }
        public double actualV { get; set; }
        public double actualW { get; set; }
        public double actualAL { get; set; }
        public double actualAR { get; set; }
        public double pitch { get; set; }
        public double roll { get; set; }

        public int IEEE_HS { get; set; }
        public int RSSI_HS { get; set; }
        public int soundWidthL_HS { get; set; }
        public int soundWidthR_HS { get; set; }
        public double distance_HS { get; set; }
        public double angle_HS { get; set; }

        public int IEEE_OA { get; set; }
        public int RSSI_OA { get; set; }
        public double obsticleDistance { get; set; }
        public int obsticleAngle { get; set; }
        public int soundWidthL_OA { get; set; }
        public int soundWidthR_OA { get; set; }
        public int cnt_connected { get; set; }
        
    }

    class Models_CarParement_db
    {
        public string barcode { get; set; }

        public double rc_l_actualV { get; set; }
        public double rc_l_actualW { get; set; }
        public double rc_l_actualAL { get; set; }
        public double rc_l_actualAR { get; set; }
        public double rc_r_actualV { get; set; }
        public double rc_r_actualW { get; set; }
        public double rc_r_actualAL { get; set; }
        public double rc_r_actualAR { get; set; }

        public double rc_b_actualV { get; set; }
        public double rc_b_actualW { get; set; }
        public double rc_b_actualAL { get; set; }
        public double rc_b_actualAR { get; set; }
        public double rc_f_actualV { get; set; }
        public double rc_f_actualW { get; set; }
        public double rc_f_actualAL { get; set; }
        public double rc_f_actualAR { get; set; }

        public double fl_3f_actualV { get; set; }
        public double fl_3f_actualW { get; set; }
        public double fl_3f_actualAL { get; set; }
        public double fl_3f_actualAR { get; set; }
        public int fl_3f_ieee { get; set; }
        public int fl_3f_rssi { get; set; }
        public double fl_3f_distance { get; set; }
        public double fl_3f_angle { get; set; }
        public double fl_3l_actualV { get; set; }
        public double fl_3l_actualW { get; set; }
        public double fl_3l_actualAL { get; set; }
        public double fl_3l_actualAR { get; set; }
        public int fl_3l_ieee { get; set; }
        public int fl_3l_rssi { get; set; }
        public double fl_3l_distance { get; set; }
        public double fl_3l_angle { get; set; }
        public double fl_3r_actualV { get; set; }
        public double fl_3r_actualW { get; set; }
        public double fl_3r_actualAL { get; set; }
        public double fl_3r_actualAR { get; set; }
        public int fl_3r_ieee { get; set; }
        public int fl_3r_rssi { get; set; }
        public double fl_3r_distance { get; set; }
        public double fl_3r_angle { get; set; }

        public double fl_5f_actualV { get; set; }
        public double fl_5f_actualW { get; set; }
        public double fl_5f_actualAL { get; set; }
        public double fl_5f_actualAR { get; set; }
        public int fl_5f_ieee { get; set; }
        public int fl_5f_rssi { get; set; }
        public double fl_5f_distance { get; set; }
        public double fl_5f_angle { get; set; }
        public double fl_5l_actualV { get; set; }
        public double fl_5l_actualW { get; set; }
        public double fl_5l_actualAL { get; set; }
        public double fl_5l_actualAR { get; set; }
        public int fl_5l_ieee { get; set; }
        public int fl_5l_rssi { get; set; }
        public double fl_5l_distance { get; set; }
        public double fl_5l_angle { get; set; }
        public double fl_5r_actualV { get; set; }
        public double fl_5r_actualW { get; set; }
        public double fl_5r_actualAL { get; set; }
        public double fl_5r_actualAR { get; set; }
        public int fl_5r_ieee { get; set; }
        public int fl_5r_rssi { get; set; }
        public double fl_5r_distance { get; set; }
        public double fl_5r_angle { get; set; }
        public double pitch { get; set; }
        public double roll { get; set; }
        public Int16 active { get; set; }


    }

    class Models_Slave
    {


        public bool hold { get; set; }
        public bool release { get; set; }
        public bool up { get; set; }
        public bool down { get; set; }
        public bool short_circuit { get; set; }
        public bool open_circuit { get; set; }
        public bool reset { get; set; }
        public Int32 cnt_Connected { get; set; }


    }


 
    class Models_User_Info
    {
        public int id { get; set; }
        public string usrname { get; set; }
        public string fullname { get; set; }
        public string lastlogintime { get; set; }
        //public string avgoperatortime { get; set; }
        public string picture { get; set; }
    }




}

public class ListItem
{
    private string id = string.Empty;
    private string name = string.Empty;
    public ListItem(string sid, string sname)
    {
        id = sid;
        name = sname;
    }
    public override string ToString()
    {
        return this.name;
    }
    public string ID
    {
        get
        {
            return this.id;
        }
        set
        {
            this.id = value;
        }
    }
    public string Name
    {
        get
        {
            return this.name;
        }
        set
        {
            this.name = value;
        }
    }
}
class Enum_Utility
{
    enum DB_Table
    {
        //sfis_rawbarcode = "sfis_rawbarcode1",
        //sfis_rawbarcode_repair = "sfis_rawbarcode_repair",

        //sfis_productstepid2option = "sfis_productstepid2option"
    }

    /// <summary>
    /// 枚举win32 api
    /// </summary>
    public enum HardwareEnum
    {
        // 硬件
        Win32_Processor, // CPU 处理器
        Win32_PhysicalMemory, // 物理内存条
        Win32_Keyboard, // 键盘
        Win32_PointingDevice, // 点输入设备，包括鼠标。
        Win32_FloppyDrive, // 软盘驱动器
        Win32_DiskDrive, // 硬盘驱动器
        Win32_CDROMDrive, // 光盘驱动器
        Win32_BaseBoard, // 主板
        Win32_BIOS, // BIOS 芯片
        Win32_ParallelPort, // 并口
        Win32_SerialPort, // 串口
        Win32_SerialPortConfiguration, // 串口配置
        Win32_SoundDevice, // 多媒体设置，一般指声卡。
        Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
        Win32_USBController, // USB 控制器
        Win32_NetworkAdapter, // 网络适配器
        Win32_NetworkAdapterConfiguration, // 网络适配器设置
        Win32_Printer, // 打印机
        Win32_PrinterConfiguration, // 打印机设置
        Win32_PrintJob, // 打印机任务
        Win32_TCPIPPrinterPort, // 打印机端口
        Win32_POTSModem, // MODEM
        Win32_POTSModemToSerialPort, // MODEM 端口
        Win32_DesktopMonitor, // 显示器
        Win32_DisplayConfiguration, // 显卡
        Win32_DisplayControllerConfiguration, // 显卡设置
        Win32_VideoController, // 显卡细节。
        Win32_VideoSettings, // 显卡支持的显示模式。

        // 操作系统
        Win32_TimeZone, // 时区
        Win32_SystemDriver, // 驱动程序
        Win32_DiskPartition, // 磁盘分区
        Win32_LogicalDisk, // 逻辑磁盘
        Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
        Win32_LogicalMemoryConfiguration, // 逻辑内存配置
        Win32_PageFile, // 系统页文件信息
        Win32_PageFileSetting, // 页文件设置
        Win32_BootConfiguration, // 系统启动配置
        Win32_ComputerSystem, // 计算机信息简要
        Win32_OperatingSystem, // 操作系统信息
        Win32_StartupCommand, // 系统自动启动程序
        Win32_Service, // 系统安装的服务
        Win32_Group, // 系统管理组
        Win32_GroupUser, // 系统组帐号
        Win32_UserAccount, // 用户帐号
        Win32_Process, // 系统进程
        Win32_Thread, // 系统线程
        Win32_Share, // 共享
        Win32_NetworkClient, // 已安装的网络客户端
        Win32_NetworkProtocol, // 已安装的网络协议
        Win32_PnPEntity,//all device
    }
    

    public enum LogMsgType { Incoming, Outgoing, Normal, Warning, Error };

    public enum AlertVoiceType
    {
        Normal = 1,
        ReSet = 2,
        NotMatchCount = 5,
        NotClosed = 10,
        Error = 20
    }

}