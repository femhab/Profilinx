using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PROFILINX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public IActionResult ErrorPage(Exception ex)
        //{
        //    var route = RouteParam;
        //    Logger.Log(string.Format("{0}/{1}: ", route.Controller, route.Action) + ex.Message + "\r" + ex.Source?.ToString().Trim() + "\r" + ex.StackTrace);
        //    return StatusCode(StatusCodes.Status500InternalServerError, new
        //    {
        //        status = false,
        //        message = $"Internal Server Error. Please try again!"
        //    });
        //}

        private (string Controller, string Action) RouteParam
        {
            get
            {
                var param = ControllerContext.RouteData.Values;
                var result = (param["controller"].ToString(), param["action"].ToString());
                return result;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public BadRequestObjectResult ValidationError()
        {
            return BadRequest(new
            {
                status = false,
                message = "Invalid records. Please check for validation error",
                errors = ModelState
            });
        }

        public string BaseUrl => $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

        protected string GetPhotoUrl(string photo)
        {
            if (!string.IsNullOrEmpty(photo) && (photo.Contains("http://") || photo.Contains("https://")))
            {
                return photo;
            }
            var photoUrl = !string.IsNullOrEmpty(photo) ? string.Format("{0}/{1}", BaseUrl, photo) : photo;
            return photoUrl;
        }

        protected string AuthToken
        {
            get
            {
                string token = Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    token = null;
                }
                return token;
            }
        }
    }
}