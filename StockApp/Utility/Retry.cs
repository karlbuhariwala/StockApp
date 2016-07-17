using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockApp.Utility
{
    public static class Retry
    {
       public static async Task<T> Do<T>(Func<Task<T>> action, TimeSpan retryInterval, int retryCount = 3)
        {
            var exceptions = new List<Exception>();
            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }

                    return await action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            //throw new AggregateException(exceptions);
            return default(T);
        }
    }
}
