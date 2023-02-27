using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers


{
	public class CoverTypeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CoverTypeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{

			IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll(); //You can remove ToList()
			return View(objCoverTypeList);
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
		public IActionResult Create(CoverType obj)
		{


			if (ModelState.IsValid)
			{
				_unitOfWork.CoverType.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "CoverType successfully created.";
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
			var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id); //Could also use _db.Catefories.Find(primary_key) or SingleOrDefault(expression)

			if (coverTypeFromDb == null)
			{
				return NotFound();
			}

			return View(coverTypeFromDb);
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(CoverType obj)
		{


			if (ModelState.IsValid)
			{
				_unitOfWork.CoverType.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "CoverType successfully updated.";
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
			var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id); //Could also use _db.Catefories.Find(primary_key) or SingleOrDefault(expression)

			if (coverTypeFromDb == null)
			{
				return NotFound();
			}

			return View(coverTypeFromDb);
		}

		//POST
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOST(int? id)
		{

			var obj = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
			if (obj == null)
			{
				return NotFound();
			}


			_unitOfWork.CoverType.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "CoverType deleted successfully";
			return RedirectToAction("Index"); //Add a second parameter that contains the name of another controller to do to the index actionof another controller



		}
	}
}
