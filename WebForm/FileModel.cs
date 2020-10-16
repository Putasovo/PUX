using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FileChecker
{
    public class FileModel
    {
        public string FileID { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public DateTime WriteTime { get; set; }
    }
}