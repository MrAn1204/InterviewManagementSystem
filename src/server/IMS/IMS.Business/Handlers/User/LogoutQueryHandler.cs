using MediatR;

namespace IMS.Business.Handlers;

public class LogoutQueryHandler : IRequestHandler<LogoutQuery, bool>
{
    public async Task<bool> Handle(LogoutQuery request, CancellationToken cancellationToken)
    {
        if (!File.Exists("token.txt"))
        {
            return false;
        }

        // TODO: Remove refresh token from database
        File.Delete("token.txt");
        File.Delete("refreshToken.txt");
        
        return true;
    }
}
