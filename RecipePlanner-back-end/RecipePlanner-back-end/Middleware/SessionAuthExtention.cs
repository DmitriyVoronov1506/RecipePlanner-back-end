using Microsoft.AspNetCore.Builder;

namespace RecipePlanner_back_end.Middleware
{
    public static class SessionAuthExtention
    {
        public static IApplicationBuilder UseSessionAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
