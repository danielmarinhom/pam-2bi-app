using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using Azure.Storage.Blobs;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class ImagemUsuarioViewModel : BaseViewModel
    {
        private readonly UsuarioService uService;
        private const string container = "arquivos";

        private const string conexaoAzureStorage = "aaa";

        public ImagemUsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);

            FotografarCommand = new Command(async () => await Fotografar());
            SalvarImagemCommand = new Command(async () => await SalvarImagemAzure());
            AbrirGaleriaCommand = new Command(async () => await AbrirGaleria());

            _ = CarregarUsuarioAzure();
        }

        public ICommand FotografarCommand { get; }
        public ICommand SalvarImagemCommand { get; }
        public ICommand AbrirGaleriaCommand { get; }

        private ImageSource fonteImagem;
        public ImageSource FonteImagem
        {
            get => fonteImagem;
            set
            {
                fonteImagem = value;
                OnPropertyChanged();
            }
        }

        private byte[] foto;

        public byte[] Foto
        {
            get => foto;
            set
            {
                foto = value;
                OnPropertyChanged();
            }
        }

        public async Task Fotografar()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        using var sourceStream = await photo.OpenReadAsync();
                        using var ms = new MemoryStream();

                        await sourceStream.CopyToAsync(ms);
                        Foto = ms.ToArray();

                        FonteImagem = ImageSource.FromStream(() => new MemoryStream(Foto));
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        public async Task AbrirGaleria()
        {
            try
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    using var sourceStream = await photo.OpenReadAsync();
                    using var ms = new MemoryStream();

                    await sourceStream.CopyToAsync(ms);
                    Foto = ms.ToArray();

                    FonteImagem = ImageSource.FromStream(() => new MemoryStream(Foto));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        public async Task SalvarImagemAzure()
        {
            try
            {
                Usuario u = new Usuario
                {
                    Id = Preferences.Get("UsuarioId", 0),
                    Foto = Foto
                };

                string fileName = $"{u.Id}.jpg";

                var blobClient = new BlobClient(conexaoAzureStorage, container, fileName);

                if (await blobClient.ExistsAsync())
                    await blobClient.DeleteAsync();

                using var stream = new MemoryStream(u.Foto);
                await blobClient.UploadAsync(stream);

                await App.Current.MainPage.DisplayAlert("Ok", "Imagem salva com sucesso!", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        public async Task CarregarUsuarioAzure()
        {
            try
            {
                int userId = Preferences.Get("UsuarioId", 0);
                string filename = $"{userId}.jpg";

                var blobClient = new BlobClient(conexaoAzureStorage, container, filename);

                if (await blobClient.ExistsAsync())
                {
                    using var ms = new MemoryStream();
                    using var blobStream = await blobClient.OpenReadAsync();

                    await blobStream.CopyToAsync(ms);

                    Foto = ms.ToArray();
                    FonteImagem = ImageSource.FromStream(() => new MemoryStream(Foto));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}
