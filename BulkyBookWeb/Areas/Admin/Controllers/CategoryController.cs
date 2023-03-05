using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers


{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{

			IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll(); //You can remove ToList()
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
				_unitOfWork.Category.Add(obj);
				_unitOfWork.Save();
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
			var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id); //Could also use _db.Catefories.Find(primary_key) or SingleOrDefault(expression)

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
				_unitOfWork.Category.Update(obj);
				_unitOfWork.Save();
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
			var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id); //Could also use _db.Catefories.Find(primary_key) or SingleOrDefault(expression)

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

			var obj = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
			if (obj == null)
			{
				return NotFound();
			}


			_unitOfWork.Category.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index"); //Add a second parameter that contains the name of another controller to do to the index actionof another controller



		}
	}
}
