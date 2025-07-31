using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public interface ITravelRepository
	{
		Task CreateTravelAsync(Travel prod);
		Task<List<Travel>> GetAllTravelsAsync();
		Task<Travel?> GetTravelByIdAsync(Guid id);
		Task UpdateTravelAsync(Travel prod);
		Task DeleteTravelAsync(Guid id);	

	}
}
