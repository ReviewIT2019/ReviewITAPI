using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewIT.Backend.Common.DTOs
{
    public class StageNoIdDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool StageInitiated { get; set; }

        public int? StudyId { get; set; }
    }
}
