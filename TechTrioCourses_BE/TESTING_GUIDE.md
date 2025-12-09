# TechTrio Courses - Testing Guide

## ?? Quick Start

### Prerequisites
- .NET 8 SDK
- PostgreSQL database (configured in Admin_BE)
- Backend API running on `https://localhost:7251`

### Running the Applications

#### 1. Start the Backend API (Admin_BE)
```bash
cd Admin_BE
dotnet run
```
The API will be available at: `https://localhost:7251`

#### 2. Start the Frontend (Admin_FE_test)
```bash
cd Admin_FE_test
dotnet run
```
The website will be available at: `https://localhost:XXXX` (check terminal for port)

---

## ?? Test Scenarios

### Test 1: User Registration with OTP Verification

1. **Navigate to Register Page**
   - Click "Register" in the navigation bar
   - URL: `/Account/Register`

2. **Fill Registration Form**
   ```
   Email: test@example.com
   Full Name: Test User
   Password: Test123!
   Confirm Password: Test123!
   Role: Select one of:
     - Admin (0)
     - Student (1)
     - Instructor (2)
   Avatar URL: (optional)
   ```

3. **Submit Registration**
   - Click "Register" button
   - You'll be redirected to OTP verification page
   - Check email `test@example.com` for OTP code

4. **Verify OTP**
   - Enter the 6-digit code from email
   - Click "Verify" button
   - On success, redirected to Login page

### Test 2: User Login

1. **Navigate to Login Page**
   - Click "Login" in navigation bar
   - URL: `/Account/Login`

2. **Enter Credentials**
   ```
 Email: test@example.com
   Password: Test123!
   ```

3. **Successful Login**
   - JWT token stored in session
   - Redirected to Courses Index (if Admin/Instructor)
 - User email displayed in navigation bar

### Test 3: Role-Based Access Control

#### Test 3a: Admin Access
1. Register/Login as **Admin** (Role = 0)
2. Navigate to `/Courses`
3. ? **Should**: Display courses list
4. ? **Should**: Show "Admin" role in navigation

#### Test 3b: Instructor Access
1. Register/Login as **Instructor** (Role = 2)
2. Navigate to `/Courses`
3. ? **Should**: Display courses list
4. ? **Should**: Show "Instructor" role in navigation

#### Test 3c: Student Access (Denied)
1. Register/Login as **Student** (Role = 1)
2. Navigate to `/Courses`
3. ? **Should**: Redirect to Access Denied page
4. ? **Should**: Show error message

### Test 4: Change Password

1. **Login as any user**
2. **Navigate to Change Password**
   - Click "Change Password" in navigation
 - URL: `/Account/ChangePassword`

3. **Fill Form**
   ```
   Current Email: (auto-filled from session)
   Current Password: Test123!
   New Password: NewTest123!
   Confirm New Password: NewTest123!
   ```

4. **Submit**
 - Click "Change Password"
   - On success: redirected to Home with success message
   - On failure: error message displayed

5. **Verify New Password**
   - Logout
   - Login with new password `NewTest123!`

### Test 5: Session Management

1. **Check Session Persistence**
   - Login as user
   - Navigate between pages
   - ? Session should persist (30 minutes timeout)

2. **Logout**
   - Click "Logout" in navigation
   - ? Session cleared
   - ? Redirected to Login page
   - ? Cannot access protected pages

---

## ?? Test Matrix

| Test Case | Role | Expected Result |
|-----------|------|-----------------|
| Register with valid data | Any | Success ? OTP verification |
| Register with existing email | Any | Error: "Email already exists" |
| Login with valid credentials | Any | Success ? Redirect to Courses or Home |
| Login with invalid credentials | Any | Error: "Invalid email or password" |
| Access /Courses | Admin (0) | ? Access Granted |
| Access /Courses | Instructor (2) | ? Access Granted |
| Access /Courses | Student (1) | ? Access Denied |
| Access /Courses | Not Logged In | ? Redirect to Login |
| Change password (correct old) | Any | ? Success |
| Change password (incorrect old) | Any | ? Error |

---

## ?? Key Features to Test

### 1. Authentication Flow
- ? Registration creates account with status = Disabled
- ? OTP sent via email
- ? OTP verification activates account (status = Active)
- ? Login generates JWT token
- ? Token stored in session (30 min expiry)

### 2. OTP System
- ? OTP stored in HTTP-only secure cookie
- ? OTP format: `code|expiresAt|purpose`
- ? OTP expires in 10 minutes
- ? OTP cookie deleted after verification
- ? Email sent with styled HTML template

### 3. Role-Based Access
- ? Admin (0) ? Full access to courses
- ? Instructor (2) ? Full access to courses
- ? Student (1) ? Cannot access courses
- ? Not logged in ? Redirect to login

### 4. Security Features
- ? Password hashing (PBKDF2 with SHA256)
- ? JWT authentication with Bearer token
- ? HTTP-only secure cookies for OTP
- ? Session-based token storage
- ? CORS configured for API

---

## ?? Common Issues & Solutions

### Issue 1: Cannot connect to API
**Solution:** 
- Ensure Admin_BE is running on port 7251
- Check `Admin_FE_test/Services/ApiService.cs` BaseUrl
- Verify HTTPS certificate is trusted

### Issue 2: OTP Email not received
**Solution:**
- Check SMTP settings in `Admin_BE/appsettings.json`
- Verify Gmail app password is correct
- Check spam/junk folder
- Ensure "Less secure app access" is enabled (if using Gmail)

### Issue 3: Session expires too quickly
**Solution:**
- Increase timeout in `Admin_FE_test/Program.cs`:
  ```csharp
  options.IdleTimeout = TimeSpan.FromMinutes(30);
  ```

### Issue 4: CORS error
**Solution:**
- Ensure Admin_BE has CORS policy configured
- Check `AllowAll` policy in `Admin_BE/Program.cs`

### Issue 5: Courses not loading
**Solution:**
- Verify JWT token is valid (check browser dev tools ? Application ? Session Storage)
- Ensure user has correct role (0 or 2)
- Check API is returning data (test with Postman)

---

## ?? UI Elements

### Navigation Bar
- **Not Logged In:** Home | Login | Register
- **Logged In:** Home | Courses (if Admin/Instructor) | Email | Change Password | Logout

### Alert Messages
- **Success (Green):** Login success, Registration success, Password changed
- **Error (Red):** Login failed, Invalid OTP, Access denied
- **Info (Blue):** OTP verification instructions

### Pages
1. **Home** (`/Home/Index`)
   - Welcome message
   - Feature cards
   - Quick links based on login status

2. **Login** (`/Account/Login`)
   - Email and Password fields
   - Link to Register

3. **Register** (`/Account/Register`)
   - Email, Full Name, Password, Role, Avatar URL
   - Link to Login

4. **Verify OTP** (`/Account/VerifyOtp`)
   - 6-digit code input
   - Email confirmation
   - Back to Register link

5. **Change Password** (`/Account/ChangePassword`)
   - Current Password
   - New Password
   - Confirm New Password

6. **Courses Index** (`/Courses/Index`)
   - Grid of course cards
   - Course details (title, description, lessons, quizzes, rating)
   - Creator and category information

7. **Access Denied** (`/Courses/AccessDenied`)
   - Error message
   - Links to Home and Logout

---

## ?? Test Accounts

Create these test accounts for comprehensive testing:

### Admin Account
```
Email: admin@test.com
Password: Admin123!
Role: 0 (Admin)
```

### Instructor Account
```
Email: instructor@test.com
Password: Instructor123!
Role: 2 (Instructor)
```

### Student Account
```
Email: student@test.com
Password: Student123!
Role: 1 (Student)
```

---

## ? Verification Checklist

- [ ] Backend API running on port 7251
- [ ] Frontend running successfully
- [ ] Can register new user
- [ ] OTP email received
- [ ] Can verify OTP
- [ ] Can login with verified account
- [ ] JWT token stored in session
- [ ] Admin can access Courses
- [ ] Instructor can access Courses
- [ ] Student cannot access Courses (Access Denied)
- [ ] Not logged in redirected to Login
- [ ] Can change password
- [ ] Can logout
- [ ] Session persists across page navigation
- [ ] Alert messages display correctly
- [ ] Navigation updates based on login status

---

## ?? API Endpoints Used

```
POST   /api/Accounts/register      ? Create account + send OTP
POST   /api/Accounts/verify-otp       ? Verify OTP and activate account
POST   /api/Accounts/login ? Authenticate and get JWT token
POST   /api/Accounts/change-password  ? Update password
GET    /api/Courses   ? Get all courses (requires JWT)
```

---

## ?? Technologies Used

### Frontend (Admin_FE_test)
- ASP.NET Core 8 MVC (Razor Pages)
- Bootstrap 5
- Session-based authentication
- HttpClient for API calls

### Backend (Admin_BE)
- ASP.NET Core 8 Web API
- JWT Bearer Authentication
- SMTP Email Service
- Cookie-based OTP storage
- Entity Framework Core (PostgreSQL)

---

## ?? Notes

1. **OTP Expiry:** OTPs expire after 10 minutes
2. **Session Timeout:** User sessions expire after 30 minutes of inactivity
3. **JWT Expiry:** JWT tokens expire after 30 minutes (configurable in JwtSettings)
4. **Password Requirements:** No specific requirements enforced (add validation as needed)
5. **HTTPS Required:** Both apps should run on HTTPS for secure cookies

---

## ?? Success Criteria

Your testing is complete when:
1. ? All 5 test scenarios pass
2. ? All roles behave as expected
3. ? All security features work correctly
4. ? No errors in browser console
5. ? No exceptions in backend logs

---

## ?? Support

If you encounter issues:
1. Check browser console for JavaScript errors
2. Check backend terminal for API errors
3. Verify database connection
4. Confirm SMTP settings are correct
5. Test API endpoints directly with Postman/Swagger
