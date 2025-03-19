using MediatR;

namespace IMS.Business.Handlers;

public class LogoutQueryHandler : IRequestHandler<LogoutQuery, bool>
{
    public async Task<bool> Handle(LogoutQuery request, CancellationToken cancellationToken)
    {
        // TODO: Remove refresh token instead
        if (File.Exists("token.txt"))
        {
            File.Delete("token.txt");
            return true;
        }
        
        return false;
    }
}
