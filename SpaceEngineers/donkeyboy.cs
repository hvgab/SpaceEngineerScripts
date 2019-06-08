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
namespace DonkeyBoy {
    public sealed class Program : MyGridProgram {
    // Your code goes between the next #endregion and #region
#endregion



public Program() {
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
}

public void Main(string args) {
    
    // Get Text Panels
    IMyTextPanel panel1 = GridTerminalSystem.GetBlockWithName("Donkey Panel Front 1") as IMyTextPanel;
    IMyTextPanel panel2 = GridTerminalSystem.GetBlockWithName("Donkey Panel Front 2") as IMyTextPanel;
    IMyTextPanel panel3 = GridTerminalSystem.GetBlockWithName("Donkey Panel Front 3") as IMyTextPanel;
    Echo ("Screen connected: " + panel1.CustomName);
    Echo ("Screen connected: " + panel2.CustomName);
    Echo ("Screen connected: " + panel3.CustomName);

    // Get Sensor [0]
    List<IMySensorBlock> sensors = new List<IMySensorBlock>();
    GridTerminalSystem.GetBlocksOfType<IMySensorBlock>(sensors);
    IMySensorBlock sensor = sensors[0];
    Echo("Sensor: " + sensor.CustomName);

    // Get Remote Control [0]
    List<IMyRemoteControl> remotes = new List<IMyRemoteControl>();
    GridTerminalSystem.GetBlocksOfType<IMyRemoteControl>(remotes);
    IMyRemoteControl remote = remotes[0];
    Echo("Remote: " + remote.CustomName);

    
    // Main

    // Write Detection to Panel 1
    panel1.WriteText("Last detected:");
    panel1.WriteText("\n" + sensor.LastDetectedEntity.Name);
    panel1.WriteText("\n" + sensor.LastDetectedEntity.TimeStamp.ToString(), true);
    panel1.WriteText("\n" + sensor.LastDetectedEntity.Relationship.ToString(), true);
    panel1.WriteText("\n" + sensor.LastDetectedEntity.Position.ToString(), true);

    // Write Remote to Panel 2
    panel2.WriteText("Flight mode: " + remote.FlightMode);
    // Get properties
    List<ITerminalProperty> remote_props = new List<ITerminalProperty>();
    remote.GetProperties(remote_props);
    foreach (ITerminalProperty property in remote_props){
        panel2.WriteText("\n" + property.Id + " : " + property.TypeName, true);
    }
    
    // Write Waypoints to Panel 3
    List<MyWaypointInfo> waypoints = new List<MyWaypointInfo>();
    remote.GetWaypointInfo(waypoints);
    foreach (MyWaypointInfo waypoint in waypoints){
         panel2.WriteText(waypoint.Name + " - " + waypoint.Coords);
    }

    // Add waypoint
    remote.ClearWaypoints();
    remote.AddWaypoint(sensor.LastDetectedEntity.Position, sensor.LastDetectedEntity.Name+" "+sensor.LastDetectedEntity.TimeStamp);
}



#region PreludeFooter
    }
}
#endregion