using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookApp.Core.Services.Interfaces;
using BookApp.Core.ViewModels;
using Mailing.Core.Services.Interfaces;
using Mailing.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookApiController : BaseApiController
    {
        private readonly IBookService _bookService;
        public BookApiController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<ApiResponse<BookResponseViewModel>> AddBook(CreateBookViewModel model)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var (errs, responseModel)= await _bookService.CreateBook(model);

                return new ApiResponse<BookResponseViewModel>
                (data: errs.Any() ? null : responseModel, message: errs.Any() ? errs.FirstOrDefault().ErrorMessage : "");

            }).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task<ApiResponse<EditBookViewModel>> UpdateBook(EditBookViewModel model)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var (errs, responseModel) = await _bookService.UpdateBook(model);

                return new ApiResponse<EditBookViewModel>
                (data: errs.Any() ? null : responseModel, message: errs.Any() ? errs.FirstOrDefault().ErrorMessage : "");

            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<BookViewModel>> GetBook(string id)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var  responseModel = await _bookService.GetBookById(id);

                return new ApiResponse<BookViewModel>(data: responseModel);

            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<List<BooksbyAuthorViewModel>>> GetBookByAuthor(string id)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var responseModel = await _bookService.GetBooksByAuthor(id);

                return new ApiResponse<List<BooksbyAuthorViewModel>>(data: responseModel);

            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<List<BooksbyAuthorViewModel>>> GetBookByCategory(string title)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var responseModel = await _bookService.GetBooksByCategory(title);

                return new ApiResponse<List<BooksbyAuthorViewModel>>(data: responseModel);

            }).ConfigureAwait(false);
        }


        [HttpDelete]
        public async Task<ApiResponse<List<ValidationResult>>> DeleteBook([Required]string id)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var responseModel = await _bookService.DeleteBook(id);

                return new ApiResponse<List<ValidationResult>>(errors: responseModel.Select(p => p.ErrorMessage).ToArray());

            }).ConfigureAwait(false);
        }
    }
}
