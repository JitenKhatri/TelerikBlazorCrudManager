using BlazorImplementation.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Telerik.Blazor.Components;

namespace BlazorImplementation.Shared
{
    public partial class NavMenu
    {
        [Inject]
        public AuthenticationStateProvider? authenticationStateProvider { get; set; }

        [Inject]
        public IHttpClientFactory? HttpClientFactory { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }
        public List<NavigationItem> SelectedNode { get; set; } = new List<NavigationItem>();

        public TelerikTreeView TelerikTreeViewRef { get; set; }
        private List<NavigationItem> navigationItems = new List<NavigationItem>();
        private bool collapseNavMenu = false;
        private ClaimsPrincipal user = new ClaimsPrincipal();

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            // Fetch user roles
            user = authState.User;

            // Fetch navigation items
            await LoadNavigationItemsAsync(user);
        }

        private async Task ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;

            if (!collapseNavMenu)
            {
                // Menu is expanded, fetch navigation items
                await LoadNavigationItemsAsync(user);
            }
        }

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private async Task LoadNavigationItemsAsync(ClaimsPrincipal user)
        {
            var allItems = GetNavigationItems();

            // Filter out admin-only items if user is not admin
            if (!user.IsInRole("Admin"))
            {
                allItems = allItems.Where(item => !item.AdminOnly).ToList();
            }

            // Get root items (items without parent)
            navigationItems = allItems;
            var currentUrl = navigationManager.Uri.Split(navigationManager.BaseUri)[1];
            if(string.IsNullOrEmpty(currentUrl))
            {
                SelectedNode = navigationItems.Where(x => x.Url == "/").ToList();
                TelerikTreeViewRef.SelectedItems = SelectedNode;
                InvokeAsync(StateHasChanged); // Thread safety
            }
            else
            {
                SelectedNode = navigationItems.Where(x => x.Url == currentUrl).ToList();
                TelerikTreeViewRef.SelectedItems = SelectedNode;
                InvokeAsync(StateHasChanged); // Thread safety
            }
        }

        private async Task LoadChildren(TreeViewExpandEventArgs args)
        {
            var item = (NavigationItem)args.Item;
            if (args.Expanded && (item.Children == null || item.Children.Count() == 0))
            {
                var children = GetChildNavigationItems(item.Id);
                item.Children.AddRange(children);
                // TelerikTreeView.Rebind();
                await InvokeAsync(StateHasChanged); // Thread safety
            }
        }

        public List<NavigationItem> GetNavigationItems()
        {
            var httpClient = HttpClientFactory.CreateClient("API");
            var allItems = httpClient.GetFromJsonAsync<List<NavigationItem>>("Navigation/GetNavigationItems").GetAwaiter().GetResult();
            return allItems;
        }

        public List<NavigationItem> GetChildNavigationItems(int? parentId)
        {
            var httpClient = HttpClientFactory.CreateClient("API");
            var childrentItems = httpClient.GetFromJsonAsync<List<NavigationItem>>($"Navigation/GetChildrenNavigationItems?parentId={parentId}").GetAwaiter().GetResult();
            return childrentItems;
        }

        private void onItemClick(TreeViewItemClickEventArgs args)
        {
            var node = (NavigationItem)args.Item;
            SelectedNode.Add(node);
            InvokeAsync(StateHasChanged); // Thread safety
        }
    }
}   
