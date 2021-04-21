
namespace CarDealer.Models
{
    using System.Collections.Generic;

    public class Part
    {
        private int supplierId;

        public Part()
        {
            this.PartCars = new HashSet<PartCar>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int SupplierId { get => supplierId; 
            set => supplierId = value; }
        public Supplier Supplier { get; set; }

        public ICollection<PartCar> PartCars { get; set; }
    }
}
