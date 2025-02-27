using System.ComponentModel.DataAnnotations;

public class UserService
{
    private readonly UserRepository userRepository = new UserRepository();

    public List<User> GetUsersByCriteria(UserCriteria criteria)
    {
        List<User> users = userRepository.GetUsers(criteria.PageIndex, criteria.UsersPerPage);
        users = sortOrFilterUsers(users, criteria);

        return users;
    }

    public User GetUserById(Guid id)
    {
        return userRepository.GetUserById(id);
    }

    public void CreateUser(UserDto userDto)
    {
        userRepository.CreateUser(userDto);
    }

    public int UpdateUser(Guid id, UserDto userDto)
    {
        return userRepository.UpdateUser(id, userDto);
    }

    public int DeleteUser(Guid id)
    {
        return userRepository.DeleteUser(id);
    }

    private List<User> sortOrFilterUsers(List<User> users, UserCriteria criteria)
    {
        if (criteria.SortBy != null && criteria.SortBy != "")
        {
            users = sortUsers(users, criteria);
        }

        if (criteria.FilterBy != null && criteria.FilterBy != "")
        {
            users = filterUsers(users, criteria);
        }

        return users;
    }

    private List<User> sortUsers(List<User> users, UserCriteria criteria)
    {
        if (criteria.SortBy == "Name")
        {
            users = criteria.SortOrder == "asc" ? users : [.. users.OrderByDescending(x => x.Name)];
        }

        if (criteria.SortBy == "Email")
        {
            users = criteria.SortOrder == "asc" ? [.. users.OrderBy(x => x.Email)] : [.. users.OrderByDescending(x => x.Email)];
        }

        if (criteria.SortBy == "Age")
        {
            users = criteria.SortOrder == "asc" ? [.. users.OrderBy(x => x.DateOfBirth)] : [.. users.OrderByDescending(x => x.DateOfBirth)];
        }

        return users;
    }

    private List<User> filterUsers(List<User> users, UserCriteria criteria)
    {
        if (criteria.FilterBy != "Age")
        {
            users = criteria.FilterBy == "Name" ? [.. users.Where(user => user.Name == criteria.NameOrEmailFilter)] : [.. users.Where(user => user.Email == criteria.NameOrEmailFilter)];
        }

        if (criteria.FilterBy == "Age")
        {
            List<User> userCopy = new List<User>(users);

            users.ForEach(user =>
            {
                DateTime today = DateTime.Today;
                int age = today.Year - user.DateOfBirth.Year;

                if (age != criteria.AgeFilter)
                {
                    userCopy.Remove(user);
                }
            });

            users = userCopy;
        }

        return users;
    }
}