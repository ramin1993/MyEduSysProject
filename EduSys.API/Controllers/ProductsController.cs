﻿using AutoMapper;
using EduSys.API.Filters;
using EduSys.Core.DTOs;
using EduSys.Core.Models;
using EduSys.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduSys.API.Controllers
{

    public class ProductsController: CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _service;
        public ProductsController(IMapper mapper,IProductService service)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpGet]
        public async  Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productsDtos =  _mapper.Map<List<ProductDto>>(products.ToList());
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto )
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201/*yaradildi status code*/, productsDto));
        }
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = _mapper.Map<Product>(id);
            if (product == null)//silinecek ele bele yazilib.
            {
                return CreateActionResult( CustomResponseDto<NoContentDto>.Fail(404,"Bu id'e sahib Product tapilmadi"));
            }
            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductWithCategory()
        {
            var productWithCategory = await _service.GetProductsWithCategory();
            return CreateActionResult(productWithCategory);
        }


    }
}
