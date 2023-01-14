using System.Net.Mail;

namespace RefuelWorkerService.Services
{
	public sealed class SendParameter
	{
		public string? Mail { get; set; }

		public string? Password { get; set; }

		public string? TargetMail { get; set; }

		public MailMessage? Content { get; set; }
	}
}
