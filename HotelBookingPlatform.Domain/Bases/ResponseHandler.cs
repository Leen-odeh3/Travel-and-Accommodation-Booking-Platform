using System.Net;

namespace HotelBookingPlatform.Domain.Bases
{
    public class ResponseHandler
    {
        public Response<T> Deleted<T>(string message)
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = message ?? "The resource has been successfully deleted."
            };
        }

        public Response<T> Success<T>(T entity)
        {
            return new Response<T>
            {
                Data = entity,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Operation succeeded.",
            };
        }

        public Response<T> Unauthorized<T>(string message = null)
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message ?? "Unauthorized access."
            };
        }

        public Response<T> BadRequest<T>(string message = null)
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message ?? "Bad request."
            };
        }

        public Response<T> UnprocessableEntity<T>(string message = null)
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message ?? "Unprocessable entity."
            };
        }

        public Response<T> NotFound<T>(string message)
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message ?? "Resource not found."
            };
        }

        public Response<T> Created<T>(T entity)
        {
            return new Response<T>
            {
                Data = entity,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = "Resource created successfully.",
            };
        }
    }
}
