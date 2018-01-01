using DbDelivery.Core;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDeliveryCmd {
    class Program {
        static int Main(string[] args) {
            try {
                if (args == null || args.Length < 2) {
                    // Print usage
                    Console.WriteLine("Usage: dbDelivery ApplicationName EnvironmentName");
                } else {
                    // get parameters application and environment
                    string applicationName = args[0];
                    string environmentName = args[1];

                    // init dependency injection
                    IoC.Kernel.Load(new INinjectModule[] { new DbDeliveryModule() });
                    IDbDeliveryEngine engine = IoC.Kernel.Get<IDbDeliveryEngine>();

                    // execute migration
                    engine.Init();
                    engine.Migrate(applicationName, environmentName);
                }
                return 0;
            } catch (ApplicationException e) {
                Console.WriteLine(e.Message);
                return 1;
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return 1;
            }
        }
    }
}
