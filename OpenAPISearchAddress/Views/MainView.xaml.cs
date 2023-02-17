using OpenAPISearchAddress.Extensions;
using OpenAPISearchAddress.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenAPISearchAddress.Views
{
  /// <summary>
  /// MainView.xaml에 대한 상호 작용 논리
  /// </summary>
  public partial class MainView : Window
  {
    private MainViewModel ViewModel => (MainViewModel)DataContext;

    private void MainView_Loaded(object sender, RoutedEventArgs e)
    {
      var scrollViewer = lstView.FindVisualChild<ScrollViewer>();
      if (scrollViewer != null)
        scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged; ;
    }

    private async void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
      ScrollViewer scrollViewer = (ScrollViewer)sender;
      if (scrollViewer.ScrollableHeight != 0 &&
        scrollViewer.ScrollableHeight == scrollViewer.VerticalOffset)
      {
        // 스크롤이 최하단에 도착함
        await ViewModel.OnScrollReachedBottom();
      }
    }

    private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
      BindingExpression be = txtSearch.GetBindingExpression(TextBox.TextProperty);
      be.UpdateSource();
    }

    private void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        ViewModel.SearchAddress();
      }
    }

    public MainView()
    {
      InitializeComponent();
      this.Loaded += MainView_Loaded;
    }   
  }
}
