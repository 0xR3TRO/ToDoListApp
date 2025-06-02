using System.Collections.ObjectModel;
using System.Text.Json;

namespace ToDoListApp;

public partial class MainPage : ContentPage
{
    public ObservableCollection<ToDoItem> Tasks { get; set; } = new();
    private readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

    public MainPage()
    {
        InitializeComponent();
        LoadTasks();
        BindingContext = this;
    }

    private async void LoadTasks()
    {
        if (File.Exists(FilePath))
        {
            var json = await File.ReadAllTextAsync(FilePath);
            var list = JsonSerializer.Deserialize<List<ToDoItem>>(json);
            if (list != null)
                Tasks = new ObservableCollection<ToDoItem>(list);
        }

        TaskListView.ItemsSource = Tasks;
    }

    private async void SaveTasks()
    {
        var json = JsonSerializer.Serialize(Tasks.ToList());
        await File.WriteAllTextAsync(FilePath, json);
    }

    private void AddTask_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NewTaskEntry.Text))
        {
            Tasks.Add(new ToDoItem { Text = NewTaskEntry.Text, Status = TaskStatus.NotStarted });
            NewTaskEntry.Text = string.Empty;
            SaveTasks();
        }
    }

    private void DeleteTask_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ToDoItem task)
        {
            Tasks.Remove(task);
            SaveTasks();
        }
    }

    private async void EditTask_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ToDoItem task)
        {
            string result = await DisplayPromptAsync("Edytuj zadanie", "Nowy tekst:", initialValue: task.Text);

            if (!string.IsNullOrWhiteSpace(result))
            {
                task.Text = result;
                TaskListView.ItemsSource = null;
                TaskListView.ItemsSource = Tasks;
                SaveTasks();
            }
        }
    }

    private void ToggleStatus(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = sender as CheckBox;
        if (checkbox?.BindingContext is ToDoItem task)
        {
            task.Status = e.Value ? TaskStatus.Completed : TaskStatus.NotStarted;
            SaveTasks();
        }
    }

    private void SortByDate_Clicked(object sender, EventArgs e)
    {
        var sorted = Tasks.OrderBy(t => t.CreatedAt).ToList();
        Tasks = new ObservableCollection<ToDoItem>(sorted);
        TaskListView.ItemsSource = Tasks;
    }

    private void SortAlphabetically_Clicked(object sender, EventArgs e)
    {
        var sorted = Tasks.OrderBy(t => t.Text).ToList();
        Tasks = new ObservableCollection<ToDoItem>(sorted);
        TaskListView.ItemsSource = Tasks;
    }

    private void ResetList_Clicked(object sender, EventArgs e)
    {
        Tasks.Clear();
        SaveTasks();
    }
}

public class ToDoItem
{
    public string Text { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum TaskStatus
{
    NotStarted,
    InProgress,
    Completed
}
