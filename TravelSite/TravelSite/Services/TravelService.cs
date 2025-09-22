using AutoMapper;
using TravelSite.Data.Models;
using TravelSite.Data.Repository;
using TravelSite.Models.Travels;
using TravelSite.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using TravelSite.Models.TravelPhoto;
using System.IO;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.IdentityModel.Tokens;
using static GleamTech.ImageUltimate.ExifTag;
using TravelSite.Models.TravelVideo;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TravelSite.Services
{
	public class TravelService : ITravelService
	{
		private readonly IMapper _mapper;
		private readonly ITravelRepository _travelRepository;
		private readonly IPhotoRepository _photoRepository;
		private readonly IVideoRepository _videoRepository;
		private readonly IFileService _fileService;
		private static readonly string _photoUpLoadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\photo";
		private static readonly string _videoUpLoadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\uploads\\video";
		public TravelService(IMapper mapper, ITravelRepository repository, IPhotoRepository photoService, IFileService fileService, IVideoRepository videoRepository)
		{
			_mapper = mapper;
			_travelRepository = repository;
			_photoRepository = photoService;
			_fileService = fileService;
			_videoRepository = videoRepository;
		}
		public async Task AddTravelAsync(CreateTravelViewModel model)
		{
			var travel = _mapper.Map<Travel>(model);

			if (model.Photo != null)
			{
				var phName = await _fileService.SaveFileInFolder(model.Photo, _photoUpLoadPath, model.Name);
				travel.Photo = phName;
			}
			await _travelRepository.CreateTravelAsync(travel);

			var tr = await _travelRepository.GetTravelByIdAsync(model.Id);

			if (model.PhotoGallery?.Count() > 0)
			{

				if (tr != null)
				{
					foreach (var photo in model.PhotoGallery)
					{
						var fileName = await _fileService.SaveFileInFolder(photo, _photoUpLoadPath, model.Name);

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
			if (model.VideoList?.Count() > 0)
			{
				if (tr != null)
				{
					foreach (var video in model.VideoList)
					{
						var fileName = await _fileService.SaveFileInFolder(video, _videoUpLoadPath, model.Name);

						var videoModel = new TravelVideo()
						{
							Name = fileName,
							TravelId = tr.Id,
						};

						await _videoRepository.CreateVideoAsync(videoModel);

						tr.VideoList.Add(videoModel);
					}
					await _travelRepository.UpdateTravelAsync(tr);
				}
			}
		}
		public async Task<EditTravelViewModel> EditTravelAsync(Guid id)
		{
			var travel = await _travelRepository.GetTravelByIdAsync(id);
			var photoList = (await _photoRepository.GetAllPhotoAsync()).Where(x => x.TravelId == id).ToList();
			var videoList = (await _videoRepository.GetAllVideoAsync()).Where(x => x.TravelId == id).ToList();
			if (travel != null)
			{
				var model = _mapper.Map<EditTravelViewModel>(travel);
				model.Photo = "\\uploads\\photo\\"+travel.Photo;
				if (photoList != null)
				{
					foreach (var photo in photoList)
					{
						var p = _mapper.Map<PhotoViewModel>(photo);
						p.Name = "\\uploads\\photo\\"+photo.Name;
						model.PhotoGallery.Add(p);
					}
				}
				if (videoList != null)
				{
					foreach (var video in videoList)
					{
						var v = _mapper.Map<VideoViewModel>(video);
						v.Name = "\\uploads\\video\\" + video.Name;
						model.VideoList.Add(v);
					}
				}
				return model;
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
						if (photo.IsChecked == true)
						{
							var delPhoto = await _photoRepository.GetPhotoByIdAsync(photo.Id);
							if (delPhoto != null)
							{
								await _photoRepository.DeletePhotoAsync(delPhoto.Id);
								_fileService.DeleteFileInFolder(_photoUpLoadPath, delPhoto.Name);
							}
						}
					}
				}
				if (model.PhotoFiles != null)
				{
					foreach (var photo in model.PhotoFiles)
					{
						var fileName = await _fileService.SaveFileInFolder(photo, _photoUpLoadPath, model.Name);

						var phModel = new TravelPhoto()
						{
							Name = fileName,
							TravelId = travel.Id,
						};
						await _photoRepository.CreatePhotoAsync(phModel);

						travel.PhotoGallery.Add(phModel);
					}
				}
				if (model.VideoList != null)
				{
					foreach (var video in model.VideoList)
					{
						if (video.IsChecked == true)
						{
							var delVideo = await _videoRepository.GetVideoByIdAsync(video.Id);
							if (delVideo != null)
							{
								await _videoRepository.DeleteVideoAsync(delVideo.Id);
								_fileService.DeleteFileInFolder(_photoUpLoadPath, delVideo.Name);
							}
						}
					}
				}
				if (model.VideoFiles != null)
				{
					foreach (var video in model.VideoFiles)
					{
						var fileName = await _fileService.SaveFileInFolder(video, _videoUpLoadPath, model.Name);

						var videoModel = new TravelVideo()
						{
							Name = fileName,
							TravelId = travel.Id,
						};
						await _videoRepository.CreateVideoAsync(videoModel);

						travel.VideoList.Add(videoModel);
					}
				}
				if (model.EditPhoto != null)
				{
					var fileName = await _fileService.SaveFileInFolder(model.EditPhoto, _photoUpLoadPath, model.Name);
					model.Photo = fileName;
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

					model.Photo = "/uploads/photo/" + travel.Photo;

					var photos = (await _photoRepository.GetAllPhotoAsync()).Where(x => x.TravelId == travel.Id).ToList();
					if (photos != null)
					{
						foreach (var photo in photos)
						{
							var p = _mapper.Map<PhotoViewModel>(photo);
							p.Name = "/uploads/photo/" + photo.Name;
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
				var videos = (await _videoRepository.GetAllVideoAsync()).Where(x => x.TravelId == travel.Id).ToList();
				if (photos != null)
				{
					foreach (var photo in photos)
					{
						var model = _mapper.Map<PhotoViewModel>(photo);
						model.Name = "/uploads/photo/" + photo.Name;
						t.PhotoGallery.Add(model);
					}
				}
				if (videos != null)
				{
					foreach (var video in videos)
					{
						var model = _mapper.Map<VideoViewModel>(video);
						model.Name = "/uploads/video/" + video.Name;
						t.VideoList.Add(model);
					}
				}
				t.Photo = "/uploads/photo/"+t.Photo;
				return t;
			}
			throw new Exception(@$"""Тур"" с id {id} не найден");
		}

		public async Task RemoveTravelAsync(Guid id)
		{
			var travel = await _travelRepository.GetTravelByIdAsync(id);
			if (travel != null)
			{
				var allPhoto = await _photoRepository.GetAllPhotoAsync();
				var allVideo = await _videoRepository.GetAllVideoAsync();

				var delPhoto = allPhoto.Where(x => x.TravelId == id);
				var delVideo = allVideo.Where(x => x.TravelId == id);

				await _travelRepository.DeleteTravelAsync(id);

				foreach (var photo in delPhoto)
				{
					_fileService.DeleteFileInFolder(_photoUpLoadPath, photo.Name);
				}
				foreach (var video in delPhoto)
				{
					_fileService.DeleteFileInFolder(_videoUpLoadPath, video.Name);
				}
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
