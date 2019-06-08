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
namespace PlukkernLCD {
    public sealed class Program : MyGridProgram {
    // Your code goes between the next #endregion and #region
#endregion

public Program() {
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public string GetVolumes() {
    return "";
}

public void Main(string args) {
    
    const string VEHICLE_GRID_NAME = "Plukker'n";
    const string OUTPUT_CONNECTOR_NAME = "Plukker'n OUTPUT Conn";
    const string LCD_STATUS_NAME = "Base Garage Panel";

    // Connector to OUTPUT from
    IMyShipConnector connector = GridTerminalSystem.GetBlockWithName(OUTPUT_CONNECTOR_NAME) as IMyShipConnector;
    IMyTextPanel panel = GridTerminalSystem.GetBlockWithName(LCD_STATUS_NAME) as IMyTextPanel;

    // LCD Test - "screen-size"
    string LCDContent = "";
    string LCDContent_Cargo =         "     0    .    .    .";
    string LCDContent_CargoToConn =   "     0 -> .    .    .";
    string LCDContent_Conn =          "     .    0    .    .";
    string LCDContent_ConnToConn2 =   "     .    0 -> .    .";
    string LCDContent_Conn2 =         "     .    .    0    .";
    string LCDContent_Conn2ToCargo2 = "     .    .    0 -> 0";
    string LCDContent_Cargo2 =        "     .    .    .    0";
    panel.WriteText(LCDContent);


    // Hvis ting i other.connector
    if(connector.OtherConnector.GetInventory().CurrentVolume.RawValue > 0L){
        // Echo("other.connector: " + connector.OtherConnector.CustomName);
        // Echo("other.connector vol: " + connector.OtherConnector.GetInventory().CurrentVolume);
        // Echo("other.connector vol=0: " + (connector.OtherConnector.GetInventory().CurrentVolume.RawValue == 0L));
        
        // Flytt til other.container
        // Get containers on static grid
        List<IMyCargoContainer> otherContainers = new List<IMyCargoContainer>();
        GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(otherContainers);
        for (int i = 0; i < otherContainers.Count; i++){
            if(otherContainers[i].CubeGrid.IsStatic && !otherContainers[i].GetInventory().IsFull){
                bool transferred = connector.OtherConnector.GetInventory().TransferItemTo(otherContainers[i].GetInventory(), 0, null, null, null);
                Echo(LCDContent_Conn2ToCargo2);
                panel.WriteText(LCDContent_Conn2ToCargo2);
            }
        }
    }
    

    // Hvis ting i this.connector
    else if(connector.GetInventory().CurrentVolume.RawValue > 0L){
        // Echo("connector: " + connector.CustomName);
        // Echo("connector vol: "+ connector.GetInventory().CurrentVolume);
        // Echo("connector vol=0: " + (connector.GetInventory().CurrentVolume.RawValue == 0L));
        
        // Flytt til other.connector
        bool transferred = connector.GetInventory().TransferItemTo(connector.OtherConnector.GetInventory(), 0, null, null, null);
        // Echo("transferred from connector to other.connector: " + transferred.ToString());
        // Update panel
        Echo(LCDContent_ConnToConn2);
        panel.WriteText(LCDContent_ConnToConn2);
        // panel.WriteText("\nMoved stuff from \nvehicle.connector to grid.connector", true);
    }
    
    else
    {
        // Hvis ting i this.container
        List<IMyTerminalBlock> vehicleCargoContainers = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(vehicleCargoContainers);
        long vehicleTotalVolume = 0L;
        for (int i = 0; i < vehicleCargoContainers.Count; i++)
        {
            if (vehicleCargoContainers[i].CubeGrid.CustomName == VEHICLE_GRID_NAME && vehicleCargoContainers[i].HasInventory)
            {
                // Echo("vehicle container: " + vehicleCargoContainers[i].CustomName);
                // Echo("connector vol: "+ vehicleCargoContainers[i].GetInventory().CurrentVolume);
                // Echo("connector vol=0: " + (vehicleCargoContainers[i].GetInventory().CurrentVolume.RawValue == 0L));
                vehicleTotalVolume += vehicleCargoContainers[i].GetInventory().CurrentVolume.RawValue;
            }
        }
        // Echo("Vehicle tot vol: " + (vehicleTotalVolume/1000).ToString("n0"));

        if(vehicleTotalVolume != 0L){
            // Flytt til this.connector
            for (int i = 0; i < vehicleCargoContainers.Count; i++){
                if (vehicleCargoContainers[i].CubeGrid.CustomName == VEHICLE_GRID_NAME && vehicleCargoContainers[i].HasInventory){
                    vehicleCargoContainers[i].GetInventory().TransferItemTo(connector.GetInventory(), 0, null, null, null);
                    Echo(LCDContent_CargoToConn);
                    panel.WriteText(LCDContent_CargoToConn);
                    break;
                }
            }
            // Update Panel
            
            // panel.WriteText("\nMoved stuff from \nvehicle.container to vehicle.connector", true);
        }
    }


    

    
    
    // // Transfer
    // for(int i = 0; i < blocks.Count; i++){
    //     if (blocks[i].CubeGrid.CustomName == VEHICLE_GRID_NAME && blocks[i].HasInventory)
    //     {
    //         if (blocks[i].GetInventory().ItemCount > 0)
    //         {
    //             Echo("Start transfer from output connector to connected connector");
    //             blocks[i].GetInventory().TransferItemTo(OUTPUT_CONNECTOR.OtherConnector.GetInventory(), 0);
    //             Echo("End transfer from output connector to connected connector");
                
    //             // Get cargo blocks in static grid
    //             List<IMyCargoContainer> other_grid_containers = new List<IMyCargoContainer>();
    //             GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(other_grid_containers);
    //             for(int j = 0; j < other_grid_containers.Count; j++ ){
    //                 if (other_grid_containers[j].CubeGrid.IsStatic)
    //                 {
    //                     Echo("Container " + other_grid_containers[j].CustomName + " is on grid " + other_grid_containers[j].CubeGrid.CustomName);
    //                     if (!other_grid_containers[j].GetInventory().IsFull){
    //                         OUTPUT_CONNECTOR.OtherConnector.GetInventory().TransferItemTo(other_grid_containers[j].GetInventory(), 0);
    //                         Echo("Breaking other_container loop");
    //                         break;
    //                     }
    //                 }
    //             }

    //         }
    //     }
    //     Echo("Breaking blocks loop");
    //     break;
    // }
    

}

#region PreludeFooter
    }
}
#endregion