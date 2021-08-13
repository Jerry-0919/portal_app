// using NAudio.Wave;
using System;
using System.IO;
using System.Linq;

namespace diga.web.Helpers
{
    public static class AudioHelper
    {
        public static string[] Formats = new string[] { ".mp3", "wav", "midi", "mva" };

        public static bool IsAudio(string fileName)
        {
            return Formats.Contains(Path.GetExtension(fileName));
        }

        public static TimeSpan GetAudioDuration(string path)
        {
            return new TimeSpan();
            // return new MediaFoundationReader(path).TotalTime;
        }
    }
}
