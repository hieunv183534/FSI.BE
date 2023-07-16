using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace FSI.Domain.MatrixRating
{
    public class UserProjectRating : Entity<Guid>
    {
        public Guid UserId { get; set; }

        public Guid ProjectId { get; set; }

        public float Rating { get; set; }
    }
}
