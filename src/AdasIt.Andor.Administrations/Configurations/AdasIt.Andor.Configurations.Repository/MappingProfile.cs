using AdasIt.Andor.Configurations.Domain;
using AdasIt.Andor.Configurations.Domain.Events;
using AdasIt.Andor.Configurations.Infrastructure.Config;
using AutoMapper;

namespace AdasIt.Andor.Configurations.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ConfigurationCreated, ConfigurationDto>();
        CreateMap<ConfigurationUpdated, ConfigurationDto>();
        CreateMap<ConfigurationDto, Configuration>();
    }
}
