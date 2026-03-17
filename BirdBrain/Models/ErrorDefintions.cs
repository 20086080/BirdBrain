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
                    Title = "Gettiing Bird Sightings Information",
                    Message = "Please wait - The information is being retreived.",
                    Image = "getting_information.png"
                },

                ErrorType.RefreshSuccessful => new AppError
                {
                    Title = "Successful Data Fetch",
                    Message = "Bird Sightings have been successfully downloaded.",
                    Image = "refresh_successful.png"
                },

                ErrorType.NoInternet => new AppError
                {
                    Title = "No Internet",
                    Message = "Please check your connection and try again.",
                    Image = "no_internet.png"
                },

                ErrorType.LocationNotSelected => new AppError
                {
                    Title = "Location Required",
                    Message = "Please select a location first.",
                    Image = "location_not_selected.png"
                },

                ErrorType.ApiFailure => new AppError
                {
                    Title = "API Service Error",
                    Message = "Bird sightings cannot be retrieved - Try again later.",
                    Image = "api_failure.png"
                },

                ErrorType.NoBirdsFound => new AppError
                {
                    Title = "No Bird Sightings Found",
                    Message = "No Sightings have been found for this Bird - Select another bird.",
                    Image = "no_bird_found.png"
                },

                ErrorType.NoLocationDataFound => new AppError
                {
                    Title = "No Location Sightings Found",
                    Message = "No sightings have been found for this location - Select another location.",
                    Image = "no_location_data_found.png"
                },

                ErrorType.ApiPleaseWait => new AppError
                {
                    Title = "API Service Attempt",
                    Message = "Bird observations recently downloaded - Please wait 60 min.",
                    Image = "api_please_wait.png"
                },

                ErrorType.BirdNotSelected => new AppError
                {
                    Title = "No Bird Selected",
                    Message = "No bird has been selected - Select a bird.",
                    Image = "bird_not_selected.png"
                },

                ErrorType.InvalidBird => new AppError
                {
                    Title = "Bird not Found",
                    Message = "This bird does not exist - Select another bird.",
                    Image = "invalid_bird.png"
                },

                ErrorType.InvalidLocation => new AppError
                {
                    Title = "Location not Found",
                    Message = "This location does not exist - Select another location.",
                    Image = "invalid_location.png"
                },

                ErrorType.ErrorFound => new AppError
                {
                    Title = "Unknown Error",
                    Message = "Something went wrong - Try again little later.",
                    Image = "error_occured.png"
                },

                _ => new AppError
                {
                    Title = "Unknown Error",
                    Message = "Something went wrong - Try again little later.",
                    Image = "error_occured.png"
                }
            };
        }
    }
}
