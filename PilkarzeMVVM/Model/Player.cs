using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilkarzeMVVM.Model
{
    class Player
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public uint Age { get; set; }
        public uint Weight { get; set; }
        public Player(string name, string surname, uint age, uint weight)
        {
            Name = name;
            Surname = surname;
            Age = age;
            Weight = weight;
        }
        public Player()
        {
            Name = null;
            Surname = null;
            Age = 25;
            Weight = 75;
        }
        public override string ToString()
        {
            return $"{Surname} {Name} lat: {Age} waga: {Weight} kg"; ;
        }
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Player p = (Player)obj;
                return (p.Age == this.Age) && (p.Weight == this.Weight) && (p.Name == this.Name) && (p.Surname == this.Surname);
            }
        }
    }
}
