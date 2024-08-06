using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ExtraDeployChecks
{
    internal class LoggingObject : MonoBehaviour
    {
        private static ConcurrentQueue<string> Messages = new ConcurrentQueue<string>();

        public static void Log(string message)
        {
            Messages.Enqueue(message);
        }

        public void Update()
        {

            while(Messages.TryDequeue(out string message))
            {
                Debug.Log(message);
            }
        }
    }
}
