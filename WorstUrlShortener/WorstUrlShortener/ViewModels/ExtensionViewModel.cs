using System.ComponentModel;
using System.Windows.Input;
using System;
using Xamarin.Forms;

namespace WorstUrlShortener.ViewModels
{
    public class ExtensionViewModel : ShortenViewModel
    {
        private string message;

        public string Message
        {
            get { return message; }

            set
            {
                this.SetProperty(ref this.message, value, "Message");
            }
        }

        private ICommand finishedCmd;

        public ICommand FinishedCommand
        {
            get { return this.finishedCmd; }

            set
            {
                this.SetProperty(ref this.finishedCmd, value, "FinishedCommand");
            }
        }

        public ExtensionViewModel()
        {
            this.FinishedCommand = new Command(this.OnFinishedCommandExecuted);
        }

        private void OnFinishedCommandExecuted(object state)
        {
            Message = $"Job {Environment.TickCount} has been completed!";
        }
    }
}
