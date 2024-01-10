using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project001.Data;
using project001.Models;
using project001.Models.Domain;

namespace project001.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly project001DbContext project01DbContext;

        public EmployeesController(project001DbContext project01DbContext)
        {
            this.project01DbContext = project01DbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await project01DbContext.Employees.ToListAsync();
            return View(employees);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department
            };

            await project01DbContext.Employees.AddAsync(employee);
            await project01DbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await project01DbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var ViewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };

                return await Task.Run(() => View("View", ViewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await project01DbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await project01DbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await project01DbContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                project01DbContext.Employees.Remove(employee);
                await project01DbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
