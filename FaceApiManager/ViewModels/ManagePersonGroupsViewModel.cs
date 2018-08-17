using FaceApiManager.Common;
using FaceApiManager.Common.FaceRoll.Common;
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
    public class ManagePersonGroupsViewModel : ViewModelBase
    {
        
        [XamlProperty]
        public IList<PersonGroup> PersonGroups { get; set; }

        [XamlProperty]
        public ICommand EditPersonGroupCommand
        {
            get
            {
                return new DelegateCommand((personGroup) => 
                {
                    App.SelectedPersonGroup = personGroup as PersonGroup;

                    NavigationHelper.Navigate(typeof(EditPersonGroupPage));
                });
            }
        }

        [XamlProperty]
        public string NewPersonGroupName { get; set; }

        [XamlProperty]
        public ICommand DeletePersonGroupCommand
        {
            get
            {
                return new DelegateCommand(async (o) => 
                {
                    var personGroup = o as PersonGroup;

                    var dlg = new MessageDialog($"Delete: {personGroup.Name}");

                    dlg.Commands.Add(new UICommand("Yes", async (h) => {
                        await App.FaceHelper.DeletePersonGroup(personGroup.PersonGroupId);
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
        public ICommand NewPersonGroupCommand
        {
            get
            {
                return new DelegateCommand(async (o) => 
                {
                    if (string.IsNullOrEmpty(NewPersonGroupName)) return;


                    try
                    {
                        await App.FaceHelper.CreatePersonGroup(NewPersonGroupName);
                        await App.FaceHelper.TrainGroup(NewPersonGroupName);

                        SetValue(() => NewPersonGroupName, string.Empty);
                        await Load();
                    }
                    catch(Exception ex)
                    {
                        var dlg = new MessageDialog(ex.Message);

                        await dlg.ShowAsync();
                    }

                });
            }
        }

        public async Task Load()
        {
            App.FaceHelper = new FaceHelper(SettingsHelper.FaceApiSubscriptionKey, SettingsHelper.FaceApiRoot);

            IList<PersonGroup> groups = await App.FaceHelper.ListGroups();

            SetValue(() => PersonGroups, groups);
        }
    }
}
