using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Godot;


public partial class Attack : Node3D
{ 
        public Type attackType { get; set; }
        public int baseDMG { get; set; }

        public string attackName { get; set; }  
        
        //where 100 is hit and 0 is miss
        public int accuracy { get; set; }
        public string attackDescripton { get; set; }
        
        //where 1 is close to pawn and 2 is 1 apart from pawn 
        public int range { get; set; }

        public AttackShape  attackShape { get; set; }



    public Attack(Type attackType, int baseDMG, string attackName, int accuracy, string attackDescripton, int range, AttackShape attackShape)
    {
        this.attackType = attackType;
        this.baseDMG = baseDMG;
        this.attackName = attackName;
        this.accuracy = accuracy;
        this.attackDescripton = attackDescripton;
        this.range = range;
        this.attackShape = attackShape;
    }
}

