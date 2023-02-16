using OpenAPISearchAddress.Converters;
using OpenAPISearchAddress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAPISearchAddress.APIs
{
  public class AddressSearchManager
  {
    private const string API_KEY = "devU01TX0FVVEgyMDIzMDIxMDIzMTkyNjExMzUwMTc=";
    private const string API_URL = "https://business.juso.go.kr/addrlink/addrLinkApi.do";

    private string keyword = "";
    private int currentPage = 1;
    private int countPerPage;

    private string SearchURL =>
      $"{API_URL}" +
      $"?confmKey={API_KEY}" +
      $"&currentPage={currentPage}" +
      $"&countPerPage={countPerPage}" +
      $"&keyword={keyword}" +
      $"&resultType=json";

    public AddressSearchManager(int countPerPage = 100)
    {
      this.countPerPage = countPerPage;
    }

    public async Task<bool> Search(string keyword)
    {
      this.keyword = keyword;
      this.currentPage = 1;

      string json = await RestfulAPI.GetAsync(SearchURL);
      JsonDocument jsonDoc = JsonDocument.Parse(json);
      JsonElement root = jsonDoc.RootElement;
      JsonElement result = root.GetProperty("results");
      AddressResults addrResults = JsonSerializer.Deserialize<AddressResults>(result.ToString(),
        new JsonSerializerOptions
        {
          Converters = { new AddressCommonConverter() }
        })!;
      return true;
    }
  }
}
