using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace EZSecCam
{
    public class ThreadHandler
    {
        private static ThreadHandler instance = null;
        private static readonly object padlock = new object();

        ThreadHandler()
        {
        }

        public static ThreadHandler Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ThreadHandler();
                    }
                    return instance;
                }
            }
        }

        public void ProcessWithThreadPoolMethod(WaitCallback callback)
        {
            ThreadPool.QueueUserWorkItem(callback);
        }
    }
}
