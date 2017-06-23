using DHLWebApiExample.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private static byte[] requestBytes;
        private static string responseString;

        public void XmlRequest(string url, string filePath)
        {
            string xmlText = File.ReadAllText(filePath);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            requestBytes = Encoding.GetEncoding("UTF-8").GetBytes(xmlText);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = requestBytes.Length;
            //request.Date = DateTime.Now;
            //request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);

            // Keep the main thread from continuing while the asynchronous 
            // operation completes. A real world application 
            // could do something useful such as updating its user interface. 
            allDone.WaitOne();
        }

        private static void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            // Write to the request stream.
            postStream.Write(requestBytes, 0, requestBytes.Length);
            postStream.Close();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }

        private static void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            responseString = streamRead.ReadToEnd();          

            DHLResponse resp = XmlResponse(responseString, REQUESTS.TRACKING);
            if (resp.RequestType == REQUESTS.TRACKING)
            {
                OutputTrackingResponse(resp.Trackings);
            }

            //Close the stream object
            streamResponse.Close();
            streamRead.Close();

            // Release the HttpWebResponse
            response.Close();
            allDone.Set();            
        }

        public static DHLResponse XmlResponse(string respString, REQUESTS reqType)
        { 
            DHLResponse resp = new DHLResponse();
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(respString);

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
                    resp.Trackings = ParseXmlTracking(respString);

                    break;
                default:
                    break;
            }
            resp.RequestType = reqType;
            return resp;
        }

        public void SetupCriteriaToRequestXml(string filePath)
        {
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode messageTime = doc.SelectSingleNode("//MessageTime");
            XmlNode siteID = doc.SelectSingleNode("//SiteID");
            XmlNode pwd = doc.SelectSingleNode("//Password");

            messageTime.InnerText = DateTime.Now.ToString("o");
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

        public static List<ResponseAWBInfo> ParseXmlTracking(string respString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(responseString);
            //try
            //{
            //    Stream respStream = response.GetResponseStream();
            //    using (var memStream = new MemoryStream())
            //    {
            //        respStream.CopyTo(memStream);
            //        memStream.Position = 0;
            //        doc.Load(memStream);
            //    }
            //    response.Close();
                
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Exception: {0}", ex.Message);
            //    Console.Read();
            //}

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
