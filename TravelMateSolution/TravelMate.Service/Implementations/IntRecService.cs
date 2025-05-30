using System;
using TravelMate.Core.DTOs;
using TravelMate.Service.Interfaces;

namespace TravelMate.Service.Implementations
{
	public class IntRecService : IIntRecService
	{
		private readonly IZhipuAIService _zhipuAIService;
		private readonly IItineraryService _itineraryService;

		public IntRecService(IZhipuAIService zhipuAIService, IItineraryService itineraryService)
		{
			_zhipuAIService = zhipuAIService;
			_itineraryService = itineraryService;
		}

		public string GetIntRec(int id)
		{
			var itineraryDetails = _itineraryService.GetItineraryDetails(id);
			var location = itineraryDetails.Location;
			var startDate = itineraryDetails.StartDate;
			var endDate = itineraryDetails.EndDate;

			try
			{
				var userMessage = string.Format(
					"我要去{0}旅行，从{1}到{2}，请根据我的预算、季节、目的地和天数推荐旅行计划，包括每一天的详细行程，不要说多余的话，直接给我计划，不要说废话，要条理一点，有序号。！",
					location, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")
				);

				var response = _zhipuAIService.InvokeChatCompletion(userMessage);
				return response;
			}
			catch (Exception ex)
			{
				return "调用 GLM-4-Flash API 出错：" + ex.Message;
			}
		}
	}
}
