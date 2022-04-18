using System.Drawing;


namespace Projektarbete
{
    class Item : Entity
    {
        
        //Värden för items.

        public int HealthBoost { get; protected set; }

        public int StrenghtBoost { get; protected set; }

        public int DefenseBoost { get; protected set; }

        //Konstruktor för items.
        public Item(string name, int healthboost, int strenghtboost, int defenseboost, Point position, char repChar = '@'):base (repChar, position)
        {
            Name = name;
            HealthBoost = healthboost;
            StrenghtBoost = strenghtboost;
            DefenseBoost = defenseboost;    
        }
    }
}
