using OfficeOpenXml;
using StaffMgtBlazorApp.Models;
using iTextSharp.text.pdf;
namespace StaffMgtBlazorApp.Services
{
    public class ExportService
    {
        public ExportService()
        {
            // Ensure the EPPlus license context is set
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<byte[]> ExportToExcelAsync(IEnumerable<StaffDto> staffs)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Staffs");

            worksheet.Cells[1, 1].Value = "Staff ID";
            worksheet.Cells[1, 2].Value = "Full Name";
            worksheet.Cells[1, 3].Value = "Birthday";
            worksheet.Cells[1, 4].Value = "Gender";

            var row = 2;
            foreach (var staff in staffs)
            {
                worksheet.Cells[row, 1].Value = staff.StaffId;
                worksheet.Cells[row, 2].Value = staff.FullName;
                worksheet.Cells[row, 3].Value = staff.Birthday.ToShortDateString();
                worksheet.Cells[row, 4].Value = staff.Gender == 1 ? "Male" : "Female";
                row++;
            }

            return await package.GetAsByteArrayAsync();
        }
        public async Task<byte[]> ExportToPdfAsync(IEnumerable<StaffDto> staffs)
        {
            using var memoryStream = new MemoryStream();
            var document = new iTextSharp.text.Document();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            var table = new PdfPTable(4);
            table.AddCell("Staff ID");
            table.AddCell("Full Name");
            table.AddCell("Birthday");
            table.AddCell("Gender");

            foreach (var staff in staffs)
            {
                table.AddCell(staff.StaffId);
                table.AddCell(staff.FullName);
                table.AddCell(staff.Birthday.ToShortDateString());
                table.AddCell(staff.Gender == 1 ? "Male" : "Female");
            }

            document.Add(table);
            document.Close();

            return memoryStream.ToArray();
        }
    }
}
