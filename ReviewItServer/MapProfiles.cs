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
}
