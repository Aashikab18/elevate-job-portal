# Elevate Job Portal

A job portal for Elevate Workforce Solutions, built with ASP.NET Core MVC (.NET 10),
Entity Framework Core, ASP.NET Core Identity and PostgreSQL.

## Features
- Register, log in and log out as a Job Seeker or a Company
- Paginated list of all jobs, and a job details page
- Companies can add, edit and delete their own jobs
- Job seekers can apply for jobs with a cover letter
- Company dashboard showing the applications received

## How to run
1. Install the .NET 10 SDK and Docker Desktop.
2. In the main folder, start the database: `docker compose up -d`
3. In the ElevateJobPortal folder, run: `dotnet run`
4. Open the address shown in the terminal (for example http://localhost:5111).

The database tables are created automatically when the application starts.
Register a company account and a job seeker account to try the features.