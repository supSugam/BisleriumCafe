using iText.Html2pdf;
using BisleriumCafe.Helpers;

namespace BisleriumCafe.Services
{
    public class PdfService()
    {

        public async Task<bool> GeneratePdfFromHtmlContent(string htmlContent, string fileName)
        {
            try
            {
                // Get the full file path for the PDF
                string pdfFilePath = Path.Combine(Explorer.GetDocumentsDirectoryPath(), fileName + ".pdf");

                if(!File.Exists(pdfFilePath))
                {
                    File.Create(pdfFilePath);
                }

                // Initialize ConverterProperties

                // Uncomment the following lines if you want to set additional properties
                // For example, setting a base URI for relative paths in the HTML
                // converterProperties.SetBaseUri(...

                using FileStream pdfDest = File.Open(pdfFilePath, FileMode.Create);
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(htmlContent, pdfDest, converterProperties);

                return true; // PDF generation successful
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the error)
                return false;
            }
        }

    }
}



