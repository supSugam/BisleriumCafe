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
                string repoDirectory = Explorer.GetWarehouseDirectoryPath();
                string salesReportsDirectory = Path.Combine(repoDirectory, "SalesReports");

                // Check if the "SalesReports" directory exists, create it if not
                if (!Directory.Exists(salesReportsDirectory))
                {
                    Directory.CreateDirectory(salesReportsDirectory);
                }

                string pdfFilePath = Path.Combine(salesReportsDirectory, fileName + ".pdf");

                // Initialize ConverterProperties
                using (FileStream pdfDest = File.Open(pdfFilePath, FileMode.CreateNew))
                {
                    ConverterProperties converterProperties = new ConverterProperties();
                    HtmlConverter.ConvertToPdf(htmlContent, pdfDest, converterProperties);
                }

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



