using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_ExtraDeployChecks_Bootstrap
{
    /// <summary>
    /// Relays the game's hook events to allow the stable and beta assemblies to respond to them.
    /// </summary>
    public class HookEvents
    {

        public static Action<IModContext> BeforeBootstrap;
        public static Action<IModContext> AfterConfigsLoaded;
    }
}
