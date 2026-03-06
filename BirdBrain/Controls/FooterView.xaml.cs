using BirdBrain.Services;

namespace BirdBrain.Controls;

public partial class FooterView : ContentView
{
	public FooterView()
	{
		InitializeComponent();
		BindingContext = App.State;
	}
}