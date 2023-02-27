using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryRepository _db;

		public CategoryController(ICategoryRepository db)
		{
			_db = db;
		}

		public IActionResult Index()
		{

			IEnumerable<Category> objCategoryList = _db.GetAll(); //You can remove ToList()
			return View(objCategoryList);
		}


		//GET
		[HttpGet]
		public IActionResult Create()
		{

			return View();
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Category obj)
		{

			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name"); //Tp display in error summary, give custom name like CustomError rather than field name
			}

			if (ModelState.IsValid)
			{
				_db.Add(obj);
				_db.Save();
				TempData["success"] = "Category successfully created.";
				return RedirectToAction("Index"); //Add a second parameter that contains the name of another controller to do to the index actionof another controller

			}
			return View(obj);
		}

		//GET
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{

				return NotFound();
			}
			var categoryFromDb = _db.GetFirstOrDefault(c => c.Id == id); //Could also use _db.Catefories.Find(primary_key) or SingleOrDefault(expression)

			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Category obj)
		{

			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name"); //Tp display in error summary, give custom name like CustomError rather than field name
			}

			if (ModelState.IsValid)
			{
				_db.Update(obj);
				_db.Save();
				TempData["success"] = "Category successfully updated.";
				return RedirectToAction("Index"); //Add a second parameter that contains the name of another controller to do to the index actionof another controller

			}
			return View(obj);

		}

		//GET
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{

				return NotFound();
			}
			var categoryFromDb = _db.GetFirstOrDefault(c => c.Id == id); //Could also use _db.Catefories.Find(primary_key) or SingleOrDefault(expression)

			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		//POST
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOST(int? id)
		{

			var obj = _db.GetFirstOrDefault(c => c.Id == id);
			if (obj == null)
			{
				return NotFound();
			}


			_db.Remove(obj);
			_db.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index"); //Add a second parameter that contains the name of another controller to do to the index actionof another controller



		}
	}
}
