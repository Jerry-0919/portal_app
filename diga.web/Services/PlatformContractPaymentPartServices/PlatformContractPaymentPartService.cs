using diga.bl.Constants;
using diga.dal.DbServices.PlatformContractChangeDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractPaymentPartDbServices;
using diga.dal.DbServices.PlatformNotificationDbServices;
using diga.web.Models.PlatformContractChanges;
using diga.web.Models.PlatformContractPaymentRequests;
using diga.web.Models.Status;
using diga.web.Services.PlatformNotificationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractPaymentPartServices
{
    public class PlatformContractPaymentPartService : IPlatformContractPaymentPartService
    {
        private readonly IPlatformContractPaymentPartDbService _platformContractPaymentPartDbService;
        private readonly IPlatformContractChangeDbService _platformContractChangeDbService;
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformNotificationService _platformNotificationService;

        public PlatformContractPaymentPartService(IPlatformContractPaymentPartDbService platformContractPaymentPartDbService, 
            IPlatformContractDbService platformContractDbService,
            IPlatformContractChangeDbService platformContractChangeDbService,
            IPlatformNotificationService platformNotificationService)
        {
            _platformContractPaymentPartDbService = platformContractPaymentPartDbService;
            _platformContractDbService = platformContractDbService;
            _platformContractChangeDbService = platformContractChangeDbService;
            _platformNotificationService = platformNotificationService;
        }

        public async Task<ResponseData> EditInvoice(int userId, int contractId, int paymentPartId, PlatformContractPaymentRequestInvoiceEditDto request)
        {
            if (await _platformContractDbService.IsExecutor(contractId, userId) == false)
            {
                return ResponseData.Fail("User", "Only executor can upload invoice");
            }

            var contract = await _platformContractDbService.Get(contractId);
            var paymentPart = await _platformContractPaymentPartDbService.Get(paymentPartId);
            if (request.InvoiceFile != paymentPart.InvoiceFile)
            {
                await _platformContractChangeDbService.Add(new bl.Models.Platform.Contracts.PlatformContractChange
                {
                    ApplicationUserId = userId,
                    Case = PlatformContractChangeCases.PaymentPartInvoiceUploaded,
                    DateTime = DateTime.Now,
                    From = paymentPart.InvoiceFileName,
                    To = request.InvoiceFileName,
                    PlatformContractId = contractId
                });

                paymentPart.InvoiceFile = request.InvoiceFile;
                paymentPart.InvoiceFileName = request.InvoiceFileName;

                await _platformNotificationService.AddNotification(contract.UserId, new Models.PlatformNotifications.PlatformNotificationDto {
                    DateTime = DateTime.Now,
                    Type = PlatformNotificationTypes.InvoiceUploaded,
                    IsRead = false,
                    Datas = new List<Models.PlatformNotifications.PlatformNotificationDataDto>()
                    {
                        new Models.PlatformNotifications.PlatformNotificationDataDto{
                            Key = "contractId",
                            Value = contract.Id.ToString()
                        },
                        new Models.PlatformNotifications.PlatformNotificationDataDto{
                            Key = "contractName",
                            Value = contract.Name
                        },
                        new Models.PlatformNotifications.PlatformNotificationDataDto{
                            Key = "invoiceFile",
                            Value = request.InvoiceFile
                        },
                        new Models.PlatformNotifications.PlatformNotificationDataDto{
                            Key = "invoiceFileName",
                            Value = request.InvoiceFileName
                        }
                    }
                });
            }
            await _platformContractPaymentPartDbService.Update(paymentPart);
            return ResponseData.Ok();
        }

        public async Task<ResponseData> EditPaid(int userId, int contractId, int paymentPartId, PlatformContractPaymentRequestPaidEditDto request)
        {
            if (await _platformContractDbService.IsExecutor(contractId, userId) == false)
            {
                return ResponseData.Fail("User", "Only executor can change payment part as paid");
            }

            var contract = await _platformContractDbService.Get(contractId);
            var paymentPart = await _platformContractPaymentPartDbService.Get(paymentPartId);
            if (request.IsPaid != paymentPart.IsPaid)
            {
                await _platformContractChangeDbService.Add(new bl.Models.Platform.Contracts.PlatformContractChange
                {
                    ApplicationUserId = userId,
                    Case = PlatformContractChangeCases.PaymentPartInvoiceUploaded,
                    DateTime = DateTime.Now,
                    From = paymentPart.IsPaid.ToString(),
                    To = request.IsPaid.ToString(),
                    PlatformContractId = contractId
                });

                paymentPart.IsPaid = request.IsPaid;
            }
            await _platformContractPaymentPartDbService.Update(paymentPart);
            return ResponseData.Ok();
        }

        public async Task<ResponseData> EditProof(int userId, int contractId, int paymentPartId, PlatformContractPaymentRequestProofEditDto request)
        {
            if (await _platformContractDbService.IsCustomer(contractId, userId) == false)
            {
                return ResponseData.Fail("User", "Only customer can upload proof");
            }

            var contract = await _platformContractDbService.Get(contractId);
            var executor = await _platformContractDbService.GetExecutor(contractId);
            var paymentPart = await _platformContractPaymentPartDbService.Get(paymentPartId);
            if (request.ProofFile != paymentPart.ProofFile)
            {
                await _platformContractChangeDbService.Add(new bl.Models.Platform.Contracts.PlatformContractChange
                {
                    ApplicationUserId = userId,
                    Case = PlatformContractChangeCases.PaymentPartProofUploaded,
                    DateTime = DateTime.Now,
                    From = paymentPart.ProofFileName,
                    To = request.ProofFileName,
                    PlatformContractId = contractId
                });

                paymentPart.ProofFile = request.ProofFile;
                paymentPart.ProofFileName = request.ProofFileName;
                paymentPart.IsMade = true;

                await _platformNotificationService.AddNotification(executor.Id, new Models.PlatformNotifications.PlatformNotificationDto
                {
                    DateTime = DateTime.Now,
                    Type = PlatformNotificationTypes.ProofUploaded,
                    IsRead = false,
                    Datas = new List<Models.PlatformNotifications.PlatformNotificationDataDto>()
                    {
                        new Models.PlatformNotifications.PlatformNotificationDataDto{
                            Key = "contractId",
                            Value = contract.Id.ToString()
                        },
                        new Models.PlatformNotifications.PlatformNotificationDataDto{
                            Key = "contractName",
                            Value = contract.Name
                        },
                        new Models.PlatformNotifications.PlatformNotificationDataDto{
                            Key = "proofFile",
                            Value = request.ProofFile
                        },
                        new Models.PlatformNotifications.PlatformNotificationDataDto{
                            Key = "proofFileName",
                            Value = request.ProofFileName
                        }
                    }
                });
            }
            await _platformContractPaymentPartDbService.Update(paymentPart);
            return ResponseData.Ok();
        }

        public async Task<List<PlatformContractPaymentPartDto>> List(int userId, int contractId)
        {
            if (await _platformContractDbService.IsCustomerOrExecutor(contractId, userId) == false)
            {
                throw new Exception("Only customer or executor can view payment parts");
            }

            return (await _platformContractPaymentPartDbService.List(contractId)).OrderBy(pp => pp.Number).Select(pp => new PlatformContractPaymentPartDto {
                Id = pp.Id,
                DateTime = pp.DateTime,
                Name = pp.Name,
                Number = pp.Number,
                Percent = pp.Percent,
                PercentOfWork = pp.PercentOfWork,
                Value = pp.Value,

                InvoiceFile = pp.InvoiceFile,
                InvoiceFileName = pp.InvoiceFileName,
                ProofFile = pp.ProofFile,
                ProofFileName = pp.ProofFileName,
                IsPaid = pp.IsPaid,
                IsMade = pp.IsMade
            }).ToList();
        }
    }
}
