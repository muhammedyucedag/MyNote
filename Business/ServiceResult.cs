namespace MyNoteSampleApp.Business
{
    public class ServiceResult<T>
    { 
        // hatalarımızı tutacağımız class

        public bool IsError { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public T Data { get; set; }

        public void AddError(string errorMessage)
        {
            IsError= true;
            Errors.Add(errorMessage);
        }
    }
}
