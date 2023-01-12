
using RecipePlanner_back_end.Contexts;
using RecipePlanner_back_end.Models.Users;
using RecipePlanner_back_end.Services;

namespace RecipePlanner_back_end.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate next;

        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService, UserdbContext _userdbContext)
        {
            string userId = context.Session.GetString("userId")!;

            if (userId != null)
            {
                authService.Set(userId);

                long authMoment = Convert.ToInt64(context.Session.GetString("AuthMoment"));
                long authInterval = (DateTime.Now.Ticks - authMoment) / (long)1e7;

                if (authInterval > 10000)
                {
                    context.Session.Remove("userId");
                    context.Session.Remove("AuthMoment");

                    return;
                }
            }

            await next(context);
        }
    }
}
