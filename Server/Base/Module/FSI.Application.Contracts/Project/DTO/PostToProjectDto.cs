using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.DTO
{
    public class PostToProjectDto 
    {
        public Guid ProjectId { get; set; }

        public string Content { get; set; }

        public string? Location { get; set; }

        public string? FileIds { get; set; }

        public string? Links { get; set; }
    }
}
