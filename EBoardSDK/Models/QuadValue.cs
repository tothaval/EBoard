namespace EBoardSDK.Models
{
    public class QuadValue<T>
    {
        public T Value1 { get; set; }

        public T Value2 { get; set; }

        public T Value3 { get; set; }

        public T Value4 { get; set; }

        public QuadValue(T value)
        {
            this.Value1 = value;
            this.Value2 = value;
            this.Value3 = value;
            this.Value4 = value;
        }

        public QuadValue(T v1, T v2, T v3, T v4)
        {
            this.Value1 = v1;
            this.Value2 = v2;
            this.Value3 = v3;
            this.Value4 = v4;
        }

        public QuadValue()
        {
            this.Value1 = default;
            this.Value2 = default;
            this.Value3 = default;
            this.Value4 = default;
        }
    }
}
