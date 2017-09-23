# MVC_Splunk_REST_API
Splunk REST API MVC App
Visual Studio 2017 Simple Splunk REST API with MVC App

This project file is a simple MVC application to show you how easy it is to connect to a Splunk instance with the REST API.
Be sure to set "allowremotelogin=always" in your Splunk "local" configuration file.

For more information:
http://docs.splunk.com/Documentation/Splunk/6.6.3/RESTUM/RESTusing


Edit: appsettings.json
set you localhost where your Splunk Server is running on port 8089.
Set your username and password to that splunk instance

All of the relevant code is in the HomeController.cs file

The program retrieves example log data from the Splunk server. 
