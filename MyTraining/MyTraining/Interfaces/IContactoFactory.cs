using MyTraining.Classes;

public interface IContactoFactory
{
	Contacto CrearContacto(string tipo, int id, string nombre, string numero);
}