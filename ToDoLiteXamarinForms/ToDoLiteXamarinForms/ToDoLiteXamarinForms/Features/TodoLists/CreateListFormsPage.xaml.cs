using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteXamarinForms.Models;
using Xamarin.Forms;

namespace ToDoLiteXamarinForms.Features.TodoLists
{
    public partial class CreateListFormsPage : ContentPage
    {
        public CreateListFormsPage()
        {
            InitializeComponent();
        }

        public async void OnButtonCreateClicked(object sender, EventArgs args)
        {
            App.StorageRepository.Upsert(
                new TodoList
                {
                    Id = entryId.Text,
                    Title = entryTitle.Text,
                    Details = entryDetails.Text
                });

            await Navigation.PopAsync();
        }

        public async void OnButtonNavigateClicked(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}