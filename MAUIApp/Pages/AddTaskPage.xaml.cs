using MAUIApp.Models;
using MAUIApp.Services;

namespace MAUIApp.Pages;

public partial class AddTaskPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	public AddTaskPage(DatabaseService databaseService)
	{
		InitializeComponent();
		_databaseService = databaseService;
	}

	private async void OnSaveTaskClicked(object sender, EventArgs e)
	{
		var newTask = new TaskModel
		{
			Title = TitleEntry.Text,
			Description = DescriptionEditor.Text,
			IsCompleted = false,
		};

		await _databaseService.SaveTaskAsync(newTask);
		await Navigation.PopAsync();
	}
}