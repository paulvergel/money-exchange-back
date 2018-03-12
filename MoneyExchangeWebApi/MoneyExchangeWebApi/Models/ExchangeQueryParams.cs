using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyExchangeWebApi.Models
{
    /*
     * ExchangeQueryParams Class:
     * Support class for use querystring parameters in the GET method
     * */
    public class ExchangeQueryParams
    {
        public string basec { get; set; }

        public string symbols { get; set; }
    }
}