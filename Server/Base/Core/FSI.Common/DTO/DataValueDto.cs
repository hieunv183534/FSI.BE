using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.Common.DTO
{
    public class DataPointDto
    {
        public DataPointDto(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public int Value { get; set; }
        public string Name { get; set; }
    }

    public class FsiDataValue
    {
        public static List<DataPointDto> Fields = new List<DataPointDto> {
            new DataPointDto("Công nghệ thông tin",1),
            new DataPointDto("Y tế và chăm sóc sức khỏe",2),
            new DataPointDto("Kinh doanh và tài chính",3),
            new DataPointDto("Năng lượng và môi trường",4),
            new DataPointDto("Thiết bị điện tử",5),
            new DataPointDto("Giáo dục",6),
            new DataPointDto("Vận tải",7),
            new DataPointDto("Khách sạn và du lịch",8),
            new DataPointDto("Thực phẩm và nông nghiệp",9),
            new DataPointDto("Thương mại điện tử",10),
            new DataPointDto("Công nghiệp và sản xuất",11)
        };

        public static List<DataPointDto> Areas = new List<DataPointDto>
        {

        };

        public static List<DataPointDto> Personalities = new List<DataPointDto> {
            new DataPointDto("Cầu tiến",1),
            new DataPointDto("Cầu toàn",2),
            new DataPointDto("Chuyên nghiệp",3),
            new DataPointDto("Chăm chỉ",4),
            new DataPointDto("Chân thành",5),
            new DataPointDto("Chủ động",6),
            new DataPointDto("Chỉnh chu",7),
            new DataPointDto("Cởi mở",8),
            new DataPointDto("Dễ gần",9),
            new DataPointDto("Hài hước",10),
            new DataPointDto("Hòa đồng",11),
            new DataPointDto("Khéo léo",12),
            new DataPointDto("Kiên cường",13),
            new DataPointDto("Kỷ luật",14),
            new DataPointDto("Lạc quan",15),
            new DataPointDto("Lắng nghe",16),
            new DataPointDto("Linh hoạt",17),
            new DataPointDto("Mạnh mẽ",18),
            new DataPointDto("Nghiêm túc",19),
            new DataPointDto("Nhạy bén",20),
            new DataPointDto("Nhẫn nhịn",21),
            new DataPointDto("Nhiệt huyết",22),
            new DataPointDto("Nhiệt tình",23),
            new DataPointDto("Năng động",24),
            new DataPointDto("Năng nổ",25),
            new DataPointDto("Quyết đoán",26),
            new DataPointDto("Quyết liệt",27),
            new DataPointDto("Sáng suốt",28),
            new DataPointDto("Sáng tạo",29),
            new DataPointDto("Táo bạo",30),
            new DataPointDto("Tâm huyết",31),
            new DataPointDto("Thân thiện",32),
            new DataPointDto("Thẳng thắn",33),
            new DataPointDto("Thích ứng",34),
            new DataPointDto("Thực tế",35),
            new DataPointDto("Tích cực",36),
            new DataPointDto("Tỉ mỉ",37),
            new DataPointDto("Tự lập",38),
            new DataPointDto("Tự tin",39),
            new DataPointDto("Tốt bụng",40),
            new DataPointDto("Trác nhiệm",41),
            new DataPointDto("Trạng tâm",42),
            new DataPointDto("Trung thực",43),
            new DataPointDto("Tập trung",44)
        };

        public static List<DataPointDto> Skills = new List<DataPointDto> {
            new DataPointDto("Kỹ năng tự học",1),
            new DataPointDto("Kỹ năng giao tiếp",2),
            new DataPointDto("Kỹ năng viết",3),
            new DataPointDto("Kỹ năng nói trước đám đông",4),
            new DataPointDto("Kỹ năng thuyết trình",5),
            new DataPointDto("Kỹ năng tư duy phản biện",6),
            new DataPointDto("Kỹ năng thuyết phục",7),
            new DataPointDto("Kỹ năng đàm phán",8),
            new DataPointDto("Kỹ năng bán hàng",9),
            new DataPointDto("Kỹ năng xây dựng niềm tin",10),
            new DataPointDto("Kỹ năng trình bày vấn đề",11),
            new DataPointDto("Kỹ năng quản lý thời gian",12),
            new DataPointDto("Kỹ năng quản lý cảm xúc",13),
            new DataPointDto("Kỹ năng tập trung",14),
            new DataPointDto("Kỹ năng ra quyết định trong điều kiện khó khăn",15),
            new DataPointDto("Kỹ năng giải quyết xung đột",16),
            new DataPointDto("Kỹ năng thay đổi và thích nghi",17),
            new DataPointDto("Kỹ năng lãnh đạo",18),
            new DataPointDto("Kỹ năng truyền cảm hứng",19),
            new DataPointDto("Kỹ năng đào tạo",20),
            new DataPointDto("Kỹ năng lắng nghe",21),
            new DataPointDto("Kỹ năng đặt câu hỏi",22),
            new DataPointDto("Kỹ năng đặt mục tiêu",23),
            new DataPointDto("Kỹ năng lên kế hoạch",24),
            new DataPointDto("Kỹ năng tạo sức ảnh hưởng",25),
            new DataPointDto("Kỹ năng tư duy sáng tạo",26),
            new DataPointDto("Kỹ năng chịu áp lực ",27),
            new DataPointDto("Kỹ năng đọc",28),
            new DataPointDto("kỹ năng sắp xếp công việc",29),
            new DataPointDto("Kỹ năng làm việc độc lập",30),
            new DataPointDto("Kỹ năng làm việc nhóm",31),
            new DataPointDto("Kỹ năng quản lý tài chính",32),
            new DataPointDto("Kỹ năng lắng nghe",33),
            new DataPointDto("Kỹ năng thấu hiểu",34),
            new DataPointDto("Kỹ năng quan sát",35),
            new DataPointDto("Kỹ năng nhìn người ",36),
            new DataPointDto("Kỹ năng xây dựng  quan hệ",37),
            new DataPointDto("Kỹ năng chăm sóc, duy trì mối quan hệ",38),
            new DataPointDto("Kỹ năng xây dựng thương hiệu cá nhân",39),
            new DataPointDto("Kỹ năng tin học văn phòng",40),
            new DataPointDto("Kỹ năng giao việc",41),
            new DataPointDto("Kỹ năng tạo năng lượng tích cực",42),
            new DataPointDto("Kỹ năng gắn kết tập thể",43),
            new DataPointDto("Kỹ năng kỷ luật bản thân",44)
        };

        public static List<DataPointDto> YearOfExps = new List<DataPointDto> {
            new DataPointDto("Dưới 1 năm",1),
            new DataPointDto("1-2 năm",2),
            new DataPointDto("2-3 năm",3),
            new DataPointDto("3-5 năm",4),
            new DataPointDto("Trên 5 năm",5)
        };

        public static List<DataPointDto> AvailableTimes = new List<DataPointDto> {
            new DataPointDto("0-10 tiếng/tuần",1),
            new DataPointDto("10-20 tiếng/tuần",2),
            new DataPointDto("20-30 tiếng/tuần",3),
            new DataPointDto("30+ tiếng/tuần",4),
            new DataPointDto("Toàn thời gian",5)
        };
    }
}
