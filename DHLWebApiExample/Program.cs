using DHLWebApiExample.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;

namespace DHLWebApiExample
{
    class Program
    {
        static void Main(string[] args)
        {
            DHLApi api = new DHLApi();
            

            string url = ConfigurationManager.AppSettings["DHLWebApi"];

            string fileName = "TrackingRequest_MultipleLP_PieceEnabled_B_1.xml";
            //string fileName = "Valid3_Capability_EU_NonEU_Dutiable.xml";
            //string fileName = "BookPickupResponse_GlobalAP_Valid1.xml";
            //string fileName = "ShipmentResponse_ShipmentValidateRequest_Global_AP_RegularShpmt.xml";
            //string fileName = "TrackingResponse_MultipleLP_PieceEnabled_B_1.xml";
#if false
            
            string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Xml/"), fileName);

#else
            string filePath = Path.Combine(Environment.CurrentDirectory + @"/Xml/", fileName);
#endif
            api.SetupCriteriaToRequestXml(filePath);

            //List<ResponseAWBInfo> responsInfos = api.ParseXmlTracking(response);
            //OutputTrackingResponse(responsInfos);

            //api.ParseXmlTracking2(response);


            api.XmlRequest(url, filePath);

            //HttpWebResponse response = (HttpWebResponse) api.XmlRequest(url, filePath);
            //DHLResponse resp = api.XmlResponse(response, REQUESTS.TRACKING);
            //if (resp.RequestType == REQUESTS.TRACKING)
            //{
            //    OutputTrackingResponse(resp.Trackings);
            //}

        }

        
    }
}
