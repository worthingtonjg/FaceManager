using FaceApiManager.Common;
using FaceApiManager.Pages;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace FaceApiManager.ViewModels
{
    public class EditPersonGroupViewModel : ViewModelBase
    {
        [XamlProperty]
        public string PersonGroupName { get; set; }

        [XamlProperty]
        public IList<Person> PersonsInGroup { get; set; }

        [XamlProperty]
        public string NewPersonName { get; set; }

        [XamlProperty]
        public ICommand BackCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    NavigationHelper.Navigate(typeof(ManagePersonGroupsPage));
                });
            }
        }

        [XamlProperty]
        public ICommand EditPersonCommand
        {
            get
            {
                return new DelegateCommand((person) => {
                    NavigationHelper.Navigate(typeof(EditPersonPage), person);
                });

            }
        }

        [XamlProperty]
        public ICommand DeletePersonCommand
        {
            get
            {
                return new DelegateCommand(async (o) =>
                {
                    var person = o as Person;

                    var dlg = new MessageDialog($"Delete: {person.Name}");

                    dlg.Commands.Add(new UICommand("Yes", async (h) => {
                        await App.FaceHelper.DeletePerson(App.SelectedPersonGroup.PersonGroupId, person.PersonId);
                        await Load();
                    }));

                    dlg.Commands.Add(new UICommand("No", (h) => {
                        // Do nothing
                    }));

                    await dlg.ShowAsync();
                });
            }
        }

        [XamlProperty]
        public ICommand NewPersonCommand
        {
            get
            {
                return new DelegateCommand(async (o) =>
                {
                    if (string.IsNullOrEmpty(NewPersonName)) return;


                    try
                    {
                        await App.FaceHelper.AddPerson(App.SelectedPersonGroup.PersonGroupId, NewPersonName);
                        await App.FaceHelper.TrainGroup(App.SelectedPersonGroup.PersonGroupId);

                        SetValue(() => NewPersonName, string.Empty);
                        await Load();
                    }
                    catch (Exception ex)
                    {
                        var dlg = new MessageDialog(ex.Message);

                        await dlg.ShowAsync();
                    }

                });
            }
        }

        public async Task Load()
        {
            SetValue(() => PersonGroupName, App.SelectedPersonGroup.Name);

            var persons = await App.FaceHelper.GetPersonsInGroup(App.SelectedPersonGroup.PersonGroupId);

            SetValue(() => PersonsInGroup, persons);
        }
    }
}
