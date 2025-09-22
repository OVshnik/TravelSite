using AutoMapper;
using TravelSite.Data.Models;
using TravelSite.Models.Account;
using TravelSite.Models.Bookings;
using TravelSite.Models.Notification;
using TravelSite.Models.Orders;
using TravelSite.Models.Roles;
using TravelSite.Models.TravelDates;
using TravelSite.Models.TravelPhoto;
using TravelSite.Models.Travels;
using TravelSite.Models.TravelVideo;

namespace TravelSite;

    public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<RegisterViewModel, User>()
			.ForMember(x => x.Email, opt => opt.MapFrom(c => c.EmailReg))
			.ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login));
		CreateMap<LoginViewModel, User>();
		CreateMap<UserEditViewModel, User>();
		CreateMap<User, UserEditViewModel>().ForMember(x=>x.UserId,opt=>opt.MapFrom(c=>c.Id));
		CreateMap<RoleViewModel, Role>();
		CreateMap<Role, RoleViewModel>();
		CreateMap<Role, CreateRoleViewModel>();
		CreateMap<CreateRoleViewModel, Role>();
		CreateMap<EditRoleViewModel, Role>();
		CreateMap<Role, EditRoleViewModel>();
		CreateMap<Booking, BookingViewModel>();
		CreateMap<BookingViewModel, Booking>();
		CreateMap<Booking, EditBookingViewModel>();
		CreateMap<Booking, CreateBookingViewModel>();
		CreateMap<CreateBookingViewModel, Booking>().ForMember(x => x.TravelDates, opt => opt.Ignore());
		CreateMap<Order,OrderViewModel>();
		CreateMap<OrderViewModel, Order>();
		CreateMap<Order, OrderViewModel>();
		CreateMap<Order, EditOrderViewModel>();
		CreateMap<CreateOrderViewModel, Order>();
		CreateMap<Travel, TravelViewModel>().ForMember(x => x.PhotoGallery, opt => opt.Ignore()).ForMember(x => x.VideoList, opt => opt.Ignore());
		CreateMap<TravelViewModel, Travel>();
		CreateMap<Travel, EditTravelViewModel>().ForMember(x => x.PhotoGallery, opt => opt.Ignore()).ForMember(x => x.VideoList, opt => opt.Ignore());
		CreateMap<CreateTravelViewModel, Travel>().ForMember(x => x.PhotoGallery, opt => opt.Ignore()).ForMember(x => x.VideoList, opt => opt.Ignore());
		CreateMap<Travel, CreateTravelViewModel>(); 
		CreateMap<CreateTravelDatesViewModel, TravelDates>();
		CreateMap<TravelDates, TravelDatesViewModel>();
		CreateMap<TravelDatesViewModel, TravelDates>();
		CreateMap<TravelDates, EditTravelDatesViewModel>();
		CreateMap<TravelPhoto, PhotoViewModel>();
		CreateMap<PhotoViewModel, TravelPhoto>();
		CreateMap<VideoViewModel, TravelVideo>();
		CreateMap<TravelVideo, VideoViewModel>();
		CreateMap<BookingNotification, NotificationViewModel>();
	}
}
