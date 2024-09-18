namespace InfluxDBTest.Models
{
    public class CounterModel
    {
        public string Time {  get; set; }
        public int Counter {  get; set; }
        public string DisplayText => $"Parts Produced: {Counter} at {Time}.";
    }
}
