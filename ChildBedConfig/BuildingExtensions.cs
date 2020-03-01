using StardewValley;
using xTile.Tiles;

namespace ChildBedConfig
{
    static class BuildingExtensions
    {
        internal static void RemoveCrib(this GameLocation building)
        {
            building.removeTiles("Buildings", 15, 3, 17, 5); //Get rid of the tiles from the building layer
            building.removeTiles("Front", 15, 2, 17, 4); //Get rid of the tiles from the front layer

            int wallpaper = building.getTileIndexAt(18, 3, "Buildings"); //get the wallpaper id so we can plaster it onto the tiles we're adding
            int flooring = building.getTileIndexAt(19, 3, "Back"); //get the flooring id too

            StaticTile WallpaperTile = new StaticTile(building.Map.GetLayer("Buildings"), building.Map.GetTileSheet("walls_and_floors"), BlendMode.Alpha, wallpaper);
            StaticTile FlooringTile = new StaticTile(building.Map.GetLayer("Back"), building.Map.GetTileSheet("walls_and_floors"), BlendMode.Alpha, flooring);

            //Add wall tiles to 15 3, 16 3, and 17 3 on the Buildings layer, which were previously occupied by the crib
            building.Map.GetLayer("Buildings").Tiles[15, 3] = WallpaperTile;
            building.Map.GetLayer("Buildings").Tiles[16, 3] = WallpaperTile;
            building.Map.GetLayer("Buildings").Tiles[17, 3] = WallpaperTile;

            //Add fooring tiles to the back layer where the crib was -- we'll have a weird black line there otherwise
            building.Map.GetLayer("Back").Tiles[15, 3] = FlooringTile;
            building.Map.GetLayer("Back").Tiles[16, 3] = FlooringTile;
            building.Map.GetLayer("Back").Tiles[17, 3] = FlooringTile;
        }

        internal static void RemoveLeftBed(this GameLocation building)
        {
            removeTiles(building, "Buildings", 22, 4, 23, 6); //Get rid of the tiles from the building layer                    
            removeTiles(building, "Front", 22, 3, 23, 5); //Get rid of the tiles from the front layer
        }

        internal static void RemoveRightBed(this GameLocation building)
        {
            removeTiles(building, "Buildings", 26, 4, 27, 6); //Get rid of the tiles from the building layer                    
            removeTiles(building, "Front", 26, 3, 27, 5); //Get rid of the tiles from the front layer
        }

        private static void removeTiles(this GameLocation location, string layer, int startX, int startY, int endX, int endY)
        {
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    location.removeTile(x, y, layer);
                }
            }
        }
    }
}
