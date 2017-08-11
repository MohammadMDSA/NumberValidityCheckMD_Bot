using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Bot_Application1.SSL.NumberValidator;

namespace Bot_Application1.Dialogs
{
	[Serializable]
	public class RootDialog : IDialog<object>
	{
		public Task StartAsync(IDialogContext context)
		{
			context.Wait(MessageReceivedAsync);

			return Task.CompletedTask;
		}

		private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
		{
			var activity = await result as Activity;

			// calculate something for us to return
			string input = activity.Text ?? string.Empty;
			int length = input.Length;

			var val = new NumberValidityChecker(input);

			await context.PostAsync("You entered a" + (val.Valid ? " vali" : "n invalid") + " number");
			if(val.Valid)
				await context.PostAsync("You entered " + (val.Value));
			await context.PostAsync($"{val.IntegerPart}");
			await context.PostAsync($".{val.DecimalPart}");
			await context.PostAsync($"{val.EPart}");

			// return our reply to the user
			await context.PostAsync($"You sent {activity.Text} which was {length} characters");

			context.Wait(MessageReceivedAsync);
		}
	}
}