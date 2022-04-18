using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Projektarbete
{
    class Program
    {

        static void Main(string[] args)
        {
            
            Console.SetWindowSize(100, 60);
            World world = new World(30, 60);
            world.GeneratePositions(12);
            world.PrintWorld();
            StartGame(world);
            
            
        }
        static void StartGame(World world)
        {
            InstantiatePlayer(world);
        }

        static void InstantiatePlayer(World world)
        {
            Point p = new Point(15, 15);
            var character = new Player(20, 14, 8, 8, 2, p); //Instantiering av spe  lare, värden kan ändras efter behov.
            

            PlayerMovement(world, character);
        }

        

        static void PlayerMovement(World world, Player character) //Kodstycke som säger åt konsolen hur "spelaren" rör sig runt på spelplanen.
        {

            var Map = world.Map;
            int Width = Map.GetLength(1);
            int Height = Map.GetLength(0);
            char player = character.RepChar;
            bool hasWon = false;

            int x = Width / 2;
            int y = Height / 2;
            Map[y, x] = player;
            Console.SetCursorPosition(x, y);
            Console.Write(player);
            character.DisplayBackpack(world.Width);
            
            while (true)
            {
                
                Map[y, x] = '.';
                Tuple<int, int> temp = new Tuple<int, int>(x, y); //Lagra koordinaten, utifall fienden inte besegras.

                Console.SetCursorPosition(x, y);
                var command = Console.ReadKey(true).Key;
                Console.Write(Map[y, x]);

                switch (command)
                {
                    case ConsoleKey.RightArrow:
                        if (x < Width - 2)
                            x++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x > 1)
                            x--;
                        break;
                    case ConsoleKey.UpArrow:
                        if (y > 1)
                            y--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (y < Height - 2)
                            y++;
                        break;
                    case ConsoleKey.Tab:
                        Console.SetCursorPosition(0, 33);
                        Console.Write("Select an item in packpack by index: ");

                        if (int.TryParse(Console.ReadLine(), out int index))
                        {
                            if (index < 1 || index > character.Backpack.Count)
                            { Console.WriteLine("That index is incorrect!"); Thread.Sleep(500); break; }
                            else
                                character.UseItem(index);
                        }
                        else
                            Console.Write("Invalid number");
                        character.DisplayBackpack(world.Width);
                        Thread.Sleep(1000);
                        Console.SetCursorPosition(0, 33);
                        Console.WriteLine("                                                 \n                            "); //Skriver över texten med mellanslag i brist på lämplig sudd-metod.
                        break;
                    default:
                        break;
                }
                if (Map[y, x] != '.')
                {
                    Entity entity = world.ListOfItemsAndEnemies.Find(e => (e.Position.X == x && e.Position.Y == y));
                    bool pass = Meet(character, entity, out bool hasFought);
                    if (hasFought)
                    {
                        if(pass)
                        {
                            world.ListOfItemsAndEnemies.Remove(world.ListOfItemsAndEnemies.Find(e => e.Position.X == x && e.Position.Y == y));
                        }    
                        Thread.Sleep(1500);
                        Console.Clear();
                        world.PrintWorld();
                    }
                    character.DisplayBackpack(world.Width);

                    if (!pass)
                    {
                        x = temp.Item1;
                        y = temp.Item2;
                
                    }

                }
                Map[y, x] = player;

                Console.SetCursorPosition(x, y);
                Console.Write(Map[y, x]);
                if (character.Health < 1)
                { break; }
                if (world.ListOfItemsAndEnemies.FindAll(e => e.Name == "Monster").Count < 1)
                { hasWon = true; break; }
                
            }
            Console.Clear();
            if (hasWon)
                Console.WriteLine("Congratulations! All your enemies have been defeated!");
            else
                Console.WriteLine("Boo! You lost!");
            string answ = "";
            while (answ != "y")
            {
                Console.WriteLine("Do you want to play again? (Y/N)");
                answ = Console.ReadLine().ToLower();
                if (answ == "y")
                    break;
                else if (answ == "n")
                    return;
                else
                    Console.WriteLine("Write 'Y' or 'N'");
            }
            RestartGame();
        }
        static void RestartGame()
        {
            Console.Clear();
            Main(null);
        }
        static bool Meet(Player player, Entity entity, out bool hasFought)
        {
            hasFought = false;
            bool pass = false;
            if (entity is Enemy)
            {
                if (Fight(player, entity as Enemy))
                    pass = true;  

                hasFought = true;
            }
            else
            {
                player.PickUp(entity as Item);
                pass = true;
            }

            return pass;

        }
        static bool Fight(Player player, Enemy enemy)
        {
            int counter = 1;
            Random rng = new Random();
            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.Clear();

                Console.WriteLine($"Round: {counter}" +
                                   $"\nYou have {player.Health} in health!" +
                                   $"\nYour enemy has {enemy.Health} in health");
                Console.WriteLine("Choose your action: \n1. Hit \n2. Kick \n3. Flee");
                while (true)
                {
                    ConsoleKey choice = Console.ReadKey(true).Key;

                    if (choice == ConsoleKey.D1 || choice == ConsoleKey.D2)
                    {
                        int damage = 0;
                        string attackType = "";
                        if (choice == ConsoleKey.D1)
                        { damage = player.Punch(rng, enemy.Armour); attackType = "hit"; }
                        else
                        { damage = player.Kick(rng, enemy.Armour); attackType = "kick"; }

                        enemy.Health -= damage;
                        Console.WriteLine($"You {attackType} the enemy, doing {damage} in damage!");

                        while (true)
                        {
                            Thread.Sleep(200);
                            if (enemy.Health > 0)
                            {
                                // int randomAttack = Random.next(5);
                                // damage = randomAttack < 2 ? enemy.Punch(rng, player.Armour) : enemy.Kick(rng, player.Armour);

                                damage = enemy.Punch(rng, player.Armour);
                                player.Health -= damage;
                                Console.WriteLine($"The dastardly foe has punched you, inflicting {damage} in damage on you!");
                                if (attackType == "kick")
                                { attackType = ""; continue; }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                        counter++;
                        break;
                    }

                    else if (choice == ConsoleKey.D3)
                    {
                        if (player.Initiative + rng.Next(1, 7) > enemy.Initiative + rng.Next(1, 7))
                        {
                            Console.WriteLine("You have managed to flee!");
                            Thread.Sleep(1500);
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("You did not manage to flee! The fight continues");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Choose option 1,2 or 3");
                    }
                }
                Thread.Sleep(1500);
            }
            //Console.SetCursorPosition(0, 32); //Så att den inte skriver ut meddelandet på kartan!
            if (player.Health > 0)
            { Console.WriteLine("You have bested your foe!"); return true; }
            else
            { Console.WriteLine("You have died in shame from a mere punch!"); return false; }

        }

    }
}

