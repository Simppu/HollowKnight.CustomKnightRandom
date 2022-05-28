using static Satchel.GameObjectUtils;

namespace CustomKinightRandom
{
    public class Baldur : Skinable_Tk2d
    {
        public static string NAME = "Baldur";
        public Baldur() : base(Baldur.NAME){}
        public override Material GetMaterial(){
            GameObject hc = HeroController.instance.gameObject;
            GameObject charmEffects = hc.FindGameObjectInChildren("Charm Effects");
            GameObject baldur = charmEffects.FindGameObjectInChildren("Blocker Shield").FindGameObjectInChildren("Shell Anim");
            var _baldurMat = baldur.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material;
            
            return _baldurMat;
        }

    }
}