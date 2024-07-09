# Disclaimer Note
A fork from https://github.com/opentok-community/basic-video-chat-angular
Updated to use New Vonage Unified Video
There might be bugs as the changes were extracted from a bigger project. If you find any, contact me

# Basic Video Chat Angular

This is a basic video chat example using .NET and Angular.

## Prerequisites

* Visual Studio, I’m using 2019, though older versions ought to work
* I'm using .NET Core 3.1 so you'll need the [developer kit](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* An Vonage Video API account, if you don’t have one sign up [here](https://tokbox.com/account/user/signup)
* You'll need to create a project in Vonage Video API which you can do from your account page

## Setup

Open the sln in Visual Studio.

Open `appsettings.json` and replace `ApiKey` and ApiSecret with the apiKey and apiSecret from you Vonage Video Api account.

Open The project properties->Debug and ensure the AppUrl matches the `SAMPLE_SERVER_BASE_URL` in ClientApp/src/config.ts
