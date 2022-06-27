using MobilİmzaUtil;

namespace Mobilİmza
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Türk Telekom mobil imza için eklendi.
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            MobilImza mobilImza = new MobilImza();

            mobilImza.MobileSignature();
        }
    }
}
