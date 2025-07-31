using AutoMapper;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Travels;
using TravelSite.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using TravelSite.Models.TravelPhoto;

namespace TravelSite.Services
{
	public class TravelService : ITravelService
	{
		private readonly IMapper _mapper;
		private readonly ITravelRepository _travelRepository;
		private readonly IPhotoRepository _photoRepository;
		private readonly IFileService _fileService;
		private static readonly string _upLoadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads";
		public TravelService(IMapper mapper, ITravelRepository repository, IPhotoRepository photoService, IFileService fileService)
		{
			_mapper = mapper;
			_travelRepository = repository;
			_photoRepository = photoService;
			_fileService = fileService;
		}
		public async Task AddTravelAsync(CreateTravelViewModel model)
		{
			var travel = _mapper.Map<Travel>(model);
			await _travelRepository.CreateTravelAsync(travel);
			if (model.PhotoGallery?.Count() > 0)
			{
				var tr = await _travelRepository.GetTravelByIdAsync(model.Id);
				if (tr != null)
				{
					foreach (var photo in model.PhotoGallery)
					{
						var path = _upLoadPath;

						var fileName = await _fileService.SaveFileInFolder(photo, path, model.Name);

						var phModel = new TravelPhoto()
						{
							Name = fileName,
							TravelId = tr.Id,
						};

						await _photoRepository.CreatePhotoAsync(phModel);

						tr.PhotoGallery.Add(phModel);
					}
					await _travelRepository.UpdateTravelAsync(tr);
				}
			}
		}
		public async Task<EditTravelViewModel> EditTravelAsync(Guid id)
		{
			var travel = await _travelRepository.GetTravelByIdAsync(id);
			var photoList = (await _photoRepository.GetAllPhotoAsync()).Where(x => x.TravelId == id).ToList();
			if (travel != null)
			{
				var model = _mapper.Map<EditTravelViewModel>(travel);
				if (photoList != null)
				{
					foreach (var photo in photoList)
					{
						var p = _mapper.Map<PhotoViewModel>(photo);
						p.Name = "/uploads/" + model.Name;
						model.PhotoGallery.Add(p);
					}
				}
				return _mapper.Map<EditTravelViewModel>(travel);
			}
			throw new Exception(@"Данный ""тур"" не найден в БД");
		}
		public async Task UpdateTravelAsync(EditTravelViewModel model)
		{
			var travel = await _travelRepository.GetTravelByIdAsync(model.Id);

			if (travel != null)
			{
				if (model.PhotoGallery != null)
				{
					foreach (var photo in model.PhotoGallery)
					{
						var p = _mapper.Map<TravelPhoto>(photo);
						if ((await _photoRepository.GetPhotoByIdAsync(photo.Id) != null))
						{
							travel.PhotoGallery.Add(p);
							await _photoRepository.CreatePhotoAsync(p);
						}
						else
						{
							var photoList = (await _photoRepository.GetAllPhotoAsync()).Where(x => x.TravelId == model.Id).
								Where(x => x.Id != photo.Id).ToList();
							foreach (var delPhoto in photoList)
							{
								await _photoRepository.DeletePhotoAsync(delPhoto.Id);
							}
						}
					}
				}
				var updTravel = travel.Convert(model);
				await _travelRepository.UpdateTravelAsync(updTravel);
			}
		}
		public async Task<List<TravelViewModel>> GetAllTravelAsync()
		{
			var travels = await _travelRepository.GetAllTravelsAsync();

			var listTravels = new List<TravelViewModel>();

			if (travels != null)
			{
				foreach (var travel in travels)
				{
					var model = _mapper.Map<TravelViewModel>(travel);

					var photos = (await _photoRepository.GetAllPhotoAsync()).Where(x => x.TravelId == travel.Id).ToList();
					if (photos != null)
					{
						foreach (var photo in photos)
						{
							var p = _mapper.Map<PhotoViewModel>(photo);
							p.Name = "/uploads/" + model.Name;
							model.PhotoGallery.Add(p);
						}
					}
					listTravels.Add(model);
				}
				return listTravels;
			}
			return listTravels;
		}

		public async Task<TravelViewModel> GetTravelAsync(Guid id)
		{
			var travel = await _travelRepository.GetTravelByIdAsync(id);

			if (travel != null)
			{
				var t = _mapper.Map<TravelViewModel>(travel);
				var photos = (await _photoRepository.GetAllPhotoAsync()).Where(x => x.TravelId == travel.Id).ToList();
				if (photos != null)
				{
					foreach (var photo in photos)
					{
						var model = _mapper.Map<PhotoViewModel>(photo);
						model.Name = "/uploads/" + model.Name;
						t.PhotoGallery.Add(model);
					}
				}
				return t;
			}
			throw new Exception(@$"""Тур"" с id {id} не найден");
		}

		public async Task RemoveTravelAsync(Guid id)
		{
			var travel = await _travelRepository.GetTravelByIdAsync(id);
			if (travel != null)
			{
				await _travelRepository.DeleteTravelAsync(id);
			}
		}
		public async Task<List<TravelViewModel>> CreateSearch(string search)
		{
			var model = new List<TravelViewModel>();
			if (!string.IsNullOrEmpty(search))
			{
				var travels = await GetAllTravelAsync();
				travels = travels.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
				if (travels.Count == 0)
				{
					travels = travels.Where(x => x.Category.ToLower().Contains(search.ToLower())).ToList();
				}
				foreach (var travel in travels)
				{
					model.Add(_mapper.Map<TravelViewModel>(travel));
				}
			}
			return model;
		}
	}
}
