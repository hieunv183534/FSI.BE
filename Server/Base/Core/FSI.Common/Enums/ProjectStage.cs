using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Common.Enums
{
    public enum ProjectStage
    {
        PreSeed,
        Seed,
        Early,
        Grow,
        Expansion,
        Exit
    }

    public enum ProjectEventType
    {
        Init,
        PersonalChange,
        PhaseSwich,
        GetInvesment
    }
}
