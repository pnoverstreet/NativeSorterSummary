﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace NativeSorterSummary
{
    public class WebProxy : System.Net.IWebProxy
    {
        public System.Net.ICredentials Credentials
        {
            get;
            set;
        }
        private readonly Uri _proxyUri;
        public WebProxy(Uri proxyUri)
        {
            _proxyUri = proxyUri;
        }
        public Uri GetProxy(Uri destination)
        {
            return _proxyUri;
        }
        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}
