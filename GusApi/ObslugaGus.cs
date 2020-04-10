using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Serialization;
using Gus;
using GusApi.Models;
using WcfCoreMtomEncoder;

namespace GusApi
{
    public class ObslugaGus
    {
        private readonly UslugaBIRzewnPublClient GusServices;
        private readonly string apiKey;
        private string sessionId;

        public ObslugaGus(string apiKey)
        {
            this.apiKey = apiKey;
            GusServices = new UslugaBIRzewnPublClient();
            SetupBinding();
        }

        public PodmiotGus PobierzDanePodmiotu(string nip)
        {
            LoginIfRequired();

            ParametryWyszukiwania nipData = new ParametryWyszukiwania();
            nipData.Nip = nip;

            try
            {
                string daneSzukajResponse = GusServices.DaneSzukajPodmioty(nipData);

                using (var reader = new StringReader(daneSzukajResponse))
                {
                    XmlRootAttribute xRoot = new XmlRootAttribute();
                    xRoot.ElementName = "root";

                    XmlSerializer daneSzukajSerializer = new XmlSerializer(typeof(DaneGus), xRoot);
                    var daneGus = (DaneGus) daneSzukajSerializer.Deserialize(reader);

                    return daneGus.dane;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private void LoginIfRequired()
        {
            if (GusServices.GetValue("StatusSesji") == "0") Login();
        }

        private void Login()
        {
            sessionId = GusServices.Zaloguj(apiKey);

            OperationContextScope scope = new OperationContextScope(GusServices.InnerChannel);

            HttpRequestMessageProperty requestProperties = new HttpRequestMessageProperty();
            requestProperties.Headers.Add("sid", sessionId);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestProperties;

        }

        public void Logout()
        {
            GusServices.Wyloguj(sessionId);
        }

        private void SetupBinding()
        {
            var encoding = new MtomMessageEncoderBindingElement(new TextMessageEncodingBindingElement());
            var transport = new HttpsTransportBindingElement();

            var customBinding = new CustomBinding(encoding, transport);

            GusServices.Endpoint.Binding = customBinding;
        }
    }
}
