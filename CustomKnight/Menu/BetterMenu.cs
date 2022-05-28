using CustomKinightRandom.Canvas;
using Satchel.BetterMenus;
using System.Linq;

namespace CustomKinightRandom
{
    internal static class BetterMenu
    {
        internal static int selectedSkin = 0;
        internal static Menu MenuRef;

        internal static void ApplySkin(){
            System.Random rng = new System.Random();
            var skinToApply = SkinManager.SkinsList[rng.Next(0, SkinManager.SkinsList.Count)];
            SkinManager.SetSkinById(skinToApply.GetId());
            SkinSwapperPanel.hidePanel("");
        }

        internal static void SelectedSkin(string skinId){
            selectedSkin = SkinManager.SkinsList.FindIndex( skin => skin.GetId() == skinId);
        }
        internal static void SetPreloadButton(){
            var btn = MenuRef.Find("PreloadButton");
            btn.Name = CustomKinightRandom.GlobalSettings.Preloads ? "Gameplay + Events" : "Gameplay only";
            btn.Update();
        }
        internal static void TogglePreloads(){
            CustomKinightRandom.GlobalSettings.Preloads = !CustomKinightRandom.GlobalSettings.Preloads;
            SetPreloadButton();
        }
        internal static void SetDumpButton(){
            //var btn = (MenuRef?.Find("AdditonalButtonGroup") as IShadowElement)?.GetElements()?.FirstOrDefault<Element>( e => e.Id == "DumpButton");
            var btn = (MenuRef?.Find("AdditonalButtonGroup") as MenuRow)?.Find("DumpButton");
            btn.Name = CustomKinightRandom.dumpManager.enabled ? "Dumping sprites" : "Dump sprites";
            btn.Update();
        }
        internal static void ToggleDumping(){
            CustomKinightRandom.dumpManager.enabled = !CustomKinightRandom.dumpManager.enabled;
            if(CustomKinightRandom.dumpManager.enabled){
                CustomKinightRandom.swapManager.Unload();
                CustomKinightRandom.dumpManager.dumpAllSprites();
            } else {
                CustomKinightRandom.swapManager.Load();
            }
            SetDumpButton();
        }

        internal static void DumpAll(){
            CustomKinightRandom.dumpManager.enabled = !CustomKinightRandom.dumpManager.enabled;
            CustomKinightRandom.dumpManager.walk();
        }

        private static void OpenSkins(){
            IoUtils.OpenDefault(SkinManager.SKINS_FOLDER);
        }

        private static void OpenLink(string link){ 
            Application.OpenURL(link);
        }
        private static void FixSkins(){ 
            FixSkinStructure.FixSkins();
            TextureCache.clearAllTextureCache(); // clear texture cache
            CustomKinightRandom.Instance.Log("Reapplying Skin");
            // reset skin folder so the same skin can be re-applied
            SkinManager.CurrentSkin = null;
            ApplySkin();
            UpdateSkinList();
        }

        internal static void UpdateSkinList(){
            SkinManager.getSkinNames();
            MenuRef.Find("SelectSkinOption").updateAfter((element)=>{
                ((HorizontalOption)element).Values = getSkinNameArray();
            });
        }
        internal static string[] getSkinNameArray(){
            return SkinManager.SkinsList.Select(s => SkinManager.MaxLength(s.GetName(),CustomKinightRandom.GlobalSettings.NameLength)).ToArray();
        }
        internal static Menu PrepareMenu(ModToggleDelegates toggleDelegates){
            return new Menu("Custom Knight",new Element[]{
                Blueprints.CreateToggle(toggleDelegates,"Custom Skins", "", "Enabled","Disabled"),
                new MenuButton("PreloadButton","Will Preload objects for modifying events",(_)=>TogglePreloads(),Id:"PreloadButton"),
                new MenuRow(
                    new List<Element>{
                        new MenuButton("Randomize Skin","Apply random skin.",(_)=> ApplySkin()),
                    },
                    Id:"ApplyButtonGroup"
                ){ XDelta = 0f},
                new MenuRow(
                    new List<Element>{
                        new MenuButton("Dump","Dumps the sprites that Swapper supports (Expect lag)",(_)=>ToggleDumping(),Id:"DumpButton"),
                        //new MenuButton("Generate Cache","Generates Cache for Everything (Can take hours)",(_)=>DumpAll(),Id:"DumpAllButton"),
                        //new MenuButton("Need Help?","Join the HK Modding Discord",(_)=>OpenLink("https://discord.gg/J4SV6NFxAA")),
                    },
                    Id:"AdditonalButtonGroup"
                ){ XDelta = 425f},
                
            });
        }
        internal static MenuScreen GetMenu(MenuScreen lastMenu, ModToggleDelegates? toggleDelegates){
            if(MenuRef == null){
                MenuRef = PrepareMenu((ModToggleDelegates)toggleDelegates);
            }
            MenuRef.OnBuilt += (_,Element) => {
                SetPreloadButton();
                SetDumpButton();
                if(SkinManager.CurrentSkin != null){
                    BetterMenu.SelectedSkin(SkinManager.CurrentSkin.GetId());
                }
            };
            return MenuRef.GetMenuScreen(lastMenu);
        }
    }
}