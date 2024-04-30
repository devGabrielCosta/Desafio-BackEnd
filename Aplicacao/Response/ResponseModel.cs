namespace Aplicacao.Response
{
    public class ResponseModel
    {
        public object Data { get; set; }
        public List<string>? Mensagens{ get; set; }

        public ResponseModel(object data, List<string> mensagens)
        {
            Data = data;
            Mensagens = mensagens;
        }
        public ResponseModel(object data)
        {
            Data = data;
            Mensagens = default;
        }
    }
}
