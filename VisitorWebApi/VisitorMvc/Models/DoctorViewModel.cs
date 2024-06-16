using VisitorCore.Models;

namespace VisitorMvc.Models
{
    public class DoctorViewModel
    {
        public List<Doctor> Doctors { get; set; }

        public DoctorFilterDto Filter { get; set; }
    }
}
