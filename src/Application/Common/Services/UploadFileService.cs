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
        public async Task<string> UploadFile(IFormFile file, string folderName)
        {
            if(file != null && file.Length > 0)
            {
                string rootPath = Directory.GetCurrentDirectory();
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string fullPath = $"{rootPath}/wwwroot/images/{folderName}/{fileName}";
                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return await Task.FromResult(fileName);
            }
            throw new InvalidDataException("Problem with file has occurred");
        }
    }
}
