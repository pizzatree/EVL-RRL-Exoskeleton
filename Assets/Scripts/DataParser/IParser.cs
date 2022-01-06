// Interface for if/when data formats are swapped

namespace DataParser
{
    public interface IParser
    {
        public ParserInfo ParseFileAt(string loc);
    }
}