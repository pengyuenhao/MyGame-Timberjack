using UnityEngine;
using System.Collections;
using System;

public static class JavaInterface {
    public static bool java = false;
    public static bool check = false;
    public static AndroidJavaClass jc;
    public static AndroidJavaObject jo;
    public static bool Java()
    {
        //return false;
        if(java == false)
        {
            //Debug.Log("设备模式:"+SystemInfo.deviceModel + " 设备名称:" + SystemInfo.deviceName);
            if(SystemInfo.deviceModel.Equals("softwinner DVK3")==true)
            {
                java = true;
                //Debug.Log("街机设备");
            }
            else
            {
                return false;
            }
            if (SystemInfo.deviceName.Equals("FRJRL79RX6CSI0B"))
            {
                //Debug.Log("指定设备");
            }
        }
        else
        {
            if (check == true) return true;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            if (jc == null || jo == null)
            {
                return false;
            }
            check = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    public static int GetGiftSelectTime()
    {
        if (Java()) return jo.Call<int>("GetGiftSelectTime");
        return 0;
    }
    public static bool GetCoinOutFlag()
    {
        if (Java()) return jo.Call<bool>("GetCoinOutFlag");
        return false;
    }
    public static bool CoinOutCheck()
    {
        if (Java()) return jo.Call<bool>("CoinOutCheck");
        return false;
    }
    public static void CoinOut1Limit(int value)
    {
        if (Java())jo.Call("CoinOut1Limit",value);
    }
    public static void CoinOut2Limit(int value)
    {
        if (Java()) jo.Call("CoinOut2Limit", value);
    }
    public static int GetErrorStack()
    {
        if (Java()) return jo.Call<int>("GetErrorStack");
        return -1;
    }
    public static void PlayVideo()
    {
        if (Java()) jo.Call("PlayVideo");
    }
    public static int GetDefInt(string name)
    {
        try
        {
            if (Java()) return jo.Call<int>("GetDefInt", name);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        return 0;
    }
    public static void SetInt(string name,int value)
    {
        try
        {
            if (Java()) jo.Call("SetInt", name, value);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
    }
    public static void AddInt(string name, int value)
    {
        try
        {
            if (Java()) jo.Call("AddInt", name, value);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
    }
    public static int GetInt(string name)
    {
        try
        {
            if (Java()) return jo.Call<int>("GetInt", name);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        return 0;
    }
    public static string GetStr(string name)
    {
        try
        {
            if (Java()) return jo.Call<string>("GeStr", name);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        return "null";
    }
    public static string GetNameStr(string name)
    {
        if (Java()) return jo.Call<string>("GetNameStr", name);
        return "null";
    }
    public static int GetScorePlayer()
    {
        if (Java()) return jo.Call<int>("GetScorePlayer");
        return 0;
    }
    public static bool openMenu()
    {
        if (Java()) return jo.Call<bool>("StartMenuActivity", "第一个Activity");
        return false;
    }
    public static int GetActualGain()
    {
        if (Java()) return jo.Call<int>("GetActualGain");
        return 0;
    }
    public static int GetShuffleScore()
    {
        if (Java()) return jo.Call<int>("GetShuffleScore");
        return 0;
    }
    public static int GetPumpScore()
    {
        if (Java()) return jo.Call<int>("GetPumpScore");
        return 0;
    }
    public static int GetBonus()
    {
        if (Java()) return jo.Call<int>("GetBonus");
        return 0;
    }
    public static int GetSiteType()
    {
        if (Java()) return jo.Call<int>("GetSiteType");
        return 0;
    }
    public static void AddScorePlayer(int i)
    {
        if (Java()) jo.Call("AddScorePlayer",i);
        return;
    }
    public static void AddShuffleScore(int i)
    {
        if (Java()) jo.Call("AddShuffleScore", i);
        return;
    }
    public static void AddGameCounter(int i)
    {
        if (Java()) jo.Call<int>("AddGameCounter", i);
        return;
    }
    public static bool readCRC(){
        if (Java()) return jo.Call<bool>("readCRC");
        return false;
    }
    public static byte[] readKey()
    {
        if (readCRC() == true)
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return jo.Call<byte[]>("readBufferByte");
        }
        else
        {
            return null;
        }
    }
    public static bool getKey(int id)
    {
        
        if (readCRC() == true)
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return jo.Call<bool>("GetKey",id);
        }
        else
        {
            return false;
        }
    }
    public static byte[] getID()
    {
        if (readCRC() == true)
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return jo.Call<byte[]>("GetID");
        }
        else
        {
            return new byte[4];
        }
    }
    public static int getOutCoin1()
    {
        if (readCRC() == true)
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return jo.Call<int>("GetOutCoin1");
        }
        else
        {
            return -1;
        }
    }
    public static int getOutCoin2()
    {
        if (readCRC() == true)
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return jo.Call<int>("GetOutCoin2");
        }
        else
        {
            return -1;
        }
    }
    public static int getInCoin1()
    {
        if (readCRC() == true)
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return jo.Call<int>("GetInCoin1");
        }
        else
        {
            return -1;
        }
    }
    public static int getInCoin2()
    {
        if (readCRC() == true)
        {
            if (jc == null)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return jo.Call<int>("GetInCoin2");
        }
        else
        {
            return -1;
        }
    }
    public static bool sendCRC(byte[] buffer)
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("sendCRC", buffer);
    }
    public static bool setOutCoin1(int n)
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("setOutCoin1", n);
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x01;
        //buffer[1] = (byte)(n >> 8);
        //buffer[2] = (byte)n;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool setOutCoin2(int n)
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("setOutCoin2", n);
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x0E;
        //buffer[1] = (byte)(n >> 8);
        //buffer[2] = (byte)n;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool ClearOutCoin1()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("ClearOutCoin1");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x02;
        //buffer[1] = 0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool ClearInCoin1()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("ClearInCoin1");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x03;
        //buffer[1] = 0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool ClearOutCoin2()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("ClearOutCoin2");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x0A;
        //buffer[1] = 0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool ClearInCoin2()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("ClearInCoin2");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x0B;
        //buffer[1] = 0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool StopOutCoin1()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("StopOutCoin1");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x0C;
        //buffer[1] = 0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool StopOutCoin2()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("StopOutCoin2");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x0D;
        //buffer[1] = 0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool ReadBoardID()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("ReadBoardID");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0xBD;
        //buffer[1] = 0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }

    public static bool SetOut12V()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("SetOut12V");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x08;
        //buffer[1] = 0xF0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
    public static bool ResetOut12V()
    {
        if (jc == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        return jo.Call<bool>("ResetOut12V");
        //byte[] buffer = new byte[4];
        //buffer[0] = 0x09;
        //buffer[1] = 0xF0;
        //buffer[2] = 0;
        //buffer[3] = 0;
        //return sendCRC(buffer);
    }
}
