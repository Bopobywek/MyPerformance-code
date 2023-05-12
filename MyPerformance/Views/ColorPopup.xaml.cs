using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPerformance.Views
{
    public partial class ColorPopup : Popup
    {
        public ColorPopup() {
            InitializeComponent();
        }

        private void ColorPicker_PickedColorChanged(object sender, Color color)
        {
            ColorResult.BackgroundColor = color;
        }

        private void OnSetColorButton_Clicked(object sender, EventArgs e)
        {
            Close(ColorPicker.PickedColor);
        }
    }
}
