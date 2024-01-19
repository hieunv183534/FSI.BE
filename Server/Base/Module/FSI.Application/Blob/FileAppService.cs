using FSI.Application.Contracts.Blob;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace FSI.Application.Blob
{
    public class FileAppService : ApplicationService, IFileAppService
    {

        private readonly IBlobContainer _blobContainer;

        public FileAppService(IBlobContainer blobContainer)
        {
            _blobContainer = blobContainer;
        }

        [AllowAnonymous]
        [Route("image/{filePath}")]
        public async Task<FileResult> GetImage([FromRoute]string filePath)
        {
            var blob = await _blobContainer.GetAllBytesOrNullAsync(filePath);
            return new FileContentResult(blob, "image/png");
        }
    }
}
