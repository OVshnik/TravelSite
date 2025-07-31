using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public interface ITravelDatesRepository
	{
		Task CreateTravelDatesAsync(TravelDates dates);
		Task<List<TravelDates>> GetAllTravelDatesAsync();
		Task <TravelDates?> GetTravelDatesByIdAsync(Guid id);
		Task UpdateTravelDatesAsync(TravelDates dates);
		Task DeleteTravelDatesAsync(Guid id);	
	}
}
