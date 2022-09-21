using Quartz.Spi;
using System;
using Ninject;
using Quartz.Simpl;

namespace Quartz.Ninject
{
    class NinjectJobFactory : SimpleJobFactory
    {
        readonly IKernel _kernel;

        public NinjectJobFactory(IKernel kernel) { _kernel = kernel; }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return (IJob)_kernel.Get(bundle.JobDetail.JobType); 
            }
            catch (Exception e)
            {
                throw new SchedulerException(
                    string.Format("Problem while instantiating job '{0}' from the NinjectJobFactory.", bundle.JobDetail.Key), e);
            }
        }
    }
}