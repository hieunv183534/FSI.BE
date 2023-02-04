using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPTNET.NOM.Common
{
    public interface IDropdownAppService
    {
        Task<List<DropdownItem>> GetDropdownItems();
    }
    public interface IDropdownAppService<TId> where TId : struct
    {
        Task<List<DropdownItem<TId>>> GetDropdownItems();
    }
    public interface IDropdownValueAppService
    {
        Task<List<DropdownItemValue>> GetDropdownItemValues();
    }
    public interface IDropdownValueAppService<TId> where TId : struct
    {
        Task<List<DropdownItemValue<TId>>> GetDropdownItemValues();
    }
    public interface IDropdownValueAppService<TId, TVal> where TId : struct where TVal : struct
    {
        Task<List<DropdownItemValue<TId, TVal>>> GetDropdownItemValues();
    }

    public interface IDropdownItem
    {
        string Code { get; set; }
        string Name { get; set; }
        bool Selected { get; set; }
        bool Hidden { get; set; }
    }
    public class DropdownItem : IDropdownItem
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public bool Hidden { get; set; }
    }
    public class DropdownItem<TId> : IDropdownItem where TId : struct
    {
        public TId Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public bool Hidden { get; set; }
    }
    public class DropdownItemValue : IDropdownItem
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public bool Hidden { get; set; }
    }
    public class DropdownItemValue<TVal> : IDropdownItem where TVal : struct
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public TVal Value { get; set; }
        public bool Selected { get; set; }
        public bool Hidden { get; set; }
    }
    public class DropdownItemValue<TId, TVal> : IDropdownItem where TId : struct where TVal : struct
    {
        public TId Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public TVal Value { get; set; }
        public bool Selected { get; set; }
        public bool Hidden { get; set; }
    }
}
