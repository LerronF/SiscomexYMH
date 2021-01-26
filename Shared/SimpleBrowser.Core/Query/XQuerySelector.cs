using System.Text.RegularExpressions;

namespace SimpleBrowser.Core.Query
{
	public interface IXQuerySelector
	{
		void Execute(XQueryResultsContext context);
		bool IsTransposeSelector { get; }
	}
}