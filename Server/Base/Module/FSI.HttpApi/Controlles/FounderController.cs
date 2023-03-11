using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSI.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace FSI.Controlles
{
    public abstract class FounderController : AbpControllerBase
    {
        protected FounderController()
        {
            LocalizationResource = typeof(FSIResource);
        }
    }
}
