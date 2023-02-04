using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPTNET.NOM.System.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace VNPTNET.NOM.System.Controlles
{
    public abstract class NOMSystemController : AbpControllerBase
    {
        protected NOMSystemController()
        {
            LocalizationResource = typeof(FSIResource);
        }
    }
}
