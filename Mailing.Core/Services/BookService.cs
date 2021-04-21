using BookApp.Core.Models;
using BookApp.Core.Services.Interfaces;
using BookApp.Core.ViewModels;
using Kromes.Core.Data;
using Kromes.Core.Models;
using Kromes.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace BookApp.Core.Services
{
    public class BookService : Service<Book>, IBookService
    {
        private IMemoryCache _cache;

        public static string AllInCategory { get { return "_allincategories"; } }
        public static string AllByAuthors { get { return "_allbyauthors"; } }



        public BookService(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : base(unitOfWork)
        {
            _cache = memoryCache;
        }

        public async Task<(List<ValidationResult>, BookResponseViewModel)> CreateBook(CreateBookViewModel model)
        {

            var responseModel = new BookResponseViewModel();


            if (!Guid.TryParse(model.CategoryId, out Guid categoryId))
            {
                results.Add(new ValidationResult("Invalid Request"));
                goto Response;
            }

            var category =  await UnitOfWork.Repository<Category>().GetByIdAsync(categoryId);

            if (category == null)
            {
                results.Add(new ValidationResult("Category not found"));
                goto Response;
            }
            var trimmedSrc = model.Title.Trim();

            var existingBook =  FirstOrDefault(p => EF.Functions.Like(p.Title, trimmedSrc) && !p.IsDeleted);

            if (existingBook != null)
            {
                results.Add(new ValidationResult("Author does not exist"));
                goto Response;
            }

            if (model.AuthorIds.Count <= 0)
            {
                results.Add(new ValidationResult("Author does not exist"));
                goto Response;
            }

            UnitOfWork.BeginTransaction();

            var book = new Book();
            book.Title = model.Title.ToLower();
            book.Published = model.Published.Date;
            book.IsbnCode = model.IsbnCode;
            book.CategoryId = category.Id;

            UnitOfWork.Repository<Book>().Insert(book);
            foreach (var authorId in model.AuthorIds)
            {
                var author = await UnitOfWork.Repository<Author>().GetByIdAsync(authorId);
                if (author == null)
                {
                    results.Add(new ValidationResult("One of the authors do not exist"));
                    goto Response;
                }    

                UnitOfWork.Repository<AuthorBook>().Insert(new AuthorBook { AuthorId = authorId, BookId = book.Id});
            }

            responseModel = (BookResponseViewModel)book;
            responseModel.Authors = model.AuthorIds;
            await UnitOfWork.CommitAsync();

            Response:
            return (results, responseModel);
        }

        public async Task<BookViewModel> GetBookById(string id)
        {
            Guid.TryParse(id, out Guid guid);
            var existingBook = await GetByIdAsync(guid);

            if (existingBook == null || existingBook.IsDeleted)
            {
                throw new Exception("Book does not exist");
            }

            var book =  (BookViewModel)existingBook;

            var bookAuthors = from authors in UnitOfWork.Repository<Author>().GetAll()
                              join books in UnitOfWork.Repository<AuthorBook>().GetAll()

                              on authors.Id equals books.AuthorId
                              where books.BookId == book.Id

                              select new BookAuthorModel
                              {
                                  AuthorId = authors.Id,
                                  AuthorName = $"{authors.FirstName} {authors.LastName}"
                              };


            book.Authors = bookAuthors.ToList();

            return book;

        }

        public async Task<List<BooksbyAuthorViewModel>> GetBooksByAuthor(string authorId)
        {

            Guid.TryParse(authorId, out Guid aid);

            if (!_cache.TryGetValue(AllByAuthors, out IQueryable<BooksbyAuthorViewModel> booksbyAuthor))
            {
                 booksbyAuthor = from books in UnitOfWork.Repository<Book>().GetAll()
                                    join authorbooks in UnitOfWork.Repository<AuthorBook>().GetAll()

                                    on books.Id equals authorbooks.BookId

                                    join author in UnitOfWork.Repository<Author>().GetAll()
                                    on authorbooks.AuthorId equals author.Id

                                    join Category cat in UnitOfWork.Repository<Category>().GetAll()
                                    on books.CategoryId equals cat.Id

                                    where author.Id == aid && !books.IsDeleted

                                    select new BooksbyAuthorViewModel
                                    {
                                        AuthorName = $"{author.FirstName} { author.LastName}",
                                        BookTitle = books.Title,
                                        BookId = books.Id,
                                        IsbnCode = books.IsbnCode,
                                        Published = books.Published.Date,
                                    };

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20));

                _cache.Set(AllInCategory, booksbyAuthor, cacheEntryOptions);
            }

           

            return await booksbyAuthor.ToListAsync();
        }


        public async Task<List<BooksbyAuthorViewModel>> GetBooksByCategory(string category)
        {

            if (!_cache.TryGetValue(AllInCategory, out IQueryable<BooksbyAuthorViewModel> listbooks))
            {
                listbooks = from books in UnitOfWork.Repository<Book>().GetAll()
                            join authorbooks in UnitOfWork.Repository<AuthorBook>().GetAll()

                            on books.Id equals authorbooks.BookId

                            join author in UnitOfWork.Repository<Author>().GetAll()
                            on authorbooks.AuthorId equals author.Id

                            join Category cat in UnitOfWork.Repository<Category>().GetAll()
                            on books.CategoryId equals cat.Id

                            where EF.Functions.Like(category, cat.Title) && !books.IsDeleted

                            select new BooksbyAuthorViewModel
                            {
                                AuthorName = $"{author.FirstName} { author.LastName}",
                                BookTitle = books.Title,
                                BookId = books.Id,
                                IsbnCode = books.IsbnCode,
                                Published = books.Published.Date,
                            };

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20));

                _cache.Set(AllInCategory, listbooks, cacheEntryOptions);
            }

            return await listbooks.ToListAsync();
        }

        public async Task<(List<ValidationResult>, EditBookViewModel)> UpdateBook(EditBookViewModel model)
        {
            var responseModel = new EditBookViewModel();

            if (!Guid.TryParse(model.Id, out Guid guid))
            {
                results.Add(new ValidationResult("Invalid request"));
                goto Response;
            }

            if (!Guid.TryParse(model.CategoryId, out Guid categoryId))
            {
                results.Add(new ValidationResult("Invalid Request"));
                goto Response;
            }

            var category = await UnitOfWork.Repository<Category>().GetByIdAsync(categoryId);

            if (category == null)
            {
                results.Add(new ValidationResult("Category not found"));
                goto Response;
            }
            UnitOfWork.BeginTransaction();


            var existingBook = await GetByIdAsync(guid);

            if (existingBook == null || existingBook.IsDeleted)
            {
                results.Add(new ValidationResult("Book with Id does not exist"));
                goto Response;
            }

            var existingBookTitle = FirstOrDefault
                (p => EF.Functions.Like(p.Title.Trim(), model.Title.Trim()) && !Equals(p.Id, guid) && !p.IsDeleted);

            if (existingBookTitle != null)
            {
                results.Add(new ValidationResult("Book with title already exist"));
                goto Response;
            }

            existingBook.Title = model.Title;
            existingBook.IsbnCode = model.IsbnCode;
            existingBook.Published = model.Published.Date;

            var currentAuthorIds = UnitOfWork.Repository<AuthorBook>().GetAll(p =>  p.BookId == existingBook.Id).Select(p => p.AuthorId);
            var incomingIds = model.AuthorIds;


            var excluding = currentAuthorIds.ToList().Except(incomingIds);

            foreach (var ex in excluding)
            {
                var getFirstDefault = UnitOfWork.Repository<AuthorBook>().GetFirstOrDefault(p => p.BookId == existingBook.Id && p.AuthorId == ex); 
                if (getFirstDefault == null) { throw new Exception("Author details not valid. Kindly confirm inputs"); };

                UnitOfWork.Repository<AuthorBook>().Remove(getFirstDefault);
            }

            var including = incomingIds.Except(currentAuthorIds.ToList());

            foreach (var @in in including)
            {
                var getFirstDefault = UnitOfWork.Repository<Author>().GetFirstOrDefault(p => p.Id == @in);
                if (getFirstDefault == null) { throw new Exception("Author details not valid. Kindly confirm inputs"); };
                UnitOfWork.Repository<AuthorBook>().Insert(new AuthorBook { BookId = existingBook.Id, AuthorId = @in});
            }

            await UnitOfWork.CommitAsync();
            responseModel = model;

            Response:
            return (results, responseModel);

        }

        public async Task<List<ValidationResult>> DeleteBook(string id)
        {
            Guid.TryParse(id, out Guid guid);
            var existingBook = await GetByIdAsync(guid);

            if (existingBook == null || existingBook.IsDeleted)
            {
                results.Add(new ValidationResult("Book does not exist or has already been deleted"));
                goto Response;
            }


            existingBook.IsDeleted = true;

            await UpdateAsync(existingBook);

            Response:

            return results;

        }

    }
}
