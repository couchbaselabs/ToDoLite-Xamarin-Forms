using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteXamarinForms.Models;
using Xamarin.Forms;

namespace ToDoLiteXamarinForms.Features.TodoLists
{
	public partial class EditListFormsPage : ContentPage
	{
        public string DocId { get; set; }
        private object model;

		public EditListFormsPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            model = App.StorageRepository.Get<TodoList>(DocId);
            this.BindingContext = model;
            base.OnAppearing();
        }

        public async void OnButtonCreateClicked(object sender, EventArgs args)
        {
            App.StorageRepository.Upsert(model as TodoList);
            await Navigation.PopAsync();
        }

        public async void OnButtonNavigateClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
	}
}