using System.Net;

namespace CraftShop.API.Models
{
    public class APIRespone
    {
        public APIRespone()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode Status { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }    
    }
}
