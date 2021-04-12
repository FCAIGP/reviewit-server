using AutoMapper;

namespace ReviewItServer.MapProfiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Models.Company, ViewModels.CompanyView>();
            CreateMap<DTOs.CompanyDTO, Models.Company>();
        }
    }
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Models.Review, ViewModels.ReviewView>();
            CreateMap<DTOs.ReviewDTO, Models.Review>().ForSourceMember(x => x.IsAnonymous, opt => opt.DoNotValidate());
        }
    }
    public class ReplyProfile : Profile
    {
        public ReplyProfile()
        {
            CreateMap<Models.Reply, ViewModels.ReplyView>();
            CreateMap<DTOs.ReplyDTO, Models.Reply>();
        }
    }
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DTOs.UserDto, Models.User>()
                .ForMember(x => x.CurrentCompanyCompanyId, opt => opt.MapFrom(y => y.CurrentCompanyId))
                .ForSourceMember(x => x.Password, opt => opt.DoNotValidate());
        }
    }
    public class ClaimRequestProfile : Profile
    {
        public ClaimRequestProfile()
        {
            CreateMap<Models.ClaimRequest, ViewModels.ClaimRequestView>();
            CreateMap<DTOs.ClaimRequestDTO, Models.ClaimRequest>();
        }
    }

    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Models.Post, ViewModels.PostView>();
            CreateMap<DTOs.PostDTO, Models.Post>();
        }
    }
}
