using Microsoft.AspNetCore.Mvc;
using VisitorCore.Models;
using VisitorMvc.Models;
using System.Reflection;


namespace VisitorMvc.Controllers
{
    public class VisitorController : Controller
    {
        public async Task<IActionResult> Index(VisitorFilterDto filter)
        {

            using HttpClient client = new HttpClient();

            var response = await client.PostAsJsonAsync("http://localhost:5301/api/Visitor/GetAll", filter);
            var model = await response.Content.ReadFromJsonAsync<VisitorGetAllDto>();

            var viewModel = new VisitorViewModel
            {
                Visitors = model.Visitors,
                Doctors = model.Doctors,
                Filter = filter
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {

            using HttpClient client = new HttpClient();

            var model = await client.DeleteAsync($"http://localhost:5301/api/Visitor?id={id}");

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Add(string name, string lastName, int doctorId, string Email, string Phone)
        {

            using HttpClient client = new HttpClient();

            var visitor = new VisitorAddDto { DoctorId = doctorId, FirstName = name, LastName = lastName, Email = Email, Phone = Phone };

            await client.PutAsJsonAsync($"http://localhost:5301/api/Visitor", visitor);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            using HttpClient client = new HttpClient();

            var model = await client.GetFromJsonAsync<List<Doctor>>("http://localhost:5301/api/Doctor/GetAll");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VisitorEditDto visitor)
        {

            using HttpClient client = new HttpClient();

            await client.PostAsJsonAsync($"http://localhost:5301/api/Visitor", visitor);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            using HttpClient client = new HttpClient();

            var model = new VisitorEditViewModel
            {
                Visitor = await client.GetFromJsonAsync<VisitorGetDto>($"http://localhost:5301/api/Visitor/GetOne?id={id}"),

                Doctors = await client.GetFromJsonAsync<List<Doctor>>("http://localhost:5301/api/Doctor/GetAll")

            };

            return View(model);

        }
    }
}
