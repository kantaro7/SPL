using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SPL.Configuration.Api.DTOs.Filters
{
    /// <summary>
    /// Representa un filtro que valida un DTO enviado como parametro usando sus data annotations
    /// </summary>
    public sealed class ValidateDTO : TypeFilterAttribute
    {
        #region Ctor

        public ValidateDTO() : base(typeof(ValidateDTOFilter))
        {
        }

        #endregion

        #region Nested class

        /// <summary>
        /// Representa un filtro que valida un DTO enviado como parametro usando sus data annotations
        /// </summary>
        private class ValidateDTOFilter : IAsyncActionFilter
        {
            #region Methods

            /// <summary>
            /// Called asynchronously before the action, after model binding is complete.
            /// </summary>
            /// <param name="context">A context for action filters</param>
            /// <param name="next">A delegate invoked to execute the next action filter or the action itself</param>
            /// <returns>
            /// A task that represents the asynchronous operation
            /// </returns>
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!context.ModelState.IsValid)
                {
                    var modelErrors = new List<string>();
                    foreach (var modelState in context.ModelState.Values)
                        foreach (var modelError in modelState.Errors)
                            modelErrors.Add(modelError.ErrorMessage);

                    var formatedErrors = string.Join("\n", modelErrors);
                    context.Result = new UnprocessableEntityObjectResult(formatedErrors);
                }
                    
                
                if (context.Result == null)
                    await next();
            }

            #endregion
        }

        #endregion
    }
}