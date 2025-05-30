namespace TravelMate.Service.Interfaces
{
	public interface IZhipuAIService
	{
		string InvokeChatCompletion(string userMessage);
	}
}
