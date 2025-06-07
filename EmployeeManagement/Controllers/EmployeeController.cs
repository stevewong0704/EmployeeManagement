using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        // ✅ 模擬資料庫的靜態列表
        private static List<Employee> employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", Remark= "1 year experience" },
            new Employee { Id = 2, Name = "Jane Smith", Position = "QA Engineer" },
            new Employee { Id = 3, Name = "Alex Tan", Position = "Business Analyst" }
        };



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            emp.Id = employees.Count + 1;
            employees.Add(emp);
            TempData["Message"] = "Employee created successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id); // 用 employees (這個靜態列表)，不是 EmployeeData
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee updatedEmp)
        {
            var employee = employees.FirstOrDefault(e => e.Id == updatedEmp.Id);
            if (employee == null)
            {
                return NotFound();
            }

            // 更新資料
            employee.Name = updatedEmp.Name;
            employee.Position = updatedEmp.Position;
            employee.Remark = updatedEmp.Remark;
            TempData["Message"] = "Employee updated successfully!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                employees.Remove(employee);
                TempData["Message"] = "Employee deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index(string? keyword)
        {
            var filteredEmployee = string.IsNullOrEmpty(keyword)
                ? employees
                : employees.Where(e => e.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                       e.Position.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                       e.Remark.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            return View(filteredEmployee);

        }
    }
}
