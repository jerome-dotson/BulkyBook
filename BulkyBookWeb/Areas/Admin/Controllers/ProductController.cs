using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers


{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _HostEnvironment;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_HostEnvironment = hostEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}




		//GET
		public IActionResult Upsert(int? id)
		{

			ProductVM productVM = new()
			{
				Product = new Product(),
				CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem(
					i.Name, i.Id.ToString())),
				CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem(
					i.Name, i.Id.ToString()))
			};
			if (id == null || id == 0)
			{
				//ViewBag.CategoryList = CategoryList;
				//ViewData["CoverTypeList"] = CoverTypeList;
				////Create product
				return View(productVM);
			}
			else
			{
				//update product
				productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
				return View(productVM);
			}



		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductVM obj, IFormFile file)
		{


			if (ModelState.IsValid)
			{
				string wwwRootPath = _HostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(wwwRootPath, @"images\products");
					var extension = Path.GetExtension(file.FileName);

					if (obj.Product.ImageUrl != null)
					{
						var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
					{
						file.CopyTo(fileStreams);
					}
					obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
				}

				if (obj.Product.Id == 0)
				{
					_unitOfWork.Product.Add(obj.Product);
				}
				else
				{
					_unitOfWork.Product.Update(obj.Product);
				}

				//_unitOfWork.CoverType.Update(obj);

				_unitOfWork.Save();
				TempData["success"] = "Product created successfully.";
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
			var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType"); //include properties cannot include space between properties. 
			return Json(new { data = productList });
		}

		//POST
		[HttpDelete]

		public IActionResult Delete(int? id)
		{

			var obj = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
			if (obj == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			var oldImagePath = Path.Combine(_HostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
			if (System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}

			_unitOfWork.Product.Remove(obj);
			_unitOfWork.Save();
			return Json(new { success = true, message = "Delete successful" });
			//return RedirectToAction("Index"); //Add a second parameter that contains the name of another controller to do to the index actionof another controller



		}

		#endregion
	}

}
