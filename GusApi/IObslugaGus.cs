using GusApi.Models;

namespace GusApi
{
    public interface IObslugaGus
    {
        void Logout();
        PodmiotGus PobierzDanePodmiotu(string nip);
    }
}