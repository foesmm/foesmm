using NGettext;

namespace foesmm
{
    internal class T
    {
        private static readonly ICatalog Catalog = new Catalog("foesmm", "Localization");


        public static string _(string text)
        {
            return Catalog.GetString(text);
        }

        public static string _(string text, params object[] args)
        {
            return Catalog.GetString(text, args);
        }

        public static string _n(string text, string pluralText, long n)
        {
            return Catalog.GetPluralString(text, pluralText, n);
        }

        public static string _n(string text, string pluralText, long n, params object[] args)
        {
            return Catalog.GetPluralString(text, pluralText, n, args);
        }

        public static string _p(string context, string text)
        {
            return Catalog.GetParticularString(context, text);
        }

        public static string _p(string context, string text, params object[] args)
        {
            return Catalog.GetParticularString(context, text, args);
        }

        public static string _pn(string context, string text, string pluralText, long n)
        {
            return Catalog.GetParticularPluralString(context, text, pluralText, n);
        }

        public static string _pn(string context, string text, string pluralText, long n, params object[] args)
        {
            return Catalog.GetParticularPluralString(context, text, pluralText, n, args);
        }
    }
}