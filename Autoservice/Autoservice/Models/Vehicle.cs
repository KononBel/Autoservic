using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoservice.Models
{
    class Vehicle
    {
        private List<Part> _parts;
        private Random _random;

        public Vehicle(string name, List<Part> parts)
        {
            _random = new Random(Guid.NewGuid().GetHashCode());

            Name = name;
            _parts = parts;

            BreakDown();
        }

        public string Name { get; private set; }

        public List<Part> Parts => new List<Part>(_parts);

        public void ReplacePart(Part replacedPart, Part newPart)
        {
            _parts.Remove(replacedPart);
            _parts.Add(newPart);
        }

        private void BreakDown()
        {
            foreach (Part part in _parts)
            {
                if (_random.Next(0, 10) % 2 == 0)
                    part.GoBad();
            }
        }
    }
}
