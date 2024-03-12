using log4net;
using log4net.Config;
using MyTraining.Classes;
using MyTraining.Factories;

public static partial class Program
{
	public static void Main(string[] args)
	{ 
		ILog log;
		IList<Contacto> contactos = new List<Contacto>();
		IContactoFactory factory = new ContactoFactory();

		var agenda = new Agenda(contactos.ToList(), factory);

		var fileInfo = new FileInfo("log4net.config");
		if (fileInfo.Exists)
		{
			XmlConfigurator.Configure(fileInfo);
		}
		else
		{
			Console.WriteLine("---------------------------------------------- Error cargando configuración de logs. ");
		}
		log = LogManager.GetLogger(typeof(Program));
		log.Info("=============================== Agreando contactos ===============================");

		agenda.AgregarContacto("Juan", "1234567890");
		agenda.AgregarContacto("ContactoConInfoAdicional", "María", "3456789012");
		agenda.AgregarContacto("Daniela", "3456789012");
		agenda.AgregarContacto("ContactoConInfoElectronica", "Luis", "7890123456");
		agenda.AgregarContacto("ContactoConInfoAdicional", "Carlos", "9876543210");
		agenda.AgregarContacto("Ernesto", "9876543210");
		agenda.AgregarContacto("ContactoConInfoElectronica", "Ana", "5432109876");

		Func<Contacto, bool> criterio = c => c.Nombre.Contains("Mar");
		Func<string, string> formateador = t => $"{t.Substring(0, 3)}-{t.Substring(3, 3)}-{t.Substring(6)}";
		Func<string, string> formateador_2 = t => $"{t.Substring(0, 2)}-{t.Substring(2, 2)}-{t.Substring(4, 2)}-{t.Substring(6)}";

		agenda.OrdenarContactos((c1, c2) => string.Compare(c1.Nombre, c2.Nombre));

		Console.WriteLine("Contactos que contienen 'Mar':");

		var encontrados = agenda.FiltrarContactos(criterio);

		var numeros = agenda.TransformarContactos(c => c.Numero);

		bool numeros_validos = agenda.ValidarContacto(c => c.Numero.All(char.IsDigit) && c.Numero.Length.Equals(10));

		foreach (var contacto in encontrados)
		{
			Console.WriteLine($"Nombre: {contacto.Nombre} - Teléfono: {contacto.Numero}");
		}

		Console.WriteLine("Todos los contactos:");

		agenda.ImprimirContactos(contacto =>
		{
			Console.WriteLine($"Nombre: {contacto.Nombre} - Teléfono: {contacto.Numero}");
			if (contacto is ContactoConInfoAdicional)
			{
				Console.WriteLine($"\tDirección: {(contacto as ContactoConInfoAdicional).Direccion}");
			} else if (contacto is ContactoConInfoElectronica)
			{
				Console.WriteLine($"\te-mail: {(contacto as ContactoConInfoElectronica).EMail}");
				Console.WriteLine($"\tTwitter (X): {(contacto as ContactoConInfoElectronica).Twitter}");
				Console.WriteLine($"\tFacebook: {(contacto as ContactoConInfoElectronica).FaceBook}");
			}

		});

		numeros.ForEach(c => Console.WriteLine($"Número: {c}"));

		numeros = agenda.TransformarContactos(c =>
		{
			return (c is ContactoConInfoAdicional) ? c.Formateador(formateador): c.Formateador(formateador_2);
		});

		numeros.ForEach(c => Console.WriteLine($"Número: {c}"));

		Console.WriteLine($"¿Todos son números válidos? {numeros_validos}");

		Console.ReadLine();
	}
}