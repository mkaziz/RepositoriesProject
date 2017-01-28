# Welcome to my ASP.NET Core test project

In order to get this to run, make sure you have a MySQL database running, and update  the connectionstrings in appSettings.json to point to the database you wish to use for the project

Additionally, to handle the messaging portion, you'll need to have a RabbitMQ server configured to run on your local machine with all default ports, etc

Project was built and tested on Zorin OS 12 (based on Ubuntu 16.04).

# Improvements
* Handle taking in input parameters for loading repositories by organization name, as well as retrieving them by the same
* Clean up Queuing Service
* Factor out EF logic to a separate service and make it dependency injectable so that the GithubService is more unit testable
* Figure out a way to make the HttpClient call that gets the Github repos is also not "newed" up and dependency injectable

