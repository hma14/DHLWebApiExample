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
            //DHLApi api = new DHLApi();
            

            string url = ConfigurationManager.AppSettings["DHLWebApi"];

            //string xmlFile = "TrackingRequest_MultipleLP_PieceEnabled_B_1.xml";
            string xmlFile = "Valid3_Capability_EU_NonEU_Dutiable_Request.xml";
            //string xmlFile = "Shipment_Global_AP_RegularShpmt_Request.xml";
            //string xmlFile = "Request.xml";
            //string xmlFile = "BookPickup_GlobalAP_Valid1_Request.xml";
            //string xmlFile = "TrackingResponse_MultipleLP_PieceEnabled_B_1.xml";
#if false
            
            string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/Xml/"), xmlFile);

#else
            string filePath = Path.Combine(Environment.CurrentDirectory + @"/Xml/", xmlFile);
#endif
            DHLApi.SetupCriteriaToRequestXml(filePath);

            DHLApi.XmlRequest(url, filePath);
            DHLResponse resp = DHLApi.XmlResponse(DHLApi.ResponseXmlString, REQUESTS.CAPABILITY);
            if (resp.RequestType == REQUESTS.TRACKING)
            {
                OutputTrackingResponse(resp.Trackings);
            }
            else
            {
                Console.WriteLine(DHLApi.ResponseXmlString);
            }
        }

        private static void OutputTrackingResponse(List<ResponseAWBInfo> info)
        {
            int nu = 0;
            foreach (var inf in info)
            {
                Console.WriteLine("\n--- No. {0} ---\n", ++nu);
                Console.WriteLine("AWBNumber = {0}", inf.AWBNumber);
                Console.WriteLine("TrackedBy_LPNumber = {0}", inf.TrackedBy_LPNumber);
                Console.WriteLine("Status_ActionStatus = {0}", inf.Status_ActionStatus);

                Console.WriteLine("OriginServiceArea_ServiceAreaCode = {0}", inf.ShipmentInfo.OriginServiceArea_ServiceAreaCode);
                Console.WriteLine("OriginServiceArea_Description = {0}", inf.ShipmentInfo.OriginServiceArea_Description);
                Console.WriteLine("DestinationServiceArea_ServiceAreaCode = {0}", inf.ShipmentInfo.DestinationServiceArea_ServiceAreaCode);
                Console.WriteLine("DestinationServiceArea_Description = {0}", inf.ShipmentInfo.DestinationServiceArea_Description);
                Console.WriteLine("ShipperName = {0}", inf.ShipmentInfo.ShipperName);
                Console.WriteLine("ShipperAccountNumber = {0}", inf.ShipmentInfo.ShipperAccountNumber);
                Console.WriteLine("ConsigneeName = {0}", inf.ShipmentInfo.ConsigneeName);
                Console.WriteLine("ShipmentDate = {0}", inf.ShipmentInfo.ShipmentDate);
                Console.WriteLine("Pieces = {0}", inf.ShipmentInfo.Pieces);
                Console.WriteLine("Weight = {0}", inf.ShipmentInfo.Weight);
                Console.WriteLine("WeightUnit = {0}", inf.ShipmentInfo.WeightUnit);
                Console.WriteLine("GlobalProductCode = {0}", inf.ShipmentInfo.GlobalProductCode);
                Console.WriteLine("ShipmentDesc = {0}", inf.ShipmentInfo.ShipmentDesc);
                Console.WriteLine("DlvyNotificationFlag = {0}", inf.ShipmentInfo.DlvyNotificationFlag);

                Console.WriteLine("City = {0}", inf.ShipmentInfo.Shipper.City);
                Console.WriteLine("PostalCode = {0}", inf.ShipmentInfo.Shipper.PostalCode);
                Console.WriteLine("CountryCode = {0}", inf.ShipmentInfo.Shipper.CountryCode);
                Console.WriteLine("Consignee.City = {0}", inf.ShipmentInfo.Consignee.City);
                Console.WriteLine("Consignee.DivisionCode = {0}", inf.ShipmentInfo.Consignee.DivisionCode);
                Console.WriteLine("Consignee.PostalCode = {0}", inf.ShipmentInfo.Consignee.PostalCode);
                Console.WriteLine("Consignee.CountryCode = {0}", inf.ShipmentInfo.Consignee.CountryCode);

                foreach (var evt in inf.ShipmentInfo.ShipmentEvents)
                {
                    Console.WriteLine("ResponseShipmentEvent Date = {0}", evt.Date);
                    Console.WriteLine("ResponseShipmentEvent Time = {0}", evt.Time);
                    Console.WriteLine("ResponseShipmentEvent ServiceEvent_EventCode = {0}", evt.ServiceEvent_EventCode);
                    Console.WriteLine("ResponseShipmentEvent ServiceEvent_Description = {0}", evt.ServiceEvent_Description);
                    Console.WriteLine("ResponseShipmentEvent Signatory = {0}", evt.Signatory);
                    Console.WriteLine("ResponseShipmentEvent ServiceArea_ServiceAreaCode = {0}", evt.ServiceArea_ServiceAreaCode);
                    Console.WriteLine("ResponseShipmentEvent ServiceArea_Description = {0}", evt.ServiceArea_Description);
                }

                Console.WriteLine("PieceDetails.AWBNumber = {0}", inf.PieceDetails.AWBNumber);
                Console.WriteLine("PieceDetails.LicensePlate = {0}", inf.PieceDetails.LicensePlate);
                Console.WriteLine("PieceDetails.PieceNumber = {0}", inf.PieceDetails.PieceNumber);
                Console.WriteLine("PieceDetails.ActualDepth = {0}", inf.PieceDetails.ActualDepth);
                Console.WriteLine("PieceDetails.ActualWidth = {0}", inf.PieceDetails.ActualWidth);
                Console.WriteLine("PieceDetails.ActualHeight = {0}", inf.PieceDetails.ActualHeight);
                Console.WriteLine("PieceDetails.ActualWeight = {0}", inf.PieceDetails.ActualWeight);
                Console.WriteLine("PieceDetails.Depth = {0}", inf.PieceDetails.Depth);
                Console.WriteLine("PieceDetails.Height = {0}", inf.PieceDetails.Height);
                Console.WriteLine("PieceDetails.Weight = {0}", inf.PieceDetails.Weight);
                Console.WriteLine("PieceDetails.PackageType = {0}", inf.PieceDetails.PackageType);
                Console.WriteLine("PieceDetails.DimWeight = {0}", inf.PieceDetails.DimWeight);
                Console.WriteLine("PieceDetails.WeightUnit = {0}", inf.PieceDetails.WeightUnit);
            }
            Console.Read();
        }
    }
}
