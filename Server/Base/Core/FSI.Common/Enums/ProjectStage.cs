using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Common.Enums
{
    public enum ProjectStage
    {
        XacLap,
        NghienCuu,
        MVP,
        KiemThu,
        TangTruong1,
        TangTruong2,
        TangTruong3,
        TangTruong4
    }

    public enum ProjectEventType
    {
        Init,
        NewMember,
        OutMember,
        NewInvestor,
        OutInvestor,
        PhaseSwich,
        GetInvesment,
        PostNotification
    }
}
