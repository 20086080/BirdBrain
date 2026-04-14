using System;
using System.Collections.Generic;
using System.Text;

namespace BirdBrain.Models
{

    public static class ErrorDefinitions
    {
        public static AppError Get(ErrorType type)
        {
            return type switch
            {
                ErrorType.GettingInformation => new AppError
                {
                    Title = "Gettiing Information",
                    Message = "Please wait...",
                    Image = "getting_information.png"
                },

                ErrorType.RefreshSuccessful => new AppError
                {
                    Title = "Successful Data Fetch",
                    Message = "Observations successfully downloaded",
                    Image = "refresh_successful.png"
                },

                ErrorType.NoInternet => new AppError
                {
                    Title = "No Internet",
                    Message = "Please check connection and try again",
                    Image = "no_internet.png"
                },

                ErrorType.LocationNotSelected => new AppError
                {
                    Title = "Location Required",
                    Message = "Select location first",
                    Image = "location_not_selected.png"
                },

                ErrorType.ApiFailure => new AppError
                {
                    Title = "API Service Error",
                    Message = "Bird data cannot be retrieved - Try later",
                    Image = "api_failure.png"
                },

                ErrorType.NoBirdsFound => new AppError
                {
                    Title = "No Observations for this Bird and Location",
                    Message = "Select another bird / location",
                    Image = "no_bird_found.png"
                },

                ErrorType.NoLocationDataFound => new AppError
                {
                    Title = "No Observations for this Location",
                    Message = "Select another location",
                    Image = "no_location_data_found.png"
                },

                ErrorType.ApiPleaseWait => new AppError
                {
                    Title = "API Service Attempt",
                    Message = "This Location has been downloaded Today",
                    Image = "api_please_wait.png"
                },

                ErrorType.BirdNotSelected => new AppError
                {
                    Title = "No Bird Selected",
                    Message = "Select a bird and try again",
                    Image = "bird_not_selected.png"
                },

                ErrorType.InvalidBird => new AppError
                {
                    Title = "Invalid Bird Entry",
                    Message = "Select a bird and try again",
                    Image = "invalid_bird.png"
                },

                ErrorType.InvalidLocation => new AppError
                {
                    Title = "Invalid Location Entry",
                    Message = "Select a location and try again",
                    Image = "invalid_location.png"
                },

                ErrorType.ErrorFound => new AppError
                {
                    Title = "Unknown Error",
                    Message = "Something broke - Try again later",
                    Image = "error_occured.png"
                },

                ErrorType.ReadytoGoSavedLocation => new AppError
                {
                    Title = "Save to Favourite",
                    Message = "Location successfully saved as Favourite",
                    Image = "ready_to_go.png"
                },


                ErrorType.ReadytoGoSavedBird => new AppError
                {
                    Title = "Save to Favourite",
                    Message = "Bird successfully saved as Favourite",
                    Image = "ready_to_go.png"
                },

                ErrorType.ReadytoGoLocationAlreadySaved => new AppError
                {
                    Title = "Good to Go",
                    Message = "Location is already a Favourite",
                    Image = "ready_to_go.png"
                },


                ErrorType.ReadytoGoBirdAlreadySaved => new AppError
                {
                    Title = "Good to Go",
                    Message = "Bird is already a Favourite",
                    Image = "ready_to_go.png"
                },
                _ => new AppError
                {
                    Title = "Unknown Error",
                    Message = "Something broke - Try again later",
                    Image = "error_occured.png"
                }
            };
        }
    }
}
