// Type: KalenderJawa.HelpPage
// Assembly: KalenderJawa, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\recovery\KalenderJawa\KalenderJawa.dll

using AdDuplex;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace KalenderJawaDay
{
  public partial class HelpPagez : PhoneApplicationPage
  {
    public HelpPagez()
    {
      InitializeComponent();
    }

    private void WebsiteHyperlink_Click(object sender, RoutedEventArgs e)
    {
      WebBrowserTask webBrowserTask = new WebBrowserTask();
      webBrowserTask.Uri = new Uri("http://www.masykur.web.id");
      webBrowserTask.Show();
    }

    private void EmailHyperlink_Click(object sender, RoutedEventArgs e)
    {
      EmailComposeTask emailComposeTask = new EmailComposeTask();
      emailComposeTask.To = "ahmad@masykur.web.id";
      emailComposeTask.Subject = "Kalender Jawa";
      emailComposeTask.Show();
    }
  }
}
