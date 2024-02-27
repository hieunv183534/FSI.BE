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

        // chuyên môn
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

        public List<string>? Collab { get; set; }

        // điểm cộng tính cách
        public List<int>? RequestPersonality { get; set; }

        // điểm cộng kĩ năng
        public List<int>? RequestSkill { get; set; }

        // mục đích tham gia fsi
        public int Purpose { get; set; }

        // lĩnh vực của ý tưởng nếu có
        public List<int>? IdeaField { get; set; }

        // lĩnh vực của dự án/ ý tưởng muốn tham gia
        public List<int>? TargetField { get; set; }

        // chuyên môn của mình
        public List<int>? Specialize { get; set; }

        // muốn tìm người chuyên môn như nào
        public List<int>? TargetSpecialize { get; set; }
    }
}
