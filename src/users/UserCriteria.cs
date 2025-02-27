

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class UserCriteria
{    
    [AllowedValues("Name", "Email", "Age", "", null)]
    public string? SortBy { get; set; }

    [AllowedValues("asc", "dsc", "", null)]
    public string? SortOrder { get; set; }

    [AllowedValues("Name", "Email", "Age", "", null)]
    public string? FilterBy { get; set; }

    public string? NameOrEmailFilter { get; set; }

    public int? AgeFilter {get; set; }

    [DefaultValue(0)]
    public int PageIndex {get; set; }

    [DefaultValue(100)]
    public int UsersPerPage {get; set; }

    public UserCriteria(string? sortBy, string? sortOrder, string? filterBy, string? nameOrEmailFilter, int? ageFilter, int pageIndex, int usersPerPage) 
    {
        SortBy = sortBy;
        SortOrder = sortOrder;
        FilterBy = filterBy;
        NameOrEmailFilter = nameOrEmailFilter;
        AgeFilter = ageFilter;
        PageIndex = pageIndex;
        UsersPerPage = usersPerPage;
    }
}