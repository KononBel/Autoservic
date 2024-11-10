using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoservice.Models
{
    class Сarservice
    {
        private List<Part> _partsStorage;
        private Queue<Vehicle> _vehicles;

        private int _money;
        private int _repairCost;
        private int _forfeitUnrepairedCoeff;
        private int _forfeitRefusalRepair;

        public Сarservice(List<Part> partsStorage)
        {
            _partsStorage = partsStorage;
            _vehicles = new Queue<Vehicle>();

            _money = 0;
            _repairCost = 100;
            _forfeitUnrepairedCoeff = 2;
            _forfeitRefusalRepair = 250;
        }

        public Vehicle CurrentVehicle { get; private set; }

        public List<Vehicle> Vehicles => new List<Vehicle>(_vehicles);

        public int Money => _money;

        public void AcceptVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
                _vehicles.Enqueue(vehicle);
        }

        public void LiftVehicle()
        {
            CurrentVehicle = _vehicles.Dequeue();
        }

        public void Repair()
        {
            InspectVehicle();

            int partNumber = ConsoleReader.ReadInt("\nВведите номер детали, которую хотите заменить: ");
            
            if (partNumber > 0 && partNumber <= CurrentVehicle.Parts.Count)
            {
                Part part = CurrentVehicle.Parts[partNumber - 1];

                if (part != null)
                {
                    Part newPart = GetAvailablePart(part.Name);
                    if (newPart != null)
                    {
                        if (part.IsBroken)
                            _money += newPart.Cost + _repairCost;

                        CurrentVehicle.ReplacePart(part, newPart);
                        RemovePart(newPart);

                        Console.WriteLine($"Деталь {part.Name} заменена в {CurrentVehicle.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"Заменить {part.Name} не получится, эта деталь отсутсвует на складе.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Такой детали в {CurrentVehicle.Name} не обнаружено.");
            }
        }

        public void CompleteRepair(ref bool isWork)
        {
            int forfeit = 0;

            foreach (Part part in CurrentVehicle.Parts)
            {
                if (part.IsBroken)
                    forfeit += part.Cost / _forfeitUnrepairedCoeff;
            }

            _money -= forfeit;

            Console.WriteLine($"\nВы завершили ремонт {CurrentVehicle.Name}.\n" +
                $"Штраф за неотремонтированные детали составил: {forfeit}$");

            isWork = false;
        }

        public void BreakRepair(ref bool isWork)
        {
            _money -= _forfeitRefusalRepair;

            Console.WriteLine($"\nВы отказались от ремонта {CurrentVehicle.Name}.\n" +
                $"Штраф за отказ от ремонта составил: {_forfeitRefusalRepair}$");

            isWork = false;
        }

        private void InspectVehicle()
        {
            Console.WriteLine($"\nТехническое состояние {CurrentVehicle.Name}: \n");

            int partNumber = 1;

            foreach (Part part in CurrentVehicle.Parts)
            {
                string brokenStatusName = (part.IsBroken) ? "Сломана" : "Целая";

                Console.WriteLine($"{partNumber++}. {part.Name}: {brokenStatusName}");
            }
        }

        private Part GetAvailablePart(string partName)
        {
            return _partsStorage.Find(part => part.Name == partName);
        }

        private void RemovePart(Part part)
        {
            _partsStorage.Remove(part);
        }
    }
}