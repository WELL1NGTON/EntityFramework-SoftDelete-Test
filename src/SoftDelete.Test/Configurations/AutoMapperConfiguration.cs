using AutoMapper;

using SoftDelete.Test.Dtos;
using SoftDelete.Test.Entities;

namespace SoftDelete.Test.Configurations;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<ToDoEntryEntity, ToDoEntryDto>();
        CreateMap<UserEntity, UserDto>();
    }
}
