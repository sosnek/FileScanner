using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FileScan
{
    class Utility
    {
        public static string CalculateMD5(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    var sb = new StringBuilder();
                    byte[] md5hash = md5.ComputeHash(stream);

                    foreach (var bite in md5hash)
                    {
                        sb.Append(bite.ToString("X2"));
                    }
                    FileInfo.FileInfoInstance.MD5 = sb.ToString();
                    return sb.ToString();
                }
            }
        }


        public static string CalculateSHA1(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                var sb = new StringBuilder();
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] sha1hash = sha1.ComputeHash(stream);
                    foreach (var bite in sha1hash)
                    {
                        sb.Append(bite.ToString("X2"));
                    }
                }
                FileInfo.FileInfoInstance.SHA1 = sb.ToString();
                return sb.ToString();
            }
        }


        public static string CalculateSHA256(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                var sb = new StringBuilder();
                using (SHA256Managed sha256 = new SHA256Managed())
                {
                    byte[] sha256hash = sha256.ComputeHash(stream);
                    foreach (var bite in sha256hash)
                    {
                        sb.Append(bite.ToString("X2"));
                    }
                }
                FileInfo.FileInfoInstance.SHA256 = sb.ToString();
                return sb.ToString();
            }
        }


        public static long CalculateFileSize(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                FileInfo.FileInfoInstance.File_Size = stream.Length;
                return stream.Length;
            }
        }
    }
}
