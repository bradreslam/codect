database initialisation commands

dotnet ef migrations add InitialCreate --startup-project Codect --project DAL
dotnet ef database update --startup-project Codect --project DAL


pipeline workflows:
  atomatic.test.on.puch.to.dev:
    automaticly runs all the unittests on the code in the dev branch if a new commit is pushed to it, and generates code coverage. Result can be viewed in the actions tab.
  Codacy analysis CLI:
    Runs static code analysis using codacy when there is a new push to the main branch, results can be seen in the Security tab.
  codect_dockerize:
    Generates an new image of the backend and the database, if there is a push to the main branch. results can be seen in packages in the Code tab.
