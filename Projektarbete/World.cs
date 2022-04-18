using System;
using System.Collections.Generic;
using System.Drawing;


namespace Projektarbete
{
    class World
    {
        //Klassmedlemmar:
        public char[,] Map { get; protected set; } //kartans rutnät. Protected set innebär att bara denna klass och underklasser har tillgång till att direkt tilldela denna egenskap ett värde.
        public int Width { get; protected set; } //Så att man kan använda sig av width och height senare.
        public int Height { get; protected set; }
        public List<Entity> ListOfItemsAndEnemies { get; protected set; }
        public World(int height, int width) //Konstruktor för att generera världen.
        {
            Map = new char[height, width]; 
            Width = Map.GetLength(1); 
            Height = Map.GetLength(0);
            ListOfItemsAndEnemies = new List<Entity>();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == 0 || i == Height - 1)
                        Map[i, j] = '#';
                    else if (j == 0 || j == Width - 1) //Enbart för att det ska bli mindre plottrigt. Annars kan man kombinera båda if-satserna
                        Map[i, j] = '#';
                    else
                        Map[i, j] = '.';
                }

            }
            
        }
        //Metoder
        public void GeneratePositions(int numberOfEntities) //Bestämmer punkter för objekt och fiender.
        {
            Random r = new Random();
            int x;
            int y;
            Point p;
            List<Item> items = GenerateItems();
            while (true)
            {
                x = r.Next(1, Width);
                y = r.Next(1, Height);
                if (Map[y,x] == '.')
                {
                     //placeholder. för att ha någonting i kodandets stund.
                    p = new Point(x, y);
                    Entity entity = new Enemy(15, 13, 10, 5, 2, p); //När det finns en konstruktor för Enemy måste detta fixas. Samma gäller för Item nedan.
                    
                    if(numberOfEntities <= items.Count)
                    {
                        entity = items[0];
                        entity.Position = p;
                        items.RemoveAt(0);
                    }
                    Map[y, x] = entity.RepChar;
                    ListOfItemsAndEnemies.Add(entity);
                    numberOfEntities--;
                }
                if (numberOfEntities < 1)
                    break;
                
                
            }
        }
        public static List<Item> GenerateItems()
        {
            //Kod för att generera items
            Point p = new Point(0, 0); //Behöver finnas där just nu.
            var listOfGeneratedItems = new List<Item>();
            Item apple = new Item("apple +7 hp", 7, 0, 0, p);//Genererar ett äpple.
            listOfGeneratedItems.Add(apple);

            Item orange = new Item("orange +5 hp", 5, 0, 0, p);
            listOfGeneratedItems.Add(orange);//Genererar en apelsin.

            Item pear = new Item("pear +4 hp", 4, 0, 0, p);
            listOfGeneratedItems.Add(pear);

            Item banana = new Item("banana +8 hp", 8, 0, 0, p);
            listOfGeneratedItems.Add(banana);//Genererar en banan.

            Item sword = new Item("sword +5 str", 0, 5, 0, p);
            listOfGeneratedItems.Add(sword);

            Item knife = new Item("knife +2 str", 0, 2, 0, p);
            listOfGeneratedItems.Add(knife);

            Item shield = new Item("shield +2 arm", 0, 0, 2, p);
            listOfGeneratedItems.Add(shield);

            return listOfGeneratedItems;
        }

        public void PrintWorld()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}
