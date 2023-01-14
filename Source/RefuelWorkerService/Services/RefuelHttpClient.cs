namespace RefuelWorkerService.Clients
{
	public sealed class RefuelHttpClient
	{
		private readonly HttpClient _httpClient = new();

		public async Task<HttpResponseMessage> Send(HttpRequestMessage message)
		{
			var result = await _httpClient.SendAsync(message);
			return result;
		}

		public HttpRequestMessage CreateMessage(string uri)
		{
			var message = new HttpRequestMessage(HttpMethod.Get, uri);
			return message;
		}
	}
}
