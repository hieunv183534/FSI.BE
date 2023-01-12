using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace FSI.WebAPI.Base
{
    public class CustomConventionalRouteBuilder : ConventionalRouteBuilder
    {
        public CustomConventionalRouteBuilder(IOptions<AbpConventionalControllerOptions> options) : base(options)
        {
        }

        //protected override string NormalizeControllerNameCase(string controllerName, ConventionalControllerSetting configuration)
        //{
        //    if (controllerName.StartsWith("NOM"))
        //    {
        //        return base.NormalizeControllerNameCase(controllerName.Substring(3, controllerName.Length - 3), configuration);
        //    }
        //    return base.NormalizeControllerNameCase(controllerName, configuration);
        //}

        //protected override string NormalizeActionNameCase(string actionName, ConventionalControllerSetting configuration)
        //{
        //    return base.NormalizeActionNameCase(actionName, configuration);
        //}
    }
}
