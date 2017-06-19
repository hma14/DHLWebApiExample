set RESPONSE_PATH=TransformXMLtoPDF\ResponseXMLS\
set SERVER_URL=https://xmlpitest-ea.dhl.com/XMLShippingServlet
set INPUT_FILE=TransformXMLtoPDF\RequestXML\ShipmentValidateRequest_INT_DUT_AP_PieceEnabled_With2Pcs_PcsSeg.xml

java DHLClient %INPUT_FILE% %SERVER_URL% %RESPONSE_PATH%
