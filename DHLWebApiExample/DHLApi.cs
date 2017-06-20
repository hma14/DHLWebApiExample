using DHLWebApiExample.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DHLWebApiExample
{
    public enum REQUESTS
    {
        CAPABILITY = 1,
        IMAGE_UPLOAD,
        PICKUP,
        ROUTING,
        SHIPMENT,
        TRACKING
    }

    public class DHLApi
    {
        public WebResponse XmlRequest(string url, string filePath)
        {
            string xmlText = File.ReadAllText(filePath);
            WebRequest request = HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            using (var stream = request.GetRequestStream())
            {
                var arrBytes = ASCIIEncoding.ASCII.GetBytes(xmlText);
                stream.Write(arrBytes, 0, arrBytes.Length);
                stream.Close();
            }
            return request.GetResponse();
            
        }

        public DHLResponse XmlResponse(WebResponse response, REQUESTS reqType)
        {
#if true
            XmlDocument doc = new XmlDocument();
            DHLResponse resp = new DHLResponse();
            var respStream = response.GetResponseStream();

            doc.Load(respStream);

            switch (reqType)
            {
                case REQUESTS.CAPABILITY:

                    break;
                case REQUESTS.IMAGE_UPLOAD:

                    break;
                case REQUESTS.PICKUP:

                    break;
                case REQUESTS.ROUTING:

                    break;
                case REQUESTS.SHIPMENT:

                    break;
                case REQUESTS.TRACKING:
                    resp.Trackings = ParseXmlTracking(response);
                    
                    break;
                default:
                    break;
            }


            resp.RequestType = reqType;

            return resp;

#else
            var respStream = response.GetResponseStream();
            //var reader = new StreamReader(respStream, System.Text.Encoding.ASCII);
            XmlTextReader reader = new XmlTextReader(respStream);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    Console.WriteLine("{0}", reader.Value.Trim());
                }
            }
            Console.ReadLine();
#endif
        }

        public void SetupCriteriaToRequestXml(string filePath)
        {
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            
            XmlNode siteID = doc.SelectSingleNode("//SiteID");
            XmlNode pwd = doc.SelectSingleNode("//Password");
           
            siteID.InnerText = ConfigurationManager.AppSettings["DHL_SiteID"];
            pwd.InnerText = ConfigurationManager.AppSettings["DHL_Password"];

            doc.Save(filePath);

        }

        public void ParseXmlCapability(WebResponse response)
        {
            XmlDocument doc = new XmlDocument();
            var respStream = response.GetResponseStream();
            doc.Load(respStream);

            //string xmlText = File.ReadAllText(filePath);

            XmlNode FacilityCode = doc.SelectSingleNode("//FacilityCode");
            Console.WriteLine("FacilityCode = {0}", FacilityCode.InnerText);
        }


        public void ParseXmlBookPickup(WebResponse response)
        {
            XmlDocument doc = new XmlDocument();
            var respStream = response.GetResponseStream();
            doc.Load(respStream);

            //string xmlText = File.ReadAllText(filePath);

            XmlNode ConfirmationNumber = doc.SelectSingleNode("//ConfirmationNumber");
            Console.WriteLine("ConfirmationNumber = {0}", ConfirmationNumber.InnerText);

            XmlNode NextPickupDate = doc.SelectSingleNode("//NextPickupDate");
            Console.WriteLine("NextPickupDate = {0}", NextPickupDate.InnerText);
        }

        public void ParseXmlShipment(WebResponse response)
        {
            XmlDocument doc = new XmlDocument();
            var respStream = response.GetResponseStream();
            doc.Load(respStream);

            //string xmlText = File.ReadAllText(filePath);

            XmlNode ShipperID = doc.SelectSingleNode("//ShipperID");
            Console.WriteLine("ShipperID = {0}", ShipperID.InnerText);

            XmlNode CompanyName = doc.SelectSingleNode("//CompanyName");
            Console.WriteLine("CompanyName = {0}", CompanyName.InnerText);

            XmlNodeList AddressLines = doc.SelectNodes("//Shipper//AddressLine");
            foreach (XmlNode address in AddressLines)
            {
                Console.WriteLine("AddressLines = {0}", address.InnerText);
            }

            XmlNode City = doc.SelectSingleNode("//Shipper//City");
            Console.WriteLine("City = {0}", City.InnerText);
            XmlNode DivisionCode = doc.SelectSingleNode("//Shipper//DivisionCode");
            Console.WriteLine("DivisionCode = {0}", DivisionCode.InnerText);
            XmlNode CountryCode = doc.SelectSingleNode("//Shipper//CountryCode");
            Console.WriteLine("CountryCode = {0}", CountryCode.InnerText);
            XmlNode CountryName = doc.SelectSingleNode("//Shipper//CountryName");
            Console.WriteLine("CountryName = {0}", CountryName.InnerText);
        }

        public void ParseXmlTracking2(string filePath)
        {
            string xmlText = File.ReadAllText(filePath);
            XmlTextReader reader = new XmlTextReader(new StringReader(xmlText));
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    Console.WriteLine("{0}", reader.Value.Trim());
                }
            }
        }

        public List<ResponseAWBInfo> ParseXmlTracking(WebResponse response)
        {
            XmlDocument doc = new XmlDocument();
            var respStream = response.GetResponseStream();
            doc.Load(respStream);

            List<ResponseAWBInfo> AWBInfoList = new List<ResponseAWBInfo>();
            XmlNodeList AWBInfos = doc.SelectNodes("//AWBInfo");

            foreach (XmlNode awbInfo in AWBInfos)
            {
                ResponseAWBInfo AWBInfo = new ResponseAWBInfo();
                AWBInfo.ShipmentInfo = new ResponseShipmentInfo();

                XmlNode AWBNumber = awbInfo.SelectSingleNode("AWBNumber");
                AWBInfo.AWBNumber = AWBNumber.InnerText;

                XmlNode TrackedBy = awbInfo.SelectSingleNode("TrackedBy");
                AWBInfo.TrackedBy_LPNumber = TrackedBy.InnerText;

                XmlNode ActionStatus = awbInfo.SelectSingleNode("Status//ActionStatus");
                AWBInfo.Status_ActionStatus = ActionStatus.InnerText;

                XmlNodeList ShipmentInfos = awbInfo.SelectNodes("ShipmentInfo");
                //AWBInfo.ShipmentInfo.ShipmentEvents = new List<ResponseShipmentEvent>();
                foreach (XmlNode info in ShipmentInfos)
                {
                    XmlNode ServiceAreaCode = info.SelectSingleNode("OriginServiceArea//ServiceAreaCode");
                    XmlNode Description = info.SelectSingleNode("OriginServiceArea//Description");
                    XmlNode ServiceAreaCode2 = info.SelectSingleNode("DestinationServiceArea//ServiceAreaCode");
                    XmlNode Description2 = info.SelectSingleNode("DestinationServiceArea//Description");

                    XmlNode shipperName = info.SelectSingleNode("ShipperName");
                    XmlNode shipperAccountNumber = info.SelectSingleNode("ShipperAccountNumber");
                    XmlNode consigneeName = info.SelectSingleNode("ConsigneeName");
                    XmlNode shipmentDate = info.SelectSingleNode("ShipmentDate");

                    XmlNode pieces = info.SelectSingleNode("Pieces");
                    XmlNode weight = info.SelectSingleNode("Weight");
                    XmlNode weightUnit = info.SelectSingleNode("WeightUnit");
                    XmlNode globalProductCode = info.SelectSingleNode("GlobalProductCode");
                    XmlNode shipmentDesc = info.SelectSingleNode("ShipmentDesc");
                    XmlNode dlvyNotificationFlag = info.SelectSingleNode("DlvyNotificationFlag");

                    XmlNode city = info.SelectSingleNode("Shipper//City");
                    XmlNode postalCode = info.SelectSingleNode("Shipper//PostalCode");
                    XmlNode countryCode = info.SelectSingleNode("Shipper//CountryCode");
                    ResponseShipper shipper = new ResponseShipper
                    {
                        City = city.InnerText,
                        PostalCode = postalCode != null ? postalCode.InnerText : null,
                        CountryCode = countryCode != null ? countryCode.InnerText : null
                    };

                    city = info.SelectSingleNode("Consignee//City");
                    XmlNode divisionCode = info.SelectSingleNode("Consignee//DivisionCode");
                    postalCode = info.SelectSingleNode("Consignee//PostalCode");
                    countryCode = info.SelectSingleNode("Consignee//CountryCode");
                    ResponseConsignee consignee = new ResponseConsignee
                    {
                        City = city.InnerText,
                        DivisionCode = divisionCode != null ? divisionCode.InnerText : null,
                        PostalCode = postalCode != null ? postalCode.InnerText : null,
                        CountryCode = countryCode != null ? countryCode.InnerText : null
                    };

                    XmlNodeList ShipmentEvents = info.SelectNodes("ShipmentEvent");
                    List<ResponseShipmentEvent> events = new List<ResponseShipmentEvent>();
                    foreach (XmlNode evt in ShipmentEvents)
                    {
                        XmlNode date = evt.SelectSingleNode("Date");                       
                        XmlNode time = evt.SelectSingleNode("Time");
                        XmlNode eventCode = evt.SelectSingleNode("ServiceEvent//EventCode");
                        XmlNode description = evt.SelectSingleNode("ServiceEvent//Description");
                        XmlNode signatory = evt.SelectSingleNode("Signatory");
                        XmlNode serviceAreaCode = evt.SelectSingleNode("ServiceArea//ServiceAreaCode");
                        XmlNode description2 = evt.SelectSingleNode("ServiceArea//Description");

                        events.Add(new ResponseShipmentEvent
                        {
                            Date = date.InnerText,
                            Time = time.InnerText,
                            ServiceEvent_EventCode = eventCode.InnerText,
                            ServiceEvent_Description = description.InnerText,
                            Signatory = signatory.InnerText,
                            ServiceArea_ServiceAreaCode = serviceAreaCode.InnerText,
                            ServiceArea_Description = description2.InnerText
                        });
                        
                    }
                    AWBInfo.ShipmentInfo = new ResponseShipmentInfo
                    {
                        OriginServiceArea_ServiceAreaCode = ServiceAreaCode.InnerText,
                        OriginServiceArea_Description = Description.InnerText,
                        DestinationServiceArea_ServiceAreaCode = ServiceAreaCode2.InnerText,
                        DestinationServiceArea_Description = Description2.InnerText,
                        ShipperName = shipperName.InnerText,
                        ShipperAccountNumber = shipperAccountNumber.InnerText,
                        ConsigneeName = consigneeName.InnerText,
                        ShipmentDate = shipmentDate.InnerText != null ? DateTime.Parse(shipmentDate.InnerText) : DateTime.MinValue,
                        Pieces = pieces != null ? int.Parse(pieces.InnerText) : 0,
                        WeightUnit = weightUnit.InnerText,
                        GlobalProductCode = globalProductCode.InnerText,
                        ShipmentDesc = shipmentDesc.InnerText,
                        DlvyNotificationFlag = dlvyNotificationFlag.InnerText,

                        Shipper = shipper,
                        Consignee = consignee,
                        ShipmentEvents = events
                    };

                }
                XmlNode pieceDetails = awbInfo.SelectSingleNode("Pieces//PieceInfo//PieceDetails");

                AWBInfo.PieceDetails = new ReponsePieceDetails
                {
                    AWBNumber = pieceDetails.SelectSingleNode("AWBNumber").InnerText,
                    LicensePlate = pieceDetails.SelectSingleNode("LicensePlate").InnerText,
                    PieceNumber = int.Parse(pieceDetails.SelectSingleNode("PieceNumber").InnerText),
                    ActualDepth = float.Parse(pieceDetails.SelectSingleNode("ActualDepth").InnerText),
                    ActualWidth = pieceDetails.SelectSingleNode("ActualWidth").InnerText != null ? float.Parse(pieceDetails.SelectSingleNode("ActualWidth").InnerText) : 0f,
                    ActualHeight = pieceDetails.SelectSingleNode("ActualHeight").InnerText != null ? float.Parse(pieceDetails.SelectSingleNode("ActualHeight").InnerText) : 0f,
                    ActualWeight = pieceDetails.SelectSingleNode("ActualWeight").InnerText != null ? float.Parse(pieceDetails.SelectSingleNode("ActualWeight").InnerText) : 0f,
                    Depth = pieceDetails.SelectSingleNode("Depth").InnerText != null ? float.Parse(pieceDetails.SelectSingleNode("Depth").InnerText) : 0f,
                    Height = pieceDetails.SelectSingleNode("Height").InnerText != null ? float.Parse(pieceDetails.SelectSingleNode("Height").InnerText) : 0f,
                    Weight = pieceDetails.SelectSingleNode("Weight").InnerText != null ? float.Parse(pieceDetails.SelectSingleNode("Weight").InnerText) : 0f,
                    PackageType = pieceDetails.SelectSingleNode("PackageType") != null ? pieceDetails.SelectSingleNode("PackageType").InnerText : null,
                    DimWeight = pieceDetails.SelectSingleNode("DimWeight").InnerText != null ? float.Parse(pieceDetails.SelectSingleNode("DimWeight").InnerText) : 0f,
                    WeightUnit = pieceDetails.SelectSingleNode("WeightUnit").InnerText,
                };
                AWBInfoList.Add(AWBInfo);
            }
            return AWBInfoList;
        }
    }
}
