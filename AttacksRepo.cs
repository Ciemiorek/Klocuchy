using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static partial class AttacksRepo
    
    {
        public readonly static  Attack hit = new Attack(Type.Neutral,5, "hit", 90,"Normal punch, nothing special",1,AttackShape.Dot);
        public readonly static Attack acurateHit = new Attack(Type.Neutral, 3, "acurateHit", 110, "Normal punch, but more acurate", 1, AttackShape.Dot);
        public readonly static Attack SuperHIt = new Attack(Type.Neutral, 7, "SuperHIt", 70, "Strng but offten miss", 1, AttackShape.Dot);
         public readonly static Attack GretestOfAllHit = new Attack(Type.Neutral, 10, "GretestOfAllHit", 120, "Best normal hit ", 1, AttackShape.Dot);

}

