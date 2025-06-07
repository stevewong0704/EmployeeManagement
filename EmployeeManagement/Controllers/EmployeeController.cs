using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        // Inject the ApplicationDbContext to interact with the database
        private readonly ApplicationDbContext _context;

        // Constructor to receive ApplicationDbContext via Dependency Injection
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Employee/Index?keyword=xxx
        // Show the list of employees, optionally filtered by a keyword
        [HttpGet]
        public IActionResult Index(string? keyword)
        {
            // Start query for all employees from database
            var employees = _context.Employees.AsQueryable();

            // If user provided a search keyword, filter employees by Name, Position, or Remark
            if (!string.IsNullOrEmpty(keyword))
            {
                employees = employees.Where(e =>
                    e.Name.Contains(keyword) ||
                    e.Position.Contains(keyword) ||
                    e.Remark.Contains(keyword));
            }

            // Execute query and send list to the View
            return View(employees.ToList());
        }

        // GET: /Employee/Create
        // Show the form to create a new employee
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Employee/Create
        // Handle form submission to add a new employee
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            // Check if the submitted data is valid according to model validation attributes
            if (ModelState.IsValid)
            {
                // Add the new employee to the database context
                _context.Employees.Add(employee);

                // Save changes to the database
                _context.SaveChanges();

                // Redirect to the Index page after successful creation
                return RedirectToAction("Index");
            }

            // If validation failed, show the form again with validation messages
            return View(employee);
        }

        // GET: /Employee/Edit/{id}
        // Show the edit form for an employee by id
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Find the employee by id from the database
            var employee = _context.Employees.Find(id);

            // If employee not found, return 404 Not Found page
            if (employee == null)
            {
                return NotFound();
            }

            // Pass the employee data to the view for editing
            return View(employee);
        }

        // POST: /Employee/Edit
        // Handle form submission to update an employee
        [HttpPost]
        public IActionResult Edit(Employee updatedEmployee)
        {
            // Validate model state
            if (ModelState.IsValid)
            {
                // Update the employee in the database
                _context.Employees.Update(updatedEmployee);

                // Save changes
                _context.SaveChanges();

                // Redirect to Index after successful update
                return RedirectToAction("Index");
            }

            // If validation failed, return the form with errors
            return View(updatedEmployee);
        }

        // POST: /Employee/Delete/{id}
        // Delete an employee by id
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Find employee by id
            var employee = _context.Employees.Find(id);

            // If found, remove from database
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }

            // Redirect back to Index regardless of success
            return RedirectToAction("Index");
        }
    }
}
