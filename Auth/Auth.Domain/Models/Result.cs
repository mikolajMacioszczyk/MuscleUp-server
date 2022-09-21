namespace Auth.Domain.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }

        public T Value { get; private set; }

        public string[] Errors { get; private set; }

        public string ErrorCombined => string.Join(",", Errors);

        public Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        public Result(string[] errors)
        {
            IsSuccess = false;
            Errors = errors;
        }

        public Result(string error) : this(new string[] { error }) 
        { }
    }
}
