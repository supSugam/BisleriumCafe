using System.IO;
using System.Threading.Tasks;
using iText.Html2pdf;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BisleriumCafe.Services
{
    public class PdfService(IJSRuntime jsRuntime)
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        public async Task GeneratePdfFromComponentAsync(ComponentBase component, string filePath)
        {
            var htmlContent = await _jsRuntime.InvokeAsync<string>("blazorSaveHtml.getHtml", component);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                HtmlConverter.ConvertToPdf(htmlContent, stream);
            }
        }
    }
}



