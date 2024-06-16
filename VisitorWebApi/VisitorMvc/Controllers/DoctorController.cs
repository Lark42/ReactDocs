using Microsoft.AspNetCore.Mvc;
using VisitorCore.Models;
using System.Xml.Linq;
using VisitorMvc.Models;

namespace VisitorMvc.Controllers
{
    public class DoctorController : Controller
    {
        public async Task<IActionResult> Index(DoctorFilterDto filter)
        {
            using HttpClient client = new HttpClient();
            // DoctorFilterDto filter = new DoctorFilterDto();

            var response = await client.PostAsJsonAsync("http://localhost:5301/api/Doctor/GetAll", filter);
            var doctors = await response.Content.ReadFromJsonAsync<List<Doctor>>();

            var viewModel = new DoctorViewModel
            {
                Filter = filter,
                Doctors = doctors
            };

            return View(viewModel);

        }
        public async Task<IActionResult> Delete(int id)
        {

            using HttpClient client = new HttpClient();

            var model = await client.DeleteAsync($"http://localhost:5301/api/Doctor?id={id}");

            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {

            using HttpClient client = new HttpClient();

            var doctor = new Doctor { Name = name};

            await client.PutAsJsonAsync($"http://localhost:5301/api/Doctor", doctor);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name)
        {

            using HttpClient client = new HttpClient();

            var doctor = new Doctor { Id = id, Name = name};

            await client.PostAsJsonAsync($"http://localhost:5301/api/Doctor", doctor);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            using HttpClient client = new HttpClient();

            var model = await client.GetFromJsonAsync<Doctor>($"http://localhost:5301/api/Doctor/GetOne?id={id}");

            return View(model);

        }
    }
}
