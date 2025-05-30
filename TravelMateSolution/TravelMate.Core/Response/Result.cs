namespace TravelMate.Core.Response
{
	public class Result
	{
		public int Code { get; set; } = 1;
		public string Msg { get; set; } = "Success";
		public object Data { get; set; }

		public static Result Success(object data)
		{
			return new Result { Data = data };
		}
	}
}
