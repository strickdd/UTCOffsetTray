# UTCOffsetTray
## Description
When working with people spanning multiple timezones and trying to adhere to servers or log timestamps being UTC in the timezone, it can be difficult to keep track of the current time in UTC. This simple tray application will display the UTC offset in the system tray. On right-click, it will show a list of configurable timezones and their current offsets. Clicking on an option will display the current time in that timezone.

## Building/Compiling
This was developed in VSCode, but should work in VisualStudio as well. Build the UTCOffsetTray.csproj to compile the application.

## Running
After [building/compiling](#buildingcompiling), run the UTCOffsetTray.exe. Optionally, add the resulting executable to your startup folder to have it run on startup. This can be done by pressing `Win+R` and typing `shell:startup` and pressing enter. Then, copy the executable or create a shortcut in the folder that opens.

## Configuration
The configuration file is located at `[runningDirectory]\config.yaml`. The file is a YAML object with the following properties:
```yaml
AdditionalTimezones:
  - Id: "America/Denver"
  - Id: "America/Chicago"
  - Id: "America/Los_Angeles"
DateTimeFormatter: "HH:mm:ss (yyyy-MM-dd)"
```

The `AdditionalTimezones` property is a list of timezone IDs in IANA format to display in the right-click menu. The `DateTimeFormatter` property is a string that will be used to format the date and time when displaying the current time in a timezone. The format is the same as the `DateTime.ToString` method in C#. The default value is `"HH:mm:ss (yyyy-MM-dd)"`.

## TODO
- [ ] Clean up the images
    - [ ] Cirlce is cut off on some sides
    - [ ] The text is not consistently placed
- [ ] Set up CI and Releases
  - [ ] Use GitHub Actions
  - [ ] Deteremine how to do version numbering and tagging