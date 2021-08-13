using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.PlatformContractApprovalDbServices;
using diga.dal.DbServices.PlatformContractChangeDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.web.Models.PlatformContractSigning;
using diga.web.Models.Status;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractSigningServices
{
    public class PlatformContractSigningService : IPlatformContractSigningService
    {
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformContractChangeDbService _platformContractChangeDbService;
        private readonly IPlatformContractApprovalDbService _platformContractApprovalDbService;
        private readonly IWebHostEnvironment _environment;

        public PlatformContractSigningService(IPlatformContractDbService platformContractDbService,
            IPlatformContractChangeDbService platformContractChangeDbService,
            IPlatformContractApprovalDbService platformContractApprovalDbService,
            IWebHostEnvironment environment)
        {
            _platformContractDbService = platformContractDbService;
            _platformContractChangeDbService = platformContractChangeDbService;
            _platformContractApprovalDbService = platformContractApprovalDbService;
            _environment = environment;
        }

        public async Task<ResponseData> Approve(int userId, int contractId)
        {
            bool isCustomer = await _platformContractDbService.IsCustomer(contractId, userId);
            bool isExecutor = await _platformContractDbService.IsExecutor(contractId, userId);
            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.Status != PlatformContractStatuses.Signing)
                return ResponseData.Fail("Status", "Contract can be signed only after contract approval!");

            if (!isCustomer && !isExecutor)
                return ResponseData.Fail("User", "Access denied!");

            PlatformContractApproval approval = await _platformContractApprovalDbService.Get(contractId);

            if (isCustomer && !approval.SigningCustomer)
            {
                await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId,
                    PlatformContractChangeCases.ApproveSigning, false.ToString(), true.ToString(), DateTime.UtcNow));

                approval.SigningCustomer = true;
                await _platformContractApprovalDbService.Update(approval);
            }
            else if (isExecutor && !approval.SigningExecutor)
            {
                await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId,
                    PlatformContractChangeCases.ApproveSigning, false.ToString(), true.ToString(), DateTime.UtcNow));

                approval.SigningExecutor = true;
                await _platformContractApprovalDbService.Update(approval);
            }

            if(approval.SigningCustomer && approval.SigningExecutor)
            {
                contract.Status = PlatformContractStatuses.Executing;
                await _platformContractDbService.Update(contract);
            }    

            return ResponseData.Ok();
        }

        public async Task<ResponseData> Edit(int userId, int contractId, PlatformContractSigningDto request)
        {
            bool isCustomer = await _platformContractDbService.IsCustomer(contractId, userId);
            bool isExecutor = await _platformContractDbService.IsExecutor(contractId, userId);

            if (!isCustomer && !isExecutor)
                return ResponseData.Fail("User", "Access denied!");
            else if (string.IsNullOrEmpty(request.Text))
                return ResponseData.Fail("Text", "Text cannot be empty!");

            var contract = await _platformContractDbService.Get(contractId);
            if (isExecutor /*&& contract.PaymentType == PlatformContractPaymentTypes.Safe*/ && contract.IsPrepayment == true && contract.PrepaymentValue > 0)
            {
                if (string.IsNullOrEmpty(request.PrepaymentInvoiceFile) || string.IsNullOrEmpty(request.PrepaymentInvoiceFileName))
                {
                    return ResponseData.Fail("Invoice", "Upload prepayment invoice");
                }
                
                string temp = Path.Combine(_environment.WebRootPath, "docs", "temp", request.PrepaymentInvoiceFile);
                string src = Path.Combine(_environment.WebRootPath, "docs", "src", $"{request.PrepaymentInvoiceFile}");

                if (File.Exists(temp))
                {
                    File.Move(temp, src);
                }

                contract.PrepaymentInvoiceFile = request.PrepaymentInvoiceFile;
                contract.PrepaymentInvoiceFileName = request.PrepaymentInvoiceFileName;
                await _platformContractDbService.Update(contract);
            }

            PlatformContractApproval approval = await _platformContractApprovalDbService.Get(contractId);

            await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId,
                PlatformContractChangeCases.EditSigning, approval.CustomContractText, request.Text, DateTime.UtcNow));

            approval.CustomContractText = request.Text;

            if (isCustomer)
                approval.SigningExecutor = false;
            else if (isExecutor)
                approval.SigningCustomer = false;

            await _platformContractApprovalDbService.Update(approval);

            return ResponseData.Ok();
        }

        public async Task<PlatformContractSigningDto> Get(int contractId)
        {
            PlatformContractApproval approval = await _platformContractApprovalDbService.Get(contractId);
            var contract = await _platformContractDbService.Get(contractId);

            if (string.IsNullOrEmpty(approval.CustomContractText))
            {
                return new PlatformContractSigningDto
                {
                    Text = await File.ReadAllTextAsync($"{_environment.WebRootPath}/docs/default/contract.txt")
                };
            }

            return new PlatformContractSigningDto
            {
                Text = approval.CustomContractText,
                PrepaymentInvoiceFile = contract.PrepaymentInvoiceFile,
                PrepaymentInvoiceFileName = contract.PrepaymentInvoiceFileName
            };
        }
    }
}
