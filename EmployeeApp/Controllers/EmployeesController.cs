using EmployeeApp.Data;
using EmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public EmployeesController(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeDbContext.Employees.ToListAsync();
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
                Department = addEmployeeRequest.Department,
            };
            await _employeeDbContext.Employees.AddAsync(employee);
            await _employeeDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        //This method is used for retriving a single employee
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await _employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployee()
                {

                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department,
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }

        //This method is used for retriving a single employee

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployee model)
        {
            var employee = await _employeeDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await _employeeDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //This method is used for Deliting a single employee

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployee model)
        {
            var employee = await _employeeDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                _employeeDbContext.Employees.Remove(employee);
                await _employeeDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
