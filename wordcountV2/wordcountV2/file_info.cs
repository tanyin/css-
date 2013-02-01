using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace wordcountV2
{
    class file_info
    {
        private string file_name = "";
        private string file_path = "";
        
        public string File_name 
        { 
            set 
            { 
                this.file_name = value; 
            } 
            get 
            { 
                return this.file_name; 
            } 
        }
        public string File_path 
        { 
            set 
            { 
                this.file_path = value;
            } 
            get 
            { 
                return this.file_path; 
            } 
        }
        public string Pic_path 
        { 
            get 
            { 
                return this.file_path + "\\" + this.file_name + ".files"; 
            } 
        }
        public string File_fullname 
        { 
            get 
            { 
                return this.file_path + "\\" + this.file_name; 
            } 
        }

        public bool file_exist()
        {
            if (File.Exists(this.file_path + "\\" + this.file_name))
                return true;
            return false;
        }

        public bool PicDirExist()
        {
            if (Directory.Exists(this.Pic_path))
                return true;
            return false;
        }
    }
}
