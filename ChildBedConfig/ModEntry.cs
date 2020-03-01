using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using System.Linq;
using StardewValley.Locations;
using System.Collections.Generic;
using StardewValley.Buildings;
using xTile.Tiles;

namespace ChildBedConfig
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {

        /*****************************/
        /**      Properties         **/
        /*****************************/
        ///<summary>The config file from the player</summary>
        private ModConfig Config;

        /*****************************/
        /**      Public methods     **/
        /*****************************/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();
            //Farmer = new Farmer();

            if (Config.Farms.Count == 0)
            {
                Config.Farms.Add(new Farm());
            }

            Helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        }

        /// <summary>
        /// Modify the farmhouse tiles
        /// </summary>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs args)
        {
            bool MadeChanges = false;
            Farm Farm = GetFarmInfo();
            if(Farm.FarmName.Equals(string.Empty))
            {
                string FarmName = Game1.player.farmName.Value;
                Monitor.Log($"Current farm name \"{FarmName}\" not found in config file!");
                Monitor.Log("Creating a new config entry for the current farm.");
                Farm.FarmName = FarmName;
                Config.Farms.Add(Farm);
                MadeChanges = true;
            }

            List<GameLocation> buildings = new List<GameLocation>() { Game1.getLocationFromName("FarmHouse") };
            buildings.AddRange(getCabins());
            
            foreach (GameLocation building in buildings)
            {
                if (!(building is FarmHouse)) // Cabins are a type of farmhouse
                {
                    continue;
                }
                
                int BuildingUpgradeLevel = 0;
                string HouseOwner = "";

                try
                {
                    Cabin c = (Cabin)building;
                    BuildingUpgradeLevel = c.upgradeLevel;
                    HouseOwner = c.owner.Name;
                }
                catch
                {
                    BuildingUpgradeLevel = Game1.player.HouseUpgradeLevel;
                    HouseOwner = Game1.player.Name;
                }

                if(HouseOwner.Equals(string.Empty)) { continue; } //Building doesn't have an owner

                if (!Farm.FarmerInfo.TryGetValue(HouseOwner, out Farmer BuildingOwner)) //No config entry for farmer name - Make a default one and continue working
                {
                    BuildingOwner = new Farmer();

                    Config.Farms[Config.Farms.IndexOf(Farm)].FarmerInfo.Add(HouseOwner, BuildingOwner);
                    MadeChanges = true;

                    Farm.FarmerInfo[HouseOwner] = BuildingOwner;
                }

                if (BuildingUpgradeLevel <= 1) //Building doesn't have bedrooms 
                {
                    //This check is after attempting to get info because this way it generates a config file block for every farmhand, not just those who have already upgraded
                    continue;
                } 

                if (!BuildingOwner.ShowCrib)
                {
                    building.RemoveCrib();
                }

                if (!BuildingOwner.ShowLeftBed)
                {
                    building.RemoveLeftBed();
                }

                if (!BuildingOwner.ShowRightBed)
                {
                    building.RemoveRightBed();
                }

            }

            if (MadeChanges)
            {
                this.Helper.WriteConfig(Config); //Write any changes made
            }
        }

        

        private Farm GetFarmInfo()
        {
            return Config.Farms.Find(x => x.FarmName == Game1.player.farmName.Value) ?? new Farm();
        }

        public static List<GameLocation> getCabins()
        {
            List<GameLocation> list = Game1.locations.ToList();
            List<GameLocation> cabins = new List<GameLocation>();

            foreach (GameLocation location in Game1.locations)
                if (location is BuildableGameLocation bgl)
                    foreach (Building building in bgl.buildings)
                        if (building.indoors.Value != null)
                            list.Add(building.indoors.Value);

            foreach(GameLocation location in list)
            {
                if(location.Name == "Cabin")
                {
                    cabins.Add(location);
                }
            }

            return cabins;
        }
    }
}