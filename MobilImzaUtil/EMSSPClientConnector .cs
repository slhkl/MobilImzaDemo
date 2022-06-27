using System;
using System.Collections.Generic;
using tr.gov.tubitak.uekae.esya.api.asn.cms;
using tr.gov.tubitak.uekae.esya.api.asn.x509;
using tr.gov.tubitak.uekae.esya.api.common.crypto;
using tr.gov.tubitak.uekae.esya.api.crypto.alg;
using tr.gov.tubitak.uekae.esya.api.infra.mobile;
using tr.gov.tubitak.uekae.esya.api.webservice.mssclient.wrapper;
using tr.gov.tubitak.uekae.esya.asn.cms;

namespace MobilİmzaUtil
{
    public class EMSSPClientConnector : MSSPClientConnector
    {
        private EMSSPRequestHandler msspRequestHandler;

        public EMSSPClientConnector(MSSParams mobilParams)
        {
            msspRequestHandler = new EMSSPRequestHandler(mobilParams);
        }

        public DigestAlg getDigestAlg()
        {
            return msspRequestHandler.getDigestAlg();
        }

        public ESignerIdentifier getSignerIdentifier()
        {
            return msspRequestHandler.getSignerIdentifier();
        }

        public ECertificate getSigningCert()
        {
            return msspRequestHandler.getSigningCert();
        }

        public SigningCertificate getSigningCertAttr()
        {
            return msspRequestHandler.getSigningCertAttr();
        }

        public SigningCertificateV2 getSigningCertAttrv2()
        {
            return msspRequestHandler.getSigningCertAttrv2();
        }

        public void setCertificateInitials(UserIdentifier aUserID)
        {
            msspRequestHandler.setCertificateInitials((PhoneNumberAndOperator)aUserID);
        }

        public byte[] sign(byte[] dataToBeSigned, SigningMode aMode, UserIdentifier aUserID, ECertificate aSigningCert, string informativeText, string aSigningAlg, IAlgorithmParameterSpec aParams)
        {
            return msspRequestHandler.Sign(dataToBeSigned, SigningMode.SIGNHASH, (PhoneNumberAndOperator)aUserID, informativeText, aSigningAlg);
        }

        public List<MultiSignResult> sign(List<byte[]> dataToBeSigned, SigningMode aMode, UserIdentifier aUserID, ECertificate aSigningCert, List<string> informativeText, string aSigningAlg, IAlgorithmParameterSpec aParams)
        {
            throw new NotImplementedException();
        }
    }
}
