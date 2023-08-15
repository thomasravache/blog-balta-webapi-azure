namespace Blog; // No C# 10 não precisa mais utilizar as chaves nos namespaces

public static class Configuration
{
    // Token - JWT - JSON Web Token (ao ser desencritado terá formato json) (pode ter vários formatos)
    public static string JwtKey = "209ea5f0831f48f18c7ef5ded68da139";
    // quem tiver essa chave consegue desencriptar e editar esse token por isso é importante ser uma chave forte

    public static string ApiKeyName = "api_key";
    public static string ApiKey = "curso_api_HIDBAF0GF2F7362";
    public static SmtpConfiguration Smtp = new();

    // é possível criar classes dentro de outras
    public class SmtpConfiguration
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}