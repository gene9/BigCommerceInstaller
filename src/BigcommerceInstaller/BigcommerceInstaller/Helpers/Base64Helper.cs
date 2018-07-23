namespace BigcommerceInstaller.Helpers
{
    public class Base64Helper
    {
        //public static string Base64ForUrlEncode(string str)
        //{
        //    byte[] encbuff = Encoding.UTF8.GetBytes(str);
        //    return HttpServerUtility.UrlTokenEncode(encbuff);
        //}

        public static string Base64ForUrlDecode(string str)
        {
            return str.PadRight(str.Length + (4 - str.Length % 4) % 4, '=');
        }
    }
}
