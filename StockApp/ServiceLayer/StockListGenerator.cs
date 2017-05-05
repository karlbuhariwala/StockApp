// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockListGenerator.cs

namespace StockApp.ServiceLayer
{
    using Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using Utility;

    public class StockListGenerator
    {
        public StockListGenerator()
        {
            string jsonText = File.ReadAllText(Constants.StockIdentityFileName);
            this.Stocks = JsonConvert.DeserializeObject<List<StockIdentity>>(jsonText);
        }

        public List<StockIdentity> Stocks { get; private set; }
    }
}
