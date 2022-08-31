using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class ThumbnailTemplateEntity
    {
        public long Id { get; set; }
        public string ThumbnailName { get; set; }
        public string ThumbnailFullPath { get; set; }
    }
}
