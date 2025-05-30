using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;
using TravelMate.Core.ViewModels;

public interface IEventService
{
	List<EventVo> FindByItiID(int itiID);
	int? AddEvent(Event evt);
	int? ModifyEvent(EventUpdateDto dto);
	bool DeleteEvent(int id);
	List<int> GetEventByItiIDs(List<int> itiIDs);
}
