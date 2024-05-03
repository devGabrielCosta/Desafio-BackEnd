namespace Application.Responses
{
    public class ResponseModel<T>
    {
        public T Data { get; set; }
        public List<string>? Messages { get; set; }

        public ResponseModel(T data, List<string> messages)
        {
            Data = data;
            Messages = messages;
        }
        public ResponseModel(T data)
        {
            Data = data;
            Messages = default;
        }
    }
}
