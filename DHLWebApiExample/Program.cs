using DHLWebApiExample.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

namespace DHLWebApiExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //DHLApi api = new DHLApi();


            string url = ConfigurationManager.AppSettings["DHLWebApi"];

            //string xmlFile = "TrackingRequest_SingleAWB_10D.xml";
            //string xmlFile = "TrackingRequest_MultipleLP_PieceEnabled_B_1.xml";
            //string xmlFile = "Shipment_Global_AP_RegularShpmt_Request.xml";
            //string xmlFile = "Request.xml";
            //string xmlFile = "BookPickup_GlobalAP_Valid1_Request.xml";
            //string xmlFile = "Valid15_Quote_VolWeightHigher_Request.xml";            
            //string xmlFile = "Valid13_Quote_IMPPricebyReceiver_Request.xml";
            string xmlFile = "Valid10_Quote_AP_PriceBreakdownRAS_Request.xml";
            //string xmlFile = "Valid11_Quote_EU_PriceBreakdownRAS_Request.xml";
            //string xmlFile = "Valid14_Quote_IMPPriceby3rdParty_Request.xml";
            //string xmlFile = "Valid17_Quote_EU_NonEU_WithAcctProdInsurance_Request.xml";
            //string xmlFile = "Valid18_Quote_BRtoUS_Request.xml";
            //string xmlFile = "Valid19_Quote_PEtoEG_Suburb_Request.xml";
            //string xmlFile = "Valid20_Quote_BRtoBR_TaxBreakdownPricing_Request.xml";
            //string xmlFile = "Valid3_Capability_EU_NonEU_Dutiable_Request.xml";
            //string xmlFile = "Valid3_Capability_EU_NonEU_Dutiable_Request.xml";







#if false
            
            string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/DHLShippingXml/"), xmlFile);

#else
            string filePath = Path.Combine(Environment.CurrentDirectory + @"/DHLShippingXml/", xmlFile);
#endif
            // Setup RequestQuote object
            RequestBase requestQuote = new RequestQuote
            {
                RequestType = REQUESTS.CAPABILITY,

                Origin = new CountryPostalCode()
                {
                    CountryCode = "BE",
                    Postalcode = "1020",
                    City = "Lima"
                },

                BkgDetails = new BkgDetails()
                {
                    PaymentCountryCode = "BE",
                    Date = "2017-08-27",
                    ReadyTime = "PT10H22M",
                    ReadyTimeGMTOffset = "+02:00",
                    DimensionUnit = "CM",
                    WeightUnit = "KG",
                    Pieces = new List<Piece>()
                    {
                        new Piece()
                        {
                            PieceID = 1,
                            Height = 30,
                            Depth = 20,
                            Width = 10,
                            Weight = 1.0f,
                        },

                        new Piece()
                        {
                            PieceID = 2,
                            Height = 60,
                            Depth = 40,
                            Width = 20,
                            Weight = 2.0f,
                        }
                    },
                    IsDutiable = "Y",
                    NetworkTypeCode = "AL",
                    QtdShp = new QtdShp()
                    {
                        LocalProductCode = "S",
                        QtdShpExChrg_SpecialServiceType = "I",
                        QtdShpExChrg_LocalSpecialServiceType = "II"
                    },
                    InsuredValue = "400.000",
                    InsuredCurrency = "EUR",
                },
                Destination = new CountryPostalCode()
                {
                    CountryCode = "US",
                    Postalcode = "86001"
                },
                Dutiable = new Dutiable()
                {
                    DeclaredCurrency = "EUR",
                    DeclaredValue = "9.0"
                }
            };

            // Setup RequestTracking object 
            RequestBase requestTracking = new RequestTracking()
            {
                RequestType = REQUESTS.TRACKING,
                LanguageCode = "en",
                Waybill = "5093841045",
                //LPNumber = "JD0144549751510007712",               
                LevelOfDetails = "ALL_CHECK_POINTS",
                PiecesEnabled = "B"
            };

#if true
            DHLApi.SetupRequest(filePath, requestQuote);
            DHLApi.XmlRequest(url, filePath);
            DHLResponse resp = DHLApi.XmlResponse(DHLApi.ResponseXmlString, REQUESTS.CAPABILITY);
#else

            DHLApi.SetupRequest(filePath, requestTracking);
            DHLApi.XmlRequest(url, filePath);
            DHLResponse resp = DHLApi.XmlResponse(DHLApi.ResponseXmlString, REQUESTS.TRACKING);

#endif
            if (resp.RequestType == REQUESTS.TRACKING)
            {
                OutputTrackingResponse(resp.Trackings);
            }
            else 
            {
                OutputQuoteResponse(resp.Quote);
            }
        }

        private static void Printout(string xmlString)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                        Console.Write("{0} ", reader.Name);
                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        Console.WriteLine(" = {0}", reader.Value.Trim());
                        Console.WriteLine();
                    }

                }
                Console.ReadLine();
            }
        }

        private static void OutputQuoteResponse(ResponseQuote info)
        {
            ServiceArea osa = info.BkgDetails.OriginServiceArea;
            Console.WriteLine("Origin Service Area:\n");
            Console.WriteLine("FacilityCode: {0}", osa.FacilityCode);
            Console.WriteLine("ServiceAreaCode: {0}", osa.ServiceAreaCode);
            Console.WriteLine("-----------------");

            ServiceArea dsa = info.BkgDetails.DestinationServiceArea;
            Console.WriteLine("Destination Service Area:\n");
            Console.WriteLine("FacilityCode: {0}", dsa.FacilityCode);
            Console.WriteLine("ServiceAreaCode: {0}", dsa.ServiceAreaCode);
            Console.WriteLine("-----------------");

            foreach ( var qtdShp in info.BkgDetails.QtdShps)
            {
                Console.WriteLine("QtdShp:\n");
                Console.WriteLine("PickupDate: {0}", qtdShp.PickupDate);
                Console.WriteLine("PickupCutoffTime: {0}", qtdShp.PickupCutoffTime);
                Console.WriteLine("BookingTime: {0}", qtdShp.BookingTime);
                Console.WriteLine("CurrencyCode: {0}", qtdShp.CurrencyCode);
                Console.WriteLine("ExchangeRate: {0}", qtdShp.ExchangeRate);
                Console.WriteLine("WeightCharge: {0}", qtdShp.WeightCharge);
                Console.WriteLine("WeightChargeTax: {0}", qtdShp.WeightChargeTax);
                Console.WriteLine("TotalTransitDays: {0}", qtdShp.TotalTransitDays);
                Console.WriteLine("PickupPostalLocAddDays: {0}", qtdShp.PickupPostalLocAddDays);
                Console.WriteLine("DeliveryPostalLocAddDays: {0}", qtdShp.DeliveryPostalLocAddDays);
                Console.WriteLine("DeliveryDate: {0}", qtdShp.DeliveryDate);
                Console.WriteLine("DeliveryTime: {0}", qtdShp.DeliveryTime);
                Console.WriteLine("DimensionalWeight: {0}", qtdShp.DimensionalWeight);
                Console.WriteLine("WeightUnit: {0}", qtdShp.WeightUnit);
                Console.WriteLine("PickupDayOfWeekNum: {0}", qtdShp.PickupDayOfWeekNum);
                Console.WriteLine("DestinationDayOfWeekNum: {0}", qtdShp.DestinationDayOfWeekNum);
                Console.WriteLine("-----------------");

                if (qtdShp.QtdShpExChrgs != null)
                {
                    foreach (var chrg in qtdShp.QtdShpExChrgs)
                    {
                        
                        Console.WriteLine("QtdShpExChrg:\n");
                        Console.WriteLine("SpecialServiceType: {0}", chrg.SpecialServiceType);
                        Console.WriteLine("LocalServiceType: {0}", chrg.LocalServiceType);
                        Console.WriteLine("GlobalServiceName: {0}", chrg.GlobalServiceName);
                        Console.WriteLine("LocalServiceTypeName: {0}", chrg.LocalServiceTypeName);
                        Console.WriteLine("ChargeCodeType: {0}", chrg.ChargeCodeType);
                        Console.WriteLine("CurrencyCode: {0}", chrg.CurrencyCode);
                        Console.WriteLine("ChargeValue: {0}", chrg.ChargeValue);
                        Console.WriteLine("ChargeTaxAmount: {0}", chrg.ChargeTaxAmount);

                        Console.WriteLine("TaxTypeRate: {0}", chrg.ChargeTaxAmountDet.TaxTypeRate);
                        Console.WriteLine("TaxTypeCode: {0}", chrg.ChargeTaxAmountDet.TaxTypeCode);
                        Console.WriteLine("TaxAmount: {0}", chrg.ChargeTaxAmountDet.TaxAmount);
                        Console.WriteLine("BaseAmount: {0}", chrg.ChargeTaxAmountDet.BaseAmount);
                        Console.WriteLine("-----------------");


                        foreach (var adCur in chrg.QtdSExtrChrgInAdCurList)
                        {
                            Console.WriteLine("QtdSExtrChrgInAdCur:\n");
                            Console.WriteLine("ChargeValue: {0}", adCur.ChargeValue);
                            Console.WriteLine("ChargeTaxAmount: {0}", adCur.ChargeTaxAmount);
                            Console.WriteLine("CurrencyCode: {0}", adCur.CurrencyCode);
                            Console.WriteLine("CurrencyRoleTypeCode: {0}", adCur.CurrencyRoleTypeCode);

                            Console.WriteLine("TaxTypeRate: {0}", adCur.ChargeTaxAmountDet.TaxTypeRate);
                            Console.WriteLine("TaxTypeCode: {0}", adCur.ChargeTaxAmountDet.TaxTypeCode);
                            Console.WriteLine("TaxAmount: {0}", adCur.ChargeTaxAmountDet.TaxAmount);
                            Console.WriteLine("BaseAmount: {0}", adCur.ChargeTaxAmountDet.BaseAmount);
                            Console.WriteLine("-----------------");
                        }

                        Console.WriteLine("-----------------");
                    }
                }
                Console.WriteLine("PricingDate: {0}", qtdShp.PricingDate);
                Console.WriteLine("ShippingCharge: {0}", qtdShp.ShippingCharge);
                Console.WriteLine("TotalTaxAmount: {0}", qtdShp.TotalTaxAmount);

                foreach(var adCur in qtdShp.QtdSInAdCurList)
                {
                    Console.WriteLine("QtdSInAdCur:\n");
                    Console.WriteLine("CurrencyCode: {0}", adCur.CurrencyCode);
                    Console.WriteLine("CurrencyRoleTypeCode: {0}", adCur.CurrencyRoleTypeCode);
                    Console.WriteLine("WeightCharge: {0}", adCur.WeightCharge);
                    Console.WriteLine("TotalAmount: {0}", adCur.TotalAmount);
                    Console.WriteLine("TotalTaxAmount: {0}", adCur.TotalTaxAmount);
                    Console.WriteLine("WeightChargeTax: {0}", adCur.WeightChargeTax);

                    Console.WriteLine("WeightChargeTaxDet:\n");
                    Console.WriteLine("TaxTypeRate: {0}", adCur.WeightChargeTaxDet.TaxTypeRate);
                    Console.WriteLine("TaxTypeCode: {0}", adCur.WeightChargeTaxDet.TaxTypeCode);
                    Console.WriteLine("WeightChargeTax: {0}", adCur.WeightChargeTaxDet.WeightChargeTax);
                    Console.WriteLine("BaseAmt: {0}", adCur.WeightChargeTaxDet.BaseAmt);


                    Console.WriteLine("-----------------");                    
                }
                Console.WriteLine("WeightChargeTaxDet:\n");
                Console.WriteLine("TaxTypeRate: {0}", qtdShp.WeightChargeTaxDet.TaxTypeRate);
                Console.WriteLine("TaxTypeCode: {0}", qtdShp.WeightChargeTaxDet.TaxTypeCode);
                Console.WriteLine("WeightChargeTax: {0}", qtdShp.WeightChargeTaxDet.WeightChargeTax);
                Console.WriteLine("BaseAmt: {0}", qtdShp.WeightChargeTaxDet.BaseAmt);


                Console.WriteLine("-----------------");
            }

            Console.WriteLine("Srvs:\n");
            foreach (var srv in info.Srvs)
            {
                Console.WriteLine("Srv:\n");
                Console.WriteLine("GlobalProductCode: {0}", srv.GlobalProductCode);
                foreach(var mrkSrv in srv.MrkSrvs)
                {
                    Console.WriteLine("MrkSrv:\n");
                    if (mrkSrv.LocalProductCode != null)
                    {
                        Console.WriteLine("LocalProductCode: {0}", mrkSrv.LocalProductCode);
                    }
                    

                    if (mrkSrv.ProductShortName != null)
                    {
                        Console.WriteLine("ProductShortName: {0}", mrkSrv.ProductShortName);
                    }
                    

                    if (mrkSrv.LocalProductName != null)
                    {
                        Console.WriteLine("LocalProductName: {0}", mrkSrv.LocalProductName);
                    }
                    

                    if (mrkSrv.NetworkTypeCode != null)
                    {
                        Console.WriteLine("NetworkTypeCode: {0}", mrkSrv.NetworkTypeCode);
                    }
                    

                    if (mrkSrv.POfferedCustAgreement != null)
                    {
                        Console.WriteLine("POfferedCustAgreement: {0}", mrkSrv.POfferedCustAgreement);
                    }
                    
                    if (mrkSrv.TransInd != null)
                    {
                        Console.WriteLine("TransInd: {0}", mrkSrv.TransInd);
                    }

                    
                    if (mrkSrv.LocalServiceType != null)
                    {
                        Console.WriteLine("LocalServiceType: {0}", mrkSrv.LocalServiceType);
                    }

                    
                    if (mrkSrv.GlobalServiceName != null)
                    {
                        Console.WriteLine("GlobalServiceName: {0}", mrkSrv.GlobalServiceName);
                    }
                    
                    if (mrkSrv.LocalServiceTypeName != null)
                    {
                        Console.WriteLine("LocalServiceTypeName: {0}", mrkSrv.LocalServiceTypeName);
                    }
                   
                    if (mrkSrv.ChargeCodeType != null)
                    {
                        Console.WriteLine("ChargeCodeType: {0}", mrkSrv.ChargeCodeType);
                    }
                    
                    if (mrkSrv.MrkSrvInd != null)
                    {
                        Console.WriteLine("MrkSrvInd: {0}", mrkSrv.MrkSrvInd);
                    }
                    
                    Console.WriteLine("-----------------");
                }
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

                if (inf.PieceDetails != null)
                {
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
            }
            Console.Read();
        }
    }
}
