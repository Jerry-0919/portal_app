using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.PlatformContractApprovalDbServices;
using diga.dal.DbServices.PlatformContractBidDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractReviewDbServices;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformContractReviews;
using diga.web.Models.PlatformNotifications;
using diga.web.Models.Status;
using diga.web.Services.PlatformNotificationServices;
using diga.web.Services.UserServices;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractReviewServices
{
    public class PlatformContractReviewService : IPlatformContractReviewService
    {
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformContractBidDbService _platformContractBidDbService;
        private readonly IPlatformContractApprovalDbService _platformContractApprovalDbService;
        private readonly IPlatformContractReviewDbService _platformContractReviewDbService;
        private readonly IPlatformNotificationService _platformNotificationService;
        private readonly IUserService _userService;

        private readonly IWebHostEnvironment _environment;

        public PlatformContractReviewService(IPlatformContractDbService platformContractDbService,
            IPlatformContractBidDbService platformContractBidDbService,
            IPlatformContractApprovalDbService platformContractApprovalDbService,
            IPlatformContractReviewDbService platformContractReviewDbService,
            IPlatformNotificationService platformNotificationService,
            IUserService userService,
            IWebHostEnvironment environment)
        {
            _platformContractDbService = platformContractDbService;
            _platformContractBidDbService = platformContractBidDbService;
            _platformContractApprovalDbService = platformContractApprovalDbService;
            _platformContractReviewDbService = platformContractReviewDbService;
            _platformNotificationService = platformNotificationService;
            _userService = userService;
            _environment = environment;
        }

        public async Task<ResponseData> Add(int userId, int contractId, PlatformContractReviewAddDto request)
        {
            if (!await _platformContractDbService.IsCustomerOrExecutor(contractId, userId))
                return ResponseData.Fail("User", "Access denied!");

            PlatformContractApproval approval = await _platformContractApprovalDbService.Get(contractId);
            if (!approval.FinishCustomer || !approval.FinishExecutor)
                return ResponseData.Fail("Approval", "Contract is not finished!");

            if (await _platformContractReviewDbService.Any(contractId, userId))
                return ResponseData.Fail("Review", "Review already added!");

            List<PlatformContractReviewDocument> documents = new List<PlatformContractReviewDocument> { };

            foreach (PlatformContractReviewImageDto image in request.Images)
            {
                string tempPath = Path.Combine(_environment.WebRootPath, "img", "temp", image.FileName);
                if (File.Exists(tempPath))
                {
                    File.Move(tempPath, Path.Combine(_environment.WebRootPath, "img", "src", image.FileName));
                    documents.Add(new PlatformContractReviewDocument { FileName = image.FileName });
                }
            }

            foreach (PlatformContractReviewDocumentDto document in request.Documents)
            {
                string tempPath = Path.Combine(_environment.WebRootPath, "docs", "temp", document.FileName);
                if (File.Exists(tempPath))
                {
                    File.Move(tempPath, Path.Combine(_environment.WebRootPath, "docs", "src", document.FileName));
                    documents.Add(new PlatformContractReviewDocument { FileName = document.FileName });
                }
            }

            PlatformContractReview review = new PlatformContractReview
            {
                ApplicationUserId = userId,
                PlatformContractId = contractId,
                LikeText = request.LikeText,
                SuggestionText = request.SuggestionText,
                PublishDate = DateTime.UtcNow,
                Marks = request.Marks.Select(m => new PlatformContractReviewMark
                {
                    Value = m.Value,
                    DescriptionId = m.Description,
                    TextId = m.TextId
                }).ToList(),
                Documents = documents
            };

            await _platformContractReviewDbService.Add(review);

            PlatformContract contract = await _platformContractDbService.Get(contractId);
            PlatformContractBid acceptedBid = await _platformContractBidDbService.GetAccepted(contractId);

            var applicationUser = await _userService.Get(contract.UserId == userId ? contract.UserId : acceptedBid.ApplicationUserId);

            await _platformNotificationService.AddNotification(contract.UserId == userId ? acceptedBid.ApplicationUserId : contract.UserId,
                new PlatformNotificationDto
                {
                    IsRead = false,
                    DateTime = review.PublishDate,
                    Type = PlatformNotificationTypes.AddReview,
                    Datas = new List<PlatformNotificationDataDto> {
                        new PlatformNotificationDataDto { Key = "ReviewId", Value = review.Id.ToString() },
                        new PlatformNotificationDataDto { Key = "ContractId", Value = contractId.ToString() },
                        new PlatformNotificationDataDto { Key = "ContractName", Value = contract.Name },
                        new PlatformNotificationDataDto { Key = "AuthorName", Value = applicationUser.Name },
                    }
                });

            return ResponseData.Ok();
        }

        private PlatformContractReviewDto GetReviewDto(PlatformContractReview review)
        {
            List<string> imageFormats = new List<string> { ".png", "jpeg" };

            if (review == null)
                return null;

            return new PlatformContractReviewDto
            {
                Id = review.Id,
                PublishDate = review.PublishDate,
                ContractName = review.PlatformContract.Name,
                BuildStart = review.PlatformContract.BuildStart,
                PlannedBuildEnd = review.PlatformContract.PlannedBuildEnd,
                LikeText = review.LikeText,
                SuggestionText = review.SuggestionText,
                Marks = review.Marks.Select(m => new PlatformContractReviewMarkDto
                {
                    Description = m.DescriptionId,
                    Value = m.Value,
                    TextId = m.TextId
                }).ToList(),
                Documents = review.Documents == null ? null : review.Documents.Where(d => Path.GetExtension(d.FileName) == ".pdf")
                    .Select(d => new PlatformContractReviewDocumentDto
                    {
                        FileName = d.FileName
                    }).ToList(),
                Images = review.Documents == null ? null : review.Documents.Where(d => imageFormats.Contains(Path.GetExtension(d.FileName)))
                    .Select(d => new PlatformContractReviewImageDto
                    {
                        FileName = d.FileName
                    }).ToList(),
                PlatformContractReviewCategories = review.PlatformContract.Categories.Select(c => new PlatformContractReviewCategoryDto
                {
                    Id = c.PlatformCategoryId,
                    Name = c.PlatformCategory.NameId,
                    ParentCategoryId = c.PlatformCategory.ParentCategoryId
                }).ToList(),
                Price = review.PlatformContract.Price,
                UserAvatar = review.ApplicationUser.Avatar,
                UserId = review.ApplicationUser.Id,
                UserName = review.ApplicationUser.Name
            };
        }

        public async Task<PaginatedList<PlatformContractReviewDto>> CustomerList(int userId, int skip, int take, int categoryId)
        {
            return new PaginatedList<PlatformContractReviewDto>
            {
                List = (await _platformContractReviewDbService.CustomerList(userId, skip, take, categoryId))
                .Select(r => GetReviewDto(r)).ToList(),
                CountAll = await _platformContractReviewDbService.CustomerCount(userId, categoryId)
            };
        }

        public async Task<PaginatedList<PlatformContractReviewDto>> ExecutorList(int userId, int skip, int take, int categoryId)
        {
            return new PaginatedList<PlatformContractReviewDto>
            {
                List = (await _platformContractReviewDbService.ExecutorList(userId, skip, take, categoryId))
                .Select(r => GetReviewDto(r)).ToList(),
                CountAll = await _platformContractReviewDbService.ExecutorCount(userId, categoryId)
            };
        }

        public async Task<PlatformContractReviewDto> GetCustomer(int contractId)
        {
            PlatformContract contract = await _platformContractDbService.Get(contractId);
            PlatformContractReview review = await _platformContractReviewDbService.Get(contractId, contract.UserId);

            return GetReviewDto(review);
        }

        public async Task<PlatformContractReviewDto> GetExecutor(int contractId)
        {
            int executorId = (await _platformContractBidDbService.GetAccepted(contractId)).ApplicationUserId;
            PlatformContractReview review = await _platformContractReviewDbService.Get(contractId, executorId);

            return GetReviewDto(review);
        }

        public async Task<PaginatedList<PlatformContractReviewDto>> AllList(int userId, int skip, int take)
        {
            return new PaginatedList<PlatformContractReviewDto>
            {
                List = (await _platformContractReviewDbService.AllList(userId, skip, take))
                .Select(r => GetReviewDto(r)).ToList(),
                CountAll = await _platformContractReviewDbService.AllCount(userId)
            };
        }
    }
}
