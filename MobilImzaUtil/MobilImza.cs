using System;
using System.Collections.Generic;
using System.IO;
using tr.gov.tubitak.uekae.esya.api.asn.x509;
using tr.gov.tubitak.uekae.esya.api.certificate.validation.policy;
using tr.gov.tubitak.uekae.esya.api.cmssignature;
using tr.gov.tubitak.uekae.esya.api.cmssignature.attribute;
using tr.gov.tubitak.uekae.esya.api.cmssignature.signature;
using tr.gov.tubitak.uekae.esya.api.common.util;
using tr.gov.tubitak.uekae.esya.api.crypto.alg;
using tr.gov.tubitak.uekae.esya.api.infra.mobile;
using tr.gov.tubitak.uekae.esya.api.webservice.mssclient.wrapper;

namespace MobilİmzaUtil
{
    public class MobilImza
    {
        public void MobileSignature()
        {
            LisansConfigure();

            string phoneNumber = "";
            int operatorEnum = 0;
            MobileSigner signer = initSigner(phoneNumber, operatorEnum);
            signData(@"C:\Users\P2049\Desktop\Visual Studio 2022.lnk", signer);
        }

        private void signData(string filePath, MobileSigner signer)
        {
            byte[] contentData = FileUtil.readBytes(filePath);

            BaseSignedData baseSignedData = new BaseSignedData();
            baseSignedData.addContent(new SignableByteArray(contentData), true);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters[EParameters.P_CERT_VALIDATION_POLICY] = getPolicy();
            parameters[EParameters.P_VALIDATE_CERTIFICATE_BEFORE_SIGNING] = false;

            List<IAttribute> optionalAttiributes = new List<IAttribute>();
            optionalAttiributes.Add(new SigningTimeAttr(DateTime.Now));

            baseSignedData.addSigner(ESignatureType.TYPE_BES, null, signer, optionalAttiributes, parameters);
            FileUtil.writeBytes(filePath + ".p7s", baseSignedData.getEncoded());
        }

        private ValidationPolicy getPolicy()
        {
            return PolicyReader.readValidationPolicy(new FileStream("resources/config/certval-policy.xml", FileMode.Open, FileAccess.Read));
        }

        private MobileSigner initSigner(string phoneNumber, int mobileOperator)
        {
            PhoneNumberAndOperator phoneNumberAndOperator = new PhoneNumberAndOperator(phoneNumber, (Operator)mobileOperator);

            MSSParams mobilParameters = getOperatorParams((Operator)mobileOperator);

            EMSSPClientConnector clientConnector = new EMSSPClientConnector(mobilParameters);
            clientConnector.setCertificateInitials(phoneNumberAndOperator);

            SignatureAlg signatureAlg = SignatureAlg.RSA_SHA256;
            ECertificate signerCertificate = null;
            string informationText = "Imza atacak onaylıyor musun?";
            return new MobileSigner(clientConnector, phoneNumberAndOperator, signerCertificate, informationText, signatureAlg.getName(), null);

        }

        private MSSParams getOperatorParams(Operator mobileOperator)
        {
            MSSParams mobilParams = null;
            if (mobileOperator == Operator.TURKCELL)
            {
                mobilParams = new MSSParams("***", "***", "www.turkcelltech.com");
                mobilParams.SetMsspSignatureQueryUrl("https://msign-test.turkcell.com.tr:443/MSSP2/services/MSS_Signature");
                mobilParams.SetMsspProfileQueryUrl("https://msign-test.turkcell.com.tr:443/MSSP2/services/MSS_ProfileQueryPort");

            }
            else if (mobileOperator == Operator.AVEA)
            {
                mobilParams = new MSSParams("***", "***", "");
                mobilParams.SetMsspSignatureQueryUrl("https://mobilimza.turktelekom.com.tr/EGAMsspWSAP2/MSS_SignatureService");
                mobilParams.SetMsspProfileQueryUrl("https://mobilimza.turktelekom.com.tr/EGAMsspWSAP2/MSS_ProfileQueryService");
            }
            else if (mobileOperator == Operator.VODAFONE)
            {
                mobilParams = new MSSParams("***", "***", "mobilimza.vodafone.com.tr");
                mobilParams.SetMsspSignatureQueryUrl("https://mobilimza.vodafone.com.tr:443/Dianta2/MSS_SignatureService");
            }

            mobilParams.QueryTimeOutInSeconds = 120;
            mobilParams.ConnectionTimeOutMs = 120000;

            return mobilParams;
        }

        private void LisansConfigure()
        {
            LicenseUtil.setLicenseXml(new FileStream("resources/lisans/lisans.xml", FileMode.Open, FileAccess.Read));
        }
    }
}
