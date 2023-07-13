using FSI.Common.ETO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace FSI.Application.EventHandle
{
    public class UpdateStartuperInfoHandle : IDistributedEventHandler<UpdateStartuperInfoEto>, ITransientDependency
    {
        public Task HandleEventAsync(UpdateStartuperInfoEto eventData)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Task.CompletedTask;
            }
        }
    }
}
