using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Common.DTO
{
    public class DataPointDto
    {
        public DataPointDto(string name, int value, string? enName = "")
        {
            Name = name;
            Value = value;
            EnName = enName;
        }

        public int Value { get; set; }
        public string Name { get; set; }
        public string? EnName { get; set; }

        public static string GetSingleEnglish(List<DataPointDto> array, int value)
        {
            var rs = array.FirstOrDefault(x => x.Value == value);
            return rs.EnName;
        }

        public static string GetMultiEnglish(List<DataPointDto> array, List<int> values)
        {
            var rs = array.Where(x => values.Contains(x.Value)).Select(x => x.EnName);
            var rsString = String.Join(" , ", rs);
            return " " + rsString + " ";
        }
    }

    public class FsiDataValue
    {
        public static List<DataPointDto> Fields = new List<DataPointDto> {
            new DataPointDto("Agency (Design/Development)", 1, "Design/Development"),
            new DataPointDto("Agency (Marketing/Advertising)", 2, "Marketing/Advertising"),
            new DataPointDto("Bán lẻ - Hàng tiêu dùng - FMCG", 3, "Retail/FMCG"),
            new DataPointDto("Bảo hiểm", 4, "Insurance"),
            new DataPointDto("Bảo trì / Sửa chữa", 5, "Maintenance/Repair"),
            new DataPointDto("Bất động sản", 6, "Real Estate"),
            new DataPointDto("Chứng khoán", 7, "Securities"),
            new DataPointDto("Cơ khí", 8, "Mechanical"),
            new DataPointDto("Cơ quan nhà nước", 9, "Government"),
            new DataPointDto("Du lịch", 10, "Tourism"),
            new DataPointDto("Dược phẩm / Y tế / Công nghệ sinh học", 11, "Pharmaceuticals/Healthcare/Biotechnology"),
            new DataPointDto("Điện tử / Điện lạnh", 12, "Electronics/Refrigeration"),
            new DataPointDto("Giải trí", 13, "Entertainment"),
            new DataPointDto("Giáo dục / Đào tạo", 14, "Education/Training"),
            new DataPointDto("In ấn / Xuất bản", 15, "Printing/Publishing"),
            new DataPointDto("Internet / Online", 16, "Internet/Online"),
            new DataPointDto("IT - Phần cứng", 17, "IT - Hardware"),
            new DataPointDto("IT - Phần mềm", 18, "IT - Software"),
            new DataPointDto("Kế toán / Kiểm toán", 19, "Accounting/Auditing"),
            new DataPointDto("Khác", 20, "Other"),
            new DataPointDto("Logistics - Vận tải", 21, "Logistics/Transportation"),
            new DataPointDto("Luật", 22, "Law"),
            new DataPointDto("Marketing / Truyền thông / Quảng cáo", 23, "Marketing/Communication/Advertising"),
            new DataPointDto("Môi trường", 24, "Environment"),
            new DataPointDto("Năng lượng", 25, "Energy"),
            new DataPointDto("Ngân hàng", 26, "Banking"),
            new DataPointDto("Nhà hàng / Khách sạn", 27, "Restaurant/Hotel"),
            new DataPointDto("Nhân sự", 28, "Human Resources"),
            new DataPointDto("Nông Lâm Ngư nghiệp", 29, "Agriculture/Fishery/Forestry"),
            new DataPointDto("Sản xuất", 30, "Manufacturing"),
            new DataPointDto("Tài chính", 31, "Finance"),
            new DataPointDto("Thiết kế / kiến trúc", 32, "Design/Architecture"),
            new DataPointDto("Thời trang", 33, "Fashion"),
            new DataPointDto("Thương mại điện tử", 34, "E-commerce"),
            new DataPointDto("Tổ chức phi lợi nhuận", 35, "Non-profit Organization"),
            new DataPointDto("Tự động hóa", 36, "Automation"),
            new DataPointDto("Tư vấn", 37, "Consulting"),
            new DataPointDto("Viễn thông", 38, "Telecommunications"),
            new DataPointDto("Xây dựng", 39, "Construction"),
            new DataPointDto("Xuất nhập khẩu", 40, "Import/Export"),
        };

        public static List<DataPointDto> Jobs = new List<DataPointDto> {
            new DataPointDto("Học sinh/sinh viên", 1, "Student"),
            new DataPointDto("Giáo viên", 2, "Teacher"),
            new DataPointDto("Kỹ sư", 3, "Engineer"),
            new DataPointDto("Nhà nghiên cứu/khoa học", 4, "Researcher/Scientist"),
            new DataPointDto("Kinh doanh/buôn bán", 5, "Business/Entrepreneur"),
            new DataPointDto("Công viên chức", 6, "Civil Servant"),
            new DataPointDto("Khác", 0, "Other")
        };

        public static List<DataPointDto> Areas = new List<DataPointDto>
        {
            new DataPointDto("Bắc Giang", 1, "BacGiang"),
            new DataPointDto("Bắc Kạn", 2, "BacKan"),
            new DataPointDto("Cao Bằng", 3, "CaoBang"),
            new DataPointDto("Hà Giang", 4, "HaGiang"),
            new DataPointDto("Lạng Sơn", 5, "LangSon"),
            new DataPointDto("Phú Thọ", 6, "PhuTho"),
            new DataPointDto("Quảng Ninh", 7, "QuangNinh"),
            new DataPointDto("Thái Nguyên", 8, "ThaiNguyen"),
            new DataPointDto("Tuyên Quang", 9, "TuyenQuang"),
            new DataPointDto("Yên Bái", 10, "YenBai"),
            new DataPointDto("Điện Biên", 11, "DienBien"),
            new DataPointDto("Hòa Bình", 12, "HoaBinh"),
            new DataPointDto("Lai Châu", 13, "LaiChau"),
            new DataPointDto("Sơn La", 14, "SonLa"),
            new DataPointDto("Bắc Ninh", 15, "BacNinh"),
            new DataPointDto("Hà Nam", 16, "HaNam"),
            new DataPointDto("Hà Nội", 17, "HaNoi"),
            new DataPointDto("Hải Dương", 18, "HaiDuong"),
            new DataPointDto("Hải Phòng", 19, "HaiPhong"),
            new DataPointDto("Hưng Yên", 20, "HungYen"),
            new DataPointDto("Nam Định", 21, "NamDinh"),
            new DataPointDto("Ninh Bình", 22, "NinhBinh"),
            new DataPointDto("Thái Bình", 23, "ThaiBinh"),
            new DataPointDto("Vĩnh Phúc", 24, "VinhPhuc"),
            new DataPointDto("Hà Tĩnh", 25, "HaTinh"),
            new DataPointDto("Nghệ An", 26, "NgheAn"),
            new DataPointDto("Quảng Bình", 27, "QuangBinh"),
            new DataPointDto("Thanh Hóa", 28, "ThanhHoa"),
            new DataPointDto("Bình Định", 29, "BinhDinh"),
            new DataPointDto("Đà Nẵng", 30, "DaNang"),
            new DataPointDto("Kon Tum", 31, "KonTum"),
            new DataPointDto("Quảng Nam", 32, "QuangNam"),
            new DataPointDto("Quảng Ngãi", 33, "QuangNgai"),
            new DataPointDto("Thừa Thiên Huế", 34, "ThuaThienHue"),
            new DataPointDto("Đắk Lắk", 35, "DakLak"),
            new DataPointDto("Đắk Nông", 36, "DakNong"),
            new DataPointDto("Gia Lai", 37, "GiaLai"),
            new DataPointDto("Khánh Hòa", 38, "KhanhHoa"),
            new DataPointDto("Lâm Đồng", 39, "LamDong"),
            new DataPointDto("Ninh Thuận", 40, "NinhThuan"),
            new DataPointDto("Phú Yên", 41, "PhuYen"),
            new DataPointDto("Quảng Trị", 42, "QuangTri"),
            new DataPointDto("Đồng Nai", 43, "DongNai"),
            new DataPointDto("Bà Rịa - Vũng Tàu", 44, "BaRiaVungTau"),
            new DataPointDto("Bình Thuận", 45, "BinhThuan"),
            new DataPointDto("Đồng Tháp", 46, "DongThap"),
            new DataPointDto("Bình Dương", 47, "BinhDuong"),
            new DataPointDto("Bình Phước", 48, "BinhPhuoc"),
            new DataPointDto("Hồ Chí Minh", 49, "HoChiMinh"),
            new DataPointDto("Long An", 50, "LongAn"),
            new DataPointDto("Tiền Giang", 51, "TienGiang"),
            new DataPointDto("An Giang", 52, "AnGiang"),
            new DataPointDto("Bạc Liêu", 53, "BacLieu"),
            new DataPointDto("Bến Tre", 54, "BenTre"),
            new DataPointDto("Cà Mau", 55, "CaMau"),
            new DataPointDto("Hậu Giang", 56, "HauGiang"),
            new DataPointDto("Kiên Giang", 57, "KienGiang"),
            new DataPointDto("Sóc Trăng", 58, "SocTrang"),
            new DataPointDto("Tây Ninh", 59, "TayNinh"),
            new DataPointDto("Trà Vinh", 60, "TraVinh"),
            new DataPointDto("Vĩnh Long", 61, "VinhLong"),
            new DataPointDto("Cần Thơ", 62, "CanTho"),
            new DataPointDto("Đồng Tháp", 63, "DongThap")
        };

        public static List<DataPointDto> Personalities = new List<DataPointDto> {
            new DataPointDto("Cầu tiến", 1, "Progressive"),
            new DataPointDto("Cầu toàn", 2, "Meticulous"),
            new DataPointDto("Chuyên nghiệp", 3, "Professional"),
            new DataPointDto("Chăm chỉ", 4, "Diligent"),
            new DataPointDto("Chân thành", 5, "Sincere"),
            new DataPointDto("Chủ động", 6, "Proactive"),
            new DataPointDto("Chỉnh chu", 7, "Meticulous"),
            new DataPointDto("Cởi mở", 8, "Open-minded"),
            new DataPointDto("Dễ gần", 9, "Approachable"),
            new DataPointDto("Hài hước", 10, "Humorous"),
            new DataPointDto("Hòa đồng", 11, "Friendly"),
            new DataPointDto("Khéo léo", 12, "Skillful"),
            new DataPointDto("Kiên cường", 13, "Persistent"),
            new DataPointDto("Kỷ luật", 14, "Disciplined"),
            new DataPointDto("Lạc quan", 15, "Optimistic"),
            new DataPointDto("Lắng nghe", 16, "Attentive"),
            new DataPointDto("Linh hoạt", 17, "Flexible"),
            new DataPointDto("Mạnh mẽ", 18, "Strong"),
            new DataPointDto("Nghiêm túc", 19, "Serious"),
            new DataPointDto("Nhạy bén", 20, "Sharp"),
            new DataPointDto("Nhẫn nhịn", 21, "Patient"),
            new DataPointDto("Nhiệt huyết", 22, "Enthusiastic"),
            new DataPointDto("Nhiệt tình", 23, "Passionate"),
            new DataPointDto("Năng động", 24, "Energetic"),
            new DataPointDto("Năng nổ", 25, "Dynamite"),
            new DataPointDto("Quyết đoán", 26, "Decisive"),
            new DataPointDto("Quyết liệt", 27, "Determined"),
            new DataPointDto("Sáng suốt", 28, "Insightful"),
            new DataPointDto("Sáng tạo", 29, "Creative"),
            new DataPointDto("Táo bạo", 30, "Bold"),
            new DataPointDto("Tâm huyết", 31, "Passionate"),
            new DataPointDto("Thân thiện", 32, "Friendly"),
            new DataPointDto("Thẳng thắn", 33, "Frank"),
            new DataPointDto("Thích ứng", 34, "Adaptable"),
            new DataPointDto("Thực tế", 35, "Practical"),
            new DataPointDto("Tích cực", 36, "Positive"),
            new DataPointDto("Tỉ mỉ", 37, "Meticulous"),
            new DataPointDto("Tự lập", 38, "Independent"),
            new DataPointDto("Tự tin", 39, "Confident"),
            new DataPointDto("Tốt bụng", 40, "Kind-hearted"),
            new DataPointDto("Trách nhiệm", 41, "Responsible"),
            new DataPointDto("Trung thực", 42, "Honest"),
            new DataPointDto("Tập trung", 43, "Focused")
        };

        public static List<DataPointDto> Skills = new List<DataPointDto> {
            new DataPointDto("Kỹ năng tự học", 1, "Self-Learning"),
            new DataPointDto("Kỹ năng giao tiếp", 2, "Communication"),
            new DataPointDto("Kỹ năng viết", 3, "Writing"),
            new DataPointDto("Kỹ năng nói trước đám đông", 4, "Public Speaking"),
            new DataPointDto("Kỹ năng thuyết trình", 5, "Presentation"),
            new DataPointDto("Kỹ năng tư duy phản biện", 6, "Critical Thinking"),
            new DataPointDto("Kỹ năng thuyết phục", 7, "Persuasion"),
            new DataPointDto("Kỹ năng đàm phán", 8, "Negotiation"),
            new DataPointDto("Kỹ năng bán hàng", 9, "Sales"),
            new DataPointDto("Kỹ năng xây dựng niềm tin", 10, "Building Trust"),
            new DataPointDto("Kỹ năng trình bày vấn đề", 11, "Problem Solving"),
            new DataPointDto("Kỹ năng quản lý thời gian", 12, "Time Management"),
            new DataPointDto("Kỹ năng quản lý cảm xúc", 13, "Emotional Management"),
            new DataPointDto("Kỹ năng tập trung", 14, "Focus"),
            new DataPointDto("Kỹ năng ra quyết định trong điều kiện khó khăn", 15, "Decision Making in Difficult Conditions"),
            new DataPointDto("Kỹ năng giải quyết xung đột", 16, "Conflict Resolution"),
            new DataPointDto("Kỹ năng thay đổi và thích nghi", 17, "Adaptability"),
            new DataPointDto("Kỹ năng lãnh đạo", 18, "Leadership"),
            new DataPointDto("Kỹ năng truyền cảm hứng", 19, "Inspiration"),
            new DataPointDto("Kỹ năng đào tạo", 20, "Training"),
            new DataPointDto("Kỹ năng lắng nghe", 21, "Listening"),
            new DataPointDto("Kỹ năng đặt câu hỏi", 22, "Questioning"),
            new DataPointDto("Kỹ năng đặt mục tiêu", 23, "Goal Setting"),
            new DataPointDto("Kỹ năng lên kế hoạch", 24, "Planning"),
            new DataPointDto("Kỹ năng tạo sức ảnh hưởng", 25, "Influence"),
            new DataPointDto("Kỹ năng tư duy sáng tạo", 26, "Creative Thinking"),
            new DataPointDto("Kỹ năng chịu áp lực", 27, "Stress Management"),
            new DataPointDto("Kỹ năng đọc", 28, "Reading"),
            new DataPointDto("kỹ năng sắp xếp công việc", 29, "Organizational Skills"),
            new DataPointDto("Kỹ năng làm việc độc lập", 30, "Independence"),
            new DataPointDto("Kỹ năng làm việc nhóm", 31, "Teamwork"),
            new DataPointDto("Kỹ năng quản lý tài chính", 32, "Financial Management"),
            new DataPointDto("Kỹ năng lắng nghe", 33, "Listening"),
            new DataPointDto("Kỹ năng thấu hiểu", 34, "Empathy"),
            new DataPointDto("Kỹ năng quan sát", 35, "Observation"),
            new DataPointDto("Kỹ năng nhìn người", 36, "Perception"),
            new DataPointDto("Kỹ năng xây dựng quan hệ", 37, "Relationship Building"),
            new DataPointDto("Kỹ năng chăm sóc, duy trì mối quan hệ", 38, "Relationship Management"),
            new DataPointDto("Kỹ năng xây dựng thương hiệu cá nhân", 39, "Personal Branding"),
            new DataPointDto("Kỹ năng tin học văn phòng", 40, "Office Computer Skills"),
            new DataPointDto("Kỹ năng giao việc", 41, "Task Delegation"),
            new DataPointDto("Kỹ năng tạo năng lượng tích cực", 42, "Positive Energy Creation"),
            new DataPointDto("Kỹ năng gắn kết tập thể", 43, "Team Cohesion"),
            new DataPointDto("Kỹ năng kỷ luật bản thân", 44, "Self-Discipline")
        };

        public static List<DataPointDto> YearOfExps = new List<DataPointDto> {
            new DataPointDto("Dưới 1 năm", 1, "Less than 1 year"),
            new DataPointDto("1-2 năm", 2, "1-2 years"),
            new DataPointDto("2-3 năm", 3, "2-3 years"),
            new DataPointDto("3-5 năm", 4, "3-5 years"),
            new DataPointDto("Trên 5 năm", 5, "More than 5 years")
        };

        public static List<DataPointDto> AvailableTimes = new List<DataPointDto> {
            new DataPointDto("0-10 tiếng/tuần", 1, "0-10 hours/week"),
            new DataPointDto("10-20 tiếng/tuần", 2, "10-20 hours/week"),
            new DataPointDto("20-30 tiếng/tuần", 3, "20-30 hours/week"),
            new DataPointDto("30+ tiếng/tuần", 4, "30+ hours/week"),
            new DataPointDto("Toàn thời gian", 5, "Full-time")
        };

        public static List<DataPointDto> ProjectStages = new List<DataPointDto>
        {
            new DataPointDto("Xác lập",1),
            new DataPointDto("Nghiên cứu",2),
            new DataPointDto("MVP",3),
            new DataPointDto("Kiểm thử",4),
            new DataPointDto("Tăng trưởng 1",5),
            new DataPointDto("Tăng trưởng 2",6),
            new DataPointDto("Tăng trưởng 3",7),
            new DataPointDto("Tăng trưởng 4",8)
        };


        public static List<UniversitySpecialized> UniversitySpecializeds = new List<UniversitySpecialized>()
        {
            new UniversitySpecialized("Trường Đại học Kinh tế - Kỹ thuật Công nghiệp UNETI",
                new List<string>()
                {
                    "Ngôn ngữ Anh",
                    "Quản trị kinh doanh",
                    "Marketing",
                    "Kinh doanh thương mại",
                    "Tài chính - Ngân hàng",
                    "Bảo hiểm",
                    "Kế toán",
                    "Kiểm toán",
                    "Khoa học dữ liệu",
                    "Mạng máy tính và truyền thông dữ liệu",
                    "Công nghệ kỹ thuật máy tính",
                    "Công nghệ thông tin",
                    "Công nghệ kỹ thuật cơ khí",
                    "Công nghệ kỹ thuật cơ điện tử",
                    "Công nghệ kỹ thuật Ô tô",
                    "Công nghệ kỹ thuật điện, điện tử",
                    "Công nghệ kỹ thuật điện tử - viễn thông",
                    "CNKT điều khiển và tự động hoá",
                    "Logistics và Quản lý chuỗi cung ứng",
                    "Công nghệ dệt, may",
                    "Công nghệ sợi, dệt",
                    "Công nghệ thực phẩm",
                    "Quản trị dịch vụ du lịch và lữ hành",
                    "Quản trị khách sạn"
                })
        };

    }
}
