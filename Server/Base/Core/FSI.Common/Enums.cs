using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class EnumDesc
    {

        public static string RecordStatusDesc(RecordStatusCode _kts)
        {
            switch (_kts)
            {
                case RecordStatusCode.active:
                    return "Hoạt động";

                case RecordStatusCode.New:
                    return "Mới tạo";

                case RecordStatusCode.drap:
                    return "Nháp";

                case RecordStatusCode.adjust:
                    return "Điều chỉnh";

                case RecordStatusCode.rejectApproved:
                    return "Không duyệt";

                case RecordStatusCode.queueApproved:
                    return "Chờ phê duyệt";

                case RecordStatusCode.approvering:
                    return "Đang phê duyệt";

                case RecordStatusCode.approved:
                    return "Đã phê duyệt";

                case RecordStatusCode.printed:
                    return "Đã in";

                case RecordStatusCode.queueProgress:
                    return "Chờ xử lý";

                case RecordStatusCode.progressing:
                    return "Đang xử lý";

                case RecordStatusCode.progressed:
                    return "Đã xử lý";

                case RecordStatusCode.queuePublic:
                    return "Chờ công bố";

                case RecordStatusCode.publicing:
                    return "Đang công bố";

                case RecordStatusCode.publiced:
                    return "Đã công bố";

                case RecordStatusCode.done:
                    return "Hoàn tất";

                case RecordStatusCode.close:
                    return "Đã đóng";

                case RecordStatusCode.deactive:
                    return "Không hoạt động";

                case RecordStatusCode.delete:
                    return "Đã xóa";

                case RecordStatusCode.migrating:
                    return "Chưa chuẩn hóa";

                case RecordStatusCode.migrated:
                    return "Đã chuẩn hóa";

                default:
                    return "";
            }
        }

        public static string RecordStatusDesc(int _kts)
        {
            try
            {
                RecordStatusCode status = (RecordStatusCode)_kts;
                return RecordStatusDesc(status);
            }
            catch
            {
                return "";
            }
        }
    }
    public struct RegexDefine
    {
        public const string Email = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const string Year = @"^\d{4}$";
        public const string IntergerDuong = @"^\d*$";//"^0|([1-9]+[0-9]*)$";
        public const string Interger = @"^((\-\d+)|(\d*))$";//@"^0|(\-?[1-9]+[0-9]*)$";
        public const string NumericDuong = @"^(\d+(\.\d+)?)$";
        public const string Numeric = @"^(\-?\d+(\.\d+)?)$";
        public const string PhoneNumber = @"^(\+?[0-9\s\-\.]{9,15})$";
        public const string AscII = @"^([a-zA-Z\s]+)$";
        public const string Unicode = "^([\u00c0-\u020f\u1ea0-\u1ef9a-zA-Z0-9_\\-\\.\\s]*)$";
        public const string Code = @"^[a-zA-Z0-9_\-\.]+$";
        public const string CodeVN = "^[Đđa-zA-Z0-9_\\-\\.]+$";
        public const string CodeVNNosign = "^[ĂÂĐÊÔƠƯăâđêôơưa-zA-Z0-9_\\-\\.]+$";
        public const string CodeVNUnicode = "^[\u00c0-\u020fa-zA-Z0-9_\\-\\.]+$";
        public const string LstCodeVN = "^[\u00c0-\u020fa-zA-Z0-9_\\-\\.\\,]+$";
        public const string CharacterNumber = @"^[a-zA-Z0-9]+$";
        public const string CardNumber = @"^[a-zA-Z0-9_ \-\.]+$";
        public const string DateVN = @"^((((31\/(0?[13578]|1[02]))|((29|30)\/(0?[1,3-9]|1[0-2])))\/(1[6-9]|[2-9]\d)?\d{2})|(29\/0?2\/(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])\/((0?[1-9])|(1[0-2]))\/((1[6-9]|[2-9]\d)?\d{2}))$";
        public const string DateVN_Null = @"^(((((31\/(0?[13578]|1[02]))|((29|30)\/(0?[1,3-9]|1[0-2])))\/(1[6-9]|[2-9]\d)?\d{2})|(29\/0?2\/(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])\/((0?[1-9])|(1[0-2]))\/((1[6-9]|[2-9]\d)?\d{2}))|(__\/__\/____))$";
        public const string DateTimeVN = @"^((((31\/(0?[13578]|1[02]))|((29|30)\/(0?[1,3-9]|1[0-2])))\/(1[6-9]|[2-9]\d)?\d{2})|(29\/0?2\/(((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))|(0?[1-9]|1\d|2[0-8])\/((0?[1-9])|(1[0-2]))\/((1[6-9]|[2-9]\d)?\d{2}))\s([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
        public const string DateIso = @"^$";
        public const string MaSoThue = @"^([a-zA-Z0-9\s\-]*)$";
        public const string Time24 = "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
        public const string Time12VN = "^(0?[0-9]|1[0-2]):[0-5][0-9] (SA|CH)$";
        public const string Time12EN = "^(0?[0-9]|1[0-2]):[0-5][0-9] (AM|PM)$";
        public const string Guid = "^\\{?[a-fA-F\\d]{8}-([a-fA-F\\d]{4}-){3}[a-fA-F\\d]{12}\\}?$";
        public const string Month = "^(0?[1-9]|1[012])/\\d\\d\\d\\d";

    }

    public struct RegexMessage
    {
        public const string Email = "Email nhập sai định dạng";
        public const string Year = "Năm nhập sai định dạng";
        public const string Interger = "{0} phải là kiểu số nguyên.";
        public const string IntergerDuong = "{0} phải là kiểu số nguyên dương.";
        public const string Date = "Ngày không tồn tại";
        public const string PercentFromZero = "{0} phải có giá trị từ 0 đến 100.";
        public const string PercentFromOne = "{0} phải có giá trị từ 1 đến 100.";
        public const string Numeric = "{0} phải là kiểu số";
        public const string NumericDuong = "{0} phải là kiểu số và lớn hơn 0.";
        public const string PhoneNumber = "{0} sai định dạng (chỉ nhập số từ 9 đến 15 ký tự)";
        public const string AscII = "Bạn chỉ được nhập ký tự AscII";
        public const string Unicode = "Bạn chỉ được nhập chữ";
        public const string Code = "Bạn chỉ được nhập số, chữ a-z và ký tự: '.', '_', '-'";
        public const string CodeVN = "Bạn chỉ được nhập số, chữ a-z và ký tự: 'đ', '.', '_', '-'";
        public const string CodeVNNosign = "Bạn chỉ được nhập số, chữ và ký tự: '.', '_', '-'";
        public const string CodeVNUnicode = "Bạn chỉ được nhập số, chữ và ký tự: '.', '_', '-'";
        public const string LstCodeVN = "Bạn chỉ được nhập số, chữ và ký tự: '.', '_', '-', ','";
        public const string CharacterNumber = "Bạn chỉ được nhập chữ và số";
        public const string CardNumber = "Bạn chỉ được nhập số, chữ và ký tự: '.', '_', '-'";
        public const string DateVN = "Ngày phải có định dạng ngày/tháng/năm(21/11/1990)";
        public const string DateIso = "Ngày phải có định dạng năm/tháng/ngày(1990/11/21)";
        public const string MaSoThue = "{0} không đúng định dạng (chỉ nhập số, chữ, '-' và dấu cách).";
        public const string Time24 = "{0} phải nhập 00:00 - 23:59";
        public const string Time12VN = "{0} phải nhập 12:00 SA - 12:00 CH";
        public const string Time12EN = "{0} phải nhập 12:00 AM - 12:00 PM";
        public const string Month = "Tháng phải trong khoảng từ 1 đến 12";
    }
    public struct FormatType
    {
        public const string FormatDateVN = "dd/MM/yyyy";
        public const string FormatMonthVN = "MM/yyyy";
        public const string FormatDateTimeVN = "dd/MM/yyyy HH:mm";
        public const string FormatTime = "HH:mm";
        public const string Currency = "##,#.## đ";
        public const string TrongLuong = "##,#.## g";
        public const string Percent = "##,#.##\\%";
        public const string Integer = "##,#";
        public const string Number = "##,#.##";
    }

    public struct FormatModel
    {
        public const string FormatDateVN = "{0:dd/MM/yyyy}";
        public const string FormatDateTimeVN = "{0:dd/MM/yyyy HH:mm}";
        public const string FormatTime = "{0:HH:mm}";
        public const string Currency = "{0:##,#.##}";
        public const string CurrencyVND = "{0:##,#.##} đ";
        public const string TrongLuong = "{0:##,#.## g}";
        public const string Percent = "{0:##,0.##\\%}";
        public const string Integer = "{0:##,#}";
        public const string Number = "{0:##,0.##}";
    }
    public enum NOMDataType : short
    {
        TypeInt,
        TypeLong,
        TypeFloat,
        TypeDouble,
        TypeDecimal,
        TypeBool,
        TypeString,
        TypeDate,
        TypeTime,
        TypeDateTime,
        TypeDateTimeOffset,
        TypeBinary
    }

    public enum NOMBaseDataType : short
    {
        String,
        Number,
        Boolean,
        DateTime,
        DateTimeOffset,
        Date,
        Time,
        Binary
    }
    public enum NOMBinaryType : short
    {
        Image,
        File
    }

    public enum NOMControlType : short
    {
        Default,
        ComboBox,
        Tag
    }

    public enum NOMSaveType : short
    {
        Collective,
        Distributed,
    }

    public enum CategoryProperty : short
    {
        Id,
        Code,
        Name,
        Value,
        CodeName
    }
    public enum NOMStatus : short
    {
        Deactive = RecordStatusCode.deactive,
        Active = RecordStatusCode.active,
    }
    public enum RecordStatusCode : short
    {

        delete = -2,
        deactive = -1,
        active = 1,

        drap = 4,
        New = 5,
        update = 6,
        changed = 7,
        revert = 8,
        recovery = 9,
        adjust = 30,

        rejectApproved = 34,
        queueApproved = 35,
        approvering = 39,
        approved = 40,

        printed = 45,

        queueProgress = 55,
        progressing = 59,
        progressed = 60,

        queuePublic = 75,
        publicing = 79,
        publiced = 80,

        migrating = 88,
        migrated = 89,

        done = 100,
        close = 101,
        complete = 102,
    }

    public enum NOMFamilyNodeType : short
    {
        Regex,
        StoreProcedure,
    }

    public enum NOMCodeReset : short
    {
        None,
        Month,
        Year,
    }

    public enum NOMRuleObjectResultType : short
    {
        Validate,
        StepRule,
        StepWork,
        Notify,
    }

    public enum NOMRuleType : short
    {
        RuleObject,
        RuleAttribute,
    }

    public enum NOMAction : short
    {
        Insert,
        Update,
        Change,
        Delete
    }
    public enum NOMTypeValueWorkColumn : short
    {
        Decimal,
        String,
        DateTime,
        Bool
    }
    public enum NOMProcessAction : short
    {
        Plan,
        Do,
        Check,
        Act
    }

    public enum DataWorkStatus : short
    {
        New,
        Assigned,
        Doing,
        Complete
    }
    public enum TableViewDisplayType : short
    {
        Table,
        TreeTable,
    }

    public enum ObjectTableMulti : short
    {
        Single,
        Multi,
        All,
    }
    public enum NOMResponseType : short
    {
        Json,
        Xml
    }
    public enum NOMHttpMethod : short
    {
        Get,
        Post
    }
    public enum NOMFormatDate : short
    {
        DayMonthYear,
        MonthDayYear,
        YearMonthDay
    }
    public enum NOMSeparatorFormat : short
    {
        Immediately,
        StrikeThrough,
        Slash
    }
    public enum NOMFormatDateTime : short
    {
        Europe,
        ISO8601
    }
    public enum NOMJobType : short
    {
        /// <summary>
        /// fomat: mm or null
        /// </summary>
        Phut,

        /// <summary>
        /// fomat: hh:mm or null
        /// </summary>
        Gio,

        /// <summary>
        /// fomat: dd:hh:mm or null
        /// </summary>
        Ngay,

        /// <summary>
        /// fomat MM:dd:hh:mm or null
        /// </summary>
        Thang,

        /// <summary>
        /// fomat dIw:hh:mm or null -> dIw (2->8)
        /// </summary>
        Tuan,

        /// <summary>
        /// fomat dIw:hh:mm or null -> dIw (2->8)
        /// </summary>
        Custom
    }
    public enum NOMFamilyOperator : short
    {
        Equals,
        Contains
    }
    public enum NOMTableStage : short
    {
        Draft,
        Migrating,
        Migrated,
        Publish
    }
}
