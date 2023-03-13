using RefuelWorkerService.Model;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace RefuelWorkerService.Services
{
	public sealed class PushOverService
	{
		private readonly SettingsCache cache;

		public PushOverService(SettingsCache cache)
		{
			this.cache = cache;
		}

		public SettingsCache Cache => cache;

		public async Task Notify(Stationrequest request, List<StationDetails> details)
		{
			 if(request.PushOver == null || string.IsNullOrEmpty(request.PushOver.ApiKey) || string.IsNullOrEmpty(request.PushOver.UserKey))
			{
				return;
			}

			var parameters = new Dictionary<string, string> {
				{ "token", request.PushOver.ApiKey },
				{ "user", request.PushOver.UserKey },
				{ "message", CreateBody(details) }
			};
			try
			{

				using (var client = new HttpClient())
				{
					await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "https://api.pushover.net/1/messages.json")
					{
						Content = new FormUrlEncodedContent(parameters)
					});
				}
			}

			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		string CreateBody(List<StationDetails> details)
		{
			var builder = new StringBuilder();
			foreach (var detail in details)
			{
				builder.AppendLine($"{detail.Station.Name} {detail.Station.Place}");
				builder.AppendLine($"{detail.Station.PostCode} {detail.Station.Street}");
				builder.AppendLine($"Diesel - {detail.Station.Diesel} / E5 - {detail.Station.E5} / E10 - {detail.Station.E10}");
				builder.AppendLine();
			}

			return builder.ToString();
		}

	}
}
