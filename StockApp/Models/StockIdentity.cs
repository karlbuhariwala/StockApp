// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = StockIdentityContainer.cs

namespace StockApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    public class StockIdentity
    {
        [DataMember]
        public string Symbol { get; set; }

        [DataMember]
        public string Exchange { get; set; }

        public double PercentageRange { get; set; }

        public DateTime Timestamp { get; set; }
    }

    internal class StockIdentityComparer : IEqualityComparer<StockIdentity>
    {
        bool IEqualityComparer<StockIdentity>.Equals(StockIdentity x, StockIdentity y)
        {
            return x.Exchange + x.Symbol == y.Exchange + y.Symbol;
        }

        int IEqualityComparer<StockIdentity>.GetHashCode(StockIdentity obj)
        {
            return (obj.Exchange + obj.Symbol).GetHashCode();
        }
    }
}
