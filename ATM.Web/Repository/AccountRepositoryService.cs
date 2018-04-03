using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ATM.Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ATM.Web.Repository
{
    public class AccountRepositoryService : DisposableBase, IAccountRepositoryService
    {

        #region Constructors

        private HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private HttpRequestMessage _request;
        private readonly ILogger _logger;


        public AccountRepositoryService(IConfiguration config, ILogger<AccountRepositoryService> logger)
        {
            _httpClient = new HttpClient();
            _baseApiUrl = config.GetValue<string>("BankServiceUrl");
            _request = new HttpRequestMessage();
            _logger = logger;
        }

        #endregion

        #region Transactions 

        /// <summary>
        /// Gets the customer information
        /// </summary>
        /// <param name="userName">card Number</param>
        /// <param name="pwd">Pin Number</param>
        /// <returns>User Customer ID and Display Name</returns>
        public async Task<UserInfo> GetUserInformation(string userName, string pwd)
        {
            //Prepare Request

            _request.Method = HttpMethod.Post;
            _request.RequestUri = new Uri(_baseApiUrl + "/api/Customer");

            HttpContent content = new StringContent(JsonConvert.SerializeObject(new { UserId = userName, Pwd = pwd }), System.Text.Encoding.UTF8, "application/json");
            _request.Content = content;

            try
            {

                //Send request to API
                HttpResponseMessage response = await _httpClient.SendAsync(_request);

                //Process Response

                //Case - Success
                if (response.IsSuccessStatusCode)
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<UserInfo>(responseString);

                    return data;
                }
                else
                //case failure
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// get the Account balance based on the customer Id and account type
        /// </summary>
        /// <param name="custId">Customer ID</param>
        /// <param name="accType">Account Type</param>
        /// <returns>Balance as string</returns>
        public async Task<string> GetBalanceAsync(string custId, string accType)
        {
            //Prepare Request
            string requestUrl = _baseApiUrl + "/api/Customer/" + custId + "/" + accType;

            try
            {
                //Send request to API
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                //Process Response

                //Case - Success
                if (response.IsSuccessStatusCode)
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<string>(responseString);
                    return data;
                }
                else
                //case failure
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Executes withdrawl transaction
        /// </summary>
        /// <param name="custId">Customer Id</param>
        /// <param name="accountType">Account Type</param>
        /// <param name="withdrawlamount">Withdrawl Amount</param>
        /// <returns>balance, errors</returns>
        public async Task<WithdrawlResponse> WithdrawAmountAsync(string custId, string accountType, int withdrawlamount)
        {
            //Initialize response
            var responseData = new WithdrawlResponse();


            //Prepare Request
            _request.Method = HttpMethod.Put;
            _request.RequestUri = new Uri(string.Format("{0}/api/customer/{1}/{2}/{3}", _baseApiUrl, custId, accountType, withdrawlamount));

            try
            {
                //Send request to API
                HttpResponseMessage response = await _httpClient.SendAsync(_request);

                //Process response
                String responseString = await response.Content.ReadAsStringAsync();

                //Case - Success
                if (response.IsSuccessStatusCode)
                {
                    responseData.Success = true;
                    int balance;
                    if (int.TryParse(responseString, out balance))
                        responseData.Balance = balance;
                }
                //case failure
                else
                {
                    responseData.ErrorMessage = responseString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            return responseData;
        }

        #endregion

    }
}
