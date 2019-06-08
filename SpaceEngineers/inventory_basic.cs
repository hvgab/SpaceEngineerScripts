#region Prelude
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

// Change this namespace for each script you create.
// namespace SpaceEngineers.UWBlockPrograms.BatteryMonitor {
namespace StorageMonitor {
    public sealed class Program : MyGridProgram {
    // Your code goes between the next #endregion and #region
#endregion

public Program() {
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Main(string args) {
    
    var panel = GridTerminalSystem.GetBlockWithName("Base Garage Panel") as IMyTextPanel;
    Echo ("Screen connected: " + panel.CustomName);

    List<IMyCargoContainer> cargocontainers = new List<IMyCargoContainer>();
    GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(cargocontainers);

    Echo ("Blocks with inventory found:");
    
    var panel_text = "";

    for(int i = 0; i < cargocontainers.Count; i++){
        Echo(cargocontainers[i].CustomName + " has inv? " + cargocontainers[i].HasInventory.ToString());
        
        var short_customname = cargocontainers[i].CustomName;
        short_customname = short_customname.Replace("Small", "S");
        short_customname = short_customname.Replace("Medium", "M");
        short_customname = short_customname.Replace("Large", "L");
        short_customname = short_customname.Replace("Cargo Container", "Cargo.C");

        var inventory = cargocontainers[i].GetInventory();
        
        panel_text += short_customname + ":" + inventory.CurrentVolume.ToIntSafe() + " / " + inventory.MaxVolume.ToIntSafe() + "\n";
        
        // panel.WriteText(cargocontainers[i].CustomName + ":" + inventory.CurrentVolume + " / " + inventory.MaxVolume + "\n", true);
    }
    panel.WriteText(panel_text);
}

#region PreludeFooter
    }
}
#endregion