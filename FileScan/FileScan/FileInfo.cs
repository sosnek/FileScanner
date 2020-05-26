using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileScan
{
    class FileInfo
    {
        private static readonly FileInfo _fileInfo = new FileInfo();

        public static FileInfo FileInfoInstance
        {
            get { return _fileInfo; }
        }

        /// <summary>
        /// MD5 hash of the resource.
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// SHA1 hash of the resource.
        /// </summary>
        public string SHA1 { get; set; }

        /// <summary>
        /// SHA256 hash of the resource.
        /// </summary>
        public string SHA256 { get; set; }

        /// <summary>
        /// File Size of the resource.
        /// </summary>
        public long File_Size { get; set; }

        /// <summary>
        /// File Name of the resource.
        /// </summary>
        public string File_Name { get; set; }

        /// <summary>
        /// File Path of the resource.
        /// </summary>
        public string File_Path { get; set; }
    }
}
