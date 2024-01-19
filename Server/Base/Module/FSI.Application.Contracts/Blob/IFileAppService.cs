using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Blob
{
    public interface IFileAppService
    {
        Task<FileResult> GetImage(string filePath);
    }
}
