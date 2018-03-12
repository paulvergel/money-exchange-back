using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyExchangeWebApi.Models
{
    /*
     * Echange Class:
     * Define an object with information on the exchange rate between two currencies 
     * */
    public class Exchange
    {
        public string baseCurrency { get; set; }

        public string date { get; set; }

        public string targetCurrency { get; set; }

        public decimal ratio { get; set; }
    }
}