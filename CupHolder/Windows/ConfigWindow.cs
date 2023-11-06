using System;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace CupHolder.Windows;

public class ConfigWindow : Window, IDisposable {
    private Configuration Configuration;

    public ConfigWindow(Plugin plugin) : base(
        "Cup Holder Configuration") {
        this.Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw() {
        ImGui.Text("Force the usage of Windows APIs to activate the cupholder");
        var enabled = this.Configuration.ForceWindowsCupholder;
        if (ImGui.Checkbox("##ForceWindowsCupholder", ref enabled)) {
            this.Configuration.ForceWindowsCupholder = enabled;
            this.Configuration.Save();
        }
        ImGui.Separator();
        ImGui.Text("Force the usage of Linux APIs to activate the cupholder");
        enabled = this.Configuration.ForceLinuxCupholder;
        if (ImGui.Checkbox("##ForceLinuxCupholder", ref enabled)) {
            this.Configuration.ForceLinuxCupholder = enabled;
            this.Configuration.Save();
        }
        ImGui.Separator();
        ImGui.Text("Force the usage of MacOS APIs to activate the cupholder");
        enabled = this.Configuration.ForceMacOSCupholder;
        if (ImGui.Checkbox("##ForceMacOSCupholder", ref enabled)) {
            this.Configuration.ForceMacOSCupholder = enabled;
            this.Configuration.Save();
        }
        ImGui.Separator();
    }
}
