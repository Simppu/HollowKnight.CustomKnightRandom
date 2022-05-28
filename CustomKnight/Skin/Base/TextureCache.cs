namespace CustomKinightRandom
{
    internal class TextureCache { // used for reducing disk reads when switching skins back and forth
        internal static Dictionary<string,Dictionary<string,CustomKinightRandomTexture>> skinTextureCache = new Dictionary<string,Dictionary<string,CustomKinightRandomTexture>>();
        internal static List<string> recentSkins = new List<string>();
        internal static void setSkinTextureCache(string skin,string filename,CustomKinightRandomTexture texture){
            if(!skinTextureCache.ContainsKey(skin)){
                 skinTextureCache[skin] = new Dictionary<string,CustomKinightRandomTexture>();
            }
            skinTextureCache[skin][filename] = texture;
        }
        internal static void clearSkinTextureCache(string skin){
            foreach(var kvp in skinTextureCache[skin]){
                GameObject.Destroy(kvp.Value.tex);
            }
            skinTextureCache.Remove(skin);
        }
        internal static void trimTextureCache(){
            if(recentSkins.Count > CustomKinightRandom.GlobalSettings.MaxSkinCache ){ 
                recentSkins = recentSkins.GetRange(recentSkins.Count - CustomKinightRandom.GlobalSettings.MaxSkinCache,CustomKinightRandom.GlobalSettings.MaxSkinCache);
            }
            var toClear = new List<string>();
            foreach(var kvp in skinTextureCache){
                if(!recentSkins.Contains(kvp.Key)){
                    toClear.Add(kvp.Key);
                }
            }
            foreach(var x in toClear){
                clearSkinTextureCache(x);
            }
        }
        internal static void clearAllTextureCache(){
            var toClear = new List<string>();
            foreach(var kvp in skinTextureCache){
                toClear.Add(kvp.Key);
            }
            foreach(var x in toClear){
                clearSkinTextureCache(x);
            }
        }

    }
}
