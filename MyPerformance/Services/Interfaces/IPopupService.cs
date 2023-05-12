using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPerformance.Services.Interfaces
{
    public interface IPopupService
    {
        Task<object?> ShowPopup(Popup popup);
    }
}
