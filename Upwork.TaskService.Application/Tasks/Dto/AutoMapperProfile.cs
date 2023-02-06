using AutoMapper;

namespace Upwork.TaskService.Tasks;

internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TaskEntity, TaskDto>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name))
            .ForMember(x => x.Description, x => x.MapFrom(y => y.Description))
            .ForMember(x => x.DueDate, x => x.MapFrom(y => y.DueDate))
            .ForMember(x => x.StartDate, x => x.MapFrom(y => y.StartDate))
            .ForMember(x => x.EndDate, x => x.MapFrom(y => y.EndDate))
            .ForMember(x => x.Priority, x => x.MapFrom(y => y.Priority))
            .ForMember(x => x.Status, x => x.MapFrom(y => y.Status));

        CreateMap<CreateTaskDto, TaskEntity>()
            .ForMember(x => x.Id, x => x.MapFrom(y => Guid.NewGuid()))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name.Trim()))
            .ForMember(x => x.Description, x => x.MapFrom(y => y.Description.Trim()))
            .ForMember(x => x.DueDate, x => x.MapFrom(y => y.DueDate.Date))
            .ForMember(x => x.StartDate, x => x.MapFrom(y => y.StartDate.Date))
            .ForMember(x => x.EndDate, x => x.MapFrom(y => y.EndDate.Date))
            .ForMember(x => x.Priority, x => x.MapFrom(y => y.Priority))
            .ForMember(x => x.Status, x => x.MapFrom(y => y.Status));

        CreateMap<UpdateTaskDto, TaskEntity>()
            .ForMember(x => x.Id, x => x.Ignore())
            .ForMember(x => x.Name, x => x.MapFrom(y => y.Name.Trim()))
            .ForMember(x => x.Description, x => x.MapFrom(y => y.Description.Trim()))
            .ForMember(x => x.DueDate, x => x.MapFrom(y => y.DueDate.Date))
            .ForMember(x => x.StartDate, x => x.MapFrom(y => y.StartDate.Date))
            .ForMember(x => x.EndDate, x => x.MapFrom(y => y.EndDate.Date))
            .ForMember(x => x.Priority, x => x.MapFrom(y => y.Priority))
            .ForMember(x => x.Status, x => x.MapFrom(y => y.Status));
    }
}