using System;
using System.Collections.Generic;
using Dalamud.Configuration;
using Newtonsoft.Json;

namespace CupHolder;

[Serializable]
public class Configuration : IPluginConfiguration {
    [JsonProperty] public Boolean ForceWindowsCupholder;
    [JsonProperty] public Boolean ForceLinuxCupholder;
    [JsonProperty] public Boolean ForceMacOSCupholder;
    public int Version { get; set; } = 0;

    public void Save() {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
