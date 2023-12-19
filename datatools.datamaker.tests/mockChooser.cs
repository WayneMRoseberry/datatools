using datatools.datamaker;

namespace datatools.datamaker.tests
{
	internal class MockChooser : IChooser
	{
		public Func<int, int> overrideChooseNumber = (length) => { throw new NotImplementedException(); };

		public int ChooseNumber(int length)
		{
			return this.overrideChooseNumber(length);
		}
	}
}
