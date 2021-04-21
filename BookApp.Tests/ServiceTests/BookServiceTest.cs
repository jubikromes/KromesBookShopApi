using BookApp.API.Controllers;
using BookApp.Core.Models;
using BookApp.Core.Services.Interfaces;
using BookApp.Core.ViewModels;
using BookApp.Tests.Setup;
using Mailing.Core.Data;
using Mailing.Core.Data.Repositories;
using Mailing.Core.Models;
using Mailing.Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BookApp.Tests.ServiceTests
{
    public class BookServiceTest : IClassFixture<ServiceSetup>
    {
        private readonly Mock<IBookService> _bookService;
        private readonly Mock<IRepository<Book>> _bookRepository;
        private readonly Mock<IRepository<Author>> _authorRepository;
        private readonly Mock<IRepository<AuthorBook>> _authorbookRepository;
        private readonly Mock<IRepository<Category>> _categoryRepository;


        private readonly Mock<IUnitOfWork> _unitofWork;

        public BookServiceTest()
        {
            _bookService = new Mock<IBookService>();
            _bookRepository = new Mock<IRepository<Book>>();
            _unitofWork = new Mock<IUnitOfWork>();
            _authorRepository = new Mock<IRepository<Author>>();
            _authorbookRepository = new Mock<IRepository<AuthorBook>>();

            //_output = output;
             _categoryRepository = new Mock<IRepository<Category>>();
        }

        [Fact]
        public void Ensure_Book_Was_Added_WithEntityRelations()
        {
            ServiceSetup setup = new ServiceSetup();

            setup.Context.AddRange(new List<Author> { new Author { FirstName = "Oldberg", LastName = "James", Id = Guid.NewGuid() } });

            setup.Context.AddRange(new List<Category> { new Category { Title = "CIA Chimp" } });

            setup.Context.SaveChanges();

            var author = setup.UnitofWork.Repository<Author>().GetFirstOrDefault(p =>!p.IsDeleted );

            var category = setup.UnitofWork.Repository<Category>().GetFirstOrDefault(p => !p.IsDeleted);

            Assert.NotNull(author);

            Assert.NotNull(category);

            var model = new CreateBookViewModel
            {
                Title = "latest book in town",
                AuthorIds = new List<Guid> { author.Id },
                CategoryId = category.Id.ToString(),
                IsbnCode = "kysf3234",
                Published = DateTime.Now.Date
            };

            var result = setup.BookService.CreateBook(model).Result;

            //var book = setup.UnitofWork.Repository<Book>().GetFirstOrDefault(p => !p.IsDeleted);

            Assert.Equal("latest book in town", result.Item2.Title);

        }


        [Fact]
        public void Ensure_Book_Cant_Add_With_UnrelatedKeys()
        {
            var customerGuid = Guid.NewGuid();
            var catGuid = Guid.NewGuid();

            _authorRepository.Setup(y => y.GetById(It.IsAny<Guid>()))
                .Returns(new Author { Id = customerGuid, FirstName = "daiusd", LastName = "dshjads"});


            _categoryRepository.Setup(y => y.GetById(It.IsAny<Guid>()))
                .Returns(new Category { Id = catGuid, Title = "E don Pass" });


            var model = new CreateBookViewModel
            {
                Title = "Latest Book in Town",
                AuthorIds = new List<Guid> { customerGuid },
                CategoryId = catGuid.ToString(),
                IsbnCode = "kysf3234",
                Published = DateTime.Now.Date
            };

            var controller = new BookApiController(_bookService.Object);

            var response = controller.AddBook(model).Result;

            Assert.Equal(StatusCodes.Status500InternalServerError, response.Code);
        }

        [Fact]
        public void Ensure_Book_Exists()
        {

            _bookService.Setup(s => s.GetById(It.IsAny<Guid>())).Returns(new Book());

            _categoryRepository.Setup(s => s.GetById(It.IsAny<Guid>())).Returns(new Category());

            var service = new BookApiController(_bookService.Object);

            var response = service.GetBook(Guid.NewGuid().ToString()).Result;

            Assert.Equal(StatusCodes.Status200OK, response.Code);
        }
    }
}
