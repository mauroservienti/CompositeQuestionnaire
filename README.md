# Composite Questionnaire PoC

## How to get the sample working locally

### Get a copy of this repository

Clone or download this repo locally on your machine. If you're downloading a zip copy of the repo please be sure the zip file is unblocked before decompressing it. In order to unblock the zip file:

- Right-click on the downloaded copy
- Choose Property
- On the Property page tick the unblock checkbox
- Press OK

### Check your machine is correctly configured

In order to run the sample the following machine configuration is required:

- Visual Studio 2017 with .NET Core 2 support (Community Edition is supported), available for download at [https://www.visualstudio.com/downloads/](https://www.visualstudio.com/downloads/)


- A SQL Server edition or the `LocalDB` instance installed by Visual Studio, in case of a clean machine with `LocalDB`only please install. A stand along `LocalDB` installer is available at [https://www.microsoft.com/en-us/download/details.aspx?id=29062](https://www.microsoft.com/en-us/download/details.aspx?id=29062)

### Databases setup

Open an **elevated** command prompt, navigate to your copy of this repo, and run:

```batchfile
powershell -NoProfile -ExecutionPolicy unrestricted -File scripts\Setup-LocalDBInstance.ps1
```

When you no longer need to run the exercises, you may optionally run `Teardown-LocalDBInstance.ps1`.

Now connect to your `LocalDB` instance and run:

* `scripts\Setup-Databases.sql`.
* `scripts\Create-Tables.sql`

You can do this using either SQL Server Management Studio (if you already have it installed) or Visual Studio. If using Visual Studio:

- Open `scripts\Setup-Databases.sql`
- From the Visual Studio menus, select SQL -> Execute
- Choose this instance: Local -> CompositeQuestionnaire
- Click "Connect"
- After the query has run, ensure that you see "Command(s) completed successfully."
- Do the same for `scripts\Create-Tables.sql`

## NServiceBus configuration

This sample has no [NServiceBus](https://particular.net/nservicebus) related pre-requisites as it's configured to use the new [Learning Transport](https://docs.particular.net/nservicebus/learning-transport/) and [Learning Persistence](https://docs.particular.net/nservicebus/learning-persistence/) explicitly designed for short term learning and experimentation purposes.

They should also not be used for longer-term development, i.e. the same transport and persistence used in production should be used in development and debug scenarios. Select a production [transport](https://docs.particular.net/transports/) and [persistence](https://docs.particular.net/persistence/) before developing features. 

NOTE: 

* Do not use the learning transport or learning persistence to perform any kind of performance analysis.
* All solutions are configured to use the [SwitchStartupProject](https://marketplace.visualstudio.com/items?itemName=vs-publisher-141975.SwitchStartupProject) Visual Studio Extension to manage startup projects. The extension is not a requirement, it's handy.
* A [LINQPad script](SetStartupProjects.linq) is available to automatically configure startup projects for all solutions in the demo