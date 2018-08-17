using FaceApiManager.Common;
using FaceApiManager.Pages;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace FaceApiManager.ViewModels
{
    public class EditPersonViewModel : ViewModelBase
    {
        private Person _person;

        [XamlProperty]
        public string PersonName { get; set; }

        [XamlProperty]
        public IList<Guid> PersistedFaces { get; set; }

        [XamlProperty]
        public ICommand BackCommand
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    NavigationHelper.Navigate(typeof(EditPersonGroupPage));
                });
            }
        }

        [XamlProperty]
        public ICommand DeleteFaceCommand
        {
            get
            {
                return new DelegateCommand(async (o) =>
                {

                    var faceId = (Guid)o;

                    var dlg = new MessageDialog($"Delete: {faceId}");

                    dlg.Commands.Add(new UICommand("Yes", async (h) => {
                        await App.FaceHelper.RemoveFace(App.SelectedPersonGroup.PersonGroupId, _person.PersonId, faceId);
                        await Load(_person);
                    }));

                    dlg.Commands.Add(new UICommand("No", (h) => {
                        // Do nothing
                    }));

                    await dlg.ShowAsync();
                });
            }
        }

        [XamlProperty]
        public ICommand FromFileCommand
        {
            get
            {
                return new DelegateCommand(async (o) => 
                {
                    FileOpenPicker openPicker = new FileOpenPicker();
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".jpeg");
                    openPicker.FileTypeFilter.Add(".bmp");
                    openPicker.FileTypeFilter.Add(".png");
                    
                    StorageFile file = await openPicker.PickSingleFileAsync();

                    try
                    {
                        if (file != null)
                        {
                            await App.FaceHelper.AddImageToPerson(App.SelectedPersonGroup.PersonGroupId, _person.PersonId, file);
                            await Load(_person);
                        }
                    }
                    catch (Exception ex)
                    {
                        var dlg = new MessageDialog(ex.Message);
                        await dlg.ShowAsync();
                    }

                });
            }
        }

        [XamlProperty]
        public ICommand FromClipboardCommand
        {
            get
            {
                return new DelegateCommand(async (o) => 
                {
                    try
                    {
                        var clipboardHelper = new ClipboardHelper();
                        var file = await clipboardHelper.ImageFromClipboard();
                        if (file != null)
                        {
                            await App.FaceHelper.AddImageToPerson(App.SelectedPersonGroup.PersonGroupId, _person.PersonId, file);
                            await Load(_person);
                        }
                    }
                    catch(Exception ex)
                    {
                        var dlg = new MessageDialog(ex.Message);
                        await dlg.ShowAsync();
                    }
                });
            }
        }

        public async Task Load(Person person)
        {
            _person = await App.FaceHelper.GetPerson(App.SelectedPersonGroup.PersonGroupId, person.PersonId);

            SetValue(() => PersonName, _person.Name);

            SetValue(() => PersistedFaces, _person.PersistedFaceIds);
        }
    }
}
