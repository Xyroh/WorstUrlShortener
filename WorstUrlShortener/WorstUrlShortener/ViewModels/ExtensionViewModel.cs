using System.ComponentModel;
using System.Windows.Input;
using System;
using Xamarin.Forms;

namespace WorstUrlShortener.ViewModels
{
    public class ExtensionViewModel : ShortenViewModel
    {

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

        }
    }
}
