# Employee Management System API

This project contains APIs for managing employees, admins, login, and report generation. Below is the list of available endpoints categorized by functionality.

---

## 🚀 **API Endpoints**

### ✅ **Admin APIs**
| Method   | Endpoint                                      | Description                         |
|---------|-----------------------------------------------|-------------------------------------|
| `GET`   | `/api/admin/Admin/get-employee`                | Retrieve all employees              |
| `GET`   | `/api/admin/Admin/get-employee-By-Id`          | Retrieve employee by ID             |
| `GET`   | `/api/admin/Admin/Export-TimeSheet-To-Excel`   | Export timesheet data to Excel      |
| `POST`  | `/api/admin/Admin/add-employee`                | Add a new employee                  |
| `POST`  | `/api/admin/Admin/delete-employee`             | Delete an employee                  |
| `PATCH` | `/api/admin/Admin/Update-Employee`             | Update employee details             |
| `POST`  | `/api/admin/Admin/Reset-Password`              | Reset employee password             |
| `POST`  | `/api/admin/Admin/Enter-New-Password`          | Set new password for employee       |
| `GET`   | `/api/admin/Admin/get-leaves-by-EmployeeID`    | Get leave details by Employee ID    |
| `DELETE`| `/api/admin/Admin/delete-leave`                | Delete a leave entry                |
| `PUT`   | `/api/admin/Admin/Update-Leave-Status`         | Update leave status                 |

---

### 👨‍💻 **Employee APIs**
| Method   | Endpoint                                     | Description                         |
|---------|----------------------------------------------|-------------------------------------|
| `POST`  | `/api/employee/Employee/Reset-Password`       | Reset employee password             |
| `POST`  | `/api/employee/Employee/Enter-New-Password`   | Set new password for employee       |
| `GET`   | `/api/employee/Employee/Employee-Profile`     | Get employee profile                |
| `POST`  | `/api/employee/Employee/Log-Working-Hours`    | Log employee working hours          |
| `PATCH` | `/api/employee/Employee/Update-Employee`      | Update employee details             |
| `GET`   | `/api/employee/Employee/Get-TimeSheet-By-Id`  | Get timesheet by ID                 |
| `POST`  | `/api/employee/Employee/Update-TimeSheet`     | Update timesheet data               |
| `GET`   | `/api/employee/Employee/Get-leaves`           | Get employee leaves                 |
| `POST`  | `/api/employee/Employee/Apply-for-leave`      | Apply for leave                     |
| `GET`   | `/api/employee/Employee`                      | Get all employees                   |

---

### 🔐 **Login APIs**
| Method   | Endpoint                        | Description             |
|---------|---------------------------------|-------------------------|
| `POST`  | `/api/Login/admin-login`         | Admin login              |
| `POST`  | `/api/Login/Employee-Login`      | Employee login           |

---

### 📊 **Report Generation APIs**
| Method   | Endpoint                                   | Description                     |
|---------|--------------------------------------------|---------------------------------|
| `GET`   | `/api/admin/ReportGenerate/WeeklyReport`    | Generate weekly report          |
| `GET`   | `/api/admin/ReportGenerate/MonthlyReport`   | Generate monthly report         |

---

## Setup Instructions 🛠️
1. Clone the repository:  
   ```sh
   git clone https://github.com/Utsav-7/EMS--Backend-REST-API.git
   ```
2. Navigate to the project directory:  
   ```sh
   cd EMS--Backend-REST-API
   ```
3. Restore dependencies:  
   ```sh
   dotnet restore
   ```
4. Configure the database in appsettings.json.
5. Run database migrations:  
   ```sh
   dotnet ef database update
   ```
6. Start the API:  
   ```sh
   dotnet run
   ```
7. Access Swagger UI at:
```
   https://localhost:7213/swagger/index.html
```
