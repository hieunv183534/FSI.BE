using FSI.Domain.Account;
using FSI.Domain.File;
using FSI.Domain.Project;
using FSI.Domain.User;
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
    [Route("api/auth")]
    [ApiController]
    public class SecretFileController : AbpController
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRepository<ProjectUser, Guid> _projectUserRepository;
        private readonly IRepository<ProjectFile, Guid> _projectFileRepository;
        private readonly IUserRootRepository _userRepository;
        private readonly IFileInfomationRepository _fileInfomationRepository;

        public SecretFileController(IProjectRepository projectRepository, IRepository<ProjectFile, Guid> projectFileRepository, IFileInfomationRepository fileInfomationRepository, IRepository<ProjectUser, Guid> projectUserRepository)
        {
            _projectRepository = projectRepository;
            _projectFileRepository = projectFileRepository;
            _fileInfomationRepository = fileInfomationRepository;
            _projectUserRepository = projectUserRepository;
        }

        [HttpGet("get-file/{projectFileId}")]
        public async Task<IActionResult> GetFile([FromRoute] Guid projectFileId)
        {
            var currentUserId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var projectFile = await _projectFileRepository.GetAsync(projectFileId,includeDetails: true);

            var myProjectUser = await _projectUserRepository.FindAsync(x => x.UserId.Equals(currentUserId) && x.ProjectId.Equals(projectFile.ProjectId));
            if (myProjectUser == null)
                throw new UserFriendlyException(message: "Dự án không tồn tại hoặc bạn không phải thành viên của dự án này!");

            var path = Path.Combine(Directory.GetCurrentDirectory(), @"Docs", projectFile.File.Url);
            var imageFileStream = System.IO.File.OpenRead(path);
            return File(imageFileStream, projectFile.File.ContentType);
        }
    }
}
