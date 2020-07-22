using System;

namespace CtrlMoney.CrossCutting
{
    [Serializable]
    public class CustomException : Exception
    {
        //Message = Attempted to divide by zero.
        //Source = ConsoleApplication1
        //StackTrace = at Program.Main() in C:\...\Program.cs:line 9
        //TargetSite = Void Main()
        public CustomException(string source)
            : base()
        {
            this.Source = source;
        }

        public CustomException(string message, string source)
            : base(message)
        {
            this.Source = source;
        }

        public CustomException(string message, Exception inner, string source)
            : base(message, inner)
        {
            this.Source = source;
        }

        public static CustomException Create<T>(string method) where T : class
        {
            return new CustomException(GetSource<T>(method));
        }

        public static CustomException Create<T>(string message, string method) where T : class
        {
            return new CustomException(message, GetSource<T>(method));
        }

        public static CustomException Create<T>(string message, string method, Exception inner) where T : class
        {
            return new CustomException(message, inner, GetSource<T>(method));
        }

        private static string GetSource<T>(string method) where T : class
        {
            return $"{typeof(T).FullName} - {method}";
        }
    }
}
