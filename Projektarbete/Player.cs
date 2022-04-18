using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Projektarbete
{
    class Player : Creature
    {
        public List<Item> Backpack { get; protected set; }
        public Item EquippedWeapon { get; protected set; }
        public Item EquippedArmour { get; protected set; }
        public Player(int health, int strength, int armour, int initiative, int luck, Point position, char repChar = '¤') : base(health, strength, armour, initiative, luck, position, repChar)
        {
            Backpack = new List<Item>();
            Point p = new Point(-1, -1);
            EquippedWeapon = new Item(" ", 0, 0, 0, p);
            EquippedArmour = EquippedWeapon;
        }

        public string ShowStats()
        {
            string s = $" ,Player stats: ," + //INTE ett fel att komma är där!
                       $"Health: {Health} ," +
                       $"Strength: {Strength} ," +
                       $"Armour: {Armour} ," +
                       $"Initiative: {Initiative} ," +
                       $"Luck: {Luck} ,";
                         
            if (EquippedArmour.Name != " ")
                s += $"Equipped Armour: {EquippedArmour.Name} ,";
            if (EquippedWeapon.Name != " ")
                s += $"Equipped Weapon: {EquippedWeapon.Name} ,";

            return s;

        }
        public void PickUp(Item item)
        {
            Backpack.Add(item);
        }
        public void UseItem(int index)
        {
            index--; //Så att om man väljer 1, så kommer man att välja första föremålet i listan. Annars måste man välja 0.
            string itemName = Backpack[index].Name;
            switch (itemName)
            {
                case "apple +7 hp":
                case "banana +8 hp":
                case "orange +5 hp":
                case "pear +4 hp":
                    Health += Backpack[index].HealthBoost;
                    Backpack.RemoveAt(index);
                    break;
                case "shield +2 arm":
                    Armour -= EquippedArmour.DefenseBoost;
                    EquippedArmour = Backpack.ElementAt(index);
                    Armour += EquippedArmour.DefenseBoost;
                    break;
                case "sword +5 str":
                case "knife +2 str":
                    Strength -= EquippedWeapon.StrenghtBoost;
                    EquippedWeapon = Backpack.ElementAt(index);
                    Strength += EquippedWeapon.StrenghtBoost;
                    break;
                default:
                    break;
            }

        }
        public void DisplayBackpack( int left, int top = 0)
        {
            left += 3;
            Console.SetCursorPosition(left, top);
            Console.WriteLine("Backpack: To use item press TAB button on keyboard. ");
            foreach (var item in Backpack)
            {
                top++;
                Console.SetCursorPosition(left, top);
                Console.Write("                        ");
                Console.SetCursorPosition(left, top);
                Console.WriteLine(top + ". " + item.Name);

            }
            Console.WriteLine();
            foreach (var s in ShowStats().Split(','))
            {
                top++;
                Console.SetCursorPosition(left, top);
                Console.Write("                               ");
                Console.SetCursorPosition(left, top);
                Console.WriteLine(s);
                
            }
            top++;
            Console.SetCursorPosition(left, top);
            Console.Write("                               ");
            Console.SetCursorPosition(left, top);
            Console.WriteLine("Symbols ");
            top++;
            Console.SetCursorPosition(left, top);
            Console.Write("                               ");
            Console.SetCursorPosition(left, top);
            Console.WriteLine("¤ = Player"); 
            top++;
            Console.SetCursorPosition(left, top);
            Console.Write("                               ");
            Console.SetCursorPosition(left, top);
            Console.WriteLine("% = Enemy");
            top++;
            Console.SetCursorPosition(left, top);
            Console.Write("                               ");
            Console.SetCursorPosition(left, top);
            Console.WriteLine("@ = Item ");
        }



    }
}
