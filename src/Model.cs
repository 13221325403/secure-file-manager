using System;
using System.Collections.Generic;

namespace SecureFileManager.Model
{

    class File
    {
        public int pk { get; set; }
        public string name { get; set; }
        public string ext { get; set; }
        public long length { get; set; }
        public byte[] file { get; set; }
        public string description { get; set; }
        public DateTime? creation_date { get; set; }
        public DateTime? last_edit_date { get; set; }
        public int? fk_directory { get; set; }
    }
    
    class Directory
    {
        public int pk { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? fk_parent_directory { get; set; }
        public string color { get; set; }
        public int? files_count { get; set; }
        public List<Directory> childs; //[TODO]

        public Directory()
        {
            childs = new List<Directory>();
        }
    }

}
