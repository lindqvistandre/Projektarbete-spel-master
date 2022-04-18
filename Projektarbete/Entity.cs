using System.Drawing;

namespace Projektarbete
{
    abstract class Entity
    {
        public Point Position { get; set; } //Så att varje föremål och fiende har en fix plats på i världen.
        public char RepChar { get; set; }
        private string name = "Monster";
        public string Name{ get { return name; } protected set { name = value; } }

        public Entity(char repChar, Point position)
        {
            RepChar = repChar;
            Position = position;
        }

    }
}
