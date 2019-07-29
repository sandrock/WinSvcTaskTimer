
# WinSvcTaskTimer

Simple Windows Service app to run apps on timers.


## How to use

### Configure tasks and timers

Open the config file. Add one or many tasks. More details below. 

```
<TaskHost>
  <Tasks>
    <Task Name="Task1" Type="WinSvcTaskTimer.ShellRunner, WinSvcTaskTimer" Argument="ping test.com" />
  </Tasks>
  <Timers>
    <Timer Name="Timer1" TaskName="Task1" Enabled="True" Interval="00:01:00" Delay="00:00:05" TickBehavior="Continue" />
  </Timers>
</TaskHost>
```


### Test the runtime

In a shell run the exe using its full path.

```
> <full path to WinSvcTaskTimer.exe>
```

You should see the service start and your task should execute.

Hit 'Q' to verify the tasks stop.


### Prepare a user account for the service

Windows Services run under a user account. You can create one with the following command. Please change the sample username and password. 

```
> net user /add MyServices key=gseVEtNAs88UKbqK
```


### Configure the service

In the config file, specify the ServiceInstaller things. More details below. 

```
<add key="ServiceInstaller/ServiceName"        value="SuperService"     />
<add key="ServiceInstaller/DisplayName"        value="SuperService will do stuff and blah" />
<add key="ServiceInstaller/ServiceAccount"     value="User"             />
<add key="ServiceInstaller/Username"           value=".\MyServices"     />
<add key="ServiceInstaller/Password"           value="gseVEtNAs88UKbqK" />
<add key="ServiceInstaller/ServicesDependedOn" value=""                 />
<add key="ServiceInstaller/DelayedAutoStart"   value="False"            />
<add key="ServiceInstaller/StartType"          value="Automatic"        />
```


### Install as a service

These commands will install the service, start it and stop it.

```
> %windir%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe <full path to WinSvcTaskTimer.exe>
> net start SuperService
> net stop  SuperService
```

You may now **remove the password from the configuration file**.


## Advanced configuration

### ServiceInstaller configuration options

TODO


### Task options

TODO


### Timer options

TODO


# Code and contribute

## Internal to-do list

- [x] Open source base project
    - [x] Use ServiceInstaller with configuration
    - [x] Task and Timer base model with configuration
    - [x] Base documentation
- [ ] More documentation
- [ ] More options to kill running tasks on service stop
- [ ] Runner modularity: allow loading an assembly to define custom tasks


## Contribute

- Contribute by use
    - report any problem (create an issue)
    - suggest new features (create an issue)
    - before creating an issue: search for an existing issue and read the documentation
- Contribute by code
    - code must pass most StyleCop rules
    - make a [pull request](https://help.github.com/en/articles/about-pull-requests) explaining your changes


