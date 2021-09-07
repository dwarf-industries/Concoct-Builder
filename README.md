
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FKristiforMilchev%2FRokonoControl.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FKristiforMilchev%2FRokonoControl?ref=badge_shield)

<link href="social-media-buttons.min.css" rel="stylesheet">

<a href="https://twitter.com/RokonoC" class="ss-btn-twitter">Follow us on Twitter</a>

<a href="https://www.facebook.com/Rokono-Control-107429234338967" class="ss-btn-twitter">Follow us on Facebook</a>

Public demo and acess to the project for contributors:
https://portal.concoctcloud.com/Accounts/ProjectSignup?projectId=11030

Concoct-Builder is an open source tool, integrated into Concoct Cloud, its main purpose is to create mockups and functional presentation using any type of controls (Widgets). By default it comes with a suite of Syncfusion controls as its the main UI powerhouse behind Concoct Cloud, all controls can be replaced to work with any design language with ease.

The tool has an integration module with Concoct Cloud which allows to build design flows, UI interatictions and is meant to be used with the planning module to display issues with the ui or mock up new software panels.
## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

What things you need to install the software and how to install them

The project is created using asp.net core 5.0, its not based on blazor because its a version that is being migrated from asp.net core v 3.1 so you need to get the proper runtime from Microsoft.
  
In order to install .net core runtime go to this link https://dotnet.microsoft.com/download 

Install Node.js . For more information use this link https://nodejs.org/en/download/ .

### Installing
 
 -You can easily get the system running, all you need to do in order to get started is to install the dependencies mentioned above and then you need to navigate to the root directory of the project
 
 The software is build using Electron.net so for development purposes you will need to get it as a dependency

 Install-Package ElectronNET.API
 dotnet tool install ElectronNET.CLI -g

 Once you're ready with all dependencies follow up with the following command for CLI 'electronize start' or start the solution from Visual Studio

## Deploying

In order to create a binary for the software use the following cli commands for your release target.

electronize build /target win
electronize build /target osx
electronize build /target linux

## Contributing

This tool is kept under our management platform and you can find the public task board at this location. If you want to get anything off the board assigned to your name please do use our public sign up page and just move the item under yourself mentioning me so i can keep track of it. Don't forget to move your cards around and only pick what you can finish in the active sprint. Each sprint on the project is 3 weeks.


For a list of planned features and tasks you can view the project public board at:

Public demo and acess to the project for contributors:
https://portal.concoctcloud.com/Accounts/ProjectSignup?projectId=11030


 [![Public Taskboard](https://portal.concoctcloud.com/IMG/LatestBoard.PNG)] https://portal.concoctcloud.com/Boards/PublicBoard?projectId=11030&iteration=9038&person=0)

## Authors

* **Kristifor Milchev** - *Initial work* - (https://github.com/kristiformilchev)

See also the  list of [contributors](https://github.com/dwarf-industries/concoct-cloud/blob/master/CODE_OF_CONDUCT.md) who participated in this project.

## License

This project is licensed under the Non-Profit Open Software License 3.0 (NPOSL-3.0) - see the [LICENSE.md](https://github.com/dwarf-industries/concoct-builder/blob/master/License.md) file for details



 