// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockSnapshotCollector.cs

namespace StockApp.ServiceLayer
{
    using System;
    using System.Threading.Tasks;

    public class StockSnapshotCollector
    {
        private readonly StockShapshotCollector stockShapshotCollector;

        public StockSnapshotCollector(StockShapshotCollector stockShapshotCollector)
        {
            this.stockShapshotCollector = stockShapshotCollector;
        }

        public async Task DoWork(bool force = false)
        {
            await this.stockShapshotCollector.CreateShapshot(@".\Result\StockSnapshot" + DateTime.Now.ToString("yyyyMMdd") + ".csv");
        }
    }
}
