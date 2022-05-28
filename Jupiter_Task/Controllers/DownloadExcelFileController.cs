using Jupiter_Task.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Jupiter_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadExcelFileController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DownloadExcelFileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult DownloadFile()
        {
            // getting emp data 
            var employees = _context.Employees.ToList();

            // to get rid of the LicenseException
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // creating object of type ExcelPackage
            ExcelPackage package = new ExcelPackage();

            // adding new excle worksheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Excel Report");
            
            //writing columns names in the worksheet
            worksheet.Cells["A1"].Value = "Id";
            worksheet.Cells["B1"].Value = "First name";
            worksheet.Cells["C1"].Value = "Last name";
            worksheet.Cells["D1"].Value = "Hiring date";

            // specifiying the starting row
            int rowToStart = 2;

            //seeding data into the sheet
            foreach (var item in employees)
            {
                worksheet.Cells[string.Format("A{0}", rowToStart)].Value = item.Id;
                worksheet.Cells[string.Format("B{0}", rowToStart)].Value = item.FirstName;
                worksheet.Cells[string.Format("C{0}", rowToStart)].Value = item.LastName;
                worksheet.Cells[string.Format("D{0}", rowToStart)].Value = item.HiringDate;
                // format the date as MM-dd-yyyy
                worksheet.Cells[string.Format("D{0}", rowToStart)].Style.Numberformat.Format = "MM-dd-yyyy";
                rowToStart++;
            }

            //adjusting colums to fit the data
            worksheet.Cells["A:AZ"].AutoFitColumns();
           
            // gets the file contents as byte array
            var fileContents = package.GetAsByteArray();

            return File(fileContents, "application/ms-excel", "Report.xlsx");          
        }
    }
}
