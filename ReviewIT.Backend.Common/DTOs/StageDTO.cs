using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewIT.Backend.Common.DTOs
{
    public class StageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool StageInitiated { get; set; }

        public int? StudyId { get; set; }
    }
}
