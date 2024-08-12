using BlazorImplementation.Models;
using BlazorImplementation.Models.FormModels;
using BlazorImplementation.Models.QueryStrings;
using BlazorImplementation.Services;
using BlazorImplementation.Shared;
using BlazorImplementation.Shared.providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace BlazorImplementation.Pages
{
    public partial class UserList
    {

        [Inject]
        public IHttpClientFactory? HttpClientFactory { get; set; }

        [Inject]
        public NotificationService? notificationService { get; set; }

        [Inject]
        public AuthenticationStateProvider? authenticationStateProvider { get; set; }

        [Inject]
        private LoaderService _LoadingService { get; set; } = new LoaderService();

        public string? CurrentUserName { get; set; }

        public TelerikGrid<UserModel> _grid { get; set; } = new TelerikGrid<UserModel>();

        public AddEditUser addUserForm { get; set; } = new AddEditUser();

        public List<string> roles = new List<string>() { "Admin", "Customer" };
        public FilterMenuTemplateContext BirthDateFilterMenuTemplateContext { get; set; } = new FilterMenuTemplateContext();

        public List<FilterLogicalOperatorDescriptor> FilterLogicalOperators { get; set; } = new List<FilterLogicalOperatorDescriptor>();

        public CompositeFilterDescriptor BirthDateCompositeFilterDescriptor
    => BirthDateFilterMenuTemplateContext.FilterDescriptor;

        public CompositeFilterDescriptor BirthDateRangeDescriptor
            => BirthDateCompositeFilterDescriptor.FilterDescriptors[0] as CompositeFilterDescriptor;
        public DateTime? BirthDateBeforeRangeDescriptor
        {
            get => (BirthDateRangeDescriptor.FilterDescriptors[1] as FilterDescriptor).Value as DateTime?;
            set
            {
                (BirthDateRangeDescriptor.FilterDescriptors[1] as FilterDescriptor).Value = value;
            }
        }

        public DateTime? BirthDateAfterRangeDescriptor
        {
            get => (BirthDateRangeDescriptor.FilterDescriptors[0] as FilterDescriptor).Value as DateTime?;
            set
            {
                (BirthDateRangeDescriptor.FilterDescriptors[0] as FilterDescriptor).Value = value;
            }
        }

        public CompositeFilterDescriptor BirthDateCurrentYearDescriptor
            => BirthDateCompositeFilterDescriptor.FilterDescriptors[1] as CompositeFilterDescriptor;

        public FilterDescriptor BirthDateStartCurrentYearDescriptor
            => BirthDateCurrentYearDescriptor.FilterDescriptors[0] as FilterDescriptor;

        public FilterDescriptor BirthDateEndCurrentYearDescriptor
            => BirthDateCurrentYearDescriptor.FilterDescriptors[1] as FilterDescriptor;

        public class FilterLogicalOperatorDescriptor
        {
            public string Text { get; set; }

            public FilterCompositionLogicalOperator Operator { get; set; } = new FilterCompositionLogicalOperator();
        }
        public bool isUserFormOpen { get; set; } = false;
        public string? dialogTitle { get; set; }
        public TelerikWindow dialogRef { get; set; } = new TelerikWindow();

        public AddEditUser AddEditUserRef { get; set; } = new AddEditUser();
        public RegisterFormModel selectedUser = new RegisterFormModel();

        public DotNetObjectReference<UserList> dotNetRef { get; set; }
        public void OpenAddUser()
        {
            selectedUser = new RegisterFormModel();
            dialogTitle = "Add User";
            isUserFormOpen = true;
        }
        public void OpenEditUser(UserModel user)
        {
            selectedUser = new RegisterFormModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Password = user.Password,
                ConfirmPassword = user.Password,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CityId = user.CityId,
                CountryId = user.CountryId
            };
            AddEditUserRef.OnCountrySelect(user.CountryId);
            dialogTitle = "Edit User";
            isUserFormOpen = true;
        }

        public void OnRowClickHandler(GridRowClickEventArgs eventArgs)
         {
            var user = eventArgs.Item as UserModel;
            selectedUser = new RegisterFormModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Password = user.Password,
                ConfirmPassword = user.Password,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CountryId = user.CountryId,
                CityId = user.CityId
            };
            AddEditUserRef.OnCountrySelect(user.CountryId);
            dialogTitle = "Edit User";
            isUserFormOpen = true;
        }

        public async Task SaveUser(RegisterFormModel user)
        {
            _LoadingService.StartLoading();
            if (user.UserId < 1)
            {
                var httpClient = HttpClientFactory.CreateClient("API");
                var response = await httpClient.PostAsJsonAsync<object>("User/register", user);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    notificationService?.ShowNotification("User Add Successfully", "Success");
                    isUserFormOpen = false;
                    _grid.Rebind();
                    _LoadingService.StopLoading();
                }
                else
                {
                    notificationService?.ShowNotification("Some Error has occurred", "error");
                    isUserFormOpen = false;
                    _LoadingService.StopLoading();
                }
            }
            else
            {
                var httpClient = HttpClientFactory.CreateClient("API");
                var response = await httpClient.PutAsJsonAsync<object>("User/update", user);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    notificationService.ShowNotification("User Updated Successfully", "Success");
                    isUserFormOpen = false;
                    _grid.Rebind();
                    _LoadingService.StopLoading();
                }
                else
                {
                    notificationService?.ShowNotification("Some Error has occurred", "error");
                    isUserFormOpen = false;
                    _LoadingService.StopLoading();
                }
            }
        }

        public void CancelUserForm()
        {
            isUserFormOpen = false;
        }

        public void CloseUserForm(WindowArgs e)
        {
            isUserFormOpen = false;
        }
        protected override async void OnInitialized()
        {
            _LoadingService.StartLoading();
            dotNetRef = DotNetObjectReference.Create(this);
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                CurrentUserName = user.Claims.Where(_ => _.Type == "email").Select(_ => _.Value).FirstOrDefault();
            }
            FilterLogicalOperators = GetFilterLogicalOperators();
            base.OnInitialized();
            _LoadingService.StopLoading();
        }

        public async ValueTask DisposeAsync()
        {
            if (dotNetRef != null)
            {
                dotNetRef.Dispose();
            }
        }

        async Task ReadItems(GridReadEventArgs args)
        {
            _LoadingService.StartLoading();
            try
            {
                var httpClient = HttpClientFactory.CreateClient("API");
                var queryString = BuildQueryString(args.Request);
                var response = httpClient.GetFromJsonAsync<List<UserModel>>($"User/GetUsers?{FlatternQueryString(queryString)}").GetAwaiter().GetResult();
                args.Data = response;
                if(response.Count() > 0)
                {
                    args.Total = response.FirstOrDefault().TotalCount;
                    _LoadingService.StopLoading();
                }
                else
                {
                    args.Total = 0;
                    _LoadingService.StopLoading();
                }
            }
            catch (Exception ex)
            {
                _LoadingService.StopLoading();
                notificationService.ShowNotification(ex.Message, "error");
            }
        }

        public UserQueryString BuildQueryString(DataSourceRequest request)
        {
            var filters = new List<FilterModel>();

            foreach (var filter in request.Filters)
            {
                if (filter is CompositeFilterDescriptor compositeFilter)
                {
                    var logicalOperator = compositeFilter.LogicalOperator == FilterCompositionLogicalOperator.And ? "AND" : "OR";
                    // Handle composite filter (e.g., AND, OR)
                    foreach (var childFilter in compositeFilter.FilterDescriptors)
                    {
                        if (childFilter is FilterDescriptor filterDescriptor)
                        {
                            AddFilter(filters, filterDescriptor, logicalOperator);
                        }
                        else if(childFilter is CompositeFilterDescriptor childCompositeFilter)
                        {
                            var childFilterLogicalOperator = childCompositeFilter.LogicalOperator == FilterCompositionLogicalOperator.And ? "AND" : "OR";
                            var count = 0;
                            foreach (var childCompFilter in childCompositeFilter.FilterDescriptors)
                            {
                                if(childCompFilter is FilterDescriptor filterDescriptor1)
                                {
                                    AddFilter(filters,filterDescriptor1, count == 0 ? "AND" :childFilterLogicalOperator);
                                    count++;
                                }
                            }
                        }
                    }
                }
                else if (filter is FilterDescriptor filterDescriptor)
                {
                    AddFilter(filters, filterDescriptor);
                }
            }

            return new UserQueryString
            {
                Filters = JsonConvert.SerializeObject(filters),
                PageIndex = request.Page,
                PageSize = request.PageSize,
                SortBy = request.Sorts.FirstOrDefault()?.Member.ToString(),
                SortOrder = request.Sorts.FirstOrDefault()?.SortDirection.ToString()
            };
        }

        public static string FlatternQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + System.Web.HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            return string.Join("&", properties.ToArray());
        }

        public void AddFilter(List<FilterModel> filters, FilterDescriptor filter, string logicalOperator = "AND")
        {
            if (filter != null)
            {
                filters.Add(new FilterModel
                {
                    Field = filter.Member,
                    Operator = filter.Operator.ToString(),
                    Value = filter.Member == "DateOfBirth" && filter.Value is DateTime date ? date.ToString("yyyy-MM-dd HH:mm:ss") : filter.Value?.ToString(),
                    LogicalOperator = logicalOperator
                });
            }
        }

        public List<string> GetFilterValues(CompositeFilterDescriptor filterDescriptor)
        {
            return filterDescriptor.FilterDescriptors.Select(f => (f as FilterDescriptor).Value?.ToString()).ToList();
        }

        public void ColumnValueChanged(bool value, string itemValue, string columnName, CompositeFilterDescriptor filterDescriptor)
        {
            bool noRoleCheckboxExist = true;
            var filter = filterDescriptor.FilterDescriptors.FirstOrDefault(f => (f as FilterDescriptor).Value?.ToString() == itemValue);
            foreach (var childFilter in filterDescriptor.FilterDescriptors)
            {
                if (childFilter is FilterDescriptor filterDescriptors)
                {
                    if(filterDescriptors.Member == "Role" && filterDescriptors.Value != null)
                    {
                        noRoleCheckboxExist = false;
                    }
                }
            }
                filterDescriptor.LogicalOperator = noRoleCheckboxExist ? FilterCompositionLogicalOperator.And : FilterCompositionLogicalOperator.Or;

            if (value && filter == null)
            {
                filterDescriptor.FilterDescriptors.Add(new FilterDescriptor(columnName, FilterOperator.IsEqualTo, itemValue));
            }
            else if (!value && filter != null)
            {
                filterDescriptor.FilterDescriptors.Remove(filter);
            }
        }

        public async Task SetPredefinedFilterAsync(FilterMenuTemplateContext filterContext)
        {
            var compositeFilterDescriptor = filterContext.FilterDescriptor;
            compositeFilterDescriptor.FilterDescriptors.Clear();
            compositeFilterDescriptor.LogicalOperator = FilterCompositionLogicalOperator.And;

            compositeFilterDescriptor.FilterDescriptors.Add(
                new FilterDescriptor()
                {
                    Member = nameof(UserModel.DateOfBirth),
                    MemberType = typeof(DateTime),
                    Operator = FilterOperator.IsGreaterThanOrEqualTo,
                    Value = new DateTime(1950, 1, 1)
                });

            compositeFilterDescriptor.FilterDescriptors.Add(
                new FilterDescriptor()
                {
                    Member = nameof(UserModel.DateOfBirth),
                    MemberType = typeof(DateTime),
                    Operator = FilterOperator.IsLessThanOrEqualTo,
                    Value = new DateTime(1960, 1, 1)
                });

            await filterContext.FilterAsync();
        }

        public void ExtendBirthDateFilterDescriptor()
        {
            if (BirthDateCompositeFilterDescriptor.FilterDescriptors.Count > 0 &&
                BirthDateCompositeFilterDescriptor.FilterDescriptors[0] is CompositeFilterDescriptor)
            {
                return;
            }

            BirthDateCompositeFilterDescriptor.FilterDescriptors.Clear();

            BirthDateCompositeFilterDescriptor.FilterDescriptors.Add(GetCompositeFilterForDateRange(null, null));
            BirthDateCompositeFilterDescriptor.FilterDescriptors.Add(GetCompositeFilterForDateRange(null, null));
        }

        public CompositeFilterDescriptor GetCompositeFilterForDateRange(DateTime? start, DateTime? end)
        {
            var firstFilter = new FilterDescriptor(nameof(UserModel.DateOfBirth), FilterOperator.IsGreaterThanOrEqualTo, start);
            var secondFilter = new FilterDescriptor(nameof(UserModel.DateOfBirth), FilterOperator.IsLessThanOrEqualTo, end);

            var filterDescriptors = new FilterDescriptorCollection();
            filterDescriptors.Add(firstFilter);
            filterDescriptors.Add(secondFilter);

            return new CompositeFilterDescriptor
            {
                FilterDescriptors = filterDescriptors,
                LogicalOperator = FilterCompositionLogicalOperator.And
            };
        }

        public List<FilterLogicalOperatorDescriptor> GetFilterLogicalOperators()
        {
            var data = new List<FilterLogicalOperatorDescriptor>();

            data.Add(new FilterLogicalOperatorDescriptor()
            {
                Text = "Or",
                Operator = FilterCompositionLogicalOperator.Or
            });

            data.Add(new FilterLogicalOperatorDescriptor()
            {
                Text = "And",
                Operator = FilterCompositionLogicalOperator.And
            });

            return data;
        }

        public void DeleteHandler(GridCommandEventArgs e)
        {
            _LoadingService.StartLoading();
            var user = (UserModel)e.Item;
            if(CurrentUserName == user.UserName)
            {
                _LoadingService.StopLoading();
                notificationService.ShowNotification("You can't delete your own account !!!", "error");
            }
            else
            {
                var httpClient = HttpClientFactory.CreateClient("API");
                var response = httpClient.DeleteAsync($"User/deleteUser/{user.UserId}").GetAwaiter().GetResult();
                notificationService.ShowNotification("User Deleted Successfully", "Success");
                _grid.Rebind();
                _LoadingService.StopLoading();
            }
        }
    }
}
