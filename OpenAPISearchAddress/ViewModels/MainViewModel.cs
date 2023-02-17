using OpenAPISearchAddress.APIs;
using OpenAPISearchAddress.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace OpenAPISearchAddress.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private AddressSearchManager addrSearchManager;

    private void SearchBase(bool success)
    {
      if (success)
      {
        addrSearchManager.Details.ForEach(juso => AddressDetails.Add(juso));
      }

      if (!addrSearchManager.IsLoading && Common.ErrorCode != "0")
      {
        MessageBox.Show(Common.ErrorMessage);
      }

      OnPropertyChanged(nameof(Common));
    }

    private void OnLoadingChanged(bool isLoading)
    {
      SearchVisibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
      OnPropertyChanged(nameof(SearchVisibility));
    }

    public MainViewModel()
    {
      addrSearchManager = new AddressSearchManager();
      addrSearchManager.OnLoadingChanged += OnLoadingChanged;
      AddressDetails = new ObservableCollection<AddressDetail>();
    }

    public string Keyword { get; set; } = "";
    public ObservableCollection<AddressDetail> AddressDetails { get; set; }
    public AddressCommon Common => addrSearchManager.Common;
    public Visibility SearchVisibility { get; set; } = Visibility.Collapsed;

    internal async void SearchAddress()
    {
      AddressDetails.Clear();
      bool success = await addrSearchManager.Search(Keyword);
      SearchBase(success);
    }

    internal async Task OnScrollReachedBottom()
    {
      bool success = await addrSearchManager.SearchPage(Common.CurrentPage + 1);
      SearchBase(success);
    }
  }
}
