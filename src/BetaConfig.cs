using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_ExtraDeployChecks_Bootstrap
{
    public class BetaConfig
    {
        public string BetaVersion { get; set; }
        public bool DisableStable { get; set; }
        public bool DisableBeta { get; set; }
    }
}
