using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace KalenderJawa
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static KalenderJawa.LocalizedResources.AppResources localizedResources = new KalenderJawa.LocalizedResources.AppResources();

        public KalenderJawa.LocalizedResources.AppResources LocalizedResources { get { return localizedResources; } }

    }
}
