using System.Windows;
using System.Windows.Media;

namespace OpenAPISearchAddress.Extensions
{
  public static class FindVisualChildExtension
  {
    public static T? FindVisualChild<T>(this DependencyObject parent) where T : DependencyObject
    {
      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
      {
        DependencyObject child = VisualTreeHelper.GetChild(parent, i);
        if (child is T)
        {
          return (T)child;
        }
        T? childOfChild = FindVisualChild<T>(child);
        if (childOfChild != null)
        {
          return childOfChild;
        }
      }
      return null;
    }
  }
}
