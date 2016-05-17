using NativeSorterSummary.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NativeSorterSummary.Model
{
    class SorterModel
    {
        public async Task<List<SorterViewModel>> GetList()
        {
            List<SorterViewModel> sorters = new List<SorterViewModel>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                client.Timeout = new TimeSpan(0, 0, 10);

                string _url = "http://outboundview.kysdf.us.ups.com/svc/PosiSorter.asmx/GetOutboundSorters";

                var response = await client.GetAsync(_url);

                var responseBody = response.Content.ReadAsStreamAsync();

                XmlSerializer serializer = new XmlSerializer(typeof(List<SorterViewModel>));
                using (StreamReader reader = new StreamReader(responseBody.Result))
                {
                    sorters = (List<SorterViewModel>)serializer.Deserialize(reader);
                }
            }
            return sorters;
        }

        public List<SorterViewModel> GetData(string pSorter)
        {
            List<SorterViewModel> sorters = new List<SorterViewModel>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                client.Timeout = new TimeSpan(0, 0, 10);

                string _url = String.Format("http://outboundview.kysdf.us.ups.com/svc/PosiSorter.asmx/GetSorterSummary?pSorterName={0}", pSorter);

                var response = client.GetAsync(_url);

                var responseBody = response.Result.Content.ReadAsStringAsync();

                XmlSerializer serializer = new XmlSerializer(typeof(List<SorterViewModel>));
                using (StringReader reader = new StringReader(responseBody.Result))
                {
                    sorters = (List<SorterViewModel>)serializer.Deserialize(reader);
                }
            }
            return sorters;
        }

        public string GetString()
        {
            string result = String.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = new TimeSpan(0, 0, 10);

                string _url = "http://outboundview.kysdf.us.ups.com/svc/PosiSorter.asmx/GetSorterSummary?pSorterName=AS1";

                using (HttpRequestMessage _request = new HttpRequestMessage(HttpMethod.Get, _url))
                {
                    var response = client.GetAsync(_url);
                    result = response.Result.Content.ReadAsStringAsync().Result;
                }
            }

            return result;
        }
    }
}
