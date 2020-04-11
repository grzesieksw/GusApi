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
    public class ObslugaGus : IObslugaGus
    {
        public string ApiKey { get; set; }

        private readonly UslugaBIRzewnPublClient _gusServices;
        private string _sessionId;

        public ObslugaGus()
        {
            _gusServices = new UslugaBIRzewnPublClient();
            SetupBinding();
        }

        public PodmiotGus PobierzDanePodmiotu(string nip)
        {
            LoginIfRequired();

            ParametryWyszukiwania nipData = new ParametryWyszukiwania();
            nipData.Nip = nip;

            try
            {
                string daneSzukajResponse = _gusServices.DaneSzukajPodmioty(nipData);

                using (var reader = new StringReader(daneSzukajResponse))
                {
                    XmlRootAttribute xRoot = new XmlRootAttribute();
                    xRoot.ElementName = "root";

                    XmlSerializer daneSzukajSerializer = new XmlSerializer(typeof(DaneGus), xRoot);
                    var daneGus = (DaneGus)daneSzukajSerializer.Deserialize(reader);

                    return daneGus.dane;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private void LoginIfRequired()
        {
            if (_gusServices.GetValue("StatusSesji") == "0") Login();
        }

        private void Login()
        {
            _sessionId = _gusServices.Zaloguj(ApiKey);

            OperationContextScope scope = new OperationContextScope(_gusServices.InnerChannel);

            HttpRequestMessageProperty requestProperties = new HttpRequestMessageProperty();
            requestProperties.Headers.Add("sid", _sessionId);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestProperties;

        }

        public void Logout()
        {
            _gusServices.Wyloguj(_sessionId);
        }

        private void SetupBinding()
        {
            var encoding = new MtomMessageEncoderBindingElement(new TextMessageEncodingBindingElement());
            var transport = new HttpsTransportBindingElement();

            var customBinding = new CustomBinding(encoding, transport);

            _gusServices.Endpoint.Binding = customBinding;
        }
    }
}
