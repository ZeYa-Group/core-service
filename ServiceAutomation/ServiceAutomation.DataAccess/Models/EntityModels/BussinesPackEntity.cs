using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class BussinesPackEntity
    {
        public int Id { get; set; }
        public int TeamBonus { get; set; }
        public int LevelBonus { get; set; }
        public int DynamicBonus { get; set; }
        public int AutoBonus { get; set; }
        public decimal Travel { get; set; }
    }
}
