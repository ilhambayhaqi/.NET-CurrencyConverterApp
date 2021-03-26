using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Currency_Converter
{
    class CBCurrencyMV
    {
        private string jsonfile = "CurrencyNames.json";
        private string jsonapi = "CurrencyExchange.json";
        private string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CurrencyConverterApp\";
        private string apiBase = "http://data.fixer.io/";
        private string apiKey = "7dd299e649864228fb2f5d07da90e14c";
        private JObject currencyName, currencyExchange;

        public Dictionary<string, KeyValuePair<double, string>> CurrencyCollection { get; set; }
        public CBCurrencyMV()
        {
            CurrencyCollection = new();            
            deserialiseJSON(ref currencyName, _filePath + jsonfile);
            requestAPIData();
            //deserialiseJSON(ref currencyExchange, _filePath + jsonapi);

            foreach(JProperty rates in ((JObject) currencyExchange["rates"]).Properties() ){
                string ratesName = (rates.Name).ToString();
                double ratesExchange = double.Parse(rates.Value.ToString());
                string ratesAlias = currencyName[ratesName]["currencyName"].ToString();
                CurrencyCollection.Add(ratesName, new(ratesExchange, ratesAlias));
            }
        }

        private void deserialiseJSON(ref JObject output, string fileJSON)
        {
            using (StreamReader file = File.OpenText(fileJSON))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                output = (JObject)JToken.ReadFrom(reader);
            }
        }

        private void requestAPIData()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiBase);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("api/latest?access_key=" + apiKey).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                using (StreamWriter sw = new StreamWriter(_filePath + jsonapi))
                {
                    sw.WriteLine(result);
                }
                deserialiseJSON(ref currencyExchange, _filePath + jsonapi);
            }
        }

        public JObject getCurrencyName()
        {
            return currencyName;
        }

        public JObject getCurrencyExchange()
        {
            return currencyExchange;
        }
    }
}
