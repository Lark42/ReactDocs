using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisitorCore.Models;
using VisitorWebApi.Models;


namespace VisitorWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorController : Controller
    {
        private VisitorAppContext _context;

        public VisitorController(VisitorAppContext context)
        {
            _context = context;
        }

        [HttpPut]
        public void Put([FromBody] VisitorAddDto model)
        {
            var visitor = new Visitor
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                DoctorId = model.DoctorId,
                CreatedAt = DateTime.Now
            };
            _context.Visitors.Add(visitor);
            _context.SaveChanges();

        }


        [HttpPost]
        public void Post(VisitorEditDto visitor)
        {
            var existVisitor = _context.Visitors.FirstOrDefault(x => x.Id == visitor.Id);

            if (existVisitor != null)
            {
                existVisitor.FirstName = visitor.FirstName;
                existVisitor.LastName = visitor.LastName;
                existVisitor.Email = visitor.Email;
                existVisitor.Phone = visitor.Phone;
                existVisitor.DoctorId = visitor.DoctorId;
                existVisitor.UpdatedAt = DateTime.Now;

                _context.Visitors.Update(existVisitor);
                _context.SaveChanges();
            }

        }

        [HttpGet]
        [Route("GetOne")]
        public VisitorGetDto? Get(int id)
        {
            var visitor = _context.Visitors.Include(p => p.Doctor).FirstOrDefault(x => x.Id == id);

            if (visitor == null) return null;

            return VisitorGetDto(visitor);
        }

        [HttpPost]
        [Route("GetAll")]
        public VisitorGetAllDto GetAll([FromBody]VisitorFilterDto filter)
        {

            var query = _context.Visitors.AsQueryable();

            if (filter.FirstName != null)
            {
                query = query.Where(x => x.FirstName.Contains(filter.FirstName));
            }

            if (filter.LastName != null)
            {
                query = query.Where(x => x.LastName.Contains(filter.LastName));
            }

            if (filter.Email != null)
            {
                query = query.Where(x => x.Email.Contains(filter.Email));
            }

            if (filter.Phone != null)
            {
                query = query.Where(x => x.Phone.Contains(filter.Phone));
            }

            if (filter.DoctorId != null)
            {
                query = query.Where(x => x.DoctorId == filter.DoctorId);
            }

            var visitors = query.ToList()
                   .Select(visitor => VisitorGetDto(visitor))
                   .ToList();

            var model = new VisitorGetAllDto
            {
                Visitors = visitors,
                Doctors = _context.Doctors.ToList()
            };
        
            return model;

        }

        private VisitorGetDto VisitorGetDto(Visitor visitor)
        {
            return new VisitorGetDto
            {
                Id = visitor.Id,
                FirstName = visitor.FirstName,
                LastName = visitor.LastName,
                Email = visitor.Email,
                Phone = visitor.Phone,
                DoctorId = visitor.DoctorId,
                Doctor = visitor.Doctor,
                CreatedAt = visitor.CreatedAt,
                UpdatedAt = visitor.UpdatedAt
            };
        }

        [HttpDelete]
        public void Delete(int id)
        {
            var visitor = _context.Visitors.FirstOrDefault(x => x.Id == id);

            if (visitor != null)
            {
                _context.Visitors.Remove(visitor);
                _context.SaveChanges();
            }

        }
    }
}
