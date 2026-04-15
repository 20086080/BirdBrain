using System;
using System.Collections.Generic;
using System.Text;

namespace BirdBrain.Models
{
    public enum ErrorType
    {
        GettingInformation,
        RefreshSuccessful,
        NoInternet,
        LocationNotSelected,
        ApiFailure,
        NoBirdsFound,
        NoLocationDataFound,
        ApiPleaseWait,
        BirdNotSelected,
        InvalidLocation,
        InvalidBird,
        ReadytoGoSavedBird,
        ReadytoGoSavedLocation,
        ReadytoGoBirdAlreadySaved,
        ReadytoGoLocationAlreadySaved,
        JsonFailure,
        ErrorFound
    }
}
