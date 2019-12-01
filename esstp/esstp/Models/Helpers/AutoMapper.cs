using AutoMapper;
using esstp.Models.ViewModels;


namespace esstp.Models.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<User, SignupViewModel>();
            CreateMap<SignupViewModel, User>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<RoleViewModel, Role>();
            CreateMap<Market, MarketViewModel>();
            CreateMap<MarketViewModel, Market>();
            CreateMap<Portfolio, PortfolioPageViewModel>();
            CreateMap<PortfolioPageViewModel, Portfolio>();
            CreateMap<Portfolio, PortfolioPositionViewModel>();
            CreateMap<PortfolioPositionViewModel, Portfolio>();
            CreateMap<Portfolio, SubjectModel>();
            CreateMap<SubjectModel, Portfolio>();
            CreateMap<MarketViewModel, SubjectModel>();
            CreateMap<SubjectModel, MarketViewModel>();
        }
    }
}
