
using BirdBrain.Models;

namespace BirdBrain.Views;

public partial class BirdSelectionPage : BasePage
{
	public BirdSelectionPage()
	{
		InitializeComponent();


        BirdCarousel.ItemsSource = new List<Bird>
        {
            new Bird { Name = "Garzette", Image = "aigrette_garzette_au_lac_sud_de_tunis_t.png" },
            new Bird { Name = "Attis", Image = "alcedo_atthis_t.png" },
            new Bird { Name = "Asian Koel", Image = "asian_koel_t.png" },
            new Bird { Name = "Black Drongo", Image = "black_drongo_in_purulia_t.png" }
        };

    }
}