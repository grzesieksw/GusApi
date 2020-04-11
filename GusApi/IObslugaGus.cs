using GusApi.Models;

namespace GusApi
{
    public interface IObslugaGus
    {
        string ApiKey { get; set; }

        PodmiotGus PobierzDanePodmiotu(string nip);
        void Logout();
    }
}