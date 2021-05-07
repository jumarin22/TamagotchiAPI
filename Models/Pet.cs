using System;

namespace TamagotchiAPI.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int HungerLevel { get; set; }
        public int HappinessLevel { get; set; }
        public DateTime LastInteractedWithDate { set; get; }

        // Adventure mode: Pet is dead if not interacted with for more than 3 days. 
        public bool IsDead
        {
            get
            {
                TimeSpan ts = DateTime.Now - LastInteractedWithDate;
                double NumberOfDays = ts.TotalDays;
                if (NumberOfDays > 3)
                    return true;

                return false;
            }

        }
    }
}