using diga.bl.Constants;
using diga.bl.Models;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.bl.OutModels.PlatformContractReviews;
using diga.dal.DbServices.PlatformChatRoomDbServices;
using diga.dal.DbServices.PlatformContractBidDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractPaymentPartDbServices;
using diga.dal.DbServices.PlatformContractReviewDbServices;
using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.dal.DbServices.PlatformEstimatePositionDbServices;
using diga.dal.DbServices.PlatformEstimateSectionDbServices;
using diga.web.Helpers;
using diga.web.Models.PlatformContractBids;
using diga.web.Models.PlatformEstimates;
using diga.web.Models.Status;
using Diga.Parsing.Builders;
using Diga.Parsing.Models.Estimates;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractBidServices
{
    public class PlatformContractBidService : IPlatformContractBidService
    {
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformEstimateDbService _platformEstimateDbService;
        private readonly IPlatformEstimateSectionDbService _platformEstimateSectionDbService;
        private readonly IPlatformEstimatePositionDbService _platformEstimatePositionDbService;
        private readonly IPlatformContractBidDbService _platformContractBidDbService;
        private readonly IPlatformContractPaymentPartDbService _platformContractPaymentPartDbService;
        private readonly IPlatformContractReviewDbService _platformContractReviewDbService;
        private readonly IPlatformChatRoomDbService _platformChatRoomDbService;
        private readonly IWebHostEnvironment _environment;

        public PlatformContractBidService(IPlatformContractDbService platformContractDbService,
            IPlatformEstimateDbService platformEstimateDbService,
            IPlatformEstimateSectionDbService platformEstimateSectionDbService,
            IPlatformEstimatePositionDbService platformEstimatePositionDbService,
            IPlatformContractBidDbService platformContractBidDbService,
            IPlatformContractPaymentPartDbService platformContractPaymentPartDbService,
            IPlatformContractReviewDbService platformContractReviewDbService,
            IPlatformChatRoomDbService platformChatRoomDbService,
            IWebHostEnvironment environment)
        {
            _platformContractDbService = platformContractDbService;
            _platformEstimateDbService = platformEstimateDbService;
            _platformEstimateSectionDbService = platformEstimateSectionDbService;
            _platformEstimatePositionDbService = platformEstimatePositionDbService;
            _platformContractBidDbService = platformContractBidDbService;
            _platformContractPaymentPartDbService = platformContractPaymentPartDbService;
            _platformContractReviewDbService = platformContractReviewDbService;
            _platformChatRoomDbService = platformChatRoomDbService;
            _environment = environment;
        }

        public async Task<ResponseData> Add(int userId, int contractId, PlatformContractBidAddDto request)
        {
            if (request.Status != PlatformContractBidStatuses.Published && request.Status != PlatformContractBidStatuses.Draft)
                return ResponseData.Fail("Status", "Incorrect status!");

            if (await _platformContractBidDbService.Any(userId, contractId))
                return ResponseData.Fail("Bid", "You already add bid for this project!");

            PlatformContract contract = await _platformContractDbService.GetFull(contractId);
            PlatformEstimate estimate = contract.PlatformEstimates.FirstOrDefault(e => e.CreatorId == contract.UserId);

            if (estimate != null && request.Sections != null)
            {
                PlatformEstimate platformEstimate = await _platformEstimateDbService.Add(new PlatformEstimate
                {
                    VersionNumber = estimate.VersionNumber + 1,
                    CreatorId = userId,
                    FileName = Path.Combine(Guid.NewGuid().ToString(), "xlsx"),
                    IsOrdered = estimate.IsOrdered,
                    Name = estimate.Name,
                    PlatformContractId = estimate.PlatformContractId,
                    VatType = estimate.VatType,
                    AnotherPercent = estimate.AnotherPercent,
                    OriginalId = estimate.Id
                });

                PlatformEstimate contractEstimate = await _platformEstimateDbService.Get(contractId, contract.UserId);

                List<PlatformEstimateSection> sections = await _platformEstimateSectionDbService.List(contractEstimate.Id);
                List<PlatformEstimatePosition> positions = await _platformEstimatePositionDbService.List(contractEstimate.Id);

                sections = sections.Select(s => s.Clone()).ToList();
                positions = positions.Select(p => p.Clone()).ToList();

                foreach (PlatformEstimateExecutorSectionDto section in request.Sections)
                    sections.First(s => s.Id == section.Id).Notes = section.Notes;

                foreach (PlatformEstimateExecutorPositionDto position in request.Positions)
                {
                    PlatformEstimatePosition positionToUpdate = positions.First(p => p.Id == position.Id);
                    positionToUpdate.Price = position.Price ?? 0;
                    positionToUpdate.Notes = position.Notes;
                }

                PlatformEstimateFullDto platformEstimateFull = PlatformEstimateHelper.Parse(platformEstimate, sections, positions);
                Tuple<List<PlatformEstimateSection>, List<PlatformEstimatePosition>> tuple =
                    PlatformEstimateHelper.Build(platformEstimateFull.Sections, platformEstimate.Id);

                foreach (PlatformEstimateSection section in tuple.Item1)
                {
                    if (section.FullNumber.Contains('.'))
                        section.ParentSection = tuple.Item1.First(i => section.FullNumber == $"{i.FullNumber}.{section.Number}");

                    section.Positions = tuple.Item2.Where(p => p.FullNumber == $"{section.FullNumber}.{p.Number}").ToList();
                }

                await _platformEstimateSectionDbService.AddRange(tuple.Item1);

                string estimatePath = Path.Combine(_environment.WebRootPath, "docs", "src", estimate.FileName);

                new EstimateBuilder(new EstimateBuildDto
                {
                    Sections = sections.Select(s => new EstimateSectionBuildDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Number = s.Number,
                        ParentSectionId = s.ParentSectionId
                    }).ToList(),
                    Positions = positions.Select(p => new EstimatePositionBuildDto
                    {
                        Id = p.Id,
                        Description = p.Description,
                        Measurement = p.Measurement,
                        Number = p.Number,
                        Quantity = p.Quantity,
                        SectionId = p.EstimateSectionId
                    }).ToList()
                }, estimatePath).BuildEstimate();
            }

            await _platformContractBidDbService.Add(new PlatformContractBid
            {
                ApplicationUserId = userId,
                PlatformContractId = contractId,
                CreatedDate = DateTime.UtcNow,
                Status = request.Status,
                Text = request.Text,
                Price = request.Price,
                DaysNumber = request.DaysNumber
            });

            return new ResponseData();
        }

        public async Task<ResponseData> Edit(int userId, int contractId, PlatformContractBidEditDto request)
        {
            PlatformContractBid bid = await _platformContractBidDbService.Get(userId, contractId);

            if (request.Status != PlatformContractBidStatuses.Published && request.Status != PlatformContractBidStatuses.Draft)
                return ResponseData.Fail("Status", "Incorrect status!");
            if (bid.Status != PlatformContractBidStatuses.Published && bid.Status != PlatformContractBidStatuses.Draft)
                return ResponseData.Fail("Status", "You cannot edit this bid status!");
            if (bid.Status != PlatformContractBidStatuses.Draft && request.Status == PlatformContractBidStatuses.Draft)
                return ResponseData.Fail("Status", "You cannot edit status to draft!");

            bid.Text = request.Text;
            bid.Price = request.Price;
            bid.DaysNumber = request.DaysNumber;
            bid.Status = request.Status;
            bid.UpdatedDate = DateTime.UtcNow;

            await _platformContractBidDbService.Update(bid);

            return new ResponseData();
        }

        public async Task<ResponseData> Withdrawn(int userId, int contractId)
        {
            if (await _platformContractBidDbService.Any(userId, contractId))
            {
                PlatformContractBid bid = await _platformContractBidDbService.Get(userId, contractId);

                if (bid.Status == PlatformContractBidStatuses.Published)
                    bid.Status = PlatformContractBidStatuses.Withdrawn;

                await _platformContractBidDbService.Update(bid);
            }

            return new ResponseData();
        }

        public async Task<ResponseData> WithdrawnReturn(int userId, int contractId)
        {
            if (await _platformContractBidDbService.Any(userId, contractId))
            {
                PlatformContractBid bid = await _platformContractBidDbService.Get(userId, contractId);

                if (bid.Status == PlatformContractBidStatuses.Withdrawn)
                    bid.Status = PlatformContractBidStatuses.Published;

                await _platformContractBidDbService.Update(bid);
            }

            return new ResponseData();
        }

        public async Task<ResponseData> RemoveReturn(int userId, int contractId, int bidUserId)
        {
            if (await _platformContractBidDbService.Any(bidUserId, contractId))
            {
                PlatformContractBid bid = await _platformContractBidDbService.GetFull(bidUserId, contractId);

                if (bid.PlatformContract.UserId != userId)
                    return ResponseData.Fail("User", "Access denied!");

                if (bid.Status == PlatformContractBidStatuses.Rejected)
                    bid.Status = PlatformContractBidStatuses.Published;

                await _platformContractBidDbService.Update(bid);
            }

            return new ResponseData();
        }

        public async Task<ResponseData> Remove(int userId, int contractId, int bidUserId)
        {
            if (await _platformContractBidDbService.Any(bidUserId, contractId))
            {
                PlatformContractBid bid = await _platformContractBidDbService.GetFull(bidUserId, contractId);

                if (bid.PlatformContract.UserId != userId)
                    return ResponseData.Fail("User", "Access denied!");

                if (bid.Status == PlatformContractBidStatuses.Published)
                    bid.Status = PlatformContractBidStatuses.Rejected;

                await _platformContractBidDbService.Update(bid);
            }

            return new ResponseData();
        }

        public async Task<PlatformContractBidListDto> List(int userId, int contractId)
        {
            List<PlatformContractBidDto> bids = (await _platformContractBidDbService.List(contractId))
                .Select(b => new PlatformContractBidDto
                {
                    UserAvatar = b.ApplicationUser.Avatar,
                    FullName = $"{b.ApplicationUser.Name} {b.ApplicationUser.Surname}",
                    UserId = b.ApplicationUser.Id,
                    UserRating = b.ApplicationUser.Ratings.Sum(r => r.Points),
                    DaysNumber = b.DaysNumber,
                    Price = b.Price,
                    Status = b.Status,
                    Text = b.Text,
                    Favorite = b.Favorite
                }).ToList();

            PlatformContractBid userBid = await _platformContractBidDbService.GetFull(userId, contractId);

            // Map reviews
            List<int> userIds = bids.Select(b => b.UserId).ToList();
            if (userBid != null && !userIds.Contains(userId))
                userIds.Add(userId);

            List<PlatformContractReviewUser> reviewUsers = await _platformContractReviewDbService.Count(bids.Select(b => b.UserId).ToList());
            foreach (PlatformContractBidDto bid in bids)
            {
                bid.UserBadReviews = reviewUsers.Count(r => (r.CustomerId == bid.UserId || r.ExecutorId == bid.UserId) && !r.IsGoodReview);
                bid.UserGoodReviews = reviewUsers.Count(r => (r.CustomerId == bid.UserId || r.ExecutorId == bid.UserId) && r.IsGoodReview);
            }

            return new PlatformContractBidListDto
            {
                Bids = bids,
                UserBid = userBid == null ? null :
                    new PlatformContractBidDto
                    {
                        UserAvatar = userBid.ApplicationUser.Avatar,
                        FullName = $"{userBid.ApplicationUser.Name} {userBid.ApplicationUser.Surname}",
                        UserId = userBid.ApplicationUser.Id,
                        UserRating = userBid.ApplicationUser.Ratings.Sum(r => r.Points),
                        DaysNumber = userBid.DaysNumber,
                        Price = userBid.Price,
                        Status = userBid.Status,
                        Text = userBid.Text,
                        Favorite = userBid.Favorite,
                        UserBadReviews = reviewUsers.Count(r => (r.CustomerId == userId || r.ExecutorId == userId) && !r.IsGoodReview),
                        UserGoodReviews = reviewUsers.Count(r => (r.CustomerId == userId || r.ExecutorId == userId) && r.IsGoodReview)
                    }
            };
        }

        public async Task<ResponseData> AddFavorite(int userId, int contractId, int bidUserId)
        {
            PlatformContract contract = await _platformContractDbService.Get(contractId);
            if (contract.UserId != userId)
                return ResponseData.Fail("User", "Access denied!");

            PlatformContractBid bid = await _platformContractBidDbService.Get(bidUserId, contractId);

            bid.Favorite = true;
            await _platformContractBidDbService.Update(bid);

            return new ResponseData();
        }

        public async Task<ResponseData> RemoveFavorite(int userId, int contractId, int bidUserId)
        {
            PlatformContract contract = await _platformContractDbService.Get(contractId);
            if (contract.UserId != userId)
                return ResponseData.Fail("User", "Access denied!");

            PlatformContractBid bid = await _platformContractBidDbService.Get(bidUserId, contractId);

            bid.Favorite = false;
            await _platformContractBidDbService.Update(bid);

            return new ResponseData();
        }

        public async Task<ResponseData> Select(int userId, int contractId, int bidUserId)
        {
            PlatformContract contract = await _platformContractDbService.Get(contractId);
            if (contract.UserId != userId)
                return ResponseData.Fail("User", "Access denied!");

            List<PlatformContractBid> bids = await _platformContractBidDbService.List(contractId);
            if (bids.Any(b => b.Status == PlatformContractBidStatuses.Accepted))
                return ResponseData.Fail("Status", "You already select bid!");

            PlatformContractBid bid = bids.First(b => b.ApplicationUserId == bidUserId);

            bid.Status = PlatformContractBidStatuses.Accepted;
            await _platformContractBidDbService.Update(bid);

            // Add payment parts for approve
            List<PlatformContractPaymentPart> parts = new List<PlatformContractPaymentPart> { };
            Dictionary<string, double> percents = PercentDivider.Divide(bid.Price, new Dictionary<string, double>
            {
                { "Na adjudicação", 0.1 },
                { "No inicio da obra", 0.35 },
                { "No decurso da obra", 0.35 },
                { "Com da conclusão da obra", 0.2 }
            });

            int number = 1;
            foreach (KeyValuePair<string, double> percent in percents)
            {
                parts.Add(new PlatformContractPaymentPart
                {
                    Name = percent.Key,
                    DateTime = DateTime.Now,
                    Percent = percent.Value * 100,
                    Value = Math.Round((contract.Price ?? 0) * percent.Value, 2),
                    Number = number,
                    PercentOfWork = percent.Value * 100,
                    PlatformContractId = contractId
                });

                number++;
            }

            await _platformContractPaymentPartDbService.AddRange(parts);

            contract.Price = bid.Price;

            PlatformEstimate platformEstimate = await _platformEstimateDbService.Get(contractId, bid.ApplicationUserId);
            if (platformEstimate != null && !string.IsNullOrEmpty(platformEstimate.Name)){
                contract.Status = PlatformContractStatuses.EstimateApproval;
            }
            else{
                contract.Status = PlatformContractStatuses.ContractApproval;
            }
            contract.MainEstimateId = platformEstimate?.Id;
            await _platformContractDbService.Update(contract);

            //create chat room
            var chatRoom = await _platformChatRoomDbService.Get(contractId, userId, bidUserId);
            if (chatRoom == null)
            {
                await _platformChatRoomDbService.Add(new bl.Models.Platform.Chats.PlatformChatRoom
                {
                    RoomName = contract.Name,
                    PlatformContractId = contract.Id,
                    Users = new List<bl.Models.Platform.Chats.PlatformChatRoomUser>
                {
                    new bl.Models.Platform.Chats.PlatformChatRoomUser
                    {
                        ApplicationUserId = userId,
                        LastStatusChanged = DateTime.Now
                    },
                    new bl.Models.Platform.Chats.PlatformChatRoomUser
                    {
                        ApplicationUserId = bidUserId,
                        LastStatusChanged = DateTime.Now
                    }
                }
                });
            }

            return new ResponseData();
        }
    }
}
