using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TravelSite.Data.Models;

namespace TravelSite.Data.Repository
{
	public interface IPhotoRepository
	{
		Task CreatePhotoAsync(TravelPhoto photo);
		Task<List<TravelPhoto>> GetAllPhotoAsync();
		Task<TravelPhoto?> GetPhotoByIdAsync(Guid id);
		Task UpdatePhotoAsync(TravelPhoto photo);
		Task DeletePhotoAsync(Guid id);	

	}
}
