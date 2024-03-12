using MyTraining.Classes;

public class Agenda
{
	private readonly List<Contacto> contactoList;
	private readonly IContactoFactory factory;

	public Agenda(List<Contacto> contactoList, IContactoFactory factory)
	{
		this.contactoList = contactoList;
		this.factory = factory;
	}

	public void AgregarContacto(string nombre, string numero)
	{
		contactoList.Add(factory.CrearContacto("Contacto", 0, nombre, numero));
	}

	public void AgregarContacto(string tipoContacto, string nombre, string numero)
	{
		contactoList.Add(factory.CrearContacto(tipoContacto, 0, nombre, numero));
	}

	public void OrdenarContactos(Func<Contacto, Contacto, int> comparador)
	{
		contactoList.Sort((c1, c2) => comparador(c1, c2));
	}

	public List<Contacto> FiltrarContactos(Func<Contacto, bool> criterio)
	{
		Predicate<Contacto> predicado = new Predicate<Contacto>(criterio);
		return contactoList.FindAll(predicado);
	}

	public List<TResult> TransformarContactos<TResult>(Func<Contacto, TResult> transformacion) => contactoList.Select(transformacion).ToList();

	public bool ValidarContacto(Func<Contacto, bool> validacion) => contactoList.All(validacion);

	public void ImprimirContactos(Action<Contacto> imprimir)
	{
		contactoList.ForEach(c => imprimir(c));
	}
}