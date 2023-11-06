using System;
using System.Linq;
using CupHolder.Windows;
using Dalamud.Game.Command;
using Dalamud.Hooking;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace CupHolder;

public sealed class Plugin : IDalamudPlugin {
    
    private const string CommandName = "/cupholder";
    private const string ConfigCommandName = "/cupholderconfig";

    private readonly ConfigWindow configWindow;
    
    public readonly WindowSystem WindowSystem = new("CupHolder");

    public Plugin() {
        this.Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        this.configWindow = new ConfigWindow(this);

        this.WindowSystem.AddWindow(this.configWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(this.OnCommand) {
            HelpMessage = "activates the free ffxiv cup holder (:"
        });
        CommandManager.AddHandler(ConfigCommandName, new CommandInfo(this.OnConfigCommand) {
            HelpMessage = "cupholder config"
        });
        

        PluginInterface.UiBuilder.Draw += this.DrawUi;
        PluginInterface.UiBuilder.OpenConfigUi += this.DrawConfigUi;
    }

    [PluginService] public static IGameInteropProvider GameInteropProvider { get; private set; } = null!;
    [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static IDataManager DataManager { get; private set; } = null!;
    [PluginService] public static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] public static IChatGui ChatGui { get; private set; } = null!;
    [PluginService] public static IPluginLog PluginLog { get; private set; } = null!;

    public Configuration Configuration { get; init; }

    public void Dispose() {
        this.WindowSystem.RemoveAllWindows();

        this.configWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
        CommandManager.RemoveHandler(ConfigCommandName);
    }

    private bool ShouldTryWindowsCupholder() {
        return (this.Configuration.ForceWindowsCupholder || !UnsafeNativeApis.CheckForWine());
    }

    private bool ShouldTryLinuxCupholder() {
        return (this.Configuration.ForceLinuxCupholder || (UnsafeNativeApis.CheckForWine() && UnsafeNativeApis.WineIsLinux()));
    }

    private bool ShouldTryMacOsCupholder() {
        return (this.Configuration.ForceMacOSCupholder ||
                (UnsafeNativeApis.CheckForWine() && !UnsafeNativeApis.WineIsLinux()));
    }
    
    private void OnCommand(string command, string args) {
        var cupholderActivated = false;
        if (this.ShouldTryWindowsCupholder()) {
            UnsafeNativeApis.OpenCupholderWindows();
            PluginLog.Debug("used windows api to open cupholder");
            cupholderActivated = true;
        }
        if (this.ShouldTryLinuxCupholder()) {
            UnsafeNativeApis.OpenCupholderLinux();
            PluginLog.Debug("used linux api to open cupholder");
            cupholderActivated = true;
        }
        if (this.ShouldTryMacOsCupholder()) {
            UnsafeNativeApis.OpenCupholderMacOS();
            PluginLog.Debug("used macos api to open cupholder");
            cupholderActivated = true;
        }
        if (cupholderActivated) {
            ChatGui.Print("cupholder activated! (:");
        } else {
            ChatGui.Print("failed to activate cupholder, please report this to the plugin developer!");
        }
    }
    
    private void OnConfigCommand(string command, string args) {
        // in response to the slash command, just display our main ui
        this.DrawConfigUi();
    }

    private void DrawUi() {
        this.WindowSystem.Draw();
    }

    public void DrawConfigUi() {
        this.configWindow.IsOpen = true;
    }

}
