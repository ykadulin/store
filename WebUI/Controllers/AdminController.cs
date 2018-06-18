using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IBookRepository repository;

        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Books);
        }

        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);

            return View(book);
        }

        public ViewResult Create()
        {
            return View("Edit", new Book());
        }

        [HttpPost]
        public ActionResult Edit(Book book, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    book.ImageMimeType = image.ContentType;
                    book.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(book.ImageData, 0, image.ContentLength);
                }
                repository.SaveBook(book);
                TempData["message"] = string.Format("Изменения товара \"{0}\" были сохранены", book.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(book);
            }
        }


        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["message"] = string.Format("Товар \"{0}\" добавлен успешно.", book.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Delete(int BookId)
        {
            Book deletedBook = repository.DeleteBook(BookId);
            if (deletedBook != null)
            {
                TempData["message"] = string.Format("Товар \"{0}\" был удален",
                    deletedBook.Name);
            }
            return RedirectToAction("Index");
        }

        

    }


}

