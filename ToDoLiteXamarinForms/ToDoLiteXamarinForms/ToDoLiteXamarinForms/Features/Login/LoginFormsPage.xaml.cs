using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteXamarinForms.Auth;
using ToDoLiteXamarinForms.Features.TodoLists;
using Xamarin.Forms;

namespace ToDoLiteXamarinForms.Features.Login
{
	public partial class LoginFormsPage : ContentPage
	{
		public LoginFormsPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            Authentication auth = new Authentication();
            //auth.Auth.GetUI();
            base.OnAppearing();
        }

        public async void OnButtonNavigateNextClicked(object sender, EventArgs args)
        {
            // Allow username and password to be ignored in demo setup.
            /*
            if (String.IsNullOrWhiteSpace(usernameEntry.Text) || string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                return;
            }
            */

            App.StorageRepository.Username = usernameEntry.Text;
            App.StorageRepository.Password = passwordEntry.Text;

            App.StorageRepository.InitializeSync();

            await Navigation.PushAsync(new TodoListsFormsPage());
        }

        
	}
}
