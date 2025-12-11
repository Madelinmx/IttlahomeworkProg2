namespace PetShop.Application.DtosPet
{
    // DTO para la creación de una nueva mascota (uso interno del administrador).
    public class PetCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}