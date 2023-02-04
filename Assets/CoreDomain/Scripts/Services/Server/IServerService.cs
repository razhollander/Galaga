using System.Threading.Tasks;
using Services.Server.Data;

namespace Services.Server
{
    public interface IServerService
    {
        Task<ServerResponse> Send(ServerRequest serverRequest);

        string HostAddress { set; }
        
        string AuthorizationToken { set; }
    }
}