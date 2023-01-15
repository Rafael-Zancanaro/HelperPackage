namespace PackageRZ.Domain.ViewModels
{
    public class ResultViewModel<T>
    {
        public bool Success => !Errors.Any();
        public T Result { get; set; }
        public List<string> Errors { get; set; }

        public ResultViewModel()
        {
            Errors = new List<string>();
        }

        public ResultViewModel(T value)
        {
            Result = value;
        }

        public ResultViewModel<T> AddError(string error)
        {
            Errors.Add(error);
            return this;
        }

        public ResultViewModel<T> AddErrors(List<string> errors)
        {
            Errors.AddRange(errors);
            return this;
        }

        public ResultViewModel<T> AddResult(T result)
        {
            Result = result;
            return this;
        }
    }
}