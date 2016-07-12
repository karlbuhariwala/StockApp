using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Provider.GoogleStock
{
    [DataContract]
    public class GoogleStock
    {
        [DataMember(Name = "t")]
        public string Symbol { get; set; }

        [DataMember(Name = "l_fix")]
        public string CurrentPrice { get; set; }
    }
}
