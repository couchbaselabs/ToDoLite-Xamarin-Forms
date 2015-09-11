using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteXamarinForms.Models;
using Xamarin.Forms;

namespace ToDoLiteXamarinForms.Features.TodoLists
{
	public partial class CreateToDoItemFormsPage : ContentPage
	{
        public string DocId { get; set; }
        private TodoItem model;

		public CreateToDoItemFormsPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            model = new TodoItem() { TodoListId = DocId };
            this.BindingContext = model;

            base.OnAppearing();
        }

        void swithToggled(object sender, ToggledEventArgs e)
        {
        }

        public async void OnButtonCreateClicked(object sender, EventArgs args)
        {
            App.StorageRepository.Upsert(model);
            await Navigation.PopAsync();
        }

        public async void OnButtonNavigateClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }

	}
}
