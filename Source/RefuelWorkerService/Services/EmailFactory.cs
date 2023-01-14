using System.Net.Mail;
using System.Net;
using RefuelWorkerService.Model;
using System.Text;

namespace RefuelWorkerService.Services
{
	public sealed class EmailFactory
	{
		readonly SettingsCache cache;

		public EmailFactory(SettingsCache cache)
		{
			this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
		}

		public void Send(SendParameter parameter)
		{
			if (parameter is null || parameter.Mail == null || parameter.TargetMail == null)
			{
				throw new ArgumentNullException(nameof(parameter));
			}

			var smtpClient = new SmtpClient("smtp-mail.outlook.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(parameter.Mail, parameter.Password),
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				
			};

			smtpClient.Send(parameter.Content ?? throw new InvalidOperationException());
		}

		public MailMessage Create(List<StationDetails> details, string targetMail)
		{
			var message = new MailMessage();

			message.From = new MailAddress(cache.Settings.WorkerMail);
			message.Subject = $"{DateTime.Now.ToShortTimeString()} - Preisupdate";
			message.Body = CreateBody(details);
			message.To.Add(new MailAddress(targetMail));

			return message;
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
