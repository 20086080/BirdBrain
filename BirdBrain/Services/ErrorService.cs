using BirdBrain.Models;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Text;
using BirdBrain.Controls;

namespace BirdBrain.Services
{
    public static class ErrorService
    {
        public static async Task Show(ErrorType type)
        {
            var error = ErrorDefinitions.Get(type);

            await Application.Current.MainPage.ShowPopupAsync(
                new ErrorPopup(error));
        }
    }
}
