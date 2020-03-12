using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunicationUI.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public ErrorModel()
        {
        }

        public string Message { get; private set; }

        public void OnGet(string msg)
        {
            Message = msg;
        }
    }
}
