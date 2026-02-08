NewDriver - Online Licensing Management SystemNewDriver is a robust .NET Web API platform designed to digitize and streamline the licensing process for private driving agencies. By moving away from manual paperwork and in-person visits, this system provides an efficient, automated workflow for both applicants and administrators.
🌟 Key FeaturesAutomated Applications: Digital submission for driving licenses and practice permits.
Request Management: A centralized dashboard for administrators to approve, reject, or update license statuses.
Error Reduction: Automated validation to ensure all applicant data is accurate before submission.
Role-Based Access: Secure endpoints for Applicants (User-side) and Agency Administrators (Admin-side).
Real-time Tracking: Applicants can monitor the progress of their permit or license requests.
🛠️ Tech StackFramework: .NET Web API (C#)Language: 
C#Database: (e.g., SQL Server / PostgreSQL / Entity Framework Core)
Authentication: (e.g., JWT Bearer Tokens / Identity Framework)
Documentation: Swagger / OpenAPI📂 Getting StartedPrerequisites.NET SDK (Version 6.0, 7.0, or 8.0 depending on your build)An IDE like Visual Studio 2022 or VS Code.A database engine (SQL Server is standard for .NET).InstallationClone the repository:Bashgit clone https://github.com/ZeQ1R/NewDriver.git
Navigate to the project directory:Bashcd NewDriver
Configure the Database:Update the ConnectionStrings in appsettings.json to point to your local database instance.Apply Migrations:Bashdotnet ef database update
Run the Application:Bashdotnet run
The API should now be running (usually at https://localhost:5001).
