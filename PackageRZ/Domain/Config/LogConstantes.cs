namespace PackageRZ.Domain.Config;

public static class LogConstantes
{
    public const int EventIdErroNaoTratado = 0;
    public const string ErroNaoTratado = "Erro nao tratado: {0} - {1}";
    public const string DetalhesRequisicao = "Rota: {0} [{1}] | IdPessoa: {2}";
    public const string ErroSistema = "Ocorreu um erro no sistema. Data: {DataErro}";
    public const string TemplateMensagem = "[{EventId}] | Origem: [{Origem}] | Mensagem: {Mensagem} | Detalhes: {Detalhes}";
    public const string TemplateHttp = "[{EventId}] | Origem: [{Origem}] | Status: {StatusCode} | Metodo: {Method} | Url: {Url} | Parametros: {Parametros} | Response: {Conteudo}";
    public const string TemplateHttpSemConteudo = "[{EventId}] | Origem: [{Origem}] | Status: {StatusCode} | Metodo: {Method} | Url: {Url} | Parametros: {Parametros}";
}
