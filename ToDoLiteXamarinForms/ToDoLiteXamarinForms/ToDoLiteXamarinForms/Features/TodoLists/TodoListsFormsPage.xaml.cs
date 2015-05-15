using System;
using System.Threading.Tasks;
using ToDoLiteXamarinForms.Models;
using Couchbase.Lite;
using Xamarin.Forms;

namespace ToDoLiteXamarinForms.Features.TodoLists
{
	public partial class TodoListsFormsPage : ContentPage
	{
		public TodoListsFormsPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            this.BindingContext = App.StorageRepository.TodoLists;
            base.OnAppearing();
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            
            ((ListView)sender).SelectedItem = null; // de-select the row

            await Navigation.PushAsync(new ToDoListDetailsFormsPage { DocId = (e.Item as TodoList).Id });
        }

        public void OnButtonCountClicked(object sender, EventArgs args)
        {
            (sender as Button).Text = string.Format("Count# {0}", Manager.SharedInstance.GetDatabase(App.StorageRepository.DatabaseName).DocumentCount);
        }

        public async void ToolbarButtonActivated(object sender, object args)
        {
            await Navigation.PushAsync(new CreateListFormsPage());
        }
	}
}