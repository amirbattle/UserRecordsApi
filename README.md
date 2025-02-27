# UserRecordsApi
*Description:* Small REST API in C# that performs operations on user records, ensuring performance, security, and maintainability.

# How to Use
Once you run the app, you can test the endpoints at [http://localhost:5095/swagger/index.html](http://localhost:5095/swagger/index.html).

## Login
To use the endpoints, you must login first. Otherwise the endpoints will throw a 401 Unauthorized error code. The /api/Account/login endpoint requires a username and a password. The database is seeded with a user for initial login: 

username: `admin@gmail.com` password `admin123`

Once you login, the Response Body will have an `accessToken`. Copy this token and then click on the Authorize button on the top right of the page. Enter the access token into the value field and click Authorize. You should now be logged in and have access to all endpoints. The access token expires after 30 minutes. You will have to re-login and get a new access token to regain access to the endpoints. 

## Get /users Endpoint
The Get /users endpoint (/api/User/users) was made into a Post method because the data retrieved from the database is manipulated. The data is paginated and can be sorted and filtered by Name, Email, or Age. The Requeset Body fields mimic how the UI would most likely send the data for sorting, filtering, and pagination. 

### How to sort and filter
All the fields are optional except pageIndex and usersPerPage which have default values. The `sortBy` and `filterBy` fields can be Name, Email, Age, null or a empty string and the `sortOrder` field can be asc, dsc, null, or a empty string. Any other value for these fields will throw a 400 Bad Request error. Defining the `sortBy` and `sortOrder` field should sort the users by name, email, or age in ascending or descending order. If you define the `filterBy` field then you should define the `nameOrEmailFilter` or the `ageFilter`, depending on if `filterBy` equals Name, Email, or Age. Defining these fields should filter the users by either name or email from `nameOrEmailFilter` or by age from `ageFilter`.

### Pagination
The `pageIndex` and `usersPerPage` mimics how the UI would send the data for pagination. The `pageIndex` is which page you are on and the `usersPerPage` is the amount of users displayed on that page. You can see this by which users are returned in the Response Body.
