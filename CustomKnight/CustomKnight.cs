﻿using CustomKinightRandom.Canvas;

namespace CustomKinightRandom
{
    public class CustomKinightRandom : Mod,  IGlobalSettings<GlobalModSettings>, ILocalSettings<SaveModSettings>,ICustomMenuMod , ITogglableMod
    {
        public static GlobalModSettings GlobalSettings { get; set; } = new GlobalModSettings();
        public static SaveModSettings SaveSettings { get; set; } = new SaveModSettings();
        public static CustomKinightRandom Instance { get; private set; }
        public static DumpManager dumpManager {get; private set;} = new DumpManager();
        public static SwapManager swapManager {get; private set;} = new SwapManager();

        public static readonly Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>();
        internal static void touchSatchelVersion(){
            Satchel.AssemblyUtils.Version();
        }
        internal static bool isSatchelInstalled(){
            var isInstalled = false;
            try{
                touchSatchelVersion();
                isInstalled = true;
            } catch (Exception e){
                Modding.Logger.Log(e);
            }
            return isInstalled;
        }
        internal void getVersionSafely(){
            version = Satchel.AssemblyUtils.GetAssemblyVersionHash();
        }
        public string version;
        new public string GetName() => "Custom Knight";
        public override string GetVersion(){
            version = "Satchel not found";
            if(isSatchelInstalled()){
                getVersionSafely();
            }
            return version;
        }
        public void OnLoadGlobal(GlobalModSettings s)
        {
            if(s.Version == GetVersion()){
                CustomKinightRandom.GlobalSettings = s;
            } else {
                var DefaultSettings = new GlobalModSettings();
                CustomKinightRandom.GlobalSettings = s;
                CustomKinightRandom.GlobalSettings.Version = GetVersion();
                CustomKinightRandom.GlobalSettings.NameLength = DefaultSettings.NameLength;
            }
        }

        public GlobalModSettings OnSaveGlobal()
        {
            return CustomKinightRandom.GlobalSettings;
        }

        public override List<(string, string)> GetPreloadNames()
        {
            if (GlobalSettings.Preloads)
            {
                return new List<(string, string)>
                {
                    ("Abyss_10", "higher_being/Dish Plat/Knight Dummy"),
                    ("Abyss_12", "Scream 2 Get/Cutscene Knight"),
                    ("Abyss_21", "Shiny Item DJ/Knight Cutscene"),
                    ("Crossroads_50", "Quirrel Lakeside/Sit Region/Knight Sit"),
                    ("Deepnest_East_12", "Hornet Blizzard Return Scene"),
                    ("Deepnest_Spider_Town", "RestBench Spider/Webbed Knight"),
                    ("Dream_Abyss", "End Cutscene/Dummy"),
                    ("GG_Door_5_Finale", "abyss_door_5_cutscene_sequence/main_chars"),
                    ("GG_Vengefly", "Boss Scene Controller/Dream Entry/Knight Dream Arrival"),
                    ("RestingGrounds_07", "Dream Moth/Knight Dummy"),
                };   
            }
            
            return new List<(string, string)>();
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log($"Initializing CustomKinightRandom {version}");
            if (Instance == null) 
            { 
                Instance = this;
            }
            if(!isSatchelInstalled()){
                return;
            }
            SkinManager.checkDirectoryStructure();

            SkinManager.getSkinNames();             
            SkinManager.CurrentSkin = SkinManager.GetSkinById(CustomKinightRandom.GlobalSettings.DefaultSkin);
            swapManager.SkinChangeSwap(SkinManager.CurrentSkin);
            // Initial load
            if (preloadedObjects != null)
            {
                GameObjects.Add("Cloak", preloadedObjects["Abyss_10"]["higher_being/Dish Plat/Knight Dummy"]);
                GameObjects.Add("Shriek", preloadedObjects["Abyss_12"]["Scream 2 Get/Cutscene Knight"]);
                GameObjects.Add("Wings", preloadedObjects["Abyss_21"]["Shiny Item DJ/Knight Cutscene"]);
                GameObjects.Add("Quirrel", preloadedObjects["Crossroads_50"]["Quirrel Lakeside/Sit Region/Knight Sit"]);
                GameObjects.Add("Hornet", preloadedObjects["Deepnest_East_12"]["Hornet Blizzard Return Scene"]);
                GameObjects.Add("Webbed", preloadedObjects["Deepnest_Spider_Town"]["RestBench Spider/Webbed Knight"]);
                GameObjects.Add("Birthplace", preloadedObjects["Dream_Abyss"]["End Cutscene/Dummy"]);
                GameObjects.Add("DreamArrival", preloadedObjects["GG_Vengefly"]["Boss Scene Controller/Dream Entry/Knight Dream Arrival"]);
                GameObjects.Add("Dreamnail", preloadedObjects["RestingGrounds_07"]["Dream Moth/Knight Dummy"]);
            }
            if(CustomKinightRandom.GlobalSettings.SwapperEnabled){
                swapManager.enabled = true;
                swapManager.active = true;
            }


            GUIController.Instance.BuildMenus();
            On.HeroController.Start += HeroControllerStart;
        }

        public  bool ToggleButtonInsideMenu {get;}= true;
        public MenuScreen GetMenuScreen(MenuScreen modListMenu,ModToggleDelegates? toggle){
            return BetterMenu.GetMenu(modListMenu,toggle);
        }

        public static event EventHandler<EventArgs> OnReady;

        public void HeroControllerStart(On.HeroController.orig_Start orig,HeroController self){
            orig(self);
            Log("HeroControllerStart");
            var currentSkinId = ( SaveSettings.DefaultSkin != GlobalSettings.DefaultSkin && SaveSettings.DefaultSkin != null ) ? SaveSettings.DefaultSkin : GlobalSettings.DefaultSkin;
            SkinManager.CurrentSkin = SkinManager.GetSkinById(currentSkinId);
            SaveSettings.DefaultSkin = SkinManager.CurrentSkin.GetId();
            BetterMenu.SelectedSkin(SkinManager.CurrentSkin.GetId());
            SkinManager.LoadSkin();
            OnReady?.Invoke(this,null);
        }
        public void OnLoadLocal(SaveModSettings s)
        {
            CustomKinightRandom.SaveSettings = s;
        }

        internal static void toggleSwap(bool enable){
            swapManager.enabled = enable;
            if(!enable){
                swapManager.Unload();
                dumpManager.enabled = false;
            } else {
                swapManager.Load();
            }
        }

        internal static void toggleDump(bool enable){
            dumpManager.enabled = enable;
        }

        public void Unload(){
            SkinManager.Unload();
            On.HeroController.Start -= HeroControllerStart;
        }
        public SaveModSettings OnSaveLocal()
        {
            return CustomKinightRandom.SaveSettings;
        }
        
    }
}
