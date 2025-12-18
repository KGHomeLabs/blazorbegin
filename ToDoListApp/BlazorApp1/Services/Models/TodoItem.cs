namespace BlazorApp1.Services.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Description { get; set; } = String.Empty;
       
        private bool isDone;
        public bool IsDone 
        { 
            get { return isDone; }
            set
            {
                isDone = value;
                if (value && String.IsNullOrEmpty(CompletionDate))
                {
                    CompletionDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else if (!value)
                {
                    CompletionDate = String.Empty;
                }
            } 
        }
        public string CompletionDate { get; set; } = String.Empty;
    }
}
