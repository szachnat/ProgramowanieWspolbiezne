namespace TPW.Dane
{
    public class Plane
    {
        private double m_width;
        private double m_height;

        public Plane(double width, double height) {
            this.m_width = width;
            this.m_height = height;
        }

        public void SetWidth(double width)
        {
            this.m_width = width;
        }

        public void SetHeight(double height)
        {
            this.m_height = height;
        }

        public double GetWidth()
        {
            return m_width;
        }

        public double GetHeight()
        {
            return m_height;
        }
    }
}
