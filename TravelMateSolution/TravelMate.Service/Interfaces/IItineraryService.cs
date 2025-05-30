using System.Collections.Generic;
using TravelMate.Core.Entities;
using TravelMate.Core.DTOs;
using TravelMate.Core.ViewModels;

namespace TravelMate.Service.Interfaces
{
	public interface IItineraryService
	{
		int AddItinerary(Itinerary itinerary);
		List<Itinerary> GetItinerariesByUserId(int userId);
		ItineraryVo GetItineraryById(int id);
		int ModifyItinerary(ItineraryUpdateVo vo);
		bool DeleteItinerary(int id);

		ItineraryDTO GetItineraryDetails(int id);
		List<int> GetEventIdsByUserId(int userId);
	}
}
