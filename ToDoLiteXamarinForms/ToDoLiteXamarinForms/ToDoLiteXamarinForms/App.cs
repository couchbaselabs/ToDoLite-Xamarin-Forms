using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoLiteXamarinForms.Features.Welcome;
using ToDoLiteXamarinForms.Storage;
using Xamarin.Forms;

namespace ToDoLiteXamarinForms
{
	public class App : Application
	{
        public static StorageRepository StorageRepository { get; set; }

        public App()
        {
            MainPage = new NavigationPage(new WelcomeFormsPage());
            StorageRepository = new Storage.StorageRepository();
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
