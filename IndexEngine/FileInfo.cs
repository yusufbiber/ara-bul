using System;
using System.Collections.Generic;
using System.Text;

namespace IndexEngine
{
    [Serializable]
    public class FileInfo
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string FileExtension { get; set; }
        public long size { get; set; }
        public string Type { get; set; }
        public string Directory { get; set; }
    }
    [Serializable]
    public class IndexHost
    {
        public string Key { get; set; }
        public List<string> files { get; set; }
    }
}
