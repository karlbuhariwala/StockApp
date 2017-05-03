// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = Retry.cs

namespace StockApp.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Retry
    {
       public static async Task Do(Func<Task> action, TimeSpan retryInterval, int retryCount = 3)
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

                    await action();
                    break;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
        }

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
