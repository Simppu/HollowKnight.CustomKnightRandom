using Modding;

namespace CustomKnight
{
    public class SaveModSettings  {
        public string DefaultSkin {get; set;} = "Default";
     }

    public class GlobalModSettings 
    {

        public bool Preloads {get; set;} = true;
        public string DefaultSkin {get; set;} = "Default";
        public int NameLength {get; set;} = 15;

        public bool showMovedText {get; set;} = true;

    }

}