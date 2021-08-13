using diga.bl.Models.Platform.PaidFeatures;
using diga.dal.DbServices.ProductDbServices;
using diga.web.Models.PaidFeatures;
using diga.web.Models.Status;
using diga.web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.ProductServices
{
    public class PaidFeatureService : IPaidFeatureService
    {
        private readonly IPaidFeatureDbService _productDbService;

        public PaidFeatureService(IPaidFeatureDbService productDbService)
        {
            _productDbService = productDbService;
        }

        public async Task<ResponseData> Add(PaidFeatureDto productModel)
        {
           await _productDbService.Add(new PaidFeature { 
                Currency = productModel.Currency,
                Description = productModel.Description,
                Price = productModel.Price,
                NameId = productModel.NameId
            });

            return new ResponseData();
        }

        public async Task<ResponseData> Delete(int productId)
        {
            var product = await _productDbService.Get(productId);

            await _productDbService.Remove(product);

            return new ResponseData();
        }

        public async Task<ResponseData> Edit(int productId, PaidFeatureDto productModel)
        {
            var product = await _productDbService.Get(productId);

            product.Currency = productModel.Currency;
            product.Description = productModel.Description;
            product.NameId = productModel.NameId;
            product.Price = productModel.Price;

            await _productDbService.Update(product);

            return new ResponseData();
        }

        public async Task<PaidFeature> Get(int productId)
        {
            var product = await _productDbService.Get(productId);

            return product;
        }

        public async Task<List<PaidFeature>> GetAll()
        {
            return await _productDbService.List();
        }

        public async Task<List<PaidFeature>> GetPaginatedList(int skip, int take)
        {
           return await _productDbService.GetPaginated(skip, take);
        }
    }
}
