namespace datatools.datamaker
{
	public class RandomChooser : IChooser
	{
		static Random rand = new Random();

		public int ChooseNumber(int length)
		{
			return rand.Next(length);
		}
	}
}