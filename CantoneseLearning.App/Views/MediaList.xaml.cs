using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.App.Views;

public partial class MediaList : ContentPage
{
	private IEnumerable<V_CantoneseMedia> medias = null;

	public MediaList(IEnumerable<V_CantoneseMedia> medias)
	{
		InitializeComponent();

		this.medias = medias;

		this.Init();
	}

	private void Init()
	{
		this.lvMedias.ItemsSource = this.medias;
	}
}