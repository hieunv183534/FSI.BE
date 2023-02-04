using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPTNET.NOM.System.Localization;
using Volo.Abp.Application.Services;

namespace VNPTNET.NOM.Struct
{
    public abstract class FSIAppService : ApplicationService
    {
        protected FSIAppService()
        {
            LocalizationResource = typeof(FSIResource);
        }
    }
}
