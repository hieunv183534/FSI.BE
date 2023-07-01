using FSI.Application.Contracts.Auth.DTO;
using FSI.Domain.File;
using FSI.Domain.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace FSI.HttpApi.Controlles
{
    [Authorize]
    [Route("api/fsi/project")]
    [ApiController]
    public class DocfileController : AbpController
    {
        private readonly IRepository<ProjectUser, Guid> _projectUserRepository;
        private readonly IRepository<ProjectFile, Guid> _projectFileRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;

        public DocfileController(IRepository<ProjectFile, Guid> projectFileRepository, IRepository<ProjectUser, Guid> projectUserRepository, IFileInfomationRepository fileInfomationRepository)
        {
            _projectFileRepository = projectFileRepository;
            _projectUserRepository = projectUserRepository;
            _fileInfomationRepository = fileInfomationRepository;
        }

        [HttpGet("get-file/{projectFileId}")]
        public async Task<IActionResult> Login([FromRoute] Guid projectFileId)
        {
            var currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var projectFile = await _projectFileRepository.GetAsync(projectFileId, includeDetails: true);
            var files = await _fileInfomationRepository.GetListAsync();

            if (projectFile.VisibleForAll)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), @"Docs", projectFile.File.Url);
                var imageFileStream = System.IO.File.OpenRead(path);
                return File(imageFileStream, projectFile.File.ContentType);
            }
            else if (projectFile.VisibleForInvestor)
            {
                var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectFile.ProjectId));
                if (myProjectUser == null)
                    throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải nhà đầu tư/thành viên của dự án này!");

                var path = Path.Combine(Directory.GetCurrentDirectory(), @"Docs", projectFile.File.Url);
                var imageFileStream = System.IO.File.OpenRead(path);
                return File(imageFileStream, projectFile.File.ContentType);
            }
            else
            {
                var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectFile.ProjectId));
                if (myProjectUser == null || myProjectUser.Role == Common.Enums.RoleInProject.Investor)
                    throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

                var path = Path.Combine(Directory.GetCurrentDirectory(), @"Docs", projectFile.File.Url);
                var imageFileStream = System.IO.File.OpenRead(path);
                return File(imageFileStream, projectFile.File.ContentType);
            }
        }
    }
}
