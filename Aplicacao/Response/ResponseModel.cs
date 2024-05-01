namespace Aplicacao.Response
{
    public class ResponseModel<T>
    {
        public T Data { get; set; }
        public List<string>? Mensagens { get; set; }

        public ResponseModel(T data, List<string> mensagens)
        {
            Data = data;
            Mensagens = mensagens;
        }
        public ResponseModel(T data)
        {
            Data = data;
            Mensagens = default;
        }
    }
}
