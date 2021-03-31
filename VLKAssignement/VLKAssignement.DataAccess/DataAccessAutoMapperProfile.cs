using AutoMapper;

namespace VLKAssignement.DataAccess
{
    public class DataAccessAutoMapperProfile:Profile
    {
        public DataAccessAutoMapperProfile()
        {
            CreateMap<Models.Transfer, Models.Transaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())                
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                ;
        }
    }
}
