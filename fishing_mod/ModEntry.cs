using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;

namespace fishing_mod
{
    public class ModEntry : Mod
    {
        private Random rng;
        public override void Entry(IModHelper helper)
        {
            helper.Events.Display.MenuChanged += this.Display_MenuChanged;
            rng = new Random();
        }

        private void Display_MenuChanged(object sender, MenuChangedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;
            BobberBar bobberbar;
            if ((bobberbar = (e.NewMenu as BobberBar)) != null)
            {
                FishingRod fishingrod = Game1.player.CurrentTool as FishingRod;
                if (fishingrod != null)
                {
                    int wichFish = this.Helper.Reflection.GetField<int>(bobberbar, "whichFish", true).GetValue();
                    int fishSize = this.Helper.Reflection.GetField<int>(bobberbar, "fishSize", true).GetValue();
                    int fishQuality = this.Helper.Reflection.GetField<int>(bobberbar, "fishQuality", true).GetValue();
                    float fishDifficulty = this.Helper.Reflection.GetField<float>(bobberbar, "difficulty", true).GetValue();
                    bool perfect = true;
                    bool treasureCaught = this.Helper.Reflection.GetField<bool>(bobberbar, "treasureCaught", true).GetValue();
                    bool fromFishPond = this.Helper.Reflection.GetField<bool>(bobberbar, "fromFishPond", true).GetValue();
                    bool caughtDouble = rng.Next(0, 3) % 2 == 0; //NOTE: 1/3 to get 2 fish in one time
                    if (Game1.isFestival())
                        Game1.CurrentEvent.perfectFishing();
                    fishingrod.pullFishFromWater(wichFish, fishSize, fishQuality, (int)fishDifficulty, treasureCaught, perfect, fromFishPond, caughtDouble);
                    bobberbar.exitThisMenu(false);
                } 
            }
        }
    }
}
