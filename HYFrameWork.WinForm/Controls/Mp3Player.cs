using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using HYFrameWork.File;

namespace HYFrameWork.WinForm.Controls
{
    public class Mp3Player
    {

        static uint SND_ASYNC = 0x0001; // play asynchronously 
        static uint SND_FILENAME = 0x00020000; // name is file name
        [DllImport("winmm.dll")]
        static extern int mciSendString(string m_strCmd, string m_strReceive, int m_v1, int m_v2);

        [DllImport("Kernel32", CharSet = CharSet.Auto)]
        static extern Int32 GetShortPathName(String path, StringBuilder shortPath, Int32 shortPathLength);

        public static void Play(string MusicFile)
        {
            if (!FileHelper.Exists(MusicFile)) return;
            StringBuilder shortpath = new StringBuilder(80);
            int result = GetShortPathName(MusicFile, shortpath, shortpath.Capacity);
            MusicFile = shortpath.ToString();
            mciSendString(@"close all", null, 0, 0);
            mciSendString(@"open " + MusicFile + " alias song", null, 0, 0); //打开
            mciSendString("play song", null, 0, 0); //播放
        }
    }
}
