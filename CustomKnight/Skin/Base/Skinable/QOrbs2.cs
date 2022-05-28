using static Satchel.GameObjectUtils;

namespace CustomKinightRandom
{
    public class QOrbs2 : Skinable_Tk2d
    {
        public static string NAME = "QOrbs2";
        public QOrbs2() : base(QOrbs2.NAME){}
        public override Material GetMaterial(){
            return HeroController.instance.gameObject.FindGameObjectInChildren("Q Orbs 2").GetComponent<ParticleSystemRenderer>().material;
        }

    }
}