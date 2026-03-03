
using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Views;

public partial class BirdSelectionPage : BasePage
{
    private readonly JsonFileReader _jsonReader;
    public BirdSelectionPage(JsonFileReader jsonReader)
	{
		InitializeComponent();
        _jsonReader = jsonReader;


        //        BirdCarousel.ItemsSource = new List<Bird>
        //        {
        //            new Bird { Name = "Aigrette Garzette", Image = "aigrette_garzette_au_lac_sud_de_tunis_t.png" },
        //new Bird { Name = "Alcedo Attis", Image = "alcedo_atthis_t.png" },
        //new Bird { Name = "Asian Koel", Image = "asian_koel_t.png" },
        //new Bird { Name = "Black Drongo", Image = "black_drongo_in_purulia_t.png" },
        //new Bird { Name = "Brahminy Kite", Image = "brahminy_kite_t.png" },
        //new Bird { Name = "Common Myna", Image = "common_myna_t.png" },
        //new Bird { Name = "Green Bee Eater", Image = "green_bee_eater_t.png" },
        //new Bird { Name = "Grey Headed Bulbul", Image = "grey_headed_bulbul_t.png" },
        //new Bird { Name = "Halcyon", Image = "halcyon_smyrnensis_white_throated_kingfisher_t.png" },
        //new Bird { Name = "Heron", Image = "heron_garde_boeufs_a_oued_mejerda_t.png" },
        //new Bird { Name = "House Crow", Image = "house_crow_t.png" },
        //new Bird { Name = "Sparrow", Image = "house_sparrow_male_t.png" },
        //new Bird { Name = "Indian Pond Heron", Image = "indian_pond_heron_t.png" },
        //new Bird { Name = "Indian Roller", Image = "indian_roller_t.png" },
        //new Bird { Name = "Jungle Myna", Image = "jungle_myna_t.png" },
        //new Bird { Name = "Milvus Migrans", Image = "milvus_migrans_front_t.png" },
        //new Bird { Name = "Pied Kingfisher", Image = "pied_kingfisher_female_t.png" },
        //new Bird { Name = "Pycnonotus", Image = "pycnonotus_jocosus_t.png" },
        //new Bird { Name = "Red Vented Bulbul", Image = "red_vented_bulbul_t.png" },
        //new Bird { Name = "Western Ref Heron", Image = "western_reef_heron_t.png" }
        //};

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var savedLocations = await _jsonReader
            .ReadListAsync<Bird>("BirdsSeedData.json");

        BirdCarousel.ItemsSource = savedLocations;
    }
}