
using ClaimManagement.Helper;
using ClamManagement.Repo;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.ComponentModel;
using System.Diagnostics;
using ClaimManagement.Data.Entities;
using ClaimManagement.Model;
namespace ClaimManagement.Data.Services
{
    public interface IClaimService
    {
        Task<object> getfile(IFormFile file);
        Dictionary<string,string>? ExcelSheetMapper(ClaimEcxalSheetMappingModel Model);
        Dictionary<string,string>? GetExcelSheetMapper();
    }
    public class ClaimService : IClaimService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        public ClaimService(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public Dictionary<string, string>? ExcelSheetMapper(ClaimEcxalSheetMappingModel Model)
        {
            var currentDir = Directory.GetCurrentDirectory();
            currentDir = Path.Combine(currentDir, "Helper\\ExcelSheetMapper.json");

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(currentDir));
            var OldMappedKeys = dictionary.Keys;
            foreach (var item in Model.GetType().GetProperties() )
            {
                var strinsgValue = item.GetValue(Model);

                var stringValue = strinsgValue?.ToString();

                if (string.IsNullOrEmpty(stringValue) || OldMappedKeys.Contains(stringValue))
                {
                    continue;
                }

                dictionary.Add(stringValue , item.Name);
            }
            File.WriteAllText(currentDir, JsonConvert.SerializeObject(dictionary));

            return dictionary;
        }

        public Dictionary<string, string>? GetExcelSheetMapper()
        {
            var currentDir = Directory.GetCurrentDirectory();
            currentDir = Path.Combine(currentDir, "Helper\\ExcelSheetMapper.json");

            var jsonString = File.ReadAllText(currentDir);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            return dictionary;
        }
        public async Task<object> getfile(IFormFile file)
        {
           return await ReadExcelFile(file);
        }
        private async Task<object>? ReadExcelFile(IFormFile file)
        {

            if (file == null)
                throw new Exception("File is Not Received...");


            // Create the Directory if it is not exist
            string dirPath = Path.Combine(_webHostEnvironment.ContentRootPath, "ReceivedClaims");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Make sure that only Excel file is used 
            string dataFileName = Path.GetFileName(file.FileName);

            string extension = Path.GetExtension(dataFileName);

            string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

            if (!allowedExtsnions.Contains(extension))
                throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.xls or.xlsx is uploaded.");

            // Make a Copy of the Posted File from the Received HTTP Request
            string saveToPath = Path.Combine(dirPath, dataFileName);
            using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            object funds;
            using (var stream = new FileStream(saveToPath, FileMode.Open))
            {
                funds = await ImportFromXlsFile(stream);
            }

            // USe this to handle Encodeing differences in .NET Core
            // read the excel file
           
            return funds;
        }
        private async Task<IList<PropertyByName<T>>> GetPropertiesByExcelCells<T>(ExcelWorksheet worksheet)
        {
            var test = GetExcelSheetMapper();
            var properties = new List<PropertyByName<T>>();
            var poz = 1;
            while (true)
            {
                try
                {
                    var cell = worksheet.Cells[1, poz];

                    if (cell == null || cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                        break;

                    poz += 1;
                    properties.Add(new PropertyByName<T>(test[cell?.Value?.ToString()]));
                }
                catch
                {
                    break;
                }
            }

            return properties;
        }
        private async Task<object> ImportFromXlsFile(Stream stream)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {

                var worksheet = xlPackage.Workbook.Worksheets[0];
                if (worksheet == null)
                    throw new Exception("No worksheet found");

                var importedClaims = new List<Claim>();

                //the columns
                var properties = await GetPropertiesByExcelCells<Claim>(worksheet);

                var manager = new PropertyManager<Claim>(properties);

                var iRow = 2;
                bool isValidRow;
                var ecxalPropetyMapper = GetExcelSheetMapper();
                while (true)
                {
                    var allColumnsAreEmpty = manager.GetProperties
                        .Select(property => worksheet.Cells[iRow, property.PropertyOrderPosition])
                        .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                    if (allColumnsAreEmpty)
                        break;

                    manager.ReadFromXlsx(worksheet, iRow);

                    isValidRow = true;
                    var importedClaim = new Claim();

                    foreach (var property in manager.GetProperties)
                    {
                        switch (property.PropertyName.Trim())
                        {
                            case "Id":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.Id = property.IntValue;
                                break;

                            case "ClaimNumber":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.ClaimNumber = property.StringValue.Trim();
                                break;
                            case "ClaimDate":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.ClaimDate = DateTime.Parse(property.StringValue);
                                break;
                            case "Notes":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.Notes = property.StringValue.Trim();
                                break;

                       
                        }
                    }

                    if (isValidRow)
                    {
                        importedClaims.Add(importedClaim);
                    }
                    else
                    {
                        Debug.WriteLine(iRow);
                    }
                    iRow++;

                }

                return importedClaims;
            }
        }

    }
}
