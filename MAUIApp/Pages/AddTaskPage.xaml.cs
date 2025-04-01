using MAUIApp.Models;
using MAUIApp.Services;
using System.Collections.ObjectModel;

namespace MAUIApp.Pages;

public partial class AddTaskPage : ContentPage
{
	private readonly DatabaseService _databaseService;

	public ObservableCollection<string> Priorities { get; set; } = new()
	{
		"Baja", "Media", "Alta"
	};
	public AddTaskPage(DatabaseService databaseService)
	{
		InitializeComponent();
		_databaseService = databaseService;
		BindingContext = this;
	}

	private async void OnSaveTaskClicked(object sender, EventArgs e)
	{
		var newTask = new TaskModel
		{
			Title = TitleEntry.Text,
			Description = DescriptionEditor.Text,
			IsCompleted = false,
            Priority = PriorityPicker.SelectedItem?.ToString() ?? "Media",
        };

		await _databaseService.SaveTaskAsync(newTask);
		await Navigation.PopAsync();
	}
}