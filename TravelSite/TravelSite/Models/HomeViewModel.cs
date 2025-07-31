using TravelSite.Models.Account;
using TravelSite.Models.Travels;

namespace TravelSite.Models
{
    public class HomeViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }=new LoginViewModel();    
        public List<TravelViewModel> Travels { get; set; }
        public HomeViewModel(List<TravelViewModel> travels)
        {
            Travels = travels;
        }   
    }
}
