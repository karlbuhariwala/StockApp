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
    public class StockIdentityContainer
    {
        [DataMember]
        public string Symbol { get; set; }

        [DataMember]
        public string Exchange { get; set; }
    }

    internal class StockIdentityComparer : IEqualityComparer<StockIdentityContainer>
    {
        bool IEqualityComparer<StockIdentityContainer>.Equals(StockIdentityContainer x, StockIdentityContainer y)
        {
            return x.Exchange + x.Symbol == y.Exchange + y.Symbol;
        }

        int IEqualityComparer<StockIdentityContainer>.GetHashCode(StockIdentityContainer obj)
        {
            return (obj.Exchange + obj.Symbol).GetHashCode();
        }
    }
}
