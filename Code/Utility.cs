using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
namespace DCTSetting
{
    class Utility
    {

        /// <summary>
        /// 十六进制换算为十进制
        /// </summary>
        /// <param name="strColorValue"></param>
        /// <returns></returns>
        public int GetHexadecimalValue(String strColorValue)
        {
            char[] nums = strColorValue.ToCharArray();
            int total = 0;
            try
            {
                for (int i = 0; i < nums.Length; i++)
                {
                    String strNum = nums[i].ToString().ToUpper();
                    switch (strNum)
                    {
                        case "A":
                            strNum = "10";
                            break;
                        case "B":
                            strNum = "11";
                            break;
                        case "C":
                            strNum = "12";
                            break;
                        case "D":
                            strNum = "13";
                            break;
                        case "E":
                            strNum = "14";
                            break;
                        case "F":
                            strNum = "15";
                            break;
                        default:
                            break;
                    }
                    double power = Math.Pow(16, Convert.ToDouble(nums.Length - i - 1));
                    total += Convert.ToInt32(strNum) * Convert.ToInt32(power);
                }

            }
            catch (System.Exception ex)
            {
                String strErorr = ex.ToString();
                return 0;
            }


            return total;
        }



        public string Parse2ASCII(string strval)
        {
            string result = "";

            byte[] array_list = Parse2Byte(strval, 0);
          
            for (int i = 0; i < array_list.Length; i++)            
                result += " " + array_list[i].ToString("X2");

            
            return result.Trim();
        }

        public static string byte2string(byte[] array_list)
        {
            string result = "";
            for (int i = 0; i < array_list.Length; i++)
                result += " " + array_list[i].ToString("X2");


            return result.Trim();
        }

        public static byte[] Parse2Byte(string strval, int intstart = 0)
        {
            byte[] array = System.Text.Encoding.ASCII.GetBytes(strval.Trim());
            byte[] array_list = new byte[array.Length + 1];
            //if (byte_item.Length > 0)
            //    byte_item.CopyTo(array_list, 0);
            array.CopyTo(array_list, intstart);
            Byte xor = 0;
            for (Byte i = 1; i < array.Length - 1; i++)
                xor ^= array[i];

            array_list[array.Length] = xor;
            return array_list;
        }

        public static byte[] GetBytes(string hexString, out int discarded)
        {

            discarded = 0;

            string newString = "";

            char c;// remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {

                c = hexString[i];
                if (Uri.IsHexDigit(c))

                    newString += c;

                else

                    discarded++;

            }// if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded++;

                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {

                hex = new String(new Char[] { newString[j], newString[j + 1] });

                bytes[i] = byte.Parse(hex);//HexToByte(hex);               
                j = j + 2;

            }

            return bytes;

        }
        //public static string[] MulGetHardwareInfo(Enum_Utility.HardwareEnum hardType, string propKey)
        //{

        //    List<string> strs = new List<string>();
        //    try
        //    {
        //        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
        //        {
        //            var hardInfos = searcher.Get();
        //            foreach (var hardInfo in hardInfos)
        //            {
        //                //if (hardInfo.Properties[propKey].Value.ToString().Contains("COM"))
        //                // {
        //                strs.Add(hardInfo.Properties[propKey].Value.ToString());
        //                // }

        //            }
        //            searcher.Dispose();
        //        }
        //        return strs.ToArray();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    finally
        //    { strs = null; }
        //}
    }
}
