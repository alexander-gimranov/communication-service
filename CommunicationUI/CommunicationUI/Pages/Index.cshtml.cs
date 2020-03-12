using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationUI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CommunicationUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICommunicationServiceProvider _communicationServiceProvider;

        public IndexModel(
            ILogger<IndexModel> logger
            , ICommunicationServiceProvider communicationServiceProvider
        )
        {
            _logger = logger;
            _communicationServiceProvider = communicationServiceProvider;
        }

        public DateTime ServiceDateTime { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ServiceDateTime = await _communicationServiceProvider.GetDateTime();
                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error", new { msg = ex.Message});
            }
        }
    }
}
