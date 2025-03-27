using MAUIApp.Models;
using MAUIApp.Services;

namespace MAUIApp.Pages;

public partial class EditTaskPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	private TaskModel _task;
	public EditTaskPage(DatabaseService databaseService, TaskModel task)
	{
		InitializeComponent();
		_databaseService = databaseService;
		_task = task;
		TitleEntry.Text = _task.Title;
		DescriptionEditor.Text = _task.Description;
	}

	private async void OnSaveChangesClicked(object sender, EventArgs e)
	{
		_task.Title = TitleEntry.Text;
		_task.Description = DescriptionEditor.Text;

		await _databaseService.SaveTaskAsync(_task);
		await Navigation.PopAsync();
	}

	private async void OnDeleteTaskClicked(object sender, EventArgs e)
	{
		bool confirm = await DisplayAlert("Eliminar", "¿Seguro que quieres eliminar esta tarea?", "Sí", "No");
		if (confirm)
		{
			await _databaseService.DeleteTaskAsync(_task);
			await Navigation.PopAsync();
		}
	}
}