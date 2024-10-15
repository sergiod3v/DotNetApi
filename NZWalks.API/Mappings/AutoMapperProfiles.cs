using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles() {
            // Region Profiles
            CreateMap<RegionDto, Region>().ReverseMap();
            CreateMap<AddRegionDto, Region>().ReverseMap();
            CreateMap<UpdateRegionDto, Region>().ReverseMap();

            // Walk Profiles
            CreateMap<WalkDto, Walk>().ReverseMap();
            CreateMap<AddWalkDto, Walk>().ReverseMap();
            CreateMap<UpdateRegionDto, Walk>().ReverseMap();
        }
    }
}
