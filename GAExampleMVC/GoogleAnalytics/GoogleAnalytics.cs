using DotNetOpenAuth.OAuth2;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Services;
using Google.Apis.Util;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace GAExampleMVC.GoogleAnalytics
{
    public class GoogleAnalyticsService
    {
        public GoogleAnalyticsService()
        {
        }

        public IList<int> GetStats()
        {
            string scope = AnalyticsService.Scopes.AnalyticsReadonly.GetStringValue();

            //UPDATE this to match your developer account address. Note, you also need to add this address 
            //as a user on your Google Analytics profile which you want to extract data from (this may take
            //up to 15 mins to recognise)
            string client_id = "nnnnnnnnnnnn-nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn@developer.gserviceaccount.com";

            //UPDATE this to match the path to your certificate
            string key_file = @"E:\path\to\cert.p12";
            string key_pass = "notasecret";

            AuthorizationServerDescription desc = GoogleAuthenticationServer.Description;

            X509Certificate2 key = new X509Certificate2(key_file, key_pass, X509KeyStorageFlags.Exportable);

            AssertionFlowClient client =
                new AssertionFlowClient(desc, key) { ServiceAccountId = client_id, Scope = scope };

            OAuth2Authenticator<AssertionFlowClient> auth =
                new OAuth2Authenticator<AssertionFlowClient>(client, AssertionFlowClient.GetState);

            AnalyticsService gas = new AnalyticsService(new BaseClientService.Initializer() { Authenticator = auth });

            //UPDATE the ga:nnnnnnnn string to match your profile Id from Google Analytics
            DataResource.GaResource.GetRequest r =
                gas.Data.Ga.Get("ga:nnnnnnnn", "2013-01-01", "2013-01-31", "ga:visitors");

            r.Dimensions = "ga:pagePath";
            r.Sort = "-ga:visitors";
            r.MaxResults = 5;

            GaData d = r.Execute();

            IList<int> stats = new List<int>();

            for (int y = 0; y < d.Rows.Count; y++)
            {
                stats.Add(Convert.ToInt32(d.Rows[y][1]));
            }

            return stats;
        }
    }
}