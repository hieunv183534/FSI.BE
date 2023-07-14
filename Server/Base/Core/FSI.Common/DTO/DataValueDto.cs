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
            new DataPointDto("Công nghệ thông tin", 1, "Information Technology"),
            new DataPointDto("Y tế và chăm sóc sức khỏe", 2, "Healthcare and Medical"),
            new DataPointDto("Kinh doanh và tài chính", 3, "Business and Finance"),
            new DataPointDto("Năng lượng và môi trường", 4, "Energy and Environment"),
            new DataPointDto("Thiết bị điện tử", 5, "Electronics"),
            new DataPointDto("Giáo dục", 6, "Education"),
            new DataPointDto("Vận tải", 7, "Transportation"),
            new DataPointDto("Khách sạn và du lịch", 8, "Hospitality and Tourism"),
            new DataPointDto("Thực phẩm và nông nghiệp", 9, "Food and Agriculture"),
            new DataPointDto("Thương mại điện tử", 10, "E-commerce"),
            new DataPointDto("Công nghiệp và sản xuất", 11, "Industrial and Manufacturing")
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
            new DataPointDto("Trác nhiệm", 41, "Responsible"),
            new DataPointDto("Trạng tâm", 42, "Committed"),
            new DataPointDto("Trung thực", 43, "Honest"),
            new DataPointDto("Tập trung", 44, "Focused")
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

    }
}
