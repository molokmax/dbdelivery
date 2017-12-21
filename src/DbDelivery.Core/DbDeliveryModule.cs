using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {
    public class DbDeliveryModule : NinjectModule {
        public override void Load() {
            Bind<ICommandInvoker>().To<CommandInvoker>();
            Bind<IPluginFactory>().To<PluginFactory>().InSingletonScope();
            Bind<IDbDeliveryEngine>().To<DbDeliveryEngine>().InSingletonScope();
        }
    }
}
