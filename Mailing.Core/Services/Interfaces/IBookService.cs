using BookApp.Core.ViewModels;
using Kromes.Core.Models;
using Kromes.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.Core.Services.Interfaces
{
    public interface IBookService : IService<Book>
    {
        Task<(List<ValidationResult>, BookResponseViewModel)> CreateBook(CreateBookViewModel model);

        Task<(List<ValidationResult>, EditBookViewModel)> UpdateBook(EditBookViewModel model);

        Task<BookViewModel> GetBookById(string id);
        Task<List<BooksbyAuthorViewModel>> GetBooksByCategory(string category);
        Task<List<BooksbyAuthorViewModel>> GetBooksByAuthor(string authorId);

        Task<List<ValidationResult>> DeleteBook(string id);

    }
}
