using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDelivery.Core {
    public static class IoC {
        public static IKernel Kernel { get; private set; }

        static IoC() {
            Kernel = new StandardKernel();
        }
    }
}
