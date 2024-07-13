using BlazorImplementation.Models;

namespace DataAPIs.Interfaces
{
    public interface INavigationService
    {
        public List<NavigationItem> GetNavigationItems();

        public List<NavigationItem> GetChildrentNavigationItems(int? parentId);
    }
}
