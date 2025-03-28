using MAUIApp.Models;
using MAUIApp.Services;
using System.Collections.ObjectModel;

namespace MAUIApp.Pages;

public partial class HomePage : ContentPage
{
	private readonly DatabaseService _databaseService;
    public ObservableCollection<TaskModel> Tasks { get; set; } = new();
    public ObservableCollection<TaskModel> FilteredTasks { get; set; } = new();

    private string searchText = string.Empty;
    private string selectedFilter = "Todas";

    public HomePage(DatabaseService databaseService)
	{
		InitializeComponent();
		_databaseService = databaseService;
		BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTasks();
    }

    private async Task LoadTasks()
    {
        try
        {
            var tasksList = await _databaseService.GetTasksAsync();
            if (tasksList != null)
            {
                Tasks.Clear();
                foreach (var task in tasksList)
                    Tasks.Add(task);

                FilterTasks();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar tareas: {ex.Message}", "OK");
        }
    }

    private void FilterTasks()
    {
        FilteredTasks.Clear();
        var filtered = Tasks.Where(t =>
            (string.IsNullOrEmpty(searchText) || t.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)) &
            (selectedFilter == "Todas" ||
            (selectedFilter == "Completadas" && t.IsCompleted) || 
            (selectedFilter == "Pendientes" && t.IsCompleted))
        );

        foreach (var task in filtered)
        {
            FilteredTasks.Add(task);
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        searchText = e.NewTextValue;
        FilterTasks();
    }

    private void OnFilterChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        selectedFilter = picker.SelectedItem?.ToString() ?? "Todas";
        FilterTasks();
    }
    
    private async void OnAddTaskClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddTaskPage(_databaseService));
	}

    private async void OnTaskTapped(object sender, EventArgs e)
    {
        if (sender is Label label && label.BindingContext is TaskModel task)
        {
            await Navigation.PushAsync(new EditTaskPage(_databaseService, task));
            await LoadTasks();
        }
    }


    private async void OnTaskCompletedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (sender is CheckBox checkBox && checkBox.BindingContext is TaskModel task)
            {
                if (task.IsCompleted == e.Value) return;

                task.IsCompleted = e.Value;
                await _databaseService.SaveTaskAsync(task);
                FilterTasks();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo identificar la tarea.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar tarea: {ex.Message}", "OK");
        }
    }
}	