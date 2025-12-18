using BlazorApp1.Services.Models;

namespace BlazorApp1.Services
{
    public static class TodoService
    {
        private static List<TodoItem> taskList { get; set; }

        static TodoService()
        {
            taskList = new List<TodoItem>
            {
                new TodoItem { Id = 1, Description = "Learn Blazor", IsDone = false, CompletionDate = "" },
                new TodoItem { Id = 2, Description = "Build a Blazor app", IsDone = false, CompletionDate = "" },
                new TodoItem { Id = 3, Description = "Deploy the app", IsDone = false, CompletionDate = "" }
            };
        }

        public static List<TodoItem> GetAllTasks()        
        {
            var sortedItems = taskList
                .OrderBy(task => task.IsDone)
                .ThenByDescending(task => task.Id)
                .ToList();
            return sortedItems;
        }

        public static void AddTask(TodoItem newTask)
        {
            var highestID = taskList.Max(t => t.Id);
            newTask.Id = highestID + 1;
            taskList.Add(newTask);
        }


        public static void UpdateTask(TodoItem updatedTask)
        {
            var existingTask = taskList.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (existingTask != null)
            {
                existingTask.Description = updatedTask.Description;
                existingTask.IsDone = updatedTask.IsDone;
                existingTask.CompletionDate = updatedTask.CompletionDate;
            }
        }
        public static void DeleteTask(int taskId)
        {
            var taskToRemove = taskList.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                taskList.Remove(taskToRemove);
            }
        }
    }
}
