//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, vJAXB 2.1.10 in JDK 6 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2012.09.14 at 12:04:40 PM IST 
//


package com.dhl.xmlpi.labelservice.errorresponse;

import java.math.BigDecimal;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlType;


/**
 * Piece
 * 
 * <p>Java class for Piece complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="Piece">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="LicencePlateNum" type="{http://www.dhl.com/LabelService_datatypes}LicencePlateNum"/>
 *         &lt;element name="Weight" type="{http://www.dhl.com/LabelService_datatypes}Weight" minOccurs="0"/>
 *         &lt;element name="Contents" type="{http://www.dhl.com/LabelService_datatypes}Contents" minOccurs="0"/>
 *         &lt;element name="Reference" type="{http://www.dhl.com/LabelService_datatypes}Reference" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "Piece", propOrder = {
    "licencePlateNum",
    "weight",
    "contents",
    "reference"
})
public class Piece {

    @XmlElement(name = "LicencePlateNum", required = true)
    protected String licencePlateNum;
    @XmlElement(name = "Weight")
    protected BigDecimal weight;
    @XmlElement(name = "Contents")
    protected String contents;
    @XmlElement(name = "Reference")
    protected String reference;

    /**
     * Gets the value of the licencePlateNum property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getLicencePlateNum() {
        return licencePlateNum;
    }

    /**
     * Sets the value of the licencePlateNum property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setLicencePlateNum(String value) {
        this.licencePlateNum = value;
    }

    /**
     * Gets the value of the weight property.
     * 
     * @return
     *     possible object is
     *     {@link BigDecimal }
     *     
     */
    public BigDecimal getWeight() {
        return weight;
    }

    /**
     * Sets the value of the weight property.
     * 
     * @param value
     *     allowed object is
     *     {@link BigDecimal }
     *     
     */
    public void setWeight(BigDecimal value) {
        this.weight = value;
    }

    /**
     * Gets the value of the contents property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getContents() {
        return contents;
    }

    /**
     * Sets the value of the contents property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setContents(String value) {
        this.contents = value;
    }

    /**
     * Gets the value of the reference property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getReference() {
        return reference;
    }

    /**
     * Sets the value of the reference property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setReference(String value) {
        this.reference = value;
    }

}