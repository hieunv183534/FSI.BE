using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Common.Enums
{
    public enum RoleInProject
    {
        Investor,
        Member,
        CoFounder,
        Founder
    }

    public enum RelationWithProject
    {
        IsMemberOfProject,
        NotMemberOfProject,
        ProjectRequestTo,
        RequestToProject,
        Admin
    }
}
