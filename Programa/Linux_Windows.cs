namespace Terminal{
    public static class SistemaOperativo{
        private static string osName(){
            OperatingSystem os = Environment.OSVersion;

            return os.VersionString.Substring(0,os.VersionString.IndexOf(' '));
        }

        public static string barra(){
            return (osName() == "Unix")? "/" : "\\";
        }

        public static string extension(){
            return (osName() == "Unix")? "" : ".exe";
        }

        public static string home(){
            return(osName() == "Unix")? ("/home/" + Environment.UserName) : ("C:\\Users\\" + Environment.UserName);
        }
    }
}