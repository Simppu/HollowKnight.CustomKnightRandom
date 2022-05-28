using static Satchel.GameObjectUtils;

namespace CustomKinightRandom
{
    public class ShadowDashBlobs : Skinable_Tk2d
    {
        public static string NAME = "ShadowDashBlobs";
        public ShadowDashBlobs() : base(ShadowDashBlobs.NAME){}
        public override Material GetMaterial(){
            return HeroController.instance.gameObject.FindGameObjectInChildren("Shadow Dash Blobs").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}