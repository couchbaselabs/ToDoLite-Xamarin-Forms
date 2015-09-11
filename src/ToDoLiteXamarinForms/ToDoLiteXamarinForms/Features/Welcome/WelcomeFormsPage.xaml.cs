using System;
using ToDoLiteXamarinForms.Models;
using Couchbase.Lite;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToDoLiteXamarinForms.Features.Login;

namespace ToDoLiteXamarinForms.Features.Welcome
{
    public partial class WelcomeFormsPage : ContentPage
    {
        public WelcomeFormsPage()
        {
            InitializeComponent();
        }

        public async void OnButtonNavigateNextClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LoginFormsPage());
        }
    }
}