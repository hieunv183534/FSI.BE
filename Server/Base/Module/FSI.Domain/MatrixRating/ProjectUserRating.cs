using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace FSI.Domain.MatrixRating
{
    public class ProjectUserRating : Entity<Guid>
    {
        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        public float Rating { get; set; }
    }
}
