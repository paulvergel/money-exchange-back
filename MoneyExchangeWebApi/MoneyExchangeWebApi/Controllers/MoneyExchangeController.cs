using System;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MoneyExchangeWebApi.Models;
using System.Text;

namespace MoneyExchangeWebApi.Controllers
{
    public class MoneyExchangeController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MoneyExchangeConnection"].ConnectionString;

        /*
         * GET Method:
         * Gets the exchange rate between two currencies.
         * The two currencies (base and target) are obtained from querystring parameters ("basec" and "symbols").
         * The result is sent by a Response Message, including a JSON object with the updated exchange rate.
         * */
        public IHttpActionResult Get([FromUri] ExchangeQueryParams query)
        {
            Exchange exchange = null;

            try
            {
                /**
                 * The exchange rate will be obtained from the database by using the 
                 * stored procedure spuGetActualExchange.
                 * */
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = "spuGetActualExchange";
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@pBaseCurrency", query.basec);
                        command.Parameters.AddWithValue("@pTargetCurrency", query.symbols);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                exchange = new Exchange();
                                exchange.baseCurrency = reader["BaseCurrency"].ToString();
                                exchange.date = reader["DateExchange"].ToString();
                                exchange.targetCurrency = reader["TargetCurrency"].ToString();
                                exchange.ratio = Convert.ToDecimal(reader["Rate"]);
                            }
                        }

                        /*
                         * If an exchange rate is not found, a message of 
                         * not found will be returned 
                         * */
                        if (exchange == null)
                        {
                            return NotFound();
                        }
                    }
                }

                /*
                 * The GET method returns the JSON object of "Exchange" type 
                 * containing the current exchange rate
                 * */
                return Ok(exchange);
            }
            catch (SqlException sqlEx)
            {
                /*
                 * SQL errors are shown by a text string
                 * */
                StringBuilder errorMessages = new StringBuilder();
                for (int i = 0; i < sqlEx.Errors.Count; i++)
                {
                    errorMessages.Append("[Index #" + i + "] " +
                        "[Message: " + sqlEx.Errors[i].Message + "] " +
                        "[LineNumber: " + sqlEx.Errors[i].LineNumber + "] " +
                        "[Source: " + sqlEx.Errors[i].Source + "] " +
                        "[Procedure: " + sqlEx.Errors[i].Procedure + "]");
                }
                return BadRequest(errorMessages.ToString());
            }
            catch (Exception ex)
            {
                /*
                 * General error are shown by a Response Message Bad Request and a description message
                 * */
                return BadRequest(ex.Message);
            }
        }

    }
}