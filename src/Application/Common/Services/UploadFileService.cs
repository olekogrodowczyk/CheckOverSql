using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Services
{
    public class UploadFileService : IUploadFileService
    {
        public Task<string> UploadFile(IFormFile file, string path)
        {
            if(file != null && file.Length > 0)
            {
                string rootPath = Directory.GetCurrentDirectory();
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string fullPath = $"{rootPath}/{path}/{fileName}";
                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Task.FromResult(fileName);
            }
            throw new InvalidDataException("Problem with file has occurred");
        }
    }
}
