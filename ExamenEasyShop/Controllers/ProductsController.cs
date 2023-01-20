using ExamenEasyShop.Configuration;
using ExamenEasyShop.Data;
using ExamenEasyShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExamenEasyShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ExamenEasyShopContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(ExamenEasyShopContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {

            return View( await _unitOfWork.ProductRepository.GetAllAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {


            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "NameCategory");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,ProductName,ProductDescription,Price,CountInStock,ImageURL,Rating")] Product product)
        {

            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Commit();
            return RedirectToAction(nameof(Index));



        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {


            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id); ;
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "NameCategory", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,ProductName,ProductDescription,Price,CountInStock,ImageURL,Rating")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }


            try
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {


            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ExamenEasyShopContext.Product'  is null.");
            }

            _unitOfWork.ProductRepository.Delete(id);
            _unitOfWork.Commit();

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {

            return _unitOfWork.ProductRepository.GetByIdAsync(id) != null;
        }
    }
}
