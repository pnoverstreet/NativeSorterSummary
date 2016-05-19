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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NativeSorterSummary.Model
{
    class SorterModel
    {
        public List<SorterViewModel> GetList()
        {
            //string _proxyUrl = "http://proxy.air.ups.com:8080";
            //string _url = "http://outboundview.kysdf.us.ups.com/svc/PosiSorter.asmx/GetOutboundSorters";
            string _url = "http://10.0.2.2:59857/api/Car";
            //string _url = "http://www.google.com/";

            List<SorterViewModel> sorters = new List<SorterViewModel>();
            
            //NetworkCredential cred = new NetworkCredential(ProxyCreds._PROXYUSER_, ProxyCreds._PROXYPWD_);
            //WebProxy prox = new WebProxy(new Uri(_proxyUrl)) { Credentials = cred };
            //HttpClientHandler handler = new HttpClientHandler()
            //{
            //    Proxy = prox,
            //    PreAuthenticate = true,
            //    UseProxy = true,
            //    UseDefaultCredentials = false
            //};

            using (HttpClient client = new HttpClient()) // handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                client.Timeout = new TimeSpan(0, 0, 10);

                var response = client.GetAsync(_url, HttpCompletionOption.ResponseContentRead).Result;
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        using (XmlReader oXR = XmlReader.Create(response.Content.ReadAsStreamAsync().Result))
                        {
                            oXR.MoveToContent();
                            while (oXR.Read())
                            {
                                if (oXR.NodeType == XmlNodeType.Element)
                                {
                                    if (String.Compare("SORTER", oXR.Name) == 0)
                                    {
                                        XElement oElem = XNode.ReadFrom(oXR) as XElement;
                                        if (oElem != null)
                                        {
                                            if (oElem.HasAttributes == true)
                                            {
                                                XAttribute xSorter = oElem.Attribute("SORTER_NAME");
                                                if (!String.IsNullOrEmpty(xSorter.Value))
                                                {
                                                    sorters.Add(new SorterViewModel() { SORTER_NAME = xSorter.Value });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // Something broke -- LogInterface.WriteLog(String.Format("{0} ERROR: Failed to retrieve SCADA information!\r\n{1}", sLocalTitle, e.ToString()));
                    }
                }
            }
            return sorters;
        }

        public List<SorterSummaryViewModel> GetSummary(string pSorter)
        {
            List<SorterSummaryViewModel> summary = new List<SorterSummaryViewModel>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                client.Timeout = new TimeSpan(0, 0, 10);

                string _url = String.Format("http://outboundview.kysdf.us.ups.com/svc/Posisorter.asmx/GetSorterSummary?pSorterName={0}", pSorter);

                var response = client.GetAsync(_url).Result;

                try
                {
                    using (XmlReader oXR = XmlReader.Create(response.Content.ReadAsStreamAsync().Result))
                    {
                        oXR.MoveToContent();
                        while (oXR.Read())
                        {
                            if (oXR.NodeType == XmlNodeType.Element)
                            {
                                if (String.Compare("SORTER", oXR.Name) == 0)
                                {
                                    XElement oElem = XNode.ReadFrom(oXR) as XElement;
                                    if (oElem != null)
                                    {
                                        if (oElem.HasAttributes == true)
                                        {
                                            SorterSummaryViewModel model = new SorterSummaryViewModel();
                                            model.LAST_AGG_TS = (oElem.Attribute("LAST_AGG_TS") != null) ? oElem.Attribute("LAST_AGG_TS").Value : String.Empty;
                                            model.SORTER_STACK_CNT = (oElem.Attribute("SORTER_STACK_CNT") != null) ? oElem.Attribute("SORTER_STACK_CNT").Value : String.Empty;
                                            model.SORTER_DNA_PERCENT = (oElem.Attribute("SORTER_DNA_PERCENT") != null) ? oElem.Attribute("SORTER_DNA_PERCENT").Value : String.Empty;
                                            model.SORTER_DNA_CNT = (oElem.Attribute("SORTER_DNA_CNT") != null) ? oElem.Attribute("SORTER_DNA_CNT").Value : String.Empty;
                                            model.SORTER_FPH_CNT = (oElem.Attribute("SORTER_FPH_CNT") != null) ? oElem.Attribute("SORTER_FPH_CNT").Value : String.Empty;
                                            model.SORTER_PKG_CNT = (oElem.Attribute("SORTER_PKG_CNT") != null) ? oElem.Attribute("SORTER_PKG_CNT").Value : String.Empty;
                                            model.SORTER_NAME = (oElem.Attribute("SORTER_NAME") != null) ? oElem.Attribute("SORTER_NAME").Value : String.Empty;
                                            summary.Add(model);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    // Something broke -- LogInterface.WriteLog(String.Format("{0} ERROR: Failed to retrieve SCADA information!\r\n{1}", sLocalTitle, e.ToString()));
                }
            }
            return summary;
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
