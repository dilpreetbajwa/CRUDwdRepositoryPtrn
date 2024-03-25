using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDwdRepository.Core;
using CRUDwdRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDwdRepositoryPtrn.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAll();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> CreateorEdit(int id = 0)
        {
            if(id == 0){
                return View(new Product());
            }
            else
            {
                Product product = await _productRepo.GetById(id);
                if(product != null)
                {
                    return View(product);
                }
                TempData["errorMessage"] = "Product details not found";
                return RedirectToAction(nameof(Index));
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> CreateorEdit(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.Id == 0)
                    {
                        await _productRepo.Add(model);
                        TempData["successMessage"] = "Product Successfully Created!";
                    }
                    else
                    {
                        await _productRepo.Update(model);
                        TempData["successMessage"] = "Product Successfully Updated!";
                    }
                    return RedirectToAction(nameof(Index));


                }
                else
                {
                    TempData["errorMessage"] = "Model State is Invalid";
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            
        }

        public async Task<IActionResult> Delete(int id)
        {
            var products = await _productRepo.GetById(id);
            if(products != null)
            {
                return View(products);
            }
            TempData["errorMessage"] = "Product Id not found!";
            return RedirectToAction("Index");
        }

        [HttpPost,ActionName("Delete")]

         public async Task<IActionResult> DeleteConfirmation(int id)
        {
             await _productRepo.Delete(id);
            TempData["successMessage"] = "Product Deleted";
            return RedirectToAction("Index");
        }
    }
}

