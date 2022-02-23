using Microsoft.AspNetCore.Mvc;

namespace MoviesAPI.APIBehavior
{
    public class BadRequestBehavior
    {
        public static void Parse(ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = actiontext =>
            {
                var response = new List<string>();
                foreach (var key in actiontext.ModelState.Keys)
                {
                    foreach (var error in actiontext.ModelState[key].Errors)
                    {
                        response.Add($"{key} : {error.ErrorMessage}");

                    }
                }
                return new BadRequestObjectResult(response);
            };
        }
    }
}
