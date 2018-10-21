using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IndexEngine
{
    public class Engine
    { 
        string[] extensions; List<string> listExt;
        List<FileInfo> fileInfos = new List<FileInfo>();
        List<IndexHost> indexHosts = new List<IndexHost>();
        List<IndexHost> _fileInfos;
        public List<IndexHost> res;
        public void Start(string dir)
        {
            extensions = System.IO.File.ReadAllLines("fileExtensions.txt");
            listExt = extensions.ToList<string>();
            Folder(dir);

            WriteToBinaryFile<List<IndexHost>>("Index.dat", indexHosts, false);
        }

        public void Search(string txt)
        {
            if (_fileInfos==null)
                _fileInfos = ReadFromBinaryFile<List<IndexHost>>("Index.dat");
            if (txt!=null)
             res = _fileInfos.Where(x => x.Key.Contains(txt)).ToList();
             
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        void Folder(string dir)
        {
            string[] dirs = System.IO.Directory.GetDirectories(dir);            
            string[] files = System.IO.Directory.GetFiles(dir);
            foreach (string file in files)
            {
                FileOperate(file);
            }
            foreach (string item in dirs)
            {
                if (item.Contains("node_modules")) return;
                if (item.Substring(0,1)!="$")
                    Folder(item);
            }
        }

        private void FileOperate(string file)
        {
            string extension = System.IO.Path.GetExtension(file);
            if (!listExt.Exists(x => x == extension)) return;

            string content=System.IO.File.ReadAllText(file);

            index(content, file);

            //System.IO.FileInfo _fileInfo = new System.IO.FileInfo(file);
            //FileInfo fileInfo = new FileInfo();
            //fileInfo.Content = System.IO.File.ReadAllText(file);
            //fileInfo.FileName = _fileInfo.Name;
            //fileInfo.Directory = _fileInfo.DirectoryName;
            //fileInfos.Add(fileInfo);

        }

        void FileOperate2(string file)
        {
            //string extension = System.IO.Path.GetExtension(file);
            //bool exists= extensions.Exists(x => x == extension);
            //if (!exists)
            //    extensions.Add(extension);
        }
 

        void Add(string word, string file)
        {
            IndexHost indexHost=indexHosts.Where(x => x.Key == word).FirstOrDefault();
            if (indexHost == null)
            {
                indexHost = new IndexHost();
                indexHost.Key = word;
                indexHosts.Add(indexHost);
            }
                
            if (indexHost.files == null)
                indexHost.files = new List<string>();
            indexHost.files.Add(file);
            
        }

        void index(string content, string file)
        {
            string[] separators = new string[] {"[","]",")","(","}","{","/","\\","=",";",":" ,",", ".", "!", "\'", " ","<",">", "\'s",Environment.NewLine };

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w");

            string[] ars = content.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            ars=ars.Distinct().ToArray();

            for (int i = 0; i < ars.Length; i++)
            {
                if (regex.Match(ars[i].Substring(0,1)).Success)
                    Add(ars[i], file);
            }
                
        }

    }

    
}
