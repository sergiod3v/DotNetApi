using Microsoft.AspNetCore.Mvc.Filters;

namespace NZWalks.API.Filters {
    public class LoggingFilter : IAsyncActionFilter {

        private readonly string _callerName;

        public LoggingFilter(string callerName) {
            _callerName = callerName;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        ) {
            Console.WriteLine("Before Action");
            await next();
            Console.WriteLine("After Action");
            Console.WriteLine($"{_callerName} executed: {context.ActionDescriptor}.");
        }
    }
}