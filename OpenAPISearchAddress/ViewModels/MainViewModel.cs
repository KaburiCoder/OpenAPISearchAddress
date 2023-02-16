using OpenAPISearchAddress.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAPISearchAddress.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    internal async void SearchAddress()
    {
      var addrSearchManager = new AddressSearchManager();
      await addrSearchManager.Search(Keyword);
    }

    public string Keyword { get; set; } = "";
  }
}
