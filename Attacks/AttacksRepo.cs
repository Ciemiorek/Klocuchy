using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public   class AttacksRepo 

{
        public   Attack hit = new Attack(Type.Neutral,5, "hit", 90,"Normal punch, nothing special",1,AttackShape.Dot);
        public Attack acurateHit = new Attack(Type.Neutral, 3, "acurateHit", 110, "Normal punch, but more acurate", 1, AttackShape.Dot);
        public  Attack SuperHIt = new Attack(Type.Neutral, 7, "SuperHIt", 70, "Strng but offten miss", 1, AttackShape.Dot);
        public   Attack GretestOfAllHit = new Attack(Type.Neutral, 10, "GretestOfAllHit", 120, "Best normal hit ", 1, AttackShape.Dot);

        public List<Attack> listOfAttacks = new List<Attack>();

    public AttacksRepo()
    {
        listOfAttacks.Add(hit);
        listOfAttacks.Add(acurateHit);
        listOfAttacks.Add(SuperHIt);
        listOfAttacks.Add(GretestOfAllHit);




    }


    public  Attack getAttackByName(String name)
    {

        return listOfAttacks.First(a => a.attackName == name);
    }

    public  String name() {

        return listOfAttacks[0].Name;
    }

}

 