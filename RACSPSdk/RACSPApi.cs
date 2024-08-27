using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tw.moica.RACSPSdk.DataObjects;

namespace tw.moica.RACSPSdk
{
    public partial class RACSPApi
    {
        private RACSPSdkConfig config { get; set; }

        public RACSPApi(RACSPSdkConfig config)
        {
            this.config = config;
        }

        public string spServiceId => config.SPServiceId;

        
        
        
    }
}