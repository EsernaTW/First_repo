namespace MyTraining.Classes
{
	public partial class Contacto
	{
		public string Formateador(Func<string, string> formateador) => formateador(Numero);
	}
}
