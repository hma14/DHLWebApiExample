using DHLWebApiExample.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        private static byte[] RequestBytes { get; set; }
        public static string ResponseXmlString { get; set; }

        public static void XmlRequest(string url, string filePath)
        {
            string xmlText = File.ReadAllText(filePath);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            RequestBytes = Encoding.GetEncoding("UTF-8").GetBytes(xmlText);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = RequestBytes.Length;
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
            postStream.Write(RequestBytes, 0, RequestBytes.Length);
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
            ResponseXmlString = streamRead.ReadToEnd();



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

        public static void SetupRequest(string filePath, RequestBase model, REQUESTS requestType)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            // Set ServiceHeader
            XmlNode messageTime = doc.SelectSingleNode("//MessageTime");
            XmlNode messageReference = doc.SelectSingleNode("//MessageReference");
            XmlNode siteID = doc.SelectSingleNode("//SiteID");
            XmlNode pwd = doc.SelectSingleNode("//Password");

            // Set values to elements in ServiceHeader pull out from above

            messageTime.InnerText = DateTime.Now.ToString("o");
            messageReference.InnerText = ConfigurationManager.AppSettings["MessageReference"];
            siteID.InnerText = ConfigurationManager.AppSettings["DHL_SiteID"];
            pwd.InnerText = ConfigurationManager.AppSettings["DHL_Password"];

            if (requestType == REQUESTS.CAPABILITY)
            {
                SetupQuoteRequest(filePath, ref doc, (RequestQuote)model, REQUESTS.CAPABILITY);
            }
            else if (requestType == REQUESTS.TRACKING)
            {
                SetupTrackingRequest(filePath, ref doc, (RequestTracking)model, REQUESTS.TRACKING);
            }
        }

        private static void SetupQuoteRequest(string filePath, ref XmlDocument doc, RequestQuote model, REQUESTS requestType)
        {         

            XmlNode FromCountryCode = doc.SelectSingleNode("//From//CountryCode");
            FromCountryCode.InnerText = model.Origin.CountryCode;
            
            XmlNode FromPostalcode = doc.SelectSingleNode("//From//Postalcode");
            if (FromPostalcode != null)
            {
                FromPostalcode.InnerText = model.Origin.Postalcode;
            }
            XmlNode FromCity = doc.SelectSingleNode("//From//City");
            if (FromCity != null)
            {
                FromCity.InnerText = model.Origin.City;
            }

            XmlNode paymentCountryCode = doc.SelectSingleNode("//BkgDetails//PaymentCountryCode");
            XmlNode date = doc.SelectSingleNode("//BkgDetails//Date");
            XmlNode readyTime = doc.SelectSingleNode("//BkgDetails//ReadyTime");
            XmlNode readyTimeGMTOffset = doc.SelectSingleNode("//BkgDetails//ReadyTimeGMTOffset");
            XmlNode dimensionUnit = doc.SelectSingleNode("//BkgDetails//DimensionUnit");
            XmlNode weightUnit = doc.SelectSingleNode("//BkgDetails//WeightUnit");

            XmlNode pieceID = doc.SelectSingleNode("//Pieces//Piece//PieceID");
            XmlNode height = doc.SelectSingleNode("//Pieces//Piece//Height");
            XmlNode depth = doc.SelectSingleNode("//Pieces//Piece//Depth");
            XmlNode width = doc.SelectSingleNode("//Pieces//Piece//Width");
            XmlNode weight = doc.SelectSingleNode("//Pieces//Piece//Weight");

            XmlNode isDutiable = doc.SelectSingleNode("//BkgDetails//IsDutiable");
            XmlNode networkTypeCode = doc.SelectSingleNode("//BkgDetails//NetworkTypeCode");
            
            
            XmlNode declaredCurrency = doc.SelectSingleNode("//Dutiable//DeclaredCurrency");
            XmlNode declaredValue = doc.SelectSingleNode("//Dutiable//DeclaredValue");

            XmlNode ToCountryCode = doc.SelectSingleNode("//To//CountryCode");
            ToCountryCode.InnerText = model.Destination.CountryCode;

            XmlNode ToPostalcode = doc.SelectSingleNode("//To//Postalcode");
            XmlNode ToCity = doc.SelectSingleNode("//To//City");
            if (ToPostalcode != null)
            {
                ToPostalcode.InnerText = model.Destination.Postalcode;
            }
            if (ToCity != null)
            {
                ToCity.InnerText = model.Destination.City;
            }


            // Set values to elements pull out from above


            paymentCountryCode.InnerText = model.BkgDetails.PaymentCountryCode;
            date.InnerText = model.BkgDetails.Date;
            readyTime.InnerText = model.BkgDetails.ReadyTime;
            readyTimeGMTOffset.InnerText = model.BkgDetails.ReadyTimeGMTOffset;
            dimensionUnit.InnerText = model.BkgDetails.DimensionUnit;
            weightUnit.InnerText = model.BkgDetails.WeightUnit;

            

            if (model.BkgDetails.Pieces.Count > 1)
            {
                XmlNode Pieces = doc.SelectSingleNode("//Pieces");
                foreach (var piece in model.BkgDetails.Pieces.Skip(1))
                {
                    XmlNode Piece = doc.CreateNode(XmlNodeType.Element, "Piece", "");

                    XmlNode PieceID = doc.CreateNode(XmlNodeType.Element, "PieceID", "");
                    PieceID.InnerText = piece.PieceID.ToString();

                    XmlNode Height = doc.CreateNode(XmlNodeType.Element, "Height", "");
                    Height.InnerText = piece.Height.ToString();

                    XmlNode Depth = doc.CreateNode(XmlNodeType.Element, "Depth", "");
                    Depth.InnerText = piece.Depth.ToString();

                    XmlNode Width = doc.CreateNode(XmlNodeType.Element, "Width", "");
                    Width.InnerText = piece.Width.ToString();

                    XmlNode Weight = doc.CreateNode(XmlNodeType.Element, "Weight", "");
                    Weight.InnerText = piece.Weight.ToString();

                    Piece.AppendChild(PieceID);
                    Piece.AppendChild(Height);
                    Piece.AppendChild(Depth);
                    Piece.AppendChild(Width);
                    Piece.AppendChild(Weight);

                    Pieces.AppendChild(Piece);
                }
            }
            isDutiable.InnerText = model.BkgDetails.IsDutiable;
            
            if (networkTypeCode != null)
            {
                networkTypeCode.InnerText = model.BkgDetails.NetworkTypeCode;
            }

            XmlNode globalProductCode = doc.SelectSingleNode("//BkgDetails//QtdShp//GlobalProductCode");
            if (model.BkgDetails.QtdShp != null && globalProductCode != null && model.BkgDetails.QtdShp.GlobalProductCode != null)
            {
                globalProductCode.InnerText = model.BkgDetails.QtdShp.GlobalProductCode;
            }
                XmlNode localProductCode = doc.SelectSingleNode("//BkgDetails//QtdShp//LocalProductCode");
            if (model.BkgDetails.QtdShp != null && localProductCode != null)
            {                
                XmlNode specialServiceType = doc.SelectSingleNode("//BkgDetails//QtdShp//QtdShpExChrg//SpecialServiceType");
                if (specialServiceType != null)
                {
                    XmlNode localSpecialServiceType = doc.SelectSingleNode("//BkgDetails//QtdShp//QtdShpExChrg//LocalSpecialServiceType");
                    specialServiceType.InnerText = model.BkgDetails.QtdShp.QtdShpExChrg_SpecialServiceType;
                    localSpecialServiceType.InnerText = model.BkgDetails.QtdShp.QtdShpExChrg_LocalSpecialServiceType;
                }
                
                localProductCode.InnerText = model.BkgDetails.QtdShp.LocalProductCode;               
            }
            XmlNode insuredValue = doc.SelectSingleNode("//BkgDetails//InsuredValue");
            XmlNode insuredCurrency = doc.SelectSingleNode("//BkgDetails//InsuredCurrency");
            if (insuredValue != null)
            {
                insuredValue.InnerText = model.BkgDetails.InsuredValue;
                insuredCurrency.InnerText = model.BkgDetails.InsuredCurrency;
            }
          
            declaredCurrency.InnerText = model.Dutiable.DeclaredCurrency;
            declaredValue.InnerText = model.Dutiable.DeclaredValue.ToString();

            doc.Save(filePath);

        }

        private static void SetupTrackingRequest(string filePath, ref XmlDocument doc, RequestTracking model, REQUESTS requestType)
        {

            XmlNode languageCode = doc.SelectSingleNode("//LanguageCode");
            languageCode.InnerText = model.LanguageCode;
            XmlNode lPNumber = doc.SelectSingleNode("//LPNumber");
            //if (model.LPNumbers.Count > 1)
            //{
            //    foreach (var LPNumber in model.LPNumbers.Skip(1))
            //    {
            //        doc.CreateNode(XmlNodeType.Element, "LPNumber", "");
            //        XmlNode number = doc.SelectSingleNode("//LPNumber");
            //        number.InnerText = LPNumber;
            //        doc.AppendChild(number);
            //    }
            //}
            XmlNode levelOfDetails = doc.SelectSingleNode("//LevelOfDetails");
            levelOfDetails.InnerText = model.LevelOfDetails;
            XmlNode piecesEnabled = doc.SelectSingleNode("//PiecesEnabled");
            piecesEnabled.InnerText = model.PiecesEnabled;

            doc.Save(filePath);
        }

        public static void SetupCriteriaToRequestXml(string filePath)
        {
            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode messageTime = doc.SelectSingleNode("//MessageTime");
            XmlNode messageReference = doc.SelectSingleNode("//MessageReference");
            XmlNode siteID = doc.SelectSingleNode("//SiteID");
            XmlNode pwd = doc.SelectSingleNode("//Password");
            XmlNode date = doc.SelectSingleNode("//Date");

            messageTime.InnerText = DateTime.Now.ToString("o");
            messageReference.InnerText = "0987654321098765432109876543";
            siteID.InnerText = ConfigurationManager.AppSettings["DHL_SiteID"];
            pwd.InnerText = ConfigurationManager.AppSettings["DHL_Password"];
            date.InnerText = DateTime.Now.ToString("yyyy-MM-dd");

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

        public static List<ResponseAWBInfo> ParseXmlTracking(string respString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(respString);

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

                AWBInfo.PieceDetails = new ResponsePieceDetails
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
