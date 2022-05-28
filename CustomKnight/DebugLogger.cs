
namespace CustomKinightRandom
{
    public static class DebugLogger{
        public static bool enabled = false; 

        internal static void Log(string s){
            if(enabled){
                CustomKinightRandom.Instance.Log(s);
            }
        }
    }
}