using static Satchel.GameObjectUtils;

namespace CustomKinightRandom
{
    public class HitPt : Skinable_Tk2ds
    {
        public static string NAME = "HitPt";
        public HitPt() : base(HitPt.NAME){}
        public override List<Material> GetMaterials(){
            GameObject hc = HeroController.instance.gameObject;
            return new List<Material>{
                hc.FindGameObjectInChildren("Hit Pt 1").GetComponent<ParticleSystemRenderer>().material,
                hc.FindGameObjectInChildren("Hit Pt 2").GetComponent<ParticleSystemRenderer>().material
            };
        }

    }
}