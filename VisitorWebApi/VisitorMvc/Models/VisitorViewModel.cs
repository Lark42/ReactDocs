using VisitorCore.Models;

namespace VisitorMvc.Models
{
    public class VisitorViewModel
    {
        public List<VisitorGetDto> Visitors { get; set; }

        public List<Doctor> Doctors { get; set; }

        public VisitorFilterDto Filter { get; set; }
    }
}
