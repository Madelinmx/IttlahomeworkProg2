namespace PetShop.Domain.Entities
{
  
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; 
        public int Age { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAdopted { get; set; } = false; 
    }
}