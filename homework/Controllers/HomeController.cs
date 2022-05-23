using homework.entityFramework;
using homework.Models;
using homework.Static.EPPlus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace homework.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Context _context;
        IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger )
        {
            _logger = logger;
            _context = new Context();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<IActionResult> Import()
        {

            var servType = _context.ServType.ToList();
            var file = Request.Form.Files[0];
            List<FileModel> resFileRows = new List<FileModel>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream).ConfigureAwait(false);
                //data = stream.ToArray();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.First();

                    IEnumerable<FileViewModel> collection = workSheet.ConvertSheetToObjects<FileViewModel>();
                    foreach(var item in collection)
                    {
                        resFileRows.Add(
                            new FileModel
                            {
                                Date= new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.Time.Hour, item.Time.Minute, item.Time.Second),
                                Number= ParsePhone(item.Phone),
                                TypeId= servType.Where(x=>x.Name == item.Type).Select(x=>x.Id).FirstOrDefault()
                            });
                    }
                }
            }
            _context.File.AddRange(resFileRows);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public void  OutInExcel()
        {
            var result = _context.get_statistics().ToList();
            // Создаём объект - экземпляр нашего приложения
            Excel.Application excelApp = new();
            // Создаём экземпляр рабочей книги Excel
            Excel.Workbook workBook;
            // Создаём экземпляр листа Excel
            Excel.Worksheet workSheet;
            workBook = excelApp.Workbooks.Add();
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);
            // Заполняем первый столбец листа из массива Y[0..n-1]
            //for (int j = 1; j <= n; j++)
            //    workSheet.Cells[j, 1] = Y[j - 1];
            
          
            for(int i = 0; i< result.Count;i++)
            {
                workSheet.Cells[i+1, 1] = result[i].Phone;
                workSheet.Cells[i+1, 2] = result[i].DateCall;
                workSheet.Cells[i+1, 3] = result[i].DateDifference;
            }
           

            // Открываем созданный excel-файл
            excelApp.Visible = true;
            excelApp.UserControl = true;
        }
        private string ParsePhone(string number)
        {
            number = number.Trim();
            number = number.Replace("(","").Replace(")", "").Replace("+", "");
            switch(number[0])
            {
                case '7':
                    return number.Substring(1, number.Length - 1);
                case '8':
                    return number.Substring(1, number.Length - 1);
                case '9':
                    return number;
            }

            return number;
        }
       

    }
}
