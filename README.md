How to run the program:
1. Download the docker-compose.yml and the .env file from the root of the repository
2. make sure the docker-compose.yml and the .env file are in the same directory on your device and that the .env file is called .env
3. go to your terminal and navigate to the location of the docker-compose.yml file using cd/{your file location}
4. pull the latest image of the application by running the "docker pull ghcr.io/bradreslam/codect:app-latest" command
5. run the command "docker-compose -f Docker-compose.yml up" in bash or powershell
6. go to http://localhost:8080 to see the application in action

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
