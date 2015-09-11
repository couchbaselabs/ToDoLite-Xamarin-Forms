using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteXamarinForms.Models;
using Xamarin.Forms;
using ToDoLiteXamarinForms.Helpers;
using ToDoLiteXamarinForms.ViewModels;

namespace ToDoLiteXamarinForms.Features.TodoLists
{
    public partial class ToDoListDetailsFormsPage : ContentPage
    {
        public string DocId { get; set; }


        private TodoDetailsViewModel viewModel;

        public ToDoListDetailsFormsPage()
        {
            InitializeComponent();
            viewModel = new TodoDetailsViewModel();
        }

        protected override void OnAppearing()
        {
            App.StorageRepository.TodoLists.CollectionChanged += TodoLists_CollectionChanged;
            viewModel.Item = App.StorageRepository.Get<TodoList>(DocId);
            viewModel.Items = App.StorageRepository.TodoItems[DocId];
            this.BindingContext = viewModel;

            base.OnAppearing();
        }

        void TodoLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            viewModel.Item = App.StorageRepository.Get<TodoList>(DocId);
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event

            ((ListView)sender).SelectedItem = null; // de-select the row

            await Navigation.PushAsync(new EditToDoItemFormsPage { DocId = (e.Item as TodoItem).Id });
        }

        public async void OnButtonEditClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new EditListFormsPage { DocId = (viewModel.Item as TodoList).Id });
        }

        public async void OnButtonAddClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CreateToDoItemFormsPage { DocId = (viewModel.Item as TodoList).Id });
        }
    }
}
