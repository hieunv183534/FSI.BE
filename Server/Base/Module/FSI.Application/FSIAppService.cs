using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSI.Localization;
using Volo.Abp.Application.Services;

namespace FSI
{
    public abstract class FSIAppService : ApplicationService
    {
        protected FSIAppService()
        {
            LocalizationResource = typeof(FSIResource);
        }
    }
}
