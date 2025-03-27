using MAUIApp.Models;
using MAUIApp.Services;

namespace MAUIApp.Pages;

public partial class HomePage : ContentPage
{
	private readonly DatabaseService _databaseService;
    public HomePage(DatabaseService databaseService)
	{
		InitializeComponent();
		_databaseService = databaseService;
		LoadTasks();
	}

    private async void LoadTasks()
	{
		TaskListView.ItemsSource = await _databaseService.GetTasksAsync();
	}

	private async void OnAddTaskClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddTaskPage(_databaseService));
	}

    private void OnTaskSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;

        var selectedTask = e.SelectedItem as TaskModel;
        DisplayAlert("Tarea seleccionada", selectedTask?.Title, "OK");

        TaskListView.SelectedItem = null;
    }
}	