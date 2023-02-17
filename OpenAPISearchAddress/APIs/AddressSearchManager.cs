using OpenAPISearchAddress.Converters;
using OpenAPISearchAddress.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAPISearchAddress.APIs
{
  public class AddressSearchManager
  {
    private const string API_KEY = "devU01TX0FVVEgyMDIzMDIxODAwMjk0ODExMzUyNjM=";
    private const string API_URL = "https://business.juso.go.kr/addrlink/addrLinkApi.do";

    private string keyword = "";
    private int currentPage = 1;
    private int countPerPage;
    private AddressResults addressResults = default!;

    private string SearchURL =>
      $"{API_URL}" +
      $"?confmKey={API_KEY}" +
      $"&currentPage={currentPage}" +
      $"&countPerPage={countPerPage}" +
      $"&keyword={keyword}" +
      $"&resultType=json";

    private async Task<bool> SearchBase()
    {
      IsLoading = true;
      OnLoadingChanged?.Invoke(IsLoading);

      string json = await RestfulAPI.GetAsync(SearchURL);
      JsonDocument jsonDoc = JsonDocument.Parse(json);
      JsonElement root = jsonDoc.RootElement;
      JsonElement result = root.GetProperty("results");
      addressResults = JsonSerializer.Deserialize<AddressResults>(result.ToString(),
        new JsonSerializerOptions
        {
          Converters = { new AddressCommonConverter() }
        })!;
      IsLoading = false;
      OnLoadingChanged?.Invoke(IsLoading);

      return Common.ErrorCode == "0";
    }

    public Action<bool>? OnLoadingChanged { get; set; }
    public AddressCommon Common => addressResults?.Common!;
    public List<AddressDetail> Details => addressResults.Details;
    public bool IsLoading { get; set; } = false;

    public AddressSearchManager(int countPerPage = 100)
    {
      this.countPerPage = countPerPage;
    }

    public async Task<bool> Search(string keyword)
    {
      if (IsLoading) return false;
      this.addressResults = default!;
      this.keyword = keyword;
      this.currentPage = 1;

      return await SearchBase();
    }

    public async Task<bool> SearchPage(int page)
    {
      if (addressResults == null
        || IsLoading
        || page > Common.TotalPage) return false;

      this.currentPage = page;

      return await SearchBase();
    }
  }
}
