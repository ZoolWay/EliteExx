﻿using System;
using System.IO;
using System.Net;
using System.Text;

namespace Zw.EliteExx.Core
{
    public static class HttpWebResponseExtensions
    {
        public static string GetContent(this HttpWebResponse response)
        {
            if (response.ContentLength == 0) return String.Empty;
            Encoding encoding = null;
            if (!String.IsNullOrWhiteSpace(response.ContentEncoding))
            {
                try
                {
                    encoding = Encoding.GetEncoding(response.ContentEncoding);
                }
                catch (ArgumentException)
                {
                    encoding = Encoding.UTF8;
                }
            }
            else
            {
                encoding = Encoding.UTF8;
            }

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream, encoding))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
