namespace PetShop.Application.DtosPet
{
    // DTO para mostrar detalles de la mascota disponible.
    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}