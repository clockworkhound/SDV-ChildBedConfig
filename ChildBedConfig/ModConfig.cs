using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildBedConfig
{
    class ModConfig
    {
        /*****************************/
        /**      Properties         **/
        /*****************************/
        //public List<Farmer> Farmers { get; set; }

        public List<Farm> Farms { get; set; } = new List<Farm>() { };
    }

    /// <summary>
    /// Class for the farm -- we have a list of them in the config
    /// </summary>
    class Farm
    {
        public string FarmName { get; set; } = "";
        public Dictionary<string, Farmer> FarmerInfo { get; set; } = new Dictionary<string, Farmer>() { };
    }

    /// <summary>
    /// Class for the farmer -- we have a list of them in the Farm object in the config
    /// </summary>
    class Farmer
    {
        /*****************************/
        /**      Properties         **/
        /*****************************/

        ///<summary>Determines whether or not to show the crib</summary>
        public bool ShowCrib { get; set; } = true;

        ///<summary>Determines whether or not to show the bed closest to the crib</summary>
        public bool ShowLeftBed { get; set; } = true;

        ///<summary>Determines whether or not to show the bed furthest from the crib</summary>
        public bool ShowRightBed { get; set; } = true;
        
    }
}
