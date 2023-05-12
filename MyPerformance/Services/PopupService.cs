using CommunityToolkit.Maui.Views;
using MyPerformance.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPerformance.Services
{
    public class PopupService : IPopupService
    {
        public async Task<object?> ShowPopup(Popup popup)
        {
            return await Shell.Current.CurrentPage.ShowPopupAsync(popup);
        }
    }
}
