database initialisation commands

dotnet ef migrations add InitialCreate --startup-project Codect --project DAL
dotnet ef database update --startup-project Codect --project DAL
