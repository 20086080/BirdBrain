
using BirdBrain.Models;

namespace BirdBrain.Views;

public partial class BirdSelectionPage : BasePage
{
	public BirdSelectionPage()
	{
		InitializeComponent();


        BirdCarousel.ItemsSource = new List<Bird>
        {
            new Bird { Name = "Parrot", Image = "parrot.png" },
            new Bird { Name = "Cuckoo", Image = "sparrow.png" },
            new Bird { Name = "Sparrow", Image = "parrot.png" },
            new Bird { Name = "Kingfisher", Image = "sparrow.png" }
        };

    }
}