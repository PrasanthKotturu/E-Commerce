using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{    
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productrepo;
        private readonly IGenericRepository<ProductType> _producttyperepo;
        private readonly IGenericRepository<ProductBrand> _productbrandrepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productrepo,
        IGenericRepository<ProductType> producttyperepo,
        IGenericRepository<ProductBrand> productbrandrepo,
        IMapper mapper)
        {
            _productrepo = productrepo;
            _producttyperepo = producttyperepo;
            _productbrandrepo = productbrandrepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductswithTypesandBrandsSpecification();
            var products = await _productrepo.ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProducts(int id)
        {
            var spec = new ProductswithTypesandBrandsSpecification(id);
            var product = await _productrepo.GetEntitywithSpec(spec);
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpGet("productbrands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productbrandrepo.ListAllAsync());
        }
        [HttpGet("producttypes")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _producttyperepo.ListAllAsync());
        }
    }
}