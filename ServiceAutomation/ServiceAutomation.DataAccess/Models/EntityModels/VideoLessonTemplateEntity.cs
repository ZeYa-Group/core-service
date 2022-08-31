using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class VideoLessonTemplateEntity
    {
        public long Id { get; set; }
        public string VideoName { get; set; }
        public string VideoFullPath { get; set; }
    }
}
