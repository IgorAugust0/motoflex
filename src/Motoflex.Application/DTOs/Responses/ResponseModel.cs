namespace Motoflex.Application.DTOs.Responses
{
    public sealed class ResponseModel<T>
    {
        public T Data { get; set; }
        public IReadOnlyList<string>? Messages { get; }

        public ResponseModel(T data, List<string>? messages)
        {
            Data = data;
            Messages = messages?.AsReadOnly();
        }

        public ResponseModel(T data)
        {
            Data = data;
            Messages = null;
        }

    }
}
