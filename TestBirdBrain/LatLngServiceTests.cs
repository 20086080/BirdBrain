using BirdBrainCore.Services;

using NUnit.Framework;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace BirdBrain.Tests;

public class LatLngServiceTests
{
    [Test]
    public async Task LoadAsync_LoadsFromJsonFile()
    {
        var reader = new JsonFileReader();
        var service = new LatLngService(reader);

        await service.LoadAsync();

        Assert.That(service.LatLng, Is.Not.Null);
        Assert.That(service.LatLng, Is.Not.Empty);
    }
}

