# LyokoPluginLoader
A library for loading LyokoAPI plugins into an application.<br>
You can read more about it [here](https://lyokoapidoc.readthedocs.io/en/latest/PluginLoader/Loading%20Plugins/)

## Commands
LPL V2 has a few commands available (if application supports the CommandInputEvent and CommandOutputEvent)

### Prefix
LPL's prefix is 'loader'. The command syntax is (api.)loader.[subcommand]

### Information
These commands will show brief information in the command output, and verbose output in the LyokoLog
```
api.loader.pluginlist         - show all plugins
          .all                - same as above
          .enabled            - show all enabled plugins
          .disabled           - show all disabled plugins
```                       


### Plugin management
```api.loader.plugins.enable.[pluginname] - enable a plugin```

```api.loader.plugins.disable[pluginname] - disable a plugin```

