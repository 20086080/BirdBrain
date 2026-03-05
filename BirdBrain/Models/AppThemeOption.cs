using BirdBrain.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace BirdBrain.Models
{
    public class AppThemeOption
    {
        public string Name { get; set; }
        public Type ThemeType { get; set; }

        public Color PreviewPrimary { get; set; }
        public Color PreviewAccent { get; set; }
        public Color PreviewBackground { get; set; }

        public Color PreviewSurface {  get; set; }

        public Color PreviewTextPrimary { get; set; }

        public Color PrimaryTextOnPrimary { get; set; }

    }
}
