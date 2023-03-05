using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers


{
	[Area("Admin")]
	public class CompanyController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _HostEnvironment;

		public CompanyController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}

		public IActionResult Index()
		{
			return View();
		}




		//GET
		public IActionResult Upsert(int? id)
		{

			Company company = new();


			if (id == null || id == 0)
			{
				//ViewBag.CategoryList = CategoryList;
				//ViewData["CoverTypeList"] = CoverTypeList;
				////Create product
				return View(company);
			}
			else
			{
				//update product
				company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
				return View(company);
			}



		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Company obj, IFormFile file)
		{


			if (ModelState.IsValid)
			{
				string wwwRootPath = _HostEnvironment.WebRootPath;


				if (obj.Id == 0)
				{
					_unitOfWork.Company.Add(obj);
					TempData["success"] = "Company created successfully.";
				}
				else
				{
					_unitOfWork.Company.Update(obj);
					TempData["success"] = "Company updated successfully.";
				}

				//_unitOfWork.CoverType.Update(obj);

				_unitOfWork.Save();

				return RedirectToAction("Index"); //Add a second parameter that contains the name of another controller to do to the index actionof another controller

			}
			return View(obj);

		}

		////GET
		//[HttpGet]
		//public IActionResult Delete(int? id)
		//{
		//	if (id == null || id == 0)
		//	{

		//		return NotFound();
		//	}
		//	var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id); //Could also use _db.Catefories.Find(primary_key) or SingleOrDefault(expression)

		//	if (coverTypeFromDb == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(coverTypeFromDb);
		//}



		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			var companyList = _unitOfWork.Company.GetAll(); //include properties cannot include space between properties. 
			return Json(new { data = companyList });
		}

		//POST
		[HttpDelete]

		public IActionResult Delete(int? id)
		{

			var obj = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
			if (obj == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}


			_unitOfWork.Company.Remove(obj);
			_unitOfWork.Save();
			return Json(new { success = true, message = "Delete successful" });
			//return RedirectToAction("Index"); //Add a second parameter that contains the name of another controller to do to the index actionof another controller



		}

		#endregion
	}

}
