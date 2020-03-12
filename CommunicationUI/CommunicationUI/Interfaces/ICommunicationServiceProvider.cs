using System;
using System.Threading.Tasks;

namespace CommunicationUI.Interfaces
{
    public interface ICommunicationServiceProvider
    {
        Task<DateTime> GetDateTime();
    }
}
