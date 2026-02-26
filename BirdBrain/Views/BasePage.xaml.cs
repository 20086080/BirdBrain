
namespace BirdBrain.Views;

[ContentProperty(nameof(PageBody))]
public partial class BasePage : ContentPage
{
    private View? _pendingBody;
    public View PageBody
    {
        get => PageContent.Content;
        set
        {
            if (PageContent == null)
            {
                _pendingBody = value;
            }
            else
            {
                PageContent.Content = value;
            }
        }
    }
    public BasePage()
    {
        InitializeComponent();
        if (_pendingBody != null)
        {
            PageContent.Content = _pendingBody;
            _pendingBody = null;
        }
    }

    protected override void OnChildAdded(Element child)
    {
        base.OnChildAdded(child);

        if (child is View view && PageContent.Content == null)
        {
            PageContent.Content = view;
        }
    }

 // public Task OpenSettingsAsync()
//  => SettingsDrawer.OpenAsync();
}