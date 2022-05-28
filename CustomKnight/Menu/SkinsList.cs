using Satchel.BetterMenus;

namespace CustomKinightRandom
{
    internal static class SkinsList
    {
        internal static Menu MenuRef;
        internal static MenuScreen lastMenu;
        private static bool applying = false;

        private static void setSkinButtonVisibility(bool isVisible){
           for(var i = 0; i < SkinManager.SkinsList.Count ; i++){
                var btn = MenuRef?.Find($"skinbutton{SkinManager.SkinsList[i].GetId()}");
                if(btn != null){
                    btn.isVisible = isVisible;
                }
            }
            MenuRef.Update();
        }

        private static IEnumerator applyAndGoBack()
        {
            //update menu ui
            MenuRef.Find("helptext").isVisible = false;
            MenuRef.Find("applying").isVisible = true;
            setSkinButtonVisibility(false);
            yield return new WaitForSecondsRealtime(0.2f);
            BetterMenu.MenuRef?.Find("SelectSkinOption")?.updateAfter(_ => {
                    try {
                        BetterMenu.ApplySkin();
                    } catch(Exception e){
                        CustomKinightRandom.Instance.Log(e.ToString());
                    }
                }
            );
            yield return new WaitForSecondsRealtime(0.2f);
            UIManager.instance.UIGoToDynamicMenu(lastMenu);  
            yield return new WaitForSecondsRealtime(0.2f);

            //menu ui initial state
            MenuRef.Find("helptext").isVisible = true;
            MenuRef.Find("applying").isVisible = false;
            setSkinButtonVisibility(true);          
        }

        internal static MenuButton ApplySkinButton(int index){
            var ButtonText = SkinManager.MaxLength(SkinManager.SkinsList[index].GetName(),CustomKinightRandom.GlobalSettings.NameLength);
            return new MenuButton(ButtonText,"",(mb) => {
                    if(!applying){
                        applying = true;
                        // apply the skin
                        BetterMenu.selectedSkin = index;
                        CoroutineHelper.GetRunner().StartCoroutine(applyAndGoBack());
                    }
                },Id:$"skinbutton{SkinManager.SkinsList[index].GetId()}");
        }
        internal static Menu PrepareMenu(){
            var menu = new Menu("Select a skin",new Element[]{
                new TextPanel("Select the Skin to Apply",Id:"helptext"),
                new TextPanel("Applying skin...",Id:"applying"){isVisible=false}
            });
            for(var i = 0; i < SkinManager.SkinsList.Count ; i++){
                menu.AddElement(ApplySkinButton(i));
            }
            return menu;
        }
        internal static MenuScreen GetMenu(MenuScreen lastMenu){
            if(MenuRef == null){
                MenuRef = PrepareMenu();
            }
            
            applying = false;
            SkinsList.lastMenu = lastMenu;
            return MenuRef.GetMenuScreen(lastMenu);
        }
    }
}