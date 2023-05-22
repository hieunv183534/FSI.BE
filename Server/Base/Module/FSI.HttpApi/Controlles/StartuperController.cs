using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSI.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace FSI.Controlles
{
    public abstract class StartuperController : AbpControllerBase
    {
        protected StartuperController()
        {
            LocalizationResource = typeof(FSIResource);
        }
    }
}
