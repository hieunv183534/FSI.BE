using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Application.Contracts.Project.DTO
{
    public class UpdateCanvasModelDto
    {
        public Guid ProjectId { get; set; }

        public string Model { get; set; }
    }
}
