
public interface IUserRepository {
    List<User> GetUsers(int pageIndex, int usersPerPage);

    User GetUserById(Guid id);

    void CreateUser(UserDto userDto);

    int UpdateUser(Guid id, UserDto userDto);

    int DeleteUser(Guid id);
}