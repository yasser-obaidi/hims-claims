
using ClaimManagement.Helper;
using Newtonsoft.Json;
using OfficeOpenXml;
using ClaimManagement.Data.Entities;
using ClaimManagement.Model;
using ClaimManagement.Enums;
using ClamManagement.Data;
using ClamManagement.Helper;
using Microsoft.EntityFrameworkCore;
using ZstdSharp.Unsafe;
namespace ClaimManagement.Services
{
    public interface IClaimService
    {
        Task ImportClaimsFromExcelFile(IFormFile file);
        Dictionary<string,string>? ExcelSheetMapper(ClaimEcxalSheetMappingModel Model);
        Dictionary<string,string>? GetExcelSheetMapper();
        Task<ClaimOutputModelDetailed?> GetClaimByIdAsync(int id);
        Task<List<ClaimOutputModelSimple>> GetClaimsAsync();
    }
    public class ClaimService : IClaimService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClaimService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

                var stringValue = strinsgValue?.ToString().ToLower();

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
        public async Task ImportClaimsFromExcelFile(IFormFile file)
        {
            var importedClaims = (await ReadExcelFile(file));
            var NetworkProviders = await GetNetworkProvidersByClaims(importedClaims);
            var claimGroupedByNetworkProvider = importedClaims.GroupBy(x => x.NetworkProviderName);

            foreach (var claimGrop in claimGroupedByNetworkProvider)
            {
               var networkProviderId = NetworkProviders.Where(x=>x.Name == claimGrop.Key).SingleOrDefault()?.Id;
               if (networkProviderId != null)
                {
                    foreach (var item in claimGrop)
                    {
                        item.NetworkProviderId = (int)networkProviderId;
                    }
                }
            }
            var claimGroupedByTPAs = claimGroupedByNetworkProvider.SelectMany(x=>x).GroupBy(x=>x.TPAName);
            var TPAs = await GetTPAsByClaims(importedClaims);

            foreach (var claimGrop in claimGroupedByTPAs)
            {
                var TPAId = TPAs.Where(x => x.Name == claimGrop.Key).SingleOrDefault()?.Id;
                if (TPAId != null)
                {
                    foreach (var item in claimGrop)
                    {
                        item.TPAId = (int)TPAId;
                    }
                }
            }

            var policies = await _unitOfWork.PolicyManagement.GetPolicyByClaims(importedClaims);
            var claimGroupedByPolicy = claimGroupedByTPAs.SelectMany(x => x).GroupBy(x => x.PolicyName);
            foreach (var claimGrop in claimGroupedByPolicy)
            {
                var PolicyId = policies.Where(x => x.Name == claimGrop.Key).SingleOrDefault()?.Id;
                if (PolicyId != null)
                {
                    foreach (var item in claimGrop)
                    {
                        item.NetworkProviderId = (int)PolicyId;
                    }
                }
            }
            var claims = claimGroupedByPolicy.SelectMany(x=>x).MapTo<List<Claim>>();
            _unitOfWork.ClaimRepo.AddRange(claims);
            await _unitOfWork.SaveChangesAsync();
        }

       

        private async Task<List<TPA>> GetTPAsByClaims(List<ClaimInputModel> importedClaims)
        {


            var tpaNames = importedClaims.GroupBy(claim => claim.TPAName).Select(claimGroup=> claimGroup.Key.ToLower().Trim());
            return await (await _unitOfWork.TPARepo.FindByCondition(tpa=> tpaNames.Contains(tpa.Name.ToLower().Trim()))).ToListAsync();
        }

        private async Task<List<NetworkProvider>> GetNetworkProvidersByClaims(List<ClaimInputModel> importedClaims)
        {

            var networkProviderNames = importedClaims.GroupBy(claim => claim.NetworkProviderName).Select(claimGroup=> claimGroup.Key.ToLower().Trim());
            return await ( await _unitOfWork.NetworkProviderRepo.FindByCondition(networkProvider=> networkProviderNames.Contains(networkProvider.Name.ToLower().Trim()))).ToListAsync();
        }

        private async Task<List<ClaimInputModel>>? ReadExcelFile(IFormFile file)
        {

            if (file == null)
                throw new Exception("File is Not Received...");


            // Create the Directory if it is not exist
            string dirPath = Path.Combine(_unitOfWork.WebHostEnvironment.ContentRootPath, "ReceivedClaims");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Make sure that only Excel file is used 
            string dataFileName = Path.GetFileName(file.FileName);

            string extension = Path.GetExtension(dataFileName);

            string[] allowedFileExtsnions = new string[] { ".xls", ".xlsx" };

            if (!allowedFileExtsnions.Contains(extension))
                throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.xls or.xlsx is uploaded.");

            // Make a Copy of the Posted File from the Received HTTP Request
            string saveToPath = Path.Combine(dirPath, dataFileName);
            using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            List<ClaimInputModel> funds;
            using (var stream = new FileStream(saveToPath, FileMode.Open))
            {
                funds = await ImportFromXlsFile(stream);
            }

           
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
                    properties.Add(new PropertyByName<T>(test[cell?.Value?.ToString().ToLower()]));
                }
                catch
                {
                    break;
                }
            }

            return properties;
        }
        private async Task<List<ClaimInputModel>> ImportFromXlsFile(Stream stream)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {

                var worksheet = xlPackage.Workbook.Worksheets[0];
                if (worksheet == null)
                    throw new Exception("No worksheet found");

                var importedClaims = new List<ClaimInputModel>();

                //the columns
                var properties = await GetPropertiesByExcelCells<ClaimInputModel>(worksheet);

                var manager = new PropertyManager<ClaimInputModel>(properties);

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
                    var importedClaim = new ClaimInputModel();

                    foreach (var property in manager.GetProperties)
                    {
                        switch (property.PropertyName.Trim())
                        {

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
                            case "TPAId":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.TPAId = property.IntValue;
                                break;
                            case "TPAClaimReferenceNumber":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.TPAClaimReferenceNumber = property.StringValue.Trim();
                                break;
                            case "NetworkProviderId":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.NetworkProviderId = property.IntValue;
                                break;
                            case "NetworkProviderInvoiceNumber":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.NetworkProviderInvoiceNumber = property.StringValue.Trim();
                                break;
                            case "ProcedureDate":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.ProcedureDate =DateTime.Parse( property.StringValue);
                                break;
                            case "ServiceCategoryId":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.ServiceCategoryId = property.IntValue;
                                break;
                            case "ServiceName":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.ServiceName = property.StringValue.Trim();
                                break;
                            case "PatientName":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.PatientName = property.StringValue.Trim();
                                break;
                            case "CardNo":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.CardNo = property.StringValue.Trim();
                                break;
                            case "DiagnosticCode":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.DiagnosticCode = property.StringValue.Trim();
                                break;

                            case "DiagnosticDescription":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.DiagnosticDescription = property.StringValue;
                                break;
                            case "AdmissionDate":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.AdmissionDate = DateTime.Parse( property.StringValue);
                                break;
                            case "DischargeDate":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.DischargeDate = DateTime.Parse( property.StringValue);
                                break;
                            case "TreatmentCountry":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.TreatmentCountry = property.StringValue.Trim();
                                break;
                            case "IsReimbursement":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.IsReimbursement =bool.Parse( property.StringValue);
                                break;

                            case "AmountClaimedOriginalCurrency":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.AmountClaimedOriginalCurrency = property.DecimalValue;
                                break;

                            case "OriginalCurrencyCode":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.OriginalCurrencyCode = property.StringValue.Trim();
                                break;

                            case "AmountClaimedPlanCurrency":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.AmountClaimedPlanCurrency = property.DecimalValue;
                                break;

                            case "PlanCurrencyCode":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.PlanCurrencyCode = property.StringValue.Trim();
                                break;

                            case "AmountApprovedOriginalCurrency":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.AmountApprovedOriginalCurrency = property.DecimalValue;
                                break;

                            case "AmountApprovedPlanCurrency":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.AmountApprovedPlanCurrency = property.DecimalValue;
                                break;

                            case "CoPaymentOriginalCurrency":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.CoPaymentOriginalCurrency = property.StringValue.Trim();
                                break;

                            case "CoPaymentPlanCurrency":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.CoPaymentPlanCurrency = property.StringValue.Trim();
                                break;

                            

                            case "PaymentMethod":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.PaymentMethod = (PaymentMethod)Enum.Parse( typeof(PaymentMethod), property.StringValue.Trim().ToUpper());
                                break;

                            case "Notes":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.Notes = property.StringValue;
                                break;

                            case "PolicyId":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.PolicyId = property.IntValue;
                                break;
                            case "PolicyName":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.PolicyName = property.StringValue;
                                break;
                            case "ServiceCategoryName":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.ServiceCategoryName = property.StringValue;
                                break;
                            case "TPAName":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.TPAName = property.StringValue;
                                break;
                            case "NetworkProviderName":
                                if (string.IsNullOrWhiteSpace(property.StringValue.Trim()))
                                {
                                    isValidRow = false;
                                    break;
                                }
                                importedClaim.NetworkProviderName = property.StringValue;
                                break;




                        }
                    }

                    
                    importedClaims.Add(importedClaim);
                    
                    iRow++;

                }

                return importedClaims;
            }
        }

        public async Task<ClaimOutputModelDetailed?> GetClaimByIdAsync(int id)
        {
            return (await _unitOfWork.ClaimRepo.FindById(id)).MapTo<ClaimOutputModelDetailed>();
        }
        private async Task<bool> ClaimsAreValid(IQueryable<Claim> claims)
        {
            var claimNumbers = claims.Select(claim => claim.ClaimNumber);
            

            if (await _unitOfWork.ClaimRepo.Any(claim => claimNumbers.Contains(claim.ClaimNumber) )) // check if claims have enters before 
            {

            }
            var TpaIds = claims.Select(x=>x.TPAId);
            if(await _unitOfWork.TPARepo.All(TPA=>TpaIds.Contains(TPA.Id)))
            {

            }
            throw new NotImplementedException();
        }
        
        public async Task<List<ClaimOutputModelSimple>> GetClaimsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
