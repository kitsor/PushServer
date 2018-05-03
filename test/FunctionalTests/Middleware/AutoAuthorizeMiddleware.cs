namespace FunctionalTests.Middleware
{
	using Microsoft.AspNetCore.Http;
	using System.Security.Claims;
	using System.Threading.Tasks;

	public class AutoAuthorizeMiddleware
	{
		private readonly RequestDelegate _next;
		public AutoAuthorizeMiddleware(RequestDelegate rd)
		{
			_next = rd;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			var identity = new ClaimsIdentity("cookies");
			identity.AddClaim(new Claim("sub", "3296643c-4681-43a9-a309-47260df37ffe"));
			httpContext.User.AddIdentity(identity);
			await _next.Invoke(httpContext);
		}
	}
}
