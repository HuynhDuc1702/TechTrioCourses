# ?? Quick Start Guide

## Start the Applications

### Step 1: Start Backend API
```bash
cd Admin_BE
dotnet run
```
? **Backend running on:** `https://localhost:7251`

### Step 2: Start Frontend Website
```bash
cd Admin_FE_test
dotnet run
```
? **Frontend running on:** Check terminal output

---

## ?? Quick Test Scenarios

### Scenario 1: Register & Login as Admin (Can Access Courses)
1. Open browser ? Go to Frontend URL
2. Click **"Register"**
3. Fill form:
   ```
   Email: admin@test.com
   Full Name: Admin User
   Password: Admin123!
   Confirm Password: Admin123!
   Role: Admin
   ```
4. Click "Register" ? Check email for OTP
5. Enter OTP ? Click "Verify"
6. Click **"Login"** ? Enter credentials
7. Click **"Courses"** in navbar ? ? **Should see courses list**

### Scenario 2: Register as Student (Cannot Access Courses)
1. Register with Role: **Student**
2. Verify OTP & Login
3. Try to access Courses ? ? **Should see "Access Denied"**

### Scenario 3: Change Password
1. Login as any user
2. Click **"Change Password"**
3. Enter old and new passwords
4. Submit ? Logout ? Login with new password

---

## ?? Expected Results

| Action | Admin (0) | Instructor (2) | Student (1) |
|--------|-----------|----------------|-------------|
| Access Courses | ? Allowed | ? Allowed | ? Denied |
| See Courses Link | ? Yes | ? Yes | ? No |
| Change Password | ? Yes | ? Yes | ? Yes |

---

## ?? Key Features

- ? **JWT Authentication** - Secure token-based auth
- ? **OTP Verification** - Email verification with 6-digit code
- ? **Role-Based Access** - Different permissions for Admin/Instructor/Student
- ? **Password Hashing** - Secure PBKDF2 encryption
- ? **Session Management** - 30-minute timeout
- ? **Responsive Design** - Bootstrap 5 mobile-friendly UI

---

## ?? Test Credentials (After Registration)

Create these accounts for testing:

```
Admin:
Email: admin@test.com
Password: Admin123!
Role: 0

Instructor:
Email: instructor@test.com
Password: Instructor123!
Role: 2

Student:
Email: student@test.com
Password: Student123!
Role: 1
```

---

## ? Verification Checklist

- [ ] Backend running on port 7251
- [ ] Frontend accessible in browser
- [ ] Can register new user
- [ ] OTP email received
- [ ] Can verify OTP
- [ ] Can login
- [ ] Admin/Instructor can see Courses
- [ ] Student cannot see Courses
- [ ] Can change password
- [ ] Can logout

---

## ?? Quick Fixes

**Issue:** Cannot connect to API  
**Fix:** Ensure Admin_BE is running on port 7251

**Issue:** OTP not received  
**Fix:** Check spam folder or SMTP settings in appsettings.json

**Issue:** "Access Denied" for Admin  
**Fix:** Verify Role is set to 0 or 2 during registration

---

## ?? API Endpoints

```
POST /api/Accounts/register       ? Register new user
POST /api/Accounts/verify-otp ? Verify OTP code
POST /api/Accounts/login          ? Login and get JWT token
POST /api/Accounts/change-password ? Update password
GET  /api/Courses                 ? Get all courses (Auth required)
```

---

## ?? All Done!

Your authentication system with OTP verification and role-based access is ready!

**Run both applications and start testing!** ??
