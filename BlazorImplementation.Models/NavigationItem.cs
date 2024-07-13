namespace BlazorImplementation.Models
{
    public class NavigationItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool AdminOnly { get; set; }
        public bool HasChildren { get; set; }
        public List<NavigationItem> Children { get; set; } = new List<NavigationItem>(); // Children of this navigation item
    }
}
