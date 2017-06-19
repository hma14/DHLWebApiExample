//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, vJAXB 2.1.10 in JDK 6 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2012.09.04 at 10:21:23 AM IST 
//


package com.dhl.xmlpi.labelservice.model.response;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlElementDecl;
import javax.xml.bind.annotation.XmlRegistry;
import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the com.dhl.xmlpi.labelservice.model.response package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {

    private final static QName _DataTypes_QNAME = new QName("http://www.dhl.com/LabelService_datatypes", "DataTypes");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: com.dhl.xmlpi.labelservice.model.response
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link Handlings }
     * 
     */
    public Handlings createHandlings() {
        return new Handlings();
    }

    /**
     * Create an instance of {@link Response }
     * 
     */
    public Response createResponse() {
        return new Response();
    }

    /**
     * Create an instance of {@link Piece }
     * 
     */
    public Piece createPiece() {
        return new Piece();
    }

    /**
     * Create an instance of {@link Note }
     * 
     */
    public Note createNote() {
        return new Note();
    }

    /**
     * Create an instance of {@link LabelPrintCommands }
     * 
     */
    public LabelPrintCommands createLabelPrintCommands() {
        return new LabelPrintCommands();
    }

    /**
     * Create an instance of {@link Condition }
     * 
     */
    public Condition createCondition() {
        return new Condition();
    }

    /**
     * Create an instance of {@link Origin }
     * 
     */
    public Origin createOrigin() {
        return new Origin();
    }

    /**
     * Create an instance of {@link AddrMap }
     * 
     */
    public AddrMap createAddrMap() {
        return new AddrMap();
    }

    /**
     * Create an instance of {@link Destination }
     * 
     */
    public Destination createDestination() {
        return new Destination();
    }

    /**
     * Create an instance of {@link Service }
     * 
     */
    public Service createService() {
        return new Service();
    }

    /**
     * Create an instance of {@link ServiceHeaderRequest }
     * 
     */
    public ServiceHeaderRequest createServiceHeaderRequest() {
        return new ServiceHeaderRequest();
    }

    /**
     * Create an instance of {@link Pieces }
     * 
     */
    public Pieces createPieces() {
        return new Pieces();
    }

    /**
     * Create an instance of {@link Labels }
     * 
     */
    public Labels createLabels() {
        return new Labels();
    }

    /**
     * Create an instance of {@link Handling }
     * 
     */
    public Handling createHandling() {
        return new Handling();
    }

    /**
     * Create an instance of {@link LabelResponse }
     * 
     */
    public LabelResponse createLabelResponse() {
        return new LabelResponse();
    }

    /**
     * Create an instance of {@link Shipment }
     * 
     */
    public Shipment createShipment() {
        return new Shipment();
    }

    /**
     * Create an instance of {@link ServiceHeaderResponse }
     * 
     */
    public ServiceHeaderResponse createServiceHeaderResponse() {
        return new ServiceHeaderResponse();
    }

    /**
     * Create an instance of {@link Services }
     * 
     */
    public Services createServices() {
        return new Services();
    }

    /**
     * Create an instance of {@link Request }
     * 
     */
    public Request createRequest() {
        return new Request();
    }

    /**
     * Create an instance of {@link Label }
     * 
     */
    public Label createLabel() {
        return new Label();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Object }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://www.dhl.com/LabelService_datatypes", name = "DataTypes")
    public JAXBElement<Object> createDataTypes(Object value) {
        return new JAXBElement<Object>(_DataTypes_QNAME, Object.class, null, value);
    }

}
