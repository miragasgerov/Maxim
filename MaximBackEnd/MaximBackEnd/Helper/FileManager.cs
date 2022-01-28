using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MaximBackEnd.Helper
{
    public static class FileManager
    {
        public static string Save(string rootPath, string folder, IFormFile file)
        {
            string FullName = file.FileName;
            FullName = FullName.Length <= 64 ? FullName : (FullName.Substring(FullName.Length - 64, 64));
            FullName = Guid.NewGuid().ToString() + FullName;

            string path = Path.Combine(rootPath, folder, FullName);

            using(FileStream stream = new FileStream(path,FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return FullName;
        }


        public static bool  Delete(string rootPath,string folder,string Image)
        {
            string path = Path.Combine(rootPath, folder, Image);

            if(File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
