using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteXamarinForms.Models;
using Xamarin.Forms;

namespace ToDoLiteXamarinForms.Features.TodoLists
{
    public partial class EditToDoItemFormsPage : ContentPage
    {
        public string DocId { get; set; }
        private TodoItem model;

        public EditToDoItemFormsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            model = App.StorageRepository.Get<TodoItem>(DocId);
            this.BindingContext = model;

            base.OnAppearing();
        }

        public async void OnButtonClicked(object sender, EventArgs args)
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
