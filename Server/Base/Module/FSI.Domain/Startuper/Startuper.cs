using FSI.Domain.Account;
using FSI.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.Startuper
{
    public class Startuper : UserRoot
    {

        // lĩnh vực 
        public int? Field { get; set; }

        // chuyên môn: mô tả những gì họ làm bằng text
        public string? Speciality { get; set; }

        // tính cách
        public List<int>? Personality { get; set; }

        // kỹ năng, kỹ năng mềm
        public List<int>? Skill { get; set; }

        // kinh nghiệm làm việc
        public string? WorkingExperience { get; set; }

        // hoạt động
        public string? Activity { get; set; }

        // chứng chỉ & giải thưởng
        public string? CertificateAndAward { get; set; }

        public bool? hasProject { get; set; }

        // mô tả bản thân, slogan
        public string? Describe { get; set; }

        // số năm kinh nghiệp trong chuyên môn
        public int? YearOfExp { get; set; }

        // thời gian khả dụng ( giờ/ tuần)
        public int? AvailableTime { get; set; }

        public string? StartuperEnglishText { get; set; }

    }
}
