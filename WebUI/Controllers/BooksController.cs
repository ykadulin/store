using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using Domain.Entities;


namespace WebUI.Controllers
{
    public class BooksController : Controller
    {
        private IBookRepository repository;
        public int pageSize = 10;

        public BooksController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string genre, int page = 1)
        {
            BooksListViewModel model = new BooksListViewModel
            {
                Books = repository.Books
                .Where(b => genre == null || b.Genre == genre)
                .OrderBy(book => book.BookId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = genre == null ? 
                        repository.Books.Count() : 
                        repository.Books.Where(book => book.Genre == genre).Count()
                },
                CurrentGenre = genre
            };
            
            return View(model);
        }

        public FileContentResult GetImage(int bookId)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                return File(book.ImageData, book.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}