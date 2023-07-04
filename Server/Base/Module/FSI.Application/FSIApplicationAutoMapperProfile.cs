using AutoMapper;
using FSI.Application.Contracts.Auth.DTO;
using FSI.Application.Contracts.Chat.DTO;
using FSI.Application.Contracts.File;
using FSI.Application.Contracts.Project.DTO;
using FSI.Application.Contracts.Startuper.DTO;
using FSI.Application.Contracts.Test.DTO;
using FSI.Application.Contracts.User.DTO;
using FSI.Domain.Account;
using FSI.Domain.Chat;
using FSI.Domain.File;
using FSI.Domain.Project;
using FSI.Domain.Startuper;
using FSI.Domain.Test;
using FSI.Domain.User;

namespace FSI
{
    public class FSIApplicationAutoMapperProfile : Profile
    {
        public FSIApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Test, TestDto>().ReverseMap();
            CreateMap<Test, CreateTestDto>().ReverseMap();
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<ProjectUser, ProjectUserDto>().ReverseMap();
            CreateMap<ProjectFile, ProjectFileDto>().ReverseMap();
            CreateMap<ProjectEvent, ProjectEventDto>().ReverseMap();
            CreateMap<ProjectCalendarEvent, ProjectCalendarEventDto>().ReverseMap();
            CreateMap<Startuper, StartuperDto>().ReverseMap();
            CreateMap<FileInfomation, FileInfomationDto>().ReverseMap();
            CreateMap<UserRootDto, FSI.Domain.Startuper.Startuper>().ReverseMap();
            CreateMap<UserRoot, UserRootDto>().ReverseMap();
            CreateMap<Conversation, ConversationDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<UserConversation, UserConversationDto>().ReverseMap();
            CreateMap<RegisterDto, Account>().ForMember(a => a.PasswordHash , r=> r.MapFrom(src => src.Password)).ReverseMap();
        }
    }
}
