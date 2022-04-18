using System;
using System.Drawing;

namespace Projektarbete
{
    class Creature : Entity
    {
        public int Health { get; set; }
        public int Strength { get; protected set; }
        public int Armour { get; protected set; }
        public int Initiative { get; protected set; }
        public int Luck { get; protected set; }

        public Creature(int health, int strength, int armour, int initiative, int luck, Point position, char repChar) : base( repChar, position)
        {
            Health = health;
            Strength = strength;
            Armour = armour;
            Initiative = initiative;
            Luck = luck;
        }

        
        

        public int Punch(Random rng, int armour)
        {
            int damage = 0;
            int luckModifier = rng.Next(-3 + Luck, 0);
            damage = luckModifier + Strength - armour;
            if (damage < 1)
                damage = 0;
            return damage;
        }
        public int Kick(Random rng, int armour)
        {
            int damage = 0;
            int luckModifier = rng.Next(-3 + Luck, 0);
            damage = luckModifier + (Strength * 3 / 2) - armour;
            if (damage < 1)
                damage = 0;
            return damage;
        }

    }


}
