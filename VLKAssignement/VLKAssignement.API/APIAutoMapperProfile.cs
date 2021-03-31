using AutoMapper;
using VLKAssignement.API.Models;

namespace VLKAssignement.API
{
    public class APIAutoMapperProfile:Profile
    {
        public APIAutoMapperProfile()
        {            
            CreateMap<DataAccess.Models.Account, AccountModel>();
            CreateMap<AccountModel, DataAccess.Models.Account>();
            
            CreateMap<DataAccess.Models.Transfer, TransferModel>();
            CreateMap<TransferModel, DataAccess.Models.Transfer>()
                 .ForMember(dest => dest.CreatedOn, opt => opt.Ignore());

            CreateMap<DataAccess.Models.Transaction, TransactionModel>();
            CreateMap<TransactionModel, DataAccess.Models.Transaction>()
                 .ForMember(dest => dest.CreatedOn, opt => opt.Ignore());
        }

        
    }
}
